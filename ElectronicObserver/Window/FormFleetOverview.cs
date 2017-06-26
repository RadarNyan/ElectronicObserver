﻿using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	public partial class FormFleetOverview : DockContent {

		private class TableFleetControl {

			public ImageLabel Number;
			public ImageLabel State;
			public ToolTip ToolTipInfo;
			private int fleetID;

			public TableFleetControl( FormFleetOverview parent, int fleetID ) {

				#region Initialize

				Number = new ImageLabel();
				Number.Anchor = AnchorStyles.Left;
				Number.ImageAlign = ContentAlignment.MiddleCenter;
				Number.Padding = new Padding( 0, 1, 0, 1 );
				Number.Margin = new Padding( 2, 1, 2, 1 );
				Number.Text = string.Format( "#{0}:", fleetID );
				Number.AutoSize = true;
				Number.Tag = null;

				State = new ImageLabel();
				State.Anchor = AnchorStyles.Left;
				State.Padding = new Padding( 0, 1, 0, 1 );
				State.Margin = new Padding( 2, 1, 2, 1 );
				State.ImageList = ResourceManager.Instance.Icons;
				State.Text = "-";
				State.AutoSize = true;
				State.Tag = FormFleet.FleetStates.NoShip;

				ConfigurationChanged( parent );

				this.fleetID = fleetID;
				ToolTipInfo = parent.ToolTipInfo;

				#endregion

			}

			public TableFleetControl( FormFleetOverview parent, int fleetID, TableLayoutPanel table )
				: this( parent, fleetID ) {

				AddToTable( table, fleetID - 1 );
			}

			public void AddToTable( TableLayoutPanel table, int row ) {

				table.Controls.Add( Number, 0, row );
				table.Controls.Add( State, 1, row );

			}


			public void Update() {

				FleetData fleet =  KCDatabase.Instance.Fleet[fleetID];
				if ( fleet == null ) return;

				DateTime dt = (DateTime?)Number.Tag ?? DateTime.Now;
				State.Tag = FormFleet.UpdateFleetState( fleet, State, ToolTipInfo, (FormFleet.FleetStates)State.Tag, ref dt );
				Number.Tag = dt;

				ToolTipInfo.SetToolTip( Number, fleet.Name );
			}

			public void ResetState() {
				State.Tag = FormFleet.FleetStates.NoShip;
			}

			public void Refresh() {

				FormFleet.RefreshFleetState( State, (FormFleet.FleetStates)State.Tag, (DateTime?)Number.Tag ?? DateTime.Now );
			}


			public void ConfigurationChanged( FormFleetOverview parent ) {
				Number.Font = parent.Font;
				State.Font = parent.Font;
				State.BackColor = Color.Transparent;
				Update();
			}
		}


		private List<TableFleetControl> ControlFleet;
		private ImageLabel CombinedTag;
		private ImageLabel AnchorageRepairingTimer;


		public FormFleetOverview( FormMain parent ) {
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( TableFleet );


			ControlFleet = new List<TableFleetControl>( 4 );
			for ( int i = 0; i < 4; i++ ) {
				ControlFleet.Add( new TableFleetControl( this, i + 1, TableFleet ) );
			}

			{
				AnchorageRepairingTimer = new ImageLabel();
				AnchorageRepairingTimer.Anchor = AnchorStyles.Left;
				AnchorageRepairingTimer.Padding = new Padding( 0, 1, 0, 1 );
				AnchorageRepairingTimer.Margin = new Padding( 2, 1, 2, 1 );
				AnchorageRepairingTimer.ImageList = ResourceManager.Instance.Icons;
				AnchorageRepairingTimer.ImageIndex = (int)ResourceManager.IconContent.FleetDocking;
				AnchorageRepairingTimer.Text = "-";
				AnchorageRepairingTimer.AutoSize = true;
				//AnchorageRepairingTimer.Visible = false;

				TableFleet.Controls.Add( AnchorageRepairingTimer, 1, 4 );

			}

			#region CombinedTag
			{
				CombinedTag = new ImageLabel();
				CombinedTag.Anchor = AnchorStyles.Left;
				CombinedTag.Padding = new Padding( 0, 1, 0, 1 );
				CombinedTag.Margin = new Padding( 2, 1, 2, 1 );
				CombinedTag.ImageList = ResourceManager.Instance.Icons;
				CombinedTag.ImageIndex = (int)ResourceManager.IconContent.FleetCombined;
				CombinedTag.Text = "-";
				CombinedTag.AutoSize = true;
				CombinedTag.Visible = false;

				TableFleet.Controls.Add( CombinedTag, 1, 5 );

			}
			#endregion



			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet] );

			Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;
		}



		private void FormFleetOverview_Load( object sender, EventArgs e ) {



			//api register
			APIObserver o = APIObserver.Instance;

			o.APIList["api_req_hensei/change"].RequestReceived += ChangeOrganization;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += ChangeOrganization;
			o.APIList["api_req_kaisou/remodeling"].RequestReceived += ChangeOrganization;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += ChangeOrganization;
			o.APIList["api_req_hensei/preset_select"].ResponseReceived += ChangeOrganization;

			o.APIList["api_req_nyukyo/start"].RequestReceived += Updated;
			o.APIList["api_req_nyukyo/speedchange"].RequestReceived += Updated;
			o.APIList["api_req_hensei/change"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o.APIList["api_req_member/updatedeckname"].RequestReceived += Updated;
			o.APIList["api_req_map/start"].RequestReceived += Updated;
			o.APIList["api_req_hensei/combined"].RequestReceived += Updated;

			o.APIList["api_port/port"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship2"].ResponseReceived += Updated;
			o.APIList["api_get_member/ndock"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/getship"].ResponseReceived += Updated;
			o.APIList["api_req_hokyu/charge"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship3"].ResponseReceived += Updated;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += Updated;		//requestのほうは面倒なのでこちらでまとめてやる
			o.APIList["api_get_member/deck"].ResponseReceived += Updated;
			o.APIList["api_req_map/start"].ResponseReceived += Updated;
			o.APIList["api_req_map/next"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship_deck"].ResponseReceived += Updated;
			o.APIList["api_req_hensei/preset_select"].ResponseReceived += Updated;
			o.APIList["api_req_kaisou/slot_exchange_index"].ResponseReceived += Updated;
			o.APIList["api_get_member/require_info"].ResponseReceived += Updated;
			o.APIList["api_req_kaisou/slot_deprive"].ResponseReceived += Updated;


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}

		void ConfigurationChanged() {

			TableFleet.SuspendLayout();

			Font = Utility.Configuration.Config.UI.MainFont;

			AutoScroll = Utility.Configuration.Config.FormFleet.IsScrollable;

			foreach ( var c in ControlFleet )
				c.ConfigurationChanged( this );

			CombinedTag.Font = Font;
			AnchorageRepairingTimer.Font = Font;
			AnchorageRepairingTimer.Visible = Utility.Configuration.Config.FormFleet.ShowAnchorageRepairingTimer;

			ControlHelper.SetTableRowStyles( TableFleet, ControlHelper.GetDefaultRowStyle() );

			TableFleet.ResumeLayout();
		}


		private void Updated( string apiname, dynamic data ) {

			TableFleet.SuspendLayout();

			TableFleet.RowCount = KCDatabase.Instance.Fleet.Fleets.Values.Count( f => f.IsAvailable );
			for ( int i = 0; i < ControlFleet.Count; i++ ) {
				ControlFleet[i].Update();
			}

			if ( KCDatabase.Instance.Fleet.CombinedFlag > 0 ) {
				CombinedTag.Text = Constants.GetCombinedFleet( KCDatabase.Instance.Fleet.CombinedFlag );

				var fleet1 = KCDatabase.Instance.Fleet[1];
				var fleet2 = KCDatabase.Instance.Fleet[2];

				int tp = Calculator.GetTPDamage( fleet1 ) + Calculator.GetTPDamage( fleet2 );

				ToolTipInfo.SetToolTip( CombinedTag, string.Format( "载有运输桶 : {0} 个\r\n载有大发动艇 : {1} 艘\r\n运输量 (TP) : S {2} / A {3}\r\n\r\n总制空值 : {4}\r\n总索敌值 : {5:f2}\r\n新判定式(33) :\r\n　分歧点系数 1 : {6:f2}\r\n　分歧点系数 3 : {7:f2}\r\n　分歧点系数 4 : {8:f2}",
					fleet1.MembersWithoutEscaped.Sum( s => s == null ? 0 : s.AllSlotInstanceMaster.Count( eq => eq != null && eq.CategoryType == 30 ) ) +
					fleet2.MembersWithoutEscaped.Sum( s => s == null ? 0 : s.AllSlotInstanceMaster.Count( eq => eq != null && eq.CategoryType == 30 ) ),
					fleet1.MembersWithoutEscaped.Sum( s => s == null ? 0 : s.AllSlotInstanceMaster.Count( eq => eq != null && eq.CategoryType == 24 ) ) +
					fleet2.MembersWithoutEscaped.Sum( s => s == null ? 0 : s.AllSlotInstanceMaster.Count( eq => eq != null && eq.CategoryType == 24 ) ),
					tp,
					(int)Math.Floor( tp * 0.7 ),
					Calculator.GetAirSuperiority( fleet1 ) + Calculator.GetAirSuperiority( fleet2 ),
					Math.Floor( fleet1.GetSearchingAbility() * 100 ) / 100 + Math.Floor( fleet2.GetSearchingAbility() * 100 ) / 100,
					Math.Floor( Calculator.GetSearchingAbility_New33( fleet1, 1 ) * 100 ) / 100 + Math.Floor( Calculator.GetSearchingAbility_New33( fleet2, 1 ) * 100 ) / 100,
					Math.Floor( Calculator.GetSearchingAbility_New33( fleet1, 3 ) * 100 ) / 100 + Math.Floor( Calculator.GetSearchingAbility_New33( fleet2, 3 ) * 100 ) / 100,
					Math.Floor( Calculator.GetSearchingAbility_New33( fleet1, 4 ) * 100 ) / 100 + Math.Floor( Calculator.GetSearchingAbility_New33( fleet2, 4 ) * 100 ) / 100
					) );


				CombinedTag.Visible = true;
			} else {
				CombinedTag.Visible = false;
			}

			if ( KCDatabase.Instance.Fleet.AnchorageRepairingTimer > DateTime.MinValue ) {
				AnchorageRepairingTimer.Text = DateTimeHelper.ToTimeElapsedString( KCDatabase.Instance.Fleet.AnchorageRepairingTimer );
				AnchorageRepairingTimer.Tag = KCDatabase.Instance.Fleet.AnchorageRepairingTimer;
				ToolTipInfo.SetToolTip( AnchorageRepairingTimer, "泊地修理计时器\r\n开始 : " + DateTimeHelper.TimeToCSVString( KCDatabase.Instance.Fleet.AnchorageRepairingTimer ) + "\r\n恢复 : " + DateTimeHelper.TimeToCSVString( KCDatabase.Instance.Fleet.AnchorageRepairingTimer.AddMinutes( 20 ) ) );
			}

			TableFleet.ResumeLayout();
		}

		void ChangeOrganization( string apiname, dynamic data ) {

			for ( int i = 0; i < ControlFleet.Count; i++ )
				ControlFleet[i].ResetState();

		}


		void UpdateTimerTick() {
			for ( int i = 0; i < ControlFleet.Count; i++ ) {
				ControlFleet[i].Refresh();
			}

			if ( AnchorageRepairingTimer.Visible && AnchorageRepairingTimer.Tag != null )
				AnchorageRepairingTimer.Text = DateTimeHelper.ToTimeElapsedString( (DateTime)AnchorageRepairingTimer.Tag );
		}



		private void TableFleet_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine(Utility.Configuration.Config.UI.SubBackColorPen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

		}



		protected override string GetPersistString() {
			return "FleetOverview";
		}

	}

}
