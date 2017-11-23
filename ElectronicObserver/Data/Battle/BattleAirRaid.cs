using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{

	/// <summary>
	/// 通常艦隊 vs 通常艦隊 長距離空襲戦
	/// </summary>
	public class BattleAirRaid : BattleDay
	{

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			JetBaseAirAttack = new PhaseJetBaseAirAttack(this, "喷式基地航空队攻击");
			JetAirBattle = new PhaseJetAirBattle(this, "喷式空袭战");
			BaseAirAttack = new PhaseBaseAirAttack(this, "基地航空队攻击");
			AirBattle = new PhaseAirBattle(this, "空袭战");
			// 支援は出ないものとする

			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);

		}

		public override string APIName => "api_req_sortie/ld_airbattle";

		public override string BattleName => "通常舰队 长距离空袭战";

		
		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return Searching;
			yield return JetBaseAirAttack;
			yield return JetAirBattle;
			yield return BaseAirAttack;
			yield return AirBattle;
		}
	}
}
