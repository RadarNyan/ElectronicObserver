﻿using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Support;
using SwfExtractor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	public partial class FormFleet : DockContent {

		private bool IsRemodeling = false;


		private class TableFleetControl {
			public Label Name;
			public FleetState State;
			public ImageLabel AirSuperiority;
			public ImageLabel SearchingAbility;
			public ImageLabel AntiAirPower;
			public ToolTip ToolTipInfo;

			public TableFleetControl( FormFleet parent ) {

				#region Initialize

				Name = new Label();
				Name.Text = "[" + parent.FleetID.ToString() + "]";
				Name.Anchor = AnchorStyles.Left;
				Name.ForeColor = parent.MainFontColor;
				Name.Padding = new Padding( 0, 1, 0, 1 );
				Name.Margin = new Padding( 2, 0, 2, 0 );
				Name.AutoSize = true;
				//Name.Visible = false;
				Name.Cursor = Cursors.Help;

				State = new FleetState();
				State.Anchor = AnchorStyles.Left;
				State.ForeColor = parent.MainFontColor;
				State.Padding = new Padding();
				State.Margin = new Padding();
				State.AutoSize = true;

				AirSuperiority = new ImageLabel();
				AirSuperiority.Anchor = AnchorStyles.Left;
				AirSuperiority.ForeColor = parent.MainFontColor;
				AirSuperiority.ImageList = ResourceManager.Instance.Equipments;
				AirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;
				AirSuperiority.Padding = new Padding( 2, 2, 2, 2 );
				AirSuperiority.Margin = new Padding( 2, 0, 2, 0 );
				AirSuperiority.AutoSize = true;

				SearchingAbility = new ImageLabel();
				SearchingAbility.Anchor = AnchorStyles.Left;
				SearchingAbility.ForeColor = parent.MainFontColor;
				SearchingAbility.ImageList = ResourceManager.Instance.Equipments;
				SearchingAbility.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedRecon;
				SearchingAbility.Padding = new Padding( 2, 2, 2, 2 );
				SearchingAbility.Margin = new Padding( 2, 0, 2, 0 );
				SearchingAbility.AutoSize = true;
				SearchingAbility.Click += (sender, e) => SearchingAbility_Click(sender, e, parent.FleetID);

				AntiAirPower = new ImageLabel();
				AntiAirPower.Anchor = AnchorStyles.Left;
				AntiAirPower.ForeColor = parent.MainFontColor;
				AntiAirPower.ImageList = ResourceManager.Instance.Equipments;
				AntiAirPower.ImageIndex = (int)ResourceManager.EquipmentContent.HighAngleGun;
				AntiAirPower.Padding = new Padding( 2, 2, 2, 2 );
				AntiAirPower.Margin = new Padding( 2, 0, 2, 0 );
				AntiAirPower.AutoSize = true;


				ConfigurationChanged( parent );

				ToolTipInfo = parent.ToolTipInfo;

				#endregion

			}

			public TableFleetControl( FormFleet parent, TableLayoutPanel table )
				: this( parent ) {
				AddToTable( table );
			}

			public void AddToTable( TableLayoutPanel table ) {

				table.SuspendLayout();
				table.Controls.Add( Name, 0, 0 );
				table.Controls.Add( State, 1, 0 );
				table.Controls.Add( AirSuperiority, 2, 0 );
				table.Controls.Add( SearchingAbility, 3, 0 );
				table.Controls.Add( AntiAirPower, 4, 0 );
				table.ResumeLayout();

			}

			private int SearchingAbilityNew33BranchWeight = 1; // can only be 1, 4, 3

			private void SearchingAbility_Click(object sender, EventArgs e, int fleetID) {
				if (Utility.Configuration.Config.FormFleet.SearchingAbilityMethod != 4)
					return;
				switch (SearchingAbilityNew33BranchWeight) {
					case 1:
						SearchingAbilityNew33BranchWeight = 4;
						break;
					case 4:
						SearchingAbilityNew33BranchWeight = 3;
						break;
					case 3:
						SearchingAbilityNew33BranchWeight = 1;
						break;
				}
				Update(KCDatabase.Instance.Fleet[fleetID]);
			}

			public void Update( FleetData fleet ) {

				KCDatabase db = KCDatabase.Instance;

				if ( fleet == null ) return;



				Name.Text = fleet.Name;
				{
					int levelSum = fleet.MembersInstance.Sum( s => s != null ? s.Level : 0 );

					int fueltotal = fleet.MembersInstance.Sum( s => s == null ? 0 : (int)Math.Floor( s.FuelMax * ( s.IsMarried ? 0.85 : 1.00 ) ) );
					int ammototal = fleet.MembersInstance.Sum( s => s == null ? 0 : (int)Math.Floor( s.AmmoMax * ( s.IsMarried ? 0.85 : 1.00 ) ) );

					int fuelunit = fleet.MembersInstance.Sum( s => s == null ? 0 : (int)Math.Floor( s.MasterShip.Fuel * 0.2 * ( s.IsMarried ? 0.85 : 1.00 ) ) );
					int ammounit = fleet.MembersInstance.Sum( s => s == null ? 0 : (int)Math.Floor( s.MasterShip.Ammo * 0.2 * ( s.IsMarried ? 0.85 : 1.00 ) ) );

					int speed = fleet.MembersWithoutEscaped.Min( s => s == null ? 10 : s.Speed );

					double expeditionBonus = Calculator.GetExpeditionBonus( fleet );
					int tp = Calculator.GetTPDamage( fleet );

					ToolTipInfo.SetToolTip( Name, string.Format(
						"Lv 合计 : {0} / 平均 : {1:0.00}\r\n{2} 舰队\r\n载有运输桶 : {3} 个 ( {4} 舰 )\r\n载有大发动艇 : {5} 个 ( {6} 舰， +{7:p1} )\r\n运输量 (TP) : S {8} / A {9}\r\n总搭载 : 油 {10} / 弹 {11}\r\n( 每战消耗 油 {12} / 弹 {13} )",
						levelSum,
						(double)levelSum / Math.Max( fleet.Members.Count( id => id != -1 ), 1 ),
						Constants.GetSpeed( speed ),
						fleet.MembersInstance.Sum( s => s == null ? 0 : s.SlotInstanceMaster.Count( q => q == null ? false : q.CategoryType == 30 ) ),
						fleet.MembersInstance.Count( s => s == null ? false : s.SlotInstanceMaster.Any( q => q == null ? false : q.CategoryType == 30 ) ),
						fleet.MembersInstance.Sum( s => s == null ? 0 : s.SlotInstanceMaster.Count( q => q == null ? false : q.CategoryType == 24 || q.CategoryType == 46 ) ),
						fleet.MembersInstance.Count( s => s == null ? false : s.SlotInstanceMaster.Any( q => q == null ? false : q.CategoryType == 24 || q.CategoryType == 46 ) ),
						expeditionBonus,
						tp,
						(int)( tp * 0.7 ),
						fueltotal,
						ammototal,
						fuelunit,
						ammounit
						) );

				}


				State.UpdateFleetState( fleet, ToolTipInfo );


				//制空戦力計算	
				{
					int airSuperiority = fleet.GetAirSuperiority();
					bool includeLevel = Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1;
					AirSuperiority.Text = fleet.GetAirSuperiorityString();
					ToolTipInfo.SetToolTip( AirSuperiority,
						string.Format( "确保 : {0}\r\n优势 : {1}\r\n均衡 : {2}\r\n劣势 : {3}\r\n({4}: {5})\r\n",
						(int)( airSuperiority / 3.0 ),
						(int)( airSuperiority / 1.5 ),
						Math.Max( (int)( airSuperiority * 1.5 - 1 ), 0 ),
						Math.Max( (int)( airSuperiority * 3.0 - 1 ), 0 ),
						includeLevel ? "不考虑熟练度" : "考虑熟练度",
						includeLevel ? Calculator.GetAirSuperiorityIgnoreLevel( fleet ) : Calculator.GetAirSuperiority( fleet ) ) );
				}


				//索敵能力計算
				{
					StringBuilder sb = new StringBuilder();
					double probStart = fleet.GetContactProbability();
					var probSelect = fleet.GetContactSelectionProbability();

					if (Utility.Configuration.Config.FormFleet.SearchingAbilityMethod == 4) {
						switch (SearchingAbilityNew33BranchWeight) {
							case 1:
								SearchingAbility.Text = String.Format("{0:f2}", Math.Floor(Calculator.GetSearchingAbility_New33(fleet, SearchingAbilityNew33BranchWeight) * 100) / 100);
								sb.Append("分歧点系数 1 ( 点击切换 4 / 3 )\r\n");
								sb.Append("　2-5-H->BOSS	31 / 33\r\n");
								break;
							case 4:
								SearchingAbility.Text = String.Format("(4) {0:f2}", Math.Floor(Calculator.GetSearchingAbility_New33(fleet, SearchingAbilityNew33BranchWeight) * 100) / 100);
								sb.Append("分歧点系数 4 ( 点击切换 3 / 1 )\r\n");
								sb.Append("　3-5-G->BOSS	23 / 28\r\n　6-1-E->F	12 / 16 (大鯨)\r\n　6-1-F->K	20 / 25 (大鯨) / 36\r\n");
								break;
							case 3:
								SearchingAbility.Text = String.Format("(3) {0:f2}", Math.Floor(Calculator.GetSearchingAbility_New33(fleet, SearchingAbilityNew33BranchWeight) * 100) / 100);
								sb.Append("分歧点系数 3 ( 点击切换 1 / 4 )\r\n");
								sb.Append("　1-6-M->J	? / 30\r\n　6-2-F->I	43 / 50\r\n　6-2-H->BOSS	? / 40\r\n　6-3-H->BOSS	36 / 38\r\n　6-5-G->BOSS	? / 50\r\n");
								break;
						}
						sb.AppendFormat("\r\n2-5 旧 / 秋 / 新秋简易式\r\n　{0} / {1} / {2}\r\n",
							fleet.GetSearchingAbilityString(0),
							fleet.GetSearchingAbilityString(1),
							fleet.GetSearchingAbilityString(2));
					} else {
						SearchingAbility.Text = fleet.GetSearchingAbilityString();
						sb.AppendFormat( "2-5 旧 / 秋 / 新秋简易式\r\n　{0} / {1} / {2}\r\n\r\n新判定式 (33)\r\n分歧点系数 1:	[ {3:f2} ]\r\n　2-5-H->BOSS	 31 / 33\r\n分歧点系数 4:	[ {4:f2} ]\r\n　3-5-G->BOSS	 23 / 28\r\n　6-1-E->F(大鯨)	 12 / 16\r\n　6-1-F->K	 20 / 25\r\n分歧点系数 3:	[ {5:f2} ]\r\n　6-2-F->I	 43 / ?\r\n　6-2-H->BOSS	 ? / 40\r\n　6-3-H->BOSS	 36 / 38\r\n",
							fleet.GetSearchingAbilityString( 0 ),
							fleet.GetSearchingAbilityString( 1 ),
							fleet.GetSearchingAbilityString( 2 ),
							Math.Floor( Calculator.GetSearchingAbility_New33( fleet, 1 ) * 100 ) / 100,
							Math.Floor( Calculator.GetSearchingAbility_New33( fleet, 4 ) * 100 ) / 100,
							Math.Floor( Calculator.GetSearchingAbility_New33( fleet, 3 ) * 100 ) / 100);
					}

					sb.AppendFormat( "\r\n触接开始率 : \r\n　确保 {0:p1} / 优势 {1:p1}\r\n",
						probStart,
						probStart * 0.6 );

					if ( probSelect.Count > 0 ) {
						sb.AppendLine( "触接选择率 : " );

						foreach ( var p in probSelect.OrderBy( p => p.Key ) ) {
							sb.AppendFormat( "　命中 {0} : {1:p1}\r\n", p.Key, p.Value );
						}
					}

					ToolTipInfo.SetToolTip( SearchingAbility, sb.ToString() );
				}

				// 対空能力計算
				{
					var sb = new StringBuilder();
					double lineahead = Calculator.GetAdjustedFleetAAValue( fleet, 1 );

					AntiAirPower.Text = lineahead.ToString( "0.0" );

					sb.AppendFormat( "舰队防空\r\n单纵阵 : {0:0.0} / 复纵阵 : {1:0.0} / 轮形阵 : {2:0.0}\r\n",
						lineahead,
						Calculator.GetAdjustedFleetAAValue( fleet, 2 ),
						Calculator.GetAdjustedFleetAAValue( fleet, 3 ) );

					ToolTipInfo.SetToolTip( AntiAirPower, sb.ToString() );
				}
			}


			public void Refresh() {

				State.RefreshFleetState();

			}

			public void ConfigurationChanged( FormFleet parent ) {
				Name.Font = parent.MainFont;
				State.Font = Utility.Configuration.Config.UI.MainFont;
				State.RefreshFleetState();
				AirSuperiority.Font = parent.MainFont;
				SearchingAbility.Font = parent.MainFont;
				AntiAirPower.Font = parent.MainFont;

				ControlHelper.SetTableRowStyles( parent.TableFleet, ControlHelper.GetDefaultRowStyle() );
			}

		}


		private class TableMemberControl {
			public ImageLabel Name;
			public ShipStatusLevel Level;
			public ShipStatusHP HP;
			public ImageLabel Condition;
			public ShipStatusResource ShipResource;
			public ShipStatusEquipment Equipments;

			private ToolTip ToolTipInfo;
			private FormFleet Parent;


			public TableMemberControl( FormFleet parent ) {

				#region Initialize

				Name = new ImageLabel();
				Name.SuspendLayout();
				Name.Text = "*nothing*";
				Name.Anchor = AnchorStyles.Left;
				Name.TextAlign = ContentAlignment.MiddleLeft;
				Name.ImageAlign = ContentAlignment.MiddleCenter;
				Name.ForeColor = parent.MainFontColor;
				Name.Padding = new Padding( 2, 1, 2, 1 );
				Name.Margin = new Padding( 2, 1, 2, 1 );
				Name.AutoSize = true;
				//Name.AutoEllipsis = true;
				Name.Visible = false;
				Name.Cursor = Cursors.Help;
				Name.MouseDown += Name_MouseDown;
				Name.ResumeLayout();

				Level = new ShipStatusLevel();
				Level.SuspendLayout();
				Level.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
				Level.Value = 0;
				Level.MaximumValue = ExpTable.ShipMaximumLevel;
				Level.ValueNext = 0;
				Level.MainFontColor = parent.MainFontColor;
				Level.SubFontColor = parent.SubFontColor;
				//Level.TextNext = "n.";
				Level.Padding = new Padding( 0, 0, 0, 0 );
				Level.Margin = new Padding( 2, 0, 2, 1 );
				Level.AutoSize = true;
				Level.Visible = false;
				Level.ResumeLayout();

				HP = new ShipStatusHP();
				HP.SuspendUpdate();
				HP.Anchor = AnchorStyles.Left;
				HP.Value = 0;
				HP.MaximumValue = 0;
				HP.MaximumDigit = 999;
				HP.UsePrevValue = false;
				HP.MainFontColor = parent.MainFontColor;
				HP.SubFontColor = parent.SubFontColor;
				HP.Padding = new Padding( 0, 0, 0, 0 );
				HP.Margin = new Padding( 2, 1, 2, 2 );
				HP.AutoSize = true;
				HP.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				HP.Visible = false;
				HP.ResumeUpdate();

				Condition = new ImageLabel();
				Condition.SuspendLayout();
				Condition.Text = "*";
				Condition.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				Condition.ForeColor = parent.MainFontColor;
				Condition.TextAlign = ContentAlignment.BottomRight;
				Condition.ImageAlign = ContentAlignment.MiddleLeft;
				Condition.ImageList = ResourceManager.Instance.Icons;
				Condition.Padding = new Padding( 2, 1, 2, 1 );
				Condition.Margin = new Padding( 2, 1, 2, 1 );
				Condition.Size = new Size( 40, 20 );
				Condition.AutoSize = true;
				Condition.Visible = false;
				Condition.ResumeLayout();

				ShipResource = new ShipStatusResource( parent.ToolTipInfo );
				ShipResource.SuspendLayout();
				ShipResource.FuelCurrent = 0;
				ShipResource.FuelMax = 0;
				ShipResource.AmmoCurrent = 0;
				ShipResource.AmmoMax = 0;
				ShipResource.Anchor = AnchorStyles.Left;
				ShipResource.Padding = new Padding( 0, 2, 0, 0 );
				ShipResource.Margin = new Padding( 2, 0, 2, 1 );
				ShipResource.Size = new Size( 30, 20 );
				ShipResource.AutoSize = false;
				ShipResource.Visible = false;
				ShipResource.ResumeLayout();

				Equipments = new ShipStatusEquipment();
				Equipments.SuspendUpdate();
				Equipments.Anchor = AnchorStyles.Left;
				Equipments.Padding = new Padding( 0, 1, 0, 1 );
				Equipments.Margin = new Padding( 2, 0, 2, 1 );
				Equipments.Size = new Size( 40, 20 );
				Equipments.AutoSize = true;
				Equipments.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				Equipments.Visible = false;
				Equipments.ResumeUpdate();

				ConfigurationChanged( parent );

				ToolTipInfo = parent.ToolTipInfo;
				Parent = parent;
				#endregion

			}


			public TableMemberControl( FormFleet parent, TableLayoutPanel table, int row )
				: this( parent ) {
				AddToTable( table, row );

				Equipments.Name = string.Format( "{0}_{1}", parent.FleetID, row + 1 );
			}


			public void AddToTable( TableLayoutPanel table, int row ) {

				table.SuspendLayout();

				table.Controls.Add( Name, 0, row );
				table.Controls.Add( Level, 1, row );
				table.Controls.Add( HP, 2, row );
				table.Controls.Add( Condition, 3, row );
				table.Controls.Add( ShipResource, 4, row );
				table.Controls.Add( Equipments, 5, row );

				table.ResumeLayout();

			}

			public void Update( int shipMasterID ) {

				KCDatabase db = KCDatabase.Instance;
				ShipData ship = db.Ships[shipMasterID];

				if ( ship != null ) {

					bool isEscaped = KCDatabase.Instance.Fleet[Parent.FleetID].EscapedShipList.Contains( shipMasterID );


					Name.Text = ship.MasterShip.NameWithClass;
					Name.Tag = ship.ShipID;
					ToolTipInfo.SetToolTip( Name,
						string.Format(
							"{0} {1}\n火力 : {2}/{3}\n雷装 : {4}/{5}\n对空 : {6}/{7}\n装甲 : {8}/{9}\n对潜 : {10}/{11}\n回避 : {12}/{13}\n索敌 : {14}/{15}\n运 : {16}\n射程 : {17} / 速度 : {18}\n( 右键单击跳转到图鉴 )\n",
							ship.MasterShip.ShipTypeName, ship.NameWithLevel,
							ship.FirepowerBase, ship.FirepowerTotal,
							ship.TorpedoBase, ship.TorpedoTotal,
							ship.AABase, ship.AATotal,
							ship.ArmorBase, ship.ArmorTotal,
							ship.ASWBase, ship.ASWTotal,
							ship.EvasionBase, ship.EvasionTotal,
							ship.LOSBase, ship.LOSTotal,
							ship.LuckTotal,
							Constants.GetRange( ship.Range ),
							Constants.GetSpeed( ship.Speed )
							) );


					Level.Value = ship.Level;
					Level.ValueNext = ship.ExpNext;

					{
						StringBuilder tip = new StringBuilder();
						tip.AppendFormat( "总计 : {0} exp.\r\n", ship.ExpTotal );

						if ( !Utility.Configuration.Config.FormFleet.ShowNextExp )
							tip.AppendFormat( "距离升级 : {0} exp.\r\n", ship.ExpNext );

						if ( ship.MasterShip.RemodelAfterShipID != 0 && ship.Level < ship.MasterShip.RemodelAfterLevel ) {
							tip.AppendFormat( "距离改装 : Lv. {0} / {1} exp.\r\n", ship.MasterShip.RemodelAfterLevel - ship.Level, ship.ExpNextRemodel );

						} else if ( ship.Level <= 99 ) {
							tip.AppendFormat( "距离 Lv99 : {0} exp.\r\n", Math.Max( ExpTable.GetExpToLevelShip( ship.ExpTotal, 99 ), 0 ) );

						} else {
							tip.AppendFormat( "距离 Lv{0}: {1} exp.\r\n", ExpTable.ShipMaximumLevel, Math.Max( ExpTable.GetExpToLevelShip( ship.ExpTotal, ExpTable.ShipMaximumLevel ), 0 ) );

						}

						ToolTipInfo.SetToolTip( Level, tip.ToString() );
					}


					HP.SuspendUpdate();
					HP.Value = HP.PrevValue = ship.HPCurrent;
					HP.MaximumValue = ship.HPMax;
					HP.UsePrevValue = false;
					HP.ShowDifference = false;
					{
						int dockID = ship.RepairingDockID;

						if ( dockID != -1 ) {
							HP.RepairTime = db.Docks[dockID].CompletionTime;
							HP.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.Visible;
						} else {
							HP.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.Invisible;
						}
					}
					HP.Tag = ( ship.RepairingDockID == -1 && 0.5 < ship.HPRate && ship.HPRate < 1.0 ) ? DateTimeHelper.FromAPITimeSpan( ship.RepairTime ).TotalSeconds : 0.0;
					if ( isEscaped ) {
						HP.BackColor = Utility.Configuration.Config.UI.SubBackColor;
					} else {
						HP.BackColor = Utility.Configuration.Config.UI.BackColor;
					}
					{
						StringBuilder sb = new StringBuilder();
						double hprate = (double)ship.HPCurrent / ship.HPMax;

						sb.AppendFormat( "HP: {0:0.0}% [{1}]\n", hprate * 100, Constants.GetDamageState( hprate ) );
						if ( isEscaped ) {
							sb.AppendLine( "退避中" );
						} else if ( hprate > 0.50 ) {
							sb.AppendFormat( "距离中破 : {0} / 距离大破 : {1}\n", ship.HPCurrent - ship.HPMax / 2, ship.HPCurrent - ship.HPMax / 4 );
						} else if ( hprate > 0.25 ) {
							sb.AppendFormat( "距离大破 : {0}\n", ship.HPCurrent - ship.HPMax / 4 );
						} else {
							sb.AppendLine( "已经大破了！" );
						}

						if ( ship.RepairTime > 0 ) {
							var span = DateTimeHelper.FromAPITimeSpan( ship.RepairTime );
							var unittime = Calculator.CalculateDockingUnitTime(ship);
							sb.AppendFormat( // "入渠耗时 : {0} @ {1}",
								"入渠耗时 : {0} @ {1:00}'{2:00}\"",
								DateTimeHelper.ToTimeRemainString( span ),
								// DateTimeHelper.ToTimeRemainString( new TimeSpan( span.Add( new TimeSpan( 0, 0, -30 ) ).Ticks / ( ship.HPMax - ship.HPCurrent ) ) ) );
								unittime.Minutes,
								unittime.Seconds
							);
						}

						ToolTipInfo.SetToolTip( HP, sb.ToString() );
					}
					HP.ResumeUpdate();


					Condition.Text = ship.Condition.ToString();
					Condition.Tag = ship.Condition;
					SetConditionDesign( ship.Condition );

					if ( ship.Condition < 49 ) {
						TimeSpan ts = new TimeSpan( 0, (int)Math.Ceiling( ( 49 - ship.Condition ) / 3.0 ) * 3, 0 );
						ToolTipInfo.SetToolTip( Condition, string.Format( "距离完全恢复约 {0:D2}:{1:D2}", (int)ts.TotalMinutes, (int)ts.Seconds ) );
					} else {
						ToolTipInfo.SetToolTip( Condition, string.Format( "还可以远征 {0} 次", (int)Math.Ceiling( ( ship.Condition - 49 ) / 3.0 ) ) );
					}

					ShipResource.SetResources( ship.Fuel, ship.FuelMax, ship.Ammo, ship.AmmoMax );


					Equipments.SetSlotList( ship );
					ToolTipInfo.SetToolTip( Equipments, GetEquipmentString( ship ) );

				} else {
					Name.Tag = -1;
				}


				Name.Visible =
				Level.Visible =
				HP.Visible =
				Condition.Visible =
				ShipResource.Visible =
				Equipments.Visible = shipMasterID != -1;

			}

			void Name_MouseDown( object sender, MouseEventArgs e ) {
				int? id = Name.Tag as int?;

				if ( id != null && id != -1 && ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
					new DialogAlbumMasterShip( (int)id ).Show( Parent );
				}

			}


			private string GetEquipmentString( ShipData ship ) {
				StringBuilder sb = new StringBuilder();

				for ( int i = 0; i < ship.Slot.Count; i++ ) {
					var eq = ship.SlotInstance[i];
					if ( eq != null )
						sb.AppendFormat( "[{0}/{1}] {2}\r\n", ship.Aircraft[i], ship.MasterShip.Aircraft[i], eq.NameWithLevel );
				}

				{
					var exslot = ship.ExpansionSlotInstance;
					if ( exslot != null )
						sb.AppendFormat( "补强 : {0}\r\n", exslot.NameWithLevel );
				}

				int[] slotmaster = ship.AllSlotMaster.ToArray();

				sb.AppendFormat( "\r\n昼战 : {0}", Constants.GetDayAttackKind( Calculator.GetDayAttackKind( slotmaster, ship.ShipID, -1 ) ) );
				{
					int shelling = ship.ShellingPower;
					int aircraft = ship.AircraftPower;
					if ( shelling > 0 ) {
						if ( aircraft > 0 )
							sb.AppendFormat( " - 炮击 : {0} / 空袭 : {1}", shelling, aircraft );
						else
							sb.AppendFormat( " - 威力 : {0}", shelling );
					} else if ( aircraft > 0 )
						sb.AppendFormat( " - 威力 : {0}", aircraft );
				}
				sb.AppendLine();

				sb.AppendFormat( "夜战 : {0}", Constants.GetNightAttackKind( Calculator.GetNightAttackKind( slotmaster, ship.ShipID, -1 ) ) );
				{
					int night = ship.NightBattlePower;
					if ( night > 0 ) {
						sb.AppendFormat( " - 威力 : {0}", night );
					}
				}
				sb.AppendLine();

				{
					int torpedo = ship.TorpedoPower;
					int asw = ship.AntiSubmarinePower;

					if ( torpedo > 0 ) {
						sb.AppendFormat( "雷击 : {0}", torpedo );
					}
					if ( asw > 0 ) {
						if ( torpedo > 0 )
							sb.Append( " / " );

						sb.AppendFormat( "对潜 : {0}", asw );

						if ( Calculator.CanOpeningASW( ship ) )
							sb.Append( " ( 可以先制 )" );
					}
					if ( torpedo > 0 || asw > 0 )
						sb.AppendLine();
				}

				{
					int aacutin = Calculator.GetAACutinKind( ship.ShipID, slotmaster );
					if ( aacutin != 0 ) {
						sb.AppendFormat( "对空 : {0}\r\n", Constants.GetAACutinKind( aacutin ) );
					}
					double adjustedaa = Calculator.GetAdjustedAAValue( ship );
					sb.AppendFormat( "加权对空 : {0} (比例击坠 : {1:p2})\r\n",
						adjustedaa,
						Calculator.GetProportionalAirDefense( adjustedaa )
						);

				}

				{
					int airsup_min;
					int airsup_max;
					if ( Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1 ) {
						airsup_min = Calculator.GetAirSuperiority( ship, false );
						airsup_max = Calculator.GetAirSuperiority( ship, true );
					} else {
						airsup_min = airsup_max = Calculator.GetAirSuperiorityIgnoreLevel( ship );
					}

					int airbattle = ship.AirBattlePower;
					if ( airsup_min > 0 ) {

						string airsup_str;
						if ( Utility.Configuration.Config.FormFleet.ShowAirSuperiorityRange && airsup_min < airsup_max ) {
							airsup_str = string.Format( "{0} ～ {1}", airsup_min, airsup_max );
						} else {
							airsup_str = airsup_min.ToString();
						}

						if ( airbattle > 0 )
							sb.AppendFormat( "制空战力 : {0} / 航空威力 : {1}\r\n", airsup_str, airbattle );
						else
							sb.AppendFormat( "制空战力 : {0}\r\n", airsup_str );
					} else if ( airbattle > 0 )
						sb.AppendFormat( "航空威力 : {0}\r\n", airbattle );
				}

				return sb.ToString();
			}

			private void SetConditionDesign( int cond ) {

				if ( Condition.ImageAlign == ContentAlignment.MiddleCenter ) {
					// icon invisible
					Condition.ImageIndex = -1;

					if ( cond < 20 )
					{
						Condition.BackColor = Utility.Configuration.Config.UI.Fleet_ColorConditionVeryTired;
						Condition.ForeColor = Utility.Configuration.Config.UI.Fleet_ColorConditionText;
					}
					else if ( cond < 30 )
					{
						Condition.BackColor = Utility.Configuration.Config.UI.Fleet_ColorConditionTired;
						Condition.ForeColor = Utility.Configuration.Config.UI.Fleet_ColorConditionText;
					}
					else if ( cond < 40 )
					{
						Condition.BackColor = Utility.Configuration.Config.UI.Fleet_ColorConditionLittleTired;
						Condition.ForeColor = Utility.Configuration.Config.UI.Fleet_ColorConditionText;
					}
					else if ( cond < 50 )
					{
						Condition.BackColor = Utility.Configuration.Config.UI.BackColor;
						Condition.ForeColor = Utility.Configuration.Config.UI.ForeColor;
					}
					else
					{
						Condition.BackColor = Utility.Configuration.Config.UI.Fleet_ColorConditionSparkle;
						Condition.ForeColor = Utility.Configuration.Config.UI.Fleet_ColorConditionText;
					}

				} else {
					Condition.BackColor = Utility.Configuration.Config.UI.BackColor;

					if ( cond < 20 )
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
					else if ( cond < 30 )
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionTired;
					else if ( cond < 40 )
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionLittleTired;
					else if ( cond < 50 )
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionNormal;
					else
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionSparkle;

				}
			}

			public void ConfigurationChanged( FormFleet parent ) {
				Name.Font = parent.MainFont;
				Level.MainFont = parent.MainFont;
				Level.SubFont = parent.SubFont;
				HP.MainFont = parent.MainFont;
				HP.SubFont = parent.SubFont;
				Condition.Font = parent.MainFont;
				SetConditionDesign( ( Condition.Tag as int? ) ?? 49 );
				Equipments.Font = parent.SubFont;
			}
		}




		public int FleetID { get; private set; }


		public Font MainFont { get; set; }
		public Font SubFont { get; set; }
		public Color MainFontColor { get; set; }
		public Color SubFontColor { get; set; }


		private TableFleetControl ControlFleet;
		private TableMemberControl[] ControlMember;

		private int AnchorageRepairBound;


		public FormFleet( FormMain parent, int fleetID ) {
			InitializeComponent();
			this.SizeChanged += FormFleet_SizeChanged;

			FleetID = fleetID;
			Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;

			ConfigurationChanged();

			MainFontColor = Utility.Configuration.Config.UI.ForeColor;
			SubFontColor = Utility.Configuration.Config.UI.SubForeColor;

			AnchorageRepairBound = 0;

			//ui init

			ControlHelper.SetDoubleBuffered( TableFleet );
			ControlHelper.SetDoubleBuffered( TableMember );


			TableFleet.Visible = false;
			TableFleet.SuspendLayout();
			// TableFleet.BorderStyle = BorderStyle.FixedSingle;
			TableFleet.BackColor = Utility.Configuration.Config.UI.SubBackColor;
			ControlFleet = new TableFleetControl( this, TableFleet );
			TableFleet.ResumeLayout();


			TableMember.Visible = false;
			TableMember.SuspendLayout();
			ControlMember = new TableMemberControl[6];
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i] = new TableMemberControl( this, TableMember, i );
			}
			TableMember.ResumeLayout();


			ConfigurationChanged();		//fixme: 苦渋の決断

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet] );

		}

		private void FormFleet_SizeChanged(object sender, EventArgs e)
		{
			TableFleet.MinimumSize = new Size(Math.Max(this.Width, TableMember.Width), 0);
			TableMember.MinimumSize = new Size(this.Width, 0);
		}

		private void FormFleet_Load( object sender, EventArgs e ) {

			Text = string.Format( "#{0}", FleetID );

			APIObserver o = APIObserver.Instance;

			o.APIList["api_req_nyukyo/start"].RequestReceived += Updated;
			o.APIList["api_req_nyukyo/speedchange"].RequestReceived += Updated;
			o.APIList["api_req_hensei/change"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o.APIList["api_req_member/updatedeckname"].RequestReceived += Updated;
			o.APIList["api_req_kaisou/remodeling"].RequestReceived += Updated;
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
			o.APIList["api_get_member/slot_item"].ResponseReceived += Updated;
			o.APIList["api_req_map/start"].ResponseReceived += Updated;
			o.APIList["api_req_map/next"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship_deck"].ResponseReceived += Updated;
			o.APIList["api_req_hensei/preset_select"].ResponseReceived += Updated;
			o.APIList["api_req_kaisou/slot_exchange_index"].ResponseReceived += Updated;
			o.APIList["api_get_member/require_info"].ResponseReceived += Updated;
			o.APIList["api_req_kaisou/slot_deprive"].ResponseReceived += Updated;


			//追加するときは FormFleetOverview にも同様に追加してください

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}


		void Updated( string apiname, dynamic data ) {

			if ( IsRemodeling ) {
				if ( apiname == "api_get_member/slot_item" )
					IsRemodeling = false;
				else
					return;
			}
			if ( apiname == "api_req_kaisou/remodeling" ) {
				IsRemodeling = true;
				return;
			}

			KCDatabase db = KCDatabase.Instance;

			if ( db.Ships.Count == 0 ) return;

			FleetData fleet = db.Fleet.Fleets[FleetID];
			if ( fleet == null ) return;

			TableFleet.SuspendLayout();
			ControlFleet.Update( fleet );
			TableFleet.Visible = true;
			TableFleet.ResumeLayout();

			AnchorageRepairBound = fleet.CanAnchorageRepair ? 2 + fleet.MembersInstance[0].SlotInstance.Count( eq => eq != null && eq.MasterEquipment.CategoryType == 31 ) : 0;

			TableMember.SuspendLayout();
			TableMember.RowCount = fleet.Members.Count( id => id > 0 );
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i].Update( fleet.Members[i] );
			}

			if (fleet.Members[0] == -1) {
				TableMember.Visible = false;
			} else {
				TableMember.Visible = true;
			}
			TableMember.ResumeLayout();


			if ( Icon != null ) ResourceManager.DestroyIcon( Icon );
			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[ControlFleet.State.GetIconIndex()] );
			if ( Parent != null ) Parent.Refresh();		//アイコンを更新するため

		}


		void UpdateTimerTick() {

			FleetData fleet = KCDatabase.Instance.Fleet.Fleets[FleetID];

			TableFleet.SuspendLayout();
			{
				if ( fleet != null )
					ControlFleet.Refresh();

			}
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i].HP.Refresh();
			}
			TableMember.ResumeLayout();


			// anchorage repairing
			if ( fleet != null && Utility.Configuration.Config.FormFleet.ReflectAnchorageRepairHealing ) {
				TimeSpan elapsedTime = DateTime.Now - KCDatabase.Instance.Fleet.AnchorageRepairingTimer;
				int elapsedMinutes = (int)elapsedTime.TotalMinutes;

				if ( elapsedMinutes >= 20 && elapsedMinutes != KCDatabase.Instance.Fleet.AnchorageRepairingLastElapsedMinutes && AnchorageRepairBound > 0) {

					for ( int i = 0; i < AnchorageRepairBound; i++ ) {
						ShipData ship = fleet.MembersInstance[i];
						if (ship == null)
							continue;

						var hpbar = ControlMember[i].HP;

						double dockingSeconds = hpbar.Tag as double? ?? 0.0;

						if ( dockingSeconds <= 0.0 )
							continue;

						hpbar.SuspendUpdate();

						if ( !hpbar.UsePrevValue ) {
							hpbar.UsePrevValue = true;
							hpbar.ShowDifference = true;
						}

						int[] healstatus = Calculator.CalculateAnchorageRepairHealAmountAndMinutesToNextHP(ship, elapsedMinutes);

						hpbar.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.MouseOver;
						hpbar.RepairTime = KCDatabase.Instance.Fleet.AnchorageRepairingTimer + TimeSpan.FromMinutes(healstatus[1]);
						hpbar.Value = hpbar.PrevValue + healstatus[0];

						hpbar.ResumeUpdate();
					}

					KCDatabase.Instance.Fleet.AnchorageRepairingLastElapsedMinutes = elapsedMinutes;
				}
			}
		}


		//艦隊編成のコピー
		private void ContextMenuFleet_CopyFleet_Click( object sender, EventArgs e ) {

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;
			FleetData fleet = db.Fleet[FleetID];
			if ( fleet == null ) return;

			sb.AppendFormat( "{0}\t制空战力 {1} / 索敌能力 {2} / 运输能力 {3}\r\n", fleet.Name, fleet.GetAirSuperiority(), fleet.GetSearchingAbilityString(), Calculator.GetTPDamage( fleet ) );
			for ( int i = 0; i < fleet.Members.Count; i++ ) {
				if ( fleet[i] == -1 )
					continue;

				ShipData ship = db.Ships[fleet[i]];

				sb.AppendFormat( "{0}/{1}\t", ship.MasterShip.Name, ship.Level );

				var eq = ship.AllSlotInstance;


				if ( eq != null ) {
					for ( int j = 0; j < eq.Count; j++ ) {

						if ( eq[j] == null ) continue;

						int count = 1;
						for ( int k = j + 1; k < eq.Count; k++ ) {
							if ( eq[k] != null && eq[k].EquipmentID == eq[j].EquipmentID && eq[k].Level == eq[j].Level && eq[k].AircraftLevel == eq[j].AircraftLevel ) {
								count++;
							} else {
								break;
							}
						}

						if ( count == 1 ) {
							sb.AppendFormat( "{0}{1}", j == 0 ? "" : "/", eq[j].NameWithLevel );
						} else {
							sb.AppendFormat( "{0}{1}x{2}", j == 0 ? "" : "/", eq[j].NameWithLevel, count );
						}

						j += count - 1;
					}
				}

				sb.AppendLine();
			}


			Clipboard.SetData( DataFormats.StringFormat, sb.ToString() );
		}


		private void ContextMenuFleet_Opening( object sender, CancelEventArgs e ) {

			ContextMenuFleet_Capture.Visible = Utility.Configuration.Config.Debug.EnableDebugMenu;

		}



		/// <summary>
		/// 「艦隊デッキビルダー」用編成コピー
		/// <see cref="http://www.kancolle-calc.net/deckbuilder.html"/>
		/// </summary>
		private void ContextMenuFleet_CopyFleetDeckBuilder_Click( object sender, EventArgs e ) {

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;

			// 手書き json の悲しみ

			sb.Append( @"{""version"":4," );

			foreach ( var fleet in db.Fleet.Fleets.Values ) {
				if ( fleet == null || fleet.MembersInstance.All( m => m == null ) ) continue;

				sb.AppendFormat( @"""f{0}"":{{", fleet.FleetID );

				int shipcount = 1;
				foreach ( var ship in fleet.MembersInstance ) {
					if ( ship == null ) break;

					sb.AppendFormat( @"""s{0}"":{{""id"":{1},""lv"":{2},""luck"":{3},""items"":{{",
						shipcount,
						ship.ShipID,
						ship.Level,
						ship.LuckBase );

					int eqcount = 1;
					foreach ( var eq in ship.AllSlotInstance.Where( eq => eq != null ) ) {
						if ( eq == null ) break;

						sb.AppendFormat( @"""i{0}"":{{""id"":{1},""rf"":{2},""mas"":{3}}},", eqcount >= 5 ? "x" : eqcount.ToString(), eq.EquipmentID, eq.Level, eq.AircraftLevel );

						eqcount++;
					}

					if ( eqcount > 1 )
						sb.Remove( sb.Length - 1, 1 );		// remove ","
					sb.Append( @"}}," );

					shipcount++;
				}

				if ( shipcount > 0 )
					sb.Remove( sb.Length - 1, 1 );		// remove ","
				sb.Append( @"}," );

			}

			sb.Remove( sb.Length - 1, 1 );		// remove ","
			sb.Append( @"}" );

			Clipboard.SetData( DataFormats.StringFormat, sb.ToString() );
		}


		/// <summary>
		/// 「艦隊晒しページ」用編成コピー
		/// <see cref="http://kancolle-calc.net/kanmusu_list.html"/>
		/// </summary>
		private void ContextMenuFleet_CopyKanmusuList_Click( object sender, EventArgs e ) {

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;

			// version
			sb.Append( ".2" );

			// <たね艦娘(完全未改造時)のID, 艦娘リスト>　に分類
			Dictionary<int, List<ShipData>> shiplist = new Dictionary<int, List<ShipData>>();

			foreach ( var ship in db.Ships.Values.Where( s => s.IsLocked ) ) {
				var master = ship.MasterShip;
				while ( master.RemodelBeforeShip != null )
					master = master.RemodelBeforeShip;

				if ( !shiplist.ContainsKey( master.ShipID ) ) {
					shiplist.Add( master.ShipID, new List<ShipData>() { ship } );
				} else {
					shiplist[master.ShipID].Add( ship );
				}
			}

			// 上で作った分類の各項を文字列化
			foreach ( var sl in shiplist ) {
				sb.Append( "|" ).Append( sl.Key ).Append( ":" );

				foreach ( var ship in sl.Value.OrderByDescending( s => s.Level ) ) {
					sb.Append( ship.Level );

					// 改造レベルに達しているのに未改造の艦は ".<たね=1, 改=2, 改二=3, ...>" を付加
					if ( ship.MasterShip.RemodelAfterShipID != 0 && ship.ExpNextRemodel == 0 ) {
						sb.Append( "." );
						int count = 1;
						var master = ship.MasterShip;
						while ( master.RemodelBeforeShip != null ) {
							master = master.RemodelBeforeShip;
							count++;
						}
						sb.Append( count );
					}
					sb.Append( "," );
				}

				// 余った "," を削除
				sb.Remove( sb.Length - 1, 1 );
			}

			Clipboard.SetData( DataFormats.StringFormat, sb.ToString() );
		}


		private void ContextMenuFleet_AntiAirDetails_Click( object sender, EventArgs e ) {

			var dialog = new DialogAntiAirDefense();

			dialog.SetFleetID( FleetID );
			dialog.Show( this );

		}


		private void ContextMenuFleet_Capture_Click( object sender, EventArgs e ) {

			using ( Bitmap bitmap = new Bitmap( this.ClientSize.Width, this.ClientSize.Height ) ) {
				this.DrawToBitmap( bitmap, this.ClientRectangle );

				Clipboard.SetData( DataFormats.Bitmap, bitmap );
			}
		}


		private void ContextMenuFleet_OutputFleetImage_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogFleetImageGenerator( FleetID ) ) {
				dialog.ShowDialog( this );
			}
		}



		void ConfigurationChanged() {

			var c = Utility.Configuration.Config;

			MainFont = Font = c.UI.JapFont;
			SubFont = c.UI.JapFont2;

			AutoScroll = c.FormFleet.IsScrollable;

			var fleet = KCDatabase.Instance.Fleet[FleetID];

			TableFleet.SuspendLayout();
			if ( ControlFleet != null && fleet != null ) {
				ControlFleet.ConfigurationChanged( this );
				ControlFleet.Update( fleet );
			}
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			if ( ControlMember != null ) {
				bool showAircraft = c.FormFleet.ShowAircraft;
				bool fixShipNameWidth = c.FormFleet.FixShipNameWidth;
				bool shortHPBar = c.FormFleet.ShortenHPBar;
				bool colorMorphing = c.UI.BarColorMorphing;
				Color[] colorScheme = c.UI.BarColorScheme.Select( col => col.ColorData ).ToArray();
				bool showNext = c.FormFleet.ShowNextExp;
				bool showConditionIcon = c.FormFleet.ShowConditionIcon;
				var levelVisibility = c.FormFleet.EquipmentLevelVisibility;
				bool showAircraftLevelByNumber = c.FormFleet.ShowAircraftLevelByNumber;
				int fixedShipNameWidth = c.FormFleet.FixedShipNameWidth;
				bool isLayoutFixed = c.UI.IsLayoutFixed;

				for ( int i = 0; i < ControlMember.Length; i++ ) {
					var member = ControlMember[i];

					member.Equipments.ShowAircraft = showAircraft;
					if ( fixShipNameWidth ) {
						member.Name.AutoSize = false;
						member.Name.Size = new Size( fixedShipNameWidth, 20 );
					} else {
						member.Name.AutoSize = true;
					}

					member.HP.SuspendUpdate();
					member.HP.Text = shortHPBar ? "" : "HP:";
					member.HP.HPBar.ColorMorphing = colorMorphing;
					member.HP.HPBar.SetBarColorScheme( colorScheme );
					member.HP.MaximumSize = isLayoutFixed ? new Size( int.MaxValue, (int)ControlHelper.GetDefaultRowStyle().Height - member.HP.Margin.Vertical ) : Size.Empty;
					member.HP.ResumeUpdate();
					member.Level.TextNext = showNext ? "next:" : null;
					member.Condition.ImageAlign = showConditionIcon ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleCenter;
					member.Equipments.LevelVisibility = levelVisibility;
					member.Equipments.ShowAircraftLevelByNumber = showAircraftLevelByNumber;
					member.Equipments.MaximumSize = isLayoutFixed ? new Size( int.MaxValue, (int)ControlHelper.GetDefaultRowStyle().Height - member.Equipments.Margin.Vertical ) : Size.Empty;
					member.ShipResource.BarFuel.ColorMorphing =
					member.ShipResource.BarAmmo.ColorMorphing = colorMorphing;
					member.ShipResource.BarFuel.SetBarColorScheme( colorScheme );
					member.ShipResource.BarAmmo.SetBarColorScheme( colorScheme );

					member.ConfigurationChanged( this );
					if ( fleet != null )
						member.Update( fleet.Members[i] );
				}
			}

			ControlHelper.SetTableRowStyles( TableMember, ControlHelper.GetDefaultRowStyle() );
			TableMember.ResumeLayout();

			TableMember.Location = new Point( TableMember.Location.X, TableFleet.Bottom /*+ Math.Max( TableFleet.Margin.Bottom, TableMember.Margin.Top )*/ );

			TableMember.PerformLayout();		//fixme:サイズ変更に親パネルが追随しない

		}



		private void TableMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine(Utility.Configuration.Config.UI.SubBackColorPen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}


		protected override string GetPersistString() {
			return "Fleet #" + FleetID.ToString();
		}


	}

}
