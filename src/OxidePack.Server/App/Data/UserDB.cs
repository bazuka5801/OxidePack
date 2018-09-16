using System.Collections.Generic;
using System.IO;
using System.Linq;
using OxidePack.Data;

namespace OxidePack.Server.App.Data
{
    public static class UserDB
    {
        static Dictionary<string, uint> KeyToUID;
        static List<UserData> Users;

        public static UserData Get(string key, string username)
        {
            UserData uData;
            if (KeyToUID.TryGetValue(key, out var index) == false)
            {
                uData = Pool.Get<UserData>();
                uData.key = key;
                uData.index = (uint)Users.Count;
                uData.username = username;
                uData.registred = Epoch.Current;
                uData.permissions = new List<Permission>();
                
                KeyToUID[key] = (uint)Users.Count;
                Users.Add(uData);
            }

            uData = Users[(int) index];
            uData.username = username;
            return uData;
        }
        
        public static void Load()
        {
            if (Directory.Exists("Database") == false)
                Directory.CreateDirectory("Database");
            
            
            if (File.Exists("Database/UserData.bin") == false)
            {
                KeyToUID = new Dictionary<string, uint>();
                Users = new List<UserData>();
            }
            else
            {
                var userDC = UserDataCollection.Deserialize(File.ReadAllBytes("Database/UserData.bin"));
                Users = userDC.users.ToList();
                Pool.Free(ref userDC);
                KeyToUID = Users.ToDictionary(p => p.key, p => p.index);
            }
        }

        public static void Save()
        {
            if (Directory.Exists("Database") == false)
                Directory.CreateDirectory("Database");
            
            var userDC = Pool.Get<UserDataCollection>();
            userDC.users = Users.ToList();
            File.WriteAllBytes("Database/UserData.bin", UserDataCollection.SerializeToBytes(userDC));
            Pool.Free(ref userDC);
        }
    }
}