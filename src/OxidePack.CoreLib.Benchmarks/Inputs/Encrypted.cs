using System.Collections.Generic;
using System.Linq;
using Oxide.Game.Rust.Cui;
using UnityEngine;
using System;

namespace Oxide.Plugins
{
    [Info("FurnaceSorter", "", "1.0.8", ResourceId = 23)]
    class FurnaceSorter : RustPlugin
    {
        bool debug;
        private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();
        private Dictionary<ulong, info> UIInfo = new Dictionary<ulong, info>();
        class info
        {
            public BaseOven oven = null;
        }

        void Unload()
        {
            generated_28 generated_239 = generated_25(this);
            generated_239.generated_27();
            generated_29(generated_239);
        }

        void OnPlayerInit(BasePlayer player)
        {
            generated_33 generated_240 = generated_30(this, player);
            generated_240.generated_32();
            generated_34(generated_240);
        }

        void DestroyPlayer(BasePlayer player)
        {
            generated_38 generated_241 = generated_35(this, player);
            generated_241.generated_37();
            generated_39(generated_241);
        }

        void OnPlayerDisconnected(BasePlayer player)
        {
            generated_43 generated_242 = generated_40(this, player);
            generated_242.generated_42();
            generated_44(generated_242);
        }

        void OnPlayerRespawned(BasePlayer player)
        {
            generated_48 generated_243 = generated_45(this, player);
            generated_243.generated_47();
            generated_49(generated_243);
        }

        void Loaded()
        {
            generated_53 generated_244 = generated_50(this);
            generated_244.generated_52();
            generated_54(generated_244);
        }

        void OnServerInitialized()
        {
            generated_58 generated_245 = generated_55(this);
            generated_245.generated_57();
            generated_59(generated_245);
        }

        [ChatCommand("sorter")]
        private void chatSorter(BasePlayer player, string command, string[] args)
        {
            generated_63 generated_246 = generated_60(this, player, command, args);
            generated_246.generated_62();
            generated_64(generated_246);
        }

        private void GetSendMSG(BasePlayer player, string message, string arg1 = "", string arg2 = "", string arg3 = "")
        {
            generated_68 generated_247 = generated_65(this, player, message, arg1, arg2, arg3);
            generated_247.generated_67();
            generated_69(generated_247);
        }

        private string GetMSG(string message, BasePlayer player = null, string arg1 = "", string arg2 = "", string arg3 = "")
        {
            generated_73 generated_248 = generated_70(this, message, player, arg1, arg2, arg3);
            try
            {
                return generated_248.generated_72();
            }
            catch (System.Exception _exception)
            {
                global::Oxide.Core.Interface.Oxide.LogError(_exception.Message + "\n" + _exception.StackTrace);
                throw;
            }
            finally
            {
                generated_74(generated_248);
            }
        }

        private string PanelSorter = "PanelSorter";
        private string PanelOnScreen = "PanelOnScreen";
        public class UI
        {
            static public CuiElementContainer CreateOverlayContainer(string panelName, string color, string aMin, string aMax, bool cursor = false)
            {
                generated_78 generated_249 = generated_75(panelName, color, aMin, aMax, cursor);
                try
                {
                    return generated_249.generated_77();
                }
                catch (System.Exception _exception)
                {
                    global::Oxide.Core.Interface.Oxide.LogError(_exception.Message + "\n" + _exception.StackTrace);
                    throw;
                }
                finally
                {
                    generated_79(generated_249);
                }
            }

            static public void CreateButton(ref CuiElementContainer container, string panel, string color, string text, int size, string aMin, string aMax, string command, TextAnchor align = TextAnchor.MiddleCenter)
            {
                generated_83 generated_250 = generated_80(ref container, panel, color, text, size, aMin, aMax, command, align);
                generated_250.generated_82();
                generated_84(generated_250);
            }

            static public void CreateTextOutline(ref CuiElementContainer element, string panel, string colorText, string colorOutline, string text, int size, string aMin, string aMax, TextAnchor align = TextAnchor.MiddleCenter)
            {
                generated_88 generated_251 = generated_85(ref element, panel, colorText, colorOutline, text, size, aMin, aMax, align);
                generated_251.generated_87();
                generated_89(generated_251);
            }
        }

        void OnLootEntity(BasePlayer player, BaseEntity entity)
        {
            generated_93 generated_252 = generated_90(this, player, entity);
            generated_252.generated_92();
            generated_94(generated_252);
        }

        private void OnPlayerLootEnd(PlayerLoot looter)
        {
            generated_98 generated_253 = generated_95(this, looter);
            generated_253.generated_97();
            generated_99(generated_253);
        }

        void SorterUI(BasePlayer player)
        {
            generated_103 generated_254 = generated_100(this, player);
            generated_254.generated_102();
            generated_104(generated_254);
        }

        [ConsoleCommand("UI_ToggleSorter")]
        void cmdUI_ToggleSorter(ConsoleSystem.Arg arg)
        {
            generated_108 generated_255 = generated_105(this, arg);
            generated_255.generated_107();
            generated_109(generated_255);
        }

        void OnScreen(BasePlayer player, string msg)
        {
            generated_113 generated_256 = generated_110(this, player, msg);
            generated_256.generated_112();
            generated_114(generated_256);
        }

        public List<ulong> Enabled = new List<ulong>();
        public List<ItemContainer> Sorting = new List<ItemContainer>();
        object CanAcceptItem(ItemContainer container, Item item)
        {
            generated_118 generated_257 = generated_115(this, container, item);
            try
            {
                return generated_257.generated_117();
            }
            catch (System.Exception _exception)
            {
                global::Oxide.Core.Interface.Oxide.LogError(_exception.Message + "\n" + _exception.StackTrace);
                throw;
            }
            finally
            {
                generated_119(generated_257);
            }
        }

        void SortFurnace(BasePlayer player, ItemContainer container, Item OriginalItem, int StackAmount, List<Item> ExistingItems, string shortname, int Remainder, int NewSlots)
        {
            generated_123 generated_258 = generated_120(this, player, container, OriginalItem, StackAmount, ExistingItems, shortname, Remainder, NewSlots);
            generated_258.generated_122();
            generated_124(generated_258);
        }

        Dictionary<int, double> WoodRatios = new Dictionary<int, double> { { -1059362949, 5 }, { 889398893, 2.5 }, { 2133577942, 10 }, { 1325935999, 3 }, { -642008142, 3 }, { -253819519, 3 }, { 179448791, 3 }, { -1658459025, 3 }, { -533484654, 3 }, { 2080339268, 3 }, { 1050986417, 3 }, { 1983936587, 6.66 }, };
        float Default_minx = 0.646f;
        float Default_miny = 0.1f;
        float Default_maxx = 0.81f;
        float Default_maxy = 0.14f;
        private ConfigData configData;
        class ConfigData
        {
            public float minx
            {
                get;
                set;
            }

            public float miny
            {
                get;
                set;
            }

            public float maxx
            {
                get;
                set;
            }

            public float maxy
            {
                get;
                set;
            }
        }

