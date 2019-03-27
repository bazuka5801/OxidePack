using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public void SetConfigType(Type type)
        {
            this.m_ConfigType = type;
            this.Load();
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
            m_ConfigWatcher.Subscribe(ConfigFileChanged);
        }

        public void DisableWatcher()
        {
            if (m_ConfigWatcher != null)
            {
                m_ConfigWatcher.Close();
                m_ConfigWatcher = null;
            }
        }

        private void ConfigFileChanged(string obj)
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
                var name = field.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>()?.PropertyName ?? field.Name;
                if (config.ContainsKey(name))
                {
                    field.SetValue(null, config[name].ToObject(field.FieldType));
                }
                else
                {
                    newFiels.Add($"{name}");
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
                var name = field.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>()?.PropertyName ??
                           field.Name;
                config[name] = JToken.FromObject(field.GetValue(null));
            }

            if (m_ConfigWatcher != null)
                m_ConfigWatcher.Enabled = false;
            File.WriteAllText("config.json", config.ToString());
            if (m_ConfigWatcher != null)
                m_ConfigWatcher.Enabled = true;
        }
    }
}