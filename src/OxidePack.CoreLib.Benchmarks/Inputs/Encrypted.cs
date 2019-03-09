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
        private NPCPlayerApex generated_0;

        [ChatCommand("zs")]
        private void generated_1(BasePlayer generated_2, string generated_3, string[] generated_4)
        {
            if (generated_2.IsAdmin)
            {
                if (generated_4.Length == 0)
                {
                    return;
                }

                var generated_5 = generated_4[0];

                if (generated_4.Length == 1)
                {
                    if (generated_4[0] == "img")
                    {
                        var generated_6 = generated_7();
                        var generated_8 = PathUtils.Combine(Interface.Oxide.DataDirectory, "NPCSystem", "Images", "img.png");
                        DirectoryUtils.CreateDirectoryForFileIfNotExist(generated_8);
                        FileUtils.WriteAllBytes(generated_8, generated_6);
                        generated_2.ChatMessage($"Saved '{generated_8}'");
                        return;
                    }

                    return;
                }

                var generated_9 = generated_4[1];

                generated_10 generated_11 = generated_12.generated_13.FirstOrDefault(generated_14 =>
                    generated_14.generated_15.IndexOf(generated_9, StringComparison.InvariantCultureIgnoreCase) != -1);
                if (generated_11 == null)
                {
                    generated_2.ChatMessage($"Zombie not found with {generated_4[1]} name.");
                    return;
                }

                if (generated_5 == "edit")
                {
                    Action<List<Item>, List<generated_16>> generated_17 = (generated_18, generated_19) =>
                    {
                        generated_19.Clear();
                        generated_18.ForEach(generated_20 =>
                        {
                            generated_19.Add(new generated_16
                            {
                                generated_21 = generated_20.info.shortname,
                                generated_22 = generated_20.skin
                            });
                        });
                    };

                    for (var generated_23 = 2; generated_23 < generated_4.Length; generated_23++)
                    {
                        if (generated_4[generated_23] == "items")
                        {
                            generated_17(generated_2.inventory.containerWear.itemList, generated_11.generated_24);
                            generated_17(generated_2.inventory.containerMain.itemList, generated_11.generated_25);
                            generated_17(generated_2.inventory.containerBelt.itemList, generated_11.generated_26);
                            generated_27(generated_12);
                            generated_2.ChatMessage($"Loot changed for '{generated_11.generated_15}' zombie.");
                        }
                    }
                }

                if (generated_5 == "spawn")
                {
                    RaycastHit generated_28;
                    if (Physics.Raycast(generated_2.eyes.HeadRay(), out generated_28, 1000, generated_29))
                    {
                        generated_30(generated_11, generated_28.point, TerrainBiome.Enum.Arctic);
                    }
                }
            }
        }

        [ChatCommand("zstest")]
        private void generated_31(BasePlayer generated_32, string generated_33, string[] generated_34)
        {
            generated_10 generated_35 = generated_12.generated_13[0];
            RaycastHit generated_36;
            if (Physics.Raycast(generated_32.eyes.HeadRay(), out generated_36, 1000, generated_29))
            {
                generated_30(generated_35, generated_36.point, TerrainBiome.Enum.Arctic);

                var generated_37 = generated_38.Last();
                generated_0 = generated_37;
            }
        }

        private void generated_39(NPCPlayerApex generated_40, NPCPlayerApex.Facts generated_41, byte generated_42, byte generated_43)
        {
            if (generated_40 == generated_0)
            {
                Server.Broadcast($"{generated_41} {generated_42} => {generated_43}");
            }
        }

        public void generated_44(NPCPlayerApex generated_45, BaseEntity generated_46)
        {
            if (generated_45 == null || generated_45.GetNavAgent == null)
            {
                return;
            }

            if (generated_46 == null)
            {
                generated_45.AttackTarget = null;
                return;
            }

            if (generated_46 == generated_45.AttackTarget)
            {
                return;
            }

            generated_45.AttackTarget = generated_46;
            generated_45.SetFact(NPCPlayerApex.Facts.Speed, (byte)NPCPlayerApex.SpeedEnum.Sprint, true, true);
        }

        public void generated_47(NPCPlayerApex generated_48, Vector3 generated_49, bool generated_50 = false)
        {
            if (generated_48 == null || generated_48.GetNavAgent == null)
            {
                return;
            }

            generated_48.AttackTarget = null;

            generated_48.finalDestination = generated_49;
            generated_48.Destination = generated_49;
            generated_48.IsStopped = false;
            generated_48.SetFact(NPCPlayerApex.Facts.Speed,
                generated_50 ? (byte)NPCPlayerApex.SpeedEnum.Sprint : (byte)NPCPlayerApex.SpeedEnum.Walk, true, true);
        }

        public partial class generated_51
        {
            public generated_52 generated_53 = new generated_52
            {
                generated_54 = 0.2f,
                generated_55 = 0.2f,
                generated_56 = 1f,
                generated_57 = 1f
            };

            public generated_52 generated_58 = new generated_52
            {
                generated_54 = 2f,
                generated_55 = 0.2f,
                generated_56 = 10f,
                generated_57 = 10f
            };

            public List<generated_10> generated_13 = new List<generated_10>();

            public class generated_10
            {
                public List<generated_16> generated_26 = new List<generated_16>();
                public float generated_59 = 75f;
                public string generated_60 = "#ffffff";
                public string generated_61;

                public Dictionary<string, generated_62> generated_63 = new Dictionary<string, generated_62>();
                public float generated_64 = 40f;
                public float generated_65 = 5f;

                public List<TerrainTopology.Enum> generated_66 = new List<TerrainTopology.Enum>
                {
                    TerrainTopology.Enum.Cliff
                };

                public float generated_67 = 100f;

                public List<TerrainTopology.Enum> generated_68 = new List<TerrainTopology.Enum>
                {
                    TerrainTopology.Enum.Beachside
                };

                public bool generated_69 = true;
                public List<generated_16> generated_25 = new List<generated_16>();
                public float generated_70 = 1f;
                public string generated_15;
                public bool generated_71 = true;

                public Dictionary<TerrainBiome.Enum, int> generated_72 = new Dictionary<TerrainBiome.Enum, int>
                {
                    [TerrainBiome.Enum.Arid] = 5,
                    [TerrainBiome.Enum.Arctic] = 5,
                    [TerrainBiome.Enum.Tundra] = 5,
                    [TerrainBiome.Enum.Temperate] = 5
                };

                public int generated_73 = 10;

                public List<generated_16> generated_24 = new List<generated_16>();
            }

            public class generated_52
            {
                [JsonProperty("Смещение по высоте вверх от точки земли")]
                public float generated_54 = 0.5f;

                [JsonProperty("Высота бокса")] public float generated_55 = 0.5f;

                [JsonProperty("Длина бокса")] public float generated_56 = 1;

                [JsonProperty("Ширина бокса")] public float generated_57 = 1;
            }
        }
        private Plugin generated_74 => Interface.Oxide.RootPluginManager.GetPlugin("LootEditor");

        /// <summary>
        /// When animal want to attack somebody
        /// </summary>
        /// <param name="generated_75">Animal</param>
        /// <param name="generated_76">Target</param>
        /// <returns></returns>
        private object generated_77(BaseNpc generated_75, BaseEntity generated_76)
        {
            var generated_78 = generated_76 as NPCPlayerApex;
            if (generated_78 != null)
            {
                generated_79 generated_80;
                if (generated_81.TryGetValue(generated_78, out generated_80))
                {
                    if (generated_80.generated_82(generated_75) == false)
                        return false;
                }
            }

            return null;
        }

        private object generated_83(BaseCombatEntity generated_84, AutoTurret generated_85)
        {
            var generated_86 = generated_84 as NPCPlayerApex;
            if (generated_86 != null)
            {
                generated_79 generated_87;
                if (generated_81.TryGetValue(generated_86, out generated_87))
                {
                    if (generated_87.generated_82(generated_85) == false)
                        return false;
                }
            }

            return null;
        }

        private object generated_88(NPCPlayerApex generated_89, BaseEntity generated_90)
        {
            var generated_91 = generated_90 as NPCPlayerApex;
            if (generated_91 != null)
            {
                generated_79 generated_92;
                if (generated_81.TryGetValue(generated_91, out generated_92))
                {
                    if (generated_92.generated_93(generated_90) == false)
                        return false;
                }
            }

            return null;
        }

        private void generated_94(BasePlayer generated_95, HitInfo generated_96)
        {
            if (generated_95.userID.IsSteamId() == false)
            {
                return;
            }

            var generated_97 = generated_96?.HitEntity as NPCPlayerApex;
            if (generated_97 != null)
            {
                if (generated_38.Contains(generated_97))
                {
                    timer.Once(1f, () =>
                    {
                        if (generated_97)
                        {
                        }
                    });
                }
            }
        }

        private void generated_98(BaseCombatEntity generated_99, HitInfo generated_100)
        {
            var generated_101 = generated_99 as BasePlayer;
            if (generated_101 == null || generated_101.userID.IsSteamId() == false)
            {
                return;
            }

            var generated_102 = generated_100.Initiator as NPCPlayerApex;
            if (generated_102 == null)
            {
                return;
            }

            if (generated_38.Contains(generated_102) == false)
            {
                return;
            }

            generated_51.generated_10 generated_103;
            if (generated_104.TryGetValue(generated_102, out generated_103) == false)
            {
                return;
            }

            generated_100.damageTypes.ScaleAll(0);
            generated_100.damageTypes.Set(DamageType.Stab, generated_103.generated_64);
            generated_101.metabolism.bleeding.Add(generated_103.generated_65);
            generated_101.ScaleDamage(generated_100);
            if (generated_100.PointStart != Vector3.zero)
            {
                for (var generated_105 = 0; generated_105 < generated_101.propDirection.Length; generated_105++)
                {
                    if (generated_101.propDirection[generated_105].extraProtection != null)
                    {
                        if (!generated_101.propDirection[generated_105].IsWeakspot(generated_101.transform, generated_100))
                        {
                            generated_101.propDirection[generated_105].extraProtection.Scale(generated_100.damageTypes, 1f);
                        }
                    }
                }
            }
        }

        private object generated_106(NPCPlayerApex generated_107, BaseCorpse generated_108)
        {
            generated_51.generated_10 generated_109;
            if (generated_104.TryGetValue(generated_107, out generated_109))
            {
                var generated_110 = (LootableCorpse)generated_108;
                // Add loot
                var generated_111 = generated_110.containers[0];
                generated_111.Clear();
                foreach (var generated_112 in generated_109.generated_63)
                {
                    var generated_113 = generated_112.Value.generated_114();
                    var generated_115 = generated_112.Key;

                    var generated_116 = generated_74?.Call("SpawnIntoContainer", generated_111, generated_115, generated_113);
                    if (generated_116 == null)
                    {
                        PrintError($"Plugin {generated_117(generated_74)} not loaded!");
                    }
                    else if ((bool)generated_116 == false)
                    {
                        PrintError($"LootEditor haven't '{generated_112.Key}' loot table in {generated_109.generated_15} zombie!");
                    }
                }

                NextTick(() => { generated_108.Kill(); });

                // Set name
                generated_110.playerName = generated_109.generated_61;
                return generated_108;
            }

            return null;
        }

        #region  [Module] Configuration
        private generated_51 generated_12;
        partial class generated_51
        {
        }

        void generated_118()
        {
            generated_119();
        }

        private void generated_119()
        {
            generated_120();
            generated_27(generated_12);
        }

        private void generated_120()
        {
            Config.Settings.Converters = this.generated_121;
            base.Config.Settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            generated_12 = Config.ReadObject<generated_51>();
        }

        protected override void LoadDefaultConfig()
        {
            Config.Settings.Converters = this.generated_121;
            generated_51 generated_122 = new generated_51();
            generated_27(generated_122);
        }

        void generated_27(object generated_123) => Config.WriteObject(generated_123, true);
        private List<JsonConverter> generated_121 = new List<JsonConverter>()
{new generated_124(), new generated_125(), new generated_126(), };
        #region [Types] Custom Fields
        #region [Folder] MinMaxData
        #region [Type] MinMaxDataBase<T>
        public abstract class generated_127<generated_128>
            where generated_128 : struct, IEquatable<generated_128>
        {
            public generated_128 generated_129;
            public generated_128 generated_130;
            public bool generated_131 => this.generated_129.Equals(this.generated_130) == false;
            public generated_127()
            {
            }

            public generated_127(generated_128 generated_132, generated_128 generated_133)
            {
                this.generated_129 = generated_132;
                this.generated_130 = generated_133;
            }

            public abstract generated_128 generated_134();
            public generated_128 generated_114() => this.generated_131 ? this.generated_134() : this.generated_129;
            public override string ToString()
            {
                return generated_131 ? $"{this.generated_129}-{this.generated_130}" : $"{this.generated_129}";
            }

            public abstract generated_127<generated_128> generated_135(string generated_136);
            public string generated_137() => this.ToString();
        }

        #endregion
        #region [Field] MinMaxIntData
        public class generated_62 : generated_127<Int32>
        {
            public generated_62()
            {
            }

            public generated_62(Int32 generated_138, Int32 generated_139) : base(generated_138, generated_139)
            {
            }

            public override Int32 generated_134() => UnityEngine.Random.Range(this.generated_129, this.generated_130 + 1);
            public override generated_127<int> generated_135(string generated_140)
            {
                if (generated_140.Contains("-") == false)
                {
                    this.generated_129 = this.generated_130 = Convert.ToInt32(generated_140);
                    return this;
                }

                var generated_141 = generated_140.Trim().Split('-');
                this.generated_129 = Convert.ToInt32(generated_141[0]);
                this.generated_130 = Convert.ToInt32(generated_141[1]);
                return this;
            }
        }

        public class generated_124 : JsonConverter
        {
            public override void WriteJson(JsonWriter generated_142, object generated_143, JsonSerializer generated_144)
            {
                generated_62 generated_145 = (generated_62)generated_143;
                generated_142.WriteValue(generated_145.generated_137());
            }

            public override object ReadJson(JsonReader generated_146, Type generated_147, object generated_148, JsonSerializer generated_149)
            {
                return new generated_62().generated_135(generated_146.Value.ToString());
            }

            public override bool CanConvert(Type generated_150) => generated_150 == typeof(generated_62);
        }

        #endregion
        #region [Field] MinMaxFloatData
        public class generated_151 : generated_127<Single>
        {
            public generated_151()
            {
            }

            public generated_151(Single generated_152, Single generated_153) : base(generated_152, generated_153)
            {
            }

            public override Single generated_134() => UnityEngine.Random.Range(this.generated_129, this.generated_130);
            public override generated_127<Single> generated_135(string generated_154)
            {
                if (generated_154.Contains("-") == false)
                {
                    this.generated_129 = this.generated_130 = Convert.ToSingle(generated_154);
                    return this;
                }

                var generated_155 = generated_154.Trim().Split('-');
                return new generated_151(Convert.ToSingle(generated_155[0]), Convert.ToSingle(generated_155[1]));
            }
        }

        public class generated_125 : JsonConverter
        {
            public override void WriteJson(JsonWriter generated_156, object generated_157, JsonSerializer generated_158)
            {
                generated_151 generated_159 = (generated_151)generated_157;
                generated_156.WriteValue(generated_159.generated_137());
            }

            public override object ReadJson(JsonReader generated_160, Type generated_161, object generated_162, JsonSerializer generated_163)
            {
                return new generated_62().generated_135(generated_160.Value.ToString());
            }

            public override bool CanConvert(Type generated_164) => generated_164 == typeof(generated_151);
        }

        #endregion
        #endregion
        #region [Field] ItemAmount
        public class generated_16
        {
            public string generated_21;
            public generated_62 generated_165 = new generated_62(1, 1);
            public ulong generated_22 = 0;
            public float generated_166 = -1f;
            public Item generated_167()
            {
                var generated_168 = ItemManager.CreateByPartialName(generated_21, generated_165.generated_114());
                generated_168.skin = generated_22;
                return generated_168;
            }

            public override string ToString()
            {
                StringBuilder generated_169 = new StringBuilder(20);
                generated_169.Append(this.generated_21);
                if (this.generated_165.generated_114() >= 0)
                    generated_169.Append($"={this.generated_165}");
                if (this.generated_22 != 0)
                    generated_169.Append($":{this.generated_22}");
                if (this.generated_166 > 0)
                    generated_169.Append($"%{this.generated_166}");
                var generated_170 = generated_169.ToString();
                generated_169.Clear();
                generated_169 = null;
                return generated_170;
            }
        }

        public class generated_126 : JsonConverter
        {
            private readonly List<char> generated_171 = new List<char>()
    {'=', ':', '%'};
            public override void WriteJson(JsonWriter generated_172, object generated_173, JsonSerializer generated_174)
            {
                generated_16 generated_175 = (generated_16)generated_173;
                generated_172.WriteValue(generated_175.ToString());
            }

            public override object ReadJson(JsonReader generated_176, Type generated_177, object generated_178, JsonSerializer generated_179)
            {
                var generated_180 = generated_176.Value.ToString().Trim();
                generated_16 generated_181 = new generated_16();
                StringBuilder generated_182 = new StringBuilder();
                var generated_183 = "";
                for (var generated_184 = 0; generated_184 < generated_180.Length; generated_184++)
                {
                    var generated_185 = generated_180[generated_184];
                    if (generated_171.Contains(generated_185) == false)
                    {
                        generated_182.Append(generated_185);
                        if (generated_184 != generated_180.Length - 1)
                            continue;
                    }

                    switch (generated_183)
                    {
                        case "":
                            generated_181.generated_21 = generated_182.ToString();
                            break;
                        case "%":
                            generated_181.generated_166 = Convert.ToSingle(generated_182.ToString());
                            break;
                        case ":":
                            generated_181.generated_22 = Convert.ToUInt64(generated_182.ToString());
                            break;
                        case "=":
                            generated_181.generated_165 = (generated_62)new generated_62().generated_135(generated_182.ToString());
                            break;
                    }

                    generated_182.Clear();
                    generated_183 = generated_185.ToString();
                }

                return generated_181;
            }

            public override bool CanConvert(Type generated_186)
            {
                return generated_186 == typeof(generated_16);
            }
        }
        #endregion
        #endregion
        #endregion


        public class generated_79 : FacepunchBehaviour
        {
            private Dictionary<NPCPlayerApex, generated_79> generated_187 =
                new Dictionary<NPCPlayerApex, generated_79>();

            private bool generated_188;
            private NPCPlayerApex generated_189;
            public generated_51.generated_10 generated_190;

            private void generated_191()
            {
                generated_189 = GetComponent<NPCPlayerApex>();
                generated_187[generated_189] = this;
            }

            private void generated_192()
            {
                generated_187.Remove(generated_189);
            }

            private void generated_193()
            {
                InvokeRepeating(generated_194, 0, 0.1f);
            }

            private void generated_194()
            {
                if (!generated_189 || generated_189.IsDestroyed)
                {
                    return;
                }

                if (generated_189.AttackTarget != null)
                {
                    if (generated_190.generated_59 * generated_190.generated_59 <
                        Vector3.SqrMagnitude(generated_189.AttackTarget.ServerPosition - generated_189.ServerPosition))
                    {
                        generated_189.SetDestination(generated_189.ServerPosition);
                        generated_189.AttackTarget = null;
                        return;
                    }

                    if (generated_190.generated_69 == false)
                    {
                        generated_195(false);

                        generated_189.SetDestination(generated_189.AttackTarget.ServerPosition +
                                           (generated_189.AttackTarget.ServerPosition - generated_189.ServerPosition).normalized);
                    }
                }
                else
                {
                    if (generated_190.generated_69 == false)
                    {
                        generated_195(false);
                    }
                }
            }

            internal bool generated_93(BaseEntity generated_196)
            {
                var generated_197 = generated_196 as BasePlayer;
                if (generated_197 == null || generated_197.userID.IsSteamId() == false)
                {
                    return false;
                }

                return true;
            }

            public bool generated_82(BaseNpc generated_198)
            {
                return false;
            }

            public bool generated_82(AutoTurret generated_199)
            {
                if (generated_199 is NPCAutoTurret)
                {
                    return false;
                }

                return false;
            }

            private void generated_195(bool generated_200)
            {
                if (generated_188 != generated_200)
                {
                    generated_189.GetNavAgent.autoBraking = generated_200;
                    generated_188 = generated_200;
                }
            }
        }
        private readonly Dictionary<TerrainBiome.Enum, List<Vector3>> generated_201 =
            new Dictionary<TerrainBiome.Enum, List<Vector3>>();

        private readonly int generated_29 = LayerMask.GetMask("Terrain", "Construction", "World");

        private readonly Dictionary<NPCPlayerApex, TerrainBiome.Enum> generated_202 =
            new Dictionary<NPCPlayerApex, TerrainBiome.Enum>();

        private readonly HashSet<NPCPlayerApex> generated_38 = new HashSet<NPCPlayerApex>();

        private readonly Dictionary<NPCPlayerApex, generated_51.generated_10> generated_104 =
            new Dictionary<NPCPlayerApex, generated_51.generated_10>();

        private readonly Dictionary<NPCPlayerApex, generated_79> generated_81 =
            new Dictionary<NPCPlayerApex, generated_79>();

        private IEnumerator generated_203;

        private void generated_204()
        {
            generated_203 = generated_205();
            ServerMgr.Instance.StartCoroutine(generated_203);
        }

        private IEnumerator generated_205()
        {
            var generated_206 = generated_207();
            while (generated_206.MoveNext())
            {
                yield return generated_206.Current;
            }
        }

        private void generated_208()
        {
            ServerMgr.Instance.StopCoroutine(generated_203);
            foreach (var generated_209 in generated_38)
            {
                UnityEngine.Object.Destroy(generated_209.gameObject.GetComponent<generated_79>());
                generated_209.Kill();
            }
            generated_38.Clear();
            generated_202.Clear();
            generated_104.Clear();
            generated_81.Clear();
            generated_201.Clear();
        }

        private void generated_210(NPCPlayerApex generated_211)
        {
            if (generated_38.Contains(generated_211))
            {
                UnityEngine.Object.Destroy(generated_211.gameObject.GetComponent<generated_79>());
                generated_38.Remove(generated_211);
                generated_51.generated_10 generated_212;
                if (generated_104.TryGetValue(generated_211, out generated_212))
                {
                    generated_104.Remove(generated_211);
                    var generated_213 = generated_202[generated_211];
                    generated_202.Remove(generated_211);
                    timer.Once(generated_212.generated_73,
                        () => { generated_214(generated_215 => generated_30(generated_212, generated_215, generated_213), generated_212, true, generated_213); });
                }
            }
        }

        private IEnumerator generated_216()
        {
            for (var generated_217 = 0; generated_217 < TerrainBiome.COUNT; generated_217++)
            {
                generated_201[(TerrainBiome.Enum)TerrainBiome.IndexToType(generated_217)] = new List<Vector3>();
            }

            var generated_218 = (int)(TerrainMeta.Size.x * TerrainMeta.Size.z);
            var generated_219 = 0;

            var generated_220 = (int)Mathf.Floor(TerrainMeta.Size.x / 2f);
            //            worldSize05 /=5;
            for (var generated_221 = -generated_220 + 1; generated_221 < generated_220; generated_221++)
            {
                if (generated_221 % 200 == 0)
                {
                    yield return CoroutineEx.waitForFixedUpdate;
                    yield return CoroutineEx.waitForFixedUpdate;
                    yield return CoroutineEx.waitForFixedUpdate;
                    yield return CoroutineEx.waitForFixedUpdate;
                }


                for (var generated_222 = -generated_220 + 1; generated_222 < generated_220; generated_222++)
                {
                    if (++generated_219 % (generated_218 / 20) == 0)
                    {
                        Puts($"Generating BiomeSpawns: {100f * generated_219 / generated_218}%");
                    }

                    for (var generated_217 = 0; generated_217 < TerrainBiome.COUNT; generated_217++)
                    {
                        var generated_223 = TerrainBiome.IndexToType(generated_217);
                        if (TerrainMeta.BiomeMap.GetBiome(TerrainMeta.NormalizeX(generated_222), TerrainMeta.NormalizeZ(generated_221), generated_223) >
                            0.5)
                        {
                            var generated_224 = Vector3.zero;
                            if (generated_225(new Vector3(generated_222, 0, generated_221), out generated_224))
                            {
                                generated_201[(TerrainBiome.Enum)generated_223].Add(generated_224);
                            }
                        }
                    }
                }
            }
        }

        private void generated_214(Action<Vector3> generated_226, generated_51.generated_10 generated_227, bool generated_228 = false,
            TerrainBiome.Enum generated_229 = TerrainBiome.Enum.Temperate)
        {
            var generated_230 = TerrainMeta.Size.x / 2;
            var generated_231 = new Vector3(Random.Range(-generated_230, generated_230), 200, Random.Range(-generated_230, generated_230));

            var generated_232 = TerrainMeta.HeightMap.GetHeight(generated_231) - TerrainMeta.WaterMap.GetHeight(generated_231);
            if (generated_232 > 0.2f)
            {
                if (!generated_228 || TerrainMeta.BiomeMap.GetBiome(generated_231.x, generated_231.z, (int)generated_229) > 0.5f)
                {
                    if (generated_227.generated_66.All(generated_233 =>
                            TerrainMeta.TopologyMap.GetTopology(generated_231, (int)generated_233) == false) &&
                        generated_227.generated_68.Any(generated_233 => TerrainMeta.TopologyMap.GetTopology(generated_231, (int)generated_233)))
                    {
                        Vector3 generated_234;
                        if (generated_225(generated_231, out generated_234))
                        {
                            var generated_235 = Pool.GetList<BuildingBlock>();
                            Vis.Entities(generated_234, 10, generated_235, LayerMask.GetMask("Construction"));
                            if (generated_235.Count == 0)
                            {
                                if (false == generated_236(generated_234, generated_12.generated_53, "World", "Terrain"))
                                {
                                    generated_226(generated_234);
                                    return;
                                }
                            }

                            Pool.FreeList(ref generated_235);
                        }
                    }
                }
            }

            timer.Once(0.05f, () => generated_214(generated_226, generated_227, generated_228, generated_229));
        }


        private bool generated_236(Vector3 generated_237, generated_51.generated_52 generated_238, params string[] generated_239)
        {
            var generated_240 = generated_237 + new Vector3(0, generated_238.generated_54, 0);
            var generated_241 = new Vector3(generated_238.generated_57, generated_238.generated_55, generated_238.generated_56);
            var generated_242 = new Bounds(generated_240, generated_241);
            return GamePhysics.CheckBounds(generated_242, LayerMask.GetMask(generated_239));
        }

        private IEnumerator generated_207()
        {
            var generated_243 = 0;
            foreach (generated_10 generated_244 in generated_12.generated_13)
                foreach (var generated_245 in generated_244.generated_72)
                {
                    var generated_246 = generated_245.Key;
                    var generated_247 = generated_245.Value;

                    for (var generated_248 = 0; generated_248 < generated_247; generated_248++)
                    {
                        generated_243++;
                        if (generated_243 % 10 == 0)
                        {
                            yield return CoroutineEx.waitForFixedUpdate;
                        }

                        generated_214(generated_249 => generated_30(generated_244, generated_249, generated_246), generated_244, true, generated_246);
                    }
                }
        }

        private bool generated_225(Vector3 generated_250, out Vector3 generated_251)
        {
            RaycastHit generated_252;
            if (Physics.Raycast(generated_250 + new Vector3(0, 200, 0), Vector3.down, out generated_252, 1000, generated_29))
            {
                generated_251 = generated_252.point;

                if (WaterLevel.Test(generated_252.point))
                {
                    return false;
                }

                return true;
            }

            generated_251 = generated_250;
            return false;
        }

        /// <summary>
        ///     Generate npc population image
        /// </summary>
        /// <param name="generated_253"></param>
        /// <returns>PNG Bytes</returns>
        private byte[] generated_7(int generated_253 = 16)
        {
            var generated_254 = MapGenerator.GetMapImage(4096);
            var generated_255 = (Bitmap)new ImageConverter().ConvertFrom(generated_254);


            var generated_256 = new Bitmap(generated_255.Width, generated_255.Height);
            var generated_257 = Graphics.FromImage(generated_256);
            generated_257.DrawImage(generated_255, new Rectangle(0, 0, generated_255.Width, generated_255.Height), 0, 0, generated_255.Width,
                generated_255.Height, GraphicsUnit.Pixel);

            var generated_258 = new Pen(Color.Blue);
            foreach (var generated_259 in generated_104)
            {
                var generated_260 = generated_261(generated_255, generated_259.Key.ServerPosition);
                generated_258.Color = ColorTranslator.FromHtml(generated_259.Value.generated_60);
                var generated_262 = generated_253 / 2;
                generated_257.FillRectangle(generated_258.Brush, generated_260.x - generated_262, generated_260.y - generated_262, generated_262, generated_262);
            }

            generated_257.Dispose();
            generated_258.Dispose();
            var generated_263 = Pool.Get<MemoryStream>();
            generated_256.Save(generated_263, ImageFormat.Png);
            var generated_264 = generated_263.ToArray();
            Pool.FreeMemoryStream(ref generated_263);
            return generated_264;
        }

        private Vector2 generated_261(Bitmap generated_265, Vector3 generated_266)
        {
            var generated_267 = generated_268(generated_266);
            return generated_269(generated_265, generated_267);
        }

        private Vector2i generated_269(Bitmap generated_270, Vector3 generated_271) =>
            new Vector2i((int)(generated_271.x * generated_270.Width),
                generated_270.Height - (int)(generated_271.y * generated_270.Height));

        private Vector2 generated_268(Vector3 generated_272)
        {
            var generated_273 = (generated_272.x + (int)World.Size * 0.5f) / (int)World.Size;
            var generated_274 = (generated_272.z + (int)World.Size * 0.5f) / (int)World.Size;
            return new Vector2(generated_273, generated_274);
        }

        private void generated_30(generated_51.generated_10 generated_275, Vector3 generated_276, TerrainBiome.Enum generated_277)
        {
            var generated_278 = GameManager.server.FindPrefab(generated_275.generated_69
                ? "assets/prefabs/npc/scientist/scientist.prefab"
                : "assets/prefabs/npc/murderer/murderer.prefab");
            var generated_279 = UnityEngine.Object.Instantiate(generated_278, generated_276, Quaternion.identity);
            generated_279.name = generated_278.name;
            SceneManager.MoveGameObjectToScene(generated_279, Rust.Server.EntityScene);
            if (generated_279.GetComponent<Spawnable>())
            {
                UnityEngine.Object.Destroy(generated_279.GetComponent<Spawnable>());
            }

            if (!generated_279.activeSelf)
            {
                generated_279.SetActive(true);
            }

            var generated_280 = generated_279.GetComponent<BaseEntity>();

            var generated_281 = generated_280 as NPCPlayerApex;
            if (generated_281 == null)
            {
                return;
            }

            generated_281.transform.position = generated_276;


            generated_281.displayName = generated_275.generated_15;
            generated_281.Spawn();

            // Remove radio chat effect
            generated_281.CancelInvoke(generated_281.RadioChatter);

            generated_281.InitializeHealth(generated_275.generated_67, generated_275.generated_67);
            generated_281.CommunicationRadius = 0;
            generated_281.clothingMoveSpeedReduction = 1 - generated_275.generated_70;
            generated_281.SetPlayerFlag(BasePlayer.PlayerFlags.NoSprint, generated_275.generated_71);
            generated_281.Stats.AggressionRange = generated_281.Stats.DeaggroRange = generated_275.generated_59;
            generated_281.Stats.CloseRange = 1;
            generated_281.StoppingDistance = 0;
            generated_281.inventory.containerWear.Clear();
            generated_281.inventory.containerWear.capacity = 10;
            generated_275.generated_24.ForEach(generated_282 => { generated_282.generated_167().MoveToContainer(generated_281.inventory.containerWear); });

            generated_281.inventory.containerBelt.Clear();
            generated_275.generated_26.ForEach(generated_282 => { generated_282.generated_167().MoveToContainer(generated_281.inventory.containerBelt); });

            generated_281.inventory.containerMain.Clear();
            generated_275.generated_25.ForEach(generated_282 => { generated_282.generated_167().MoveToContainer(generated_281.inventory.containerMain); });
            generated_281.clothingMoveSpeedReduction = 1 - generated_275.generated_70;
            generated_281.svActiveItemID = 0;
            generated_281.CommunicationRadius = 0;

            generated_79 generated_283 = generated_281.gameObject.AddComponent<generated_79>();
            generated_283.generated_190 = generated_275;

            generated_38.Add(generated_281);
            generated_104[generated_281] = generated_275;
            generated_202[generated_281] = generated_277;
            generated_81[generated_281] = generated_283;
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
        void generated_284(NPCPlayerApex generated_285, NPCPlayerApex.Facts generated_286, byte generated_287, byte generated_288)
        {
            generated_39(generated_285, generated_286, generated_287, generated_288);
        }

        [Oxide.Core.Plugins.HookMethod("OnNpcTarget")]
        object generated_289(BaseNpc generated_290, BaseEntity generated_291)
        {
            object generated_292 = null;
            object generated_293 = null;
            generated_293 = generated_77(generated_290, generated_291);
            if (generated_293 != null)
                generated_292 = generated_293;
            return generated_292;
        }

        [Oxide.Core.Plugins.HookMethod("CanBeTargeted")]
        object generated_294(BaseCombatEntity generated_295, AutoTurret generated_296)
        {
            object generated_297 = null;
            object generated_298 = null;
            generated_298 = generated_83(generated_295, generated_296);
            if (generated_298 != null)
                generated_297 = generated_298;
            return generated_297;
        }

        [Oxide.Core.Plugins.HookMethod("OnNpcPlayerTarget")]
        object generated_299(NPCPlayerApex generated_300, BaseEntity generated_301)
        {
            object generated_302 = null;
            object generated_303 = null;
            generated_303 = generated_88(generated_300, generated_301);
            if (generated_303 != null)
                generated_302 = generated_303;
            return generated_302;
        }

        [Oxide.Core.Plugins.HookMethod("OnPlayerAttack")]
        void generated_304(BasePlayer generated_305, HitInfo generated_306)
        {
            generated_94(generated_305, generated_306);
        }

        [Oxide.Core.Plugins.HookMethod("OnEntityTakeDamage")]
        void generated_307(BaseCombatEntity generated_308, HitInfo generated_309)
        {
            generated_98(generated_308, generated_309);
        }

        [Oxide.Core.Plugins.HookMethod("OnCreateCorpse")]
        object generated_310(NPCPlayerApex generated_311, BaseCorpse generated_312)
        {
            object generated_313 = null;
            object generated_314 = null;
            generated_314 = generated_106(generated_311, generated_312);
            if (generated_314 != null)
                generated_313 = generated_314;
            return generated_313;
        }

        [Oxide.Core.Plugins.HookMethod("Loaded")]
        void generated_315()
        {
            generated_118();
        }

        [Oxide.Core.Plugins.HookMethod("OnServerInitialized")]
        void generated_316()
        {
            generated_204();
        }

        [Oxide.Core.Plugins.HookMethod("Unload")]
        void generated_317()
        {
            generated_208();
        }

        [Oxide.Core.Plugins.HookMethod("OnEntityKill")]
        void generated_318(NPCPlayerApex generated_319)
        {
            generated_210(generated_319);
        }
        #endregion
    }
}