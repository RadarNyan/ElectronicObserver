﻿using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
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

	public partial class FormArsenal : DockContent {

		private class TableArsenalControl {

			public Label ShipName;
			public Label CompletionTime;
			private ToolTip tooltip;

			public TableArsenalControl( FormArsenal parent ) {

				#region Initialize

				ShipName = new ImageLabel();
				ShipName.Text = "???";
				ShipName.Anchor = AnchorStyles.Left;
				ShipName.TextAlign = ContentAlignment.MiddleLeft;
				ShipName.Padding = new Padding( 0, 1, 0, 1 );
				ShipName.Margin = new Padding( 2, 0, 2, 0 );
				ShipName.MaximumSize = new Size( 60, 20 );
				//ShipName.AutoEllipsis = true;
				ShipName.ImageAlign = ContentAlignment.MiddleCenter;
				ShipName.AutoSize = true;
				ShipName.Visible = true;

				CompletionTime = new Label();
				CompletionTime.Text = "";
				CompletionTime.Anchor = AnchorStyles.Left;
				CompletionTime.Tag = null;
				CompletionTime.TextAlign = ContentAlignment.MiddleLeft;
				CompletionTime.Padding = new Padding( 0, 1, 0, 1 );
				CompletionTime.Margin = new Padding( 2, 0, 2, 0 );
				CompletionTime.MinimumSize = new Size( 60, 10 );
				CompletionTime.AutoSize = true;
				CompletionTime.Visible = true;

				ConfigurationChanged( parent );

				tooltip = parent.ToolTipInfo;
				#endregion

			}


			public TableArsenalControl( FormArsenal parent, TableLayoutPanel table, int row )
				: this( parent ) {

				AddToTable( table, row );
			}


			public void AddToTable( TableLayoutPanel table, int row ) {

				table.Controls.Add( ShipName, 0, row );
				table.Controls.Add( CompletionTime, 1, row );

				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.Absolute, 21 );

				if ( table.RowStyles.Count > row )
					table.RowStyles[row] = rs;
				else
					while ( table.RowStyles.Count <= row )
						table.RowStyles.Add( rs );
				#endregion

			}


			public void Update( int arsenalID ) {

				KCDatabase db = KCDatabase.Instance;
				ArsenalData arsenal = db.Arsenals[arsenalID];
				bool showShipName = Utility.Configuration.Config.FormArsenal.ShowShipName;

				CompletionTime.BackColor = Color.Transparent;
				CompletionTime.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				tooltip.SetToolTip( ShipName, null );
				tooltip.SetToolTip( CompletionTime, null );

				if ( arsenal == null || arsenal.State == -1 ) {
					//locked
					ShipName.Text = "";
					CompletionTime.Text = "";
					CompletionTime.Tag = null;

				} else if ( arsenal.State == 0 ) {
					//empty
					ShipName.Text = "----";
					CompletionTime.Text = "";
					CompletionTime.Tag = null;

				} else if ( arsenal.State == 2 ) {
					//building
					string name = showShipName ? db.MasterShips[arsenal.ShipID].Name : "???";
					ShipName.Text = name;
					tooltip.SetToolTip( ShipName, name );
					CompletionTime.Text = DateTimeHelper.ToTimeRemainString( arsenal.CompletionTime );
					CompletionTime.Tag = arsenal.CompletionTime;
					tooltip.SetToolTip( CompletionTime, "完成时间 : " + arsenal.CompletionTime.ToString() );

				} else if ( arsenal.State == 3 ) {
					//complete!
					string name = showShipName ? db.MasterShips[arsenal.ShipID].Name : "???";
					ShipName.Text = name;
					tooltip.SetToolTip( ShipName, name );
					CompletionTime.Text = "完成！";
					CompletionTime.Tag = null;

				}

			}


			public void Refresh( int arsenalID ) {

				if ( CompletionTime.Tag != null ) {

					var time = (DateTime)CompletionTime.Tag;

					CompletionTime.Text = DateTimeHelper.ToTimeRemainString( time );

					if ( Utility.Configuration.Config.FormArsenal.BlinkAtCompletion && ( time - DateTime.Now ).TotalMilliseconds <= Utility.Configuration.Config.NotifierConstruction.AccelInterval ) {
						CompletionTime.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Blink_BackColorLightGreen : Color.Transparent;
						CompletionTime.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Blink_ForeColor : Utility.Configuration.Config.UI.ForeColor;
					}

				} else if ( Utility.Configuration.Config.FormArsenal.BlinkAtCompletion && !string.IsNullOrWhiteSpace( CompletionTime.Text ) ) {
					//完成しているので
					CompletionTime.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Blink_BackColorLightGreen : Color.Transparent;
					CompletionTime.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Blink_ForeColor : Utility.Configuration.Config.UI.ForeColor;
				}
			}


			public void ConfigurationChanged( FormArsenal parent ) {
				ShipName.Font = parent.Font;
				CompletionTime.Font = parent.Font;
				CompletionTime.BackColor = Color.Transparent;
				ShipName.ForeColor = CompletionTime.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			}

		}


		private TableArsenalControl[] ControlArsenal;
		private int _buildingID;

		public FormArsenal( FormMain parent ) {
			InitializeComponent();

			Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;

			ControlHelper.SetDoubleBuffered( TableArsenal );

			TableArsenal.SuspendLayout();
			ControlArsenal = new TableArsenalControl[4];
			for ( int i = 0; i < ControlArsenal.Length; i++ ) {
				ControlArsenal[i] = new TableArsenalControl( this, TableArsenal, i );
			}
			TableArsenal.ResumeLayout();

			_buildingID = -1;

			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormArsenal] );
		}



		private void FormArsenal_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			o["api_req_kousyou/createship"].RequestReceived += Updated;
			o["api_req_kousyou/createship_speedchange"].RequestReceived += Updated;

			o["api_get_member/kdock"].ResponseReceived += Updated;
			o["api_req_kousyou/getship"].ResponseReceived += Updated;
			o["api_get_member/require_info"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		}


		void Updated( string apiname, dynamic data ) {

			if ( _buildingID != -1 && apiname == "api_get_member/kdock" ) {

				ArsenalData arsenal = KCDatabase.Instance.Arsenals[_buildingID];
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[arsenal.ShipID];
				string name;

				if ( Utility.Configuration.Config.Log.ShowSpoiler && Utility.Configuration.Config.FormArsenal.ShowShipName ) {

					name = string.Format( "{0}「{1}」", ship.ShipTypeName, ship.NameWithClass );

					Utility.Logger.Add(2, "", string.Format("工厂船坞 #{0} 开始建造 : ", _buildingID),
						string.Format("{0}。({1}/{2}/{3}/{4}-{5})",
							name,
							arsenal.Fuel,
							arsenal.Ammo,
							arsenal.Steel,
							arsenal.Bauxite,
							arsenal.DevelopmentMaterial),
						" 秘书舰 : ", KCDatabase.Instance.Fleet[1].MembersInstance[0].NameWithLevel);
				} else {

					Utility.Logger.Add(2, "", string.Format("工厂船坞 #{0} 开始建造新舰娘。", _buildingID),
					string.Format("({0}/{1}/{2}/{3}-{4})",
						arsenal.Fuel,
						arsenal.Ammo,
						arsenal.Steel,
						arsenal.Bauxite,
						arsenal.DevelopmentMaterial),
					" 秘书舰 : ", KCDatabase.Instance.Fleet[1].MembersInstance[0].NameWithLevel);
				}

				_buildingID = -1;
			}

			if ( apiname == "api_req_kousyou/createship" ) {
				_buildingID = int.Parse( data["api_kdock_id"] );
			}

			UpdateUI();
		}

		void UpdateUI() {

			if ( ControlArsenal == null ) return;

			TableArsenal.SuspendLayout();
			for ( int i = 0; i < ControlArsenal.Length; i++ )
				ControlArsenal[i].Update( i + 1 );
			TableArsenal.ResumeLayout();

		}

		void UpdateTimerTick() {

			TableArsenal.SuspendLayout();
			for ( int i = 0; i < ControlArsenal.Length; i++ )
				ControlArsenal[i].Refresh( i + 1 );
			TableArsenal.ResumeLayout();

		}


		void ConfigurationChanged() {

			Font = Utility.Configuration.Config.UI.JapFont;
			MenuMain_ShowShipName.Checked = Utility.Configuration.Config.FormArsenal.ShowShipName;

			if ( ControlArsenal != null ) {
				foreach ( var c in ControlArsenal )
					c.ConfigurationChanged( this );
			}
		}


		private void MenuMain_ShowShipName_CheckedChanged( object sender, EventArgs e ) {
			Utility.Configuration.Config.FormArsenal.ShowShipName = MenuMain_ShowShipName.Checked;

			UpdateUI();
		}


		private void TableArsenal_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}



		protected override string GetPersistString() {
			return "Arsenal";
		}



	}

}
