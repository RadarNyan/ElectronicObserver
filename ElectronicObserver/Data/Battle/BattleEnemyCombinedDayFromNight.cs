using ElectronicObserver.Data.Battle.Phase;
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
	public class BattleEnemyCombinedDayFromNight : BattleDay
	{

		public PhaseSupport NightSupport { get; protected set; }
		public PhaseNightBattle NightBattle1 { get; protected set; }
		public PhaseNightBattle NightBattle2 { get; protected set; }

		public bool NextToDay => (int)RawData.api_day_flag != 0;


		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			NightSupport = new PhaseSupport(this, "夜间支援", true);
			NightBattle1 = new PhaseNightBattle(this, "第一次夜战", 1, false);
			NightBattle2 = new PhaseNightBattle(this, "第二次夜战", 2, false);

			JetBaseAirAttack = new PhaseJetBaseAirAttack(this, "喷式基地航空队攻击");
			JetAirBattle = new PhaseJetAirBattle(this, "喷式航空战");
			BaseAirAttack = new PhaseBaseAirAttack(this, "基地航空队攻击");
			Support = new PhaseSupport(this, "支援攻击");
			AirBattle = new PhaseAirBattle(this, "航空战");
			OpeningASW = new PhaseOpeningASW(this, "先制对潜", false);
			OpeningTorpedo = new PhaseTorpedo(this, "开幕雷击", 0);
			Shelling1 = new PhaseShelling(this, "炮击战", 1, "1", false);
			Torpedo = new PhaseTorpedo(this, "雷击战", 2);

			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);
		}

		public override string APIName => "api_req_combined_battle/ec_night_to_day";

		public override string BattleName => "对联合舰队　夜昼战";


		public override BattleTypeFlag BattleType => BattleTypeFlag.Day | BattleTypeFlag.EnemyCombined;


		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return NightSupport;
			yield return NightBattle1;
			yield return NightBattle2;
			yield return JetBaseAirAttack;
			yield return JetAirBattle;
			yield return BaseAirAttack;
			yield return AirBattle;
			yield return Support;
			yield return OpeningASW;
			yield return OpeningTorpedo;
			yield return Shelling1;
			yield return Torpedo;
		}
	}
}