        private void LoadVariables()
        {
            generated_128 generated_259 = generated_125(this);
            generated_259.generated_127();
            generated_129(generated_259);
        }

        protected override void LoadDefaultConfig()
        {
            generated_133 generated_260 = generated_130(this);
            generated_260.generated_132();
            generated_134(generated_260);
        }

        private void LoadConfigVariables()
        {
            generated_138 generated_261 = generated_135(this);
            generated_261.generated_137();
            generated_139(generated_261);
        }

        void SaveConfig(ConfigData config)
        {
            generated_143 generated_262 = generated_140(this, config);
            generated_262.generated_142();
            generated_144(generated_262);
        }

        Dictionary<string, string> messages = new Dictionary<string, string>()
        {{"title", "Сортировка печей: "}, {"NoPerm", "У вас нет прав, чтобы использовать эту команду"}, {"FurnaceSorterDisabled", "Сортировка печей выключена."}, {"FurnaceOnSorterDisabled", "Сортировка не может использоваться, когда печь включена."}, {"FurnaceSorterEnabled", "Сортировка печей включена."}, {"ToggleOff", "СОРТИРОВКА Выключить"}, {"ToggleOn", "СОРТИРОВКА Включить"}, {"OptimizationUnavailable", "Вы не можете использовать Оптимизацию, пока печь включена!"}, {"NothingToOptimize", "Оптимизировать в печи нечего. Ошибка при оптимизации!"}, {"NoWood", "В печи нет дерева. Ошибка при оптимизации!"}, {"NoAcceptableItems", "В печи нет каких-либо действительных ресурсов для оптимизации дерева. Ошибка при оптимизации!"}, {"WoodRatioGood", "Допустимое соотношение древесины правильное. Оптимизация не требуется!"}, {"WoodNeeded", "Оптимизация показала, что у вас не хватает <color=\"#ffd479\">{0}</color> дерева"}, {"ExtraWoodGiven", "Оптимизация показала, что у вас есть <color=\"#ffd479\">{0}</color> лишней древесины. Это дерево было помещено в ваш инвентарь!"}, {"InventoryFull", "У вас недостаточно места в инвентаре, чтобы получить лишнию древесину. Ошибка при оптимизации!"}, {"FurnaceOptimized", "Ваша печь оптимизирована!"}, };
        class generated_28
        {
            public FurnaceSorter generated_0;
            public void generated_26(FurnaceSorter generated_0)
            {
                this.generated_0 = generated_0;
            }

            public object generated_145()
            {
                generated_0.timers.Clear();
                return null;
            }

            public object generated_146()
            {
                foreach (var entry in generated_0.timers)
                    entry.Value.Destroy();
                return null;
            }

            public object generated_147()
            {
                foreach (var player in BasePlayer.activePlayerList)
                    generated_0.DestroyPlayer(player);
                return null;
            }

            public void generated_27()
            {
                generated_147();
                generated_146();
                generated_145();
            }
        }

        class generated_33
        {
            public FurnaceSorter generated_1;
            public BasePlayer player;
            public void generated_31(FurnaceSorter generated_1, BasePlayer player)
            {
                this.generated_1 = generated_1;
                this.player = player;
            }

            public object generated_148()
            {
                if (!generated_1.Enabled.Contains(player.userID))
                    generated_1.Enabled.Add(player.userID);
                return null;
            }

            public void generated_32()
            {
                generated_148();
            }
        }

        class generated_38
        {
            public FurnaceSorter generated_2;
            public BasePlayer player;
            public void generated_36(FurnaceSorter generated_2, BasePlayer player)
            {
                this.generated_2 = generated_2;
                this.player = player;
            }

            public object generated_149()
            {
                CuiHelper.DestroyUi(player, generated_2.PanelSorter);
                return null;
            }

            public object generated_150()
            {
                CuiHelper.DestroyUi(player, generated_2.PanelOnScreen);
                return null;
            }

            public object generated_151()
            {
                if (generated_2.Enabled.Contains(player.userID))
                    generated_2.Enabled.Remove(player.userID);
                return null;
            }

            public object generated_152()
            {
                if (generated_2.timers.ContainsKey(player.userID.ToString()))
                {
                    generated_2.timers[player.userID.ToString()].Destroy();
                    generated_2.timers.Remove(player.userID.ToString());
                }

                return null;
            }

            public object generated_153()
            {
                if (generated_2.UIInfo.ContainsKey(player.userID))
                    generated_2.UIInfo.Remove(player.userID);
                return null;
            }

            public object generated_154()
            {
                if (player == null)
                    return true;
                return null;
            }

            public void generated_37()
            {
                if (generated_154() is generated_24)
                    return;
                generated_153();
                generated_152();
                generated_151();
                generated_150();
                generated_149();
            }
        }

        class generated_43
        {
            public FurnaceSorter generated_3;
            public BasePlayer player;
            public void generated_41(FurnaceSorter generated_3, BasePlayer player)
            {
                this.generated_3 = generated_3;
                this.player = player;
            }

            public object generated_155()
            {
                generated_3.DestroyPlayer(player);
                return null;
            }

            public void generated_42()
            {
                generated_155();
            }
        }

        class generated_48
        {
            public FurnaceSorter generated_4;
            public BasePlayer player;
            public void generated_46(FurnaceSorter generated_4, BasePlayer player)
            {
                this.generated_4 = generated_4;
                this.player = player;
            }

            public object generated_156()
            {
                CuiHelper.DestroyUi(player, generated_4.PanelSorter);
                return null;
            }

            public object generated_157()
            {
                CuiHelper.DestroyUi(player, generated_4.PanelOnScreen);
                return null;
            }

            public void generated_47()
            {
                generated_157();
                generated_156();
            }
        }

        class generated_53
        {
            public FurnaceSorter generated_5;
            public void generated_51(FurnaceSorter generated_5)
            {
                this.generated_5 = generated_5;
            }

            public object generated_158()
            {
                generated_5.lang.RegisterMessages(generated_5.messages, generated_5);
                return null;
            }

            public void generated_52()
            {
                generated_158();
            }
        }

        class generated_58
        {
            public FurnaceSorter generated_6;
            public void generated_56(FurnaceSorter generated_6)
            {
                this.generated_6 = generated_6;
            }

            public object generated_159()
            {
                generated_6.debug = false;
                return null;
            }

            public object generated_160()
            {
                generated_6.permission.RegisterPermission(generated_6.Title + ".allow", generated_6);
                return null;
            }

            public object generated_161()
            {
                generated_6.LoadVariables();
                return null;
            }

            public void generated_57()
            {
                generated_161();
                generated_160();
                generated_159();
            }
        }

        class generated_63
        {
            public FurnaceSorter generated_7;
            public BasePlayer player;
            public string command;
            public string[] args;
            public void generated_61(FurnaceSorter generated_7, BasePlayer player, string command, string[] args)
            {
                this.generated_7 = generated_7;
                this.player = player;
                this.command = command;
                this.args = args;
            }

