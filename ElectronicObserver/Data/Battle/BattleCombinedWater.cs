using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 連合艦隊(水上部隊)昼戦
	/// </summary>
	public class BattleCombinedWater : BattleDay {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			BaseAirAttack = new PhaseBaseAirAttack( this, "基地航空队攻击" );
			AirBattle = new PhaseAirBattle( this, "航空战" );
			Support = new PhaseSupport( this, "支援攻击" );
			OpeningASW = new PhaseOpeningASW( this, "先制对潜", true );
			OpeningTorpedo = new PhaseTorpedo( this, "开幕雷击", 0 );
			Shelling1 = new PhaseShelling( this, "第一次炮击战", 1, "1", false );
			Shelling2 = new PhaseShelling( this, "第二次炮击战", 2, "2", false );
			Shelling3 = new PhaseShelling( this, "第三次炮击战", 3, "3", true );
			Torpedo = new PhaseTorpedo( this, "雷击战", 4 );


			BaseAirAttack.EmulateBattle( _resultHPs, _attackDamages );
			AirBattle.EmulateBattle( _resultHPs, _attackDamages );
			Support.EmulateBattle( _resultHPs, _attackDamages );
			OpeningASW.EmulateBattle( _resultHPs, _attackDamages );
			OpeningTorpedo.EmulateBattle( _resultHPs, _attackDamages );
			Shelling1.EmulateBattle( _resultHPs, _attackDamages );
			Shelling2.EmulateBattle( _resultHPs, _attackDamages );
			Shelling3.EmulateBattle( _resultHPs, _attackDamages );
			Torpedo.EmulateBattle( _resultHPs, _attackDamages );

		}


		public override string APIName {
			get { return "api_req_combined_battle/battle_water"; }
		}

		public override string BattleName {
			get { return "联合舰队-水上部队 昼战"; }
		}

		public override BattleData.BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Day | BattleTypeFlag.Combined; }
		}


		public override IEnumerable<PhaseBase> GetPhases() {
			yield return Initial;
			yield return Searching;
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
