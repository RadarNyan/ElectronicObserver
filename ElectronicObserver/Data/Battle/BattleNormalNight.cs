﻿using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{

	/// <summary>
	/// 通常艦隊 vs 通常艦隊 夜戦
	/// </summary>
	public class BattleNormalNight : BattleNight
	{

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			// 支援なし?
			NightBattle = new PhaseNightBattle(this, "夜战", 0, false);

			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);
		}


		public override string APIName => "api_req_battle_midnight/battle";

		public override string BattleName => "通常舰队 夜战";

		public override BattleTypeFlag BattleType => BattleTypeFlag.Night;


		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return NightBattle;
		}
	}
}
