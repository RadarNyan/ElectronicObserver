using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.Window.Dialog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Utility {


	public sealed class Configuration {


		private static readonly Configuration instance = new Configuration();

		public static Configuration Instance {
			get { return instance; }
		}


		private const string SaveFileName = @"Settings\Configuration.xml";


		public delegate void ConfigurationChangedEventHandler();
		public event ConfigurationChangedEventHandler ConfigurationChanged = delegate { };


		[DataContract( Name = "Configuration" )]
		public class ConfigurationData : DataStorage {

			public class ConfigPartBase {
				//reserved
			}


			/// <summary>
			/// 通信の設定を扱います。
			/// </summary>
			public class ConfigConnection : ConfigPartBase {

				/// <summary>
				/// ポート番号
				/// </summary>
				public ushort Port { get; set; }

				/// <summary>
				/// 通信内容を保存するか
				/// </summary>
				public bool SaveReceivedData { get; set; }

				/// <summary>
				/// 通信内容保存：保存先
				/// </summary>
				public string SaveDataPath { get; set; }

				/// <summary>
				/// 通信内容保存：Requestを保存するか
				/// </summary>
				public bool SaveRequest { get; set; }

				/// <summary>
				/// 通信内容保存：Responseを保存するか
				/// </summary>
				public bool SaveResponse { get; set; }

				/// <summary>
				/// 通信内容保存：SWFを保存するか
				/// </summary>
				public bool SaveSWF { get; set; }

				/// <summary>
				/// 通信内容保存：その他ファイルを保存するか
				/// </summary>
				public bool SaveOtherFile { get; set; }

				/// <summary>
				/// 通信内容保存：バージョンを追加するか
				/// </summary>
				public bool ApplyVersion { get; set; }


				/// <summary>
				/// システムプロキシに登録するか
				/// </summary>
				public bool RegisterAsSystemProxy { get; set; }

				/// <summary>
				/// 上流プロキシを利用するか
				/// </summary>
				public bool UseUpstreamProxy { get; set; }

				/// <summary>
				/// 上流プロキシのポート番号
				/// </summary>
				public ushort UpstreamProxyPort { get; set; }

				/// <summary>
				/// 上流プロキシのアドレス
				/// </summary>
				public string UpstreamProxyAddress { get; set; }

				/// <summary>
				/// システムプロキシを利用するか
				/// </summary>
				public bool UseSystemProxy { get; set; }

				/// <summary>
				/// 下流プロキシ設定
				/// 空なら他の設定から自動生成する
				/// </summary>
				public string DownstreamProxy { get; set; }


				/// <summary>
				/// kancolle-db.netに送信する
				/// </summary>
				public bool SendDataToKancolleDB { get; set; }

				/// <summary>
				/// kancolle-db.netのOAuth認証
				/// </summary>
				public string SendKancolleOAuth { get; set; }


				public ConfigConnection() {

					Port = 40620;
					SaveReceivedData = false;
					SaveDataPath = @"KCAPI";
					SaveRequest = false;
					SaveResponse = true;
					SaveSWF = false;
					SaveOtherFile = false;
					ApplyVersion = false;
					RegisterAsSystemProxy = false;
					UseUpstreamProxy = false;
					UpstreamProxyPort = 0;
					UpstreamProxyAddress = "127.0.0.1";
					UseSystemProxy = false;
					DownstreamProxy = "";
					SendDataToKancolleDB = false;
					SendKancolleOAuth = "";
				}

			}
			/// <summary>通信</summary>
			[DataMember]
			public ConfigConnection Connection { get; private set; }


			public class ConfigUI : ConfigPartBase {

				/// <summary>
				/// メインフォント
				/// </summary>
				public SerializableFont MainFont { get; set; }

				/// <summary>
				/// サブフォント
				/// </summary>
				public SerializableFont SubFont { get; set; }

				/// <summary>
				/// Japanese Main Font ( Hard-coded to Meiryo UI, 12px )
				/// </summary>
				public SerializableFont JapFont { get; set; }

				/// <summary>
				/// Japanese Sub Font ( Hard-coded to Meiryo UI, 10px )
				/// </summary>
				public SerializableFont JapFont2 { get; set; }

				// ThemeID
				public int ThemeID { get; set; }

				// 司令部显示战果
				public bool ShowGrowthInsteadOfNextInHQ { get; set; }

				// 明石 ToolTip 每 HP 耗时最大显示数量
				public int MaxAkashiPerHP { get; set; }

				[IgnoreDataMember]
				private bool _barColorMorphing;

				/// <summary>
				/// HPバーの色を滑らかに変化させるか
				/// </summary>
				public bool BarColorMorphing {
					get {
						SetBarColorScheme();
						return _barColorMorphing;
					} set {
						_barColorMorphing = value;
						SetBarColorScheme();
					}
				}

				public void SetBarColorScheme() {
					if ( BarColorSchemes == null ) return;
					if ( !_barColorMorphing ) {
						BarColorScheme = new List<SerializableColor>( BarColorSchemes[0] );
					} else {
						BarColorScheme = new List<SerializableColor>( BarColorSchemes[1] );
					}
				}

				/// <summary>
				/// HPバーのカラーリング
				/// </summary>
				public List<SerializableColor> BarColorScheme { get; private set; }

				#region - UI Colors -

				// 数值条 ( 耐久、燃料、弹药... ) 颜色
				[IgnoreDataMember]
				public List<SerializableColor>[] BarColorSchemes { get; set; }
				// 面板颜色
				[IgnoreDataMember]
				public Color ForeColor { get; set; }
				[IgnoreDataMember]
				public Color BackColor { get; set; }
				[IgnoreDataMember]
				public Color SubForeColor { get; set; }
				[IgnoreDataMember]
				public Color SubBackColor { get; set; }
				[IgnoreDataMember]
				public Pen SubBackColorPen { get; set; }
				// 状态栏颜色
				[IgnoreDataMember]
				public Color StatusBarForeColor { get; set; }
				[IgnoreDataMember]
				public Color StatusBarBackColor { get; set; }
				// 标签页颜色 ( DockPanelSuite )
				[IgnoreDataMember]
				public string[] DockPanelSuiteStyles { get; set; }
				// 基本颜色
				[IgnoreDataMember]
				public Color Color_Red { get; set; }
				[IgnoreDataMember]
				public Color Color_Orange { get; set; }
				[IgnoreDataMember]
				public Color Color_Yellow { get; set; }
				[IgnoreDataMember]
				public Color Color_Green { get; set; }
				[IgnoreDataMember]
				public Color Color_Cyan { get; set; }
				[IgnoreDataMember]
				public Color Color_Blue { get; set; }
				[IgnoreDataMember]
				public Color Color_Magenta { get; set; }
				[IgnoreDataMember]
				public Color Color_Violet { get; set; }

				// 待整理
				public Color Blink_ForeColor { get { return BackColor; } }
				public Color Blink_SubForeColor { get { return SubBackColor; } }
				public Color Blink_BackColorLightCoral { get { return Color_Red; } }
				public Color Blink_BackColorLightGreen { get { return Color_Cyan; } }
/*
				public Color Blink_ForeColor { get {
				switch (ThemeID) {
					case 0:  return SolarizedBase3.ColorData;
					case 1:  return SolarizedBase03.ColorData;
					default: return SystemColors.ControlText;
				}}}
				public Color Blink_SubForeColor { get {
				switch (ThemeID) {
					//case 0:  return SolarizedBase2.ColorData;
					//case 1:  return SolarizedBase02.ColorData;
					default: return SystemColors.ControlText;
				}}}
				public Color Blink_BackColorLightCoral { get {
				switch (ThemeID) {
					//case 0:  return SolarizedRed.ColorData;
					//case 1:  return SolarizedRed.ColorData;
					default: return Color.LightCoral;
				}}}
				public Color Blink_BackColorLightGreen { get {
				switch (ThemeID) {
					//case 0:  return SolarizedCyan.ColorData;
					//case 1:  return SolarizedCyan.ColorData;
					default: return Color.LightGreen;
				}}}
*/
				// 视图 - 舰队
				[IgnoreDataMember] // 严重疲劳
				public Color Fleet_ColorConditionVeryTired { get; set; }
				[IgnoreDataMember] // 中等疲劳
				public Color Fleet_ColorConditionTired { get; set; }
				[IgnoreDataMember] // 轻微疲劳
				public Color Fleet_ColorConditionLittleTired { get; set; }
				[IgnoreDataMember] // 战意高扬
				public Color Fleet_ColorConditionSparkle { get; set; }
				[IgnoreDataMember] // 装备改修值
				public Color Fleet_equipmentLevelColor { get; set; }

				// 视图 - 任务
				[IgnoreDataMember] // 编成
				public Color Quest_Type1Color { get; set; }
				[IgnoreDataMember] // 出击
				public Color Quest_Type2Color { get; set; }
				[IgnoreDataMember] // 演习
				public Color Quest_Type3Color { get; set; }
				[IgnoreDataMember] // 远征
				public Color Quest_Type4Color { get; set; }
				[IgnoreDataMember] // 补给、入渠
				public Color Quest_Type5Color { get; set; }
				[IgnoreDataMember] // 工厂
				public Color Quest_Type6Color { get; set; }
				[IgnoreDataMember] // 改装
				public Color Quest_Type7Color { get; set; }
				[IgnoreDataMember] // 进度 <50%
				public Color Quest_ColorProcessLT50 { get; set; }
				[IgnoreDataMember] // 进度 <80%
				public Color Quest_ColorProcessLT80 { get; set; }
				[IgnoreDataMember] // 进度 <100%
				public Color Quest_ColorProcessLT100 { get; set; }
				[IgnoreDataMember] // 进度 100%
				public Color Quest_ColorProcessDefault { get; set; }

				// 视图 - 罗盘
				[IgnoreDataMember] // 敌舰名 - elite
				public Color Compass_ShipNameColor2 { get; set; }
				[IgnoreDataMember] // 敌舰名 - flagship
				public Color Compass_ShipNameColor3 { get; set; }
				[IgnoreDataMember] // 敌舰名 - 改 flagship / 后期型
				public Color Compass_ShipNameColor4 { get; set; }
				[IgnoreDataMember] // 敌舰名 - 后期型 elite
				public Color Compass_ShipNameColor5 { get; set; }
				[IgnoreDataMember] // 敌舰名 - 后期型 flagship
				public Color Compass_ShipNameColor6 { get; set; }
				[IgnoreDataMember] // 事件类型 - 夜战
				public Color Compass_ColorTextEventKind3 { get; set; }
				[IgnoreDataMember] // 事件类型 - 航空战 / 长距离空袭战
				public Color Compass_ColorTextEventKind6 { get; set; }
				[IgnoreDataMember] // 事件类型 - 敌联合舰队
				public Color Compass_ColorTextEventKind5 { get; set; }
				[IgnoreDataMember] // 半透明背景色，当舰载机数量叠加到飞机图标上时背景填充的色块
				public Color Compass_ColoroverlayBrush { get; set; }
				// default: return Color.FromArgb(0xC0, 0xF0, 0xF0, 0xF0);

				// 视图 - 战斗：血条背景色、血条文字色
				[IgnoreDataMember] // 受损状态 BOSS
				public Color Battle_ColorHPBarsBossDamaged { get; set; }
				[IgnoreDataMember] // MVP
				public Color Battle_ColorHPBarsMVP { get; set; }
				[IgnoreDataMember] // 已退避
				public Color Battle_ColorHPBarsEscaped { get; set; }

				// 视图 - 舰队：入渠中计时器
				public Color Battle_ColorHPTextRepair { get {
					return SubBackColor;
					// return Color.FromArgb(0x00, 0x00, 0x88);
				}}
				public bool RemoveBarShadow { get {
				switch (ThemeID) {
					case 0:  return true;
					case 1:  return true;
					default: return false;
				}}}

				#endregion

				public ConfigUI() {
					MainFont = new Font( "Microsoft YaHei", 12, FontStyle.Regular, GraphicsUnit.Pixel );
					SubFont  = new Font( "Microsoft YaHei", 10, FontStyle.Regular, GraphicsUnit.Pixel );
					JapFont  = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
					JapFont2 = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
					ThemeID = 0;
					ShowGrowthInsteadOfNextInHQ = false;
					MaxAkashiPerHP = 5;
					BarColorMorphing = false;
				}
			}
			/// <summary>UI</summary>
			[DataMember]
			public ConfigUI UI { get; private set; }


			/// <summary>
			/// ログの設定を扱います。
			/// </summary>
			public class ConfigLog : ConfigPartBase {

				/// <summary>
				/// ログのレベル
				/// </summary>
				public int LogLevel { get; set; }

				/// <summary>
				/// ログを保存するか
				/// </summary>
				public bool SaveLogFlag { get; set; }

				/// <summary>
				/// エラーレポートを保存するか
				/// </summary>
				public bool SaveErrorReport { get; set; }

				/// <summary>
				/// ファイル エンコーディングのID
				/// </summary>
				public int FileEncodingID { get; set; }

				/// <summary>
				/// ファイル エンコーディング
				/// </summary>
				[IgnoreDataMember]
				public Encoding FileEncoding {
					get {
						switch ( FileEncodingID ) {
							case 0:
								return new System.Text.UTF8Encoding( false );
							case 1:
								return new System.Text.UTF8Encoding( true );
							case 2:
								return new System.Text.UnicodeEncoding( false, false );
							case 3:
								return new System.Text.UnicodeEncoding( false, true );
							case 4:
								return Encoding.GetEncoding( 932 );
							default:
								return new System.Text.UTF8Encoding( false );

						}
					}
				}

				/// <summary>
				/// ネタバレを許可するか
				/// </summary>
				public bool ShowSpoiler { get; set; }

				/// <summary>
				/// プレイ時間
				/// </summary>
				public double PlayTime { get; set; }

				/// <summary>
				/// これ以上の無通信時間があったときプレイ時間にカウントしない
				/// </summary>
				public double PlayTimeIgnoreInterval { get; set; }

				/// <summary>
				/// 戦闘ログを保存するか
				/// </summary>
				public bool SaveBattleLog { get; set; }

				public ConfigLog() {
					LogLevel = 2;
					SaveLogFlag = true;
					SaveErrorReport = true;
					FileEncodingID = 1;
					ShowSpoiler = true;
					PlayTime = 0;
					PlayTimeIgnoreInterval = 10 * 60;
					SaveBattleLog = false;
				}

			}
			/// <summary>ログ</summary>
			[DataMember]
			public ConfigLog Log { get; private set; }


			/// <summary>
			/// 動作の設定を扱います。
			/// </summary>
			public class ConfigControl : ConfigPartBase {

				/// <summary>
				/// 疲労度ボーダー
				/// </summary>
				public int ConditionBorder { get; set; }

				/// <summary>
				/// レコードを自動保存するか
				/// 0=しない、1=1時間ごと、2=1日ごと
				/// </summary>
				public int RecordAutoSaving { get; set; }

				/// <summary>
				/// システムの音量設定を利用するか
				/// </summary>
				public bool UseSystemVolume { get; set; }

				/// <summary>
				/// 前回終了時の音量
				/// </summary>
				public float LastVolume { get; set; }

				/// <summary>
				/// 前回終了時にミュート状態だったか
				/// </summary>
				public bool LastIsMute { get; set; }

				/// <summary>
				/// 威力表示の基準となる交戦形態
				/// </summary>
				public int PowerEngagementForm { get; set; }


				public ConfigControl() {
					ConditionBorder = 40;
					RecordAutoSaving = 1;
					UseSystemVolume = true;
					LastVolume = 0.8f;
					LastIsMute = false;
					PowerEngagementForm = 1;
				}
			}
			/// <summary>動作</summary>
			[DataMember]
			public ConfigControl Control { get; private set; }


			/// <summary>
			/// デバッグの設定を扱います。
			/// </summary>
			public class ConfigDebug : ConfigPartBase {

				/// <summary>
				/// デバッグメニューを有効にするか
				/// </summary>
				public bool EnableDebugMenu { get; set; }

				/// <summary>
				/// 起動時にAPIリストをロードするか
				/// </summary>
				public bool LoadAPIListOnLoad { get; set; }

				/// <summary>
				/// APIリストのパス
				/// </summary>
				public string APIListPath { get; set; }

				/// <summary>
				/// エラー発生時に警告音を鳴らすか
				/// </summary>
				public bool AlertOnError { get; set; }

				public ConfigDebug() {
					EnableDebugMenu = false;
					LoadAPIListOnLoad = false;
					APIListPath = "";
					AlertOnError = false;
				}
			}
			/// <summary>デバッグ</summary>
			[DataMember]
			public ConfigDebug Debug { get; private set; }


			/// <summary>
			/// 起動と終了の設定を扱います。
			/// </summary>
			public class ConfigLife : ConfigPartBase {

				/// <summary>
				/// 終了時に確認するか
				/// </summary>
				public bool ConfirmOnClosing { get; set; }

				/// <summary>
				/// 最前面に表示するか
				/// </summary>
				public bool TopMost { get; set; }

				/// <summary>
				/// レイアウトファイルのパス
				/// </summary>
				public string LayoutFilePath { get; set; }

				/// <summary>
				/// 更新情報を取得するか
				/// </summary>
				public bool CheckUpdateInformation { get; set; }

				/// <summary>
				/// ステータスバーを表示するか
				/// </summary>
				public bool ShowStatusBar { get; set; }

				/// <summary>
				/// 時計表示のフォーマット
				/// </summary>
				public int ClockFormat { get; set; }

				/// <summary>
				/// レイアウトをロックするか
				/// </summary>
				public bool LockLayout { get; set; }

				/// <summary>
				/// レイアウトロック中でもフロートウィンドウを閉じられるようにするか
				/// </summary>
				public bool CanCloseFloatWindowInLock { get; set; }

				public ConfigLife() {
					ConfirmOnClosing = true;
					TopMost = false;
					LayoutFilePath = @"Settings\WindowLayout.zip";
					CheckUpdateInformation = true;
					ShowStatusBar = true;
					ClockFormat = 0;
					LockLayout = false;
					CanCloseFloatWindowInLock = false;
				}
			}
			/// <summary>起動と終了</summary>
			[DataMember]
			public ConfigLife Life { get; private set; }


			/// <summary>
			/// [工廠]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormArsenal : ConfigPartBase {

				/// <summary>
				/// 艦名を表示するか
				/// </summary>
				public bool ShowShipName { get; set; }

				/// <summary>
				/// 完了時に点滅させるか
				/// </summary>
				public bool BlinkAtCompletion { get; set; }

				public ConfigFormArsenal() {
					ShowShipName = true;
					BlinkAtCompletion = true;
				}
			}
			/// <summary>[工廠]ウィンドウ</summary>
			[DataMember]
			public ConfigFormArsenal FormArsenal { get; private set; }


			/// <summary>
			/// [入渠]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormDock : ConfigPartBase {

				/// <summary>
				/// 完了時に点滅させるか
				/// </summary>
				public bool BlinkAtCompletion { get; set; }

				public ConfigFormDock() {
					BlinkAtCompletion = true;
				}
			}
			/// <summary>[入渠]ウィンドウ</summary>
			[DataMember]
			public ConfigFormDock FormDock { get; private set; }


			/// <summary>
			/// [司令部]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormHeadquarters : ConfigPartBase {

				/// <summary>
				/// 艦船/装備が満タンの時点滅するか
				/// </summary>
				public bool BlinkAtMaximum { get; set; }


				/// <summary>
				/// 項目の可視/不可視設定
				/// </summary>
				public SerializableList<bool> Visibility { get; set; }

				/// <summary>
				/// 任意アイテム表示のアイテムID
				/// </summary>
				public int DisplayUseItemID { get; set; }

				public ConfigFormHeadquarters() {
					BlinkAtMaximum = true;
					Visibility = null;		// フォーム側で設定します
					DisplayUseItemID = 68;	// 秋刀魚
				}
			}
			/// <summary>[司令部]ウィンドウ</summary>
			[DataMember]
			public ConfigFormHeadquarters FormHeadquarters { get; private set; }


			/// <summary>
			/// [艦隊]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormFleet : ConfigPartBase {

				/// <summary>
				/// 艦載機を表示するか
				/// </summary>
				public bool ShowAircraft { get; set; }

				/// <summary>
				/// 索敵式の計算方法
				/// </summary>
				public int SearchingAbilityMethod { get; set; }

				/// <summary>
				/// スクロール可能か
				/// </summary>
				public bool IsScrollable { get; set; }

				/// <summary>
				/// 艦名表示の幅を固定するか
				/// </summary>
				public bool FixShipNameWidth { get; set; }

				/// <summary>
				/// HPバーを短縮するか
				/// </summary>
				public bool ShortenHPBar { get; set; }

				/// <summary>
				/// next lv. を表示するか
				/// </summary>
				public bool ShowNextExp { get; set; }

				/// <summary>
				/// 装備の改修レベル・艦載機熟練度の表示フラグ
				/// </summary>
				public Window.Control.ShipStatusEquipment.LevelVisibilityFlag EquipmentLevelVisibility { get; set; }

				/// <summary>
				/// 制空戦力の計算方法
				/// </summary>
				public int AirSuperiorityMethod { get; set; }

				/// <summary>
				/// 泊地修理タイマを表示するか
				/// </summary>
				public bool ShowAnchorageRepairingTimer { get; set; }

				/// <summary>
				/// タイマー完了時に点滅させるか
				/// </summary>
				public bool BlinkAtCompletion { get; set; }

				/// <summary>
				/// 疲労度アイコンを表示するか
				/// </summary>
				public bool ShowConditionIcon { get; set; }

				/// <summary>
				/// 艦名表示幅固定時の幅
				/// </summary>
				public int FixedShipNameWidth { get; set; }

				public ConfigFormFleet() {
					ShowAircraft = true;
					SearchingAbilityMethod = 4;
					IsScrollable = true;
					FixShipNameWidth = false;
					ShortenHPBar = false;
					ShowNextExp = true;
					EquipmentLevelVisibility = Window.Control.ShipStatusEquipment.LevelVisibilityFlag.Both;
					AirSuperiorityMethod = 1;
					ShowAnchorageRepairingTimer = true;
					BlinkAtCompletion = true;
					ShowConditionIcon = true;
					FixedShipNameWidth = 40;
				}
			}
			/// <summary>[艦隊]ウィンドウ</summary>
			[DataMember]
			public ConfigFormFleet FormFleet { get; private set; }


			/// <summary>
			/// [任務]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormQuest : ConfigPartBase {

				/// <summary>
				/// 遂行中の任務のみ表示するか
				/// </summary>
				public bool ShowRunningOnly { get; set; }


				/// <summary>
				/// 単発を表示
				/// </summary>
				public bool ShowOnce { get; set; }

				/// <summary>
				/// デイリーを表示
				/// </summary>
				public bool ShowDaily { get; set; }

				/// <summary>
				/// ウィークリーを表示
				/// </summary>
				public bool ShowWeekly { get; set; }

				/// <summary>
				/// マンスリーを表示
				/// </summary>
				public bool ShowMonthly { get; set; }

				/// <summary>
				/// その他を表示
				/// </summary>
				public bool ShowOther { get; set; }

				/// <summary>
				/// 列の可視性
				/// </summary>
				public SerializableList<bool> ColumnFilter { get; set; }

				/// <summary>
				/// 列の幅
				/// </summary>
				public SerializableList<int> ColumnWidth { get; set; }

				/// <summary>
				/// どの行をソートしていたか
				/// </summary>
				public int SortParameter { get; set; }

				/// <summary>
				/// 進捗を自動保存するか
				/// 0 = しない、1 = 一時間ごと、2 = 一日ごと
				/// </summary>
				public int ProgressAutoSaving { get; set; }

				public bool AllowUserToSortRows { get; set; }

				public ConfigFormQuest() {
					ShowRunningOnly = false;
					ShowOnce = true;
					ShowDaily = true;
					ShowWeekly = true;
					ShowMonthly = true;
					ShowOther = true;
					ColumnFilter = null;		//実際の初期化は FormQuest で行う
					ColumnWidth = null;			//上に同じ
					SortParameter = 3 << 1 | 0;
					ProgressAutoSaving = 1;
					AllowUserToSortRows = true;
				}
			}
			/// <summary>[任務]ウィンドウ</summary>
			[DataMember]
			public ConfigFormQuest FormQuest { get; private set; }


			/// <summary>
			/// [艦船グループ]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormShipGroup : ConfigPartBase {

				/// <summary>
				/// 自動更新するか
				/// </summary>
				public bool AutoUpdate { get; set; }

				/// <summary>
				/// ステータスバーを表示するか
				/// </summary>
				public bool ShowStatusBar { get; set; }


				/// <summary>
				/// 艦名列のソート方法
				/// 0 = 図鑑番号順, 1 = あいうえお順
				/// </summary>
				public int ShipNameSortMethod { get; set; }

				public ConfigFormShipGroup() {
					AutoUpdate = true;
					ShowStatusBar = true;
					ShipNameSortMethod = 0;
				}
			}
			/// <summary>[艦船グループ]ウィンドウ</summary>
			[DataMember]
			public ConfigFormShipGroup FormShipGroup { get; private set; }


			/// <summary>
			/// [ブラウザ]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormBrowser : ConfigPartBase {

				/// <summary>
				/// ブラウザの拡大率 10-1000(%)
				/// </summary>
				public int ZoomRate { get; set; }

				/// <summary>
				/// ブラウザをウィンドウサイズに合わせる
				/// </summary>
				[DataMember]
				public bool ZoomFit { get; set; }

				/// <summary>
				/// ログインページのURL
				/// </summary>
				public string LogInPageURL { get; set; }

				/// <summary>
				/// ブラウザを有効にするか
				/// </summary>
				public bool IsEnabled { get; set; }

				/// <summary>
				/// スクリーンショットの保存先フォルダ
				/// </summary>
				public string ScreenShotPath { get; set; }

				/// <summary>
				/// スクリーンショットのフォーマット
				/// 1=jpeg, 2=png
				/// </summary>
				public int ScreenShotFormat { get; set; }

				/// <summary>
				/// 適用するスタイルシート
				/// </summary>
				public string StyleSheet { get; set; }

				/// <summary>
				/// スクロール可能かどうか
				/// </summary>
				public bool IsScrollable { get; set; }

				/// <summary>
				/// スタイルシートを適用するか
				/// </summary>
				public bool AppliesStyleSheet { get; set; }

				/// <summary>
				/// ツールメニューの配置
				/// </summary>
				public DockStyle ToolMenuDockStyle { get; set; }

				/// <summary>
				/// ツールメニューの可視性
				/// </summary>
				public bool IsToolMenuVisible { get; set; }

				/// <summary>
				/// 再読み込み時に確認ダイアログを入れるか
				/// </summary>
				public bool ConfirmAtRefresh { get; set; }

				/// <summary>
				/// flashのパラメータ指定 'wmode'
				/// </summary>
				public string FlashWMode { get; set; }

				/// <summary>
				/// flashのパラメータ指定 'quality'
				/// </summary>
				public string FlashQuality { get; set; }


				public ConfigFormBrowser() {
					ZoomRate = 100;
					ZoomFit = false;
					LogInPageURL = @"http://www.dmm.com/netgame_s/kancolle/";
					IsEnabled = false;
					ScreenShotPath = "ScreenShot";
					ScreenShotFormat = 2;
					StyleSheet = "\r\nbody {\r\n	margin:0;\r\n	overflow:hidden\r\n}\r\n\r\n#game_frame {\r\n	position:fixed;\r\n	left:50%;\r\n	top:-16px;\r\n	margin-left:-450px;\r\n	z-index:1\r\n}\r\n";
					IsScrollable = false;
					AppliesStyleSheet = true;
					ToolMenuDockStyle = DockStyle.Top;
					IsToolMenuVisible = true;
					ConfirmAtRefresh = true;
					FlashWMode = "opaque";
					FlashQuality = "high";
				}
			}
			/// <summary>[ブラウザ]ウィンドウ</summary>
			[DataMember]
			public ConfigFormBrowser FormBrowser { get; private set; }


			/// <summary>
			/// [羅針盤]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormCompass : ConfigPartBase {

				/// <summary>
				/// 一度に表示する敵艦隊候補数
				/// </summary>
				public int CandidateDisplayCount { get; set; }

				public ConfigFormCompass() {
					CandidateDisplayCount = 4;
				}
			}
			/// <summary>[羅針盤]ウィンドウ</summary>
			[DataMember]
			public ConfigFormCompass FormCompass { get; private set; }


			/// <summary>
			/// [JSON]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormJson : ConfigPartBase {

				/// <summary>
				/// 自動更新するか
				/// </summary>
				public bool AutoUpdate { get; set; }

				/// <summary>
				/// TreeView を更新するか
				/// </summary>
				public bool UpdatesTree { get; set; }

				/// <summary>
				/// 自動更新時のフィルタ
				/// </summary>
				public string AutoUpdateFilter { get; set; }


				public ConfigFormJson() {
					AutoUpdate = false;
					UpdatesTree = true;
					AutoUpdateFilter = "";
				}
			}
			/// <summary>[JSON]ウィンドウ</summary>
			[DataMember]
			public ConfigFormJson FormJson { get; private set; }


			/// <summary>
			/// [戦闘]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormBattle : ConfigPartBase {

				/// <summary>
				/// スクロール可能か
				/// </summary>
				public bool IsScrollable { get; set; }

				public bool HideDuringBattle { get; set; }

				public ConfigFormBattle() {
					IsScrollable = false;
					HideDuringBattle = false;
				}
			}

			/// <summary>
			/// [戦闘]ウィンドウ
			/// </summary>
			[DataMember]
			public ConfigFormBattle FormBattle { get; private set; }


			/// <summary>
			/// 各[通知]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigNotifierBase : ConfigPartBase {

				public bool IsEnabled { get; set; }

				public bool IsSilenced { get; set; }

				public bool ShowsDialog { get; set; }

				public string ImagePath { get; set; }

				public bool DrawsImage { get; set; }

				public string SoundPath { get; set; }

				public bool PlaysSound { get; set; }

				public int SoundVolume { get; set; }

				public bool LoopsSound { get; set; }

				public bool DrawsMessage { get; set; }

				public int ClosingInterval { get; set; }

				public int AccelInterval { get; set; }

				public bool CloseOnMouseMove { get; set; }

				public Notifier.NotifierDialogClickFlags ClickFlag { get; set; }

				public Notifier.NotifierDialogAlignment Alignment { get; set; }

				public Point Location { get; set; }

				public bool HasFormBorder { get; set; }

				public bool TopMost { get; set; }

				public bool ShowWithActivation { get; set; }

				public SerializableColor ForeColor { get; set; }

				public SerializableColor BackColor { get; set; }


				public ConfigNotifierBase() {
					IsEnabled = true;
					IsSilenced = false;
					ShowsDialog = true;
					ImagePath = "";
					DrawsImage = false;
					SoundPath = "";
					PlaysSound = false;
					SoundVolume = 100;
					LoopsSound = false;
					DrawsMessage = true;
					ClosingInterval = 10000;
					AccelInterval = 0;
					CloseOnMouseMove = false;
					ClickFlag = Notifier.NotifierDialogClickFlags.Left;
					Alignment = Notifier.NotifierDialogAlignment.BottomRight;
					Location = new Point( 0, 0 );
					HasFormBorder = true;
					TopMost = true;
					ShowWithActivation = true;
					ForeColor = SystemColors.ControlText;
					BackColor = SystemColors.Control;
				}

			}


			/// <summary>
			/// [大破進撃通知]の設定を扱います。
			/// </summary>
			public class ConfigNotifierDamage : ConfigNotifierBase {

				public bool NotifiesBefore { get; set; }
				public bool NotifiesNow { get; set; }
				public bool NotifiesAfter { get; set; }
				public int LevelBorder { get; set; }
				public bool ContainsNotLockedShip { get; set; }
				public bool ContainsSafeShip { get; set; }
				public bool ContainsFlagship { get; set; }
				public bool NotifiesAtEndpoint { get; set; }
				public ConfigNotifierDamage()
					: base() {
					NotifiesBefore = false;
					NotifiesNow = true;
					NotifiesAfter = true;
					LevelBorder = 1;
					ContainsNotLockedShip = true;
					ContainsSafeShip = true;
					ContainsFlagship = true;
					NotifiesAtEndpoint = false;
				}
			}


			/// <summary>
			/// [泊地修理通知]の設定を扱います。
			/// </summary>
			public class ConfigNotifierAnchorageRepair : ConfigNotifierBase {

				public int NotificationLevel { get; set; }

				public ConfigNotifierAnchorageRepair()
					: base() {
					NotificationLevel = 2;
				}
			}


			/// <summary>[遠征帰投通知]</summary>
			[DataMember]
			public ConfigNotifierBase NotifierExpedition { get; private set; }

			/// <summary>[建造完了通知]</summary>
			[DataMember]
			public ConfigNotifierBase NotifierConstruction { get; private set; }

			/// <summary>[入渠完了通知]</summary>
			[DataMember]
			public ConfigNotifierBase NotifierRepair { get; private set; }

			/// <summary>[疲労回復通知]</summary>
			[DataMember]
			public ConfigNotifierBase NotifierCondition { get; private set; }

			/// <summary>[大破進撃通知]</summary>
			[DataMember]
			public ConfigNotifierDamage NotifierDamage { get; private set; }

			/// <summary>[泊地修理通知]</summary>
			[DataMember]
			public ConfigNotifierAnchorageRepair NotifierAnchorageRepair { get; private set; }



			/// <summary>
			/// SyncBGMPlayer の設定を扱います。
			/// </summary>
			public class ConfigBGMPlayer : ConfigPartBase {

				public bool Enabled { get; set; }
				public List<SyncBGMPlayer.SoundHandle> Handles { get; set; }
				public bool SyncBrowserMute { get; set; }

				public ConfigBGMPlayer()
					: base() {
					// 初期値定義は SyncBGMPlayer 内でも
					Enabled = false;
					Handles = new List<SyncBGMPlayer.SoundHandle>();
					foreach ( SyncBGMPlayer.SoundHandleID id in Enum.GetValues( typeof( SyncBGMPlayer.SoundHandleID ) ) )
						Handles.Add( new SyncBGMPlayer.SoundHandle( id ) );
					SyncBrowserMute = false;
				}
			}
			[DataMember]
			public ConfigBGMPlayer BGMPlayer { get; private set; }


			public class ConfigWhitecap : ConfigPartBase {

				public bool ShowInTaskbar { get; set; }
				public bool TopMost { get; set; }
				public int BoardWidth { get; set; }
				public int BoardHeight { get; set; }
				public int ZoomRate { get; set; }
				public int UpdateInterval { get; set; }
				public int ColorTheme { get; set; }
				public int BirthRule { get; set; }
				public int AliveRule { get; set; }

				public ConfigWhitecap()
					: base() {
					ShowInTaskbar = true;
					TopMost = false;
					BoardWidth = 200;
					BoardHeight = 150;
					ZoomRate = 2;
					UpdateInterval = 100;
					ColorTheme = 0;
					BirthRule = ( 1 << 3 );
					AliveRule = ( 1 << 2 ) | ( 1 << 3 );
				}
			}
			[DataMember]
			public ConfigWhitecap Whitecap { get; private set; }



			[DataMember]
			public string Version {
				get { return SoftwareInformation.VersionEnglish; }
				set { }	//readonly
			}


			[DataMember]
			public string VersionUpdateTime { get; set; }


			public override void Initialize() {

				Connection = new ConfigConnection();
				UI = new ConfigUI();
				Log = new ConfigLog();
				Control = new ConfigControl();
				Debug = new ConfigDebug();
				Life = new ConfigLife();

				FormArsenal = new ConfigFormArsenal();
				FormDock = new ConfigFormDock();
				FormFleet = new ConfigFormFleet();
				FormHeadquarters = new ConfigFormHeadquarters();
				FormQuest = new ConfigFormQuest();
				FormShipGroup = new ConfigFormShipGroup();
				FormBattle = new ConfigFormBattle();
				FormBrowser = new ConfigFormBrowser();
				FormCompass = new ConfigFormCompass();
				FormJson = new ConfigFormJson();

				NotifierExpedition = new ConfigNotifierBase();
				NotifierConstruction = new ConfigNotifierBase();
				NotifierRepair = new ConfigNotifierBase();
				NotifierCondition = new ConfigNotifierBase();
				NotifierDamage = new ConfigNotifierDamage();
				NotifierAnchorageRepair = new ConfigNotifierAnchorageRepair();

				BGMPlayer = new ConfigBGMPlayer();
				Whitecap = new ConfigWhitecap();

				VersionUpdateTime = DateTimeHelper.TimeToCSVString( SoftwareInformation.UpdateTime );

			}
		}
		private static ConfigurationData _config;

		public static ConfigurationData Config {
			get { return _config; }
		}



		private Configuration()
			: base() {

			_config = new ConfigurationData();
		}


		internal void OnConfigurationChanged() {
			ConfigurationChanged();
		}


		public void Load( Form mainForm ) {
			var temp = (ConfigurationData)_config.Load( SaveFileName );
			if ( temp != null ) {
				_config = temp;
				CheckUpdate( mainForm );
				OnConfigurationChanged();
			} else {
				MessageBox.Show( "欢迎使用" + SoftwareInformation.SoftwareNameJapanese + " 。\r\n\r\n设置和使用方法请参考 [帮助] - [在线帮助]\r\n使用前敬请阅读。",
					"初次启动信息", MessageBoxButtons.OK, MessageBoxIcon.Information );
				MessageBox.Show( "※ 本汉化版启动时浏览器不会自动加载页面 ※\r\n\r\n如有需要请在确保代理设置准确无误后手动通\r\n过 [文件] - [设置] - [子窗口] - [浏览器] 勾选\r\n[启动时自动加载页面] 开启此功能。",
					"重要提示", MessageBoxButtons.OK, MessageBoxIcon.Information );

				// そのままだと正常に動作しなくなった(らしい)ので、ブラウザバージョンの書き込み
				Microsoft.Win32.RegistryKey reg = null;
				try {

					reg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey( DialogConfiguration.RegistryPathMaster + DialogConfiguration.RegistryPathBrowserVersion );
					reg.SetValue( Window.FormBrowserHost.BrowserExeName, DialogConfiguration.DefaultBrowserVersion, Microsoft.Win32.RegistryValueKind.DWord );
					reg.Close();

					reg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey( DialogConfiguration.RegistryPathMaster + DialogConfiguration.RegistryPathGPURendering );
					reg.SetValue( Window.FormBrowserHost.BrowserExeName, DialogConfiguration.DefaultGPURendering ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord );

					Utility.Logger.Add(2, "", "已将浏览器版本写入注册表。想要清除的话点击 [设置]-[子窗口]-[浏览器2] 里的 [清除] 按钮。");


				} catch ( Exception ex ) {
					Utility.ErrorReporter.SendErrorReport( ex, "ブラウザバージョンをレジストリに書き込めませんでした。" );

				} finally {
					if ( reg != null )
						reg.Close();
				}

			}

			// 读取配色主题 ( 默认值待编辑 )
			dynamic json = DynamicJson.Parse(@"[{""name"":""VS2012Light""}]");
			//if (File.Exists(@"Settings\ColorScheme.json")) {
			try {
				string s = String.Empty;
				StringBuilder sb = new StringBuilder();
				// 读取配色文件
				using (StreamReader sr = File.OpenText(@"Settings\ColorScheme.json")) {
					while ((s = sr.ReadLine()) != null) {
						// 干掉注释，因为 DynamicJson 不支持注释
						s = Regex.Replace(s, @"\/\/.*?$", string.Empty);
						if (!String.IsNullOrWhiteSpace(s)) sb.Append(s);
					}
				}
				json = DynamicJson.Parse(sb.ToString());
			} catch (FileNotFoundException) {
				Logger.Add(2, "", @"Settings\ColorScheme.json 不存在。");
			} catch {
				Logger.Add(2, "", @"解析 Settings\ColorScheme.json 失败。");
			}
			int themeId = Config.UI.ThemeID;
			if (!json.IsDefined(themeId)) {
				themeId = Config.UI.ThemeID = 0;
				Logger.Add(2, "", "指定的 ThemeID 不存在。");
			}
			ThemeStyle = json[themeId];
			Logger.Add(2, "", "载入配色主题 : " + ThemeStyle["name"]);
			// 定义基本颜色
			Config.UI.Color_Red     = ThemeColor("basicColors", "red");
			Config.UI.Color_Orange  = ThemeColor("basicColors", "orange");
			Config.UI.Color_Yellow  = ThemeColor("basicColors", "yellow");
			Config.UI.Color_Green   = ThemeColor("basicColors", "green");
			Config.UI.Color_Cyan    = ThemeColor("basicColors", "cyan");
			Config.UI.Color_Blue    = ThemeColor("basicColors", "blue");
			Config.UI.Color_Magenta = ThemeColor("basicColors", "magenta");
			Config.UI.Color_Violet  = ThemeColor("basicColors", "violet");
			// 定义面板颜色
			Config.UI.ForeColor = ThemeColor("panelColors", "foreground");
			Config.UI.BackColor = ThemeColor("panelColors", "background");
			Config.UI.SubForeColor = ThemeColor("panelColors", "foreground2");
			Config.UI.SubBackColor = ThemeColor("panelColors", "background2");
			Config.UI.SubBackColorPen = new Pen(Config.UI.SubBackColor);
			// 状态栏颜色
			Config.UI.StatusBarForeColor = ThemeColor("panelColors", "statusBarFG");
			Config.UI.StatusBarBackColor = ThemeColor("panelColors", "statusBarBG");
			// 定义 UI (DockPanelSuite) 颜色
			Config.UI.DockPanelSuiteStyles = new string[] {
				ThemeColorHex("panelColors", "foreground"),
				ThemeColorHex("panelColors", "background"),
				ThemeColorHex("panelColors", "background2"),
				ThemeColorHex("panelColors", "tabActiveFG"),
				ThemeColorHex("panelColors", "tabActiveBG"),
				ThemeColorHex("panelColors", "tabLostFocusFG"),
				ThemeColorHex("panelColors", "tabLostFocusBG"),
				ThemeColorHex("panelColors", "tabHoverFG"),
				ThemeColorHex("panelColors", "tabHoverBG")
			};
			// 定义数值条颜色
			Config.UI.BarColorSchemes = new List<SerializableColor>[] {
				new List<SerializableColor>() {
					ThemeBarColor(0, 0),
					ThemeBarColor(0, 1),
					ThemeBarColor(0, 2),
					ThemeBarColor(0, 3),
					ThemeBarColor(0, 4),
					ThemeBarColor(0, 5),
					ThemeBarColor(0, 6),
					ThemeBarColor(0, 7),
					ThemeBarColor(0, 8),
					ThemeBarColor(0, 9),
					ThemeBarColor(0, 10),
					ThemeBarColor(0, 11)
				},
				new List<SerializableColor>() {
					ThemeBarColor(1, 0),
					ThemeBarColor(1, 1),
					ThemeBarColor(1, 2),
					ThemeBarColor(1, 3),
					ThemeBarColor(1, 4),
					ThemeBarColor(1, 5),
					ThemeBarColor(1, 6),
					ThemeBarColor(1, 7),
					ThemeBarColor(1, 8),
					ThemeBarColor(1, 9),
					ThemeBarColor(1, 10),
					ThemeBarColor(1, 11)
				}
			};
			Config.UI.SetBarColorScheme();
			// 设定各面板颜色
			Config.UI.Fleet_ColorConditionVeryTired   = ThemePanelColor("fleet", "conditionVeryTired");
			Config.UI.Fleet_ColorConditionTired       = ThemePanelColor("fleet", "conditionTired");
			Config.UI.Fleet_ColorConditionLittleTired = ThemePanelColor("fleet", "conditionLittleTired");
			Config.UI.Fleet_ColorConditionSparkle     = ThemePanelColor("fleet", "conditionSparkle");
			Config.UI.Fleet_equipmentLevelColor = ThemePanelColor("fleet", "equipmentLevel");
			Config.UI.Quest_Type1Color = ThemePanelColor("quest", "typeHensei");
			Config.UI.Quest_Type2Color = ThemePanelColor("quest", "typeShutsugeki");
			Config.UI.Quest_Type3Color = ThemePanelColor("quest", "typeEnshu");
			Config.UI.Quest_Type4Color = ThemePanelColor("quest", "typeEnsei");
			Config.UI.Quest_Type5Color = ThemePanelColor("quest", "typeHokyu");
			Config.UI.Quest_Type6Color = ThemePanelColor("quest", "typeKojo");
			Config.UI.Quest_Type7Color = ThemePanelColor("quest", "typeKaiso");
			Config.UI.Quest_ColorProcessLT50 = ThemePanelColor("quest", "processLT50");
			Config.UI.Quest_ColorProcessLT80 = ThemePanelColor("quest", "processLT80");
			Config.UI.Quest_ColorProcessLT100 = ThemePanelColor("quest", "processLT100");
			Config.UI.Quest_ColorProcessDefault = ThemePanelColor("quest", "processDefault");
			Config.UI.Compass_ShipNameColor2 = ThemePanelColor("compass", "shipClass2");
			Config.UI.Compass_ShipNameColor3 = ThemePanelColor("compass", "shipClass3");
			Config.UI.Compass_ShipNameColor4 = ThemePanelColor("compass", "shipClass4");
			Config.UI.Compass_ShipNameColor5 = ThemePanelColor("compass", "shipClass5");
			Config.UI.Compass_ShipNameColor6 = ThemePanelColor("compass", "shipClass6");
			Config.UI.Compass_ColorTextEventKind3 = ThemePanelColor("compass", "eventKind3");
			Config.UI.Compass_ColorTextEventKind6 = ThemePanelColor("compass", "eventKind6");
			Config.UI.Compass_ColorTextEventKind5 = ThemePanelColor("compass", "eventKind5");
			Config.UI.Compass_ColoroverlayBrush = ThemePanelColor("compass", "overlayBrush");
			Config.UI.Battle_ColorHPBarsBossDamaged = ThemePanelColor("battle", "barBossDamaged");
			Config.UI.Battle_ColorHPBarsMVP = ThemePanelColor("battle", "barMVP");
			Config.UI.Battle_ColorHPBarsEscaped = ThemePanelColor("battle", "barBossDamaged");
		}

		private dynamic ThemeStyle;

		private Color ThemeColor(string type, string name) {
			if (ThemeStyle.IsDefined(type) && ThemeStyle[type].IsDefined(name)) {
				return ColorTranslator.FromHtml(ThemeStyle[type][name]);
			} else {
				switch (type + "_" + name) {
					case "basicColors_red":
						return Color.Red;
					case "basicColors_orange":
						return Color.Orange;
					case "basicColors_yellow":
						return Color.Yellow;
					case "basicColors_green":
						return Color.Green;
					case "basicColors.cyan":
						return Color.Cyan;
					case "basicColors.blue":
						return Color.Blue;
					case "basicColors.magenta":
						return Color.Magenta;
					case "basicColors.violet":
						return Color.Violet;
					case "panelColors_foreground":
						return SystemColors.ControlText;
					case "panelColors_background":
						return SystemColors.Control;
					case "panelColors_foreground2":
						return SystemColors.ControlText;
					case "panelColors_background2":
						return SystemColors.ControlLight;
					case "panelColors_statusBarFG":
						return Config.UI.SubForeColor;
					case "panelColors_statusBarBG":
						return Config.UI.SubBackColor;
					default:
						return Color.Magenta;
				}
			}
		}

		private Color ThemePanelColor(string form, string name) {
			if (ThemeStyle.IsDefined("panelColors") && ThemeStyle["panelColors"].IsDefined(form) && ThemeStyle["panelColors"][form].IsDefined(name)) {
				return ColorTranslator.FromHtml(ThemeStyle["panelColors"][form][name]);
			} else {
				switch (form + "_" + name) {
					// 视图 - 舰队
					case "fleet_conditionVeryTired":
						return Config.UI.Color_Red;
					case "fleet_conditionTired":
						return Config.UI.Color_Orange;
					case "fleet_conditionLittleTired":
						return Config.UI.Color_Yellow;
					case "fleet_conditionSparkle":
						return Config.UI.Color_Blue;
					case "fleet_equipmentLevel":
						return Config.UI.Color_Cyan;
					// 视图 - 司令部
					// 视图 - 任务
					case "quest_typeHensei":
						return Config.UI.Color_Green;
					case "quest_typeShutsugeki":
						return Config.UI.Color_Red;
					case "quest_typeEnshu":
						return Config.UI.Color_Green;
					case "quest_typeEnsei":
						return Config.UI.Color_Cyan;
					case "quest_typeHokyu":
						return Config.UI.Color_Yellow;
					case "quest_typeKojo":
						return Config.UI.Color_Orange;
					case "quest_typeKaiso":
						return Config.UI.Color_Violet;
					case "quest_processLT50":
						return Config.UI.Color_Orange;
					case "quest_processLT80":
						return Config.UI.Color_Green;
					case "quest_processLT100":
						return Config.UI.Color_Cyan;
					case "quest_processDefault":
						return Config.UI.Color_Blue;
					// 视图 - 罗盘
					case "compass_shipClass2":
						return Config.UI.Color_Red;
					case "compass_shipClass3":
						return Config.UI.Color_Orange;
					case "compass_shipClass4":
						return Config.UI.Color_Blue;
					case "compass_shipClass5":
						return Config.UI.Color_Magenta;
					case "compass_shipClass6":
						return Config.UI.Color_Yellow;
					case "compass_eventKind3":
						return Config.UI.Color_Violet;
					case "compass_eventKind6":
						return Config.UI.Color_Green;
					case "compass_eventKind5":
						return Config.UI.Color_Red;
					case "compass_overlayBrush": // %75 透明度背景色
						return Color.FromArgb(0xC0, Config.UI.BackColor);
					// 视图 - 战斗
					case "battle_barBossDamaged":
						return Config.UI.Color_Orange;
					case "battle_barMVP":
						return Config.UI.Color_Blue;
					case "battle_barEscaped":
						return Config.UI.SubBackColor;
					default:
						return Color.Magenta;
				}
			}
		}

		private String ThemeColorHex(string type, string name) {
			if (ThemeStyle.IsDefined(type) && ThemeStyle[type].IsDefined(name)) {
				return ThemeStyle[type][name];
			} else {
				switch (type + "_" + name) {
					case "panelColors_tabActiveFG":
						return ThemeColorHex("panelColors", "foreground2");
					case "panelColors_tabActiveBG":
						return ThemeColorHex("panelColors", "background2");
					case "panelColors_tabLostFocusFG":
						return ThemeColorHex("panelColors", "foreground2");
					case "panelColors_tabLostFocusBG":
						return ThemeColorHex("panelColors", "background2");
					case "panelColors_tabHoverFG":
						return ThemeColorHex("panelColors", "foreground2");
					case "panelColors_tabHoverBG":
						return ThemeColorHex("panelColors", "background2");
					default:
						var c = ThemeColor(type, name);
						return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
				}
			}
		}

		private Color ThemeBarColor(int type, int index) {
			if (ThemeStyle.IsDefined("barColors") && ThemeStyle["barColors"].IsDefined(type) && ThemeStyle["barColors"][type].IsDefined(11)) {
				return ColorTranslator.FromHtml(ThemeStyle["barColors"][type][index]);
			} else {
				switch (type + "_" + index) {
					case "0_0":
						return ColorTranslator.FromHtml("#FF0000");
					case "0_1":
						return ColorTranslator.FromHtml("#FF0000");
					case "0_2":
						return ColorTranslator.FromHtml("#FF8800");
					case "0_3":
						return ColorTranslator.FromHtml("#FF8800");
					case "0_4":
						return ColorTranslator.FromHtml("#FFCC00");
					case "0_5":
						return ColorTranslator.FromHtml("#FFCC00");
					case "0_6":
						return ColorTranslator.FromHtml("#00CC00");
					case "0_7":
						return ColorTranslator.FromHtml("#00CC00");
					case "0_8":
						return ColorTranslator.FromHtml("#0044CC");
					case "0_9":
						return ColorTranslator.FromHtml("#44FF00");
					case "0_10":
						return ColorTranslator.FromHtml("#882222");
					case "0_11":
						return ColorTranslator.FromHtml("#888888");
					case "1_0":
						return ColorTranslator.FromHtml("#FF0000");
					case "1_1":
						return ColorTranslator.FromHtml("#FF0000");
					case "1_2":
						return ColorTranslator.FromHtml("#FF4400");
					case "1_3":
						return ColorTranslator.FromHtml("#FF8800");
					case "1_4":
						return ColorTranslator.FromHtml("#FFAA00");
					case "1_5":
						return ColorTranslator.FromHtml("#EEEE00");
					case "1_6":
						return ColorTranslator.FromHtml("#CCEE00");
					case "1_7":
						return ColorTranslator.FromHtml("#00CC00");
					case "1_8":
						return ColorTranslator.FromHtml("#0044CC");
					case "1_9":
						return ColorTranslator.FromHtml("#00FF44");
					case "1_10":
						return ColorTranslator.FromHtml("#882222");
					case "1_11":
						return ColorTranslator.FromHtml("#888888");
					default:
						return Color.Magenta;
				}
			}
		}

		public void Save() {
			_config.Save( SaveFileName );
		}



		private void CheckUpdate( Form mainForm ) {
			DateTime dt = Config.VersionUpdateTime == null ? new DateTime( 0 ) : DateTimeHelper.CSVStringToTime( Config.VersionUpdateTime );

			// version 1.4.6 or earlier
			if ( dt <= DateTimeHelper.CSVStringToTime( "2015/08/27 21:00:00" ) ) {

				if ( MessageBox.Show(
					"バージョンアップが検出されました。\r\n古いレコードファイルを新しいフォーマットにコンバートします。\r\n(元のファイルは Record_Backup フォルダに残されます。)\r\nよろしいですか？\r\n(コンバートせずに続行した場合、読み込めなくなる可能性があります。)\r\n",
					"バージョンアップに伴う確認(～1.4.6)",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 )
					 == DialogResult.Yes ) {

					try {

						Directory.CreateDirectory( "Record_Backup" );

						if ( File.Exists( RecordManager.Instance.MasterPath + "\\EnemyFleetRecord.csv" ) ) {
							File.Copy( RecordManager.Instance.MasterPath + "\\EnemyFleetRecord.csv", "Record_Backup\\EnemyFleetRecord.csv", false );

							//ヒャッハー！！
							using ( var writer = new StreamWriter( RecordManager.Instance.MasterPath + "\\EnemyFleetRecord.csv", false, Config.Log.FileEncoding ) ) {
								writer.WriteLine();
							}
						}


						if ( File.Exists( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv" ) ) {
							File.Copy( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv", "Record_Backup\\ShipDropRecord.csv", false );

							using ( var reader = new StreamReader( "Record_Backup\\ShipDropRecord.csv", Config.Log.FileEncoding ) ) {
								using ( var writer = new StreamWriter( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv", false, Config.Log.FileEncoding ) ) {

									while ( !reader.EndOfStream ) {
										string line = reader.ReadLine();
										var elem = line.Split( ",".ToCharArray() ).ToList();

										elem.Insert( 6, Constants.GetDifficulty( -1 ) );	//difficulty
										elem[8] = "0";		//EnemyFleetID


										writer.WriteLine( string.Join( ",", elem ) );
									}
								}
							}
						}


						if ( File.Exists( RecordManager.Instance.MasterPath + "\\ShipParameterRecord.csv" ) ) {
							File.Copy( RecordManager.Instance.MasterPath + "\\ShipParameterRecord.csv", "Record_Backup\\ShipParameterRecord.csv", false );

							using ( var reader = new StreamReader( "Record_Backup\\ShipParameterRecord.csv", Config.Log.FileEncoding ) ) {
								using ( var writer = new StreamWriter( RecordManager.Instance.MasterPath + "\\ShipParameterRecord.csv", false, Config.Log.FileEncoding ) ) {

									while ( !reader.EndOfStream ) {
										string line = reader.ReadLine();
										var elem = line.Split( ",".ToCharArray() ).ToList();

										elem.InsertRange( 2, Enumerable.Repeat( "0", 10 ) );
										elem.InsertRange( 21, Enumerable.Repeat( "0", 3 ) );
										elem.InsertRange( 29, Enumerable.Repeat( "null", 5 ) );
										elem.Insert( 34, "null" );

										writer.WriteLine( string.Join( ",", elem ) );
									}
								}
							}
						}



						if ( File.Exists( RecordManager.Instance.MasterPath + "\\ConstructionRecord.csv" ) ) {
							File.Copy( RecordManager.Instance.MasterPath + "\\ConstructionRecord.csv", "Record_Backup\\ConstructionRecord.csv", false );

							using ( var reader = new StreamReader( "Record_Backup\\ConstructionRecord.csv", Config.Log.FileEncoding ) ) {
								using ( var writer = new StreamWriter( RecordManager.Instance.MasterPath + "\\ConstructionRecord.csv", false, Config.Log.FileEncoding ) ) {

									string[] prev = null;

									while ( !reader.EndOfStream ) {
										string line = reader.ReadLine();
										var elem = line.Split( ",".ToCharArray() );

										// 以前のバージョンのバグによる無効行・重複行の削除
										if ( prev != null ) {
											if ( elem[0] == "0" || (	//invalid id
												elem[0] == prev[0] &&	//id
												elem[1] == prev[1] &&	//name
												elem[3] == prev[3] &&	//fuel
												elem[4] == prev[4] &&	//ammo
												elem[5] == prev[5] &&	//steel
												elem[6] == prev[6] &&	//bauxite
												elem[7] == prev[7] &&	//dev.mat
												elem[8] == prev[8] &&	//islarge
												elem[9] == prev[9]		//emptydock
												) ) {

												prev = elem;
												continue;
											}
										}

										writer.WriteLine( string.Join( ",", elem ) );
										prev = elem;
									}
								}
							}
						}


						// 読み書き方式が変わったので念のため
						if ( File.Exists( RecordManager.Instance.MasterPath + "\\DevelopmentRecord.csv" ) ) {
							File.Copy( RecordManager.Instance.MasterPath + "\\DevelopmentRecord.csv", "Record_Backup\\DevelopmentRecord.csv", false );
						}


					} catch ( Exception ex ) {

						Utility.ErrorReporter.SendErrorReport( ex, "バージョンアップに伴うレコードのコンバートに失敗しました。" );

						if ( MessageBox.Show( "コンバートに失敗しました。\r\n" + ex.Message + "\r\n起動処理を続行しますか？\r\n(データが破壊される可能性があります)\r\n",
							"エラー", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2 )
							== DialogResult.No )
							Environment.Exit( -1 );

					}
				}


			}

			// version 1.5.0 or earlier
			if ( dt <= DateTimeHelper.CSVStringToTime( "2015/09/04 21:00:00" ) ) {

				if ( MessageBox.Show(
					"バージョンアップが検出されました。\r\n艦船グループデータの互換性がなくなったため、当該データを初期化します。\r\n(古いファイルは Settings_Backup フォルダに退避されます。)\r\nよろしいですか？\r\n(初期化せずに続行した場合、エラーが発生します。)\r\n",
					"バージョンアップに伴う確認(～1.5.0)",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 )
					 == DialogResult.Yes ) {

					try {

						Directory.CreateDirectory( "Settings_Backup" );
						File.Move( "Settings\\ShipGroups.xml", "Settings_Backup\\ShipGroups.xml" );

					} catch ( Exception ex ) {

						Utility.ErrorReporter.SendErrorReport( ex, "バージョンアップに伴うグループデータの削除に失敗しました。" );

						// エラーが出るだけなのでシャットダウンは不要
						MessageBox.Show( "削除に失敗しました。\r\n" + ex.Message,
							"エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );

					}
				}
			}


			// version 1.6.3 or earlier
			if ( dt <= DateTimeHelper.CSVStringToTime( "2015/10/03 22:00:00" ) ) {

				if ( MessageBox.Show(
					"バージョンアップが検出されました。\r\nアイテムドロップ仕様の変更に伴い、艦船ドロップレコードのフォーマットを変更します。\r\n(古いファイルは Record_Backup フォルダに退避されます。)\r\nよろしいですか？\r\n(初期化せずに続行した場合、エラーが発生します。)\r\n",
					"バージョンアップに伴う確認(～1.6.3)",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 )
					 == DialogResult.Yes ) {

					try {

						if ( File.Exists( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv" ) ) {

							Directory.CreateDirectory( "Record_Backup" );

							if ( File.Exists( "Record_Backup\\ShipDropRecord.csv" ) ) {
								var result = MessageBox.Show( "バックアップ先に既にファイルが存在します。\r\n上書きしますか？\r\n(キャンセルした場合、コンバート処理を中止します。)",
									"バックアップの上書き確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );

								switch ( result ) {
									case DialogResult.Yes:
										File.Copy( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv", "Record_Backup\\ShipDropRecord.csv", true );
										break;
									case DialogResult.No:
										break;
									case DialogResult.Cancel:
										throw new InvalidOperationException( "バックアップ処理がキャンセルされました。" );
								}
							} else {
								File.Copy( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv", "Record_Backup\\ShipDropRecord.csv", false );
							}


							using ( var reader = new StreamReader( "Record_Backup\\ShipDropRecord.csv", Config.Log.FileEncoding ) ) {
								using ( var writer = new StreamWriter( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv", false, Config.Log.FileEncoding ) ) {

									while ( !reader.EndOfStream ) {
										string line = reader.ReadLine();
										var elem = line.Split( ",".ToCharArray() ).ToList();

										// 旧IDの変換
										int oldID;
										if ( !int.TryParse( elem[0], out oldID ) )
											oldID = -1;

										if ( oldID > 2000 ) {
											elem[0] = "-1";
											elem[1] = "(なし)";
											elem.InsertRange( 2, new string[] { "-1", "(なし)", ( oldID - 2000 ).ToString(), "???" } );

										} else if ( oldID > 1000 ) {
											elem[0] = "-1";
											elem[1] = "(なし)";
											elem.InsertRange( 2, new string[] { ( oldID - 1000 ).ToString(), "???", "-1", "(なし)" } );

										} else {
											elem.InsertRange( 2, new string[] { "-1", "(なし)", "-1", "(なし)" } );

										}


										writer.WriteLine( string.Join( ",", elem ) );
									}
								}
							}
						}


					} catch ( Exception ex ) {

						Utility.ErrorReporter.SendErrorReport( ex, "バージョンアップに伴うレコードのコンバートに失敗しました。" );

						if ( MessageBox.Show( "コンバートに失敗しました。\r\n" + ex.Message + "\r\n起動処理を続行しますか？\r\n(データが破壊される可能性があります)\r\n",
							"エラー", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2 )
							== DialogResult.No )
							Environment.Exit( -1 );

					}
				}
			}




			Config.VersionUpdateTime = DateTimeHelper.TimeToCSVString( SoftwareInformation.UpdateTime );
		}

	}


}
