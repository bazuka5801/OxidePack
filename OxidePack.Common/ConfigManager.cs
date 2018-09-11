using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace OxidePack
{
    public static class ConfigManager
    {
        public static void Load(Type type)
        {
            if (File.Exists("config.json") == false)
            {
                Write();
                return;
            }
            var config = JObject.Parse(File.ReadAllText("config.json"));
            foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (config.ContainsKey(field.Name))
                    field.SetValue(null, config[field.Name]);
            }
        }
        
        public static void Write(Type type)
        {
            JObject config = new JObject();
            foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                config[field.Name] = JToken.FromObject(field.GetValue(null));
            }
            File.WriteAllText("config.json", config.ToString());
        }
    }
}