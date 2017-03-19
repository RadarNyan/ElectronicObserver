﻿using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {
	public partial class FormBaseAirCorps : DockContent {


		private class TableBaseAirCorpsControl {

			public ImageLabel Name;
			public ImageLabel ActionKind;
			public ImageLabel AirSuperiority;
			public ImageLabel Distance;
			public ShipStatusEquipment Squadrons;

			public ToolTip ToolTipInfo;

			public TableBaseAirCorpsControl( FormBaseAirCorps parent ) {

				#region Initialize

				Name = new ImageLabel();
				Name.Name = "Name";
				Name.Text = "*";
				Name.Anchor = AnchorStyles.Left;
				Name.TextAlign = ContentAlignment.MiddleLeft;
				Name.ImageAlign = ContentAlignment.MiddleRight;
				Name.ImageList = ResourceManager.Instance.Icons;
				Name.Padding = new Padding( 0, 1, 0, 1 );
				Name.Margin = new Padding( 2, 0, 2, 0 );
				Name.AutoSize = true;
				Name.ContextMenuStrip = parent.ContextMenuBaseAirCorps;
				Name.Visible = false;
				Name.Cursor = Cursors.Help;

				ActionKind = new ImageLabel();
				ActionKind.Text = "*";
				ActionKind.Anchor = AnchorStyles.Left;
				ActionKind.TextAlign = ContentAlignment.MiddleLeft;
				ActionKind.ImageAlign = ContentAlignment.MiddleCenter;
				//ActionKind.ImageList =
				ActionKind.Padding = new Padding( 0, 1, 0, 1 );
				ActionKind.Margin = new Padding( 2, 0, 2, 0 );
				ActionKind.AutoSize = true;
				ActionKind.Visible = false;

				AirSuperiority = new ImageLabel();
				AirSuperiority.Text = "*";
				AirSuperiority.Anchor = AnchorStyles.Left;
				AirSuperiority.TextAlign = ContentAlignment.MiddleLeft;
				AirSuperiority.ImageAlign = ContentAlignment.MiddleLeft;
				AirSuperiority.ImageList = ResourceManager.Instance.Equipments;
				AirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;
				AirSuperiority.Padding = new Padding( 0, 1, 0, 1 );
				AirSuperiority.Margin = new Padding( 2, 0, 2, 0 );
				AirSuperiority.AutoSize = true;
				AirSuperiority.Visible = false;

				Distance = new ImageLabel();
				Distance.Text = "*";
				Distance.Anchor = AnchorStyles.Left;
				Distance.TextAlign = ContentAlignment.MiddleLeft;
				Distance.ImageAlign = ContentAlignment.MiddleLeft;
				Distance.ImageList = ResourceManager.Instance.Icons;
				Distance.ImageIndex = (int)ResourceManager.IconContent.ParameterAircraftDistance;
				Distance.Padding = new Padding( 0, 1, 0, 1 );
				Distance.Margin = new Padding( 2, 0, 2, 0 );
				Distance.AutoSize = true;
				Distance.Visible = false;

				Squadrons = new ShipStatusEquipment();
				Squadrons.Anchor = AnchorStyles.Left;
				Squadrons.Padding = new Padding( 0, 2, 0, 1 );
				Squadrons.Margin = new Padding( 2, 0, 2, 0 );
				Squadrons.Size = new Size( 40, 20 );
				Squadrons.AutoSize = true;
				Squadrons.Visible = false;
				Squadrons.ResumeLayout();

				ConfigurationChanged( parent );

				ToolTipInfo = parent.ToolTipInfo;

				#endregion

			}


			public TableBaseAirCorpsControl( FormBaseAirCorps parent, TableLayoutPanel table, int row )
				: this( parent ) {
				AddToTable( table, row );
			}

			public void AddToTable( TableLayoutPanel table, int row ) {

				table.SuspendLayout();
				table.Controls.Add( Name, 0, row );
				table.Controls.Add( ActionKind, 1, row );
				table.Controls.Add( AirSuperiority, 2, row );
				table.Controls.Add( Distance, 3, row );
				table.Controls.Add( Squadrons, 4, row );
				table.ResumeLayout();

				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.Absolute, 21 );

				if ( table.RowStyles.Count > row )
					table.RowStyles[row] = rs;
				else
					while ( table.RowStyles.Count <= row )
						table.RowStyles.Add( rs );
				#endregion
			}


			public void Update( int baseAirCorpsID ) {

				KCDatabase db = KCDatabase.Instance;
				var corps = db.BaseAirCorps[baseAirCorpsID];

				if ( corps == null || !KCDatabase.Instance.MapArea.ContainsKey(corps.MapAreaID) ) {
					baseAirCorpsID = -1;

				} else {

					Name.Text = string.Format( "#{0} - {1}", corps.MapAreaID, corps.Name );
					Name.Tag = corps.MapAreaID;
					var sb = new StringBuilder();
					sb.AppendLine( "所属海域 : " + KCDatabase.Instance.MapArea[corps.MapAreaID].Name );

					// state 
					if ( corps.Squadrons.Values.Any( sq => sq != null && sq.AircraftCurrent < sq.AircraftMax ) ) {
						// 未補給
						Name.ImageAlign = ContentAlignment.MiddleRight;
						Name.ImageIndex = (int)ResourceManager.IconContent.FleetNotReplenished;
						sb.AppendLine( "未补给" );

					} else if ( corps.Squadrons.Values.Any( sq => sq != null && sq.Condition > 1 ) ) {
						// 疲労
						int tired = corps.Squadrons.Values.Max( sq => sq != null ? sq.Condition : 0 );

						if ( tired == 2 ) {
							Name.ImageAlign = ContentAlignment.MiddleRight;
							Name.ImageIndex = (int)ResourceManager.IconContent.ConditionTired;
							sb.AppendLine( "疲劳" );

						} else {
							Name.ImageAlign = ContentAlignment.MiddleRight;
							Name.ImageIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
							sb.AppendLine( "过劳" );

						}

					} else {
						Name.ImageAlign = ContentAlignment.MiddleCenter;
						Name.ImageIndex = -1;

					}
					ToolTipInfo.SetToolTip( Name, sb.ToString() );


					ActionKind.Text = "[" + Constants.GetBaseAirCorpsActionKind( corps.ActionKind ) + "]";

					{
						int airSuperiority = Calculator.GetAirSuperiority( corps );
						AirSuperiority.Text = airSuperiority.ToString();
						ToolTipInfo.SetToolTip( AirSuperiority,
							string.Format( "确保 : {0}\r\n优势 : {1}\r\n均衡 : {2}\r\n劣势 : {3}\r\n",
							(int)( airSuperiority / 3.0 ),
							(int)( airSuperiority / 1.5 ),
							Math.Max( (int)( airSuperiority * 1.5 - 1 ), 0 ),
							Math.Max( (int)( airSuperiority * 3.0 - 1 ), 0 ) ) );
					}

					Distance.Text = corps.Distance.ToString();

					Squadrons.SetSlotList( corps );
					ToolTipInfo.SetToolTip( Squadrons, GetEquipmentString( corps ) );

				}


				Name.Visible =
				ActionKind.Visible =
				AirSuperiority.Visible =
				Distance.Visible =
				Squadrons.Visible =
					baseAirCorpsID != -1;
			}


			public void ConfigurationChanged( FormBaseAirCorps parent ) {
				var config = Utility.Configuration.Config;

				var mainfont = config.UI.JapFont;
				var subfont = config.UI.JapFont2;

				Name.Font = mainfont;
				ActionKind.Font = Utility.Configuration.Config.UI.MainFont;
				AirSuperiority.Font = mainfont;
				Distance.Font = mainfont;
				Squadrons.Font = subfont;

				Squadrons.ShowAircraft = config.FormFleet.ShowAircraft;
				Squadrons.ShowAircraftLevelByNumber = config.FormFleet.ShowAircraftLevelByNumber;
				Squadrons.LevelVisibility = config.FormFleet.EquipmentLevelVisibility;
			}


			private string GetEquipmentString( BaseAirCorpsData corps ) {
				var sb = new StringBuilder();

				if ( corps == null )
					return "( 未开放 )\r\n";

				foreach ( var squadron in corps.Squadrons.Values ) {
					if ( squadron == null )
						continue;

					var eq = squadron.EquipmentInstance;

					switch ( squadron.State ) {
						case 0:		// 未配属
						default:
							sb.AppendLine( "( 无 )" );
							break;

						case 1:		// 配属済み
							if ( eq == null )
								goto case 0;
							sb.AppendFormat( "[{0}/{1}] ",
								squadron.AircraftCurrent,
								squadron.AircraftMax );

							switch ( squadron.Condition ) {
								case 1:
								default:
									break;
								case 2:
									sb.Append( "[疲劳] " );
									break;
								case 3:
									sb.Append( "[过劳] " );
									break;
							}

							sb.AppendLine( eq.NameWithLevel );
							break;

						case 2:		// 配置転換中
							sb.AppendFormat( "配置转换中 ( 开始时间 : {0} )\r\n",
								DateTimeHelper.TimeToCSVString( squadron.RelocatedTime ) );
							break;
					}
				}

				return sb.ToString();
			}

		}


		private TableBaseAirCorpsControl[] ControlMember;

		public FormBaseAirCorps( FormMain parent ) {
			InitializeComponent();


			ControlMember = new TableBaseAirCorpsControl[9];
			TableMember.SuspendLayout();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i] = new TableBaseAirCorpsControl( this, TableMember, i );
			}
			TableMember.ResumeLayout();

			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBaseAirCorps] );
		}

		private void FormBaseAirCorps_Load( object sender, EventArgs e ) {

			var api = Observer.APIObserver.Instance;

			api["api_port/port"].ResponseReceived += Updated;
			api["api_get_member/mapinfo"].ResponseReceived += Updated;
			api["api_get_member/base_air_corps"].ResponseReceived += Updated;
			api["api_req_air_corps/change_name"].ResponseReceived += Updated;
			api["api_req_air_corps/set_action"].ResponseReceived += Updated;
			api["api_req_air_corps/set_plane"].ResponseReceived += Updated;
			api["api_req_air_corps/supply"].ResponseReceived += Updated;
			api["api_req_air_corps/expand_base"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		}


		private void ConfigurationChanged() {

			var c = Utility.Configuration.Config;

			Font = c.UI.MainFont;

			foreach ( var control in ControlMember )
				control.ConfigurationChanged( this );

		}


		void Updated( string apiname, dynamic data ) {

			var keys = KCDatabase.Instance.BaseAirCorps.Keys;

			TableMember.SuspendLayout();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i].Update( i < keys.Count() ? keys.ElementAt( i ) : -1 );
			}
			TableMember.ResumeLayout();

		}


		private void ContextMenuBaseAirCorps_Opening( object sender, System.ComponentModel.CancelEventArgs e ) {
			if ( KCDatabase.Instance.BaseAirCorps.Count == 0 ) {
				e.Cancel = true;
				return;
			}

			if ( ContextMenuBaseAirCorps.SourceControl.Name == "Name" )
				ContextMenuBaseAirCorps_CopyOrganization.Tag = ContextMenuBaseAirCorps.SourceControl.Tag as int? ?? -1;
			else
				ContextMenuBaseAirCorps_CopyOrganization.Tag = -1;
		}

		private void ContextMenuBaseAirCorps_CopyOrganization_Click( object sender, EventArgs e ) {

			var sb = new StringBuilder();
			int areaid = ContextMenuBaseAirCorps_CopyOrganization.Tag as int? ?? -1;

			var baseaircorps = KCDatabase.Instance.BaseAirCorps.Values;
			if ( areaid != -1 )
				baseaircorps = baseaircorps.Where( c => c.MapAreaID == areaid );

			foreach ( var corps in baseaircorps ) {

				if(!KCDatabase.Instance.MapArea.ContainsKey(corps.MapAreaID)) continue;

				sb.AppendFormat( "{0}\t[{1}] 制空战力 {2} / 战斗行动半径 {3}\r\n",
					( areaid == -1 ? ( KCDatabase.Instance.MapArea[corps.MapAreaID].Name + "：" ) : "" ) + corps.Name,
					Constants.GetBaseAirCorpsActionKind( corps.ActionKind ),
					Calculator.GetAirSuperiority( corps ),
					corps.Distance );

				var sq = corps.Squadrons.Values.ToArray();

				for ( int i = 0; i < sq.Length; i++ ) {
					if ( i > 0 )
						sb.Append( "/" );

					if ( sq[i] == null ) {
						sb.Append( "( 消息不明 )" );
						continue;
					}

					switch ( sq[i].State ) {
						case 0:
							sb.Append( "( 未配属 )" );
							break;
						case 1: {
								var eq = sq[i].EquipmentInstance;

								sb.Append( eq == null ? "( 无 )" : eq.NameWithLevel );

								if ( sq[i].AircraftCurrent < sq[i].AircraftMax )
									sb.AppendFormat( "[{0}/{1}]", sq[i].AircraftCurrent, sq[i].AircraftMax );
							} break;
						case 2:
							sb.Append( "( 配置转换中 )" );
							break;
					}
				}

				sb.AppendLine();
			}

			Clipboard.SetData( DataFormats.StringFormat, sb.ToString() );
		}

		private void ContextMenuBaseAirCorps_DisplayRelocatedEquipments_Click( object sender, EventArgs e ) {

			string message = string.Join( "\r\n", KCDatabase.Instance.RelocatedEquipments.Values
				.Where( eq => eq.EquipmentInstance != null )
				.Select( eq => string.Format( "{0} ({1}～)", eq.EquipmentInstance.NameWithLevel, DateTimeHelper.TimeToCSVString( eq.RelocatedTime ) ) ) );

			if ( message.Length == 0 )
				message = "现在没有装备正在配置转换中。";

			MessageBox.Show( message, "配置转换中的装备", MessageBoxButtons.OK, MessageBoxIcon.Information );
		}


		private void TableMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine(Utility.Configuration.Config.UI.SubBackColorPen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}

		protected override string GetPersistString() {
			return "BaseAirCorps";
		}




	}
}
