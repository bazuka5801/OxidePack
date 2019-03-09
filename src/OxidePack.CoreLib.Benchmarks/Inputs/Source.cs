namespace Oxide.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BCore;
    using Oxide.Core;
    using UnityEngine;
    using Newtonsoft.Json;
    using Oxide.Core.Plugins;
    using Rust;
    using System.Text;
    using System.Collections;
    using Facepunch;
    using Random = UnityEngine.Random;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using Color = System.Drawing.Color;
    using Graphics = System.Drawing.Graphics;
    using UnityEngine.SceneManagement;

    [Info("NPCSystem", "bazuka5801", "1.2.471")]
    public class NpcSystem : RustPlugin
    {
        private NPCPlayerApex _myZombie;

        [ChatCommand("zs")]
        private void CmdChatZs(BasePlayer player, string command, string[] args)
        {
            if (player.IsAdmin)
            {
                if (args.Length == 0)
                {
                    return;
                }

                var option = args[0];

                if (args.Length == 1)
                {
                    if (args[0] == "img")
                    {
                        var bytes = GeneratePopulationImage();
                        var file = PathUtils.Combine(Interface.Oxide.DataDirectory, "NPCSystem", "Images", "img.png");
                        DirectoryUtils.CreateDirectoryForFileIfNotExist(file);
                        FileUtils.WriteAllBytes(file, bytes);
                        player.ChatMessage($"Saved '{file}'");
                        return;
                    }

                    return;
                }

                var zombiename = args[1];

                var zombieData = Configuration.Zombies.FirstOrDefault(zData =>
                    zData.Name.IndexOf(zombiename, StringComparison.InvariantCultureIgnoreCase) != -1);
                if (zombieData == null)
                {
                    player.ChatMessage($"Zombie not found with {args[1]} name.");
                    return;
                }

                if (option == "edit")
                {
                    Action<List<Item>, List<ItemData>> copyItems = (from, to) =>
                    {
                        to.Clear();
                        from.ForEach(item =>
                        {
                            to.Add(new ItemData
                            {
                                Shortname = item.info.shortname,
                                SkinID = item.skin
                            });
                        });
                    };

                    for (var i = 2; i < args.Length; i++)
                    {
                        if (args[i] == "items")
                        {
                            copyItems(player.inventory.containerWear.itemList, zombieData.WearItems);
                            copyItems(player.inventory.containerMain.itemList, zombieData.MainItems);
                            copyItems(player.inventory.containerBelt.itemList, zombieData.BeltItems);
                            SaveConfig(Configuration);
                            player.ChatMessage($"Loot changed for '{zombieData.Name}' zombie.");
                        }
                    }
                }

                if (option == "spawn")
                {
                    RaycastHit hit;
                    if (Physics.Raycast(player.eyes.HeadRay(), out hit, 1000, _groundLayer))
                    {
                        SpawnZombie(zombieData, hit.point, TerrainBiome.Enum.Arctic);
                    }
                }
            }
        }

        [ChatCommand("zstest")]
        private void CmdChatZsTest(BasePlayer player, string command, string[] args)
        {
            var zombieData = Configuration.Zombies[0];
            RaycastHit hit;
            if (Physics.Raycast(player.eyes.HeadRay(), out hit, 1000, _groundLayer))
            {
                SpawnZombie(zombieData, hit.point, TerrainBiome.Enum.Arctic);

                var zombie = _npcPlayers.Last();
                _myZombie = zombie;
            }
        }

        private void HOOK__OnNpcFactChanged(NPCPlayerApex npc, NPCPlayerApex.Facts fact, byte oldValue, byte newValue)
        {
            if (npc == _myZombie)
            {
                Server.Broadcast($"{fact} {oldValue} => {newValue}");
            }
        }

        public void UpdateTargetEntity(NPCPlayerApex npc, BaseEntity targetEntity)
        {
            if (npc == null || npc.GetNavAgent == null)
            {
                return;
            }

            if (targetEntity == null)
            {
                npc.AttackTarget = null;
                return;
            }

            if (targetEntity == npc.AttackTarget)
            {
                return;
            }

            npc.AttackTarget = targetEntity;
            npc.SetFact(NPCPlayerApex.Facts.Speed, (byte)NPCPlayerApex.SpeedEnum.Sprint, true, true);
        }

        public void UpdateTargetPosition(NPCPlayerApex npc, Vector3 targetPosition, bool isRegrouping = false)
        {
            if (npc == null || npc.GetNavAgent == null)
            {
                return;
            }

            npc.AttackTarget = null;

            npc.finalDestination = targetPosition;
            npc.Destination = targetPosition;
            npc.IsStopped = false;
            npc.SetFact(NPCPlayerApex.Facts.Speed,
                isRegrouping ? (byte)NPCPlayerApex.SpeedEnum.Sprint : (byte)NPCPlayerApex.SpeedEnum.Walk, true, true);
        }

        public partial class ConfigData
        {
            public BoundsData BoundsChecking = new BoundsData
            {
                CenterHeightOffset = 0.2f,
                Height = 0.2f,
                Length = 1f,
                Width = 1f
            };

            public BoundsData ConstructionsBoundsChecking = new BoundsData
            {
                CenterHeightOffset = 2f,
                Height = 0.2f,
                Length = 10f,
                Width = 10f
            };

            public List<NpcData> Zombies = new List<NpcData>();

            public class NpcData
            {
                public List<ItemData> BeltItems = new List<ItemData>();
                public float ChaseDistance = 75f;
                public string Color = "#ffffff";
                public string ColoredName;

                public Dictionary<string, MinMaxIntData> CustomLootTable = new Dictionary<string, MinMaxIntData>();
                public float Damage = 40f;
                public float DamageBleeding = 5f;

                public List<TerrainTopology.Enum> ExcludedTopologys = new List<TerrainTopology.Enum>
                {
                    TerrainTopology.Enum.Cliff
                };

                public float Health = 100f;

                public List<TerrainTopology.Enum> IncludedTopologys = new List<TerrainTopology.Enum>
                {
                    TerrainTopology.Enum.Beachside
                };

                public bool LongRange = true;
                public List<ItemData> MainItems = new List<ItemData>();
                public float MoveSpeedMultiplier = 1f;
                public string Name;
                public bool NoSprint = true;

                public Dictionary<TerrainBiome.Enum, int> Population = new Dictionary<TerrainBiome.Enum, int>
                {
                    [TerrainBiome.Enum.Arid] = 5,
                    [TerrainBiome.Enum.Arctic] = 5,
                    [TerrainBiome.Enum.Tundra] = 5,
                    [TerrainBiome.Enum.Temperate] = 5
                };

                public int RespawnSeconds = 10;

                public List<ItemData> WearItems = new List<ItemData>();
            }

            public class BoundsData
            {
                [JsonProperty("Смещение по высоте вверх от точки земли")]
                public float CenterHeightOffset = 0.5f;

                [JsonProperty("Высота бокса")] public float Height = 0.5f;

                [JsonProperty("Длина бокса")] public float Length = 1;

                [JsonProperty("Ширина бокса")] public float Width = 1;
            }
        }
        private Plugin LootEditor => Interface.Oxide.RootPluginManager.GetPlugin("LootEditor");

        /// <summary>
        /// When animal want to attack somebody
        /// </summary>
        /// <param name="animal">Animal</param>
        /// <param name="target">Target</param>
        /// <returns></returns>
        private object HOOK__OnNpcTarget(BaseNpc animal, BaseEntity target)
        {
            var npcPlayerApex = target as NPCPlayerApex;
            if (npcPlayerApex != null)
            {
                NpcEntity npcEntity;
                if (_npcEntities.TryGetValue(npcPlayerApex, out npcEntity))
                {
                    if (npcEntity.CanBeTargeted(animal) == false)
                        return false;
                }
            }

            return null;
        }

        private object HOOK__CanBeTargeted(BaseCombatEntity ent, AutoTurret turret)
        {
            var npcPlayerApex = ent as NPCPlayerApex;
            if (npcPlayerApex != null)
            {
                NpcEntity npcEntity;
                if (_npcEntities.TryGetValue(npcPlayerApex, out npcEntity))
                {
                    if (npcEntity.CanBeTargeted(turret) == false)
                        return false;
                }
            }

            return null;
        }

        private object HOOK__OnNpcPlayerTarget(NPCPlayerApex npc, BaseEntity target)
        {
            var npcPlayerApex = target as NPCPlayerApex;
            if (npcPlayerApex != null)
            {
                NpcEntity npcEntity;
                if (_npcEntities.TryGetValue(npcPlayerApex, out npcEntity))
                {
                    if (npcEntity.ValidateTarget(target) == false)
                        return false;
                }
            }

            return null;
        }

        private void HOOK__OnPlayerAttack(BasePlayer player, HitInfo info)
        {
            if (player.userID.IsSteamId() == false)
            {
                return;
            }

            var npc = info?.HitEntity as NPCPlayerApex;
            if (npc != null)
            {
                if (_npcPlayers.Contains(npc))
                {
                    timer.Once(1f, () =>
                    {
                        if (npc)
                        {
                        }
                    });
                }
            }
        }

        private void HOOK__OnEntityTakeDamage(BaseCombatEntity entity, HitInfo info)
        {
            var player = entity as BasePlayer;
            if (player == null || player.userID.IsSteamId() == false)
            {
                return;
            }

            var zombie = info.Initiator as NPCPlayerApex;
            if (zombie == null)
            {
                return;
            }

            if (_npcPlayers.Contains(zombie) == false)
            {
                return;
            }

            ConfigData.NpcData npcData;
            if (_zombiesData.TryGetValue(zombie, out npcData) == false)
            {
                return;
            }

            info.damageTypes.ScaleAll(0);
            info.damageTypes.Set(DamageType.Stab, npcData.Damage);
            player.metabolism.bleeding.Add(npcData.DamageBleeding);
            player.ScaleDamage(info);
            if (info.PointStart != Vector3.zero)
            {
                for (var i = 0; i < player.propDirection.Length; i++)
                {
                    if (player.propDirection[i].extraProtection != null)
                    {
                        if (!player.propDirection[i].IsWeakspot(player.transform, info))
                        {
                            player.propDirection[i].extraProtection.Scale(info.damageTypes, 1f);
                        }
                    }
                }
            }
        }

        private object HOOK__OnCreateCorpse(NPCPlayerApex npc, BaseCorpse corpse)
        {
            ConfigData.NpcData npcData;
            if (_zombiesData.TryGetValue(npc, out npcData))
            {
                var lootableCorpse = (LootableCorpse)corpse;
                // Add loot
                var container = lootableCorpse.containers[0];
                container.Clear();
                foreach (var kvp in npcData.CustomLootTable)
                {
                    var slots = kvp.Value.Value();
                    var table = kvp.Key;

                    var ret = LootEditor?.Call("SpawnIntoContainer", container, table, slots);
                    if (ret == null)
                    {
                        PrintError($"Plugin {nameof(LootEditor)} not loaded!");
                    }
                    else if ((bool)ret == false)
                    {
                        PrintError($"LootEditor haven't '{kvp.Key}' loot table in {npcData.Name} zombie!");
                    }
                }

                NextTick(() => { corpse.Kill(); });

                // Set name
                lootableCorpse.playerName = npcData.ColoredName;
                return corpse;
            }

            return null;
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
                return IsRange ? $"{this.Min}-{this.Max}" : $"{this.Min}";
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
                if (data.Contains("-") == false)
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
                if (data.Contains("-") == false)
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
                    sb.Append($"={this.Amount}");
                if (this.SkinID != 0)
                    sb.Append($":{this.SkinID}");
                if (this.Chance > 0)
                    sb.Append($"%{this.Chance}");
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
                var option = "";
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
                        case "":
                            data.Shortname = sb.ToString();
                            break;
                        case "%":
                            data.Chance = Convert.ToSingle(sb.ToString());
                            break;
                        case ":":
                            data.SkinID = Convert.ToUInt64(sb.ToString());
                            break;
                        case "=":
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


        public class NpcEntity : FacepunchBehaviour
        {
            private Dictionary<NPCPlayerApex, NpcEntity> _npcEntities =
                new Dictionary<NPCPlayerApex, NpcEntity>();

            private bool _autoBreaking;
            private NPCPlayerApex _npc;
            public ConfigData.NpcData npcData;

            private void Awake()
            {
                _npc = GetComponent<NPCPlayerApex>();
                _npcEntities[_npc] = this;
            }

            private void OnDestroy()
            {
                _npcEntities.Remove(_npc);
            }

            private void Start()
            {
                InvokeRepeating(NpcTick, 0, 0.1f);
            }

            private void NpcTick()
            {
                if (!_npc || _npc.IsDestroyed)
                {
                    return;
                }

                if (_npc.AttackTarget != null)
                {
                    if (npcData.ChaseDistance * npcData.ChaseDistance <
                        Vector3.SqrMagnitude(_npc.AttackTarget.ServerPosition - _npc.ServerPosition))
                    {
                        _npc.SetDestination(_npc.ServerPosition);
                        _npc.AttackTarget = null;
                        return;
                    }

                    if (npcData.LongRange == false)
                    {
                        AdjustAutoBreaking(false);

                        _npc.SetDestination(_npc.AttackTarget.ServerPosition +
                                           (_npc.AttackTarget.ServerPosition - _npc.ServerPosition).normalized);
                    }
                }
                else
                {
                    if (npcData.LongRange == false)
                    {
                        AdjustAutoBreaking(false);
                    }
                }
            }

            internal bool ValidateTarget(BaseEntity target)
            {
                var targetPlayer = target as BasePlayer;
                if (targetPlayer == null || targetPlayer.userID.IsSteamId() == false)
                {
                    return false;
                }

                return true;
            }

            public bool CanBeTargeted(BaseNpc animal)
            {
                return false;
            }

            public bool CanBeTargeted(AutoTurret turret)
            {
                if (turret is NPCAutoTurret)
                {
                    return false;
                }

                return false;
            }

            private void AdjustAutoBreaking(bool state)
            {
                if (_autoBreaking != state)
                {
                    _npc.GetNavAgent.autoBraking = state;
                    _autoBreaking = state;
                }
            }
        }
        private readonly Dictionary<TerrainBiome.Enum, List<Vector3>> _biomeSpawns =
            new Dictionary<TerrainBiome.Enum, List<Vector3>>();

        private readonly int _groundLayer = LayerMask.GetMask("Terrain", "Construction", "World");

        private readonly Dictionary<NPCPlayerApex, TerrainBiome.Enum> _zombieBiomes =
            new Dictionary<NPCPlayerApex, TerrainBiome.Enum>();

        private readonly HashSet<NPCPlayerApex> _npcPlayers = new HashSet<NPCPlayerApex>();

        private readonly Dictionary<NPCPlayerApex, ConfigData.NpcData> _zombiesData =
            new Dictionary<NPCPlayerApex, ConfigData.NpcData>();

        private readonly Dictionary<NPCPlayerApex, NpcEntity> _npcEntities =
            new Dictionary<NPCPlayerApex, NpcEntity>();

        private IEnumerator _initCoroutine;

        private void Spawner__OnServerInitialized()
        {
            _initCoroutine = SpawnerInitialization();
            ServerMgr.Instance.StartCoroutine(_initCoroutine);
        }

        private IEnumerator SpawnerInitialization()
        {
            var enumerator = Populate();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        private void Spawner__Unload()
        {
            ServerMgr.Instance.StopCoroutine(_initCoroutine);
            foreach (var zombie in _npcPlayers)
            {
                UnityEngine.Object.Destroy(zombie.gameObject.GetComponent<NpcEntity>());
                zombie.Kill();
            }
            _npcPlayers.Clear();
            _zombieBiomes.Clear();
            _zombiesData.Clear();
            _npcEntities.Clear();
            _biomeSpawns.Clear();
        }

        private void Spawner__OnEntityKill(NPCPlayerApex zombie)
        {
            if (_npcPlayers.Contains(zombie))
            {
                UnityEngine.Object.Destroy(zombie.gameObject.GetComponent<NpcEntity>());
                _npcPlayers.Remove(zombie);
                ConfigData.NpcData zData;
                if (_zombiesData.TryGetValue(zombie, out zData))
                {
                    _zombiesData.Remove(zombie);
                    var biome = _zombieBiomes[zombie];
                    _zombieBiomes.Remove(zombie);
                    timer.Once(zData.RespawnSeconds,
                        () => { FindZombiePosition(vec => SpawnZombie(zData, vec, biome), zData, true, biome); });
                }
            }
        }

        private IEnumerator GenerateBiomeSpawns()
        {
            for (var i = 0; i < TerrainBiome.COUNT; i++)
            {
                _biomeSpawns[(TerrainBiome.Enum)TerrainBiome.IndexToType(i)] = new List<Vector3>();
            }

            var totalSize = (int)(TerrainMeta.Size.x * TerrainMeta.Size.z);
            var progress = 0;

            var worldSize05 = (int)Mathf.Floor(TerrainMeta.Size.x / 2f);
            //            worldSize05 /=5;
            for (var z = -worldSize05 + 1; z < worldSize05; z++)
            {
                if (z % 200 == 0)
                {
                    yield return CoroutineEx.waitForFixedUpdate;
                    yield return CoroutineEx.waitForFixedUpdate;
                    yield return CoroutineEx.waitForFixedUpdate;
                    yield return CoroutineEx.waitForFixedUpdate;
                }


                for (var x = -worldSize05 + 1; x < worldSize05; x++)
                {
                    if (++progress % (totalSize / 20) == 0)
                    {
                        Puts($"Generating BiomeSpawns: {100f * progress / totalSize}%");
                    }

                    for (var i = 0; i < TerrainBiome.COUNT; i++)
                    {
                        var type = TerrainBiome.IndexToType(i);
                        if (TerrainMeta.BiomeMap.GetBiome(TerrainMeta.NormalizeX(x), TerrainMeta.NormalizeZ(z), type) >
                            0.5)
                        {
                            var result = Vector3.zero;
                            if (GetGround(new Vector3(x, 0, z), out result))
                            {
                                _biomeSpawns[(TerrainBiome.Enum)type].Add(result);
                            }
                        }
                    }
                }
            }
        }

        private void FindZombiePosition(Action<Vector3> callback, ConfigData.NpcData npcData, bool useBiome = false,
            TerrainBiome.Enum biome = TerrainBiome.Enum.Temperate)
        {
            var size = TerrainMeta.Size.x / 2;
            var vec = new Vector3(Random.Range(-size, size), 200, Random.Range(-size, size));

            var height = TerrainMeta.HeightMap.GetHeight(vec) - TerrainMeta.WaterMap.GetHeight(vec);
            if (height > 0.2f)
            {
                if (!useBiome || TerrainMeta.BiomeMap.GetBiome(vec.x, vec.z, (int)biome) > 0.5f)
                {
                    if (npcData.ExcludedTopologys.All(p =>
                            TerrainMeta.TopologyMap.GetTopology(vec, (int)p) == false) &&
                        npcData.IncludedTopologys.Any(p => TerrainMeta.TopologyMap.GetTopology(vec, (int)p)))
                    {
                        Vector3 position;
                        if (GetGround(vec, out position))
                        {
                            var blocks = Pool.GetList<BuildingBlock>();
                            Vis.Entities(position, 10, blocks, LayerMask.GetMask("Construction"));
                            if (blocks.Count == 0)
                            {
                                if (false == CheckBounds(position, Configuration.BoundsChecking, "World", "Terrain"))
                                {
                                    callback(position);
                                    return;
                                }
                            }

                            Pool.FreeList(ref blocks);
                        }
                    }
                }
            }

            timer.Once(0.05f, () => FindZombiePosition(callback, npcData, useBiome, biome));
        }


        private bool CheckBounds(Vector3 position, ConfigData.BoundsData boundsData, params string[] layers)
        {
            var boundsCenter = position + new Vector3(0, boundsData.CenterHeightOffset, 0);
            var boundsSize = new Vector3(boundsData.Width, boundsData.Height, boundsData.Length);
            var bounds = new Bounds(boundsCenter, boundsSize);
            return GamePhysics.CheckBounds(bounds, LayerMask.GetMask(layers));
        }

        private IEnumerator Populate()
        {
            var totalZombies = 0;
            foreach (var zombieData in Configuration.Zombies)
                foreach (var quantityItem in zombieData.Population)
                {
                    var biome = quantityItem.Key;
                    var quantity = quantityItem.Value;

                    for (var i = 0; i < quantity; i++)
                    {
                        totalZombies++;
                        if (totalZombies % 10 == 0)
                        {
                            yield return CoroutineEx.waitForFixedUpdate;
                        }

                        FindZombiePosition(vec => SpawnZombie(zombieData, vec, biome), zombieData, true, biome);
                    }
                }
        }

        private bool GetGround(Vector3 pos, out Vector3 result)
        {
            RaycastHit hit;
            if (Physics.Raycast(pos + new Vector3(0, 200, 0), Vector3.down, out hit, 1000, _groundLayer))
            {
                result = hit.point;

                if (WaterLevel.Test(hit.point))
                {
                    return false;
                }

                return true;
            }

            result = pos;
            return false;
        }

        /// <summary>
        ///     Generate npc population image
        /// </summary>
        /// <param name="rectangleSize"></param>
        /// <returns>PNG Bytes</returns>
        private byte[] GeneratePopulationImage(int rectangleSize = 16)
        {
            var mapBytes = MapGenerator.GetMapImage(4096);
            var mapImage = (Bitmap)new ImageConverter().ConvertFrom(mapBytes);


            var img = new Bitmap(mapImage.Width, mapImage.Height);
            var graphic = Graphics.FromImage(img);
            graphic.DrawImage(mapImage, new Rectangle(0, 0, mapImage.Width, mapImage.Height), 0, 0, mapImage.Width,
                mapImage.Height, GraphicsUnit.Pixel);

            var pen = new Pen(Color.Blue);
            foreach (var zombie in _zombiesData)
            {
                var imagePos = EntityPosToImagePos(mapImage, zombie.Key.ServerPosition);
                pen.Color = ColorTranslator.FromHtml(zombie.Value.Color);
                var sizeHalf = rectangleSize / 2;
                graphic.FillRectangle(pen.Brush, imagePos.x - sizeHalf, imagePos.y - sizeHalf, sizeHalf, sizeHalf);
            }

            graphic.Dispose();
            pen.Dispose();
            var ms = Pool.Get<MemoryStream>();
            img.Save(ms, ImageFormat.Png);
            var imgBytes = ms.ToArray();
            Pool.FreeMemoryStream(ref ms);
            return imgBytes;
        }

        private Vector2 EntityPosToImagePos(Bitmap img, Vector3 position)
        {
            var screenPos = ToScreenCoords(position);
            return ScreenToImagePos(img, screenPos);
        }

        private Vector2i ScreenToImagePos(Bitmap img, Vector3 screenPos) =>
            new Vector2i((int)(screenPos.x * img.Width),
                img.Height - (int)(screenPos.y * img.Height));

        private Vector2 ToScreenCoords(Vector3 vec)
        {
            var x = (vec.x + (int)World.Size * 0.5f) / (int)World.Size;
            var z = (vec.z + (int)World.Size * 0.5f) / (int)World.Size;
            return new Vector2(x, z);
        }

        private void SpawnZombie(ConfigData.NpcData npcData, Vector3 position, TerrainBiome.Enum biome)
        {
            var prefab = GameManager.server.FindPrefab(npcData.LongRange
                ? "assets/prefabs/npc/scientist/scientist.prefab"
                : "assets/prefabs/npc/murderer/murderer.prefab");
            var gameObject = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
            gameObject.name = prefab.name;
            SceneManager.MoveGameObjectToScene(gameObject, Rust.Server.EntityScene);
            if (gameObject.GetComponent<Spawnable>())
            {
                UnityEngine.Object.Destroy(gameObject.GetComponent<Spawnable>());
            }

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            var component = gameObject.GetComponent<BaseEntity>();

            var zombie = component as NPCPlayerApex;
            if (zombie == null)
            {
                return;
            }

            zombie.transform.position = position;


            zombie.displayName = npcData.Name;
            zombie.Spawn();

            // Remove radio chat effect
            zombie.CancelInvoke(zombie.RadioChatter);

            zombie.InitializeHealth(npcData.Health, npcData.Health);
            zombie.CommunicationRadius = 0;
            zombie.clothingMoveSpeedReduction = 1 - npcData.MoveSpeedMultiplier;
            zombie.SetPlayerFlag(BasePlayer.PlayerFlags.NoSprint, npcData.NoSprint);
            zombie.Stats.AggressionRange = zombie.Stats.DeaggroRange = npcData.ChaseDistance;
            zombie.Stats.CloseRange = 1;
            zombie.StoppingDistance = 0;
            zombie.inventory.containerWear.Clear();
            zombie.inventory.containerWear.capacity = 10;
            npcData.WearItems.ForEach(item => { item.CreateItem().MoveToContainer(zombie.inventory.containerWear); });

            zombie.inventory.containerBelt.Clear();
            npcData.BeltItems.ForEach(item => { item.CreateItem().MoveToContainer(zombie.inventory.containerBelt); });

            zombie.inventory.containerMain.Clear();
            npcData.MainItems.ForEach(item => { item.CreateItem().MoveToContainer(zombie.inventory.containerMain); });
            zombie.clothingMoveSpeedReduction = 1 - npcData.MoveSpeedMultiplier;
            zombie.svActiveItemID = 0;
            zombie.CommunicationRadius = 0;

            var npcEnt = zombie.gameObject.AddComponent<NpcEntity>();
            npcEnt.npcData = npcData;

            _npcPlayers.Add(zombie);
            _zombiesData[zombie] = npcData;
            _zombieBiomes[zombie] = biome;
            _npcEntities[zombie] = npcEnt;
        }
        /*
                private void HOOK__OnNpcFactChanged(NPCPlayerApex npc, NPCPlayerApex.Facts fact, byte oldValue, byte newValue)
                {
                    if (fact == NPCPlayerApex.Facts.EnemyEngagementRange && newValue == 1)
                    {
                        npc.UpdateActiveItem(0);
                        Puts("OnNpcDeaggro " + ((NPCPlayerApex.EnemyEngagementRangeEnum)newValue) + " "+npc.userID);
                    }
                    if (fact == NPCPlayerApex.Facts.EnemyEngagementRange && newValue == 0)
                    {
                    //    int index = NPCPlayerApex.PlayerTargetContext.Index;
                        npc.UpdateActiveItem(npc.inventory.containerBelt.itemList[0].uid);
                        Puts("OnNpcAggro " + ((NPCPlayerApex.EnemyEngagementRangeEnum)newValue) + " "+npc.userID);
                    }
                }
        */

        #region [Generated] [Hook Methods]
        [Oxide.Core.Plugins.HookMethod("OnNpcFactChanged")]
        void OnNpcFactChanged(NPCPlayerApex npc, NPCPlayerApex.Facts fact, byte oldValue, byte newValue)
        {
            HOOK__OnNpcFactChanged(npc, fact, oldValue, newValue);
        }

        [Oxide.Core.Plugins.HookMethod("OnNpcTarget")]
        object OnNpcTarget(BaseNpc animal, BaseEntity target)
        {
            object ret = null;
            object temp = null;
            temp = HOOK__OnNpcTarget(animal, target);
            if (temp != null)
                ret = temp;
            return ret;
        }

        [Oxide.Core.Plugins.HookMethod("CanBeTargeted")]
        object CanBeTargeted(BaseCombatEntity ent, AutoTurret turret)
        {
            object ret = null;
            object temp = null;
            temp = HOOK__CanBeTargeted(ent, turret);
            if (temp != null)
                ret = temp;
            return ret;
        }

        [Oxide.Core.Plugins.HookMethod("OnNpcPlayerTarget")]
        object OnNpcPlayerTarget(NPCPlayerApex npc, BaseEntity target)
        {
            object ret = null;
            object temp = null;
            temp = HOOK__OnNpcPlayerTarget(npc, target);
            if (temp != null)
                ret = temp;
            return ret;
        }

        [Oxide.Core.Plugins.HookMethod("OnPlayerAttack")]
        void OnPlayerAttack(BasePlayer player, HitInfo info)
        {
            HOOK__OnPlayerAttack(player, info);
        }

        [Oxide.Core.Plugins.HookMethod("OnEntityTakeDamage")]
        void OnEntityTakeDamage(BaseCombatEntity entity, HitInfo info)
        {
            HOOK__OnEntityTakeDamage(entity, info);
        }

        [Oxide.Core.Plugins.HookMethod("OnCreateCorpse")]
        object OnCreateCorpse(NPCPlayerApex npc, BaseCorpse corpse)
        {
            object ret = null;
            object temp = null;
            temp = HOOK__OnCreateCorpse(npc, corpse);
            if (temp != null)
                ret = temp;
            return ret;
        }

        [Oxide.Core.Plugins.HookMethod("Loaded")]
        void Loaded()
        {
            CONFIGURATION__Loaded();
        }

        [Oxide.Core.Plugins.HookMethod("OnServerInitialized")]
        void OnServerInitialized()
        {
            Spawner__OnServerInitialized();
        }

        [Oxide.Core.Plugins.HookMethod("Unload")]
        void Unload()
        {
            Spawner__Unload();
        }

        [Oxide.Core.Plugins.HookMethod("OnEntityKill")]
        void OnEntityKill(NPCPlayerApex zombie)
        {
            Spawner__OnEntityKill(zombie);
        }
        #endregion
    }
}