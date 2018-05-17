using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{

	/// <summary>
	/// 任務のデータを保持します。
	/// </summary>
	public class QuestData : ResponseWrapper, IIdentifiable
	{

		/// <summary>
		/// 任務ID
		/// </summary>
		public int QuestID => (int)RawData.api_no;

		/// <summary>
		/// 任務カテゴリ
		/// </summary>
		public int Category => (int)RawData.api_category;

		/// <summary>
		/// 任務出現タイプ
		/// 1=デイリー, 2=ウィークリー, 3=マンスリー, 4=単発, 5=他
		/// </summary>
		public int Type => (int)RawData.api_type;

		/// <summary>
		/// 遂行状態
		/// 1=未受領, 2=遂行中, 3=達成
		/// </summary>
		public int State
		{
			get { return (int)RawData.api_state; }
			set { RawData.api_state = value; }
		}

		/// <summary>
		/// 任務名
		/// </summary>
		public string Name => (string)RawData.api_title;

		/// <summary>
		/// 説明
		/// </summary>
		public string Description => ((string)RawData.api_detail).Replace("<br>", "\r\n");

		/// <summary>
		/// 報酬
		/// </summary>
		public string Reward
		{
			get
			{
				int[] materials = RawData.api_get_material;

				var materialsReward = new List<string>();
				if (materials[0] != 0)
					materialsReward.Add($"油 x {materials[0]}");
				if (materials[1] != 0)
					materialsReward.Add($"弹 x {materials[1]}");
				if (materials[2] != 0)
					materialsReward.Add($"钢 x {materials[2]}");
				if (materials[3] != 0)
					materialsReward.Add($"铝 x {materials[3]}");

				string result;
				string otherReward = QuestRewards.GetReward(QuestID);
				if (otherReward == "") {
					result = string.Join(", ", materialsReward);
				} else {
					result = $"{string.Join(", ", materialsReward)}, {otherReward}";
				}

				if (result == "") {
					return "";
				} else {
					return $"\r\n报酬 : {result}";
				}
			}
		}

		//undone:api_bonus_flag

		/// <summary>
		/// 進捗
		/// </summary>
		public int Progress => (int)RawData.api_progress_flag;



		public int ID => QuestID;
		public override string ToString() => $"[{QuestID}] {Name}";
	}


}
