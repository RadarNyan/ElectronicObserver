using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou
{

	public class destroyship : APIBase
	{


		public override void OnRequestReceived(Dictionary<string, string> data)
		{

			KCDatabase db = KCDatabase.Instance;

			bool destroyEquipments = int.Parse(data["api_slot_dest_flag"]) == 1;
			int[] shipIDs = data["api_ship_id"].Split(',').Select(id => int.Parse(id)).ToArray();

			Dictionary<string, int> shipsToDestroy = new Dictionary<string, int>();

			foreach (int shipID in shipIDs) {
				string name = db.Ships[shipID].NameWithLevel;
				shipsToDestroy.TryGetValue(name, out int amount);
				shipsToDestroy[name] = amount + 1;

				db.Ships[shipID].LoadFromRequest(APIName, data);
			}

			db.Fleet.LoadFromRequest(APIName, data);

			var shipsDestroyed = new List<string>();

			foreach (var ship in shipsToDestroy) {
				shipsDestroyed.Add(String.Format("{0}{1}", ship.Key, ship.Value > 1 ? " x " + ship.Value : ""));
			}
			Utility.Logger.Add(2, "", "已解体 : ", String.Join(" , ", shipsDestroyed), destroyEquipments ? " ( 废弃装备 )" : " ( 保留装备 )");

			base.OnRequestReceived(data);
		}

		public override void OnResponseReceived(dynamic data)
		{

			KCDatabase.Instance.Material.LoadFromResponse(APIName, data.api_material);

			base.OnResponseReceived((object)data);
		}


		public override bool IsRequestSupported => true;
		public override bool IsResponseSupported => true;

		public override string APIName => "api_req_kousyou/destroyship";
	}


}