            public object generated_162()
            {
                if (player.net.connection.authLevel > 1 && args != null && args.Count() > 0 && args[0] == "debug")
                    if (generated_7.debug)
                        generated_7.debug = false;
                    else
                        generated_7.debug = true;
                return null;
            }

            public object generated_163()
            {
                if (player == null)
                    return true;
                return null;
            }

            public void generated_62()
            {
                if (generated_163() is generated_24)
                    return;
                generated_162();
            }
        }

        class generated_68
        {
            public FurnaceSorter generated_8;
            public BasePlayer player;
            public string message;
            public string arg1;
            public string arg2;
            public string arg3;
            public string msg;
            public void generated_66(FurnaceSorter generated_8, BasePlayer player, string message, string arg1 = "", string arg2 = "", string arg3 = "")
            {
                this.generated_8 = generated_8;
                this.player = player;
                this.message = message;
                this.arg1 = arg1;
                this.arg2 = arg2;
                this.arg3 = arg3;
            }

            public object generated_164()
            {
                generated_8.SendReply(player, "<color=orange>" + generated_8.lang.GetMessage("title", generated_8, player.UserIDString) + "</color>" + "<color=#A9A9A9>" + msg + "</color>");
                return null;
            }

            public object generated_165()
            {
                msg = string.Format(generated_8.lang.GetMessage(message, generated_8, player.UserIDString), arg1, arg2, arg3);
                return null;
            }

            public void generated_67()
            {
                generated_165();
                generated_164();
            }
        }

        class generated_73
        {
            public FurnaceSorter generated_9;
            public string message;
            public BasePlayer player;
            public string arg1;
            public string arg2;
            public string arg3;
            public string p;
            public void generated_71(FurnaceSorter generated_9, string message, BasePlayer player = null, string arg1 = "", string arg2 = "", string arg3 = "")
            {
                this.generated_9 = generated_9;
                this.message = message;
                this.player = player;
                this.arg1 = arg1;
                this.arg2 = arg2;
                this.arg3 = arg3;
            }

            public object generated_166()
            {
                if (generated_9.messages.ContainsKey(message))
                    return string.Format(generated_9.lang.GetMessage(message, generated_9, p), arg1, arg2, arg3);
                else
                    return message;
                return null;
            }

            public object generated_167;
            public object generated_168()
            {
                if (player != null)
                    p = player.UserIDString;
                return null;
            }

            public object generated_169()
            {
                p = null;
                return null;
            }

            public string generated_72()
            {
                generated_169();
                generated_168();
                generated_167 = generated_166();
                if (generated_167 is generated_24)
                    return default(string);
                return (string)generated_167;
            }
        }

        class generated_78
        {
            public string panelName;
            public string color;
            public string aMin;
            public string aMax;
            public bool cursor;
            public Oxide.Game.Rust.Cui.CuiElementContainer NewElement;
            public void generated_76(string panelName, string color, string aMin, string aMax, bool cursor = false)
            {
                this.panelName = panelName;
                this.color = color;
                this.aMin = aMin;
                this.aMax = aMax;
                this.cursor = cursor;
            }

            public object generated_170()
            {
                return NewElement;
            }

            public object generated_171;
            public object generated_172()
            {
                NewElement = new CuiElementContainer()
                {{new CuiPanel{Image = {Color = color}, RectTransform = {AnchorMin = aMin, AnchorMax = aMax}, CursorEnabled = cursor}, new CuiElement().Parent = "Overlay", panelName}};
                return null;
            }

            public CuiElementContainer generated_77()
            {
                generated_172();
                generated_171 = generated_170();
                if (generated_171 is generated_24)
                    return default(CuiElementContainer);
                return (CuiElementContainer)generated_171;
            }
        }

        class generated_83
        {
            public CuiElementContainer container;
            public string panel;
            public string color;
            public string text;
            public int size;
            public string aMin;
            public string aMax;
            public string command;
            public TextAnchor align;
            public void generated_81(ref CuiElementContainer container, string panel, string color, string text, int size, string aMin, string aMax, string command, TextAnchor align = TextAnchor.MiddleCenter)
            {
                this.container = container;
                this.panel = panel;
                this.color = color;
                this.text = text;
                this.size = size;
                this.aMin = aMin;
                this.aMax = aMax;
                this.command = command;
                this.align = align;
            }

            public object generated_173()
            {
                container.Add(new CuiButton { Button = { Color = color, Command = command, FadeIn = 1.0f }, RectTransform = { AnchorMin = aMin, AnchorMax = aMax }, Text = { Text = text, FontSize = size, Align = align } }, panel);
                return null;
            }

            public void generated_82()
            {
                generated_173();
            }
        }

        class generated_88
        {
            public CuiElementContainer element;
            public string panel;
            public string colorText;
            public string colorOutline;
            public string text;
            public int size;
            public string aMin;
            public string aMax;
            public TextAnchor align;
            public void generated_86(ref CuiElementContainer element, string panel, string colorText, string colorOutline, string text, int size, string aMin, string aMax, TextAnchor align = TextAnchor.MiddleCenter)
            {
                this.element = element;
                this.panel = panel;
                this.colorText = colorText;
                this.colorOutline = colorOutline;
                this.text = text;
                this.size = size;
                this.aMin = aMin;
                this.aMax = aMax;
                this.align = align;
            }

            public object generated_174()
            {
                element.Add(new CuiElement { Parent = panel, Components = { new CuiTextComponent { Color = colorText, FontSize = size, Align = align, Text = text }, new CuiOutlineComponent { Distance = "1 1", Color = colorOutline }, new CuiRectTransformComponent { AnchorMax = aMax, AnchorMin = aMin } } });
                return null;
            }

            public void generated_87()
            {
                generated_174();
            }
        }

        class generated_93
        {
            public FurnaceSorter generated_13;
            public BasePlayer player;
            public BaseEntity entity;
            public void generated_91(FurnaceSorter generated_13, BasePlayer player, BaseEntity entity)
            {
                this.generated_13 = generated_13;
                this.player = player;
                this.entity = entity;
            }

            public object generated_175()
            {
                if (entity as BaseOven != null && generated_13.permission.UserHasPermission(player.UserIDString, generated_13.Name + ".allow"))
                {
                    if (!generated_13.UIInfo.ContainsKey(player.userID))
                        generated_13.UIInfo.Add(player.userID, new info());
                    generated_13.UIInfo[player.userID].oven = (entity as BaseOven);
                    generated_13.SorterUI(player);
                }

                return null;
            }

            public object generated_176()
            {
                if (player == null || entity == null)
                    return true;
                return null;
            }

            public void generated_92()
            {
                if (generated_176() is generated_24)
                    return;
                generated_175();
            }
        }

        class generated_98
        {
            public FurnaceSorter generated_14;
            public PlayerLoot looter;
            public BasePlayer player;
            public void generated_96(FurnaceSorter generated_14, PlayerLoot looter)
            {
                this.generated_14 = generated_14;
                this.looter = looter;
            }

