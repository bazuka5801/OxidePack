using System.Collections.Generic;
using System.IO;
using System.Linq;
using OxidePack.Data;
using SapphireEngine;

namespace OxidePack.Server.App.Data
{
    public static class UserDb
    {
        private static Dictionary<string, uint> _keyToUid;
        private static List<UserData> _users;

        public static IEnumerable<UserData> All => _users;

        public static bool Get(string key, string username, out UserData uData)
        {
            if (_keyToUid.TryGetValue(key, out var index) == false)
            {
                uData = null;
                return false;
            }

            uData = _users[(int) index];
            return true;
        }

        public static void AddUser(string key, string username)
        {
            var uData = Pool.Get<UserData>();
            uData.key = key;
            uData.index = (uint)_users.Count;
            uData.username = username;
            uData.registred = Epoch.Current;
            uData.permissions = new List<Permission>();

            _keyToUid[key] = (uint)_users.Count;
            _users.Add(uData);
            ConsoleSystem.Log($"User '{username}' added");
            Save();
        }

        public static void Load()
        {
            if (Directory.Exists("Database") == false)
                Directory.CreateDirectory("Database");


            if (File.Exists("Database/UserData.bin") == false)
            {
                _keyToUid = new Dictionary<string, uint>();
                _users = new List<UserData>();
            }
            else
            {
                var userDc = UserDataCollection.Deserialize(File.ReadAllBytes("Database/UserData.bin"));
                _users = userDc.users.ToList();
                Pool.Free(ref userDc);
                _keyToUid = _users.ToDictionary(p => p.key, p => p.index);
            }
        }

        public static void Save()
        {
            if (Directory.Exists("Database") == false)
                Directory.CreateDirectory("Database");

            var userDc = Pool.Get<UserDataCollection>();
            userDc.users = _users.ToList();
            File.WriteAllBytes("Database/UserData.bin", UserDataCollection.SerializeToBytes(userDc));
            Pool.Free(ref userDc);
        }
    }
}