﻿using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 敵連合艦隊夜戦
	/// </summary>
	public class BattleEnemyCombinedNight : BattleNight {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			NightBattle = new PhaseNightBattle( this, "夜战", false );

			NightBattle.EmulateBattle( _resultHPs, _attackDamages );

		}


		public override string APIName {
			get { return "api_req_combined_battle/ec_midnight_battle"; }
		}

		public override string BattleName {
			get { return "对联合舰队 夜战"; }
		}

		public override BattleData.BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Night | BattleTypeFlag.EnemyCombined | ( NightBattle.IsFriendEscort ? BattleTypeFlag.Combined : 0 ); }
		}


		public override IEnumerable<PhaseBase> GetPhases() {
			yield return Initial;
			yield return NightBattle;
		}
	}
}
