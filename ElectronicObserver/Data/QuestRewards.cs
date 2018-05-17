﻿
namespace ElectronicObserver.Data
{
	public class QuestRewards
	{
		public static string GetReward(int questID)
		{
			switch(questID) {
				case 201: // |201|日|敵艦隊を撃破せよ！
					return "开发资材 x 1";
				case 216: // |216|日|敵艦隊主力を撃滅せよ！
					return "高速修复材 x 1, 开发资材 x 1";
				case 210: // |210|日|敵艦隊を10回邀撃せよ！
					return "开发资材 x 1";
				case 211: // |211|変|敵空母を3隻撃沈せよ！
					return "高速修复材 x 2";
				case 212: // |212|変|敵輸送船団を叩け！
					return "开发资材 x 2";
				case 218: // |218|日|敵補給艦を3隻撃沈せよ！
					return "高速修复材 x 1, 开发资材 x 1";
				case 226: // |226|日|南西諸島海域の制海権を握れ！
					return "高速修复材 x 1, 高速建造材 x 1";
				case 230: // |230|日|敵潜水艦を制圧せよ！
					return "高速修复材 x 1";
				case 303: // |303|日|「演習」で練度向上！
					return "高速建造材 x 1";
				case 304: // |304|日|「演習」で他提督を圧倒せよ！
					return "开发资材 x 1";
				case 402: // |402|日|「遠征」を3回成功させよう！
					return "开发资材 x 1";
				case 403: // |403|日|「遠征」を10回成功させよう！
					return "家具箱(小) x 1, 家具箱(中) x 1";
				case 503: // |503|日|艦隊大整備！
					return "高速修复材 x 2";
				case 504: // |504|日|艦隊酒保祭り！
					return "开发资材 x 1, 高速建造材 x 1";
				case 605: // |605|日|新装備「開発」指令
					return "开发资材 x 1, 高速建造材 x 1";
				case 606: // |606|日|新造艦「建造」指令
					return "高速修复材 x 1, 开发资材 x 1";
				case 607: // |607|日|装備「開発」集中強化！
					return "开发资材 x 2";
				case 608: // |608|日|艦娘「建造」艦隊強化！
					return "开发资材 x 2, 高速建造材 x 1";
				case 609: // |609|日|軍縮条約対応！
					return "高速修复材 x 1";
				case 619: // |619|日|装備の改修強化
					return "改修资材 x 1";
				case 673: // |673|日|装備開発力の整備
					return "开发资材 x 1";
				case 674: // |674|日|工廠環境の整備
					return "高速修复材 x 1, 开发资材 x 1";
				case 702: // |702|日|艦の「近代化改修」を実施せよ！
					return "高速修复材 x 1";

				case 213: // |213|週|海上通商破壊作戦
					return "开发资材 x 3";
				case 214: // |214|週|あ号作戦
					return "开发资材 x 2, 高速建造材 x 2";
				case 220: // |220|週|い号作戦
					return "开发资材 x 2";
				case 221: // |221|週|ろ号作戦
					return "高速修复材 x 3";
				case 228: // |228|週|海上護衛戦
					return "高速修复材 x 2, 改修资材 x 1";
				case 229: // |229|週|敵東方艦隊を撃滅せよ！
					return "开发资材 x 2";
				case 242: // |242|週|敵東方中枢艦隊を撃破せよ！
					return "高速修复材 x 1, 开发资材 x 1";
				case 243: // |243|週|南方海域珊瑚諸島沖の制空権を握れ！
					return "开发资材 x 2, 改修资材 x 2";
				case 261: // |261|週|海上輸送路の安全確保に努めよ！
					return "改修资材 x 3";
				case 241: // |241|週|敵北方艦隊主力を撃滅せよ！
					return "开发资材 x 3, 改修资材 x 3";
				case 302: // |302|週|大規模演習
					return "开发资材 x 2, 改修资材 x 1";
				case 404: // |404|週|大規模遠征作戦、発令！
					return "开发资材 x 3, 家具箱(大) x 1";
				case 410: // |410|週|南方への輸送作戦を成功させよ！
					return "家具箱(小) x 1";
				case 411: // |411|週|南方への鼠輸送を継続実施せよ！
					return "开发资材 x 2, 改修资材 x 1";
				case 613: // |613|週|資源の再利用
					return "简易运输部材「ドラム缶(輸送用)」 x 1";
				case 638: // |638|週|対空機銃量産
					return "开发资材 x 2, 改修资材 x 1";
				case 676: // |676|週|装備開発力の集中整備
					return "高速修复材 x 1, 开发资材 x 7";
				case 677: // |677|週|継戦支援能力の整備
					return "高速修复材 x 5";
				case 703: // |703|週|「近代化改修」を進め、戦備を整えよ！
					return "开发资材 x 2, 高速建造材 x 1";

				case 256: // |256|月|「潜水艦隊」出撃せよ！
					return "家具箱(大) x 1, 给粮舰「伊良湖」 x 1";
				case 257: // |257|月|「水雷戦隊」南西へ！
					return "改修资材 x 3, 给粮舰「伊良湖」 x 1";
				case 249: // |249|月|「第五戦隊」出撃せよ！
					return "开发资材 x 5, 家具箱(大) x 1";
				case 259: // |259|月|「水上打撃部隊」南方へ！
					return "高速修复材 x 3, 改修资材 x 4";
				case 265: // |265|月|海上護衛強化月間
					return "开发资材 x 5, 改修资材 x 3";
				case 264: // |264|月|「空母機動部隊」西へ！
					return "改修资材 x 2, 家具箱(大) x 2";
				case 266: // |266|月|「水上反撃部隊」突入せよ！
					return "开发资材 x 4, 改修资材 x 2";
				case 311: // |311|月|精鋭艦隊演習
					return "高速修复材 x 2, 战斗粮食 x 1";
				case 318: // |318|月|給糧艦「伊良湖」の支援
					return "[ 高速修复材 x 2 / 开发资材 x 2 ], 给粮舰「伊良湖」 x 1";
				case 424: // |424|月|輸送船団護衛を強化せよ！
					return "家具箱(中) x 2";
				case 626: // |626|月|精鋭「艦戦」隊の新編成
					return "舰上战斗机「艦上戦闘機零式艦戦21型(熟練)」";
				case 628: // |628|月|機種転換
					return "舰上战斗机「艦上戦闘機零式艦戦52型(熟練)」";
				case 645: // |645|月|「洋上補給」物資の調達
					return "补给物资「洋上補給 x 1」";

				case 822: // |822|季|沖ノ島海域迎撃戦
					return "改修资材 x 5, 给粮舰「間宮」 x 1";
				case 854: // |854|季|戦果拡張任務！「Z作戦」前段作戦
					return "改修资材 x 4, 给粮舰「伊良湖」 x 3, 战果 +350";
				case 861: // |861|季|強行輸送艦隊、抜錨！
					return "高速修复材 x 4, 洋上補給 x 1";
				case 862: // |862|季|前線の航空偵察を実施せよ！
					return "开发资材 x 8, 改修资材 x 4";
				case 873: // |873|季|北方海域警備を実施せよ！
					return "[ 特注家具职人 x 1 / 小口径主炮「12.7cm連装砲C型改二★3」 x 1 / 勋章 x 1 ], 战斗粮食 x 1";
				case 875: // |875|季|精鋭「三一駆」、鉄底海域に突入せよ！
					return "[ 对空电探「13号対空電探」 x 2 / 对水上电探「22号対水上電探」 x 2 / 战斗详报 x 1 ], [ 新型炮熕兵装资材 x 1 / 礼物箱 x 1 ]";
				case 428: // |428|季|近海に侵入する敵潜を制圧せよ！
					return "改修资材 x 3, 爆雷「九五式爆雷」 x 1";
				case 643: // |643|季|主力「陸攻」の調達
					return "「开发资材」 x 2, 陆上攻击机「一式陸攻」 x 1";
				case 663: // |663|季|新型艤装の継続研究
					return "[ 勋章 x 1, 新型炮熕兵装资材 x 1 ], 开发资材 x 3";
				case 675: // |675|季|運用装備の統合整備
					return "( 每次出现时可能变化 ) [ 陆上战斗机「一式戦 隼II型」 x 1 / 局地战斗机「紫電一一型」 x 1 / 改修资材 x 4 ]";
				case 678: // |678|季|主力艦上戦闘機の更新
					return "[ 开发资材 x 8, 新型航空兵装资材 x 1], 「紫電改二」 x 2, ";

				default:
					return "";
			}
		}
	}
}