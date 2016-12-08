using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 演習昼戦
	/// </summary>
	public class BattlePracticeDay : BattleDay {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			AirBattle = new PhaseAirBattle( this, "航空战" );
			OpeningASW = new PhaseOpeningASW( this, "先制对潜", false );
			OpeningTorpedo = new PhaseTorpedo( this, "开幕雷击", 0 );
			Shelling1 = new PhaseShelling( this, "第一次炮击战", 1, "1", false );
			Shelling2 = new PhaseShelling( this, "第二次炮击战", 2, "2", false );
			Shelling3 = new PhaseShelling( this, "第三次炮击战", 3, "3", false );
			Torpedo = new PhaseTorpedo( this, "雷击战", 4 );


			AirBattle.EmulateBattle( _resultHPs, _attackDamages );
			OpeningASW.EmulateBattle( _resultHPs, _attackDamages );
			OpeningTorpedo.EmulateBattle( _resultHPs, _attackDamages );
			Shelling1.EmulateBattle( _resultHPs, _attackDamages );
			Shelling2.EmulateBattle( _resultHPs, _attackDamages );
			Shelling3.EmulateBattle( _resultHPs, _attackDamages );
			Torpedo.EmulateBattle( _resultHPs, _attackDamages );

		}


		public override string APIName {
			get { return "api_req_practice/battle"; }
		}

		public override string BattleName {
			get { return "演习 昼战"; }
		}

		public override BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day | BattleTypeFlag.Practice; }
		}


		public override IEnumerable<PhaseBase> GetPhases() {
			yield return Initial;
			yield return Searching;
			yield return AirBattle;
			yield return OpeningASW;
			yield return OpeningTorpedo;
			yield return Shelling1;
			yield return Shelling2;
			yield return Shelling3;
			yield return Torpedo;
		}
	}

}
