﻿using ElectronicObserver.Data.Battle;
using ElectronicObserver.Data.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{


	/// <summary>
	/// 艦これのデータを扱う中核です。
	/// </summary>
	public sealed class KCDatabase
	{


		#region Singleton

		private static readonly KCDatabase instance = new KCDatabase();

		public static KCDatabase Instance => instance;

		#endregion



		/// <summary>
		/// 艦船のマスターデータ
		/// </summary>
		public IDDictionary<ShipDataMaster> MasterShips { get; private set; }

		/// <summary>
		/// 艦種データ
		/// </summary>
		public IDDictionary<ShipType> ShipTypes { get; private set; }

		/// <summary>
		/// 装備のマスターデータ
		/// </summary>
		public IDDictionary<EquipmentDataMaster> MasterEquipments { get; private set; }

		/// <summary>
		/// 装備種別
		/// </summary>
		public IDDictionary<EquipmentType> EquipmentTypes { get; private set; }


		/// <summary>
		/// 保有艦娘のデータ
		/// </summary>
		public IDDictionary<ShipData> Ships { get; private set; }

		/// <summary>
		/// 舰娘排序 | TKey: MasterID, [ 舰种序, Lv 序, 改装「他」序 ]
		/// </summary>
		public Dictionary<int, int[]> ShipsOrder { get; private set; }

		/// <summary>
		/// 保有装備のデータ
		/// </summary>
		public IDDictionary<EquipmentData> Equipments { get; private set; }


		/// <summary>
		/// 提督・司令部データ
		/// </summary>
		public AdmiralData Admiral { get; private set; }


		/// <summary>
		/// アイテムのマスターデータ
		/// </summary>
		public IDDictionary<UseItemMaster> MasterUseItems { get; private set; }

		/// <summary>
		/// アイテムデータ
		/// </summary>
		public IDDictionary<UseItem> UseItems { get; private set; }


		/// <summary>
		/// 工廠ドックデータ
		/// </summary>
		public IDDictionary<ArsenalData> Arsenals { get; private set; }

		/// <summary>
		/// 入渠ドックデータ
		/// </summary>
		public IDDictionary<DockData> Docks { get; private set; }


		/// <summary>
		/// 艦隊データ
		/// </summary>
		public FleetManager Fleet { get; private set; }


		/// <summary>
		/// 資源データ
		/// </summary>
		public MaterialData Material { get; private set; }


		/// <summary>
		/// 任務データ
		/// </summary>
		public QuestManager Quest { get; private set; }

		/// <summary>
		/// 任務進捗データ
		/// </summary>
		public QuestProgressManager QuestProgress { get; private set; }


		/// <summary>
		/// 戦闘データ
		/// </summary>
		public BattleManager Battle { get; private set; }


		/// <summary>
		/// 海域カテゴリデータ
		/// </summary>
		public IDDictionary<MapAreaData> MapArea { get; private set; }

		/// <summary>
		/// 海域データ
		/// </summary>
		public IDDictionary<MapInfoData> MapInfo { get; private set; }


		/// <summary>
		/// 遠征データ
		/// </summary>
		public IDDictionary<MissionData> Mission { get; private set; }


		/// <summary>
		/// 艦船グループデータ
		/// </summary>
		public ShipGroupManager ShipGroup { get; private set; }


		/// <summary>
		/// 基地航空隊データ
		/// </summary>
		public IDDictionary<BaseAirCorpsData> BaseAirCorps { get; private set; }

		/// <summary>
		/// 配置転換中装備データ
		/// </summary>
		public IDDictionary<RelocationData> RelocatedEquipments { get; private set; }

		private KCDatabase()
		{

			MasterShips = new IDDictionary<ShipDataMaster>();
			ShipTypes = new IDDictionary<ShipType>();
			MasterEquipments = new IDDictionary<EquipmentDataMaster>();
			EquipmentTypes = new IDDictionary<EquipmentType>();
			Ships = new IDDictionary<ShipData>();
			ShipsOrder = new Dictionary<int, int[]>();
			Equipments = new IDDictionary<EquipmentData>();
			Admiral = new AdmiralData();
			MasterUseItems = new IDDictionary<UseItemMaster>();
			UseItems = new IDDictionary<UseItem>();
			Arsenals = new IDDictionary<ArsenalData>();
			Docks = new IDDictionary<DockData>();
			Fleet = new FleetManager();
			Material = new MaterialData();
			Quest = new QuestManager();
			QuestProgress = new QuestProgressManager();
			Battle = new BattleManager();
			MapArea = new IDDictionary<MapAreaData>();
			MapInfo = new IDDictionary<MapInfoData>();
			Mission = new IDDictionary<MissionData>();
			ShipGroup = new ShipGroupManager();
			BaseAirCorps = new IDDictionary<BaseAirCorpsData>();
			RelocatedEquipments = new IDDictionary<RelocationData>();
		}


		public void Load()
		{

			{
				var temp = (ShipGroupManager)ShipGroup.Load();
				if (temp != null)
					ShipGroup = temp;
			}
			{
				var temp = QuestProgress.Load();
				if (temp != null)
				{
					if (QuestProgress != null)
						QuestProgress.RemoveEvents();
					QuestProgress = temp;
				}
			}

		}

		public void Save()
		{
			ShipGroup.Save();
			QuestProgress.Save();
		}

		public void UpdateSortShips()
		{
			if (Utility.Configuration.Config.UI.AllowSortIndexing) {

				// 几种不同的 ID
				// s.MasterID				获取时的 ID ( 每个舰娘独立，作为 KEY 使用 )
				// s.ShipID					舰娘 ID ( 每种改装状态独立，以下排序未使用 )
				// s.SortID					排序 ID ( 图鉴 ID )
				ShipsOrder.Clear();

				// 舰种序
				var ShipsTypeSorted = Ships.Values.OrderByDescending(s => s.MasterShip.ShipType)
					.ThenBy(s => s.SortID)
					.ThenByDescending(s => s.Level)
					.ThenBy(s => s.MasterID);
				int index = 1;
				foreach (var ship in ShipsTypeSorted) {
					ShipsOrder.Add(ship.MasterID, new int[] { index, 0, 0 });
					index++;
				}

				// Lv 序 & 改装「他」序
				var ShipsLvSorted = Ships.Values.OrderByDescending(s => s.Level)
					.ThenBy(s => s.SortID)
					.ThenBy(s => s.MasterID);
				index = 1;
				// 寻找已在舰队中的舰娘
				List<int> ShipsInFleet = new List<int>();
				FleetManager fm = KCDatabase.Instance.Fleet;
				foreach (var f in fm.Fleets.Values) {
					foreach (int s in f.Members) {
						if (s != -1)
							ShipsInFleet.Add(s);
					}
					//ShipsInFleet.AddRange(f.Members);
				}
				//ShipsInFleet.RemoveAll(s => s == -1);
				int count = Ships.Values.Count() - ShipsInFleet.Count;
				foreach (var ship in ShipsLvSorted) {
					ShipsOrder[ship.MasterID][1] = index;
					index++;
					if (!ShipsInFleet.Contains(ship.MasterID)) {
						ShipsOrder[ship.MasterID][2] = count;
						count--;
					}
				}
			}
		}

	}


}
