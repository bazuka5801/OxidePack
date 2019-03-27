using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Ether.Network.Packets;
using OxidePack.CoreLib;
using OxidePack.CoreLib.Utils;
using OxidePack.Data;
using SapphireEngine;
using CompilerError = OxidePack.Data.CompilerError;

namespace OxidePack.Server.App
{
    public class OpUser : BaseUser
    {
        public Plugin ActivePlugin;


        public override void HandleRpcMessage(RPCMessageType type, NetPacket stream)
        {
            if (IsAuthed == false)
            {
                return;
            }
            switch (type)
            {
                case RPCMessageType.StatusRequest:
                    break;
                case RPCMessageType.BuildRequest:
                    BuildRequest bRequest = BuildRequest.Deserialize(stream);
                    if (ActivePlugin == null || ActivePlugin.PluginName != bRequest.buildOptions.name)
                    {
                        var pOptions = new PluginOptions() { Debug = true, Encrypt = false, ReferencesPath = ".references" };
                        ActivePlugin = new Plugin(bRequest.buildOptions.name, pOptions);
                    }

                    ThreadPool.QueueUserWorkItem((s) =>
                    {
                        string buildResult = "";
                        Stopwatch sw = Stopwatch.StartNew();
                        try
                        {
                            buildResult = ActivePlugin.Build(bRequest);

                            sw.Stop();
                            Data.milliseconds_used += (ulong)sw.ElapsedMilliseconds;
                            Data.milliseconds_build += (ulong)sw.ElapsedMilliseconds;
                            Data.statBuild++;
                            ConsoleSystem.Log($"User '{Data.username}' build '{bRequest.buildOptions.name}:{bRequest.buildOptions.plugininfo.version}-{bRequest.buildOptions.plugininfo.author}' in {sw.Elapsed.ToString("c")}");
                        }
                        catch (Exception e)
                        {
                            LogUtils.PutsException(e, "BuildTask");
                        }
                        sw.Reset();

                        string encryptResult = null;
                        CompilerResults compilerResults = null;

                        if (bRequest.encryptOptions.enabled && Data.CanEncrypt())
                        {

                            var options = new EncryptorOptions()
                            {
                                LocalVarsCompressing = bRequest.encryptOptions.localvars,
                                FieldsCompressing = bRequest.encryptOptions.fields,
                                MethodsCompressing = bRequest.encryptOptions.methods,
                                TypesCompressing = bRequest.encryptOptions.types,
                                SpacesRemoving = bRequest.encryptOptions.spacesremoving,
                                TrashRemoving = bRequest.encryptOptions.trashremoving,
                                Encoding = bRequest.encryptOptions.encoding,
                            };
                            if ((Data.HasPermission("spaghetti") || Data.HasPermission("vip")) && bRequest.encryptOptions.spaghetti)
                            {
                                options.Spaghetti = true;
                                options.SpaghettiControlFlow = bRequest.encryptOptions.spaghettiControlFlow;
                            }

                            try
                            {
                                sw.Start();

                                (compilerResults, encryptResult) = ActivePlugin.EncryptWithCompiling(buildResult, options);

                                sw.Stop();
                                Data.milliseconds_used += (ulong)sw.ElapsedMilliseconds;
                                Data.milliseconds_encryption += (ulong)sw.ElapsedMilliseconds;
                                Data.statEncryption++;
                                ConsoleSystem.Log($"User '{Data.username}' encrypt '{bRequest.buildOptions.name}:{bRequest.buildOptions.plugininfo.version}-{bRequest.buildOptions.plugininfo.author}' in {sw.Elapsed.ToString("c")}");
                            }
                            catch (Exception e)
                            {
                                LogUtils.PutsException(e, "EncryptionTask");
                            }
                        }



                        BuildResponse bResponse = new BuildResponse()
                        {
                            pluginname = bRequest.buildOptions.name,
                            content = buildResult,
                            encrypted = encryptResult
                        };

                        if (bRequest.buildOptions.compileDll)
                        {
                            var (compiledAssembly, compilerErrors) = CompileUtils.CompileAssembly(buildResult, bRequest.buildOptions.name,
                                bRequest.buildOptions.forClient ? "client" : "server");
                            bResponse.compiledAssembly = compiledAssembly;
                            bResponse.buildErrors = compilerErrors;
                        }

                        if (compilerResults?.Errors.HasErrors ?? false)
                        {
                            List<OxidePack.Data.CompilerError> errorList = new List<CompilerError>();
                            foreach (System.CodeDom.Compiler.CompilerError error in compilerResults.Errors)
                            {
                                var errorProto = Pool.Get<OxidePack.Data.CompilerError>();
                                errorProto.line = error.Line;
                                errorProto.column = error.Column;
                                errorProto.errorText = error.ErrorText;
                                errorList.Add(errorProto);
                            }

                            bResponse.encryptErrors = errorList;
                        }

                        SendRpc(RPCMessageType.BuildResponse, bResponse);
                    });
                    break;
                case RPCMessageType.EncryptionRequest:
                    break;
                case RPCMessageType.GeneratedFileRequest:
                    OnRPC_GeneratedFileRequest(GeneratedFileRequest.Deserialize(stream));
                    break;
                case RPCMessageType.ModuleListRequest:
                    SendModuleList();
                    break;
            }
        }

        public void OnRPC_GeneratedFileRequest(GeneratedFileRequest request)
        {
            if (ActivePlugin == null || ActivePlugin.PluginName != request.pluginname)
            {
                var pOptions = new PluginOptions() { Debug = true, Encrypt = false, ReferencesPath = ".references" };
                ActivePlugin = new Plugin(request.pluginname, pOptions);
            }
            var generatedFile = ActivePlugin.GetGeneratedFile(request.modules, request.@namespace);
            var response = new GeneratedFileResponse()
            {
                pluginname = request.pluginname,
                content = generatedFile
            };

            SendRpc(RPCMessageType.GeneratedFileResponse, response);
        }

        public void SendModuleList()
        {
            var moduleList = ModuleMgr.GetModuleList().Select(module => new ModuleInfo()
            {
                name = module.Manifest.Name,
                version = module.Manifest.Version,
                description = module.Manifest.Description
            }).ToList();

            var moduleListResponse = new ModuleListResponse()
            {
                modules = moduleList
            };

            SendRpc(RPCMessageType.ModuleListResponse, moduleListResponse);
        }
    }
}