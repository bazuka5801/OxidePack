using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SapphireEngine;

namespace OxidePack
{
    public class BaseConfig
    {
        public static event Action OnConfigLoaded;
        public static bool IsLoaded;

        internal static void FireOnConfigLoaded()
        {
            OnConfigLoaded?.Invoke();
        }
    }
    
    public class ConfigManager : SapphireType
    {
        private Type m_ConfigType;
        private FSWatcher m_ConfigWatcher;
        
        public override void OnAwake()
        {
            m_ConfigType = FindConfigType();
            Load();
        }

        public override void OnDestroy()
        {
            Write();
        }

        public void RunWatcher()
        {
            if (m_ConfigWatcher != null)
            {
                m_ConfigWatcher.Close();
                m_ConfigWatcher = null;
            }
            m_ConfigWatcher = new FSWatcher(Directory.GetCurrentDirectory(), "config.json");
            m_ConfigWatcher.Subscribe(OnConfigChanged);
        }

        public void DisableWatcher()
        {
            if (m_ConfigWatcher != null)
            {
                m_ConfigWatcher.Close();
                m_ConfigWatcher = null;
            }
        }

        private void OnConfigChanged(string obj)
        {
            Load();
        }

        public void Load()
        {
            if (m_ConfigType == null)
            {
                return;
            }
            
            if (File.Exists("config.json") == false)
            {
                Write();
                return;
            }
            
            List<string> newFiels = new List<string>();
            JObject config;
            try
            {
                config = JObject.Parse(File.ReadAllText("config.json"));
            }
            catch (Exception e)
            {
                ConsoleSystem.LogError("[ConfigManager] Invalid config!\nDetails: "+e.Message);
                return;
            }
            foreach (var field in m_ConfigType.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(f=>f.IsLiteral == false))
            {
                if (config.ContainsKey(field.Name))
                {
                    field.SetValue(null, config[field.Name].ToObject(field.FieldType));
                }
                else
                {
                    newFiels.Add($"{field.FieldType.Name} {field.Name}");
                }
            }
            
            BaseConfig.IsLoaded = true;
            BaseConfig.FireOnConfigLoaded();
            
            Write();
            if (newFiels.Count > 0)
            {
                ConsoleSystem.LogWarning($"New fields in config:\n{string.Join("\n", newFiels)}");
            }
            ConsoleSystem.Log("Config reloaded!");

        }

        public void Write()
        {
            if (m_ConfigType == null)
            {
                return;
            }
            
            JObject config = new JObject();
            foreach (var field in m_ConfigType.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(f=>f.IsLiteral == false))
            {
                config[field.Name] = JToken.FromObject(field.GetValue(null));
            }

            if (m_ConfigWatcher != null)
                m_ConfigWatcher.Enabled = false;
            File.WriteAllText("config.json", config.ToString());
            if (m_ConfigWatcher != null)
                m_ConfigWatcher.Enabled = true;
        }

        public Type FindConfigType()
        {
            Type configType = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.BaseType == typeof(BaseConfig))
                    {
                        if (configType != null)
                        {
                            ConsoleSystem.LogError($"[ConfigManger] Multiple config found => <{configType.FullName}> <{type.FullName}>");
                            return configType;
                        }

                        configType = type;
                    }
                }
            }

            if (configType == null)
            {
                ConsoleSystem.LogError("[ConfigManager] Config class not found");
            }

            return configType;
        }
    }
}