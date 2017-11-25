﻿using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle
{
	/// <summary>
	/// 夜昼戦
	/// </summary>
	public abstract class BattleDayFromNight : BattleNight
	{

		public PhaseSupport NightSupport { get; protected set; }
		public PhaseNightBattle NightBattle2 { get; protected set; }

		public bool NextToDay => (int)RawData.api_day_flag != 0;

		public PhaseJetBaseAirAttack JetBaseAirAttack { get; protected set; }
		public PhaseJetAirBattle JetAirBattle { get; protected set; }
		public PhaseBaseAirAttack BaseAirAttack { get; protected set; }
		public PhaseAirBattle AirBattle { get; protected set; }
		public PhaseOpeningASW OpeningASW { get; protected set; }
		public PhaseTorpedo OpeningTorpedo { get; protected set; }
		public PhaseShelling Shelling1 { get; protected set; }
		public PhaseShelling Shelling2 { get; protected set; }
		public PhaseTorpedo Torpedo { get; protected set; }

	}
}
