﻿using ElectronicObserver.Data.Battle.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase
{

	/// <summary>
	/// 夜戦フェーズの処理を行います。
	/// </summary>
	public class PhaseNightBattle : PhaseBase
	{

		private readonly bool isEscort;

		public PhaseNightBattle(BattleData data, string title, bool isEscort)
			: base(data, title)
		{

			this.isEscort = isEscort;

			if (!IsAvailable)
				return;


			// TEMP FIX
			/*
			int[] attackers = ((int[])ShellingData.api_at_list).Skip(1).ToArray();
			int[] nightAirAttackFlags = ((int[])ShellingData.api_n_mother_list).Skip(1).ToArray();
			int[] attackTypes = ((int[])ShellingData.api_sp_list).Skip(1).ToArray();
			int[][] defenders = ((dynamic[])ShellingData.api_df_list).Skip(1).Select(elem => ((int[])elem).Where(e => e != -1).ToArray()).ToArray();
			int[][] attackEquipments = ((dynamic[])ShellingData.api_si_list).Skip(1).Select(elem => ((dynamic[])elem).Select<dynamic, int>(ch => ch is string ? int.Parse(ch) : (int)ch).ToArray()).ToArray();
			int[][] criticals = ((dynamic[])ShellingData.api_cl_list).Skip(1).Select(elem => ((int[])elem).Where(e => e != -1).ToArray()).ToArray();
			double[][] rawDamages = ((dynamic[])ShellingData.api_damage).Skip(1).Select(elem => ((double[])elem).Where(e => e != -1).ToArray()).ToArray();
			*/
			int[] attackers = ((int[])ShellingData.api_at_list).ToArray();
			int[] nightAirAttackFlags = ((int[])ShellingData.api_n_mother_list).ToArray();
			int[] attackTypes = ((int[])ShellingData.api_sp_list).ToArray();
			int[][] defenders = ((dynamic[])ShellingData.api_df_list).Select(elem => ((int[])elem).Where(e => e != -1).ToArray()).ToArray();
			int[][] attackEquipments = ((dynamic[])ShellingData.api_si_list).Select(elem => ((dynamic[])elem).Select<dynamic, int>(ch => ch is string ? int.Parse(ch) : (int)ch).ToArray()).ToArray();
			int[][] criticals = ((dynamic[])ShellingData.api_cl_list).Select(elem => ((int[])elem).Where(e => e != -1).ToArray()).ToArray();
			double[][] rawDamages = ((dynamic[])ShellingData.api_damage).Select(elem => ((double[])elem).Where(e => e != -1).ToArray()).ToArray();

			int[] temp_fleetflag = !ShellingData.api_at_eflag() ? null : ((int[])ShellingData.api_at_eflag).ToArray();
			int temp_f_shipcount = ((int[])RawData.api_f_nowhps).Length;

			for (int i = 0; i < attackers.Length; ++i) {
				switch (temp_fleetflag[i]) {
					case 0: // 我方攻击
						if (attackers[i] == 6) // 第 7 艘
							attackers[i] += 6;
						break;
					case 1: // 敌方攻击
						if (attackers[i] == 6) // 第 7 艘
							attackers[i] += 6;
						attackers[i] += 6;
						break;
				}
				++attackers[i];
				for (int j = 0; j < defenders[i].Length; ++j) {
					switch (temp_fleetflag[i]) {
						case 1: // 敌方攻击 ( 我方防御 )
							if (defenders[i][j] == 6) // 第 7 艘
								defenders[i][j] += 6;
							break;
						case 0: // 我方攻击 ( 敌方防御 )
							if (defenders[i][j] == 6) // 第 7 艘
								defenders[i][j] += 6;
							defenders[i][j] += 6;
							break;
					}
					++defenders[i][j];
				}
			}
			// TEMP FIX END

			Attacks = new List<PhaseNightBattleAttack>();

			for (int i = 0; i < attackers.Length; i++)
			{
				var attack = new PhaseNightBattleAttack
				{
					Attacker = GetIndex(attackers[i]),
					NightAirAttackFlag = nightAirAttackFlags[i] == -1,
					AttackType = attackTypes[i],
					EquipmentIDs = attackEquipments[i],

					Defenders = new List<PhaseNightBattleDefender>()
				};
				for (int k = 0; k < defenders[i].Length; k++)
				{
					var defender = new PhaseNightBattleDefender
					{
						Defender = GetIndex(defenders[i][k]),
						CriticalFlag = criticals[i][k],
						RawDamage = rawDamages[i][k]
					};
					attack.Defenders.Add(defender);
				}

				Attacks.Add(attack);
			}

		}

		public override bool IsAvailable => true;

		public dynamic ShellingData => RawData.api_hougeki;


		public override void EmulateBattle(int[] hps, int[] damages)
		{

			if (!IsAvailable) return;


			foreach (var attack in Attacks)
			{

				foreach (var defs in attack.Defenders.GroupBy(d => d.Defender))
				{
					BattleDetails.Add(new BattleNightDetail(_battleData, attack.Attacker, defs.Key, defs.Select(d => d.RawDamage).ToArray(), defs.Select(d => d.CriticalFlag).ToArray(), attack.AttackType, attack.EquipmentIDs, attack.NightAirAttackFlag, hps[defs.Key]));
					AddDamage(hps, defs.Key, defs.Sum(d => d.Damage));
				}

				damages[attack.Attacker] += attack.Defenders.Sum(d => d.Damage);
			}

		}


		public List<PhaseNightBattleAttack> Attacks { get; private set; }
		public class PhaseNightBattleAttack
		{
			public int Attacker;
			public int AttackType;
			public bool NightAirAttackFlag;
			public List<PhaseNightBattleDefender> Defenders;
			public int[] EquipmentIDs;

			public PhaseNightBattleAttack() { }

			public override string ToString() => $"{Attacker}[{AttackType}] -> [{string.Join(", ", Defenders)}]";

		}
		public class PhaseNightBattleDefender
		{
			public int Defender;
			public int CriticalFlag;
			public double RawDamage;
			public bool GuardsFlagship => RawDamage != Math.Floor(RawDamage);
			public int Damage => (int)RawDamage;

			public override string ToString()
			{
				return string.Format("{0};{1}-{2}{3}", Defender, Damage,
					CriticalFlag == 0 ? "miss" : CriticalFlag == 1 ? "dmg" : CriticalFlag == 2 ? "crit" : "INVALID",
					GuardsFlagship ? " (guard)" : "");
			}
		}



		/// <summary>
		/// 戦闘する自軍艦隊
		/// 1=主力艦隊, 2=随伴艦隊
		/// </summary>
		public int ActiveFriendFleet => !RawData.api_active_deck() ? 1 : (int)RawData.api_active_deck[0];

		/// <summary>
		/// 自軍艦隊ID
		/// </summary>
		public int FriendFleetID
		{
			get
			{
				if (IsFriendEscort)
					return 2;
				else
					return _battleData.Initial.FriendFleetID;
			}
		}

		/// <summary>
		/// 自軍艦隊
		/// </summary>
		public FleetData FriendFleet => KCDatabase.Instance.Fleet[FriendFleetID];

		/// <summary>
		/// 自軍が随伴艦隊かどうか
		/// </summary>
		public bool IsFriendEscort => isEscort || ActiveFriendFleet != 1;


		/// <summary>
		/// 敵軍艦隊ID
		/// </summary>
		public int EnemyFleetID => !RawData.api_active_deck() ? 1 : (int)RawData.api_active_deck[1];

		/// <summary>
		/// 敵軍艦隊
		/// </summary>
		public int[] EnemyMembers => !IsEnemyEscort ? _battleData.Initial.EnemyMembers : _battleData.Initial.EnemyMembersEscort;

		/// <summary>
		/// 敵軍艦隊
		/// </summary>
		public ShipDataMaster[] EnemyMembersInstance => !IsEnemyEscort ? _battleData.Initial.EnemyMembersInstance : _battleData.Initial.EnemyMembersEscortInstance;

		/// <summary>
		/// 敵軍が随伴艦隊かどうか
		/// </summary>
		public bool IsEnemyEscort => EnemyFleetID != 1;


		/// <summary>
		/// 自軍触接機ID
		/// </summary>
		public int TouchAircraftFriend => (RawData.api_touch_plane[0] is string) ? int.Parse(RawData.api_touch_plane[0]) : (int)RawData.api_touch_plane[0];


		/// <summary>
		/// 敵軍触接機ID
		/// </summary>
		public int TouchAircraftEnemy => (RawData.api_touch_plane[1] is string) ? int.Parse(RawData.api_touch_plane[1]) : (int)RawData.api_touch_plane[1];


		/// <summary>
		/// 自軍照明弾投射艦番号(0-5, -1=発動せず)
		/// </summary>
		public int FlareIndexFriend
		{
			get
			{
				int index = (int)RawData.api_flare_pos[0];
				return index != -1 ? index - 1 : -1;
			}
		}

		/// <summary>
		/// 敵軍照明弾投射艦番号(0-5, -1=発動せず)
		/// </summary>
		public int FlareIndexEnemy
		{
			get
			{
				int index = (int)RawData.api_flare_pos[1];
				return index != -1 ? index - 1 : -1;
			}
		}

		/// <summary>
		/// 敵軍照明弾投射艦
		/// </summary>
		public ShipDataMaster FlareEnemyInstance
		{
			get
			{
				int index = FlareIndexEnemy;
				return index == -1 ? null : EnemyMembersInstance[index];
			}
		}

		/// <summary>
		/// 自軍探照灯照射艦番号
		/// </summary>
		public int SearchlightIndexFriend
		{
			get
			{
				var ships = FriendFleet.MembersWithoutEscaped;
				int index = -1;

				for (int i = 0; i < ships.Count; i++)
				{

					var ship = ships[i];
					if (ship != null && _battleData.Initial.InitialHPs[(isEscort ? 12 : 0) + i] > 1)
					{

						if (ship.SlotInstanceMaster.Any(e => e?.CategoryType == EquipmentTypes.SearchlightLarge))
							return i;
						else if (ship.SlotInstanceMaster.Any(e => e?.CategoryType == EquipmentTypes.Searchlight) && index == -1)
							index = i;
					}
				}

				return index;
			}
		}

		/// <summary>
		/// 敵軍探照灯照射艦番号(0-5)
		/// </summary>
		public int SearchlightIndexEnemy
		{
			get
			{
				var ships = _battleData.Initial.EnemyMembersInstance;
				var eqs = _battleData.Initial.EnemySlotsInstance;
				int index = -1;

				for (int i = 0; i < ships.Length; i++)
				{

					if (ships[i] != null && _battleData.Initial.InitialHPs[6 + i] > 1)
					{

						if (eqs[i].Any(e => e?.CategoryType == EquipmentTypes.SearchlightLarge))
							return i;
						else if (eqs[i].Any(e => e?.CategoryType == EquipmentTypes.Searchlight) && index == -1)
							index = i;

					}
				}

				return index;
			}
		}

		public ShipDataMaster SearchlightEnemyInstance
		{
			get
			{
				int index = SearchlightIndexEnemy;
				return index == -1 ? null : EnemyMembersInstance[index];
			}
		}


		private int GetIndex(int index)
		{
			index--;
			if (index < 6)
			{
				if (IsFriendEscort)
					index += 12;
			}
			else
			{
				if (IsEnemyEscort)
					index += 12;
			}
			return index;
		}

	}
}
