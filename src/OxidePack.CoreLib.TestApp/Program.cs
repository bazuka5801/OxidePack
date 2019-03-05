using System;
using System.Diagnostics;
using OxidePack.CoreLib.Experimental.Method2Sequence;

namespace OxidePack.CoreLib.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
//            var result = new Method2Sequence().ProcessSource(ex1);
            Stopwatch sw = Stopwatch.StartNew();
            var result = ex1;
            for (int i = 0; i < 1; i++)
            {
                result = new Method2Sequence().ProcessSource(result); 
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine(TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds).ToString("g"));
            Console.ReadKey();
        }
const string ex1 = @"
using System;
using System.Collections;
using System.Collections.Generic;

class MainClass {
    int maz = 0;

    void f1Method() {
        int b = 0;
        maz = 1;
        b = 1;
    }

    void f2Method(int a) {
        
    }

}";
        private const string ex2 = @"
namespace Oxide.Plugins
{
    using System.Linq;
    using BCore;
    using BCore.Rust;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Oxide.Core;
    using System;
    using Facepunch.Extend;
    using System.Collections.Generic;
    using Facepunch;
    using System.Text;

    [Info(""BKits"", ""bazuka5801"", ""1.0.60"")]
    public class BKits : RustPlugin
    {
        object HOOK__GetKitsForPlayer(object pObj)
        {
            ulong userId;
            if (PlayerUtils.GetUserID(pObj, out userId))
            {
                var kits = Kits.GetAll()
                    .Where(p => string.IsNullOrEmpty(p.Permission) || userId.HasPermission(p.Permission));
                JArray jObj = new JArray();
                foreach (var kit in kits)
                {
                    var kObj = new JObject();
                    jObj.Add(kObj);

                    kObj[""name""] = kit.Name;
                    long remain;
                    if (Players.GetCooldown(userId, kit.Name, out remain) == false)
                        remain = 0;
                    kObj[""remain""] = remain - TimeUtils.GetTimeStamp();
                    kObj[""cooldown""] = kit.Cooldown;
                    var pngName = $""kitimg.{kit.Name}"";
                    kObj[""png""] = ImageStorage.Exists(pngName) ? pngName : ""default"";
                    kObj[""displayname""] = kit.DisplayName ?? kit.Name;
                }
                return jObj;
            }

            return null;
        }

        object HOOK__API_TryGiveKit(BasePlayer player, string kitname)
        {
            return TryGiveKit(player, kitname);
        }

