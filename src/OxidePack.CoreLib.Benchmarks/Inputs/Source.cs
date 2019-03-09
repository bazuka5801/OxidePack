using System.Collections.Generic; 
using System.Linq; 
using Oxide.Game.Rust.Cui; 
using UnityEngine; 
using System;  

namespace Oxide.Plugins 
{
	[Info("FurnaceSorter", "", "1.0.8", ResourceId = 23)]  
	class FurnaceSorter2 : RustPlugin 
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
			foreach (var player in BasePlayer.activePlayerList) DestroyPlayer(player); 
			foreach (var entry in timers) entry.Value.Destroy(); 
			
			timers.Clear(); 
		}  
		void OnPlayerInit(BasePlayer player) 
		{
			if(!Enabled.Contains(player.userID)) 
				Enabled.Add(player.userID); 
		} 
		void DestroyPlayer(BasePlayer player) 
		{
			if (player == null) 
				return; 
			if (UIInfo.ContainsKey(player.userID)) 
				UIInfo.Remove(player.userID); 
			if (timers.ContainsKey(player.userID.ToString())) 
			{
				timers[player.userID.ToString()].Destroy(); 
				timers.Remove(player.userID.ToString()); 
			} 
			if (Enabled.Contains(player.userID)) 
				Enabled.Remove(player.userID); 
			
			CuiHelper.DestroyUi(player, PanelOnScreen); 
			CuiHelper.DestroyUi(player, PanelSorter); 
		}  
		void OnPlayerDisconnected(BasePlayer player) 
		{
			DestroyPlayer(player); 
		}  
		void OnPlayerRespawned(BasePlayer player) 
		{
			CuiHelper.DestroyUi(player, PanelOnScreen); 
			CuiHelper.DestroyUi(player, PanelSorter); 
		}  
		void Loaded() 
		{
			lang.RegisterMessages(messages, this); 
		}   
		void OnServerInitialized() 
		{
			LoadVariables(); 
			permission.RegisterPermission(this.Title + ".allow", this); 
			debug = false; 
		}   
		[ChatCommand("sorter")] 
		private void chatSorter(BasePlayer player, string command, string[] args) 
		{
			if (player == null) 
				return; 
			if (player.net.connection.authLevel > 1 && args != null && args.Count() > 0 && args[0] == "debug") 
				if (debug) 
					debug = false; 
				else 
					debug = true; 
		}  
		private void GetSendMSG(BasePlayer player, string message, string arg1 = "", string arg2 = "", string arg3 = "") 
		{
			string msg = string.Format(lang.GetMessage(message, this, player.UserIDString), arg1, arg2, arg3); 
			
			SendReply(player, "<color=orange>" + lang.GetMessage("title", this, player.UserIDString) + "</color>" + "<color=#A9A9A9>" + msg + "</color>"); 
		}  
		private string GetMSG(string message, BasePlayer player = null, string arg1 = "", string arg2 = "", string arg3 = "") 
		{
			string p = null; 
			
			if (player != null) 
				p = player.UserIDString; 
			if (messages.ContainsKey(message)) 
				return string.Format(lang.GetMessage(message, this, p), arg1, arg2, arg3); 
			else 
				return message; 
		}  
		private string PanelSorter = "PanelSorter"; 
		private string PanelOnScreen = "PanelOnScreen";  
		
		public class UI 
		{
			static public CuiElementContainer CreateOverlayContainer(string panelName, string color, string aMin, string aMax, bool cursor = false) 
			{
				var NewElement = new CuiElementContainer() 
				{
					{ 
						new CuiPanel { 
							Image = {Color = color}, 
							RectTransform = {AnchorMin = aMin, AnchorMax = aMax}, 
							CursorEnabled = cursor 
						}, 
						new CuiElement().Parent = "Overlay", panelName 
					} 
				}; 
				return NewElement; 
			}  
			static public void CreateButton(ref CuiElementContainer container, string panel, string color, string text, int size, string aMin, string aMax, string command, TextAnchor align = TextAnchor.MiddleCenter) 
			{
				container.Add(new CuiButton { 
					Button = { Color = color, Command = command, FadeIn = 1.0f },
					RectTransform = { AnchorMin = aMin, AnchorMax = aMax }, 
					Text = { Text = text, FontSize = size, Align = align } 
				}, panel); 
			}  
			static public void CreateTextOutline(ref CuiElementContainer element, string panel, string colorText, string colorOutline, string text, int size, string aMin, string aMax, TextAnchor align = TextAnchor.MiddleCenter)
			{
				element.Add(new CuiElement {
					Parent = panel, 
					Components = { 
						new CuiTextComponent{Color = colorText, FontSize = size, Align = align, Text = text }, 
						new CuiOutlineComponent {Distance = "1 1", Color = colorOutline}, 
						new CuiRectTransformComponent {AnchorMax = aMax, AnchorMin = aMin } 
					} 
				}); 
			} 
		}  
		
		void OnLootEntity(BasePlayer player, BaseEntity entity) 
		{
			if (player == null || entity == null) 
				return; 
			if (entity as BaseOven != null && permission.UserHasPermission(player.UserIDString, this.Name + ".allow")) 
			{
				if (!UIInfo.ContainsKey(player.userID)) 
					UIInfo.Add(player.userID, new info()); 
				
				UIInfo[player.userID].oven = (entity as BaseOven); 
				SorterUI(player);
			} 
		}  
		private void OnPlayerLootEnd(PlayerLoot looter) 
		{
			if (looter != null && looter.entitySource != null && looter.entitySource is BaseOven) 
			{
				BasePlayer player = looter.GetComponent<BasePlayer>(); 
				
				if (player == null) 
					return; 
				if (UIInfo.ContainsKey(player.userID)) 
					UIInfo[player.userID].oven = null; 
				
				CuiHelper.DestroyUi(player, PanelSorter); 
			} 
		}   
		void SorterUI(BasePlayer player) 
		{ 
			CuiHelper.DestroyUi(player, PanelSorter); 
			
			var element = UI.CreateOverlayContainer(PanelSorter, "0 0 0 0", $"{configData.minx} {configData.miny}", $"{configData.maxx} {configData.maxy}"); 
			
			if (Enabled.Contains(player.userID)) 
				UI.CreateButton(ref element, PanelSorter, "0.584 0.29 0.211 1.0", GetMSG("<size=11>СОРТИРОВКА</size> Выключить", player), 12, "1.83 2.7", "2.14 4.03", $"UI_ToggleSorter {3}"); 
			else 
				UI.CreateButton(ref element, PanelSorter, "0.439 0.509 0.294 1.0", GetMSG("<size=11>СОРТИРОВКА</size> Включить", player), 12, "1.83 2.7", "2.14 4.03", $"UI_ToggleSorter {3}"); 
			
			CuiHelper.AddUi(player, element);			
		}  
		[ConsoleCommand("UI_ToggleSorter")] 
		void cmdUI_ToggleSorter(ConsoleSystem.Arg arg) 
		{
			var player = arg.Connection.player as BasePlayer; 
			
			if (player == null) 
				return; 
			if (!permission.UserHasPermission(player.UserIDString, this.Name + ".allow")) 
				return; 
			if (Enabled.Contains(player.userID)) 
			{ 
				Enabled.Remove(player.userID);
				OnScreen(player, "FurnaceSorterDisabled"); 
				SorterUI(player); 
			}
			else 
			{
				Enabled.Add(player.userID); 
				OnScreen(player, "FurnaceSorterEnabled"); 
				SorterUI(player); 
			} 
		}  
		void OnScreen(BasePlayer player, string msg) 
		{
			if (timers.ContainsKey(player.userID.ToString())) 
			{
				timers[player.userID.ToString()].Destroy(); 
				timers.Remove(player.userID.ToString()); 
			} 
			
			CuiHelper.DestroyUi(player, PanelOnScreen); 
			
			var element = UI.CreateOverlayContainer(PanelOnScreen, "0.0 0.0 0.0 0.0", "0.3 0.5", "0.7 0.8"); 
			
			UI.CreateTextOutline(ref element, PanelOnScreen, string.Empty, "0 0 0 1", GetMSG(msg, player), 32, "0.0 0.0", "1.0 1.0"); 
			CuiHelper.AddUi(player, element); 
			timers.Add(player.userID.ToString(), timer.Once(4, () => CuiHelper.DestroyUi(player, PanelOnScreen)));
		}   
		
		public List<ulong> Enabled = new List<ulong>(); 
		public List<ItemContainer> Sorting = new List<ItemContainer>(); 
		
		object CanAcceptItem(ItemContainer container, Item item) 
		{
			if (container == null || item == null || container.entityOwner == null || container.entityOwner.GetComponent<BaseOven>() == null || item.parent == container|| Sorting.Contains(container)) 
				return null; 
			if (debug) 
				Puts("Item Being Accepted?"); 
			
			BasePlayer player = item.GetOwnerPlayer(); 
			
			if (player == null || !Enabled.Contains(player.userID)) 
				return null; 
			
			List<string> AcceptableItems = new List<string>(); 
			
			if (container.entityOwner.GetComponent<BaseOven>().ShortPrefabName.Contains("campfire")) 
				AcceptableItems = new List<string> 
				{
					"bearmeat",
					"deermeat.raw", 
					"humanmeat.raw", 
					"meat.boar", 
					"wolfmeat.raw", 
					"can.beans.empty", 
					"can.tuna.empty", 
					"chicken.raw", 
					"fish.raw"
				}; 
			else if (container.entityOwner.GetComponent<BaseOven>().ShortPrefabName.Contains("furnace")) 
				AcceptableItems = new List<string> 
				{
					"hq.metal.ore", 
					"metal.ore", 
					"sulfur.ore", 
					"can.beans.empty", 
					"can.tuna.empty" 
				}; 
			else if (container.entityOwner.GetComponent<BaseOven>().ShortPrefabName.Contains("refinery")) 
				AcceptableItems = new List<string> 
				{
					"crude.oil" 
				}; 
			if (!AcceptableItems.Contains(item.info.shortname)) 
				return null; 
			if (container.entityOwner.GetComponent<BaseOven>().IsOn()) 
			{
				Enabled.Remove(player.userID); 
				OnScreen(player, "FurnaceOnSorterDisabled"); 
				SorterUI(player); 
				return null; 
			} 
			
			var TotalAmount = item.amount; 
			
			if (debug) 
				Puts($"TotalAmount = {TotalAmount}"); 
			
			List<Item> LessThenMax = new List<global::Item>();
			List<Item> Items = new List<Item>(); 
			
			foreach (var entry in container.itemList.Where(k => k.info.shortname == item.info.shortname)) 
			{
				Items.Add(entry); 
				TotalAmount += entry.amount; 
				
				if (entry.amount < item.MaxStackable()) 
					LessThenMax.Add(entry); 
				if (debug) 
					Puts($"TotalAmount = {TotalAmount}"); 
			} 
			
			var NewSlots = container.capacity - container.itemList.Count(); 
			
			if (NewSlots <= 2 && LessThenMax.Count == 0) 
				return ItemContainer.CanAcceptResult.CannotAccept; 
			if (NewSlots >= 2) 
			{
				if (container.entityOwner.GetComponent<BaseOven>().ShortPrefabName.Contains("refinery")) 
					NewSlots -= 1; 
				else 
					NewSlots -= 2; 
			} 
			if (NewSlots > TotalAmount)
				NewSlots = TotalAmount; 
			if (debug) 
				Puts($"{NewSlots} - Available Slots"); 
			
			var totalSlots = Items.Count(); 
			
			if (totalSlots == 0) 
				totalSlots += NewSlots; 
			else 
				NewSlots = 0; 
			if (totalSlots > TotalAmount) 
				totalSlots = TotalAmount; 
			
			var remainder = TotalAmount % totalSlots; 
			
			if (debug) 
				Puts($"Remainder: {remainder}"); 
			
			var SplitableAmount = TotalAmount - remainder;

			if (debug) 
				Puts($"SplitAmount: {SplitableAmount}"); 
			
			var eachStack = SplitableAmount / totalSlots; 
			
			if (debug) 
				Puts($"EachStack: {eachStack}"); 
			if (eachStack > item.MaxStackable()) 
			{
				eachStack = item.MaxStackable(); 
				remainder = TotalAmount - (eachStack * totalSlots); 
			} 
			
			SortFurnace(player, container, item, eachStack, Items, item.info.shortname, remainder, NewSlots); 
			return ItemContainer.CanAcceptResult.CannotAccept; 
		} 
		
		void SortFurnace(BasePlayer player, ItemContainer container, Item OriginalItem, int StackAmount, List<Item> ExistingItems, string shortname, int Remainder, int NewSlots) 
		{
			Sorting.Add(container);

			if (debug) 
				Puts("Starting Sort"); 
			
			ItemDefinition def = ItemManager.FindItemDefinition(shortname); 
			
			foreach (var entry in ExistingItems) 
			if (entry != null) 
				entry.amount = StackAmount; 
			
			while (NewSlots > 0) 
			{
				Item newItem = ItemManager.Create(def, StackAmount); 
				newItem.MoveToContainer(container, -1, false); 
				NewSlots--;
			} 
			if (Remainder > 0) 
			{
				foreach (var entry in container.itemList.Where(k => k.info.shortname == OriginalItem.info.shortname))
				{
					if (debug) 
						Puts($"Amount: {entry.amount} - Remainder Amount: {Remainder}"); 
					if (entry.amount != entry.MaxStackable()) 
					{
						entry.amount++; 
						Remainder--; 
						
						if (debug) 
							Puts($"Amount: {entry.amount} - Remainder Amount: {Remainder}"); 
					} 
					if (Remainder == 0) 
					{
						if (debug) 
							Puts($"Remainder = 0"); 
						
						OriginalItem.RemoveFromContainer(); 
						OriginalItem.Remove(0f); 
						break; 
					}
					else 
					{
						if (debug) 
							Puts("Continuing..."); 
						continue; 
					} 
				} 
				if (Remainder > 0 && player != null) 
				{
					if (debug) 
						if (debug) 
							Puts($"Remainder > 0 - Remainder Amount: {Remainder}"); 
					if (debug) 
						Puts($"OriginalItem Amount - {OriginalItem.amount}"); 
					
					OriginalItem.amount = Remainder; 
					
					if (debug) 
						Puts($"OriginalItem Amount - {OriginalItem.amount}"); 
					
					OriginalItem.MarkDirty(); 
				} 
			} 
			else 
			{
				if (debug) 
					Puts($"Remainder = 0"); 
				
				OriginalItem.RemoveFromContainer();
				OriginalItem.Remove(0f); 
			} 
			
			Sorting.Remove(container); 
			container.MarkDirty();
		}  
		
		Dictionary<int, double> WoodRatios = new Dictionary<int, double> 
		{
			{-1059362949, 5 }, 
			{889398893, 2.5 }, 
			{2133577942, 10 }, 
			{1325935999, 3 }, 
			{-642008142, 3 }, 
			{-253819519, 3 }, 
			{179448791, 3 }, 
			{-1658459025, 3 }, 
			{-533484654, 3 }, 
			{2080339268, 3 }, 
			{1050986417, 3 }, 
			{1983936587, 6.66 }, 
		};  
		
		float Default_minx = 0.646f; 
		float Default_miny = 0.1f;
		float Default_maxx = 0.81f; 
		float Default_maxy = 0.14f;  
		
		private ConfigData configData; 
		
		class ConfigData 
		{
			public float minx { get; set; } 
			public float miny { get; set; } 
			public float maxx { get; set; } 
			public float maxy { get; set; } 
		} 
		
		private void LoadVariables() 
		{
			LoadConfigVariables();

			if (configData == null) 
				LoadDefaultConfig(); 
			
			SaveConfig(); 
			
			if (configData.maxx == new float() && configData.maxy == new float() && configData.minx == new float() && configData.miny == new float()) 
			{
				configData.minx = Default_minx; 
				configData.miny = Default_miny; 
				configData.maxx = Default_maxx; 
				configData.maxy = Default_maxy; 
				SaveConfig(configData); 
			} 
		}
		protected override void LoadDefaultConfig() 
		{
			var config = new ConfigData 
			{
				minx = Default_minx, 
				miny = Default_miny, 
				maxx = Default_maxx, 
				maxy = Default_maxy, 
			}; 
			SaveConfig(config); 
		}
		private void LoadConfigVariables() => configData = Config.ReadObject<ConfigData>(); 
		void SaveConfig(ConfigData config) => Config.WriteObject(config, true);  
		
		Dictionary<string, string> messages = new Dictionary<string, string>() 
		{
			{"title", "Сортировка печей: " }, 
			{"NoPerm", "У вас нет прав, чтобы использовать эту команду" }, 
			{"FurnaceSorterDisabled", "Сортировка печей выключена." }, 
			{"FurnaceOnSorterDisabled", "Сортировка не может использоваться, когда печь включена." }, 
			{"FurnaceSorterEnabled", "Сортировка печей включена." }, 
			{"ToggleOff", "СОРТИРОВКА Выключить" }, 
			{"ToggleOn", "СОРТИРОВКА Включить" }, 
			{"OptimizationUnavailable", "Вы не можете использовать Оптимизацию, пока печь включена!" }, 
			{"NothingToOptimize", "Оптимизировать в печи нечего. Ошибка при оптимизации!" }, 
			{"NoWood", "В печи нет дерева. Ошибка при оптимизации!" }, 
			{"NoAcceptableItems", "В печи нет каких-либо действительных ресурсов для оптимизации дерева. Ошибка при оптимизации!" }, 
			{"WoodRatioGood", "Допустимое соотношение древесины правильное. Оптимизация не требуется!" }, 
			{"WoodNeeded", "Оптимизация показала, что у вас не хватает <color=\"#ffd479\">{0}</color> дерева" }, 
			{"ExtraWoodGiven", "Оптимизация показала, что у вас есть <color=\"#ffd479\">{0}</color> лишней древесины. Это дерево было помещено в ваш инвентарь!" }, 
			{"InventoryFull", "У вас недостаточно места в инвентаре, чтобы получить лишнию древесину. Ошибка при оптимизации!" }, 
			{"FurnaceOptimized", "Ваша печь оптимизирована!" }, 
		}; 
	} 
} 