            public object generated_177()
            {
                if (looter != null && looter.entitySource != null && looter.entitySource is BaseOven)
                {
                    player = looter.GetComponent<BasePlayer>();
                    if (player == null)
                        return true;
                    if (generated_14.UIInfo.ContainsKey(player.userID))
                        generated_14.UIInfo[player.userID].oven = null;
                    CuiHelper.DestroyUi(player, generated_14.PanelSorter);
                }

                return null;
            }

            public void generated_97()
            {
                if (generated_177() is generated_24)
                    return;
            }
        }

        class generated_103
        {
            public FurnaceSorter generated_15;
            public BasePlayer player;
            public Oxide.Game.Rust.Cui.CuiElementContainer element;
            public void generated_101(FurnaceSorter generated_15, BasePlayer player)
            {
                this.generated_15 = generated_15;
                this.player = player;
            }

            public object generated_178()
            {
                CuiHelper.AddUi(player, element);
                return null;
            }

            public object generated_179()
            {
                if (generated_15.Enabled.Contains(player.userID))
                    UI.CreateButton(ref element, generated_15.PanelSorter, "0.584 0.29 0.211 1.0", generated_15.GetMSG("<size=11>СОРТИРОВКА</size> Выключить", player), 12, "1.83 2.7", "2.14 4.03", $"UI_ToggleSorter {3}");
                else
                    UI.CreateButton(ref element, generated_15.PanelSorter, "0.439 0.509 0.294 1.0", generated_15.GetMSG("<size=11>СОРТИРОВКА</size> Включить", player), 12, "1.83 2.7", "2.14 4.03", $"UI_ToggleSorter {3}");
                return null;
            }

            public object generated_180()
            {
                element = UI.CreateOverlayContainer(generated_15.PanelSorter, "0 0 0 0", $"{generated_15.configData.minx} {generated_15.configData.miny}", $"{generated_15.configData.maxx} {generated_15.configData.maxy}");
                return null;
            }

            public object generated_181()
            {
                CuiHelper.DestroyUi(player, generated_15.PanelSorter);
                return null;
            }

            public void generated_102()
            {
                generated_181();
                generated_180();
                generated_179();
                generated_178();
            }
        }

        class generated_108
        {
            public FurnaceSorter generated_16;
            public ConsoleSystem.Arg arg;
            public BasePlayer player;
            public void generated_106(FurnaceSorter generated_16, ConsoleSystem.Arg arg)
            {
                this.generated_16 = generated_16;
                this.arg = arg;
            }

            public object generated_182()
            {
                if (generated_16.Enabled.Contains(player.userID))
                {
                    generated_16.Enabled.Remove(player.userID);
                    generated_16.OnScreen(player, "FurnaceSorterDisabled");
                    generated_16.SorterUI(player);
                }
                else
                {
                    generated_16.Enabled.Add(player.userID);
                    generated_16.OnScreen(player, "FurnaceSorterEnabled");
                    generated_16.SorterUI(player);
                }

                return null;
            }

            public object generated_183()
            {
                if (!generated_16.permission.UserHasPermission(player.UserIDString, generated_16.Name + ".allow"))
                    return true;
                return null;
            }

            public object generated_184()
            {
                if (player == null)
                    return true;
                return null;
            }

            public object generated_185()
            {
                player = arg.Connection.player as BasePlayer;
                return null;
            }

            public void generated_107()
            {
                generated_185();
                if (generated_184() is generated_24)
                    return;
                if (generated_183() is generated_24)
                    return;
                generated_182();
            }
        }

        class generated_113
        {
            public FurnaceSorter generated_17;
            public BasePlayer player;
            public string msg;
            public Oxide.Game.Rust.Cui.CuiElementContainer element;
            public void generated_111(FurnaceSorter generated_17, BasePlayer player, string msg)
            {
                this.generated_17 = generated_17;
                this.player = player;
                this.msg = msg;
            }

            public object generated_186()
            {
                generated_17.timers.Add(player.userID.ToString(), generated_17.timer.Once(4, () => CuiHelper.DestroyUi(player, generated_17.PanelOnScreen)));
                return null;
            }

            public object generated_187()
            {
                CuiHelper.AddUi(player, element);
                return null;
            }

            public object generated_188()
            {
                UI.CreateTextOutline(ref element, generated_17.PanelOnScreen, string.Empty, "0 0 0 1", generated_17.GetMSG(msg, player), 32, "0.0 0.0", "1.0 1.0");
                return null;
            }

            public object generated_189()
            {
                element = UI.CreateOverlayContainer(generated_17.PanelOnScreen, "0.0 0.0 0.0 0.0", "0.3 0.5", "0.7 0.8");
                return null;
            }

            public object generated_190()
            {
                CuiHelper.DestroyUi(player, generated_17.PanelOnScreen);
                return null;
            }

            public object generated_191()
            {
                if (generated_17.timers.ContainsKey(player.userID.ToString()))
                {
                    generated_17.timers[player.userID.ToString()].Destroy();
                    generated_17.timers.Remove(player.userID.ToString());
                }

                return null;
            }

            public void generated_112()
            {
                generated_191();
                generated_190();
                generated_189();
                generated_188();
                generated_187();
                generated_186();
            }
        }

        class generated_118
        {
            public FurnaceSorter generated_18;
            public ItemContainer container;
            public Item item;
            public BasePlayer player;
            public List<string> AcceptableItems;
            public int TotalAmount;
            public List<Item> LessThenMax;
            public List<Item> Items;
            public int NewSlots;
            public int totalSlots;
            public int remainder;
            public int SplitableAmount;
            public int eachStack;
            public void generated_116(FurnaceSorter generated_18, ItemContainer container, Item item)
            {
                this.generated_18 = generated_18;
                this.container = container;
                this.item = item;
            }

            public object generated_192()
            {
                return ItemContainer.CanAcceptResult.CannotAccept;
            }

            public object generated_193;
            public object generated_194()
            {
                generated_18.SortFurnace(player, container, item, eachStack, Items, item.info.shortname, remainder, NewSlots);
                return null;
            }

            public object generated_195()
            {
                if (eachStack > item.MaxStackable())
                {
                    eachStack = item.MaxStackable();
                    remainder = TotalAmount - (eachStack * totalSlots);
                }

                return null;
            }

            public object generated_196()
            {
                if (generated_18.debug)
                    generated_18.Puts($"EachStack: {eachStack}");
                return null;
            }

            public object generated_197()
            {
                eachStack = SplitableAmount / totalSlots;
                return null;
            }

            public object generated_198()
            {
                if (generated_18.debug)
                    generated_18.Puts($"SplitAmount: {SplitableAmount}");
                return null;
            }

            public object generated_199()
            {
                SplitableAmount = TotalAmount - remainder;
                return null;
            }

            public object generated_200()
            {
                if (generated_18.debug)
                    generated_18.Puts($"Remainder: {remainder}");
                return null;
            }

            public object generated_201()
            {
                remainder = TotalAmount % totalSlots;
                return null;
            }

            public object generated_202()
            {
                if (totalSlots > TotalAmount)
                    totalSlots = TotalAmount;
                return null;
            }