        [ChatCommand(""kit"")]
        void cmdChatKit(BasePlayer player, string command, string[] args)
        {
            if (args.Length == 0)
            {
                Interface.Oxide.RootPluginManager.GetPlugin(""ServerMenu"")?.Call(""DrawGUI"", player, ""main"", ""kits"");
                return;
            }

            if (args.Length == 1)
            {
                string kitname = args[0];
                TryGiveKit(player, kitname);
            }

            if (args.Length > 1)
            {
                string name = args[1];
                if (args[0] == ""create"" && player.IsAdmin)
                {
                    KitData kit;
                    if (Kits.Create(name, out kit))
                    {
                        player.ChatMessage($""Кит {name} создан!"");
                        return;
                    }
                    else
                    {
                        player.ChatMessage($""Кит с таким названием уже существует!"");
                        return;
                    }
                }

                if (args[0] == ""edit"" && player.IsAdmin)
                {
                    KitData kit;
                    if (Kits.GetByName(name, out kit) == false)
                    {
                        player.ChatMessage(""Кит не найден!"");
                        return;
                    }

                    for (int i = 2; i < args.Length; i++)
                    {
                        switch (args[i])
                        {
                            case ""perm"":
                                string perm = args[++i];
                                kit.Permission = perm;
                                if (perm.IsNullOrEmpty() == false)
                                {
                                    this.RegisterPermission(perm);
                                    player.ChatMessage($""Добавлен пермишен '{perm}' киту '{kit.Name}'"");
                                    return;
                                }
                                player.ChatMessage($""Убран пермишен у кита '{kit.Name}'"");
                                return;
                            case ""cooldown"":
                                int cooldown = (int)TimeUtils.ToSeconds(args[++i]);
                                kit.Cooldown = cooldown;
                                string time = TimeUtils.Format(TimeSpan.FromSeconds(cooldown));
                                player.ChatMessage($""Поставлен откат {time} киту '{kit.Name}'"");
                                return;
                            case ""items"":
                                kit.Items = SaveItems(player);
                                player.ChatMessage($""Перезаписаны предметы у кита '{kit.Name}'"");
                                return;
                            case ""revokeafteruse"":
                                kit.RevokeAfterUse = args[++i].ToBool();
                                player.ChatMessage($""Пермишен {(kit.RevokeAfterUse == false ? ""не "" : """")}удаляется после использования"");
                                return;
                        }
                    }
                }
            }
        }
        public List<SavedItem> SaveItems(BasePlayer player)
        {
            var inventory = player.inventory;
            List<SavedItem> list = Pool.GetList<SavedItem>();

            for (int i = 0; i < inventory.containerWear.capacity; i++)
            {
                var item = inventory.containerWear.GetSlot(i);
                if (item != null)
                {
                    item.RemoveFromContainer();
                    list.Add(SaveItem(item));
                }
            }

            for (int i = 0; i < inventory.containerMain.capacity; i++)
            {
                var item = inventory.containerMain.GetSlot(i);
                if (item != null)
                {
                    item.RemoveFromContainer();
                    list.Add(SaveItem(item));
                }
            }

            for (int i = 0; i < inventory.containerBelt.capacity; i++)
            {
                var item = inventory.containerBelt.GetSlot(i);
                if (item != null)
                {
                    item.RemoveFromContainer();
                    list.Add(SaveItem(item));
                }
            }

            return list;
        }


        public List<KitData> GetAvailableKits(BasePlayer player)
        {
            var list = Pool.GetList<KitData>();
            foreach (var kitData in Kits.GetAll())
            {
                if (kitData.Permission.IsNullOrEmpty() || player.HasPermission(kitData.Permission))
                {
                    list.Add(kitData);
                }
            }

            return list;
        }

        private bool TryGiveKit(BasePlayer player, string kitname)
        {
            KitData kit;
            if (Kits.GetByName(kitname, out kit) == false)
            {
                ReplyChat(player, ""TryGiveKit:NotFound"", kitname);
                return false;
            }

            if (kit.Permission.IsNullOrEmpty() == false && player.HasPermission(kit.Permission) == false)
            {
                ReplyChat(player, ""TryGiveKit:AccessDenied"");
                return false;
            }

            long expired;
            if (kit.Cooldown > 0 && Players.GetCooldown(player.userID, kitname, out expired))
            {
                long remainSeconds = expired - TimeUtils.GetTimeStamp();

                ReplyChat(player, ""TryGiveKit:Cooldown"", TimeUtils.Format(TimeSpan.FromSeconds(remainSeconds), player));
                return false;
            }

            if (kit.RevokeAfterUse)
            {
                permission.RevokeUserPermission(player.UserIDString, kit.Permission);
            }

            GiveKit(player, kit);

            if (kit.Cooldown > 0)
            {
                Players.SetCooldown(player.userID, kit);
            }

            return true;
        }

        private void GiveKit(BasePlayer player, KitData kit)
        {
            RestoreItems(kit.Items).ForEach(item =>
            {
                if (item.info.isWearable)
                    if (item.MoveToContainer(player.inventory.containerWear) == true)
                        return;
                player.GiveItem(item);
            });
        }

        void HOOK__OnServerInitialized()
        {
        }

        void HOOK__Unload()
        {

        }


        private KitsData Kits = null;
        private PlayersData Players = null;
        void DATA__OnServerInitialized()
        {
            this.RegisterData(""BKits/Kits"", ref Kits);
            this.RegisterData(""BKits/Players"", ref Players);

            foreach (var kit in Kits.GetAll())
            {
                if (kit.Permission.IsNullOrEmpty() == false)
                    this.RegisterPermission(kit.Permission);
                if (string.IsNullOrEmpty(kit.PNG) == false)
                {
                    ServerMgr.Instance.StartCoroutine(ImageStorage.Store($""kitimg.{kit.Name}"", kit.PNG));
                }
            }
        }


        public class PlayersData
        {
            public Dictionary<ulong, PlayerData> Players = new Dictionary<ulong, PlayerData>();

            public bool GetCooldown(ulong steamid, string kitName, out long expired)
            {
                PlayerData pData;
                if (Players.TryGetValue(steamid, out pData))
                    return pData.GetCooldown(kitName, out expired);

                expired = -1;
                return false;
            }

            public void SetCooldown(ulong steamid, KitData kit)
            {
                PlayerData pData;
                if (Players.TryGetValue(steamid, out pData) == false)
                    Players[steamid] = pData = new PlayerData();
                pData.SetCooldown(kit);
            }
        }

        public class PlayerData
        {
            public Dictionary<string, long> Cooldowns = new Dictionary<string, long>();

            public bool GetCooldown(string kitName, out long expired)
            {
                if (Cooldowns.TryGetValue(kitName, out expired))
                {
                    if (expired - TimeUtils.GetTimeStamp() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        Cooldowns.Remove(kitName);
                        return false;
                    }
                }

                return false;
            }

            public void SetCooldown(KitData kit)
            {
                Cooldowns[kit.Name] = TimeUtils.GetTimeStamp() + kit.Cooldown;
            }
        }

        public class KitsData
        {
            public Dictionary<string, KitData> Kits = new Dictionary<string, KitData>();

            public bool Create(string name, out KitData kit)
            {
                if (GetByName(name, out kit) == false)
                {
                    Kits[name] = new KitData() { Name = name, Permission = string.Empty, Cooldown = 0 };
                    return true;
                }

                return false;
            }

            public bool GetByName(string name, out KitData kit)
            {
                return this.Kits.TryGetValue(name, out kit);
            }

            public IEnumerable<KitData> GetAll() => Kits.Values;
        }

        public class KitData
        {
            public string Name;
            public string Permission;
            public bool RevokeAfterUse;
            public int Cooldown;
            public string PNG;
            public string DisplayName;
            public List<SavedItem> Items = new List<SavedItem>();
        }

        #region  [Module] Configuration
        private ConfigData Configuration;
        partial class ConfigData
        {
        }

        void CONFIGURATION__Loaded()
        {
            LoadVariables();
        }

        private void LoadVariables()
        {
            LoadConfigVariables();
            SaveConfig(Configuration);
        }

        private void LoadConfigVariables()
        {
            Config.Settings.Converters = this.ConfigConvertors;
            base.Config.Settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            Configuration = Config.ReadObject<ConfigData>();
        }

        protected override void LoadDefaultConfig()
        {
            Config.Settings.Converters = this.ConfigConvertors;
            var config = new ConfigData();
            SaveConfig(config);
        }

        void SaveConfig(object config) => Config.WriteObject(config, true);
        private List<JsonConverter> ConfigConvertors = new List<JsonConverter>()
{new MinMaxIntDataConverter(), new MinMaxFloatDataConverter(), new ItemDataConverter(), };
        #region [Types] Custom Fields
        #region [Folder] MinMaxData
        #region [Type] MinMaxDataBase<T>
        public abstract class MinMaxDataBase<T>
            where T : struct, IEquatable<T>
        {
            public T Min;
            public T Max;
            public bool IsRange => this.Min.Equals(this.Max) == false;
            public MinMaxDataBase()
            {
            }

            public MinMaxDataBase(T min, T max)
            {
                this.Min = min;
                this.Max = max;
            }

            public abstract T Random();
            public T Value() => this.IsRange ? this.Random() : this.Min;
            public override string ToString()
            {
                return IsRange ? $""{this.Min}-{this.Max}"" : $""{this.Min}"";
            }

            public abstract MinMaxDataBase<T> Parse(string data);
            public string Serialize() => this.ToString();
        }

        #endregion
        #region [Field] MinMaxIntData
        public class MinMaxIntData : MinMaxDataBase<Int32>
        {
            public MinMaxIntData()
            {
            }

            public MinMaxIntData(Int32 min, Int32 max) : base(min, max)
            {
            }

            public override Int32 Random() => UnityEngine.Random.Range(this.Min, this.Max + 1);
            public override MinMaxDataBase<int> Parse(string data)
            {
                if (data.Contains(""-"") == false)
                {
                    this.Min = this.Max = Convert.ToInt32(data);
                    return this;
                }

                var values = data.Trim().Split('-');
                this.Min = Convert.ToInt32(values[0]);
                this.Max = Convert.ToInt32(values[1]);
                return this;
            }
        }

        public class MinMaxIntDataConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var data = (MinMaxIntData)value;
                writer.WriteValue(data.Serialize());
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new MinMaxIntData().Parse(reader.Value.ToString());
            }

            public override bool CanConvert(Type objectType) => objectType == typeof(MinMaxIntData);
        }

        #endregion
        #region [Field] MinMaxFloatData
        public class MinMaxFloatData : MinMaxDataBase<Single>
        {
            public MinMaxFloatData()
            {
            }

            public MinMaxFloatData(Single min, Single max) : base(min, max)
            {
            }

            public override Single Random() => UnityEngine.Random.Range(this.Min, this.Max);
            public override MinMaxDataBase<Single> Parse(string data)
            {
                if (data.Contains(""-"") == false)
                {
                    this.Min = this.Max = Convert.ToSingle(data);
                    return this;
                }

                var values = data.Trim().Split('-');
                return new MinMaxFloatData(Convert.ToSingle(values[0]), Convert.ToSingle(values[1]));
            }
        }

        public class MinMaxFloatDataConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var data = (MinMaxFloatData)value;
                writer.WriteValue(data.Serialize());
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new MinMaxIntData().Parse(reader.Value.ToString());
            }

            public override bool CanConvert(Type objectType) => objectType == typeof(MinMaxFloatData);
        }

        #endregion
        #endregion
        #region [Field] ItemAmount
        public class ItemData
        {
            public string Shortname;
            public MinMaxIntData Amount = new MinMaxIntData(1, 1);
            public ulong SkinID = 0;
            public float Chance = -1f;
            public Item CreateItem()
            {
                var item = ItemManager.CreateByPartialName(Shortname, Amount.Value());
                item.skin = SkinID;
                return item;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder(20);
                sb.Append(this.Shortname);
                if (this.Amount.Value() >= 0)
                    sb.Append($""={this.Amount}"");
                if (this.SkinID != 0)
                    sb.Append($"":{this.SkinID}"");
                if (this.Chance > 0)
                    sb.Append($""%{this.Chance}"");
                var result = sb.ToString();
                sb.Clear();
                sb = null;
                return result;
            }
        }

        public class ItemDataConverter : JsonConverter
        {
            private readonly List<char> operators = new List<char>()
    {'=', ':', '%'};
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var data = (ItemData)value;
                writer.WriteValue(data.ToString());
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var value = reader.Value.ToString().Trim();
                var data = new ItemData();
                StringBuilder sb = new StringBuilder();
                var option = """";
                for (var i = 0; i < value.Length; i++)
                {
                    var symbol = value[i];
                    if (operators.Contains(symbol) == false)
                    {
                        sb.Append(symbol);
                        if (i != value.Length - 1)
                            continue;
                    }

                    switch (option)
                    {
                        case """":
                            data.Shortname = sb.ToString();
                            break;
                        case ""%"":
                            data.Chance = Convert.ToSingle(sb.ToString());
                            break;
                        case "":"":
                            data.SkinID = Convert.ToUInt64(sb.ToString());
                            break;
                        case ""="":
                            data.Amount = (MinMaxIntData)new MinMaxIntData().Parse(sb.ToString());
                            break;
                    }

                    sb.Clear();
                    option = symbol.ToString();
                }

                return data;
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(ItemData);
            }
        }
        #endregion
        #endregion
        #endregion
        #region  [Module] ItemsSerialization
        List<SavedItem> SaveItems(List<Item> items)
        {
            return items.Select(SaveItem).ToList();
        }

        public class SavedItem
        {
            public string shortname;
            public int itemid;
            public float condition;
            public float maxcondition;
            public int amount;
            public int ammoamount;
            public string ammotype;
            public int flamefuel;
            public int chainsawfuel;
            public ulong skinid;
            public bool weapon;
            public List<SavedItem> mods;
        }

        SavedItem SaveItem(Item item)
        {
            SavedItem iItem = new SavedItem { shortname = item.info?.shortname, amount = item.amount, mods = new List<SavedItem>(), skinid = item.skin };
            if (item.info == null)
                return iItem;
            iItem.itemid = item.info.itemid;
            iItem.weapon = false;
            if (item.hasCondition)
            {
                iItem.condition = item.condition;
                iItem.maxcondition = item.maxCondition;
            }

            FlameThrower flameThrower = item.GetHeldEntity()?.GetComponent<FlameThrower>();
            if (flameThrower != null)
                iItem.flamefuel = flameThrower.ammo;
            Chainsaw chainsaw = item.GetHeldEntity()?.GetComponent<Chainsaw>();
            if (chainsaw != null)
                iItem.chainsawfuel = chainsaw.ammo;
            if (item.info.category.ToString() != ""Weapon"")
                return iItem;
            BaseProjectile weapon = item.GetHeldEntity() as BaseProjectile;
            if (weapon == null)
                return iItem;
            if (weapon.primaryMagazine == null)
                return iItem;
            iItem.ammoamount = weapon.primaryMagazine.contents;
            iItem.ammotype = weapon.primaryMagazine.ammoType.shortname;
            iItem.weapon = true;
            if (item.contents != null)
                foreach (var mod in item.contents.itemList)
                    if (mod.info.itemid != 0)
                        iItem.mods.Add(SaveItem(mod));
            return iItem;
        }

        List<Item> RestoreItems(List<SavedItem> sItems)
        {
            return sItems.Select(sItem =>
            {
                if (sItem.weapon)
                    return BuildWeapon(sItem);
                return BuildItem(sItem);
            }

            ).Where(i => i != null).ToList();
        }

        Item BuildItem(SavedItem sItem)
        {
            if (sItem.amount < 1)
                sItem.amount = 1;
            Item item = ItemManager.CreateByItemID(sItem.itemid, sItem.amount, sItem.skinid);
            if (item.hasCondition)
            {
                item.condition = sItem.condition;
                item.maxCondition = sItem.maxcondition;
            }

            FlameThrower flameThrower = item.GetHeldEntity()?.GetComponent<FlameThrower>();
            if (flameThrower)
                flameThrower.ammo = sItem.flamefuel;
            Chainsaw chainsaw = item.GetHeldEntity()?.GetComponent<Chainsaw>();
            if (chainsaw)
                chainsaw.ammo = sItem.chainsawfuel;
            return item;
        }

        Item BuildWeapon(SavedItem sItem)
        {
            Item item = ItemManager.CreateByItemID(sItem.itemid, 1, sItem.skinid);
            if (item.hasCondition)
            {
                item.condition = sItem.condition;
                item.maxCondition = sItem.maxcondition;
            }

            var weapon = item.GetHeldEntity() as BaseProjectile;
            if (weapon != null)
            {
                var def = ItemManager.FindItemDefinition(sItem.ammotype);
                weapon.primaryMagazine.ammoType = def;
                weapon.primaryMagazine.contents = sItem.ammoamount;
            }

            if (sItem.mods != null)
                foreach (var mod in sItem.mods)
                    item.contents.AddItem(BuildItem(mod).info, 1);
            return item;
        }
        #endregion


        void MESSAGES__OnServerInitialized()
        {
            this.lang.RegisterMessages(this.Messages, this, ""en"");
            this.Messages = lang.GetMessages(""en"", this);
        }

        private Dictionary<string, string> Messages = new Dictionary<string, string>()
        {
            [""TryGiveKit:NotFound""] = ""Кит '{0}' не найден!"",
            [""TryGiveKit:AccessDenied""] = ""У вас нет доступа к этому киту"",
            [""TryGiveKit:Cooldown""] = ""Кит будет доступен через {0}"",
        };

        string Format(string line, object[] args)
        {
            string result = line;
            for (var i = 0; i < args.Length; i++)
            {
                result = result.Replace($""{{{i}}}"", args[i].ToString());
            }

            return result;
        }

        void ReplyChat(BasePlayer player, string key, params object[] args)
        {
            player.ChatMessage(Format(Messages[key], args));
        }

        #region [Generated] [Hook Methods]
        object GetKitsForPlayer(object pObj)
        {
            object ret = null;
            object temp = null;
            temp = HOOK__GetKitsForPlayer(pObj);
            if (temp != null)
                ret = temp;
            return ret;
        }

        object API_TryGiveKit(BasePlayer player, string kitname)
        {
            object ret = null;
            object temp = null;
            temp = HOOK__API_TryGiveKit(player, kitname);
            if (temp != null)
                ret = temp;
            return ret;
        }

        void OnServerInitialized()
        {
            HOOK__OnServerInitialized();
            DATA__OnServerInitialized();
            MESSAGES__OnServerInitialized();
        }

        void Unload()
        {
            HOOK__Unload();
        }

        void Loaded()
        {
            CONFIGURATION__Loaded();
        }
        #endregion
    }
}";
    }
}