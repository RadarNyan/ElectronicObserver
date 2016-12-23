﻿using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 通常艦隊航空戦
	/// </summary>
	public class BattleAirBattle : BattleDay {

		public PhaseAirBattle AirBattle2 { get; protected set; }

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			JetBaseAirAttack = new PhaseJetBaseAirAttack( this, "喷式基地航空队攻击" );
			JetAirBattle = new PhaseJetAirBattle( this, "喷式航空战" );
			BaseAirAttack = new PhaseBaseAirAttack( this, "基地航空队攻击" );
			AirBattle = new PhaseAirBattle( this, "第一次航空战" );
			Support = new PhaseSupport( this, "支援攻击" );
			AirBattle2 = new PhaseAirBattle( this, "第二次航空战", "2" );

			foreach ( var phase in GetPhases() )
				phase.EmulateBattle( _resultHPs, _attackDamages );

		}

		public override string APIName {
			get { return "api_req_sortie/airbattle"; }
		}

		public override string BattleName {
			get { return "通常舰队 航空战"; }
		}

		public override BattleData.BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day; }
		}


		public override IEnumerable<PhaseBase> GetPhases() {
			yield return Initial;
			yield return Searching;
			yield return JetBaseAirAttack;
			yield return JetAirBattle;
			yield return BaseAirAttack;
			yield return AirBattle;
			yield return Support;
			yield return AirBattle2;
		}

	}
}
