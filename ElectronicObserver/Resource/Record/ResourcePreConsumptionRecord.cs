using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronicObserver.Resource.Record
{

	/// <summary>
	/// 資源消費レコード書き前のレコードを保持します。
	/// </summary>
	public class ResourcePreConsumptionRecord : RecordBase
	{

		public sealed class ResourcePreConsumptionElement : RecordElementBase
		{

			/// <summary>
			/// 帰投日時
			/// </summary>
			public DateTime Date { get; set; }

			/// <summary>
			/// 帰投前航路 eg: 6-5-1-3-4-7-13-BOSS / practice / expedition-6
			/// </summary>
			public string Destination { get; set; }

			/// <summary>
			/// 艦娘固有ID ( eg: 1 )
			/// </summary>
			public int MasterID { get; set; }

			/// <summary>
			/// 艦船ID ( eg: 237 - 電改 )
			/// </summary>
			public int ShipID { get; set; }

			/// <summary>
			/// 現時点消耗した燃料
			/// </summary>
			public int UsedFuel { get; set; }

			/// <summary>
			/// 現時点消耗した弾薬
			/// </summary>
			public int UsedAmmo { get; set; }

			/// <summary>
			/// 現時点撃墜された艦載機数
			/// </summary>
			public int AircraftLost { get; set; }

			/// <summary>
			/// 現時点入渠需要の燃料
			/// </summary>
			public int RepairFuel { get; set; }

			/// <summary>
			/// 現時点入渠需要の鋼材
			/// </summary>
			public int RepairSteel { get; set; }

			/// <summary>
			/// 帰投状态
			/// 0 = 帰投済み / 1 = 未帰投 ( 出撃・演習・遠征 ) / -1 = 削除待ち
			/// </summary>
			public int RecordStatus { get; set; }

			public ResourcePreConsumptionElement() { }

			public ResourcePreConsumptionElement(string line)
				: this()
			{
				LoadLine(line);
			}

			public ResourcePreConsumptionElement(DateTime date, string destination, int masterID, int shipID,
				int usedFuel, int usedAmmo, int aircraftLost, int repairFuel, int repairSteel, int recordStatus)
				: this()
			{
				Date = date;
				Destination = destination;
				MasterID = masterID;
				ShipID = shipID;
				UsedFuel = usedFuel;
				UsedAmmo = usedAmmo;
				AircraftLost = aircraftLost;
				RepairFuel = repairFuel;
				RepairSteel = repairSteel;
				RecordStatus = recordStatus;
			}

			public override void LoadLine(string line)
			{

				string[] elem = line.Split(",".ToCharArray());
				if (elem.Length < 10) throw new ArgumentException("要素数が少なすぎます。");

				Date = DateTimeHelper.CSVStringToTime(elem[0]);
				Destination = elem[1];
				MasterID = int.Parse(elem[2]);
				ShipID = int.Parse(elem[3]);
				UsedFuel = int.Parse(elem[4]);
				UsedAmmo = int.Parse(elem[5]);
				AircraftLost = int.Parse(elem[6]);
				RepairFuel = int.Parse(elem[7]);
				RepairSteel = int.Parse(elem[8]);
				RecordStatus = int.Parse(elem[9]);
			}

			public override string SaveLine()
			{
				return string.Join(",",
					DateTimeHelper.TimeToCSVString(Date),
					Destination,
					MasterID,
					ShipID,
					UsedFuel,
					UsedAmmo,
					AircraftLost,
					RepairFuel,
					RepairSteel,
					RecordStatus);
			}

		}


		public List<ResourcePreConsumptionElement> Record { get; private set; }
		private bool _initialFlag;
		private bool _changed;


		public ResourcePreConsumptionRecord()
			: base()
		{

			Record = new List<ResourcePreConsumptionElement>();
			_initialFlag = false;
			_changed = false;
		}

		public void UpdateDestination(string map, int destination, string endpoint)
		{
			foreach (var record in Record.Where(record => record.RecordStatus == 1)) {
				record.Destination = string.Format(
					"{0}-{1}{2}",
					record.Destination.StartsWith(map) ? record.Destination : map,
					destination,
					endpoint);
			}
		}

		public override void RegisterEvents()
		{
			var ao = APIObserver.Instance;

			ao["api_start2"].ResponseReceived += ResourcePreConsumptionRecord_Start;
			ao["api_port/port"].ResponseReceived += ResourcePreConsumptionRecord_Updated;

			ao["api_req_map/start"].RequestReceived += OnSortie;
			ao["api_req_practice/battle"].RequestReceived += OnPractice;
			ao["api_req_mission/result"].ResponseReceived += OnExpeditionReturn;

			ao["api_req_hokyu/charge"].RequestReceived += OnCharge;
			ao["api_req_nyukyo/start"].RequestReceived += OnRepair;
			ao["api_req_nyukyo/speedchange"].RequestReceived += OnRepairSpeedUp;

			ao["api_req_kousyou/destroyship"].RequestReceived += OnDestoryShip;
			ao["api_req_kaisou/powerup"].RequestReceived += OnPowerUp;
			ao["api_req_kaisou/remodeling"].RequestReceived += OnRemodeling;
		}

		private void ResourcePreConsumptionRecord_Start(string apiname, dynamic data)
		{
			_initialFlag = true;
		}

		private void ResourcePreConsumptionRecord_Updated(string apiname, dynamic data)
		{
			DateTime returnDateTime = DateTime.Now;
			var ships = KCDatabase.Instance.Ships;
			var fleets = KCDatabase.Instance.Fleet.Fleets.Values;

			if (_initialFlag) {
				// ログイン後最初の帰投
				// 他端末の操作、接続異常による記録不正確の可能性があるため、ここでリコードをチェクします。
				_initialFlag = false;

				// 在籍チェク
				foreach (var masterID in Record.Select(r => r.MasterID).Distinct()) {
					if (!ships.ContainsKey(masterID)) {
						Record.RemoveAll(record => record.MasterID == masterID);
					}
				}

				// 全員チェクします
				foreach (var ship in ships) {
					int masterID = ship.Key;
					int usedFuel = ship.Value.FuelMax - ship.Value.Fuel;
					int usedAmmo = ship.Value.AmmoMax - ship.Value.Ammo;
					int aircraftLost = ship.Value.MasterShip.AircraftTotal - ship.Value.AircraftTotal;
					int repairFuel = ship.Value.RepairingDockID == -1 ? ship.Value.RepairFuel : 0;
					int repairSteel = ship.Value.RepairingDockID == -1 ? ship.Value.RepairSteel : 0;
					var oldRecords = Record.Where(r => r.RecordStatus == 0 && r.MasterID == masterID);

					if (oldRecords.Count() == 0) {
						if (usedFuel + usedAmmo + aircraftLost + repairFuel + repairSteel > 0) {
							Record.Add(new ResourcePreConsumptionElement(returnDateTime, "unknown-norecord", masterID, ship.Value.ShipID,
								usedFuel, usedAmmo, aircraftLost, repairFuel, repairSteel, 0));
						}
					} else {
						var lastRecord = oldRecords.LastOrDefault();
						string lastDestiation = lastRecord.Destination;

						if (lastRecord.RepairFuel != repairFuel || lastRecord.RepairSteel != repairSteel) {
							if (repairFuel + repairSteel > 0) {
								int repairedFuel = lastRecord.RepairFuel - repairFuel;
								int repairedSteel = lastRecord.RepairSteel - repairSteel;
								if (repairedFuel >= 0 && repairedSteel >= 0 &&
									ship.Value.HPRate > 0.5 && fleets.FirstOrDefault(f => f.Members.Contains(masterID)).PreviouslyCanAnchorageRepair) {
									// Could be Akashi
									RecordRepair(returnDateTime, lastRecord, repairedFuel, repairedSteel);
								} else {
									// Definitly NOT Akashi
									RepairToConsumptionRecord(returnDateTime, masterID);
									lastDestiation = "unknown-repair";
								}
								foreach (var record in Record.Where(record => record.RecordStatus == 0 && record.MasterID == masterID)) {
									record.RepairFuel = 0;
									record.RepairSteel = 0;
								}
								Record.Add(new ResourcePreConsumptionElement(returnDateTime, lastDestiation, masterID, ship.Value.ShipID, usedFuel, usedAmmo, aircraftLost, repairFuel, repairSteel, 0));
							} else {
								RepairToConsumptionRecord(returnDateTime, masterID);
							}
						}

						if (lastRecord.UsedFuel != usedFuel || lastRecord.UsedAmmo != usedAmmo || lastRecord.AircraftLost != aircraftLost) {
							if (usedFuel + usedAmmo + aircraftLost > 0) {
								if (aircraftLost == 0) { // Refilled
									if (usedAmmo > 0) { // Fuel Only
										RecordCharge(returnDateTime, lastRecord, lastRecord.UsedFuel, 0, lastRecord.AircraftLost * 5);
									} else if (usedFuel > 0) { // Ammo Only
										RecordCharge(returnDateTime, lastRecord, 0, lastRecord.UsedAmmo, lastRecord.AircraftLost * 5);
									}
								} else {
									// Something else happened
									ChargeToConsumptionRecord(returnDateTime, new int[] { masterID });
									lastDestiation = "unknown-refill";
								}
								foreach (var record in Record.Where(record => record.RecordStatus == 0 && record.MasterID == masterID)) {
									record.UsedFuel = 0;
									record.UsedAmmo = 0;
									record.AircraftLost = 0;
								}
								Record.Add(new ResourcePreConsumptionElement(returnDateTime, lastDestiation, masterID, ship.Value.ShipID,
									usedFuel, usedAmmo, aircraftLost, repairFuel, repairSteel, 0));
							} else {
								ChargeToConsumptionRecord(returnDateTime, new int[] { masterID });
							}
						}
					}
				}
				goto BackPort;
			}

			// 泊地修理処理
			foreach (var fleet in fleets.Where(f => f.PreviouslyCanAnchorageRepair)) {
				foreach (var masterID in fleet.Members) {
					var oldRecords = Record.Where(r => r.RecordStatus == 0 && r.MasterID == masterID);
					if (oldRecords.Count() > 0) {
						var ship = ships[masterID];
						int repairFuel = ship.RepairingDockID == -1 ? ship.RepairFuel : 0;
						int repairSteel = ship.RepairingDockID == -1 ? ship.RepairSteel : 0;
						var lastRecord = oldRecords.LastOrDefault();
						string lastDestiation = lastRecord.Destination;
						if (lastRecord.RepairFuel != repairFuel || lastRecord.RepairSteel != repairSteel) {
							if (repairFuel + repairSteel > 0) {
								int repairedFuel = lastRecord.RepairFuel - repairFuel;
								int repairedSteel = lastRecord.RepairSteel - repairSteel;
								if (repairedFuel >= 0 && repairedSteel >= 0 && ship.HPRate > 0.5) {
									// Utility.Logger.Add(2, $"明石修理部分: {ship.NameWithLevel} 油 {repairedFuel} 钢 {repairedSteel}");
									RecordRepair(returnDateTime, lastRecord, repairedFuel, repairedSteel);
								} else {
									Utility.Logger.Add(2, $"泊地修理不正: [{masterID}]{ship.NameWithLevel}");
									RepairToConsumptionRecord(returnDateTime, masterID);
									lastDestiation = "unknown-repair";
								}
								foreach (var record in Record.Where(record => record.RecordStatus == 0 && record.MasterID == masterID)) {
									record.RepairFuel = 0;
									record.RepairSteel = 0;
								}
								Record.Add(new ResourcePreConsumptionElement(returnDateTime, lastDestiation, masterID, ship.ShipID, ship.FuelMax - ship.Fuel, ship.AmmoMax - ship.Ammo, ship.MasterShip.AircraftTotal - ship.AircraftTotal, repairFuel, repairSteel, 0));
							} else {
								// Utility.Logger.Add(2, $"明石修理全快: {ship.NameWithLevel}");
								RepairToConsumptionRecord(returnDateTime, masterID);
							}
						}
					}
				}
			}

			// 出撃・演習・遠征帰投処理
			BackPort:
			foreach (var record in Record.Where(record => record.RecordStatus == 1)) {
				var shipData = ships[record.MasterID];
				int usedFuel = shipData.FuelMax - shipData.Fuel;
				int usedAmmo = shipData.AmmoMax - shipData.Ammo;
				int aircraftLost = shipData.MasterShip.AircraftTotal - shipData.AircraftTotal;
				int repairFuel = shipData.RepairFuel;
				int repairSteel = shipData.RepairSteel;
				if (usedFuel + usedAmmo + aircraftLost + repairFuel + repairSteel > 0) {
					record.Date = returnDateTime;
					record.UsedFuel = usedFuel;
					record.UsedAmmo = usedAmmo;
					record.AircraftLost = aircraftLost;
					record.RepairFuel = repairFuel;
					record.RepairSteel = repairSteel;
					record.RecordStatus = 0;
				} else {
					record.RecordStatus = -1;
					Utility.Logger.Add(2, $"帰投不正: [{record.MasterID}]{shipData.NameWithLevel}");
				}
			}

			Record.RemoveAll(record => record.RecordStatus == -1);
			Record.RemoveAll(record => Cleanable(record));
			_changed = true;
		}

		private void OnSortie(string apiname, dynamic data)
		{
			DateTime sortieDateTime = DateTime.Now;
			var fleetID = int.Parse((string)data["api_deck_id"]);
			FleetManager fm = KCDatabase.Instance.Fleet;
			List<int> masterIDs = fm.Fleets[fleetID].Members.ToList();
			if (fm.CombinedFlag != 0 && fleetID == 1) {
				masterIDs.AddRange(fm.Fleets[2].Members);
			}
			foreach (var masterID in masterIDs) {
				if (masterID == -1) continue;
				Record.Add(new ResourcePreConsumptionElement(sortieDateTime, "sortie", masterID, KCDatabase.Instance.Ships[masterID].ShipID, 0, 0, 0, 0, 0, 1));
			}
			_changed = true;
		}

		private void OnPractice(string apiname, dynamic data)
		{
			DateTime practiceDateTime = DateTime.Now;
			var fleetID = int.Parse((string)data["api_deck_id"]);
			var enemyID = int.Parse((string)data["api_enemy_id"]);
			FleetManager fm = KCDatabase.Instance.Fleet;
			List<int> masterIDs = fm.Fleets[fleetID].Members.ToList();
			foreach (var masterID in masterIDs) {
				if (masterID == -1) continue;
				Record.Add(new ResourcePreConsumptionElement(practiceDateTime, "practice-" + enemyID, masterID, KCDatabase.Instance.Ships[masterID].ShipID, 0, 0, 0, 0, 0, 1));
			}
			_changed = true;
		}

		private void OnExpeditionReturn(string apiname, dynamic data)
		{
			DateTime returnDateTime = DateTime.Now;
			int[] masterIDs = (int[])data["api_ship_id"];
			var fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.Members.Contains(masterIDs[1]));
			int missionID = fleet.ExpeditionDestination;
			foreach (var masterID in masterIDs) {
				if (masterID == -1) continue;
				Record.Add(new ResourcePreConsumptionElement(returnDateTime, "expedition-" + missionID, masterID, KCDatabase.Instance.Ships[masterID].ShipID, 0, 0, 0, 0, 0, 1));
			}
			_changed = true;
		}

		private void OnDestoryShip(string apiname, dynamic data)
		{
			RemoveShips(((string)data["api_ship_id"]).Split(',').Select(id => int.Parse(id)).ToArray());
		}

		private void OnRemodeling(string apiname, dynamic data)
		{
			DateTime remodelDateTime = DateTime.Now;
			var masterID = int.Parse((string)data["api_id"]);
			var record = GetLastRecord(masterID);
			if (record != null) {
				RemoveShips(new int[] { masterID });
				var masterShip = KCDatabase.Instance.Ships[masterID].MasterShip;
				RecordManager.Instance.ResourceConsumption.Remodel(remodelDateTime, record.Destination, masterID, record.ShipID, masterShip.RemodelAmmo, masterShip.RemodelSteel);
			}
		}

		private void OnPowerUp(string apiname, dynamic data)
		{
			RemoveShips(((string)data["api_id_items"]).Split(',').Select(id => int.Parse(id)).ToArray());
		}

		private void OnCharge(string apiname, dynamic data)
		{
			DateTime chargeDateTime = DateTime.Now;
			var masterIDs = ((string)data["api_id_items"]).Split(',').Select(id => int.Parse(id)).ToArray();
			var chargeType = int.Parse((string)data["api_kind"]);
			ChargeToConsumptionRecord(chargeDateTime, masterIDs, chargeType);
			Record.RemoveAll(record => Cleanable(record));
			_changed = true;
		}

		private void ChargeToConsumptionRecord(DateTime chargeDateTime, int[] masterIDs, int chargeType = 0)
		{
			foreach (int masterID in masterIDs) {
				var chargedRecords = Record.Where(record => record.RecordStatus == 0 && record.MasterID == masterID);
				var chargedRecordsList = chargedRecords.ToList();
				int count = chargedRecordsList.Count;

				int[] chargedFuel = new int[count];
				int[] chargedAmmo = new int[count];
				int[] chargedBauxite = new int[count];

				int i;
				for (i = count - 1; i > 0; --i) {
					chargedFuel[i] = chargedRecordsList[i].UsedFuel - chargedRecordsList[i - 1].UsedFuel;
					chargedAmmo[i] = chargedRecordsList[i].UsedAmmo - chargedRecordsList[i - 1].UsedAmmo;
					chargedBauxite[i] = (chargedRecordsList[i].AircraftLost - chargedRecordsList[i - 1].AircraftLost) * 5;
				}
				{
					chargedFuel[0] = chargedRecordsList[0].UsedFuel;
					chargedAmmo[0] = chargedRecordsList[0].UsedAmmo;
					chargedBauxite[0] = chargedRecordsList[0].AircraftLost * 5;
				}

				i = 0;
				foreach (var record in chargedRecords) {
					switch (chargeType) {
						case 1: // Fuel Only
							RecordCharge(chargeDateTime, record, chargedFuel[i], 0, chargedBauxite[i]);
							record.UsedFuel = 0;
							break;
						case 2: // Ammo Only
							RecordCharge(chargeDateTime, record, 0, chargedAmmo[i], chargedBauxite[i]);
							record.UsedAmmo = 0;
							break;
						default: // Fuel & Ammo
							RecordCharge(chargeDateTime, record, chargedFuel[i], chargedAmmo[i], chargedBauxite[i]);
							record.UsedFuel = 0;
							record.UsedAmmo = 0;
							break;
					}
					record.AircraftLost = 0;
					++i;
				}

			}
		}

		private void OnRepair(string apiname, dynamic data)
		{
			DateTime repairDateTime = DateTime.Now;
			var masterID = int.Parse((string)data["api_ship_id"]);
			var dockID = int.Parse((string)data["api_ndock_id"]);
			var highspeed = int.Parse((string)data["api_highspeed"]);
			RepairToConsumptionRecord(repairDateTime, masterID, highspeed);
			Record.RemoveAll(record => Cleanable(record));
			_changed = true;
		}

		private void OnRepairSpeedUp(string apiname, dynamic data)
		{
			DateTime repairDateTime = DateTime.Now;
			int masterID = KCDatabase.Instance.Docks[int.Parse((string)data["api_ndock_id"])].PreviousShipID;
			int shipID = KCDatabase.Instance.Ships[masterID].ShipID;
			UseBucket(repairDateTime, masterID, shipID);
		}

		private void UseBucket(DateTime repairDateTime, int masterID, int shipID)
		{
			RecordManager.Instance.ResourceConsumption.UseBucket(repairDateTime, masterID, shipID);
		}

		private void RepairToConsumptionRecord(DateTime repairDateTime, int masterID, int highspeed = 0)
		{
			var repairedRecords = Record.Where(record => record.RecordStatus == 0 && record.MasterID == masterID);
			var repairedRecordsList = repairedRecords.ToList();
			int count = repairedRecordsList.Count;
			if (count == 0) return;

			int[] repairedFuel = new int[count];
			int[] repairedSteel = new int[count];

			int i;
			for (i = count - 1; i > 0; --i) {
				repairedFuel[i] = repairedRecordsList[i].RepairFuel - repairedRecordsList[i - 1].RepairFuel;
				repairedSteel[i] = repairedRecordsList[i].RepairSteel - repairedRecordsList[i - 1].RepairSteel;
			}
			{
				repairedFuel[0] = repairedRecordsList[0].RepairFuel;
				repairedSteel[0] = repairedRecordsList[0].RepairSteel;
			}

			i = 0;
			foreach (var record in repairedRecords) {
				RecordRepair(repairDateTime, record, repairedFuel[i], repairedSteel[i]);
				if (highspeed == 1 && i == count - 1) {
					// 最後の帰投記録のみに高速修復材の使用状況を計入
					UseBucket(repairDateTime, masterID, record.ShipID);
				}
				record.RepairFuel = 0;
				record.RepairSteel = 0;
				++i;
			}
		}

		private void RecordCharge(DateTime chargeDateTime, ResourcePreConsumptionElement record, int chargedFuel, int chargedAmmo, int chargedBauxite)
		{
			if (chargedFuel + chargedAmmo + chargedBauxite > 0)
				RecordManager.Instance.ResourceConsumption.Add(chargeDateTime, 1, record.Date, record.Destination, record.MasterID, record.ShipID, chargedFuel, chargedAmmo, 0, chargedBauxite, 0);
		}

		private void RecordRepair(DateTime repairDateTime, ResourcePreConsumptionElement record, int repairedFuel, int repairedSteel, int highspeed = 0)
		{
			if (repairedFuel + repairedSteel > 0)
				RecordManager.Instance.ResourceConsumption.Add(repairDateTime, 2, record.Date, record.Destination, record.MasterID, record.ShipID, repairedFuel, 0, repairedSteel, 0, highspeed);
		}

		private void RemoveShips(int[] masterIDs)
		{
			int recordCount = Record.Count;
			Record.RemoveAll(record => masterIDs.Contains(record.MasterID));
			if (Record.Count < recordCount)
				_changed = true;
		}

		private bool Cleanable(ResourcePreConsumptionElement record)
		{
			return (record.RecordStatus == 0 && record.UsedFuel + record.UsedAmmo + record.AircraftLost + record.RepairFuel + record.RepairSteel == 0);
		}

		public ResourcePreConsumptionElement this[int i]
		{
			get { return Record[i]; }
			set {
				Record[i] = value;
				_changed = true;
			}
		}

		private ResourcePreConsumptionElement GetLastRecord(int masterID)
		{
			for (int i = Record.Count - 1; i >= 0; --i) {
				if (Record[i].RecordStatus == 0 && Record[i].MasterID == masterID) {
					return Record[i];
				}
			}
			return null;
		}

		protected override void LoadLine(string line)
		{
			Record.Add(new ResourcePreConsumptionElement(line));
		}

		protected override string SaveLinesAll()
		{
			var sb = new StringBuilder();
			foreach (var elem in Record.OrderBy(r => r.Date).ThenBy(r => r.ShipID)) {
				sb.AppendLine(elem.SaveLine());
			}
			return sb.ToString();
		}

		protected override string SaveLinesPartial()
		{
			throw new NotSupportedException();
		}

		protected override void UpdateLastSavedIndex()
		{
			_changed = false;
		}

		public override bool NeedToSave
		{
			get { return _changed; }
		}

		public override bool SupportsPartialSave
		{
			get { return false; }
		}

		protected override void ClearRecord()
		{
			Record.Clear();
		}

		public override string RecordHeader
		{
			get { return "帰投日時,帰投前航路,艦娘固有ID,艦船ID,消耗燃料,消耗弾薬,損失艦載機数,入渠燃料,入渠鋼材,帰投状态"; }
		}

		public override string FileName
		{
			get { return "ResourcePreConsumptionRecord.csv"; }
		}


	}
}
