﻿using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 夜戦フェーズの処理を行います。
	/// </summary>
	public class PhaseNightBattle : PhaseBase {

		private readonly bool isEscort;

		public PhaseNightBattle( BattleData data, string title, bool isEscort )
			: base( data, title ) {

			this.isEscort = isEscort;

			if ( !IsAvailable )
				return;


			int[] attackers = ( (int[])ShellingData.api_at_list ).Skip( 1 ).ToArray();
			int[] attackTypes = ( (int[])ShellingData.api_sp_list ).Skip( 1 ).ToArray();
			int[][] defenders = ( (dynamic[])ShellingData.api_df_list ).Skip( 1 ).Select( elem => ( (int[])elem ).Where( e => e != -1 ).ToArray() ).ToArray();
			int[][] criticals = ( (dynamic[])ShellingData.api_cl_list ).Skip( 1 ).Select( elem => ( (int[])elem ).Where( e => e != -1 ).ToArray() ).ToArray();
			int[][] damages = ( (dynamic[])ShellingData.api_damage ).Skip( 1 ).Select( elem => ( (int[])elem ).Where( e => e != -1 ).ToArray() ).ToArray();

			Attacks = new List<PhaseNightBattleAttack>();

			for ( int i = 0; i < attackers.Length; i++ ) {
				var attack = new PhaseNightBattleAttack();

				attack.Attacker = GetIndex( attackers[i] );
				attack.AttackType = attackTypes[i];

				attack.Defenders = new List<PhaseNightBattleDefender>();
				for ( int k = 0; k < defenders[i].Length; k++ ) {
					var defender = new PhaseNightBattleDefender();
					defender.Defender = GetIndex( defenders[i][k] );
					defender.CriticalFlag = criticals[i][k];
					defender.Damage = damages[i][k];
					attack.Defenders.Add( defender );
				}

				Attacks.Add( attack );
			}

		}

		public override bool IsAvailable {
			get { return true; }
		}

		public dynamic ShellingData { get { return RawData.api_hougeki; } }


		public override void EmulateBattle( int[] hps, int[] damages ) {

			if ( !IsAvailable ) return;


			foreach ( var attack in Attacks ) {

				foreach ( var defs in attack.Defenders.GroupBy( d => d.Defender ) ) {
					BattleDetails.Add( new BattleNightDetail( _battleData, attack.Attacker, defs.Key, defs.Select( d => d.Damage ).ToArray(), defs.Select( d => d.CriticalFlag ).ToArray(), attack.AttackType, hps[defs.Key] ) );
					AddDamage( hps, defs.Key, defs.Sum( d => d.Damage ) );
				}

				damages[attack.Attacker] += attack.Defenders.Sum( d => d.Damage );
			}

		}


		public List<PhaseNightBattleAttack> Attacks { get; private set; }
		public class PhaseNightBattleAttack {
			public int Attacker;
			public int AttackType;
			public List<PhaseNightBattleDefender> Defenders;

			public PhaseNightBattleAttack() { }

			public override string ToString() {
				return string.Format( "{0}[{1}] -> [{2}]", Attacker, AttackType, string.Join( ", ", Defenders ) );
			}
		}
		public class PhaseNightBattleDefender {
			public int Defender;
			public int CriticalFlag;
			public int Damage;

			public override string ToString() {
				return string.Format( "{0};{1}-{2}", Defender, Damage, CriticalFlag == 0 ? "miss" : CriticalFlag == 1 ? "dmg" : CriticalFlag == 2 ? "crit" : "INVALID" );
			}
		}



		/// <summary>
		/// 戦闘する自軍艦隊
		/// 1=主力艦隊, 2=随伴艦隊
		/// </summary>
		public int ActiveFriendFleet { get { return !RawData.api_active_deck() ? 1 : (int)RawData.api_active_deck[0]; } }

		/// <summary>
		/// 自軍艦隊ID
		/// </summary>
		public int FriendFleetID {
			get {
				if ( IsFriendEscort )
					return 2;
				else
					return _battleData.Initial.FriendFleetID;
			}
		}

		/// <summary>
		/// 自軍艦隊
		/// </summary>
		public FleetData FriendFleet { get { return KCDatabase.Instance.Fleet[FriendFleetID]; } }

		/// <summary>
		/// 自軍が随伴艦隊かどうか
		/// </summary>
		public bool IsFriendEscort { get { return isEscort || ActiveFriendFleet != 1; } }


		/// <summary>
		/// 敵軍艦隊ID
		/// </summary>
		public int EnemyFleetID { get { return !RawData.api_active_deck() ? 1 : (int)RawData.api_active_deck[1]; } }

		/// <summary>
		/// 敵軍艦隊
		/// </summary>
		public int[] EnemyMembers { get { return !IsEnemyEscort ? _battleData.Initial.EnemyMembers : _battleData.Initial.EnemyMembersEscort; } }

		/// <summary>
		/// 敵軍艦隊
		/// </summary>
		public ShipDataMaster[] EnemyMembersInstance { get { return !IsEnemyEscort ? _battleData.Initial.EnemyMembersInstance : _battleData.Initial.EnemyMembersEscortInstance; } }

		/// <summary>
		/// 敵軍が随伴艦隊かどうか
		/// </summary>
		public bool IsEnemyEscort { get { return EnemyFleetID != 1; } }


		/// <summary>
		/// 自軍触接機ID
		/// </summary>
		public int TouchAircraftFriend { get { return (int)RawData.api_touch_plane[0]; } }

		/// <summary>
		/// 敵軍触接機ID
		/// </summary>
		public int TouchAircraftEnemy { get { return (int)RawData.api_touch_plane[1]; } }

		/// <summary>
		/// 自軍照明弾投射艦番号(0-5, -1=発動せず)
		/// </summary>
		public int FlareIndexFriend {
			get {
				int index = (int)RawData.api_flare_pos[0];
				return index != -1 ? index - 1 : -1;
			}
		}

		/// <summary>
		/// 敵軍照明弾投射艦番号(0-5, -1=発動せず)
		/// </summary>
		public int FlareIndexEnemy {
			get {
				int index = (int)RawData.api_flare_pos[1];
				return index != -1 ? index - 1 : -1;
			}
		}

		/// <summary>
		/// 敵軍照明弾投射艦
		/// </summary>
		public ShipDataMaster FlareEnemyInstance {
			get {
				int index = FlareIndexEnemy;
				return index == -1 ? null : EnemyMembersInstance[index];
			}
		}

		/// <summary>
		/// 自軍探照灯照射艦番号
		/// </summary>
		public int SearchlightIndexFriend {
			get {
				var ships = FriendFleet.MembersWithoutEscaped;
				int index = -1;

				for ( int i = 0; i < ships.Count; i++ ) {

					var ship = ships[i];
					if ( ship != null && _battleData.Initial.InitialHPs[( isEscort ? 12 : 0 ) + i] > 1 ) {

						if ( ship.SlotInstanceMaster.Count( e => e != null && e.CategoryType == 42 ) > 0 )		//大型探照灯
							return i;
						else if ( ship.SlotInstanceMaster.Count( e => e != null && e.CategoryType == 29 ) > 0 && index == -1 )		//探照灯
							index = i;
					}
				}

				return index;
			}
		}

		/// <summary>
		/// 敵軍探照灯照射艦番号(0-5)
		/// </summary>
		public int SearchlightIndexEnemy {
			get {
				var ships = _battleData.Initial.EnemyMembersInstance;
				var eqs = _battleData.Initial.EnemySlotsInstance;
				int index = -1;

				for ( int i = 0; i < ships.Length; i++ ) {

					if ( ships[i] != null && _battleData.Initial.InitialHPs[6 + i] > 1 ) {

						if ( eqs[i].Count( e => e != null && e.CategoryType == 42 ) > 0 )		//大型探照灯
							return i;
						else if ( eqs[i].Count( e => e != null && e.CategoryType == 29 ) > 0 && index == -1 )		//探照灯
							index = i;

					}
				}

				return index;
			}
		}

		public ShipDataMaster SearchlightEnemyInstance {
			get {
				int index = SearchlightIndexEnemy;
				return index == -1 ? null : EnemyMembersInstance[index];
			}
		}


		private int GetIndex( int index ) {
			index--;
			if ( index < 6 ) {
				if ( IsFriendEscort )
					index += 12;
			} else {
				if ( IsEnemyEscort )
					index += 12;
			}
			return index;
		}

	}
}
