﻿using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
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

	public partial class FormCompass : DockContent {


		private class TableEnemyMemberControl {

			public ImageLabel ShipName;
			public ShipStatusEquipment Equipments;

			public FormCompass Parent;
			public ToolTip ToolTipInfo;


			public TableEnemyMemberControl( FormCompass parent ) {

				#region Initialize

				Parent = parent;
				ToolTipInfo = parent.ToolTipInfo;


				ShipName = new ImageLabel();
				ShipName.Anchor = AnchorStyles.Left;
				ShipName.ForeColor = parent.MainFontColor;
				ShipName.ImageAlign = ContentAlignment.MiddleCenter;
				ShipName.Padding = new Padding( 2, 2, 2, 2 );
				ShipName.Margin = new Padding( 2, 0, 2, 1 );
				ShipName.AutoEllipsis = true;
				ShipName.AutoSize = true;
				ShipName.Cursor = Cursors.Help;
				ShipName.MouseClick += ShipName_MouseClick;

				Equipments = new ShipStatusEquipment();
				Equipments.SuspendLayout();
				Equipments.Anchor = AnchorStyles.Left;
				Equipments.Padding = new Padding( 0, 1, 0, 2 );
				Equipments.Margin = new Padding( 2, 0, 2, 0 );
				Equipments.AutoSize = true;
				Equipments.ResumeLayout();

				ConfigurationChanged();

				#endregion

			}


			public TableEnemyMemberControl( FormCompass parent, TableLayoutPanel table, int row )
				: this( parent ) {

				AddToTable( table, row );
			}

			public void AddToTable( TableLayoutPanel table, int row ) {

				table.Controls.Add( ShipName, 0, row );
				table.Controls.Add( Equipments, 1, row );

			}


			public void Update( int shipID ) {
				var slot = shipID != -1 ? KCDatabase.Instance.MasterShips[shipID].DefaultSlot : null;
				Update( shipID, slot != null ? slot.ToArray() : null );
			}


			public void Update( int shipID, int[] slot ) {

				ShipName.Tag = shipID;

				if ( shipID == -1 ) {
					//なし
					ShipName.Text = "-";
					ShipName.ForeColor = Utility.Configuration.Config.UI.ForeColor;
					Equipments.Visible = false;
					ToolTipInfo.SetToolTip( ShipName, null );
					ToolTipInfo.SetToolTip( Equipments, null );

				} else {

					ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];


					ShipName.Text = ship.Name;
					ShipName.ForeColor = ship.GetShipNameColor();
					ToolTipInfo.SetToolTip( ShipName, GetShipString( shipID, slot ) );

					Equipments.SetSlotList( shipID, slot );
					Equipments.Visible = true;
					ToolTipInfo.SetToolTip( Equipments, GetEquipmentString( shipID, slot ) );
				}

			}

			public void UpdateEquipmentToolTip( int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor ) {

				ToolTipInfo.SetToolTip( ShipName, GetShipString( shipID, slot, level, hp, firepower, torpedo, aa, armor ) );
			}


			void ShipName_MouseClick( object sender, MouseEventArgs e ) {

				if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
					int shipID = ShipName.Tag as int? ?? -1;

					if ( shipID != -1 )
						new DialogAlbumMasterShip( shipID ).Show( Parent );
				}

			}


			public void ConfigurationChanged() {
				ShipName.Font = Utility.Configuration.Config.UI.JapFont;
				Equipments.Font = Utility.Configuration.Config.UI.JapFont2;

				ShipName.MaximumSize = new Size( Utility.Configuration.Config.FormCompass.MaxShipNameWidth, int.MaxValue );
			}

		}


		private class TableEnemyCandidateControl {

			public ImageLabel[] ShipNames;
			public ImageLabel Formation;
			public ImageLabel AirSuperiority;

			public FormCompass Parent;
			public ToolTip ToolTipInfo;


			public TableEnemyCandidateControl( FormCompass parent ) {

				#region Initialize

				Parent = parent;
				ToolTipInfo = parent.ToolTipInfo;


				ShipNames = new ImageLabel[6];
				for ( int i = 0; i < ShipNames.Length; i++ ) {
					ShipNames[i] = InitializeImageLabel();
					ShipNames[i].Cursor = Cursors.Help;
					ShipNames[i].MouseClick += TableEnemyCandidateControl_MouseClick;
				}

				Formation = InitializeImageLabel();
				Formation.Anchor = AnchorStyles.None;
				/*
				Formation.ImageAlign = ContentAlignment.MiddleLeft;
				Formation.ImageList = ResourceManager.Instance.Icons;
				Formation.ImageIndex = -1;
				*/

				AirSuperiority = InitializeImageLabel();
				AirSuperiority.Anchor = AnchorStyles.Right;
				AirSuperiority.ImageAlign = ContentAlignment.MiddleLeft;
				AirSuperiority.ImageList = ResourceManager.Instance.Equipments;
				AirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;


				ConfigurationChanged();

				#endregion

			}

			private ImageLabel InitializeImageLabel() {
				var label = new ImageLabel();
				label.Anchor = AnchorStyles.Left;
				label.ForeColor = Parent.MainFontColor;
				label.ImageAlign = ContentAlignment.MiddleCenter;
				label.Padding = new Padding( 0, 1, 0, 1 );
				label.Margin = new Padding( 4, 0, 4, 1 );
				label.AutoEllipsis = true;
				label.AutoSize = true;

				return label;
			}



			public TableEnemyCandidateControl( FormCompass parent, TableLayoutPanel table, int column )
				: this( parent ) {

				AddToTable( table, column );
			}

			public void AddToTable( TableLayoutPanel table, int column ) {

				table.ColumnCount = Math.Max( table.ColumnCount, column + 1 );
				table.RowCount = Math.Max( table.RowCount, 8 );

				for ( int i = 0; i < 6; i++ )
					table.Controls.Add( ShipNames[i], column, i );
				table.Controls.Add( Formation, column, 6 );
				table.Controls.Add( AirSuperiority, column, 7 );

			}


			public void ConfigurationChanged() {
				for ( int i = 0; i < ShipNames.Length; i++ )
					ShipNames[i].Font = Utility.Configuration.Config.UI.JapFont;
				Formation.Font = Parent.MainFont;
				AirSuperiority.Font = Utility.Configuration.Config.UI.JapFont;

				var maxSize = new Size( Utility.Configuration.Config.FormCompass.MaxShipNameWidth, int.MaxValue );
				foreach ( var label in ShipNames )
					label.MaximumSize = maxSize;
				Formation.MaximumSize = maxSize;
				AirSuperiority.MaximumSize = maxSize;
			}

			public void Update( EnemyFleetRecord.EnemyFleetElement fleet ) {

				if ( fleet == null ) {
					for ( int i = 0; i < 6; i++ )
						ShipNames[i].Visible = false;
					Formation.Visible = false;
					AirSuperiority.Visible = false;
					ToolTipInfo.SetToolTip( AirSuperiority, null );

					return;
				}

				for ( int i = 0; i < 6; i++ ) {

					var ship = KCDatabase.Instance.MasterShips[fleet.FleetMember[i]];

					// カッコカリ 上のとマージするといいかもしれない

					if ( ship == null ) {
						// nothing
						ShipNames[i].Text = "-";
						ShipNames[i].ForeColor = Utility.Configuration.Config.UI.ForeColor;
						ShipNames[i].Tag = -1;
						ShipNames[i].Cursor = Cursors.Default;
						ToolTipInfo.SetToolTip( ShipNames[i], null );

					} else {

						ShipNames[i].Text = ship.Name;
						ShipNames[i].ForeColor = ship.GetShipNameColor();
						ShipNames[i].Tag = ship.ShipID;
						ShipNames[i].Cursor = Cursors.Help;
						ToolTipInfo.SetToolTip( ShipNames[i], GetShipString( ship.ShipID, ship.DefaultSlot != null ? ship.DefaultSlot.ToArray() : null ) );
					}

					ShipNames[i].Visible = true;

				}

				Formation.Text = Constants.GetFormationShort( fleet.Formation );
				Formation.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				//Formation.ImageIndex = (int)ResourceManager.IconContent.BattleFormationEnemyLineAhead + fleet.Formation - 1;
				Formation.Visible = true;

				{
					int air = Calculator.GetAirSuperiority( fleet.FleetMember );
					AirSuperiority.Text = air.ToString();
					AirSuperiority.ForeColor = Utility.Configuration.Config.UI.ForeColor;
					ToolTipInfo.SetToolTip( AirSuperiority, GetAirSuperiorityString( air ) );
					AirSuperiority.Visible = true;
				}

			}


			void TableEnemyCandidateControl_MouseClick( object sender, MouseEventArgs e ) {

				if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
					int shipID = ( (ImageLabel)sender ).Tag as int? ?? -1;

					if ( shipID != -1 )
						new DialogAlbumMasterShip( shipID ).Show( Parent );
				}
			}

		}



		#region ***Control method

		private static string GetShipString( int shipID, int[] slot ) {

			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
			if ( ship == null ) return null;

			return GetShipString( shipID, slot, -1, ship.HPMin, ship.FirepowerMax, ship.TorpedoMax, ship.AAMax, ship.ArmorMax,
				 ship.ASW != null && !ship.ASW.IsMaximumDefault ? ship.ASW.Maximum : -1,
				 ship.Evasion != null && !ship.Evasion.IsMaximumDefault ? ship.Evasion.Maximum : -1,
				 ship.LOS != null && !ship.LOS.IsMaximumDefault ? ship.LOS.Maximum : -1,
				 ship.LuckMin );
		}

		private static string GetShipString( int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor ) {
			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
			if ( ship == null ) return null;

			return GetShipString( shipID, slot, level, hp, firepower, torpedo, aa, armor,
				ship.ASW != null && ship.ASW.IsAvailable ? ship.ASW.GetParameter( level ) : -1,
				ship.Evasion != null && ship.Evasion.IsAvailable ? ship.Evasion.GetParameter( level ) : -1,
				ship.LOS != null && ship.LOS.IsAvailable ? ship.LOS.GetParameter( level ) : -1,
				level > 99 ? Math.Min( ship.LuckMin + 3, ship.LuckMax ) : ship.LuckMin );
		}

		private static string GetShipString( int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor, int asw, int evasion, int los, int luck ) {

			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
			if ( ship == null ) return null;

			int firepower_c = firepower;
			int torpedo_c = torpedo;
			int aa_c = aa;
			int armor_c = armor;
			int asw_c = asw;
			int evasion_c = evasion;
			int los_c = los;
			int luck_c = luck;
			int range = ship.Range;

			asw = Math.Max( asw, 0 );
			evasion = Math.Max( evasion, 0 );
			los = Math.Max( los, 0 );

			if ( slot != null ) {
				int count = slot.Length;
				for ( int i = 0; i < count; i++ ) {
					EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[i]];
					if ( eq == null ) continue;

					firepower += eq.Firepower;
					torpedo += eq.Torpedo;
					aa += eq.AA;
					armor += eq.Armor;
					asw += eq.ASW;
					evasion += eq.Evasion;
					los += eq.LOS;
					luck += eq.Luck;
					range = Math.Max( range, eq.Range );
				}
			}

			/*
			return string.Format(
						"{0} {1}{2}\n耐久: {3}\n火力: {4}/{5}\n雷装: {6}/{7}\n対空: {8}/{9}\n装甲: {10}/{11}\n対潜: {12}/{13}\n回避: {14}/{15}\n索敵: {16}/{17}\n運: {18}/{19}\n射程: {20} / 速力: {21}\n(右クリックで図鑑)\n",
						ship.ShipTypeName, ship.NameWithClass, level < 1 ? "" : string.Format( " Lv. {0}", level ),
						hp,
						firepower_c, firepower,
						torpedo_c, torpedo,
						aa_c, aa,
						armor_c, armor,
						asw_c == -1 ? "???" : asw_c.ToString(), asw,
						evasion_c == -1 ? "???" : evasion_c.ToString(), evasion,
						los_c == -1 ? "???" : los_c.ToString(), los,
						luck_c, luck,
						Constants.GetRange( range ),
						Constants.GetSpeed( ship.Speed )
						);
			*/

			var sb = new StringBuilder();

			sb.Append( ship.ShipTypeName ).Append( " " ).Append( ship.NameWithClass );
			if ( level > 0 )
				sb.Append( " Lv. " ).Append( level );
			sb.AppendLine();

			sb.Append( "耐久 : " ).Append( hp ).AppendLine();

			sb.Append( "火力 : " ).Append( firepower_c );
			if ( firepower_c != firepower )
				sb.Append( "/" ).Append( firepower );
			sb.AppendLine();

			sb.Append( "雷装 : " ).Append( torpedo_c );
			if ( torpedo_c != torpedo )
				sb.Append( "/" ).Append( torpedo );
			sb.AppendLine();

			sb.Append( "对空 : " ).Append( aa_c );
			if ( aa_c != aa )
				sb.Append( "/" ).Append( aa );
			sb.AppendLine();

			sb.Append( "装甲 : " ).Append( armor_c );
			if ( armor_c != armor )
				sb.Append( "/" ).Append( armor );
			sb.AppendLine();

			sb.Append( "对潜 : " );
			if ( asw_c < 0 ) sb.Append( "???" );
			else sb.Append( asw_c );
			if ( asw_c != asw )
				sb.Append( "/" ).Append( asw );
			sb.AppendLine();

			sb.Append( "回避 : " );
			if ( evasion_c < 0 ) sb.Append( "???" );
			else sb.Append( evasion_c );
			if ( evasion_c != evasion )
				sb.Append( "/" ).Append( evasion );
			sb.AppendLine();

			sb.Append( "索敌 : " );
			if ( los_c < 0 ) sb.Append( "???" );
			else sb.Append( los_c );
			if ( los_c != los )
				sb.Append( "/" ).Append( los );
			sb.AppendLine();

			sb.Append( "运 : " ).Append( luck_c );
			if ( luck_c != luck )
				sb.Append( "/" ).Append( luck );
			sb.AppendLine();

			sb.AppendFormat( "射程 : {0} / 速度 : {1}\r\n( 点击鼠标右键转到图鉴 )\r\n",
				Constants.GetRange( range ),
				Constants.GetSpeed( ship.Speed ) );

			return sb.ToString();

		}

		private static string GetEquipmentString( int shipID, int[] slot ) {
			StringBuilder sb = new StringBuilder();
			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];

			if ( ship == null || slot == null ) return null;

			for ( int i = 0; i < slot.Length; i++ ) {
				var eq = KCDatabase.Instance.MasterEquipments[slot[i]];
				if ( eq != null )
					sb.AppendFormat( "[{0}] {1}\r\n", ship.Aircraft[i], eq.Name );
			}

			sb.AppendFormat( "\r\n昼战 : {0}\r\n夜战 : {1}\r\n",
				Constants.GetDayAttackKind( Calculator.GetDayAttackKind( slot, ship.ShipID, -1 ) ),
				Constants.GetNightAttackKind( Calculator.GetNightAttackKind( slot, ship.ShipID, -1 ) ) );

			{
				int aacutin = Calculator.GetAACutinKind( shipID, slot );
				if ( aacutin != 0 ) {
					sb.AppendFormat( "对空 : {0}\r\n", Constants.GetAACutinKind( aacutin ) );
				}
			}
			{
				int airsup = Calculator.GetAirSuperiority( slot, ship.Aircraft.ToArray() );
				if ( airsup > 0 ) {
					sb.AppendFormat( "制空战力 : {0}\r\n", airsup );
				}
			}

			return sb.ToString();
		}

		private static string GetAirSuperiorityString( int air ) {
			if ( air > 0 ) {
				return string.Format( "确保 : {0}\r\n优势 : {1}\r\n均衡 : {2}\r\n劣势 : {3}\r\n",
							(int)( air * 3.0 ),
							(int)Math.Ceiling( air * 1.5 ),
							(int)( air / 1.5 + 1 ),
							(int)( air / 3.0 + 1 ) );
			}
			return null;
		}

		#endregion




		public Font MainFont { get; set; }
		public Font SubFont { get; set; }
		public Color MainFontColor { get; set; }
		public Color SubFontColor { get; set; }


		private TableEnemyMemberControl[] ControlMembers;
		private TableEnemyCandidateControl[] ControlCandidates;

		private int _candidatesDisplayCount;


		/// <summary>
		/// 次に遭遇する敵艦隊候補
		/// </summary>
		private List<EnemyFleetRecord.EnemyFleetElement> _enemyFleetCandidate = null;

		/// <summary>
		/// 表示中の敵艦隊候補のインデックス
		/// </summary>
		private int _enemyFleetCandidateIndex = 0;




		public FormCompass( FormMain parent ) {
			InitializeComponent();



			MainFontColor = Color.FromArgb( 0x00, 0x00, 0x00 );
			SubFontColor = Color.FromArgb( 0x88, 0x88, 0x88 );


			ControlHelper.SetDoubleBuffered( BasePanel );
			ControlHelper.SetDoubleBuffered( TableEnemyFleet );
			ControlHelper.SetDoubleBuffered( TableEnemyMember );


			TableEnemyMember.SuspendLayout();
			ControlMembers = new TableEnemyMemberControl[6];
			for ( int i = 0; i < ControlMembers.Length; i++ ) {
				ControlMembers[i] = new TableEnemyMemberControl( this, TableEnemyMember, i );
			}
			TableEnemyMember.ResumeLayout();

			TableEnemyCandidate.SuspendLayout();
			ControlCandidates = new TableEnemyCandidateControl[6];
			for ( int i  = 0; i < ControlCandidates.Length; i++ ) {
				ControlCandidates[i] = new TableEnemyCandidateControl( this, TableEnemyCandidate, i );
			}
			TableEnemyCandidate.ResumeLayout();


			//BasePanel.SetFlowBreak( TextMapArea, true );
			BasePanel.SetFlowBreak( TextDestination, true );
			//BasePanel.SetFlowBreak( TextEventKind, true );
			BasePanel.SetFlowBreak( TextEventDetail, true );


			TextDestination.ImageList = ResourceManager.Instance.Equipments;
			TextEventKind.ImageList = ResourceManager.Instance.Equipments;
			TextEventDetail.ImageList = ResourceManager.Instance.Equipments;
			TextFormation.ImageList = ResourceManager.Instance.Icons;
			TextAirSuperiority.ImageList = ResourceManager.Instance.Equipments;
			TextAirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;



			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormCompass] );

		}


		private void FormCompass_Load( object sender, EventArgs e ) {

			BasePanel.Visible = false;


			APIObserver o = APIObserver.Instance;

			o.APIList["api_port/port"].ResponseReceived += Updated;
			o.APIList["api_req_map/start"].ResponseReceived += Updated;
			o.APIList["api_req_map/next"].ResponseReceived += Updated;
			o.APIList["api_req_member/get_practice_enemyinfo"].ResponseReceived += Updated;

			o.APIList["api_req_sortie/battle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_battle_midnight/sp_midnight"].ResponseReceived += BattleStarted;
			o.APIList["api_req_sortie/airbattle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_sortie/ld_airbattle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/battle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/sp_midnight"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/airbattle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/battle_water"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/ld_airbattle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/ec_battle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/each_battle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/each_battle_water"].ResponseReceived += BattleStarted;
			o.APIList["api_req_practice/battle"].ResponseReceived += BattleStarted;


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}


		private void Updated( string apiname, dynamic data ) {

			Func<int, Color> getColorFromEventKind = ( int kind ) => {
				switch ( kind ) {
					case 0:
					case 1:
					default:	//昼夜戦・その他
						return Utility.Configuration.Config.UI.ForeColor;
					case 2:
					case 3:		//夜戦・夜昼戦
						return Utility.Configuration.Config.UI.Compass_ColorTextEventKind3;
					case 4:		//航空戦
					case 6:		//長距離空襲戦
						return Utility.Configuration.Config.UI.Compass_ColorTextEventKind6;
					case 5:		// 敵連合
						return Utility.Configuration.Config.UI.Compass_ColorTextEventKind5;
				}
			};

			if ( apiname == "api_port/port" ) {

				BasePanel.Visible = false;

			} else if ( apiname == "api_req_member/get_practice_enemyinfo" ) {

				TextMapArea.Text = "演习";
				TextDestination.Text = string.Format( "{0} {1}", data.api_nickname, Constants.GetAdmiralRank( (int)data.api_rank ) );
				TextDestination.Font = Utility.Configuration.Config.UI.JapFont;
				TextDestination.ImageAlign = ContentAlignment.MiddleCenter;
				TextDestination.ImageIndex = -1;
				ToolTipInfo.SetToolTip( TextDestination, null );
				TextEventKind.Text = data.api_cmt;
				TextEventKind.ForeColor = getColorFromEventKind( 0 );
				TextEventKind.ImageAlign = ContentAlignment.MiddleCenter;
				TextEventKind.ImageIndex = -1;
				TextEventKind.Font = Utility.Configuration.Config.UI.JapFont;
				ToolTipInfo.SetToolTip( TextEventKind, null );
				TextEventDetail.Text = string.Format( "Lv. {0} / {1} exp.", data.api_level, data.api_experience[0] );
				TextEventDetail.Font = Utility.Configuration.Config.UI.JapFont;
				TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
				TextEventDetail.ImageIndex = -1;
				ToolTipInfo.SetToolTip( TextEventDetail, null );
				TextEnemyFleetName.Text = data.api_deckname;
				TextEnemyFleetName.Font = Utility.Configuration.Config.UI.JapFont;

			} else {

				CompassData compass = KCDatabase.Instance.Battle.Compass;


				BasePanel.SuspendLayout();
				PanelEnemyFleet.Visible = false;
				PanelEnemyCandidate.Visible = false;

				_enemyFleetCandidate = null;
				_enemyFleetCandidateIndex = -1;


				TextMapArea.Text = string.Format( "出击海域 : {0}-{1}{2}", compass.MapAreaID, compass.MapInfoID,
					compass.MapInfo.EventDifficulty > 0 ? " [" + Constants.GetDifficulty( compass.MapInfo.EventDifficulty ) + "]" : "" );
				{
					var mapinfo = compass.MapInfo;

					if ( mapinfo.IsCleared ) {
						ToolTipInfo.SetToolTip( TextMapArea, null );

					} else if ( mapinfo.RequiredDefeatedCount != -1 ) {
						ToolTipInfo.SetToolTip( TextMapArea, string.Format( "击破 : {0} / {1} 次", mapinfo.CurrentDefeatedCount, mapinfo.RequiredDefeatedCount ) );

					} else if ( mapinfo.MapHPMax > 0 ) {
						int current = compass.MapHPCurrent > 0 ? compass.MapHPCurrent : mapinfo.MapHPCurrent;
						int max = compass.MapHPMax > 0 ? compass.MapHPMax : mapinfo.MapHPMax;

						ToolTipInfo.SetToolTip( TextMapArea, string.Format( "{0}: {1} / {2}", mapinfo.GaugeType == 3 ? "TP" : "HP", current, max ) );

					} else {
						ToolTipInfo.SetToolTip( TextMapArea, null );
					}
				}


				TextDestination.Text = string.Format( "下一点 : {0}{1}", compass.Destination, ( compass.IsEndPoint ? " ( 终点 )" : "" ) );
				TextDestination.Font = Utility.Configuration.Config.UI.MainFont;
				if ( compass.LaunchedRecon != 0 ) {
					TextDestination.ImageAlign = ContentAlignment.MiddleRight;
					TextDestination.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;

					string tiptext;
					switch ( compass.CommentID ) {
						case 1:
							tiptext = "发现敌舰队！";
							break;
						case 2:
							tiptext = "发现攻击目标！";
							break;
						case 3:
							tiptext = "針路哨戒！";
							break;
						default:
							tiptext = "索敌机离舰！";
							break;
					}
					ToolTipInfo.SetToolTip( TextDestination, tiptext );

				} else {
					TextDestination.ImageAlign = ContentAlignment.MiddleCenter;
					TextDestination.ImageIndex = -1;
					ToolTipInfo.SetToolTip( TextDestination, null );
				}

				//とりあえずリセット
				TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
				TextEventDetail.ImageIndex = -1;
				ToolTipInfo.SetToolTip( TextEventDetail, null );


				TextEventKind.ForeColor = getColorFromEventKind( 0 );

				{
					string eventkind = Constants.GetMapEventID( compass.EventID );

					switch ( compass.EventID ) {

						case 0:		//初期位置
						case 1:		//不明
							TextEventDetail.Text = "どうしてこうなった";
							break;

						case 2:		//資源
						case 8:		//船団護衛成功
							TextEventDetail.Text = GetMaterialInfo( compass );
							TextEventDetail.Font = Utility.Configuration.Config.UI.MainFont;
							break;

						case 3:		//渦潮
							{
								int materialmax = KCDatabase.Instance.Fleet.Fleets.Values
									.Where( f => f != null && f.IsInSortie )
									.SelectMany( f => f.MembersWithoutEscaped )
									.Max( s => {
										if ( s == null ) return 0;
										switch ( compass.WhirlpoolItemID ) {
											case 1:
												return s.Fuel;
											case 2:
												return s.Ammo;
											default:
												return 0;
										}
									} );

								TextEventDetail.Text = string.Format( "{0} x {1} ({2:p0})",
									Constants.GetMaterialName( compass.WhirlpoolItemID ),
									compass.WhirlpoolItemAmount,
									(double)compass.WhirlpoolItemAmount / Math.Max( materialmax, 1 ) );
								TextEventDetail.Font = Utility.Configuration.Config.UI.MainFont;

							}
							break;

						case 4:		//通常戦闘
							if ( compass.EventKind >= 2 ) {
								eventkind += "/" + Constants.GetMapEventKind( compass.EventKind );

								TextEventKind.ForeColor = getColorFromEventKind( compass.EventKind );
							}
							UpdateEnemyFleet();
							break;

						case 5:		//ボス戦闘
							TextEventKind.ForeColor = Utility.Configuration.Config.UI.Color_Red;

							if ( compass.EventKind >= 2 ) {
								eventkind += "/" + Constants.GetMapEventKind( compass.EventKind );
							}
							UpdateEnemyFleet();
							break;

						case 6:		//気のせいだった
							switch ( compass.EventKind ) {

								case 0:		//気のせいだった
								default:
									break;
								case 1:
									eventkind = "不见敌影";
									break;
								case 2:
									eventkind = "能动分歧";
									break;
								case 3:
									eventkind = "平稳的海面";
									break;
								case 4:
									eventkind = "平稳的海峡";
									break;
								case 5:
									eventkind = "需要提高警惕";
									break;
								case 6:
									eventkind = "安静的海洋";
									break;
								case 7:
									eventkind = "向多佛尔海峡推进中";
									break;
							}
							if ( compass.RouteChoices != null ) {
								TextEventDetail.Text = string.Join( "/", compass.RouteChoices );
								TextEventDetail.Font = Utility.Configuration.Config.UI.MainFont;
							} else {
								TextEventDetail.Text = "";
							}

							break;

						case 7:		//航空戦or航空偵察
							TextEventKind.ForeColor = getColorFromEventKind( compass.EventKind );

							switch ( compass.EventKind ) {
								case 0:		//航空偵察
									eventkind = "航空侦察";

									switch ( compass.AirReconnaissanceResult ) {
										case 0:
										default:
											TextEventDetail.Text = "失败";
											TextEventDetail.Font = Utility.Configuration.Config.UI.MainFont;
											break;
										case 1:
											TextEventDetail.Text = "成功";
											TextEventDetail.Font = Utility.Configuration.Config.UI.MainFont;
											break;
										case 2:
											TextEventDetail.Text = "大成功";
											TextEventDetail.Font = Utility.Configuration.Config.UI.MainFont;
											break;
									}

									switch ( compass.AirReconnaissancePlane ) {
										case 0:
										default:
											TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
											TextEventDetail.ImageIndex = -1;
											break;
										case 1:
											TextEventDetail.ImageAlign = ContentAlignment.MiddleLeft;
											TextEventDetail.ImageIndex = (int)ResourceManager.EquipmentContent.FlyingBoat;
											break;
										case 2:
											TextEventDetail.ImageAlign = ContentAlignment.MiddleLeft;
											TextEventDetail.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
											break;
									}

									if ( compass.GetItems.Any() ) {
										TextEventDetail.Text += "　" + GetMaterialInfo( compass );
										TextEventDetail.Font = Utility.Configuration.Config.UI.MainFont;
									}

									break;

								case 4:		//航空戦
								default:
									UpdateEnemyFleet();
									break;
							}
							break;

						case 9:		//揚陸地点
							TextEventDetail.Text = "";
							break;

						default:
							TextEventDetail.Text = "";
							break;

					}
					TextEventKind.Text = eventkind;
					TextEventKind.Font = Utility.Configuration.Config.UI.MainFont;
				}


				if ( compass.HasAirRaid ) {
					TextEventKind.ImageAlign = ContentAlignment.MiddleRight;
					TextEventKind.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedBomber;
					ToolTipInfo.SetToolTip( TextEventKind, Constants.GetAirRaidDamage( compass.AirRaidDamageKind ) );
				} else {
					TextEventKind.ImageAlign = ContentAlignment.MiddleCenter;
					TextEventKind.ImageIndex = -1;
					ToolTipInfo.SetToolTip( TextEventKind, null );
				}


				BasePanel.ResumeLayout();

				BasePanel.Visible = true;
			}


		}


		private string GetMaterialInfo( CompassData compass ) {

			var strs = new LinkedList<string>();

			foreach ( var item in compass.GetItems ) {

				string itemName;

				if ( item.ItemID == 4 ) {
					itemName = Constants.GetMaterialName( item.Metadata );

				} else {
					var itemMaster = KCDatabase.Instance.MasterUseItems[item.Metadata];
					if ( itemMaster != null )
						itemName = itemMaster.Name;
					else
						itemName = "謎のアイテム";
				}

				strs.AddLast( itemName + " x " + item.Amount );
			}

			if ( !strs.Any() ) {
				return "( 无 )";

			} else {
				return string.Join( ", ", strs );
			}
		}



		private void BattleStarted( string apiname, dynamic data ) {
			UpdateEnemyFleetInstant( apiname.Contains( "practice" ) );
		}





		private void UpdateEnemyFleet() {

			CompassData compass = KCDatabase.Instance.Battle.Compass;

			_enemyFleetCandidate = RecordManager.Instance.EnemyFleet.Record.Values.Where(
				r =>
					r.MapAreaID == compass.MapAreaID &&
					r.MapInfoID == compass.MapInfoID &&
					r.CellID == compass.Destination &&
					r.Difficulty == compass.MapInfo.EventDifficulty
				).ToList();
			_enemyFleetCandidateIndex = 0;


			if ( _enemyFleetCandidate.Count == 0 ) {
				TextEventDetail.Text = "( 尚无敌舰队候选 )";
				TextEventDetail.Font = Utility.Configuration.Config.UI.MainFont;
				TextEnemyFleetName.Text = "( 敌舰队名不明 )";
				TextEnemyFleetName.Font = Utility.Configuration.Config.UI.MainFont;


				TableEnemyCandidate.Visible = false;

			} else {
				_enemyFleetCandidate.Sort( ( a, b ) => {
					for ( int i = 0; i < a.FleetMember.Length; i++ ) {
						int diff = a.FleetMember[i] - b.FleetMember[i];
						if ( diff != 0 )
							return diff;
					}
					return a.Formation - b.Formation;
				} );

				NextEnemyFleetCandidate( 0 );
			}


			PanelEnemyFleet.Visible = false;

		}


		private void UpdateEnemyFleetInstant( bool isPractice = false ) {

			BattleManager bm = KCDatabase.Instance.Battle;
			BattleData bd = bm.StartsFromDayBattle ? (BattleData)bm.BattleDay : (BattleData)bm.BattleNight;

			int[] enemies = bd.Initial.EnemyMembers;
			int[][] slots = bd.Initial.EnemySlots;
			int[] levels = bd.Initial.EnemyLevels;
			int[][] parameters = bd.Initial.EnemyParameters;
			int[] hps = bd.Initial.MaxHPs;


			_enemyFleetCandidate = null;
			_enemyFleetCandidateIndex = -1;



			if ( !bm.IsPractice ) {
				var efcurrent = EnemyFleetRecord.EnemyFleetElement.CreateFromCurrentState();
				var efrecord = RecordManager.Instance.EnemyFleet[efcurrent.FleetID];
				if ( efrecord != null ) {
					TextEnemyFleetName.Text = efrecord.FleetName;
					TextEnemyFleetName.Font = Utility.Configuration.Config.UI.JapFont;
				}
				TextEventDetail.Text = "敌舰队 ID: " + efcurrent.FleetID.ToString( "x8" );
				TextEventDetail.Font = Utility.Configuration.Config.UI.MainFont;
				ToolTipInfo.SetToolTip( TextEventDetail, null );
			}

			TextFormation.Text = Constants.GetFormationShort( (int)bd.Searching.FormationEnemy );
			//TextFormation.ImageIndex = (int)ResourceManager.IconContent.BattleFormationEnemyLineAhead + bd.Searching.FormationEnemy - 1;
			TextFormation.Visible = true;
			{
				int air = Calculator.GetAirSuperiority( enemies, slots );
				TextAirSuperiority.Text = isPractice ?
					air.ToString() + " ～ " + Calculator.GetAirSuperiorityAtMaxLevel( enemies, slots ).ToString() :
					air.ToString();
				ToolTipInfo.SetToolTip( TextAirSuperiority, GetAirSuperiorityString( isPractice ? 0 : air ) );
				TextAirSuperiority.Visible = true;
			}

			TableEnemyMember.SuspendLayout();
			for ( int i = 0; i < ControlMembers.Length; i++ ) {
				int shipID = enemies[i];
				ControlMembers[i].Update( shipID, shipID != -1 ? slots[i] : null );

				if ( shipID != -1 )
					ControlMembers[i].UpdateEquipmentToolTip( shipID, slots[i], levels[i], hps[i + 6], parameters[i][0], parameters[i][1], parameters[i][2], parameters[i][3] );
			}
			TableEnemyMember.ResumeLayout();
			TableEnemyMember.Visible = true;

			PanelEnemyFleet.Visible = true;

			PanelEnemyCandidate.Visible = false;

			BasePanel.Visible = true;			//checkme

		}



		private void TextEnemyFleetName_MouseDown( object sender, MouseEventArgs e ) {

			if ( e.Button == System.Windows.Forms.MouseButtons.Left )
				NextEnemyFleetCandidate();
			else if ( e.Button == System.Windows.Forms.MouseButtons.Right )
				NextEnemyFleetCandidate( -_candidatesDisplayCount );
		}


		private void NextEnemyFleetCandidate() {
			NextEnemyFleetCandidate( _candidatesDisplayCount );
		}

		private void NextEnemyFleetCandidate( int offset ) {

			if ( _enemyFleetCandidate != null && _enemyFleetCandidate.Count != 0 ) {

				_enemyFleetCandidateIndex += offset;
				if ( _enemyFleetCandidateIndex < 0 )
					_enemyFleetCandidateIndex = ( _enemyFleetCandidate.Count - 1 ) - ( _enemyFleetCandidate.Count - 1 ) % _candidatesDisplayCount;
				else if ( _enemyFleetCandidateIndex >= _enemyFleetCandidate.Count )
					_enemyFleetCandidateIndex = 0;


				var candidate = _enemyFleetCandidate[_enemyFleetCandidateIndex];


				TextEventDetail.Text = TextEnemyFleetName.Text = candidate.FleetName;
				TextEventDetail.Font = TextEnemyFleetName.Font = Utility.Configuration.Config.UI.JapFont;

				if ( _enemyFleetCandidate.Count > _candidatesDisplayCount ) {
					TextEventDetail.Text += " ▼";
					ToolTipInfo.SetToolTip( TextEventDetail, string.Format( "候选 : {0} / {1}\r\n( 左右点击滚动 )\r\n", _enemyFleetCandidateIndex + 1, _enemyFleetCandidate.Count ) );
				} else {
					ToolTipInfo.SetToolTip( TextEventDetail, string.Format( "候选 : {0}\r\n", _enemyFleetCandidate.Count ) );
				}

				TableEnemyCandidate.SuspendLayout();
				for ( int i = 0; i < ControlCandidates.Length; i++ ) {
					if ( i + _enemyFleetCandidateIndex >= _enemyFleetCandidate.Count || i >= _candidatesDisplayCount ) {
						ControlCandidates[i].Update( null );
						continue;
					}

					ControlCandidates[i].Update( _enemyFleetCandidate[i + _enemyFleetCandidateIndex] );
				}
				TableEnemyCandidate.ResumeLayout();
				TableEnemyCandidate.Visible = true;

				PanelEnemyCandidate.Visible = true;

			}
		}


		void ConfigurationChanged() {

			Font = PanelEnemyFleet.Font = MainFont = Utility.Configuration.Config.UI.MainFont;
			SubFont = Utility.Configuration.Config.UI.SubFont;

			TextMapArea.Font =
			TextDestination.Font =
			TextEventKind.Font =
			TextEventDetail.Font = Font;

			BasePanel.AutoScroll = Utility.Configuration.Config.FormCompass.IsScrollable;

			_candidatesDisplayCount = Utility.Configuration.Config.FormCompass.CandidateDisplayCount;
			_enemyFleetCandidateIndex = 0;
			if ( PanelEnemyCandidate.Visible )
				NextEnemyFleetCandidate( 0 );

			if ( ControlMembers != null ) {
				TableEnemyMember.SuspendLayout();

				TableEnemyMember.Location = new Point( TableEnemyMember.Location.X, TableEnemyFleet.Bottom + 6 );

				bool flag = Utility.Configuration.Config.FormFleet.ShowAircraft;
				for ( int i = 0; i < ControlMembers.Length; i++ ) {
					ControlMembers[i].Equipments.ShowAircraft = flag;
					ControlMembers[i].ConfigurationChanged();
				}

				ControlHelper.SetTableRowStyles( TableEnemyMember, ControlHelper.GetDefaultRowStyle() );
				TableEnemyMember.ResumeLayout();
			}


			if ( ControlCandidates != null ) {
				TableEnemyCandidate.SuspendLayout();

				for ( int i = 0; i < ControlCandidates.Length; i++ )
					ControlCandidates[i].ConfigurationChanged();

				ControlHelper.SetTableRowStyles( TableEnemyCandidate, new RowStyle( SizeType.AutoSize ) );
				ControlHelper.SetTableColumnStyles( TableEnemyCandidate, ControlHelper.GetDefaultColumnStyle() );
				TableEnemyCandidate.ResumeLayout();
			}
		}



		protected override string GetPersistString() {
			return "Compass";
		}

		private void TableEnemyMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine(Utility.Configuration.Config.UI.SubBackColorPen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}

		private void TableEnemyCandidateMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {

			if ( _enemyFleetCandidate == null || _enemyFleetCandidateIndex + e.Column >= _enemyFleetCandidate.Count )
				return;


			if (e.Column != (Utility.Configuration.Config.FormCompass.CandidateDisplayCount - 1)) {
				e.Graphics.DrawLine(Utility.Configuration.Config.UI.SubBackColorPen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
				if (e.Row == 5) e.Graphics.DrawLine(Utility.Configuration.Config.UI.SubBackColorPen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
			} else if (e.Row == 5) {
				e.Graphics.DrawLine(Utility.Configuration.Config.UI.SubBackColorPen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 2, e.CellBounds.Bottom - 1);
			}
		}

	}

}
