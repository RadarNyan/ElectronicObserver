using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	public partial class FormInformation : DockContent {

		private int _ignorePort;
		private List<int> _inSortie;
		private int[] _prevResource;

		public FormInformation( FormMain parent ) {
			InitializeComponent();

			_ignorePort = 0;
			_inSortie = null;
			_prevResource = new int[4];

			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormInformation] );
		}


		private void FormInformation_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			o["api_port/port"].ResponseReceived += Updated;
			o["api_req_member/get_practice_enemyinfo"].ResponseReceived += Updated;
			o["api_get_member/picture_book"].ResponseReceived += Updated;
			o["api_req_kousyou/createitem"].ResponseReceived += Updated;
			o["api_get_member/mapinfo"].ResponseReceived += Updated;
			o["api_req_mission/result"].ResponseReceived += Updated;
			o["api_req_practice/battle_result"].ResponseReceived += Updated;
			o["api_req_sortie/battleresult"].ResponseReceived += Updated;
			o["api_req_combined_battle/battleresult"].ResponseReceived += Updated;
			o["api_req_hokyu/charge"].ResponseReceived += Updated;
			o["api_req_map/start"].ResponseReceived += Updated;
			o["api_req_practice/battle"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}


		void ConfigurationChanged() {

			Font = TextInformation.Font = Utility.Configuration.Config.UI.JapFont;
			TextInformation.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
			TextInformation.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			TextInformation.BackColor = Utility.Configuration.Config.UI.BackColor;
		}


		void UpdateInfoText(string text, bool append = false)
		{
			if (!append)
				TextInformation.Text = "";

			foreach (string infoText in Regex.Split(text, "\\[Font")) {
				if (infoText.StartsWith("Chs]")) {
					TextInformation.SelectionFont = Utility.Configuration.Config.UI.MainFont;
					TextInformation.AppendText(infoText.Substring(4));
				} else if (infoText.StartsWith("Jpn]")) {
					TextInformation.SelectionFont = Utility.Configuration.Config.UI.JapFont;
					TextInformation.AppendText(infoText.Substring(4));
				} else { // firstline always in Japanese
					TextInformation.SelectionFont = Utility.Configuration.Config.UI.JapFont;
					TextInformation.AppendText(infoText);
				}
			}
		}


		void Updated( string apiname, dynamic data ) {

			switch ( apiname ) {

				case "api_port/port":
					if ( _ignorePort > 0 )
						_ignorePort--;
					else
						TextInformation.Text = "";		//とりあえずクリア

					if ( _inSortie != null ) {
						UpdateInfoText(GetConsumptionResource(data));
					}
					_inSortie = null;

					RecordMaterials();

					// '16 summer event
					if ( data.api_event_object() && data.api_event_object.api_m_flag2() && (int)data.api_event_object.api_m_flag2 > 0 ) {
						UpdateInfoText("[FontChs]＊解谜成功＊\r\n", true);
						Utility.Logger.Add(2, "", "已确认敌势力弱化！");
					}
					break;

				case "api_req_member/get_practice_enemyinfo":
					UpdateInfoText(GetPracticeEnemyInfo(data));
					RecordMaterials();
					break;

				case "api_get_member/picture_book":
					UpdateInfoText(GetAlbumInfo(data));
					break;

				case "api_req_kousyou/createitem":
					UpdateInfoText(GetCreateItemInfo(data));
					break;

				case "api_get_member/mapinfo":
					UpdateInfoText(GetMapGauge(data));
					break;

				case "api_req_mission/result":
					UpdateInfoText(GetExpeditionResult(data));
					_ignorePort = 1;
					break;

				case "api_req_practice/battle_result":
				case "api_req_sortie/battleresult":
				case "api_req_combined_battle/battleresult":
					UpdateInfoText(GetBattleResult(data));
					break;

				case "api_req_hokyu/charge":
					UpdateInfoText(GetSupplyInformation(data));
					break;

				case "api_req_map/start":
					_inSortie = KCDatabase.Instance.Fleet.Fleets.Values.Where( f => f.IsInSortie || f.ExpeditionState == 1 ).Select( f => f.FleetID ).ToList();

					RecordMaterials();
					break;

				case "api_req_practice/battle":
					_inSortie = new List<int>() { KCDatabase.Instance.Battle.BattleDay.Initial.FriendFleetID };
					break;
			}

		}


		private string GetPracticeEnemyInfo( dynamic data ) {

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("[FontChs][ 演习信息 ]");
			sb.AppendLine("对手提督名 : [FontJpn]" + data.api_nickname + "[FontChs]");
			sb.AppendLine("对手舰队名 : [FontJpn]" + data.api_deckname + "[FontChs]");

			{
				int ship1lv = (int)data.api_deck.api_ships[0].api_id != -1 ? (int)data.api_deck.api_ships[0].api_level : 1;
				int ship2lv = (int)data.api_deck.api_ships[1].api_id != -1 ? (int)data.api_deck.api_ships[1].api_level : 1;

				// 経験値テーブルが拡張されたとき用の対策
				ship1lv = Math.Min( ship1lv, ExpTable.ShipExp.Keys.Max() );
				ship2lv = Math.Min( ship2lv, ExpTable.ShipExp.Keys.Max() );

				double expbase = ExpTable.ShipExp[ship1lv].Total / 100.0 + ExpTable.ShipExp[ship2lv].Total / 300.0;
				if ( expbase >= 500.0 )
					expbase = 500.0 + Math.Sqrt( expbase - 500.0 );

				expbase = (int)expbase;

				sb.AppendFormat( "获得经验值 : {0} / S 胜利 : {1}\r\n", expbase, (int)( expbase * 1.2 ) );


				// 練巡ボーナス計算 - きたない
				var fleet = KCDatabase.Instance.Fleet[1];
				if ( fleet.MembersInstance.Any( s => s != null && s.MasterShip.ShipType == 21 ) ) {
					var members = fleet.MembersInstance;
					var subCT = members.Skip( 1 ).Where( s => s != null && s.MasterShip.ShipType == 21 );

					double bonus;

					// 旗艦が練巡
					if ( members[0] != null && members[0].MasterShip.ShipType == 21 ) {

						int level = members[0].Level;

						if ( subCT != null && subCT.Any() ) {
							// 旗艦+随伴
							if ( level < 10 ) bonus = 1.10;
							else if ( level < 30 ) bonus = 1.13;
							else if ( level < 60 ) bonus = 1.16;
							else if ( level < 100 ) bonus = 1.20;
							else bonus = 1.25;

						} else {
							// 旗艦のみ
							if ( level < 10 ) bonus = 1.05;
							else if ( level < 30 ) bonus = 1.08;
							else if ( level < 60 ) bonus = 1.12;
							else if ( level < 100 ) bonus = 1.15;
							else bonus = 1.20;
						}

					} else {

						int level = subCT.Max( s => s.Level );

						if ( subCT.Count() > 1 ) {
							// 随伴複数	
							if ( level < 10 ) bonus = 1.04;
							else if ( level < 30 ) bonus = 1.06;
							else if ( level < 60 ) bonus = 1.08;
							else if ( level < 100 ) bonus = 1.12;
							else bonus = 1.175;

						} else {
							// 随伴単艦
							if ( level < 10 ) bonus = 1.03;
							else if ( level < 30 ) bonus = 1.05;
							else if ( level < 60 ) bonus = 1.07;
							else if ( level < 100 ) bonus = 1.10;
							else bonus = 1.15;
						}
					}

					sb.AppendFormat( "( 练巡强化 : {0} / S 胜利 : {1} )\r\n", (int)( expbase * bonus ), (int)( (int)( expbase * 1.2 ) * bonus ) );


				}
			}

			return sb.ToString();
		}


		private string GetAlbumInfo( dynamic data ) {

			StringBuilder sb = new StringBuilder();

			if ( data != null && data.api_list() && data.api_list != null ) {

				if ( data.api_list[0].api_yomi() ) {
					//艦娘図鑑
					const int bound = 70;		// 図鑑1ページあたりの艦船数
					int startIndex = ( ( (int)data.api_list[0].api_index_no - 1 ) / bound ) * bound + 1;
					bool[] flags = Enumerable.Repeat<bool>( false, bound ).ToArray();

					sb.AppendLine("[FontChs][ 未回收中破绘 ][FontJpn]");

					foreach ( dynamic elem in data.api_list ) {

						flags[(int)elem.api_index_no - startIndex] = true;

						dynamic[] state = elem.api_state;
						for ( int i = 0; i < state.Length; i++ ) {
							if ( (int)state[i][1] == 0 ) {

								var target = KCDatabase.Instance.MasterShips[(int)elem.api_table_id[i]];
								if ( target != null )		//季節の衣替え艦娘の場合存在しないことがある
									sb.AppendLine( target.Name );
							}
						}

					}

					sb.AppendLine("[FontChs][ 未持有舰 ][FontJpn]");
					for ( int i = 0; i < bound; i++ ) {
						if ( !flags[i] ) {
							ShipDataMaster ship = KCDatabase.Instance.MasterShips.Values.FirstOrDefault( s => s.AlbumNo == startIndex + i );
							if ( ship != null ) {
								sb.AppendLine( ship.Name );
							}
						}
					}

				} else {
					//装備図鑑
					const int bound = 70;		// 図鑑1ページあたりの装備数
					int startIndex = ( ( (int)data.api_list[0].api_index_no - 1 ) / bound ) * bound + 1;
					bool[] flags = Enumerable.Repeat<bool>( false, bound ).ToArray();

					foreach ( dynamic elem in data.api_list ) {

						flags[(int)elem.api_index_no - startIndex] = true;
					}

					sb.AppendLine("[FontChs][ 未持有装备 ][FontJpn]");
					for ( int i = 0; i < bound; i++ ) {
						if ( !flags[i] ) {
							EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments.Values.FirstOrDefault( s => s.AlbumNo == startIndex + i );
							if ( eq != null ) {
								sb.AppendLine( eq.Name );
							}
						}
					}
				}
			}

			return sb.ToString();
		}


		private string GetCreateItemInfo( dynamic data ) {

			if ( (int)data.api_create_flag == 0 ) {

				StringBuilder sb = new StringBuilder();
				sb.AppendLine("[FontChs][ 开发失败 ][FontJpn]");
				sb.AppendLine( data.api_fdata );

				EquipmentDataMaster eqm = KCDatabase.Instance.MasterEquipments[int.Parse( ( (string)data.api_fdata ).Split( ",".ToCharArray() )[1] )];
				if ( eqm != null )
					sb.AppendLine( eqm.Name );


				return sb.ToString();

			} else
				return "";
		}


		private string GetMapGauge( dynamic data ) {

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("[FontChs][ 海域血条 ]");

			var list = data.api_map_info() ? data.api_map_info : data;

			foreach ( dynamic elem in list ) {

				int mapID = (int)elem.api_id;
				MapInfoData map = KCDatabase.Instance.MapInfo[mapID];

				if ( map != null ) {
					if ( map.RequiredDefeatedCount != -1 && elem.api_defeat_count() ) {

						sb.AppendFormat( "{0}-{1} : 击破 {2}/{3} 次\r\n", map.MapAreaID, map.MapInfoID, (int)elem.api_defeat_count, map.RequiredDefeatedCount );

					} else if ( elem.api_eventmap() ) {

						string difficulty = "";
						if ( elem.api_eventmap.api_selected_rank() ) {
							difficulty = "[" + Constants.GetDifficulty( (int)elem.api_eventmap.api_selected_rank ) + "] ";
						}

						sb.AppendFormat( "{0}-{1} {2}: {3} {4}/{5}\r\n",
							map.MapAreaID, map.MapInfoID, difficulty,
							elem.api_eventmap.api_gauge_type() && (int)elem.api_eventmap.api_gauge_type == 3 ? "TP" : "HP",
							(int)elem.api_eventmap.api_now_maphp, (int)elem.api_eventmap.api_max_maphp );

					}
				}
			}

			return sb.ToString();
		}


		private string GetExpeditionResult( dynamic data ) {
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("[FontChs][ 远征返回 ]");
			sb.AppendLine( data.api_quest_name );
			sb.AppendFormat( "结果 : {0}\r\n", Constants.GetExpeditionResult( (int)data.api_clear_result ) );
			sb.AppendFormat( "提督经验值 : +{0}\r\n", (int)data.api_get_exp );
			sb.AppendFormat( "舰娘经验值 : +{0}\r\n", ( (int[])data.api_get_ship_exp ).Min() );

			return sb.ToString();
		}


		private string GetBattleResult( dynamic data ) {
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("[FontChs][ 战斗结束 ]");
			sb.AppendFormat( "敌舰队名 : [FontJpn]{0}\r\n[FontChs]", data.api_enemy_info.api_deck_name );
			sb.AppendFormat( "胜负判定 : {0}\r\n", data.api_win_rank );
			sb.AppendFormat( "提督经验值 : +{0}\r\n", (int)data.api_get_exp );

			return sb.ToString();
		}


		private string GetSupplyInformation( dynamic data ) {

			StringBuilder sb = new StringBuilder();

			sb.AppendLine("[FontChs][ 补给完成 ]");
			sb.AppendFormat( "铝土 : {0} ( 舰载机 {1} 架 )\r\n", (int)data.api_use_bou, (int)data.api_use_bou / 5 );

			return sb.ToString();
		}


		private string GetConsumptionResource( dynamic data ) {

			StringBuilder sb = new StringBuilder();
			var material = KCDatabase.Instance.Material;

			int fuel_supply = 0,
				fuel_repair = 0,
				ammo = 0,
				steel = 0,
				bauxite = 0;

			int fuel_diff = material.Fuel - _prevResource[0],
				ammo_diff = material.Ammo - _prevResource[1],
				steel_diff = material.Steel - _prevResource[2],
				bauxite_diff = material.Bauxite - _prevResource[3];

			sb.AppendLine( "[FontChs][ 舰队回港 ]" );

			foreach ( var f in KCDatabase.Instance.Fleet.Fleets.Values.Where( f => _inSortie.Contains( f.FleetID ) ) ) {

				fuel_supply += f.MembersInstance.Sum( s => s == null ? 0 : (int)Math.Floor( ( s.FuelMax - s.Fuel ) * ( s.IsMarried ? 0.85 : 1.0 ) ) );
				ammo += f.MembersInstance.Sum( s => s == null ? 0 : (int)Math.Floor( ( s.AmmoMax - s.Ammo ) * ( s.IsMarried ? 0.85 : 1.0 ) ) );
				bauxite += f.MembersInstance.Sum( s => s == null ? 0 : s.Aircraft.Zip( s.MasterShip.Aircraft, ( current, max ) => new { Current = current, Max = max } ).Sum( a => ( a.Max - a.Current ) * 5 ) );

				fuel_repair += f.MembersInstance.Sum( s => s == null ? 0 : s.RepairFuel );
				steel += f.MembersInstance.Sum( s => s == null ? 0 : s.RepairSteel );

			}

			sb.AppendFormat( "燃料 : {0:+0;-0} ( 自然 {1:+0;-0} - 补给 {2} - 入渠 {3} )\r\n弹药 : {4:+0;-0} ( 自然 {5:+0;-0} - 补给 {6} )\r\n钢材 : {7:+0;-0} ( 自然 {8:+0;-0} - 入渠 {9} )\r\n铝土 : {10:+0;-0} ( 自然 {11:+0;-0} - 补给 {12} ( 舰载机 {13} 架 ) )",
				fuel_diff - fuel_supply - fuel_repair, fuel_diff, fuel_supply, fuel_repair,
				ammo_diff - ammo, ammo_diff, ammo,
				steel_diff - steel, steel_diff, steel,
				bauxite_diff - bauxite, bauxite_diff, bauxite, bauxite / 5 );

			return sb.ToString();
		}


		private void RecordMaterials() {
			var material = KCDatabase.Instance.Material;
			_prevResource[0] = material.Fuel;
			_prevResource[1] = material.Ammo;
			_prevResource[2] = material.Steel;
			_prevResource[3] = material.Bauxite;
		}

		protected override string GetPersistString() {
			return "Information";
		}


		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern int HideCaret (IntPtr hwnd);


		private void HideCaret(object sender, EventArgs e)
		{
			HideCaret(TextInformation.Handle);
		}
	}

}
