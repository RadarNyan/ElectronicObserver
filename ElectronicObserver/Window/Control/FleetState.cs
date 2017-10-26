using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElectronicObserver.Resource;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Window.Control {

	/// <summary>
	/// 艦隊の状態を表示します。
	/// </summary>
	public partial class FleetState : UserControl {

		[System.Diagnostics.DebuggerDisplay( "{State}" )]
		private class StateLabel {
			public FleetStates State;
			public ImageLabel Label;
			public DateTime Timer;
			private bool _onmouse;

			private string _text;
			public string Text {
				get { return _text; }
				set {
					_text = value;
					UpdateText();
				}
			}
			private string _shortenedText;
			public string ShortenedText {
				get { return _shortenedText; }
				set {
					_shortenedText = value;
					UpdateText();
				}
			}
			private bool _autoShorten;
			public bool AutoShorten {
				get { return _autoShorten; }
				set {
					_autoShorten = value;
					UpdateText();
				}
			}

			private bool _enabled;
			public bool Enabled {
				get { return _enabled; }
				set {
					_enabled = value;
					Label.Visible = value;
				}
			}


			public StateLabel() {
				Label = GetDefaultLabel();
				Label.MouseEnter += Label_MouseEnter;
				Label.MouseLeave += Label_MouseLeave;
				Enabled = false;
			}

			public static ImageLabel GetDefaultLabel() {
				var label = new ImageLabel();
				label.Anchor = AnchorStyles.Left;
				label.ImageList = ResourceManager.Instance.Icons;
				label.Padding = new Padding( 2, 2, 2, 2 );
				label.Margin = new Padding( 2, 0, 2, 0 );
				label.AutoSize = true;
				return label;
			}

			public void SetInformation( FleetStates state, string text, string shortenedText, int imageIndex, Color backColor, Color foreColor ) {
				State = state;
				Text = text;
				ShortenedText = shortenedText;
				UpdateText();
				Label.ImageIndex = imageIndex;
				Label.BackColor = backColor;
				Label.ForeColor = foreColor;
			}

			public void SetInformation( FleetStates state, string text, string shortenedText, int imageIndex ) {
				SetInformation( state, text, shortenedText, imageIndex, Color.Transparent, Utility.Configuration.Config.UI.ForeColor );
			}

			public void UpdateText() {
				Label.Text = ( !AutoShorten || _onmouse ) ? Text : ShortenedText;
			}


			void Label_MouseEnter( object sender, EventArgs e ) {
				_onmouse = true;
				UpdateText();
			}

			void Label_MouseLeave( object sender, EventArgs e ) {
				_onmouse = false;
				UpdateText();
			}

		}


		public override Font Font {
			get {
				return base.Font;
			}
			set {
				base.Font = value;
				foreach ( var state in StateLabels )
					state.Label.Font = value;
			}
		}


		private List<StateLabel> StateLabels;


		public FleetState() {
			InitializeComponent();

			StateLabels = new List<StateLabel>();
		}


		private StateLabel AddStateLabel() {
			StateLabels.Add( new StateLabel() );
			var ret = StateLabels.Last();
			LayoutBase.Controls.Add( ret.Label );
			return ret;
		}

		private StateLabel GetStateLabel( int index ) {
			if ( index >= StateLabels.Count ) {
				for ( int i = StateLabels.Count; i <= index; i++ )
					AddStateLabel();
			}
			StateLabels[index].Enabled = true;
			return StateLabels[index];
		}



		public void UpdateFleetState( FleetData fleet, ToolTip tooltip ) {

			KCDatabase db = KCDatabase.Instance;

			int index = 0;

			bool emphasizesSubFleetInPort = Utility.Configuration.Config.FormFleet.EmphasizesSubFleetInPort &&
				( db.Fleet.CombinedFlag > 0 ? fleet.FleetID >= 3 : fleet.FleetID >= 2 );

			//所属艦なし
			if ( fleet == null || fleet.Members.All( id => id == -1 ) ) {
				var state = GetStateLabel( index );

				state.SetInformation( FleetStates.NoShip, "无所属舰", "", (int)ResourceManager.IconContent.FleetNoShip );
				tooltip.SetToolTip( state.Label, null );

				emphasizesSubFleetInPort = false;
				index++;

			} else {

				if ( fleet.IsInSortie ) {

					//大破出撃中
					if ( fleet.MembersWithoutEscaped.Any( s => s != null && s.HPRate <= 0.25 ) ) {
						var state = GetStateLabel( index );

						state.SetInformation( FleetStates.SortieDamaged, "！！大破进击中！！", "！！大破进击中！！",
							(int)ResourceManager.IconContent.FleetSortieDamaged,
							Utility.Configuration.Config.UI.FleetOverview_ShipDamagedBG,
							Utility.Configuration.Config.UI.FleetOverview_ShipDamagedFG );
						tooltip.SetToolTip( state.Label, null );

						index++;

					} else {	//出撃中
						var state = GetStateLabel( index );

						state.SetInformation( FleetStates.Sortie, "出击中", "", (int)ResourceManager.IconContent.FleetSortie );
						tooltip.SetToolTip( state.Label, null );

						index++;
					}

					emphasizesSubFleetInPort = false;
				}

				//遠征中
				if ( fleet.ExpeditionState != 0 ) {
					var state = GetStateLabel( index );

					state.Timer = fleet.ExpeditionTime;
					state.SetInformation( FleetStates.Expedition, "远征中 " + DateTimeHelper.ToTimeRemainString( state.Timer ),
						DateTimeHelper.ToTimeRemainString( state.Timer ), (int)ResourceManager.IconContent.FleetExpedition );

					var dest = db.Mission[fleet.ExpeditionDestination];
					tooltip.SetToolTip( state.Label,
						string.Format( "{0} : {1}\r\n完成时间 : {2}",
						dest.ID, dest.Name, DateTimeHelper.TimeToCSVString( state.Timer ) ) );

					emphasizesSubFleetInPort = false;
					index++;
				}

				//大破艦あり
				if ( !fleet.IsInSortie && fleet.MembersWithoutEscaped.Any( s => s != null && s.HPRate <= 0.25 && s.RepairingDockID == -1 ) ) {
					var state = GetStateLabel( index );

					state.SetInformation( FleetStates.Damaged, "有舰娘大破！", "有舰娘大破！", (int)ResourceManager.IconContent.FleetDamaged,
						Utility.Configuration.Config.UI.FleetOverview_ShipDamagedBG, Utility.Configuration.Config.UI.FleetOverview_ShipDamagedFG );
					tooltip.SetToolTip( state.Label, null );

					emphasizesSubFleetInPort = false;
					index++;
				}

				//泊地修理中
				if ( fleet.CanAnchorageRepair ) {
					var state = GetStateLabel( index );

					state.Timer = db.Fleet.AnchorageRepairingTimer;
					state.SetInformation( FleetStates.AnchorageRepairing, "修理中 " + DateTimeHelper.ToTimeElapsedString( state.Timer ),
						DateTimeHelper.ToTimeElapsedString( state.Timer ), (int)ResourceManager.IconContent.FleetAnchorageRepairing );


					StringBuilder sb = new StringBuilder();
					sb.AppendFormat( "开始时间 :\r\n{0}\r\n修理耗时 :\r\n",
						DateTimeHelper.TimeToCSVString( KCDatabase.Instance.Fleet.AnchorageRepairingTimer ) );

					for ( int i = 0; i < fleet.Members.Count; i++ ) {
						var ship = fleet.MembersInstance[i];
						if ( ship != null && ship.HPRate < 1.0 && ship.HPRate > 0.5 ) {
							var unittime = Calculator.CalculateDockingUnitTime(ship, 1, false);
							var totaltime = Calculator.CalculateDockingUnitTime(ship, (ship.HPMax - ship.HPCurrent));
							sb.AppendFormat( "#{0} : {1:00}:{2:00}:00 @ {3:00}'{4:00}\" x -{5} HP\r\n", i + 1,
								(int)totaltime.TotalHours, totaltime.Seconds != 0 ? totaltime.Minutes + 1 : totaltime.Minutes,
								unittime.Minutes, unittime.Seconds, ship.HPMax - ship.HPCurrent );
						}
					}

					if (Utility.Configuration.Config.UI.MaxAkashiPerHP != 0) {
						sb.Append("每 HP 耗时 : (hh:mm)\r\n");
						for (int i = 0; i < fleet.Members.Count; i++) {
							var ship = fleet.MembersInstance[i];
							if (ship == null) {
								break;
							} else if (ship.HPRate < 1.0 && ship.HPRate > 0.5) {
								sb.AppendFormat("#{0} :", i + 1);
								int hpToFix = ship.HPMax - ship.HPCurrent;
								for (int hp = 1; hp <= hpToFix; ++hp) {
									var perhp = Calculator.CalculateDockingUnitTime(ship, hp);
									sb.AppendFormat(" {0:00}:{1:00} ", (int)perhp.Hours,
										perhp.Seconds != 0 ? perhp.Minutes + 1 : perhp.Minutes);
									if (hp == hpToFix) {
										sb.Append("\r\n");
									} else if (hp == Utility.Configuration.Config.UI.MaxAkashiPerHP) {
										sb.Append("...\r\n");
										break;
									} else {
										sb.Append("|");
									}
								}
							}
						}
					}

					tooltip.SetToolTip( state.Label, sb.ToString() );

					emphasizesSubFleetInPort = false;
					index++;
				}

				//入渠中
				{
					long ntime = db.Docks.Values.Where( d => d.State == 1 && fleet.Members.Contains( d.ShipID ) ).Select( d => d.CompletionTime.Ticks ).DefaultIfEmpty().Max();

					if ( ntime > 0 ) {	//入渠中
						var state = GetStateLabel( index );

						state.Timer = new DateTime( ntime );
						state.SetInformation( FleetStates.Docking, "入渠中 " + DateTimeHelper.ToTimeRemainString( state.Timer ),
							DateTimeHelper.ToTimeRemainString( state.Timer ), (int)ResourceManager.IconContent.FleetDocking );

						tooltip.SetToolTip( state.Label, "完成时间 : " + DateTimeHelper.TimeToCSVString( state.Timer ) );

						emphasizesSubFleetInPort = false;
						index++;
					}

				}

				//未補給
				{
					int fuel = fleet.MembersInstance.Sum( ship => ship == null ? 0 : (int)( ( ship.FuelMax - ship.Fuel ) * ( ship.IsMarried ? 0.85 : 1.00 ) ) );
					int ammo = fleet.MembersInstance.Sum( ship => ship == null ? 0 : (int)( ( ship.AmmoMax - ship.Ammo ) * ( ship.IsMarried ? 0.85 : 1.00 ) ) );
					int aircraft = fleet.MembersInstance.Where( s => s != null ).SelectMany( s => s.MasterShip.Aircraft.Zip( s.Aircraft, ( max, now ) => max - now ) ).Sum();
					int bauxite = aircraft * 5;

					if ( fuel > 0 || ammo > 0 || bauxite > 0 ) {
						var state = GetStateLabel( index );

						state.SetInformation( FleetStates.NotReplenished, "未补给", "", (int)ResourceManager.IconContent.FleetNotReplenished );
						tooltip.SetToolTip( state.Label, string.Format( "油 : {0}\r\n弹 : {1}\r\n铝 : {2} ( 舰载机 {3} 架 )", fuel, ammo, bauxite, aircraft ) );

						index++;
					}
				}

				//疲労
				{
					int cond = fleet.MembersInstance.Min( s => s == null ? 100 : s.Condition );

					if ( cond < Utility.Configuration.Config.Control.ConditionBorder && fleet.ConditionTime != null ) {
						var state = GetStateLabel( index );

						int iconIndex;
						if ( cond < 20 )
							iconIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
						else if ( cond < 30 )
							iconIndex = (int)ResourceManager.IconContent.ConditionTired;
						else
							iconIndex = (int)ResourceManager.IconContent.ConditionLittleTired;

						state.Timer = (DateTime)fleet.ConditionTime;
						state.SetInformation( FleetStates.Tired, "疲劳 " + DateTimeHelper.ToTimeRemainString( state.Timer ),
							DateTimeHelper.ToTimeRemainString( state.Timer ), iconIndex );

						tooltip.SetToolTip( state.Label, string.Format( "预计恢复时间 : {0}\r\n( 预测误差 : {1})",
							DateTimeHelper.TimeToCSVString( state.Timer ), DateTimeHelper.ToTimeRemainString( TimeSpan.FromSeconds( db.Fleet.ConditionBorderAccuracy ) ) ) );

						index++;

					} else if ( cond >= 50 ) {		//戦意高揚
						var state = GetStateLabel( index );

						state.SetInformation( FleetStates.Sparkled, "战意高扬！", "", (int)ResourceManager.IconContent.ConditionSparkle );
						tooltip.SetToolTip( state.Label, string.Format( "最低 cond: {0}\r\n还可以远征 {1} 次", cond, Math.Ceiling( ( cond - 49 ) / 3.0 ) ) );

						index++;
					}

				}

				//出撃可能！
				if ( index == 0 ) {
					var state = GetStateLabel( index );

					state.SetInformation( FleetStates.Ready, "可以出击！", "", (int)ResourceManager.IconContent.FleetReady );
					tooltip.SetToolTip( state.Label, null );

					index++;
				}

			}


			if ( emphasizesSubFleetInPort ) {
				for ( int i = 0; i < index; i++ ) {
					if ( StateLabels[i].Label.BackColor == Color.Transparent) {
						StateLabels[i].Label.BackColor = Utility.Configuration.Config.UI.FleetOverview_AlertNotInExpeditionBG;
						StateLabels[i].Label.ForeColor = Utility.Configuration.Config.UI.FleetOverview_AlertNotInExpeditionFG;
					}
				}
			}


			for ( int i = index; i < StateLabels.Count; i++ )
				StateLabels[i].Enabled = false;


			if ( index == 1 ) {
				StateLabels[0].AutoShorten = false;

			} else {
				for ( int i = 0; i < index; i++ )
					StateLabels[i].AutoShorten = true;
			}

		}


		public void RefreshFleetState() {

			var configUI = Utility.Configuration.Config.UI;

			foreach ( var state in StateLabels ) {

				if ( !state.Enabled )
					continue;

				switch ( state.State ) {

					case FleetStates.Damaged:
						if ( Utility.Configuration.Config.FormFleet.BlinkAtDamaged) {
							state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? configUI.FleetOverview_ShipDamagedBG : Color.Transparent;
							state.Label.ForeColor = DateTime.Now.Second % 2 == 0 ? configUI.FleetOverview_ShipDamagedFG : configUI.ForeColor;
						}
						break;

					case FleetStates.SortieDamaged:
						state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? configUI.FleetOverview_ShipDamagedBG : Color.Transparent;
						state.Label.ForeColor = DateTime.Now.Second % 2 == 0 ? configUI.FleetOverview_ShipDamagedFG : configUI.ForeColor;
						break;

					case FleetStates.Docking:
						state.ShortenedText = DateTimeHelper.ToTimeRemainString( state.Timer );
						state.Text = "入渠中 " + state.ShortenedText;
						state.UpdateText();
						if ( Utility.Configuration.Config.FormFleet.BlinkAtCompletion && ( state.Timer - DateTime.Now ).TotalMilliseconds <= Utility.Configuration.Config.NotifierRepair.AccelInterval) {
							state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? configUI.Dock_RepairFinishedBG : Color.Transparent;
							state.Label.ForeColor = DateTime.Now.Second % 2 == 0 ? configUI.Dock_RepairFinishedFG : configUI.ForeColor;
						}
						break;

					case FleetStates.Expedition:
						state.ShortenedText = DateTimeHelper.ToTimeRemainString( state.Timer );
						state.Text = "远征中 " + state.ShortenedText;
						state.UpdateText();
						if ( Utility.Configuration.Config.FormFleet.BlinkAtCompletion && ( state.Timer - DateTime.Now ).TotalMilliseconds <= Utility.Configuration.Config.NotifierExpedition.AccelInterval ) {
							state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? configUI.FleetOverview_ExpeditionOverBG : Color.Transparent;
							state.Label.ForeColor = DateTime.Now.Second % 2 == 0 ? configUI.FleetOverview_ExpeditionOverFG : configUI.ForeColor;
						}
						break;

					case FleetStates.Tired:
						state.ShortenedText = DateTimeHelper.ToTimeRemainString( state.Timer );
						state.Text = "疲劳 " + state.ShortenedText;
						state.UpdateText();
						if ( Utility.Configuration.Config.FormFleet.BlinkAtCompletion && ( state.Timer - DateTime.Now ).TotalMilliseconds <= 0 ) {
							state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? configUI.FleetOverview_TiredRecoveredBG : Color.Transparent;
							state.Label.ForeColor = DateTime.Now.Second % 2 == 0 ? configUI.FleetOverview_TiredRecoveredFG : configUI.ForeColor;
						}
						break;

					case FleetStates.AnchorageRepairing:
						state.ShortenedText = DateTimeHelper.ToTimeElapsedString( KCDatabase.Instance.Fleet.AnchorageRepairingTimer );
						state.Text = "修理中 " + state.ShortenedText;
						state.UpdateText();
						break;

				}

			}

		}

		public int GetIconIndex() {
			var first = StateLabels.Where( s => s.Enabled ).OrderBy( s => s.State ).FirstOrDefault();
			return first == null ? (int)ResourceManager.IconContent.FormFleet : first.Label.ImageIndex;
		}

	}

	/// <summary>
	/// 艦隊の状態を表します。
	/// </summary>
	public enum FleetStates {
		NoShip,
		SortieDamaged,
		Sortie,
		Expedition,
		Damaged,
		AnchorageRepairing,
		Docking,
		NotReplenished,
		Tired,
		Sparkled,
		Ready,
	}
}