            public object generated_203()
            {
                if (totalSlots == 0)
                    totalSlots += NewSlots;
                else
                    NewSlots = 0;
                return null;
            }

            public object generated_204()
            {
                totalSlots = Items.Count();
                return null;
            }

            public object generated_205()
            {
                if (generated_18.debug)
                    generated_18.Puts($"{NewSlots} - Available Slots");
                return null;
            }

            public object generated_206()
            {
                if (NewSlots > TotalAmount)
                    NewSlots = TotalAmount;
                return null;
            }

            public object generated_207()
            {
                if (NewSlots >= 2)
                {
                    if (container.entityOwner.GetComponent<BaseOven>().ShortPrefabName.Contains("refinery"))
                        NewSlots -= 1;
                    else
                        NewSlots -= 2;
                }

                return null;
            }

            public object generated_208()
            {
                if (NewSlots <= 2 && LessThenMax.Count == 0)
                    return ItemContainer.CanAcceptResult.CannotAccept;
                return null;
            }

            public object generated_209()
            {
                NewSlots = container.capacity - container.itemList.Count();
                return null;
            }

            public object generated_210()
            {
                foreach (var entry in container.itemList.Where(k => k.info.shortname == item.info.shortname))
                {
                    Items.Add(entry);
                    TotalAmount += entry.amount;
                    if (entry.amount < item.MaxStackable())
                        LessThenMax.Add(entry);
                    if (generated_18.debug)
                        generated_18.Puts($"TotalAmount = {TotalAmount}");
                }

                return null;
            }

            public object generated_211()
            {
                Items = new List<Item>();
                return null;
            }

            public object generated_212()
            {
                LessThenMax = new List<global::Item>();
                return null;
            }

            public object generated_213()
            {
                if (generated_18.debug)
                    generated_18.Puts($"TotalAmount = {TotalAmount}");
                return null;
            }

            public object generated_214()
            {
                TotalAmount = item.amount;
                return null;
            }

            public object generated_215()
            {
                if (container.entityOwner.GetComponent<BaseOven>().IsOn())
                {
                    generated_18.Enabled.Remove(player.userID);
                    generated_18.OnScreen(player, "FurnaceOnSorterDisabled");
                    generated_18.SorterUI(player);
                    return new generated_24();
                }

                return null;
            }

            public object generated_216()
            {
                if (!AcceptableItems.Contains(item.info.shortname))
                    return new generated_24();
                return null;
            }

            public object generated_217()
            {
                if (container.entityOwner.GetComponent<BaseOven>().ShortPrefabName.Contains("campfire"))
                    AcceptableItems = new List<string> { "bearmeat", "deermeat.raw", "humanmeat.raw", "meat.boar", "wolfmeat.raw", "can.beans.empty", "can.tuna.empty", "chicken.raw", "fish.raw" };
                else if (container.entityOwner.GetComponent<BaseOven>().ShortPrefabName.Contains("furnace"))
                    AcceptableItems = new List<string> { "hq.metal.ore", "metal.ore", "sulfur.ore", "can.beans.empty", "can.tuna.empty" };
                else if (container.entityOwner.GetComponent<BaseOven>().ShortPrefabName.Contains("refinery"))
                    AcceptableItems = new List<string> { "crude.oil" };
                return null;
            }

            public object generated_218()
            {
                AcceptableItems = new List<string>();
                return null;
            }

            public object generated_219()
            {
                if (player == null || !generated_18.Enabled.Contains(player.userID))
                    return new generated_24();
                return null;
            }

            public object generated_220()
            {
                player = item.GetOwnerPlayer();
                return null;
            }

            public object generated_221()
            {
                if (generated_18.debug)
                    generated_18.Puts("Item Being Accepted?");
                return null;
            }

            public object generated_222()
            {
                if (container == null || item == null || container.entityOwner == null || container.entityOwner.GetComponent<BaseOven>() == null || item.parent == container || generated_18.Sorting.Contains(container))
                    return new generated_24();
                return null;
            }

            public object generated_117()
            {
                generated_193 = generated_222();
                if (generated_193 is generated_24)
                    return default(object);
                if (generated_193 != null)
                    return generated_193;
                generated_221();
                generated_220();
                generated_193 = generated_219();
                if (generated_193 is generated_24)
                    return default(object);
                if (generated_193 != null)
                    return generated_193;
                generated_218();
                generated_217();
                generated_193 = generated_216();
                if (generated_193 is generated_24)
                    return default(object);
                if (generated_193 != null)
                    return generated_193;
                generated_193 = generated_215();
                if (generated_193 is generated_24)
                    return default(object);
                if (generated_193 != null)
                    return generated_193;
                generated_214();
                generated_213();
                generated_212();
                generated_211();
                generated_210();
                generated_209();
                generated_193 = generated_208();
                if (generated_193 is generated_24)
                    return default(object);
                if (generated_193 != null)
                    return generated_193;
                generated_207();
                generated_206();
                generated_205();
                generated_204();
                generated_203();
                generated_202();
                generated_201();
                generated_200();
                generated_199();
                generated_198();
                generated_197();
                generated_196();
                generated_195();
                generated_194();
                generated_193 = generated_192();
                if (generated_193 is generated_24)
                    return default(object);
                return generated_193;
            }
        }

        class generated_123
        {
            public FurnaceSorter generated_19;
            public BasePlayer player;
            public ItemContainer container;
            public Item OriginalItem;
            public int StackAmount;
            public List<Item> ExistingItems;
            public string shortname;
            public int Remainder;
            public int NewSlots;
            public ItemDefinition def;
            public Item newItem;
            public void generated_121(FurnaceSorter generated_19, BasePlayer player, ItemContainer container, Item OriginalItem, int StackAmount, List<Item> ExistingItems, string shortname, int Remainder, int NewSlots)
            {
                this.generated_19 = generated_19;
                this.player = player;
                this.container = container;
                this.OriginalItem = OriginalItem;
                this.StackAmount = StackAmount;
                this.ExistingItems = ExistingItems;
                this.shortname = shortname;
                this.Remainder = Remainder;
                this.NewSlots = NewSlots;
            }

            public object generated_223()
            {
                container.MarkDirty();
                return null;
            }

            public object generated_224()
            {
                generated_19.Sorting.Remove(container);
                return null;
            }

