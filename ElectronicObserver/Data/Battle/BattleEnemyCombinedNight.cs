﻿using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{

	/// <summary>
	/// 通常/連合艦隊 vs 連合艦隊 夜戦
	/// </summary>
	public class BattleEnemyCombinedNight : BattleNight
	{

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			// 支援なし?
			NightBattle = new PhaseNightBattle(this, "夜战", 0, false);


			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);
		}


		public override string APIName => "api_req_combined_battle/ec_midnight_battle";

		public override string BattleName => "对联合舰队 夜战";

		public override BattleTypeFlag BattleType => BattleTypeFlag.Night | BattleTypeFlag.EnemyCombined | (NightBattle == null ? BattleTypeFlag.Combined : (NightBattle.IsFriendEscort ? BattleTypeFlag.Combined : 0));


		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return NightBattle;
		}
	}
}
