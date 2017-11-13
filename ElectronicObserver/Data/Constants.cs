using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	public static class Constants {

		#region 艦船・装備

		/// <summary>
		/// 艦船の速力を表す文字列を取得します。
		/// </summary>
		public static string GetSpeed( int value ) {
			switch ( value ) {
				case 0:
					return "陆基";
				case 5:
					return "低速";
				case 10:
					return "高速";
				case 15:
					return "高速+";
				case 20:
					return "最速";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 射程を表す文字列を取得します。
		/// </summary>
		public static string GetRange( int value ) {
			switch ( value ) {
				case 0:
					return "无";
				case 1:
					return "短";
				case 2:
					return "中";
				case 3:
					return "长";
				case 4:
					return "超长";
				case 5:
					return "超长+";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 艦船のレアリティを表す文字列を取得します。
		/// </summary>
		public static string GetShipRarity( int value ) {
			switch ( value ) {
				case 0:
					return "赤";
				case 1:
					return "藍";
				case 2:
					return "青";
				case 3:
					return "水";
				case 4:
					return "銀";
				case 5:
					return "金";
				case 6:
					return "虹";
				case 7:
					return "輝虹";
				case 8:
					return "桜虹";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 装備のレアリティを表す文字列を取得します。
		/// </summary>
		public static string GetEquipmentRarity( int value ) {
			switch ( value ) {
				case 0:
					return "コモン";
				case 1:
					return "レア";
				case 2:
					return "ホロ";
				case 3:
					return "Sホロ";
				case 4:
					return "SSホロ";
				case 5:
					return "SSホロ'";
				case 6:
					return "SSホロ+";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 装備のレアリティの画像インデックスを取得します。
		/// </summary>
		public static int GetEquipmentRarityID( int value ) {
			switch ( value ) {
				case 0:
					return 1;
				case 1:
					return 3;
				case 2:
					return 4;
				case 3:
					return 5;
				case 4:
					return 6;
				case 5:
					return 7;
				case 6:
					return 8;
				default:
					return 0;
			}
		}


		/// <summary>
		/// 艦船のボイス設定フラグを表す文字列を取得します。
		/// </summary>
		public static string GetVoiceFlag( int value ) {

			switch ( value ) {
				case 0:
					return "-";
				case 1:
					return "時報";
				case 2:
					return "放置";
				case 3:
					return "時報+放置";
				case 4:
					return "特殊放置";
				case 5:
					return "時報+特殊放置";
				case 6:
					return "放置+特殊放置";
				case 7:
					return "時報+放置+特殊放置";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 艦船の損傷度合いを表す文字列を取得します。
		/// </summary>
		/// <param name="hprate">現在HP/最大HPで表される割合。</param>
		/// <param name="isPractice">演習かどうか。</param>
		/// <param name="isLandBase">陸上基地かどうか。</param>
		/// <param name="isEscaped">退避中かどうか。</param>
		/// <returns></returns>
		public static string GetDamageState( double hprate, bool isPractice = false, bool isLandBase = false, bool isEscaped = false ) {

			if ( isEscaped )
				return "退避";
			else if ( hprate <= 0.0 )
				return isPractice ? "脱离" : ( !isLandBase ? "击沉" : "破坏" );
			else if ( hprate <= 0.25 )
				return !isLandBase ? "大破" : "损坏";
			else if ( hprate <= 0.5 )
				return !isLandBase ? "中破" : "损害";
			else if ( hprate <= 0.75 )
				return !isLandBase ? "小破" : "混乱";
			else if ( hprate < 1.0 )
				return "健在";
			else
				return "无伤";

		}


		/// <summary>
		/// 基地航空隊の行動指示を表す文字列を取得します。
		/// </summary>
		public static string GetBaseAirCorpsActionKind( int value ) {
			switch ( value ) {
				case 0:
					return "待机";
				case 1:
					return "出击";
				case 2:
					return "防空";
				case 3:
					return "退避";
				case 4:
					return "休息";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 艦種略号を取得します。
		/// </summary>
		public static string GetShipClassClassification( int shiptype ) {
			switch ( shiptype ) {
				case 1:
					return "DE";
				case 2:
					return "DD";
				case 3:
					return "CL";
				case 4:
					return "CLT";
				case 5:
					return "CA";
				case 6:
					return "CAV";
				case 7:
					return "CVL";
				case 8:
					return "BC";	// ? FBB, CC?
				case 9:
					return "BB";
				case 10:
					return "BBV";
				case 11:
					return "CV";
				case 12:
					return "BB";
				case 13:
					return "SS";
				case 14:
					return "SSV";
				case 15:
					return "AP";	// ? AO?
				case 16:
					return "AV";
				case 17:
					return "LHA";
				case 18:
					return "CVB";
				case 19:
					return "AR";
				case 20:
					return "AS";
				case 21:
					return "CT";
				case 22:
					return "AO";
				default:
					return "IX";
			}
		}

		#endregion


		#region 出撃

		/// <summary>
		/// マップ上のセルでのイベントを表す文字列を取得します。
		/// </summary>
		public static string GetMapEventID( int value ) {

			switch ( value ) {

				case 0:
					return "起始位置";
				case 1:
					return "无";
				case 2:
					return "资源";
				case 3:
					return "旋窝";
				case 4:
					return "普通战斗";
				case 5:
					return "BOSS 战";
				case 6:
					return "気のせいだった";
				case 7:
					return "航空战";
				case 8:
					return "船団護衛成功";
				case 9:
					return "登陆地点";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// マップ上のセルでのイベント種別を表す文字列を取得します。
		/// </summary>
		public static string GetMapEventKind( int value ) {

			switch ( value ) {
				case 0:
					return "非战斗";
				case 1:
					return "昼夜战";
				case 2:
					return "夜战";
				case 3:
					return "夜昼战";
				case 4:
					return "航空战";
				case 5:
					return "敌联合";
				case 6:
					return "空袭战";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 海域難易度を表す文字列を取得します。
		/// </summary>
		public static string GetDifficulty( int value ) {

			switch ( value ) {
				case -1:
					return "无";
				case 0:
					return "未选择";
				case 1:
					return "丙";
				case 2:
					return "乙";
				case 3:
					return "甲";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 海域難易度を表す数値を取得します。
		/// </summary>
		public static int GetDifficulty( string value ) {

			switch ( value ) {
				case "未選択":
					return 0;
				case "丙":
					return 1;
				case "乙":
					return 2;
				case "甲":
					return 3;
				default:
					return -1;
			}

		}

		/// <summary>
		/// 空襲被害の状態を表す文字列を取得します。
		/// </summary>
		public static string GetAirRaidDamage( int value ) {
			switch ( value ) {
				case 1:
					return "发生空袭 - 资源受损";
				case 2:
					return "发生空袭 - 资源、航空队受损";
				case 3:
					return "发生空袭 - 航空队受损";
				case 4:
					return "发生空袭 - 未受损";
				default:
					return "未发生空袭";
			}
		}

		/// <summary>
		/// 空襲被害の状態を表す文字列を取得します。(短縮版)
		/// </summary>
		public static string GetAirRaidDamageShort( int value ) {
			switch ( value ) {
				case 1:
					return "资源受损";
				case 2:
					return "资源、航空";
				case 3:
					return "航空队受损";
				case 4:
					return "未受损";
				default:
					return "-";
			}
		}


		#endregion


		#region 戦闘

		/// <summary>
		/// 陣形を表す文字列を取得します。
		/// </summary>
		public static string GetFormation( int id ) {
			switch ( id ) {
				case 1:
					return "単縦陣";
				case 2:
					return "複縦陣";
				case 3:
					return "輪形陣";
				case 4:
					return "梯形陣";
				case 5:
					return "単横陣";
				case 11:
					return "第一警戒航行序列";
				case 12:
					return "第二警戒航行序列";
				case 13:
					return "第三警戒航行序列";
				case 14:
					return "第四警戒航行序列";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 陣形を表す数値を取得します。
		/// </summary>
		public static int GetFormation( string value ) {
			switch ( value ) {
				case "単縦陣":
					return 1;
				case "複縦陣":
					return 2;
				case "輪形陣":
					return 3;
				case "梯形陣":
					return 4;
				case "単横陣":
					return 5;
				case "第一警戒航行序列":
					return 11;
				case "第二警戒航行序列":
					return 12;
				case "第三警戒航行序列":
					return 13;
				case "第四警戒航行序列":
					return 14;
				default:
					return -1;
			}
		}

		/// <summary>
		/// 陣形を表す文字列(短縮版)を取得します。
		/// </summary>
		public static string GetFormationShort( int id ) {
			switch ( id ) {
				case 1:
					return "单纵阵";
				case 2:
					return "复纵阵";
				case 3:
					return "轮形阵";
				case 4:
					return "梯形阵";
				case 5:
					return "单横阵";
				case 11:
					return "第一警戒";
				case 12:
					return "第二警戒";
				case 13:
					return "第三警戒";
				case 14:
					return "第四警戒";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 交戦形態を表す文字列を取得します。
		/// </summary>
		public static string GetEngagementForm( int id ) {
			switch ( id ) {
				case 1:
					return "同航战";
				case 2:
					return "反航战";
				case 3:
					return "T字有利";
				case 4:
					return "T字不利";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 索敵結果を表す文字列を取得します。
		/// </summary>
		public static string GetSearchingResult( int id ) {
			switch ( id ) {
				case 1:
					return "成功";
				case 2:
					return "成功(未帰還有)";
				case 3:
					return "未帰還";
				case 4:
					return "失敗";
				case 5:
					return "成功(非索敵機)";
				case 6:
					return "失敗(非索敵機)";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 索敵結果を表す文字列(短縮版)を取得します。
		/// </summary>
		public static string GetSearchingResultShort( int id ) {
			switch ( id ) {
				case 1:
					return "成功";
				case 2:
					return "成功△";
				case 3:
					return "未归还";
				case 4:
					return "失败";
				case 5:
					return "成功";
				case 6:
					return "失败";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 制空戦の結果を表す文字列を取得します。
		/// </summary>
		public static string GetAirSuperiority( int id ) {
			switch ( id ) {
				case 0:
					return "航空均衡";
				case 1:
					return "制空权确保";
				case 2:
					return "航空优势";
				case 3:
					return "航空劣势";
				case 4:
					return "制空权丧失";
				default:
					return "不明";
			}
		}



		/// <summary>
		/// 昼戦攻撃種別を表す文字列を取得します。
		/// </summary>
		public static string GetDayAttackKind( DayAttackKind id ) {
			switch ( id ) {
				case DayAttackKind.NormalAttack:
					return "一般攻击";
				case DayAttackKind.Laser:
					return "レーザー攻撃";
				case DayAttackKind.DoubleShelling:
					return "连续射击";
				case DayAttackKind.CutinMainSub:
					return "CI ( 主炮 / 副炮 )";
				case DayAttackKind.CutinMainLadar:
					return "CI ( 主炮 / 电探 )";
				case DayAttackKind.CutinMainAP:
					return "CI ( 主炮 / 穿甲 )";
				case DayAttackKind.CutinMainMain:
					return "CI ( 主炮 / 主炮 )";
				case DayAttackKind.CutinAirAttack:
					return "空母 CI";
				case DayAttackKind.Shelling:
					return "炮击";
				case DayAttackKind.AirAttack:
					return "空袭";
				case DayAttackKind.DepthCharge:
					return "爆雷攻击";
				case DayAttackKind.Torpedo:
					return "雷击";
				case DayAttackKind.Rocket:
					return "火箭炮炮击";
				case DayAttackKind.LandingDaihatsu:
					return "登陆攻击 ( 大发动艇 )";
				case DayAttackKind.LandingTokuDaihatsu:
					return "登陆攻击 ( 特大发动艇 )";
				case DayAttackKind.LandingDaihatsuTank:
					return "登陆攻击 ( 大发战车 )";
				case DayAttackKind.LandingAmphibious:
					return "登陆攻击 ( 内火艇 )";
				case DayAttackKind.LandingTokuDaihatsuTank:
					return "登陆攻击 ( 特大发战车 )";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 夜戦攻撃種別を表す文字列を取得します。
		/// </summary>
		public static string GetNightAttackKind( NightAttackKind id ) {
			switch ( id ) {
				case NightAttackKind.NormalAttack:
					return "一般攻击";
				case NightAttackKind.DoubleShelling:
					return "连续射击";
				case NightAttackKind.CutinMainTorpedo:
					return "CI ( 主炮 / 鱼雷 )";
				case NightAttackKind.CutinTorpedoTorpedo:
					return "CI ( 主炮 x 2 )";
				case NightAttackKind.CutinMainSub:
					return "CI ( 主炮 x 2 / 副炮 )";
				case NightAttackKind.CutinMainMain:
					return "CI ( 主炮 x 3 )";
				case NightAttackKind.CutinAirAttack:
					return "空母 CI";
				case NightAttackKind.Shelling:
					return "炮击";
				case NightAttackKind.AirAttack:
					return "空袭";
				case NightAttackKind.DepthCharge:
					return "爆雷攻击";
				case NightAttackKind.Torpedo:
					return "雷击";
				case NightAttackKind.Rocket:
					return "火箭炮炮击";
				case NightAttackKind.LandingDaihatsu:
					return "登陆攻击 ( 大发动艇 )";
				case NightAttackKind.LandingTokuDaihatsu:
					return "登陆攻击 ( 特大发动艇 )";
				case NightAttackKind.LandingDaihatsuTank:
					return "登陆攻击 ( 大发战车 )";
				case NightAttackKind.LandingAmphibious:
					return "登陆攻击 ( 内火艇 )";
				case NightAttackKind.LandingTokuDaihatsuTank:
					return "登陆攻击 ( 特大发战车 )";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 対空カットイン種別を表す文字列を取得します。
		/// </summary>
		public static string GetAACutinKind( int id ) {
			switch ( id ) {
				case 0:
					return "无";
				case 1:
					return "高角炮 x 2 / 电探 ( 秋月型 )";
				case 2:
					return "高角炮 / 电探 ( 秋月型 )";
				case 3:
					return "高角炮 x 2 ( 秋月型 )";
				case 4:
					return "大口径主炮 / 三式弹 / 高射装置 / 电探";
				case 5:
					return "高角炮＋高射装置 x 2 / 电探";
				case 6:
					return "大口径主炮 / 三式弹 / 高射装置";
				case 7:
					return "高角炮 / 高射装置 / 电探";
				case 8:
					return "高角炮＋高射装置 / 电探";
				case 9:
					return "高角炮 / 高射装置";
				case 10:
					return "高角炮 / 集中机枪 / 电探 ( 摩耶 )";
				case 11:
					return "高角炮 / 集中机枪 ( 摩耶 )";
				case 12:
					return "集中机枪 / 机枪 / 电探";
				case 14:
					return "高角炮 / 机枪 / 电探 ( 五十鈴 )";
				case 15:
					return "高角炮 / 机枪 ( 五十鈴 )";
				case 16:
					return "高角炮 / 机枪 / 电探 ( 霞 )";
				case 17:
					return "高角炮 / 机枪 ( 霞 )";
				case 18:
					return "集中机枪 ( 皐月 )";
				case 19:
					return "高角炮 ( 无高射装置 ) / 集中机枪 ( 鬼怒 )";
				case 20:
					return "集中机枪 ( 鬼怒 )";
				case 21:
					return "高角炮 / 电探 ( 由良 )";
				case 22:
					return "集中机枪 ( 文月 )";
				case 23:
					return "机枪 ( 非集中 ) ( UIT-25 )";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 勝利ランクを表すIDを取得します。
		/// </summary>
		public static int GetWinRank( string rank ) {
			switch ( rank.ToUpper() ) {
				case "E":
					return 1;
				case "D":
					return 2;
				case "C":
					return 3;
				case "B":
					return 4;
				case "A":
					return 5;
				case "S":
					return 6;
				case "SS":
					return 7;
				default:
					return 0;
			}
		}

		/// <summary>
		/// 勝利ランクを表す文字列を取得します。
		/// </summary>
		public static string GetWinRank( int rank ) {
			switch ( rank ) {
				case 1:
					return "E";
				case 2:
					return "D";
				case 3:
					return "C";
				case 4:
					return "B";
				case 5:
					return "A";
				case 6:
					return "S";
				case 7:
					return "SS";
				default:
					return "不明";
			}
		}

		#endregion


		#region その他

		/// <summary>
		/// 資源の名前を取得します。
		/// </summary>
		/// <param name="materialID">資源のID。</param>
		/// <returns>資源の名前。</returns>
		public static string GetMaterialName( int materialID ) {

			switch ( materialID ) {
				case 1:
					return "燃料";
				case 2:
					return "弹药";
				case 3:
					return "钢材";
				case 4:
					return "铝土";
				case 5:
					return "高速建造材";
				case 6:
					return "高速修复材";
				case 7:
					return "开发资材";
				case 8:
					return "改修资材";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 階級を表す文字列を取得します。
		/// </summary>
		public static string GetAdmiralRank( int id ) {
			switch ( id ) {
				case 1:
					return "元帥";
				case 2:
					return "大将";
				case 3:
					return "中将";
				case 4:
					return "少将";
				case 5:
					return "大佐";
				case 6:
					return "中佐";
				case 7:
					return "新米中佐";
				case 8:
					return "少佐";
				case 9:
					return "中堅少佐";
				case 10:
					return "新米少佐";
				default:
					return "提督";
			}
		}


		/// <summary>
		/// 任務の発生タイプを表す文字列を取得します。
		/// </summary>
		public static string GetQuestType( int id ) {
			switch ( id ) {
				case 1:		//デイリー
					return "日";
				case 2:		//ウィークリー
					return "周";
				case 3:		//マンスリー
					return "月";
				case 4:		//単発
					return "单";
				case 5:		//その他(輸送5/空母3)
					return "他";
				default:
					return "?";
			}

		}


		/// <summary>
		/// 任務のカテゴリを表す文字列を取得します。
		/// </summary>
		public static string GetQuestCategory( int id ) {
			switch ( id ) {
				case 1:
					return "编成";
				case 2:
					return "出击";
				case 3:
					return "演习";
				case 4:
					return "远征";
				case 5:
					return "补给";		//入渠も含むが、文字数の関係
				case 6:
					return "工厂";
				case 7:
					return "改装";
				case 8:
					return "出击";
				case 9:
					return "其他";
				default:
					return "未知";
			}
		}


		/// <summary>
		/// 遠征の結果を表す文字列を取得します。
		/// </summary>
		public static string GetExpeditionResult( int value ) {
			switch ( value ) {
				case 0:
					return "失败";
				case 1:
					return "成功";
				case 2:
					return "大成功";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 連合艦隊の編成名を表す文字列を取得します。
		/// </summary>
		public static string GetCombinedFleet( int value ) {
			switch ( value ) {
				case 0:
					return "通常艦隊";
				case 1:
					return "機動部隊";
				case 2:
					return "水上部隊";
				case 3:
					return "輸送部隊";
				default:
					return "不明";
			}
		}

		#endregion

	}

}
