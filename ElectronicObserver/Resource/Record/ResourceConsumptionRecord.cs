using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronicObserver.Resource.Record
{

	/// <summary>
	/// 資源消費のレコードを保持します。
	/// 基地航空隊はまだ未対応
	/// </summary>
	public class ResourceConsumptionRecord : RecordBase
	{

		public sealed class ResourceConsumptionElement : RecordElementBase
		{

			/// <summary>
			/// 記録日時
			/// </summary>
			public DateTime Date { get; set; }

			/// <summary>
			/// 消費理由 ( 1 = 補給 / 2 = 入渠 / 3 = 改造 )
			/// </summary>
			public int RecordType { get; set; }

			/// <summary>
			/// 帰投日時
			/// </summary>
			public DateTime ReturnDateTime { get; set; }

			/// <summary>
			/// 帰投前航路（セル）eg: 6-5-13-BOSS / practice / expedition-6
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
			/// 燃料 ( 補給、入渠 )
			/// </summary>
			public int Fuel { get; set; }

			/// <summary>
			/// 弾薬 ( 補給 )
			/// </summary>
			public int Ammo { get; set; }

			/// <summary>
			/// 鋼材 ( 入渠 )
			/// </summary>
			public int Steel { get; set; }

			/// <summary>
			/// ボーキサイト ( 補給 )
			/// </summary>
			public int Bauxite { get; set; }

			/// <summary>
			/// 高速修復材 ( 入渠 )
			/// 複数回の出撃でまとめ入渠の場合、一番最後の帰投に計入。
			/// </summary>
			public int InstantRepair { get; set; }

			public ResourceConsumptionElement() { }

			public ResourceConsumptionElement(string line)
				: this()
			{
				LoadLine(line);
			}

			public ResourceConsumptionElement(DateTime date, int recordType, DateTime returnDateTime, string destination, int masterID, int shipID, int fuel, int ammo, int steel, int bauxite, int instantRepair)
				: this()
			{
				Date = date;
				RecordType = recordType;
				ReturnDateTime = returnDateTime;
				Destination = destination;
				MasterID = masterID;
				ShipID = shipID;
				Fuel = fuel;
				Ammo = ammo;
				Steel = steel;
				Bauxite = bauxite;
				InstantRepair = instantRepair;
			}

			public override void LoadLine(string line)
			{

				string[] elem = line.Split(",".ToCharArray());
				if (elem.Length < 11) throw new ArgumentException("要素数が少なすぎます。");

				Date = DateTimeHelper.CSVStringToTime(elem[0]);
				RecordType = int.Parse(elem[1]);
				ReturnDateTime = DateTimeHelper.CSVStringToTime(elem[2]);
				Destination = elem[3];
				MasterID = int.Parse(elem[4]);
				ShipID = int.Parse(elem[5]);
				Fuel = int.Parse(elem[6]);
				Ammo = int.Parse(elem[7]);
				Steel = int.Parse(elem[8]);
				Bauxite = int.Parse(elem[9]);
				InstantRepair = int.Parse(elem[10]);

			}

			public override string SaveLine()
			{
				return string.Join(",",
					DateTimeHelper.TimeToCSVString(Date),
					RecordType,
					DateTimeHelper.TimeToCSVString(ReturnDateTime),
					Destination,
					MasterID,
					ShipID,
					Fuel,
					Ammo,
					Steel,
					Bauxite,
					InstantRepair);
			}

		}


		public List<ResourceConsumptionElement> Record { get; private set; }
		private int LastSavedCount;


		public ResourceConsumptionRecord()
			: base()
		{

			Record = new List<ResourceConsumptionElement>();
		}

		public ResourceConsumptionElement this[int i]
		{
			get { return Record[i]; }
			set { Record[i] = value; }
		}

		public void Add(DateTime date, int recordType, DateTime returnDateTime, string destination, int masterID, int shipID, int fuel, int ammo, int steel, int bauxite, int instantRepair)
		{
			Record.Add(new ResourceConsumptionElement(date, recordType, returnDateTime, destination, masterID, shipID, fuel, ammo, steel, bauxite, instantRepair));
		}

		public void UseBucket(DateTime date, int masterID, int shipID)
		{
			DateTime returnDateTime;
			string destination;
			int lastRecordID = GetLastRecordID(2, masterID);
			if (lastRecordID == -1) {
				returnDateTime = date;
				destination = "unknown-bucket";
			} else {
				var record = Record[lastRecordID];
				if (record.InstantRepair != 0) {
					returnDateTime = date;
					destination = "unknown-bucket";
				} else {
					if (record.Date == date) {
						Record[lastRecordID].InstantRepair = 1;
						return;
					} else {
						returnDateTime = record.ReturnDateTime;
						destination = record.Destination;
					}
				}
			}
			Record.Add(new ResourceConsumptionElement(date, 2, returnDateTime, destination, masterID, shipID, 0, 0, 0, 0, 1));
		}

		public void Remodel(DateTime date, string destination, int masterID, int shipID, int remodelAmmo, int remodelSteel)
		{
			Record.Add(new ResourceConsumptionElement(date, 3, date, destination, masterID, shipID, 0, remodelAmmo, remodelSteel, 0, 0));
		}

		private int GetLastRecordID(int recordType, int masterID)
		{
			if (Utility.Configuration.Config.UI.RCR_BucketSkip1_1) {
				for (int i = Record.Count - 1; i >= 0; --i) {
					if (Record[i].RecordType == recordType && Record[i].MasterID == masterID) {
						if (Record[i].Destination.StartsWith("1-1-")) {
							continue;
						} else {
							return i;
						}
					}
				}
			}
			for (int i = Record.Count - 1; i >= 0; --i) {
				if (Record[i].RecordType == recordType && Record[i].MasterID == masterID) {
					return i;
				}
			}
			return -1;
		}

		protected override void LoadLine(string line)
		{
			Record.Add(new ResourceConsumptionElement(line));
		}

		protected override string SaveLinesAll()
		{
			var sb = new StringBuilder();
			foreach (var elem in Record.OrderBy(r => r.Date)) {
				sb.AppendLine(elem.SaveLine());
			}
			return sb.ToString();
		}

		protected override string SaveLinesPartial()
		{
			var sb = new StringBuilder();
			foreach (var elem in Record.Skip(LastSavedCount).OrderBy(r => r.Date)) {
				sb.AppendLine(elem.SaveLine());
			}
			return sb.ToString();
		}

		protected override void UpdateLastSavedIndex()
		{
			LastSavedCount = Record.Count;
		}

		public override bool NeedToSave
		{
			get { return LastSavedCount < Record.Count; }
		}

		public override bool SupportsPartialSave
		{
			get { return true; }
		}

		protected override void ClearRecord()
		{
			Record.Clear();
			LastSavedCount = 0;
		}

		public override void RegisterEvents()
		{
			// Nothing
		}

		public override string RecordHeader
		{
			get { return "記録日時,消費理由,帰投日時,帰投前航路,艦娘固有ID,艦船ID,燃料,弾薬,鋼材,ボーキ,高速修復材"; }
		}

		public override string FileName
		{
			get { return "ResourceConsumptionRecord.csv"; }
		}


	}
}
