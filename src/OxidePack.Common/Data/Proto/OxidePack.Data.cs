﻿// Classes and structures being serialized

// Generated by ProtocolBuffer
// - a pure c# code generation implementation of protocol buffers
// Report bugs to: https://silentorbit.com/protobuf/

// DO NOT EDIT
// This file will be overwritten when CodeGenerator is run.
// To make custom modifications, edit the .proto file and add //:external before the message line
// then write the code and the changes in a separate file.
using System;
using System.Collections.Generic;
using SilentOrbit.ProtocolBuffers;
using OxidePack;

namespace OxidePack.Data
{
    public partial class UserInformation : IDisposable, Pool.IPooled, IProto
    {
        public string key;

        public string username;

        public string version;

    }

    public partial class GeneratedFileRequest : IDisposable, Pool.IPooled, IProto
    {
        public string pluginname;

        public string @namespace;

        public List<string> modules;

    }

    public partial class GeneratedFileResponse : IDisposable, Pool.IPooled, IProto
    {
        public string pluginname;

        public string content;

    }

    public partial class BuildRequest : IDisposable, Pool.IPooled, IProto
    {
        public OxidePack.Data.BuildOptions buildOptions;

        public OxidePack.Data.EncryptOptions encryptOptions;

        public List<OxidePack.Data.SourceFile> sources;

    }

    public partial class BuildResponse : IDisposable, Pool.IPooled, IProto
    {
        public string pluginname;

        public string content;

        public string encrypted;

        public List<OxidePack.Data.CompilerError> buildErrors;

        public List<OxidePack.Data.CompilerError> encryptErrors;

        public byte[] compiledAssembly;

    }

    public partial class BuildOptions : IDisposable, Pool.IPooled, IProto
    {
        public string name;

        public OxidePack.Data.PluginInfo plugininfo;

        public bool compileDll;

        public bool forClient;

    }

    public partial class EncryptOptions : IDisposable, Pool.IPooled, IProto
    {
        public bool enabled;

        public bool localvars;

        public bool fields;

        public bool methods;

        public bool types;

        public bool spacesremoving;

        public bool trashremoving;

        public bool secret;

        public bool forClient;

    }

    public partial class PluginInfo : IDisposable, Pool.IPooled, IProto
    {
        public string name;

        public string author;

        public string version;

        public string description;

        public string baseclass;

    }

    public partial class SourceFile : IDisposable, Pool.IPooled, IProto
    {
        public string filename;

        public string content;

        public string sha256;

    }

    public partial class ModuleInfo : IDisposable, Pool.IPooled, IProto
    {
        public string name;

        public string version;

        public string description;

    }

    public partial class ModuleListResponse : IDisposable, Pool.IPooled, IProto
    {
        public List<OxidePack.Data.ModuleInfo> modules;

    }

    public partial class CompilerError : IDisposable, Pool.IPooled, IProto
    {
        public int line;

        public int column;

        public int lineEnd;

        public int columnEnd;

        public string errorText;

    }

}