            public object generated_225()
            {
                if (Remainder > 0)
                {
                    foreach (var entry in container.itemList.Where(k => k.info.shortname == OriginalItem.info.shortname))
                    {
                        if (generated_19.debug)
                            generated_19.Puts($"Amount: {entry.amount} - Remainder Amount: {Remainder}");
                        if (entry.amount != entry.MaxStackable())
                        {
                            entry.amount++;
                            Remainder--;
                            if (generated_19.debug)
                                generated_19.Puts($"Amount: {entry.amount} - Remainder Amount: {Remainder}");
                        }

                        if (Remainder == 0)
                        {
                            if (generated_19.debug)
                                generated_19.Puts($"Remainder = 0");
                            OriginalItem.RemoveFromContainer();
                            OriginalItem.Remove(0f);
                            break;
                        }
                        else
                        {
                            if (generated_19.debug)
                                generated_19.Puts("Continuing...");
                            continue;
                        }
                    }

                    if (Remainder > 0 && player != null)
                    {
                        if (generated_19.debug)
                            if (generated_19.debug)
                                generated_19.Puts($"Remainder > 0 - Remainder Amount: {Remainder}");
                        if (generated_19.debug)
                            generated_19.Puts($"OriginalItem Amount - {OriginalItem.amount}");
                        OriginalItem.amount = Remainder;
                        if (generated_19.debug)
                            generated_19.Puts($"OriginalItem Amount - {OriginalItem.amount}");
                        OriginalItem.MarkDirty();
                    }
                }
                else
                {
                    if (generated_19.debug)
                        generated_19.Puts($"Remainder = 0");
                    OriginalItem.RemoveFromContainer();
                    OriginalItem.Remove(0f);
                }

                return null;
            }

            public object generated_226()
            {
                while (NewSlots > 0)
                {
                    newItem = ItemManager.Create(def, StackAmount);
                    newItem.MoveToContainer(container, -1, false);
                    NewSlots--;
                }

                return null;
            }

            public object generated_227()
            {
                foreach (var entry in ExistingItems)
                    if (entry != null)
                        entry.amount = StackAmount;
                return null;
            }

            public object generated_228()
            {
                def = ItemManager.FindItemDefinition(shortname);
                return null;
            }

            public object generated_229()
            {
                if (generated_19.debug)
                    generated_19.Puts("Starting Sort");
                return null;
            }

            public object generated_230()
            {
                generated_19.Sorting.Add(container);
                return null;
            }

            public void generated_122()
            {
                generated_230();
                generated_229();
                generated_228();
                generated_227();
                generated_226();
                generated_225();
                generated_224();
                generated_223();
            }
        }

        class generated_128
        {
            public FurnaceSorter generated_20;
            public void generated_126(FurnaceSorter generated_20)
            {
                this.generated_20 = generated_20;
            }

            public object generated_231()
            {
                if (generated_20.configData.maxx == new float() && generated_20.configData.maxy == new float() && generated_20.configData.minx == new float() && generated_20.configData.miny == new float())
                {
                    generated_20.configData.minx = generated_20.Default_minx;
                    generated_20.configData.miny = generated_20.Default_miny;
                    generated_20.configData.maxx = generated_20.Default_maxx;
                    generated_20.configData.maxy = generated_20.Default_maxy;
                    generated_20.SaveConfig(generated_20.configData);
                }

                return null;
            }

            public object generated_232()
            {
                generated_20.SaveConfig();
                return null;
            }

            public object generated_233()
            {
                if (generated_20.configData == null)
                    generated_20.LoadDefaultConfig();
                return null;
            }

            public object generated_234()
            {
                generated_20.LoadConfigVariables();
                return null;
            }

            public void generated_127()
            {
                generated_234();
                generated_233();
                generated_232();
                generated_231();
            }
        }

        class generated_133
        {
            public FurnaceSorter generated_21;
            public Oxide.Plugins.FurnaceSorter.ConfigData config;
            public void generated_131(FurnaceSorter generated_21)
            {
                this.generated_21 = generated_21;
            }

            public object generated_235()
            {
                generated_21.SaveConfig(config);
                return null;
            }

            public object generated_236()
            {
                config = new ConfigData { minx = generated_21.Default_minx, miny = generated_21.Default_miny, maxx = generated_21.Default_maxx, maxy = generated_21.Default_maxy, };
                return null;
            }

            public void generated_132()
            {
                generated_236();
                generated_235();
            }
        }

        class generated_138
        {
            public FurnaceSorter generated_22;
            public void generated_136(FurnaceSorter generated_22)
            {
                this.generated_22 = generated_22;
            }

            public object generated_237()
            {
                generated_22.configData = generated_22.Config.ReadObject<ConfigData>();
                return null;
            }

            public void generated_137()
            {
                generated_237();
            }
        }

        class generated_143
        {
            public FurnaceSorter generated_23;
            public ConfigData config;
            public void generated_141(FurnaceSorter generated_23, ConfigData config)
            {
                this.generated_23 = generated_23;
                this.config = config;
            }

            public object generated_238()
            {
                generated_23.Config.WriteObject(config, true);
                return null;
            }

            public void generated_142()
            {
                generated_238();
            }
        }

        public struct generated_24
        {
        }

        static System.Collections.Generic.List<generated_28> generated_263 = new System.Collections.Generic.List<generated_28>();
        static void generated_29(generated_28 generated_264)
        {
            generated_263.Add(generated_264);
        }

        static generated_28 generated_25(FurnaceSorter generated_0)
        {
            generated_28 generated_265 ;
            if (generated_263.Count > 0)
            {
                generated_265 = generated_263[0];
                generated_263.RemoveAt(0);
                generated_265.generated_26(generated_0);
                return generated_265;
            }

            generated_265 = new generated_28();
            generated_265.generated_26(generated_0);
            return generated_265;
        }

        static System.Collections.Generic.List<generated_33> generated_266 = new System.Collections.Generic.List<generated_33>();
        static void generated_34(generated_33 generated_267)
        {
            generated_266.Add(generated_267);
        }

        static generated_33 generated_30(FurnaceSorter generated_1, BasePlayer player)
        {
            generated_33 generated_268 ;
            if (generated_266.Count > 0)
            {
                generated_268 = generated_266[0];
                generated_266.RemoveAt(0);
                generated_268.generated_31(generated_1, player);
                return generated_268;
            }

            generated_268 = new generated_33();
            generated_268.generated_31(generated_1, player);
            return generated_268;
        }

        static System.Collections.Generic.List<generated_38> generated_269 = new System.Collections.Generic.List<generated_38>();
        static void generated_39(generated_38 generated_270)
        {
            generated_269.Add(generated_270);
        }

        static generated_38 generated_35(FurnaceSorter generated_2, BasePlayer player)
        {
            generated_38 generated_271 ;
            if (generated_269.Count > 0)
            {
                generated_271 = generated_269[0];
                generated_269.RemoveAt(0);
                generated_271.generated_36(generated_2, player);
                return generated_271;
            }

            generated_271 = new generated_38();
            generated_271.generated_36(generated_2, player);
            return generated_271;
        }

        static System.Collections.Generic.List<generated_43> generated_272 = new System.Collections.Generic.List<generated_43>();
        static void generated_44(generated_43 generated_273)
        {
            generated_272.Add(generated_273);
        }

        static generated_43 generated_40(FurnaceSorter generated_3, BasePlayer player)
        {
            generated_43 generated_274 ;
            if (generated_272.Count > 0)
            {
                generated_274 = generated_272[0];
                generated_272.RemoveAt(0);
                generated_274.generated_41(generated_3, player);
                return generated_274;
            }

            generated_274 = new generated_43();
            generated_274.generated_41(generated_3, player);
            return generated_274;
        }

