using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.Record {

	/// <summary>
	/// 資源のレコードを保持します。
	/// </summary>
	public class ResourceRecord : RecordBase {

		public class ResourceElement : RecordElementBase {

			/// <summary>
			/// 記録日時
			/// </summary>
			public DateTime Date { get; set; }

			/// <summary>
			/// 燃料
			/// </summary>
			public int Fuel { get; set; }

			/// <summary>
			/// 弾薬
			/// </summary>
			public int Ammo { get; set; }

			/// <summary>
			/// 鋼材
			/// </summary>
			public int Steel { get; set; }

			/// <summary>
			/// ボーキサイト
			/// </summary>
			public int Bauxite { get; set; }


			/// <summary>
			/// 高速建造材
			/// </summary>
			public int InstantConstruction { get; set; }

			/// <summary>
			/// 高速修復材
			/// </summary>
			public int InstantRepair { get; set; }

			/// <summary>
			/// 開発資材
			/// </summary>
			public int DevelopmentMaterial { get; set; }

			/// <summary>
			/// 改修資材
			/// </summary>
			public int ModdingMaterial { get; set; }

			/// <summary>
			/// 艦隊司令部Lv.
			/// </summary>
			public int HQLevel { get; set; }

			/// <summary>
			/// 提督経験値
			/// </summary>
			public int HQExp { get; set; }


			public ResourceElement() {
				Date = DateTime.Now;
			}

			public ResourceElement( string line )
				: base( line ) { }

			public ResourceElement( int fuel, int ammo, int steel, int bauxite, int instantConstruction, int instantRepair, int developmentMaterial, int moddingMaterial, int hqLevel, int hqExp )
				: this() {
				Fuel = fuel;
				Ammo = ammo;
				Steel = steel;
				Bauxite = bauxite;
				InstantConstruction = instantConstruction;
				InstantRepair = instantRepair;
				DevelopmentMaterial = developmentMaterial;
				ModdingMaterial = moddingMaterial;
				HQLevel = hqLevel;
				HQExp = hqExp;
			}

			public override void LoadLine( string line ) {

				string[] elem = line.Split( ",".ToCharArray() );
				if ( elem.Length < 11 ) throw new ArgumentException( "要素数が少なすぎます。" );

				Date = DateTimeHelper.CSVStringToTime( elem[0] );
				Fuel = int.Parse( elem[1] );
				Ammo = int.Parse( elem[2] );
				Steel = int.Parse( elem[3] );
				Bauxite = int.Parse( elem[4] );
				InstantConstruction = int.Parse( elem[5] );
				InstantRepair = int.Parse( elem[6] );
				DevelopmentMaterial = int.Parse( elem[7] );
				ModdingMaterial = int.Parse( elem[8] );
				HQLevel = int.Parse( elem[9] );
				HQExp = int.Parse( elem[10] );

			}

			public override string SaveLine() {
				return string.Format( "{" + string.Join( "},{", Enumerable.Range( 0, 11 ) ) + "}",
					DateTimeHelper.TimeToCSVString( Date ),
					Fuel,
					Ammo,
					Steel,
					Bauxite,
					InstantConstruction,
					InstantRepair,
					DevelopmentMaterial,
					ModdingMaterial,
					HQLevel,
					HQExp );
			}

		}


		public List<ResourceElement> Record { get; private set; }
		private DateTime _prevTime;
		private bool _initialFlag;
		private int LastSavedCount;


		public ResourceRecord()
			: base() {

			Record = new List<ResourceElement>();
			_prevTime = DateTime.Now;
			_initialFlag = false;
		}

		public override void RegisterEvents() {
			var ao = APIObserver.Instance;

			ao["api_start2"].ResponseReceived += ResourceRecord_Started;
			ao["api_port/port"].ResponseReceived += ResourceRecord_Updated;
		}


		private void ResourceRecord_Started( string apiname, dynamic data ) {
			_initialFlag = true;
		}


		void ResourceRecord_Updated( string apiname, dynamic data ) {

			if ( _initialFlag || DateTimeHelper.IsCrossedHour( _prevTime ) ) {
				_prevTime = DateTime.Now;
				_initialFlag = false;

				var material = KCDatabase.Instance.Material;
				var admiral = KCDatabase.Instance.Admiral;
				Record.Add( new ResourceElement(
					material.Fuel,
					material.Ammo,
					material.Steel,
					material.Bauxite,
					material.InstantConstruction,
					material.InstantRepair,
					material.DevelopmentMaterial,
					material.ModdingMaterial,
					admiral.Level,
					admiral.Exp ) );
			}
		}


		public ResourceElement this[int i] {
			get { return Record[i]; }
			set { Record[i] = value; }
		}


		/// <summary>
		/// 指定した日時以降の最も古い記録を返します。
		/// </summary>
		public ResourceElement GetRecord( DateTime target ) {
			if ( Record.Count == 0 || Record.Last().Date < target ) {
				return null;
			}
			for ( int i = Record.Count - 2; i >= 0; i-- ) {
				if ( Record[i].Date < target ) {
					return Record[i + 1];
				}
			}
			return Record[0];
		}

		/// <summary>
		/// 战果时间范围 (e.g. 4/30 22:00 ~ 5/1 14:00)
		/// </summary>
		public string RankingPeriodString { get; private set; }

		/// <summary>
		/// 每月战果作战名 (e.g. "五月作战")
		/// </summary>
		private int rankingMonth;
		public string MonthString { get {
			switch(rankingMonth) {
				case 1:
					return "一月作战";
				case 2:
					return "二月作战";
				case 3:
					return "三月作战";
				case 4:
					return "四月作战";
				case 5:
					return "五月作战";
				case 6:
					return "六月作战";
				case 7:
					return "七月作战";
				case 8:
					return "八月作战";
				case 9:
					return "九月作战";
				case 10:
					return "十月作战";
				case 11:
					return "十一月作战";
				case 12:
					return "十二月作战";
				default:
					return "战果黑洞"; // 年末 22:00 ~ 次年初 00:00 获得的经验值不会计入战果 * not used
			}
		} }

		private TimeSpan timeZoneOffset = DateTimeOffset.Now.Offset - new TimeSpan(9, 0, 0);

		/// <summary>
		/// 半日提督经验 ( previous = true 时返回上次结算总经验，否则返回上次结算时经验值 )
		/// </summary>
		public int GetExpHalfDay(DateTime now, bool previous = false) {
			// 确定日期 ( date 的时间仅以 02:00 / 14:00 表示上午 / 下午，并不一定是记录的起始时间 )
			DateTime date;
			if (now.Hour < 2) {
				if (now.Day == 1) {
					date = previous ? new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(-1) : new DateTime(now.Year, now.Month, now.Day, 2, 0, 0);
				} else {
					date = previous ? new DateTime(now.Year, now.Month, now.Day, 2, 0, 0).AddDays(-1) : new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(-1);
				}
			} else if (now.Hour < 14) {
				date = previous ? new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(-1) : new DateTime(now.Year, now.Month, now.Day, 2, 0, 0);
			} else {
				if (now.Hour >= 22 && now.Day == DateTime.DaysInMonth(now.Year, now.Month)) {
					if (previous) {
						date = new DateTime(now.Year, now.Month, now.Day, 14, 0, 0);
					} else {
						if (now.Month == 12) {
							date = new DateTime(2013, 4, 23, 14, 0, 0); // 『艦これ』开始运营日期，时间是假的
						} else {
							date = new DateTime(now.Year, now.Month, now.Day, 2, 0, 0).AddDays(1);
						}
					}
				} else {
					date = previous ? new DateTime(now.Year, now.Month, now.Day, 2, 0, 0) : new DateTime(now.Year, now.Month, now.Day, 14, 0, 0);
				}
			}
			// 确定记录范围
			DateTime begins; DateTime ends;
			if (date.Hour < 14) {
				if (date.Day == 1) {
					if (date.DayOfYear == 1) {
						// 年初上午：00:00 ~ 14:00 (14h)
						begins = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
					} else {
						// 月初上午：上月末 22:00 ~ 今日 14:00 (16h)
						begins = new DateTime(date.Year, date.Month, date.Day, 22, 0, 0).AddDays(-1);
					}
				} else {
					// 一般上午：02:00 ~ 14:00 (12h)
					begins = date;
				}
				ends = new DateTime(date.Year, date.Month, date.Day, 14, 0, 0);
			} else {
				// 判断年末战果黑洞
				if (date.Year == 2013) { return 0; }
				begins = date;
				if (date.Day == DateTime.DaysInMonth(date.Year, date.Month)) {
					// 月末下午：14:00 ~ 22:00 (8h)
					ends = new DateTime(date.Year, date.Month, date.Day, 22, 0, 0);
				} else {
					// 一般下午：14:00 ~ 次日 02:00 (12h)
					ends = new DateTime(date.Year, date.Month, date.Day, 2, 0, 0).AddDays(1);
				}
			}
			rankingMonth = date.Month;
			// 修正时区
			begins += timeZoneOffset; ends += timeZoneOffset;
			// 返回记录值
			var recordBegins = GetRecord(begins);
			var recordEnds = GetRecord(ends);
			if (recordBegins != null) {
				if (begins.Date == ends.Date) {
					RankingPeriodString = string.Format("{0} ~ {1}", begins.ToString("M'/'d HH':'mm"), ends.ToString("HH':'mm"));
				} else {
					RankingPeriodString = string.Format("{0} ~ {1}", begins.ToString("M'/'d HH':'mm"), ends.ToString("M'/'d HH':'mm"));
				}
				if (previous && recordEnds != null) {
					return recordEnds.HQExp - recordBegins.HQExp;
				} else {
					return recordBegins.HQExp;
				}
			}
			return -1;
		}

		/// <summary>
		/// 单日提督经验 ( previous = true 时返回昨日总经验，否则返回本日初经验值 )
		/// </summary>
		public int GetExpDay(DateTime now, bool previous = false) {
			// 确定日期
			DateTime date;
			if (now.Day != 1 && now.Hour < 2) {
				date = previous ? now.AddDays(-2) : now.AddDays(-1);
			} else if (now.Day == DateTime.DaysInMonth(now.Year, now.Month) && now.Hour >= 22) {
				date = previous ? now : now.AddDays(1);
			} else {
				date = previous ? now.AddDays(-1) : now;
			}
			// 确定记录范围
			DateTime begins; DateTime ends;
			if (date.Day == 1) {
				if (date.DayOfYear == 1) {
					// 年初：1/1 00:00 ~ 1/2 02:00 (26h)
					begins = new DateTime(date.Year, 1, 1, 0, 0, 0);
					ends = new DateTime(date.Year, 1, 2, 2, 0, 0);
				} else {
					// 月初：上月末 22:00 ~ 次日(2nd) 02:00 (28h)
					begins = new DateTime(date.Year, date.Month, 1, 22, 0, 0).AddDays(-1);
					ends = new DateTime(date.Year, date.Month, 2, 2, 0, 0);
				}
			} else {
				begins = new DateTime(date.Year, date.Month, date.Day, 2, 0, 0);
				if (date.Day == DateTime.DaysInMonth(date.Year, date.Month)) {
					// 月末：02:00 ~ 22:00 (20h)
					ends = new DateTime(date.Year, date.Month, date.Day, 22, 0, 0);
				} else {
					// 一般：02:00 ~ 次日 02:00 (24h)
					ends = new DateTime(date.Year, date.Month, date.Day, 2, 0, 0).AddDays(1);
				}
			}
			// 修正时区
			begins += timeZoneOffset; ends += timeZoneOffset;
			// 返回记录值
			var recordBegins = GetRecord(begins);
			var recordEnds = GetRecord(ends);
			if (recordBegins != null) {
				if (begins.Date == ends.Date) {
					RankingPeriodString = string.Format("{0} ~ {1}", begins.ToString("M'/'d HH':'mm"), ends.ToString("HH':'mm"));
				} else {
					RankingPeriodString = string.Format("{0} ~ {1}", begins.ToString("M'/'d HH':'mm"), ends.ToString("M'/'d HH':'mm"));
				}
				if (previous && recordEnds != null) {
					return recordEnds.HQExp - recordBegins.HQExp;
				} else {
					return recordBegins.HQExp;
				}
			}
			return -1;
		}

		/// <summary>
		/// 单月提督经验 ( previous = true 时返回上月总经验，否则返回本月初经验值 )
		/// </summary>
		public int GetExpMonth(DateTime now, bool previous = false) {
			// 确定月份
			DateTime date;
			if (now.Hour < 22) {
				date = previous ? now.AddMonths(-1) : now;
			} else {
				date = previous ? now.AddDays(1).AddMonths(-1) : now.AddDays(1);
			}
			// 确定记录范围
			DateTime begins; DateTime ends;
			if (date.Month == 1) {
				// 一月：1/1 00:00 ~ 1/31 22:00
				begins = new DateTime(date.Year, 1, 1, 0, 0, 0);
				ends   = new DateTime(date.Year, 1, 31, 22, 0, 0);
			} else {
				// 一般：上月末 22:00 ~ 本月末 22：00
				begins = new DateTime(date.Year, date.Month, 1, 0, 0, 0).AddHours(-2);
				ends   = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 22, 0, 0);
			}
			// 修正时区
			begins += timeZoneOffset; ends += timeZoneOffset;
			// 返回记录值
			var recordBegins = GetRecord(begins);
			var recordEnds = GetRecord(ends);
			if (recordBegins != null) {
				RankingPeriodString = string.Format("{0} ~ {1}", begins.ToString("M'/'d HH':'mm"), ends.ToString("M'/'d HH':'mm"));
				if (previous && recordEnds != null) {
					return recordEnds.HQExp - recordBegins.HQExp;
				} else {
					return recordBegins.HQExp;
				}
			}
			return -1;
		}




		protected override void LoadLine( string line ) {
			Record.Add( new ResourceElement( line ) );
		}

		protected override string SaveLinesAll() {
			var sb = new StringBuilder();
			foreach ( var elem in Record.OrderBy( r => r.Date ) ) {
				sb.AppendLine( elem.SaveLine() );
			}
			return sb.ToString();
		}

		protected override string SaveLinesPartial() {
			var sb = new StringBuilder();
			foreach ( var elem in Record.Skip( LastSavedCount ).OrderBy( r => r.Date ) ) {
				sb.AppendLine( elem.SaveLine() );
			}
			return sb.ToString();
		}

		protected override void UpdateLastSavedIndex() {
			LastSavedCount = Record.Count;
		}

		public override bool NeedToSave {
			get { return LastSavedCount < Record.Count; }
		}

		public override bool SupportsPartialSave {
			get { return true; }
		}

		protected override void ClearRecord() {
			Record.Clear();
			LastSavedCount = 0;
		}



		public override string RecordHeader {
			get { return "日時,燃料,弾薬,鋼材,ボーキ,高速建造材,高速修復材,開発資材,改修資材,司令部Lv,提督Exp"; }
		}

		public override string FileName {
			get { return "ResourceRecord.csv"; }
		}


	}
}
