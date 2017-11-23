using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{

	/// <summary>
	/// 通常/連合艦隊 vs 通常艦隊 開幕夜戦
	/// </summary>
	public class BattleNightOnly : BattleNight
	{

		public override void LoadFromResponse(string apiname, dynamic data)
		{
			base.LoadFromResponse(apiname, (object)data);

			Support = new PhaseSupport(this, "支援攻击", false);
			NightBattle = new PhaseNightBattle(this, "夜战", 0, false);
			

			foreach (var phase in GetPhases())
				phase.EmulateBattle(_resultHPs, _attackDamages);
		}


		public override string APIName => "api_req_battle_midnight/sp_midnight";

		public override string BattleName => "通常舰队 开幕夜战";

	

		public override IEnumerable<PhaseBase> GetPhases()
		{
			yield return Initial;
			yield return Searching;
			yield return Support;
			yield return NightBattle;
		}
	}

}
