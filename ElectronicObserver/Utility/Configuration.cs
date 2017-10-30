﻿using Codeplex.Data;
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

				// 修理时间偏移值 ( 暂定 +30s, 待验证 )
				public int DockingUnitTimeOffset { get; set; }

				// 使用旧版熟练度图标
				public bool UseOldAircraftLevelIcons { get; set; }

				// 日志窗口自动换行
				public bool TextWrapInLogWindow { get; set; }

				// 日志窗口精简模式
				public bool CompactModeLogWindow { get; set; }

				// 日志窗口反向滚动 ( 新日志在顶端 )
				public bool InvertedLogWindow { get; set; }


				// 司令部各资源储量过低警告值
				// -1: 自然恢复上限 ( 桶无效 ) | 3000: 小于 3000
				public int HqResLowAlertFuel { get; set; }
				public int HqResLowAlertAmmo { get; set; }
				public int HqResLowAlertSteel { get; set; }
				public int HqResLowAlertBauxite { get; set; }
				public int HqResLowAlertBucket { get; set; }

				/// <summary>
				/// 舰队编成 - 新增排序列 ( Lv 序, 舰种序, NEW 序 )
				/// | true: 开启此功能 ( 默认 ) | false: 禁用此功能 ( 若过分拖慢可关闭 )
				/// </summary>
				public bool AllowSortIndexing { get; set; }

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

				// 视图 - 舰队
				[IgnoreDataMember] // 入渠计时器文字色
				public Color Fleet_ColorRepairTimerText { get; set; }
				[IgnoreDataMember] // 疲劳状态文字色
				public Color Fleet_ColorConditionText { get; set; }
				[IgnoreDataMember] // 严重疲劳
				public Color Fleet_ColorConditionVeryTired { get; set; }
				[IgnoreDataMember] // 中等疲劳
				public Color Fleet_ColorConditionTired { get; set; }
				[IgnoreDataMember] // 轻微疲劳
				public Color Fleet_ColorConditionLittleTired { get; set; }
				[IgnoreDataMember] // 战意高扬
				public Color Fleet_ColorConditionSparkle { get; set; }
				[IgnoreDataMember] // 装备改修值
				public Color Fleet_EquipmentLevelColor { get; set; }

				// 视图 - 舰队一览
				[IgnoreDataMember] // 大破 / 大破进击文字色
				public Color FleetOverview_ShipDamagedFG { get; set; }
				[IgnoreDataMember] // 大破 / 大破进击背景色
				public Color FleetOverview_ShipDamagedBG { get; set; }
				[IgnoreDataMember] // 远征返回文字色
				public Color FleetOverview_ExpeditionOverFG { get; set; }
				[IgnoreDataMember] // 远征返回背景色
				public Color FleetOverview_ExpeditionOverBG { get; set; }
				[IgnoreDataMember] // 疲劳恢复文字色
				public Color FleetOverview_TiredRecoveredFG { get; set; }
				[IgnoreDataMember] // 疲劳恢复背景色
				public Color FleetOverview_TiredRecoveredBG { get; set; }
				[IgnoreDataMember] // 未远征提醒文字色
				public Color FleetOverview_AlertNotInExpeditionFG { get; set; }
				[IgnoreDataMember] // 未远征提醒背景色
				public Color FleetOverview_AlertNotInExpeditionBG { get; set; }

				// 视图 - 入渠
				[IgnoreDataMember] // 修理完成文字色
				public Color Dock_RepairFinishedFG { get; set; }
				[IgnoreDataMember] // 修理完成背景色
				public Color Dock_RepairFinishedBG { get; set; }

				// 视图 - 工厂
				[IgnoreDataMember] // 建造完成文字色
				public Color Arsenal_BuildCompleteFG { get; set; }
				[IgnoreDataMember] // 建造完成背景色
				public Color Arsenal_BuildCompleteBG { get; set; }

				// 视图 - 司令部
				[IgnoreDataMember] // 资源超过自然恢复上限文字色
				public Color Headquarters_ResourceOverFG { get; set; }
				[IgnoreDataMember] // 资源超过自然恢复上限背景色
				public Color Headquarters_ResourceOverBG { get; set; }
				[IgnoreDataMember] // 剩余船位、装备位不满足活动图出击要求时闪烁文字色
				public Color Headquarters_ShipCountOverFG { get; set; }
				[IgnoreDataMember] // 剩余船位、装备位不满足活动图出击要求时闪烁背景色
				public Color Headquarters_ShipCountOverBG { get; set; }
				[IgnoreDataMember] // 资材达到 3,000 个时文字色
				public Color Headquarters_MaterialMaxFG { get; set; }
				[IgnoreDataMember] // 资材达到 3,000 个时背景色
				public Color Headquarters_MaterialMaxBG { get; set; }
				[IgnoreDataMember] // 家具币达到 200,000 个时文字色
				public Color Headquarters_CoinMaxFG { get; set; }
				[IgnoreDataMember] // 家具币达到 200,000 个时背景色
				public Color Headquarters_CoinMaxBG { get; set; }
				[IgnoreDataMember] // 资源储量低于警告值文字色
				public Color Headquarters_ResourceLowFG { get; set; }
				[IgnoreDataMember] // 资源储量低于警告值背景色
				public Color Headquarters_ResourceLowBG { get; set; }
				[IgnoreDataMember] // 资源储量达到 300,000 时文字色
				public Color Headquarters_ResourceMaxFG { get; set; }
				[IgnoreDataMember] // 资源储量达到 300,000 时背景色
				public Color Headquarters_ResourceMaxBG { get; set; }

				// 视图 - 任务
				[IgnoreDataMember] // 任务类型文字色
				public Color Quest_TypeFG { get; set; }
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
				[IgnoreDataMember] // 敌舰名 - 鬼 / 改 flagship / 后期型
				public Color Compass_ShipNameColor4 { get; set; }
				[IgnoreDataMember] // 敌舰名 - 姫 / 后期型 elite
				public Color Compass_ShipNameColor5 { get; set; }
				[IgnoreDataMember] // 敌舰名 - 水鬼 / 后期型 flagship
				public Color Compass_ShipNameColor6 { get; set; }
				[IgnoreDataMember] // 敌舰名 - 水姫
				public Color Compass_ShipNameColor7 { get; set; }
				[IgnoreDataMember] // 敌舰名 - 壊
				public Color Compass_ShipNameColorDestroyed { get; set; }
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
				[IgnoreDataMember] // MVP
				public Color Battle_ColorHPBarsMVP { get; set; }
				[IgnoreDataMember] // MVP 主文字色
				public Color Battle_ColorTextMVP { get; set; }
				[IgnoreDataMember] // MVP 副文字色
				public Color Battle_ColorTextMVP2 { get; set; }
				[IgnoreDataMember] // 已退避
				public Color Battle_ColorHPBarsEscaped { get; set; }
				[IgnoreDataMember] // 已退避主文字色
				public Color Battle_ColorTextEscaped { get; set; }
				[IgnoreDataMember] // 已退避副文字色
				public Color Battle_ColorTextEscaped2 { get; set; }
				[IgnoreDataMember] // 受损状态 BOSS
				public Color Battle_ColorHPBarsBossDamaged { get; set; }
				[IgnoreDataMember] // 受损状态 BOSS 主文字色
				public Color Battle_ColorTextBossDamaged { get; set; }
				[IgnoreDataMember] // 受损状态 BOSS 副文字色
				public Color Battle_ColorTextBossDamaged2 { get; set; }

				public bool RemoveBarShadow { get {
				switch (ThemeID) {
					case 0:  return true;
					case 1:  return true;
					default: return false;
				}}}

				#endregion

				/// <summary>
				/// 固定レイアウト(フォントに依存しないレイアウト)を利用するか
				/// </summary>
				public bool IsLayoutFixed;


				public ConfigUI() {
					MainFont = new Font( "Microsoft YaHei", 12, FontStyle.Regular, GraphicsUnit.Pixel );
					SubFont  = new Font( "Microsoft YaHei", 10, FontStyle.Regular, GraphicsUnit.Pixel );
					JapFont  = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
					JapFont2 = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
					ThemeID = 0;
					ShowGrowthInsteadOfNextInHQ = false;
					MaxAkashiPerHP = 5;
					DockingUnitTimeOffset = 30;
					UseOldAircraftLevelIcons = true;
					TextWrapInLogWindow = false;
					CompactModeLogWindow = false;
					InvertedLogWindow = false;
					HqResLowAlertFuel    = 0;
					HqResLowAlertAmmo    = 0;
					HqResLowAlertSteel   = 0;
					HqResLowAlertBauxite = 0;
					HqResLowAlertBucket  = 0;
					AllowSortIndexing = true;
					BarColorMorphing = false;
					IsLayoutFixed = true;
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

				/// <summary>
				/// ログを即時保存するか
				/// </summary>
				public bool SaveLogImmediately { get; set; }


				public ConfigLog() {
					LogLevel = 2;
					SaveLogFlag = true;
					SaveErrorReport = true;
					FileEncodingID = 1;
					ShowSpoiler = true;
					PlayTime = 0;
					PlayTimeIgnoreInterval = 10 * 60;
					SaveBattleLog = false;
					SaveLogImmediately = false;
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
				/// 0=しない、1=1時間ごと、2=1日ごと, 3=即時
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

				/// <summary>
				/// 出撃札がない艦娘が出撃したときに警告ダイアログを表示するか
				/// </summary>
				public bool ShowSallyAreaAlertDialog { get; set; }

				public ConfigControl() {
					ConditionBorder = 40;
					RecordAutoSaving = 1;
					UseSystemVolume = true;
					LastVolume = 0.8f;
					LastIsMute = false;
					PowerEngagementForm = 1;
					ShowSallyAreaAlertDialog = true;
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

				/// <summary>
				/// 艦名表示の最大幅
				/// </summary>
				public int MaxShipNameWidth { get; set; }

				public ConfigFormArsenal() {
					ShowShipName = true;
					BlinkAtCompletion = true;
					MaxShipNameWidth = 60;
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

				/// <summary>
				/// 艦名表示の最大幅
				/// </summary>
				public int MaxShipNameWidth { get; set; }

				public ConfigFormDock() {
					BlinkAtCompletion = true;
					MaxShipNameWidth = 64;
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
				/// 艦載機熟練度を数字で表示するフラグ
				/// </summary>
				public bool ShowAircraftLevelByNumber { get; set; }

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

				/// <summary>
				/// 制空戦力を範囲表示するか
				/// </summary>
				public bool ShowAirSuperiorityRange { get; set; }

				/// <summary>
				/// 泊地修理によるHP回復を表示に反映するか
				/// </summary>
				public bool ReflectAnchorageRepairHealing { get; set; }

				/// <summary>
				/// 遠征艦隊が母港にいるとき強調表示
				/// </summary>
				public bool EmphasizesSubFleetInPort { get; set; }

				/// <summary>
				/// 大破時に点滅させる
				/// </summary>
				public bool BlinkAtDamaged { get; set; }

				/// <summary>
				/// 艦隊状態の表示方法
				/// </summary>
				public int FleetStateDisplayMode { get; set; }

				public ConfigFormFleet() {
					ShowAircraft = true;
					SearchingAbilityMethod = 4;
					IsScrollable = true;
					FixShipNameWidth = false;
					ShortenHPBar = false;
					ShowNextExp = true;
					EquipmentLevelVisibility = Window.Control.ShipStatusEquipment.LevelVisibilityFlag.Both;
					ShowAircraftLevelByNumber = false;
					AirSuperiorityMethod = 1;
					ShowAnchorageRepairingTimer = true;
					BlinkAtCompletion = true;
					ShowConditionIcon = true;
					FixedShipNameWidth = 40;
					ShowAirSuperiorityRange = false;
					ReflectAnchorageRepairHealing = true;
					EmphasizesSubFleetInPort = false;
					BlinkAtDamaged = true;
					FleetStateDisplayMode = 2;
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
				/// スクリーンショットの保存モード
				/// 1=ファイル, 2=クリップボード, 3=両方
				/// </summary>
				public int ScreenShotSaveMode { get; set; }

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
				/// DMMによるページ更新ダイアログを非表示にするか
				/// </summary>
				public bool IsDMMreloadDialogDestroyable { get; set; }

				/// <summary>
				/// Twitter の画像圧縮を回避するか
				/// </summary>
				public bool AvoidTwitterDeterioration { get; set; }

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
					ScreenShotSaveMode = 1;
					StyleSheet = "\r\nbody {\r\n	margin:0;\r\n	overflow:hidden\r\n}\r\n\r\n#game_frame {\r\n	position:fixed;\r\n	left:50%;\r\n	top:-16px;\r\n	margin-left:-450px;\r\n	z-index:1\r\n}\r\n";
					IsScrollable = false;
					AppliesStyleSheet = true;
					IsDMMreloadDialogDestroyable = false;
					AvoidTwitterDeterioration = false;
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

				/// <summary>
				/// スクロール可能か
				/// </summary>
				public bool IsScrollable { get; set; }

				/// <summary>
				/// 艦名表示の最大幅
				/// </summary>
				public int MaxShipNameWidth { get; set; }


				public ConfigFormCompass() {
					CandidateDisplayCount = 4;
					IsScrollable = false;
					MaxShipNameWidth = 60;
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

				/// <summary>
				/// 戦闘中は表示を隠し、戦闘後のみ表示する
				/// </summary>
				public bool HideDuringBattle { get; set; }

				/// <summary>
				/// HP バーを表示するか
				/// </summary>
				public bool ShowHPBar { get; set; }

				/// <summary>
				/// HP バーに艦種を表示するか
				/// </summary>
				public bool ShowShipTypeInHPBar { get; set; }

				public ConfigFormBattle() {
					IsScrollable = false;
					HideDuringBattle = false;
					ShowHPBar = true;
					ShowShipTypeInHPBar = false;
				}
			}

			/// <summary>
			/// [戦闘]ウィンドウ
			/// </summary>
			[DataMember]
			public ConfigFormBattle FormBattle { get; private set; }


			/// <summary>
			/// [基地航空隊]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormBaseAirCorps : ConfigPartBase {

				/// <summary>
				/// イベント海域のもののみ表示するか
				/// </summary>
				public bool ShowEventMapOnly { get; set; }

				public ConfigFormBaseAirCorps() {
					ShowEventMapOnly = false;
				}
			}

			/// <summary>
			/// [基地航空隊]ウィンドウ
			/// </summary>
			[DataMember]
			public ConfigFormBaseAirCorps FormBaseAirCorps { get; private set; }



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


			/// <summary>
			/// 編成画像出力の設定を扱います。
			/// </summary>
			public class ConfigFleetImageGenerator : ConfigPartBase {

				public FleetImageArgument Argument { get; set; }
				public int ImageType { get; set; }
				public int OutputType { get; set; }
				public bool OpenImageAfterOutput { get; set; }
				public string LastOutputPath { get; set; }
				public bool DisableOverwritePrompt { get; set; }
				public bool AutoSetFileNameToDate { get; set; }
				public bool SyncronizeTitleAndFileName { get; set; }

				public ConfigFleetImageGenerator()
					: base() {
					Argument = FleetImageArgument.GetDefaultInstance();
					ImageType = 0;
					OutputType = 0;
					OpenImageAfterOutput = false;
					LastOutputPath = "";
					DisableOverwritePrompt = false;
					AutoSetFileNameToDate = false;
					SyncronizeTitleAndFileName = false;
				}
			}
			[DataMember]
			public ConfigFleetImageGenerator FleetImageGenerator { get; private set; }



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
				FormBaseAirCorps = new ConfigFormBaseAirCorps();

				NotifierExpedition = new ConfigNotifierBase();
				NotifierConstruction = new ConfigNotifierBase();
				NotifierRepair = new ConfigNotifierBase();
				NotifierCondition = new ConfigNotifierBase();
				NotifierDamage = new ConfigNotifierDamage();
				NotifierAnchorageRepair = new ConfigNotifierAnchorageRepair();

				BGMPlayer = new ConfigBGMPlayer();
				FleetImageGenerator = new ConfigFleetImageGenerator();
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

			// 读取配色主题
			#region dynamic json = 内嵌主题;
			dynamic json = DynamicJson.Parse(@"[{
""name"":""VS2012 Light"",
""basicColors"":{
""red"":""#FF0000"",
""orange"":""#FFA500"",
""yellow"":""#FFFF00"",
""green"":""#00FF00"",
""cyan"":""#00FFFF"",
""blue"":""#0000FF"",
""magenta"":""#FF00FF"",
""violet"":""#EE82EE""
},
""barColors"":[[
""#FF0000"",
""#FF0000"",
""#FF8800"",
""#FF8800"",
""#FFCC00"",
""#FFCC00"",
""#00CC00"",
""#00CC00"",
""#0044CC"",
""#44FF00"",
""#882222"",
""#888888""
],[
""#FF0000"",
""#FF0000"",
""#FF4400"",
""#FF8800"",
""#FFAA00"",
""#EEEE00"",
""#CCEE00"",
""#00CC00"",
""#0044CC"",
""#00FF44"",
""#882222"",
""#888888""
]],
""panelColors"":{
""foreground"":""#000000"",
""background"":""#F0F0F0"",
""foreground2"":""#888888"",
""background2"":""#E3E3E3"",
""statusBarFG"":""#000000"",
""statusBarBG"":""#E3E3E3"",
""skin"":{
""panelSplitter"":""#E3E3E3"",
""docTabBarFG"":""#000000"",
""docTabBarBG"":""#F0F0F0"",
""docTabActiveFG"":""#FFFFFF"",
""docTabActiveBG"":""#007ACC"",
""docTabActiveLostFocusFG"":""#6D6D6D"",
""docTabActiveLostFocusBG"":""#CCCEDB"",
""docTabInactiveHoverFG"":""#FFFFFF"",
""docTabInactiveHoverBG"":""#1C97EA"",
""docBtnActiveHoverFG"":""#FFFFFF"",
""docBtnActiveHoverBG"":""#1C97EA"",
""docBtnActiveLostFocusHoverFG"":""#717171"",
""docBtnActiveLostFocusHoverBG"":""#E6E7ED"",
""docBtnInactiveHoverFG"":""#FFFFFF"",
""docBtnInactiveHoverBG"":""#52B0EF"",
""toolTabBarFG"":""#6D6D6D"",
""toolTabBarBG"":""#F0F0F0"",
""toolTabActive"":""#007ACC"",
""toolTitleActiveFG"":""#FFFFFF"",
""toolTitleActiveBG"":""#007ACC"",
""toolTitleLostFocusFG"":""#6D6D6D"",
""toolTitleLostFocusBG"":""#F0F0F0"",
""toolTitleDotActive"":""#50AADC"",
""toolTitleDotLostFocus"":""#A0A0A0"",
""autoHideTabBarFG"":""#E3E3E3"",
""autoHideTabBarBG"":""#F0F0F0"",
""autoHideTabActive"":""#007ACC"",
""autoHideTabInactive"":""#6D6D6D""
},
""fleet"":{
""repairTimerText"":""#888888"",
""conditionText"":""#000000"",
""conditionVeryTired"":""#F08080"",
""conditionTired"":""#FFA07A"",
""conditionLittleTired"":""#FFE4B5"",
""conditionSparkle"":""#90EE90"",
""equipmentLevel"":""#006666""
},
""fleetOverview"":{
""shipDamagedFG"":""#000000"",
""shipDamagedBG"":""#F08080"",
""expeditionOverFG"":""#000000"",
""expeditionOverBG"":""#90EE90"",
""tiredRecoveredFG"":""#000000"",
""tiredRecoveredBG"":""#90EE90"",
""alertNotInExpeditionFG"":""#000000"",
""alertNotInExpeditionBG"":""#90EE90""
},
""dock"":{
""repairFinishedFG"":""#000000"",
""repairFinishedBG"":""#90EE90""
},
""arsenal"":{
""buildCompleteFG"":""#000000"",
""buildCompleteBG"":""#90EE90""
},
""hq"":{
""resOverFG"":""#000000"",
""resOverBG"":""#FFE4B5"",
""shipOverFG"":""#000000"",
""shipOverBG"":""#F08080"",
""materialMaxFG"":""#000000"",
""materialMaxBG"":""#F08080"",
""coinMaxFG"":""#000000"",
""coinMaxFG"":""#F08080"",
""resLowFG"":""#000000"",
""resLowBG"":""#F08080"",
""resMaxFG"":""#000000"",
""resMaxBG"":""#F08080""
},
""quest"":{
""typeFG"":""#000000"",
""typeHensei"":""#AAFFAA"",
""typeShutsugeki"":""#FFCCCC"",
""typeEnshu"":""#DDFFAA"",
""typeEnsei"":""#DDFFAA"",
""typeHokyu"":""#CCFFFF"",
""typeKojo"":""#DDCCBB"",
""typeKaiso"":""#DDCCFF"",
""processLT50"":""#FF8800"",
""processLT80"":""#00CC00"",
""processLT100"":""#008800"",
""processDefault"":""#0088FF""
},
""compass"":{
""shipClass2"":""#FF0000"",
""shipClass3"":""#FF8800"",
""shipClass4"":""#006600"",
""shipClass5"":""#880000"",
""shipClass6"":""#0088FF"",
""shipClass7"":""#FF00FF"",
""shipDestroyed"":""#0000FF"",
""eventKind3"":""#000080"",
""eventKind6"":""#006400"",
""eventKind5"":""#8B0000""
},
""battle"":{
""barMVP"":""#FFE4B5"",
""textMVP"":""#000000"",
""textMVP2"":""#888888"",
""barEscaped"":""#C0C0C0"",
""textEscaped"":""#000000"",
""textEscaped2"":""#888888"",
""barBossDamaged"":""#FFE4E1"",
""textBossDamaged"":""#000000"",
""textBossDamaged2"":""#888888""
}}}]");
			#endregion
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
				ThemePanelColorHex("skin", "panelSplitter"),
				ThemePanelColorHex("skin", "docTabBarFG"),
				ThemePanelColorHex("skin", "docTabBarBG"),
				ThemePanelColorHex("skin", "docTabActiveFG"),
				ThemePanelColorHex("skin", "docTabActiveBG"),
				ThemePanelColorHex("skin", "docTabActiveLostFocusFG"),
				ThemePanelColorHex("skin", "docTabActiveLostFocusBG"),
				ThemePanelColorHex("skin", "docTabInactiveHoverFG"),
				ThemePanelColorHex("skin", "docTabInactiveHoverBG"),
				ThemePanelColorHex("skin", "docBtnActiveHoverFG"),
				ThemePanelColorHex("skin", "docBtnActiveHoverBG"),
				ThemePanelColorHex("skin", "docBtnActiveLostFocusHoverFG"),
				ThemePanelColorHex("skin", "docBtnActiveLostFocusHoverBG"),
				ThemePanelColorHex("skin", "docBtnInactiveHoverFG"),
				ThemePanelColorHex("skin", "docBtnInactiveHoverBG"),
				ThemePanelColorHex("skin", "toolTabBarFG"),
				ThemePanelColorHex("skin", "toolTabBarBG"),
				ThemePanelColorHex("skin", "toolTabActive"),
				ThemePanelColorHex("skin", "toolTitleActiveFG"),
				ThemePanelColorHex("skin", "toolTitleActiveBG"),
				ThemePanelColorHex("skin", "toolTitleLostFocusFG"),
				ThemePanelColorHex("skin", "toolTitleLostFocusBG"),
				ThemePanelColorHex("skin", "toolTitleDotActive"),
				ThemePanelColorHex("skin", "toolTitleDotLostFocus"),
				ThemePanelColorHex("skin", "autoHideTabBarFG"),
				ThemePanelColorHex("skin", "autoHideTabBarBG"),
				ThemePanelColorHex("skin", "autoHideTabActive"),
				ThemePanelColorHex("skin", "autoHideTabInactive")
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
			Config.UI.Fleet_ColorRepairTimerText      = ThemePanelColor("fleet", "repairTimerText");
			Config.UI.Fleet_ColorConditionText        = ThemePanelColor("fleet", "conditionText");
			Config.UI.Fleet_ColorConditionVeryTired   = ThemePanelColor("fleet", "conditionVeryTired");
			Config.UI.Fleet_ColorConditionTired       = ThemePanelColor("fleet", "conditionTired");
			Config.UI.Fleet_ColorConditionLittleTired = ThemePanelColor("fleet", "conditionLittleTired");
			Config.UI.Fleet_ColorConditionSparkle     = ThemePanelColor("fleet", "conditionSparkle");
			Config.UI.Fleet_EquipmentLevelColor       = ThemePanelColor("fleet", "equipmentLevel");
			Config.UI.FleetOverview_ShipDamagedFG    = ThemePanelColor("fleetOverview", "shipDamagedFG");
			Config.UI.FleetOverview_ShipDamagedBG    = ThemePanelColor("fleetOverview", "shipDamagedBG");
			Config.UI.FleetOverview_ExpeditionOverFG = ThemePanelColor("fleetOverview", "expeditionOverFG");
			Config.UI.FleetOverview_ExpeditionOverBG = ThemePanelColor("fleetOverview", "expeditionOverBG");
			Config.UI.FleetOverview_TiredRecoveredFG = ThemePanelColor("fleetOverview", "tiredRecoveredFG");
			Config.UI.FleetOverview_TiredRecoveredBG = ThemePanelColor("fleetOverview", "tiredRecoveredBG");
			Config.UI.FleetOverview_AlertNotInExpeditionFG = ThemePanelColor("fleetOverview", "alertNotInExpeditionFG");
			Config.UI.FleetOverview_AlertNotInExpeditionBG = ThemePanelColor("fleetOverview", "alertNotInExpeditionBG");
			Config.UI.Dock_RepairFinishedFG = ThemePanelColor("dock", "repairFinishedFG");
			Config.UI.Dock_RepairFinishedBG = ThemePanelColor("dock", "repairFinishedBG");
			Config.UI.Arsenal_BuildCompleteFG = ThemePanelColor("arsenal", "buildCompleteFG");
			Config.UI.Arsenal_BuildCompleteBG = ThemePanelColor("arsenal", "buildCompleteBG");
			Config.UI.Headquarters_ResourceOverFG  = ThemePanelColor("hq", "resOverFG");
			Config.UI.Headquarters_ResourceOverBG  = ThemePanelColor("hq", "resOverBG");
			Config.UI.Headquarters_ShipCountOverFG = ThemePanelColor("hq", "shipOverFG");
			Config.UI.Headquarters_ShipCountOverBG = ThemePanelColor("hq", "shipOverBG");
			Config.UI.Headquarters_MaterialMaxFG   = ThemePanelColor("hq", "materialMaxFG");
			Config.UI.Headquarters_MaterialMaxBG   = ThemePanelColor("hq", "materialMaxBG");
			Config.UI.Headquarters_CoinMaxFG       = ThemePanelColor("hq", "coinMaxFG");
			Config.UI.Headquarters_CoinMaxBG       = ThemePanelColor("hq", "coinMaxBG");
			Config.UI.Headquarters_ResourceLowFG   = ThemePanelColor("hq", "resLowFG");
			Config.UI.Headquarters_ResourceLowBG   = ThemePanelColor("hq", "resLowBG");
			Config.UI.Headquarters_ResourceMaxFG   = ThemePanelColor("hq", "resMaxFG");
			Config.UI.Headquarters_ResourceMaxBG   = ThemePanelColor("hq", "resMaxBG");
			Config.UI.Quest_TypeFG     = ThemePanelColor("quest", "typeFG");
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
			Config.UI.Compass_ShipNameColor7 = ThemePanelColor("compass", "shipClass7");
			Config.UI.Compass_ShipNameColorDestroyed = ThemePanelColor("compass", "shipDestroyed");
			Config.UI.Compass_ColorTextEventKind3 = ThemePanelColor("compass", "eventKind3");
			Config.UI.Compass_ColorTextEventKind6 = ThemePanelColor("compass", "eventKind6");
			Config.UI.Compass_ColorTextEventKind5 = ThemePanelColor("compass", "eventKind5");
			Config.UI.Compass_ColoroverlayBrush = ThemePanelColor("compass", "overlayBrush");
			Config.UI.Battle_ColorHPBarsMVP = ThemePanelColor("battle", "barMVP");
			Config.UI.Battle_ColorTextMVP   = ThemePanelColor("battle", "textMVP");
			Config.UI.Battle_ColorTextMVP2  = ThemePanelColor("battle", "textMVP2");
			Config.UI.Battle_ColorHPBarsEscaped = ThemePanelColor("battle", "barEscaped");
			Config.UI.Battle_ColorTextEscaped   = ThemePanelColor("battle", "textEscaped");
			Config.UI.Battle_ColorTextEscaped2  = ThemePanelColor("battle", "textEscaped2");
			Config.UI.Battle_ColorHPBarsBossDamaged = ThemePanelColor("battle", "barBossDamaged");
			Config.UI.Battle_ColorTextBossDamaged   = ThemePanelColor("battle", "textBossDamaged");
			Config.UI.Battle_ColorTextBossDamaged2  = ThemePanelColor("battle", "textBossDamaged2");
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
						return SystemColors.GrayText;
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
					case "fleet_repairTimerText":
						return Config.UI.SubForeColor;
					case "fleet_conditionText":
						return Config.UI.BackColor;
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
					// 视图 - 舰队一览
					case "fleetOverview_shipDamagedFG":
						return Config.UI.BackColor;
					case "fleetOverview_shipDamagedBG":
						return Config.UI.Color_Red;
					case "fleetOverview_expeditionOverFG":
						return Config.UI.BackColor;
					case "fleetOverview_expeditionOverBG":
						return Config.UI.Color_Blue;
					case "fleetOverview_tiredRecoveredFG":
						return Config.UI.BackColor;
					case "fleetOverview_tiredRecoveredBG":
						return Config.UI.Color_Blue;
					case "fleetOverview_alertNotInExpeditionFG":
						return Config.UI.BackColor;
					case "fleetOverview_alertNotInExpeditionBG":
						return Config.UI.Color_Blue;
					// 视图 - 司令部
					case "hq_resOverFG":
						return Config.UI.ForeColor;
					case "hq_resOverBG":
						return Config.UI.SubBackColor;
					case "hq_shipOverFG":
						return Config.UI.BackColor;
					case "hq_shipOverBG":
						return Config.UI.Color_Red;
					case "hq_materialMaxFG":
						return Config.UI.BackColor;
					case "hq_materialMaxBG":
						return Config.UI.Color_Blue;
					case "hq_coinMaxFG":
						return Config.UI.BackColor;
					case "hq_coinMaxBG":
						return Config.UI.Color_Blue;
					case "hq_resLowFG":
						return Config.UI.BackColor;
					case "hq_resLowBG":
						return Config.UI.Color_Red;
					case "hq_resMaxFG":
						return Config.UI.BackColor;
					case "hq_resMaxBG":
						return Config.UI.Color_Blue;
					// 视图 - 入渠
					case "dock_repairFinishedFG":
						return Config.UI.BackColor;
					case "dock_repairFinishedBG":
						return Config.UI.Color_Blue;
					// 视图 - 工厂
					case "arsenal_buildCompleteFG":
						return Config.UI.BackColor;
					case "arsenal_buildCompleteBG":
						return Config.UI.Color_Blue;
					// 视图 - 任务
					case "quest_typeFG":
						return Config.UI.BackColor;
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
						return Config.UI.Color_Green;
					case "compass_shipClass5":
						return Config.UI.Color_Violet;
					case "compass_shipClass6":
						return Config.UI.Color_Cyan;
					case "compass_shipClass7":
						return Config.UI.Color_Blue;
					case "compass_shipDestroyed":
						return Config.UI.Color_Magenta;
					case "compass_eventKind3":
						return Config.UI.Color_Violet;
					case "compass_eventKind6":
						return Config.UI.Color_Green;
					case "compass_eventKind5":
						return Config.UI.Color_Red;
					case "compass_overlayBrush": // %75 透明度背景色
						return Color.FromArgb(0xC0, Config.UI.BackColor);
					// 视图 - 战斗
					case "battle_barMVP":
						return Config.UI.Color_Blue;
					case "battle_textMVP":
						return Config.UI.BackColor;
					case "battle_textMVP2":
						return Config.UI.SubBackColor;
					case "battle_barEscaped":
						return Config.UI.SubBackColor;
					case "battle_textEscaped":
						return Config.UI.ForeColor;
					case "battle_textEscaped2":
						return Config.UI.SubForeColor;
					case "battle_barBossDamaged":
						return Config.UI.Color_Orange;
					case "battle_textBossDamaged":
						return Config.UI.BackColor;
					case "battle_textBossDamaged2":
						return Config.UI.SubBackColor;
					// 未定义颜色
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

		private String ThemePanelColorHex(string form, string name) {
			if (ThemeStyle.IsDefined("panelColors") && ThemeStyle["panelColors"].IsDefined(form) && ThemeStyle["panelColors"][form].IsDefined(name)) {
				return ThemeStyle["panelColors"][form][name];
			} else {
				switch (form + "_" + name) {
					// 面板分割线
					case "skin_panelSplitter":
						return ThemeColorHex("panelColors", "background2");
					case "skin_docTabBarFG":
						return ThemeColorHex("panelColors", "foreground2");
					case "skin_docTabBarBG":
						return ThemeColorHex("panelColors", "background");
					case "skin_docTabActiveFG":
						return ThemeColorHex("panelColors", "foreground");
					case "skin_docTabActiveBG":
						return ThemeColorHex("panelColors", "background2");
					case "skin_docTabActiveLostFocusFG":
						return ThemeColorHex("panelColors", "foreground");
					case "skin_docTabActiveLostFocusBG":
						return ThemeColorHex("panelColors", "background2");
					case "skin_docTabInactiveHoverFG":
						return ThemeColorHex("panelColors", "foreground");
					case "skin_docTabInactiveHoverBG":
						return ThemeColorHex("panelColors", "background2");
					case "skin_docBtnActiveHoverFG":
						return ThemeColorHex("panelColors", "foreground");
					case "skin_docBtnActiveHoverBG":
						return ThemeColorHex("panelColors", "background2");
					case "skin_docBtnActiveLostFocusHoverFG":
						return ThemeColorHex("panelColors", "foreground");
					case "skin_docBtnActiveLostFocusHoverBG":
						return ThemeColorHex("panelColors", "background2");
					case "skin_docBtnInactiveHoverFG":
						return ThemeColorHex("panelColors", "foreground");
					case "skin_docBtnInactiveHoverBG":
						return ThemeColorHex("panelColors", "background2");
					case "skin_toolTabBarFG":
						return ThemeColorHex("panelColors", "foreground2");
					case "skin_toolTabBarBG":
						return ThemeColorHex("panelColors", "background");
					case "skin_toolTabActive":
						return ThemeColorHex("panelColors", "foreground");
					case "skin_toolTitleActiveFG":
						return ThemeColorHex("panelColors", "foreground");
					case "skin_toolTitleActiveBG":
						return ThemeColorHex("panelColors", "background2");
					case "skin_toolTitleLostFocusFG":
						return ThemeColorHex("panelColors", "foreground2");
					case "skin_toolTitleLostFocusBG":
						return ThemeColorHex("panelColors", "background");
					case "skin_toolTitleDotActive":
						return ThemeColorHex("panelColors", "background");
					case "skin_toolTitleDotLostFocus":
						return ThemeColorHex("panelColors", "background2");
					case "skin_autoHideTabBarFG":
						return ThemeColorHex("panelColors", "background2");
					case "skin_autoHideTabBarBG":
						return ThemeColorHex("panelColors", "background");
					case "skin_autoHideTabActive":
						return ThemeColorHex("panelColors", "foreground");
					case "skin_autoHideTabInactive":
						return ThemeColorHex("panelColors", "foreground2");
					default:
						var c = ThemePanelColor(form, name);
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
						return Config.UI.Color_Red;
					case "0_1":
						return Config.UI.Color_Red;
					case "0_2":
						return Config.UI.Color_Orange;
					case "0_3":
						return Config.UI.Color_Orange;
					case "0_4":
						return Config.UI.Color_Yellow;
					case "0_5":
						return Config.UI.Color_Yellow;
					case "0_6":
						return Config.UI.Color_Green;
					case "0_7":
						return Config.UI.Color_Green;
					case "0_8":
						return Config.UI.Color_Blue;
					case "0_9":
						return Config.UI.Color_Magenta;
					case "0_10":
						return Config.UI.Color_Magenta;
					case "0_11":
						return Config.UI.SubBackColor;
					case "1_0":
						return ThemeBarColor(0, 0);
					case "1_1":
						return ThemeBarColor(0, 1);
					case "1_2":
						return ThemeBarColor(0, 2);
					case "1_3":
						return ThemeBarColor(0, 3);
					case "1_4":
						return ThemeBarColor(0, 4);
					case "1_5":
						return ThemeBarColor(0, 5);
					case "1_6":
						return ThemeBarColor(0, 6);
					case "1_7":
						return ThemeBarColor(0, 7);
					case "1_8":
						return ThemeBarColor(0, 8);
					case "1_9":
						return ThemeBarColor(0, 9);
					case "1_10":
						return ThemeBarColor(0, 10);
					case "1_11":
						return ThemeBarColor(0, 11);
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



			// version RN-2.5.4.1-m1 or earlier
			if ( dt <= DateTimeHelper.CSVStringToTime( "2017/03/19 16:08:50" ) ) {

				if ( MessageBox.Show(
					"由于「艦これ」的更新，需要转换记录文件。\r\n要进行转换吗？\r\n( 若不进行转换，可能导致工作不正常。)\r\n\r\n转换前请备份：\r\nEnemyFleetRecord.csv\r\nShipDropRecord.csv\r\nShipParameterRecord.csv", "由 2.5.4.1-m1-patch1 或更早版本的更新确认",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 ) == DialogResult.Yes ) {

					// 敵編成レコードの敵編成ID再計算とドロップレコードの敵編成ID振りなおし
					try {
						var convertPair = new Dictionary<uint, uint>();
						var enemyFleetRecord = new EnemyFleetRecord();
						var shipDropRecord = new ShipDropRecord();
						enemyFleetRecord.Load( RecordManager.Instance.MasterPath );
						shipDropRecord.Load( RecordManager.Instance.MasterPath );

						if (MessageBox.Show(
							"是否已经运行过包含 patch1 的 2.5.4.1-m1 ?\r\n( 若不确定的情况，请选择「是」 )", "确认 patch1 应用状态",
							MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 ) == DialogResult.Yes) {
							// 将敌舰 ID 转换回 501 起、修复 patch1 导致的 999 / 1999 错误、移除重复项
							List<uint> ids = new List<uint>();
							List<uint> keys_to_remove = new List<uint>();
							foreach (var record in enemyFleetRecord.Record) {
								uint drop_key = record.Value.FleetID;
								record.Value.FleetMember = record.Value.FleetMember.Select(id => {
									if (id == 999 || id == 1999) {
										return -1;
									} else if (id > 1500) {
										return id - 1000;
									} else {
										return id;
									}
								}).ToArray();
								uint temp_id = record.Value.FleetID;
								if (ids.Contains(temp_id)) {
									keys_to_remove.Add(record.Key);
								} else {
									ids.Add(temp_id);
								}
								convertPair[drop_key] = record.Value.FleetID;
							}
							foreach (uint key in keys_to_remove) {
								enemyFleetRecord.Record.Remove(key);
							}
							// 修复掉落记录中的错误项
							foreach ( var record in shipDropRecord.Record ) {
								if ( convertPair.ContainsKey( record.EnemyFleetID ) )
									record.EnemyFleetID = convertPair[record.EnemyFleetID];
							}
							// 清空转换表
							convertPair.Clear();
						}

						foreach ( var record in enemyFleetRecord.Record.Values ) {
							uint key = record.FleetID;
							record.FleetMember = record.FleetMember.Select( id => 500 < id && id < 1000 ? id + 1000 : id ).ToArray();
							convertPair.Add( key, record.FleetID );
						}

						foreach ( var record in shipDropRecord.Record ) {
							if ( convertPair.ContainsKey( record.EnemyFleetID ) )
								record.EnemyFleetID = convertPair[record.EnemyFleetID];
						}

						enemyFleetRecord.SaveAll( RecordManager.Instance.MasterPath );
						shipDropRecord.SaveAll( RecordManager.Instance.MasterPath );

					} catch ( Exception ex ) {
						ErrorReporter.SendErrorReport( ex, "CheckUpdate: ドロップレコードのID振りなおしに失敗しました。" );
					}


					// パラメータレコードの移動と破損データのダウンロード
					try {

						var currentRecord = new ShipParameterRecord();
						currentRecord.Load( RecordManager.Instance.MasterPath );

						foreach ( var record in currentRecord.Record.Values ) {
							if ( 500 < record.ShipID && record.ShipID <= 1000 ) {
								record.ShipID += 1000;
							}
						}

						string defaultRecordPath = Path.Combine( Path.GetTempPath(), Path.GetRandomFileName() );
						while ( Directory.Exists( defaultRecordPath ) )
							defaultRecordPath = Path.Combine( Path.GetTempPath(), Path.GetRandomFileName() );

						Directory.CreateDirectory( defaultRecordPath );

						ElectronicObserver.Resource.ResourceManager.CopyDocumentFromArchive( "Record/" + currentRecord.FileName, Path.Combine( defaultRecordPath, currentRecord.FileName ) );

						var defaultRecord = new ShipParameterRecord();
						defaultRecord.Load( defaultRecordPath );
						var changed = new List<int>();

						foreach ( var pair in defaultRecord.Record.Keys.GroupJoin( currentRecord.Record.Keys, i => i, i => i, ( id, list ) => new { id, list } ) ) {
							if ( defaultRecord[pair.id].HPMin > 0 && ( pair.list == null || defaultRecord[pair.id].SaveLine() != currentRecord[pair.id].SaveLine() ) )
								changed.Add( pair.id );
						}

						foreach ( var id in changed ) {
							if ( currentRecord[id] == null )
								currentRecord.Update( new ShipParameterRecord.ShipParameterElement() );
							currentRecord[id].LoadLine( defaultRecord.Record[id].SaveLine() );
						}

						currentRecord.SaveAll( RecordManager.Instance.MasterPath );

						Directory.Delete( defaultRecordPath, true );


					} catch ( Exception ex ) {
						ErrorReporter.SendErrorReport( ex, "パラメータレコードの再編に失敗しました。" );
					}

				}


			}


			// version 2.6.2 or earlier
			if ( dt <= DateTimeHelper.CSVStringToTime( "2017/06/24 17:35:44" ) ) {

				// 開発レコードを重複記録してしまう不具合があったため、重複行の削除を行う

				try {

					var dev = new DevelopmentRecord();
					string path = RecordManager.Instance.MasterPath + "\\" + dev.FileName;


					string backupPath = RecordManager.Instance.MasterPath + "\\Backup_" + DateTimeHelper.GetTimeStamp();
					Directory.CreateDirectory( backupPath );
					File.Copy( path, backupPath + "\\" + dev.FileName );


					if ( File.Exists( path ) ) {

						var lines = new List<string>();
						using ( StreamReader sr = new StreamReader( path, Utility.Configuration.Config.Log.FileEncoding ) ) {
							sr.ReadLine();		// skip header row
							while ( !sr.EndOfStream )
								lines.Add( sr.ReadLine() );
						}

						int beforeCount = lines.Count;
						lines = lines.Distinct().ToList();
						int afterCount = lines.Count;

						using ( StreamWriter sw = new StreamWriter( path, false, Utility.Configuration.Config.Log.FileEncoding ) ) {
							sw.WriteLine( dev.RecordHeader );
							foreach ( var line in lines ) {
								sw.WriteLine( line );
							}
						}

						Utility.Logger.Add(2, "", "<= ver. 2.6.2 开发记录重复 BUG 对应：处理完成。删除了 " + ( beforeCount - afterCount ) + " 条重复记录。" );

					}

				} catch ( Exception ex ) {
					ErrorReporter.SendErrorReport( ex, "<= ver. 2.6.2 開発レコード重複不具合対応: 失敗しました。" );
				}
			}


			Config.VersionUpdateTime = DateTimeHelper.TimeToCSVString( SoftwareInformation.UpdateTime );
		}

	}


}
