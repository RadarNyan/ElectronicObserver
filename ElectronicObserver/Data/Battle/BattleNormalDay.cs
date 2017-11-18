using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{

	/// <summary>
	/// 通常艦隊昼戦
	/// </summary>
	public class BattleNormalDay : BattleDay
	{


		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			JetBaseAirAttack = new PhaseJetBaseAirAttack(this, "喷式基地航空队攻击");
			JetAirBattle = new PhaseJetAirBattle(this, "喷式航空战");
			BaseAirAttack = new PhaseBaseAirAttack(this, "基地航空队攻击");
			AirBattle = new PhaseAirBattle(this, "航空战");
			Support = new PhaseSupport(this, "支援攻击");
			OpeningASW = new PhaseOpeningASW(this, "先制对潜", false);
			OpeningTorpedo = new PhaseTorpedo(this, "开幕雷击", 0);
			Shelling1 = new PhaseShelling(this, "第一次炮击战", 1, "1", false);
			Shelling2 = new PhaseShelling(this, "第二次炮击战", 2, "2", false);
			Shelling3 = new PhaseShelling(this, "第三次炮击战", 3, "3", false);
			Torpedo = new PhaseTorpedo(this, "雷击战", 4);

			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);

		}


		public override string APIName => "api_req_sortie/battle";

		public override string BattleName => "通常舰队 昼战";

		public override BattleData.BattleTypeFlag BattleType => BattleTypeFlag.Day;


		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return Searching;
			yield return JetBaseAirAttack;
			yield return JetAirBattle;
			yield return BaseAirAttack;
			yield return AirBattle;
			yield return Support;
			yield return OpeningASW;
			yield return OpeningTorpedo;
			yield return Shelling1;
			yield return Shelling2;
			yield return Shelling3;
			yield return Torpedo;
		}
	}

}