        static System.Collections.Generic.List<generated_48> generated_275 = new System.Collections.Generic.List<generated_48>();
        static void generated_49(generated_48 generated_276)
        {
            generated_275.Add(generated_276);
        }

        static generated_48 generated_45(FurnaceSorter generated_4, BasePlayer player)
        {
            generated_48 generated_277 ;
            if (generated_275.Count > 0)
            {
                generated_277 = generated_275[0];
                generated_275.RemoveAt(0);
                generated_277.generated_46(generated_4, player);
                return generated_277;
            }

            generated_277 = new generated_48();
            generated_277.generated_46(generated_4, player);
            return generated_277;
        }

        static System.Collections.Generic.List<generated_53> generated_278 = new System.Collections.Generic.List<generated_53>();
        static void generated_54(generated_53 generated_279)
        {
            generated_278.Add(generated_279);
        }

        static generated_53 generated_50(FurnaceSorter generated_5)
        {
            generated_53 generated_280 ;
            if (generated_278.Count > 0)
            {
                generated_280 = generated_278[0];
                generated_278.RemoveAt(0);
                generated_280.generated_51(generated_5);
                return generated_280;
            }

            generated_280 = new generated_53();
            generated_280.generated_51(generated_5);
            return generated_280;
        }

        static System.Collections.Generic.List<generated_58> generated_281 = new System.Collections.Generic.List<generated_58>();
        static void generated_59(generated_58 generated_282)
        {
            generated_281.Add(generated_282);
        }

        static generated_58 generated_55(FurnaceSorter generated_6)
        {
            generated_58 generated_283 ;
            if (generated_281.Count > 0)
            {
                generated_283 = generated_281[0];
                generated_281.RemoveAt(0);
                generated_283.generated_56(generated_6);
                return generated_283;
            }

            generated_283 = new generated_58();
            generated_283.generated_56(generated_6);
            return generated_283;
        }

        static System.Collections.Generic.List<generated_63> generated_284 = new System.Collections.Generic.List<generated_63>();
        static void generated_64(generated_63 generated_285)
        {
            generated_284.Add(generated_285);
        }

        static generated_63 generated_60(FurnaceSorter generated_7, BasePlayer player, string command, string[] args)
        {
            generated_63 generated_286 ;
            if (generated_284.Count > 0)
            {
                generated_286 = generated_284[0];
                generated_284.RemoveAt(0);
                generated_286.generated_61(generated_7, player, command, args);
                return generated_286;
            }

            generated_286 = new generated_63();
            generated_286.generated_61(generated_7, player, command, args);
            return generated_286;
        }

        static System.Collections.Generic.List<generated_68> generated_287 = new System.Collections.Generic.List<generated_68>();
        static void generated_69(generated_68 generated_288)
        {
            generated_287.Add(generated_288);
        }

        static generated_68 generated_65(FurnaceSorter generated_8, BasePlayer player, string message, string arg1 = "", string arg2 = "", string arg3 = "")
        {
            generated_68 generated_289 ;
            if (generated_287.Count > 0)
            {
                generated_289 = generated_287[0];
                generated_287.RemoveAt(0);
                generated_289.generated_66(generated_8, player, message, arg1, arg2, arg3);
                return generated_289;
            }

            generated_289 = new generated_68();
            generated_289.generated_66(generated_8, player, message, arg1, arg2, arg3);
            return generated_289;
        }

        static System.Collections.Generic.List<generated_73> generated_290 = new System.Collections.Generic.List<generated_73>();
        static void generated_74(generated_73 generated_291)
        {
            generated_290.Add(generated_291);
        }

        static generated_73 generated_70(FurnaceSorter generated_9, string message, BasePlayer player = null, string arg1 = "", string arg2 = "", string arg3 = "")
        {
            generated_73 generated_292 ;
            if (generated_290.Count > 0)
            {
                generated_292 = generated_290[0];
                generated_290.RemoveAt(0);
                generated_292.generated_71(generated_9, message, player, arg1, arg2, arg3);
                return generated_292;
            }

            generated_292 = new generated_73();
            generated_292.generated_71(generated_9, message, player, arg1, arg2, arg3);
            return generated_292;
        }

        static System.Collections.Generic.List<generated_78> generated_293 = new System.Collections.Generic.List<generated_78>();
        static void generated_79(generated_78 generated_294)
        {
            generated_293.Add(generated_294);
        }

        static generated_78 generated_75(string panelName, string color, string aMin, string aMax, bool cursor = false)
        {
            generated_78 generated_295 ;
            if (generated_293.Count > 0)
            {
                generated_295 = generated_293[0];
                generated_293.RemoveAt(0);
                generated_295.generated_76(panelName, color, aMin, aMax, cursor);
                return generated_295;
            }

            generated_295 = new generated_78();
            generated_295.generated_76(panelName, color, aMin, aMax, cursor);
            return generated_295;
        }

        static System.Collections.Generic.List<generated_83> generated_296 = new System.Collections.Generic.List<generated_83>();
        static void generated_84(generated_83 generated_297)
        {
            generated_296.Add(generated_297);
        }

        static generated_83 generated_80(ref CuiElementContainer container, string panel, string color, string text, int size, string aMin, string aMax, string command, TextAnchor align = TextAnchor.MiddleCenter)
        {
            generated_83 generated_298 ;
            if (generated_296.Count > 0)
            {
                generated_298 = generated_296[0];
                generated_296.RemoveAt(0);
                generated_298.generated_81(ref container, panel, color, text, size, aMin, aMax, command, align);
                return generated_298;
            }

            generated_298 = new generated_83();
            generated_298.generated_81(ref container, panel, color, text, size, aMin, aMax, command, align);
            return generated_298;
        }

        static System.Collections.Generic.List<generated_88> generated_299 = new System.Collections.Generic.List<generated_88>();
        static void generated_89(generated_88 generated_300)
        {
            generated_299.Add(generated_300);
        }

        static generated_88 generated_85(ref CuiElementContainer element, string panel, string colorText, string colorOutline, string text, int size, string aMin, string aMax, TextAnchor align = TextAnchor.MiddleCenter)
        {
            generated_88 generated_301 ;
            if (generated_299.Count > 0)
            {
                generated_301 = generated_299[0];
                generated_299.RemoveAt(0);
                generated_301.generated_86(ref element, panel, colorText, colorOutline, text, size, aMin, aMax, align);
                return generated_301;
            }

            generated_301 = new generated_88();
            generated_301.generated_86(ref element, panel, colorText, colorOutline, text, size, aMin, aMax, align);
            return generated_301;
        }

        static System.Collections.Generic.List<generated_93> generated_302 = new System.Collections.Generic.List<generated_93>();
        static void generated_94(generated_93 generated_303)
        {
            generated_302.Add(generated_303);
        }

