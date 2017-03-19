using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.ShipGroup {

	/// <summary>
	/// 艦船フィルタの式データ
	/// </summary>
	[DataContract( Name = "ExpressionData" )]
	public class ExpressionData : ICloneable {

		public enum ExpressionOperator {
			Equal,
			NotEqual,
			LessThan,
			LessEqual,
			GreaterThan,
			GreaterEqual,
			Contains,
			NotContains,
			BeginWith,
			NotBeginWith,
			EndWith,
			NotEndWith,
			ArrayContains,
			ArrayNotContains,
		}


		[DataMember]
		public string LeftOperand { get; set; }

		[DataMember]
		public ExpressionOperator Operator { get; set; }

		[DataMember]
		public object RightOperand { get; set; }


		[DataMember]
		public bool Enabled { get; set; }


		[IgnoreDataMember]
		private static readonly Regex regex_index = new Regex( @"\.(?<name>\w+)(\[(?<index>\d+?)\])?", RegexOptions.Compiled );

		[IgnoreDataMember]
		public static readonly Dictionary<string, string> LeftOperandNameTable = new Dictionary<string, string>() {
			{ ".MasterID", "舰船固有ID" },
			{ ".ShipID", "舰船ID" },
			{ ".MasterShip.NameWithClass", "舰名" },
			{ ".MasterShip.ShipType", "舰种" },
			{ ".Level", "等级" },
			{ ".ExpTotal", "经验值" },
			{ ".ExpNext", "距离升级" },
			{ ".ExpNextRemodel", "距离改装" },
			{ ".HPCurrent", "现在 HP" },
			{ ".HPMax", "最大 HP" },
			{ ".HPRate", "HP 比例" },
			{ ".Condition", "疲劳值" },
			{ ".AllSlotMaster", "装备" },
			{ ".SlotMaster[0]", "装备 #1" },	//checkme: 要る?
			{ ".SlotMaster[1]", "装备 #2" },
			{ ".SlotMaster[2]", "装备 #3" },
			{ ".SlotMaster[3]", "装备 #4" },
			{ ".SlotMaster[4]", "装备 #5" },
			{ ".ExpansionSlotMaster", "补强装备" },
			{ ".Aircraft[0]", "搭载 #1" },
			{ ".Aircraft[1]", "搭载 #2" },
			{ ".Aircraft[2]", "搭载 #3" },
			{ ".Aircraft[3]", "搭载 #4" },
			{ ".Aircraft[4]", "搭载 #5" },
			{ ".AircraftTotal", "搭载机数合计" },
			{ ".MasterShip.Aircraft[0]", "最大搭载 #1" },
			{ ".MasterShip.Aircraft[1]", "最大搭载 #2" },
			{ ".MasterShip.Aircraft[2]", "最大搭载 #3" },
			{ ".MasterShip.Aircraft[3]", "最大搭载 #4" },
			{ ".MasterShip.Aircraft[4]", "最大搭载 #5" },
			{ ".MasterShip.AircraftTotal", "最大搭载机数" },		//要る？
			{ ".AircraftRate[0]", "搭载比例 #1" },
			{ ".AircraftRate[1]", "搭载比例 #2" },
			{ ".AircraftRate[2]", "搭载比例 #3" },
			{ ".AircraftRate[3]", "搭载比例 #4" },
			{ ".AircraftRate[4]", "搭载比例 #5" },
			{ ".AircraftTotalRate", "搭载比例合计" },
			{ ".Fuel", "搭载燃料" },
			{ ".Ammo", "搭载弹药" },
			{ ".FuelMax", "最大搭载燃料" },
			{ ".AmmoMax", "最大搭载弹药" },
			{ ".FuelRate", "搭载燃料比例" },
			{ ".AmmoRate", "搭载弹药比例" },
			{ ".SlotSize", "装备格数" },
			{ ".RepairingDockID", "入渠船坞" },
			{ ".RepairTime", "入渠时间" },
			{ ".RepairSteel", "入渠消费钢材" },
			{ ".RepairFuel", "入渠消费燃料" },
			//強化値シリーズは省略
			{ ".FirepowerBase", "基本火力" },
			{ ".TorpedoBase", "基本雷装" },
			{ ".AABase", "基本对空" },
			{ ".ArmorBase", "基本装甲" },
			{ ".EvasionBase", "基本回避" },
			{ ".ASWBase", "基本对潜" },
			{ ".LOSBase", "基本索敌" },
			{ ".LuckBase", "基本运" },
			{ ".FirepowerTotal", "合计火力" },
			{ ".TorpedoTotal", "合计雷装" },
			{ ".AATotal", "合计对空" },
			{ ".ArmorTotal", "合计装甲" },
			{ ".EvasionTotal", "合计回避" },
			{ ".ASWTotal", "合计对潜" },
			{ ".LOSTotal", "合计索敌" },
			{ ".LuckTotal", "合计运" },
			{ ".BomberTotal", "合计爆装" },
			{ ".FirepowerRemain", "火力改修剩余" },
			{ ".TorpedoRemain", "雷装改修剩余" },
			{ ".AARemain", "对空改修剩余" },
			{ ".ArmorRemain", "装甲改修剩余" },
			{ ".LuckRemain", "运改修剩余" },
			{ ".Range", "射程" },		//現在の射程
			{ ".Speed", "速度" },
			{ ".MasterShip.Speed", "基础速度" },
			{ ".MasterShip.Rarity", "稀有度" },
			{ ".IsLocked", "锁定" },
			{ ".IsLockedByEquipment", "有带锁装备" },
			{ ".SallyArea", "出击海域" },
			{ ".FleetWithIndex", "所属舰队" },
			{ ".InExpedition", "远征中" },
			{ ".IsMarried", "结婚" },
			{ ".AirBattlePower", "航空威力" },
			{ ".ShellingPower", "炮击威力" },
			{ ".AircraftPower", "空袭威力" },
			{ ".AntiSubmarinePower", "对潜威力" },
			{ ".TorpedoPower", "雷击威力" },
			{ ".NightBattlePower", "夜战威力" },
			{ ".MasterShip.AlbumNo", "图鉴编号" },
			{ ".MasterShip.NameReading", "舰名读法" },
			{ ".MasterShip.RemodelBeforeShipID", "改装前舰船 ID" },
			{ ".MasterShip.RemodelAfterShipID", "改装后舰船 ID" },
			//マスターのパラメータ系もおそらく意味がないので省略		
		};

		private static Dictionary<string, Type> ExpressionTypeTable = new Dictionary<string, Type>();


		[IgnoreDataMember]
		public static readonly Dictionary<ExpressionOperator, string> OperatorNameTable = new Dictionary<ExpressionOperator, string>() {
			{ ExpressionOperator.Equal, "等于" },
			{ ExpressionOperator.NotEqual, "不等于" },
			{ ExpressionOperator.LessThan, "小于" },
			{ ExpressionOperator.LessEqual, "小于等于" },
			{ ExpressionOperator.GreaterThan, "大于" },
			{ ExpressionOperator.GreaterEqual, "大于等于" },
			{ ExpressionOperator.Contains, "包括" },
			{ ExpressionOperator.NotContains, "不包括" },
			{ ExpressionOperator.BeginWith, "开头为" },
			{ ExpressionOperator.NotBeginWith, "开头不为" },
			{ ExpressionOperator.EndWith, "结尾是" },
			{ ExpressionOperator.NotEndWith, "结尾不是" },
			{ ExpressionOperator.ArrayContains, "包含" },
			{ ExpressionOperator.ArrayNotContains, "不包含" },
			
		};



		public ExpressionData() {
			Enabled = true;
		}

		public ExpressionData( string left, ExpressionOperator ope, object right )
			: this() {
			LeftOperand = left;
			Operator = ope;
			RightOperand = right;
		}


		public Expression Compile( ParameterExpression paramex ) {

			Expression memberex = null;
			Expression constex = Expression.Constant( RightOperand, RightOperand.GetType() );

			{
				Match match = regex_index.Match( LeftOperand );
				if ( match.Success ) {

					do {

						if ( memberex == null ) {
							memberex = Expression.PropertyOrField( paramex, match.Groups["name"].Value );
						} else {
							memberex = Expression.PropertyOrField( memberex, match.Groups["name"].Value );
						}

						int index;
						if ( int.TryParse( match.Groups["index"].Value, out index ) ) {
							memberex = Expression.Property( memberex, "Item", Expression.Constant( index, typeof( int ) ) );
						}

					} while ( ( match = match.NextMatch() ).Success );

				} else {
					memberex = Expression.PropertyOrField( paramex, LeftOperand );
				}
			}

			Expression  condex;
			switch ( Operator ) {
				case ExpressionOperator.Equal:
					condex = Expression.Equal( memberex, constex );
					break;
				case ExpressionOperator.NotEqual:
					condex = Expression.NotEqual( memberex, constex );
					break;
				case ExpressionOperator.LessThan:
					condex = Expression.LessThan( memberex, constex );
					break;
				case ExpressionOperator.LessEqual:
					condex = Expression.LessThanOrEqual( memberex, constex );
					break;
				case ExpressionOperator.GreaterThan:
					condex = Expression.GreaterThan( memberex, constex );
					break;
				case ExpressionOperator.GreaterEqual:
					condex = Expression.GreaterThanOrEqual( memberex, constex );
					break;
				case ExpressionOperator.Contains:
					condex = Expression.Call( memberex, typeof( string ).GetMethod( "Contains", new Type[] { typeof( string ) } ), constex );
					break;
				case ExpressionOperator.NotContains:
					condex = Expression.Not( Expression.Call( memberex, typeof( string ).GetMethod( "Contains", new Type[] { typeof( string ) } ), constex ) );
					break;
				case ExpressionOperator.BeginWith:
					condex = Expression.Equal( Expression.Call( memberex, typeof( string ).GetMethod( "IndexOf", new Type[] { typeof( string ) } ), constex ), Expression.Constant( 0, typeof( int ) ) );
					break;
				case ExpressionOperator.NotBeginWith:
					condex = Expression.NotEqual( Expression.Call( memberex, typeof( string ).GetMethod( "IndexOf", new Type[] { typeof( string ) } ), constex ), Expression.Constant( 0, typeof( int ) ) );
					break;
				case ExpressionOperator.EndWith:	// returns memberex.LastIndexOf( constex ) == ( memberex.Length - constex.Length )
					condex = Expression.Equal(
						Expression.Call( memberex, typeof( string ).GetMethod( "LastIndexOf", new Type[] { typeof( string ) } ), constex ),
						Expression.Subtract( Expression.PropertyOrField( memberex, "Length" ), Expression.PropertyOrField( constex, "Length" ) ) );
					break;
				case ExpressionOperator.NotEndWith:	// returns memberex.LastIndexOf( constex ) != ( memberex.Length - constex.Length )
					condex = Expression.NotEqual(
						Expression.Call( memberex, typeof( string ).GetMethod( "LastIndexOf", new Type[] { typeof( string ) } ), constex ),
						Expression.Subtract( Expression.PropertyOrField( memberex, "Length" ), Expression.PropertyOrField( constex, "Length" ) ) );
					break;
				case ExpressionOperator.ArrayContains:	// returns Enumerable.Contains<>( memberex )
					condex = Expression.Call( typeof( Enumerable ), "Contains", new Type[] { memberex.Type.GetElementType() ?? memberex.Type.GetGenericArguments().First() }, memberex, constex );
					break;
				case ExpressionOperator.ArrayNotContains:	// returns !Enumerable.Contains<>( memberex )
					condex = Expression.Not( Expression.Call( typeof( Enumerable ), "Contains", new Type[] { memberex.Type.GetElementType() ?? memberex.Type.GetGenericArguments().First() }, memberex, constex ) );
					break;

				default:
					throw new NotImplementedException();
			}

			return condex;
		}



		public static Type GetLeftOperandType( string left ) {

			if ( ExpressionTypeTable.ContainsKey( left ) ) {
				return ExpressionTypeTable[left];

			} else if ( KCDatabase.Instance.Ships.Count > 0 ) {

				object obj = KCDatabase.Instance.Ships.Values.First();

				Match match = regex_index.Match( left );
				if ( match.Success ) {

					do {

						int index;
						if ( int.TryParse( match.Groups["index"].Value, out index ) ) {
							obj = ( (dynamic)obj.GetType().InvokeMember( match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null ) )[index];
						} else {
							object obj2 = obj.GetType().InvokeMember( match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null );
							if ( obj2 == null ) {	//プロパティはあるけどnull
								var type = obj.GetType().GetProperty( match.Groups["name"].Value ).GetType();
								ExpressionTypeTable.Add( left, type );
								return type;
							} else {
								obj = obj2;
							}
						}

					} while ( obj != null && ( match = match.NextMatch() ).Success );


					if ( obj != null ) {
						ExpressionTypeTable.Add( left, obj.GetType() );
						return obj.GetType();
					}
				}

			}

			return null;
		}

		public Type GetLeftOperandType() {
			return GetLeftOperandType( LeftOperand );
		}



		public override string ToString() {
			return string.Format( "{0} {2} {1} ", LeftOperandToString(), RightOperandToString(), OperatorToString() );
		}


		/// <summary>
		/// 左辺値の文字列表現を求めます。
		/// </summary>
		public string LeftOperandToString() {
			if ( LeftOperandNameTable.ContainsKey( LeftOperand ) )
				return LeftOperandNameTable[LeftOperand];
			else
				return LeftOperand;
		}

		/// <summary>
		/// 演算子の文字列表現を求めます。
		/// </summary>
		public string OperatorToString() {
			return OperatorNameTable[Operator];
		}

		/// <summary>
		/// 右辺値の文字列表現を求めます。
		/// </summary>
		public string RightOperandToString() {

			if ( LeftOperand == ".MasterID" ) {
				var ship = KCDatabase.Instance.Ships[(int)RightOperand];
				if ( ship != null )
					return string.Format( "{0} ({1})", ship.MasterID, ship.NameWithLevel );
				else
					return string.Format( "{0} ( 未在籍 )", (int)RightOperand );

			} else if ( LeftOperand == ".ShipID" ) {
				var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
				if ( ship != null )
					return string.Format( "{0} ({1})", ship.ShipID, ship.NameWithClass );
				else
					return string.Format( "{0} ( 不存在 )", (int)RightOperand );

			} else if ( LeftOperand == ".MasterShip.ShipType" ) {
				var shiptype = KCDatabase.Instance.ShipTypes[(int)RightOperand];
				if ( shiptype != null )
					return shiptype.Name;
				else
					return string.Format( "{0} ( 未定义 )", (int)RightOperand );

			} else if ( LeftOperand.Contains( "SlotMaster" ) ) {
				if ( (int)RightOperand == -1 ) {
					return "( 无 )";
				} else {
					var eq = KCDatabase.Instance.MasterEquipments[(int)RightOperand];
					if ( eq != null )
						return eq.Name;
					else
						return string.Format( "{0} ( 未定义 )", (int)RightOperand );
				}
			} else if ( LeftOperand.Contains( "Rate" ) && RightOperand is double ) {
				return ( (double)RightOperand ).ToString( "P0" );

			} else if ( LeftOperand == ".RepairTime" ) {
				return DateTimeHelper.ToTimeRemainString( DateTimeHelper.FromAPITimeSpan( (int)RightOperand ) );

			} else if ( LeftOperand == ".Range" ) {
				return Constants.GetRange( (int)RightOperand );

			} else if ( LeftOperand == ".Speed" || LeftOperand == ".MasterShip.Speed" ) {
				return Constants.GetSpeed( (int)RightOperand );

			} else if ( LeftOperand == ".MasterShip.Rarity" ) {
				return Constants.GetShipRarity( (int)RightOperand );

			} else if ( LeftOperand == ".MasterShip.AlbumNo" ) {
				var ship = KCDatabase.Instance.MasterShips.Values.FirstOrDefault( s => s.AlbumNo == (int)RightOperand );
				if ( ship != null )
					return string.Format( "{0} ({1})", (int)RightOperand, ship.NameWithClass );
				else
					return string.Format( "{0} ( 不存在 )", (int)RightOperand );

			} else if ( LeftOperand == ".MasterShip.RemodelAfterShipID" ) {

				if ( ( (int)RightOperand ) == 0 )
					return "最终改装";

				var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
				if ( ship != null )
					return string.Format( "{0} ({1})", ship.ShipID, ship.NameWithClass );
				else
					return string.Format( "{0} ( 不存在 )", (int)RightOperand );

			} else if ( LeftOperand == ".MasterShip.RemodelBeforeShipID" ) {

				if ( ( (int)RightOperand ) == 0 )
					return "未改装";

				var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
				if ( ship != null )
					return string.Format( "{0} ({1})", ship.ShipID, ship.NameWithClass );
				else
					return string.Format( "{0} ( 不存在 )", (int)RightOperand );

			} else if ( RightOperand is bool ) {
				return ( (bool)RightOperand ) ? "〇" : "✕";

			} else {
				return RightOperand.ToString();

			}

		}


		public ExpressionData Clone() {
			var clone = MemberwiseClone();		//checkme: 右辺値に参照型を含む場合死ぬ
			return (ExpressionData)clone;
		}

		object ICloneable.Clone() {
			return Clone();
		}
	}




}
