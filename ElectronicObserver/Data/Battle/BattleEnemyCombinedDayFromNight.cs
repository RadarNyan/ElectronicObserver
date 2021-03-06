﻿using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{
	/// <summary>
	/// 通常/連合艦隊 vs 連合艦隊　夜昼戦
	/// </summary>
	public class BattleEnemyCombinedDayFromNight : BattleDayFromNight
	{

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			NightSupport = new PhaseSupport(this, "夜间支援攻击", true);
			NightBattle = new PhaseNightBattle(this, "第一次夜战", 1, false);
			NightBattle2 = new PhaseNightBattle(this, "第二次夜战", 2, false);


			if (NextToDay)
			{
				JetBaseAirAttack = new PhaseJetBaseAirAttack(this, "喷式基地航空队攻击");
				JetAirBattle = new PhaseJetAirBattle(this, "喷式航空战");
				BaseAirAttack = new PhaseBaseAirAttack(this, "基地航空队攻击");
				Support = new PhaseSupport(this, "支援攻击");
				AirBattle = new PhaseAirBattle(this, "航空战");
				OpeningASW = new PhaseOpeningASW(this, "先制对潜");
				OpeningTorpedo = new PhaseTorpedo(this, "开幕雷击", 0);
				Shelling1 = new PhaseShelling(this, "第一次炮击战", 1, "1");
				Shelling2 = new PhaseShelling(this, "第二次炮击战", 2, "2");
				Torpedo = new PhaseTorpedo(this, "雷击战", 3);
			}

			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);
		}

		public override string APIName => "api_req_combined_battle/ec_night_to_day";

		public override string BattleName => "对联合舰队　夜昼战";



		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return Searching;
			yield return NightSupport;
			yield return NightBattle;
			yield return NightBattle2;

			if (NextToDay)
			{
				yield return JetBaseAirAttack;
				yield return JetAirBattle;
				yield return BaseAirAttack;
				yield return Support;
				yield return AirBattle;
				yield return OpeningASW;
				yield return OpeningTorpedo;
				yield return Shelling1;
				yield return Shelling2;
				yield return Torpedo;
			}
		}
	}
}