        static generated_93 generated_90(FurnaceSorter generated_13, BasePlayer player, BaseEntity entity)
        {
            generated_93 generated_304 ;
            if (generated_302.Count > 0)
            {
                generated_304 = generated_302[0];
                generated_302.RemoveAt(0);
                generated_304.generated_91(generated_13, player, entity);
                return generated_304;
            }

            generated_304 = new generated_93();
            generated_304.generated_91(generated_13, player, entity);
            return generated_304;
        }

        static System.Collections.Generic.List<generated_98> generated_305 = new System.Collections.Generic.List<generated_98>();
        static void generated_99(generated_98 generated_306)
        {
            generated_305.Add(generated_306);
        }

        static generated_98 generated_95(FurnaceSorter generated_14, PlayerLoot looter)
        {
            generated_98 generated_307 ;
            if (generated_305.Count > 0)
            {
                generated_307 = generated_305[0];
                generated_305.RemoveAt(0);
                generated_307.generated_96(generated_14, looter);
                return generated_307;
            }

            generated_307 = new generated_98();
            generated_307.generated_96(generated_14, looter);
            return generated_307;
        }

        static System.Collections.Generic.List<generated_103> generated_308 = new System.Collections.Generic.List<generated_103>();
        static void generated_104(generated_103 generated_309)
        {
            generated_308.Add(generated_309);
        }

        static generated_103 generated_100(FurnaceSorter generated_15, BasePlayer player)
        {
            generated_103 generated_310 ;
            if (generated_308.Count > 0)
            {
                generated_310 = generated_308[0];
                generated_308.RemoveAt(0);
                generated_310.generated_101(generated_15, player);
                return generated_310;
            }

            generated_310 = new generated_103();
            generated_310.generated_101(generated_15, player);
            return generated_310;
        }

        static System.Collections.Generic.List<generated_108> generated_311 = new System.Collections.Generic.List<generated_108>();
        static void generated_109(generated_108 generated_312)
        {
            generated_311.Add(generated_312);
        }

        static generated_108 generated_105(FurnaceSorter generated_16, ConsoleSystem.Arg arg)
        {
            generated_108 generated_313 ;
            if (generated_311.Count > 0)
            {
                generated_313 = generated_311[0];
                generated_311.RemoveAt(0);
                generated_313.generated_106(generated_16, arg);
                return generated_313;
            }

            generated_313 = new generated_108();
            generated_313.generated_106(generated_16, arg);
            return generated_313;
        }

        static System.Collections.Generic.List<generated_113> generated_314 = new System.Collections.Generic.List<generated_113>();
        static void generated_114(generated_113 generated_315)
        {
            generated_314.Add(generated_315);
        }

        static generated_113 generated_110(FurnaceSorter generated_17, BasePlayer player, string msg)
        {
            generated_113 generated_316 ;
            if (generated_314.Count > 0)
            {
                generated_316 = generated_314[0];
                generated_314.RemoveAt(0);
                generated_316.generated_111(generated_17, player, msg);
                return generated_316;
            }

            generated_316 = new generated_113();
            generated_316.generated_111(generated_17, player, msg);
            return generated_316;
        }

        static System.Collections.Generic.List<generated_118> generated_317 = new System.Collections.Generic.List<generated_118>();
        static void generated_119(generated_118 generated_318)
        {
            generated_317.Add(generated_318);
        }

        static generated_118 generated_115(FurnaceSorter generated_18, ItemContainer container, Item item)
        {
            generated_118 generated_319 ;
            if (generated_317.Count > 0)
            {
                generated_319 = generated_317[0];
                generated_317.RemoveAt(0);
                generated_319.generated_116(generated_18, container, item);
                return generated_319;
            }

            generated_319 = new generated_118();
            generated_319.generated_116(generated_18, container, item);
            return generated_319;
        }

        static System.Collections.Generic.List<generated_123> generated_320 = new System.Collections.Generic.List<generated_123>();
        static void generated_124(generated_123 generated_321)
        {
            generated_320.Add(generated_321);
        }

        static generated_123 generated_120(FurnaceSorter generated_19, BasePlayer player, ItemContainer container, Item OriginalItem, int StackAmount, List<Item> ExistingItems, string shortname, int Remainder, int NewSlots)
        {
            generated_123 generated_322 ;
            if (generated_320.Count > 0)
            {
                generated_322 = generated_320[0];
                generated_320.RemoveAt(0);
                generated_322.generated_121(generated_19, player, container, OriginalItem, StackAmount, ExistingItems, shortname, Remainder, NewSlots);
                return generated_322;
            }

            generated_322 = new generated_123();
            generated_322.generated_121(generated_19, player, container, OriginalItem, StackAmount, ExistingItems, shortname, Remainder, NewSlots);
            return generated_322;
        }

        static System.Collections.Generic.List<generated_128> generated_323 = new System.Collections.Generic.List<generated_128>();
        static void generated_129(generated_128 generated_324)
        {
            generated_323.Add(generated_324);
        }

        static generated_128 generated_125(FurnaceSorter generated_20)
        {
            generated_128 generated_325 ;
            if (generated_323.Count > 0)
            {
                generated_325 = generated_323[0];
                generated_323.RemoveAt(0);
                generated_325.generated_126(generated_20);
                return generated_325;
            }

            generated_325 = new generated_128();
            generated_325.generated_126(generated_20);
            return generated_325;
        }

        static System.Collections.Generic.List<generated_133> generated_326 = new System.Collections.Generic.List<generated_133>();
        static void generated_134(generated_133 generated_327)
        {
            generated_326.Add(generated_327);
        }

        static generated_133 generated_130(FurnaceSorter generated_21)
        {
            generated_133 generated_328 ;
            if (generated_326.Count > 0)
            {
                generated_328 = generated_326[0];
                generated_326.RemoveAt(0);
                generated_328.generated_131(generated_21);
                return generated_328;
            }

            generated_328 = new generated_133();
            generated_328.generated_131(generated_21);
            return generated_328;
        }

        static System.Collections.Generic.List<generated_138> generated_329 = new System.Collections.Generic.List<generated_138>();
        static void generated_139(generated_138 generated_330)
        {
            generated_329.Add(generated_330);
        }

        static generated_138 generated_135(FurnaceSorter generated_22)
        {
            generated_138 generated_331 ;
            if (generated_329.Count > 0)
            {
                generated_331 = generated_329[0];
                generated_329.RemoveAt(0);
                generated_331.generated_136(generated_22);
                return generated_331;
            }

            generated_331 = new generated_138();
            generated_331.generated_136(generated_22);
            return generated_331;
        }

        static System.Collections.Generic.List<generated_143> generated_332 = new System.Collections.Generic.List<generated_143>();
        static void generated_144(generated_143 generated_333)
        {
            generated_332.Add(generated_333);
        }

        static generated_143 generated_140(FurnaceSorter generated_23, ConfigData config)
        {
            generated_143 generated_334 ;
            if (generated_332.Count > 0)
            {
                generated_334 = generated_332[0];
                generated_332.RemoveAt(0);
                generated_334.generated_141(generated_23, config);
                return generated_334;
            }

            generated_334 = new generated_143();
            generated_334.generated_141(generated_23, config);
            return generated_334;
        }
    }
}