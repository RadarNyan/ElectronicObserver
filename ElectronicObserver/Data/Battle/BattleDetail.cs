﻿using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {
	/// <summary>
	/// 戦闘詳細のデータを保持します。
	/// </summary>
	public abstract class BattleDetail {

		public int[] Damages { get; protected set; }
		public CriticalType[] CriticalTypes { get; protected set; }
		public int AttackType { get; protected set; }

		public ShipDataMaster Attacker { get; protected set; }
		public ShipDataMaster Defender { get; protected set; }


		/// <summary>
		/// 攻撃側インデックス [0-23]
		/// </summary>
		public int AttackerIndex { get; protected set; }

		/// <summary>
		/// 防御側インデックス [0-23]
		/// </summary>
		public int DefenderIndex { get; protected set; }


		public enum CriticalType {
			Miss = 0,
			Hit = 1,
			Critical = 2,
			Invalid = -1
		}


		/// <param name="bd">戦闘情報。</param>
		/// <param name="attackerIndex">攻撃側のインデックス。 [0-23]</param>
		/// <param name="defenderIndex">防御側のインデックス。 [0-23]</param>
		/// <param name="damages">ダメージの配列。</param>
		/// <param name="criticalTypes">命中判定の配列。</param>
		/// <param name="attackType">攻撃種別。</param>
		public BattleDetail( BattleData bd, int attackerIndex, int defenderIndex, int[] damages, int[] criticalTypes, int attackType ) {

			AttackerIndex = attackerIndex;
			DefenderIndex = defenderIndex;
			Damages = damages;
			CriticalTypes = criticalTypes.Select( i => (CriticalType)i ).ToArray();
			AttackType = attackType;


			int[] slots;

			if ( AttackerIndex < 0 ) {
				Attacker = null;
				slots = null;

			} else if ( AttackerIndex < 6 ) {
				var atk = bd.Initial.FriendFleet.MembersInstance[AttackerIndex];
				Attacker = atk.MasterShip;
				slots = atk.SlotMaster.ToArray();

			} else if ( AttackerIndex < 12 ) {
				Attacker = bd.Initial.EnemyMembersInstance[AttackerIndex - 6];
				slots = bd.Initial.EnemySlots[AttackerIndex - 6];

			} else if ( attackerIndex < 18 ) {
				var atk = bd.Initial.FriendFleetEscort.MembersInstance[AttackerIndex - 12];
				Attacker = atk.MasterShip;
				slots = atk.SlotMaster.ToArray();

			} else {
				Attacker = bd.Initial.EnemyMembersEscortInstance[AttackerIndex - 18];
				slots = bd.Initial.EnemySlotsEscort[AttackerIndex - 18];

			}


			if ( DefenderIndex < 6 )
				Defender = bd.Initial.FriendFleet.MembersInstance[DefenderIndex].MasterShip;
			else if ( DefenderIndex < 12 )
				Defender = bd.Initial.EnemyMembersInstance[DefenderIndex - 6];
			else if ( DefenderIndex < 18 )
				Defender = bd.Initial.FriendFleetEscort.MembersInstance[DefenderIndex - 12].MasterShip;
			else
				Defender = bd.Initial.EnemyMembersEscortInstance[DefenderIndex - 18];

			if ( AttackType == 0 && Attacker != null ) {
				AttackType = CaclulateAttackKind( slots, Attacker.ShipID, Defender.ShipID );
			}

		}


		/// <summary>
		/// 戦闘詳細の情報を出力します。
		/// </summary>
		public override string ToString() {

			StringBuilder builder = new StringBuilder();

			builder.AppendFormat( "{0} → {1} #{2}\r\n", GetAttackerName(), Defender.NameWithClass, GetDisplayIndex( DefenderIndex ) );


			if ( AttackType >= 0 )
				builder.Append( "[" ).Append( GetAttackKind() ).Append( "] " );

			for ( int i = 0; i < Damages.Length; i++ ) {
				if ( CriticalTypes[i] == CriticalType.Invalid )	// カットイン(主砲/主砲)、カットイン(主砲/副砲)時に発生する
					continue;

				if ( i > 0 )
					builder.Append( " , " );

				switch ( CriticalTypes[i] ) {
					case CriticalType.Miss:
						builder.Append( "Miss" );
						break;
					case CriticalType.Hit:
						builder.Append( Damages[i] ).Append( " Dmg" );
						break;
					case CriticalType.Critical:
						builder.Append( Damages[i] ).Append( " Critical!" );
						break;
				}
			}

			builder.AppendLine();
			return builder.ToString();
		}

		protected static bool IsFriendIndex( int i ) {
			return ( 0 <= i && i < 6 ) || ( 12 <= i && i < 18 );
		}

		protected static bool IsEnemyIndex( int i ) {
			return ( 6 <= i && i < 12 ) || ( 18 <= i && i < 24 );
		}

		protected static int GetDisplayIndex( int i ) {
			return i % 6 + ( i / 12 ) * 6 + 1;
		}


		protected virtual string GetAttackerName() {
			if ( Attacker == null )
				return "#" + GetDisplayIndex( AttackerIndex );
			return Attacker.NameWithClass + " #" + GetDisplayIndex( AttackerIndex );
		}

		protected abstract int CaclulateAttackKind( int[] slots, int attackerShipID, int defenderShipID );
		protected abstract string GetAttackKind();

	}


	/// <summary>
	/// 昼戦の戦闘詳細データを保持します。
	/// </summary>
	public class BattleDayDetail : BattleDetail {

		public BattleDayDetail( BattleData bd, int attackerId, int defenderId, int[] damages, int[] criticalTypes, int attackType )
			: base( bd, attackerId, defenderId, damages, criticalTypes, attackType ) {
		}

		protected override int CaclulateAttackKind( int[] slots, int attackerShipID, int defenderShipID ) {
			return Calculator.GetDayAttackKind( slots, attackerShipID, defenderShipID, false );
		}

		protected override string GetAttackKind() {
			return Constants.GetDayAttackKind( AttackType );
		}
	}

	/// <summary>
	/// 支援攻撃の戦闘詳細データを保持します。
	/// </summary>
	public class BattleSupportDetail : BattleDetail {

		public BattleSupportDetail( BattleData bd, int defenderId, int damage, int criticalType, int attackType )
			: base( bd, -1, defenderId, new int[] { damage }, new int[] { criticalType }, attackType ) {
		}

		protected override string GetAttackerName() {
			return GetAttackKind() + "支援";
		}

		protected override int CaclulateAttackKind( int[] slots, int attackerShipID, int defenderShipID ) {
			return -1;
		}

		protected override string GetAttackKind() {
			switch ( AttackType ) {
				case 1:
					return "航空";
				case 2:
					return "炮击";
				case 3:
					return "雷击";
				default:
					return "不明";
			}
		}

	}

	/// <summary>
	/// 夜戦における戦闘詳細データを保持します。
	/// </summary>
	public class BattleNightDetail : BattleDetail {

		public BattleNightDetail( BattleData bd, int attackerId, int defenderId, int[] damages, int[] criticalTypes, int attackType )
			: base( bd, attackerId, defenderId, damages, criticalTypes, attackType ) {
		}

		protected override int CaclulateAttackKind( int[] slots, int attackerShipID, int defenderShipID ) {
			return Calculator.GetNightAttackKind( slots, attackerShipID, defenderShipID, false );
		}

		protected override string GetAttackKind() {
			return Constants.GetNightAttackKind( AttackType );
		}
	}

	/// <summary>
	/// 航空戦における戦闘詳細データを保持します。
	/// </summary>
	public class BattleAirDetail : BattleDayDetail {

		public BattleAirDetail( BattleData bd, int attackerId, int defenderId, int damage, int criticalType, int attackType )
			: base( bd, attackerId, defenderId, new int[] { damage }, new int[] { criticalType }, attackType ) {

		}

		protected override string GetAttackerName() {
			if ( AttackerIndex == 0 ) {
				return "航空队";

			} else if ( AttackerIndex > 0 ) {
				return string.Format( "基地航空队 第{0}波", AttackerIndex );

			} else {
				return "未確認飛行物体";
			}
		}

		protected override int CaclulateAttackKind( int[] slots, int attackerShipID, int defenderShipID ) {
			return -1;
		}

		protected override string GetAttackKind() {
			switch ( AttackType ) {
				case 1:
					return "雷撃";
				case 2:
					return "爆撃";
				case 3:
					return "雷撃+爆撃";
				default:
					return "不明";
			}
		}

	}
}
