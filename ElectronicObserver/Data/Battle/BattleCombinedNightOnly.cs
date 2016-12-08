﻿using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 連合艦隊開幕夜戦
	/// </summary>
	public class BattleCombinedNightOnly : BattleNight {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			NightBattle = new PhaseNightBattle( this, "夜战", true );

			NightBattle.EmulateBattle( _resultHPs, _attackDamages );

		}



		public override string APIName {
			get { return "api_req_combined_battle/sp_midnight"; }
		}

		public override string BattleName {
			get { return "联合舰队 开幕夜战"; }
		}

		public override BattleData.BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Night | BattleTypeFlag.Combined; }
		}


		public override IEnumerable<PhaseBase> GetPhases() {
			yield return Initial;
			yield return Searching;
			yield return NightBattle;
		}
	}

}
