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

			int[] shipIDs = data["api_ship_id"].Split(',').Select(id => int.Parse(id)).ToArray();

			foreach (int shipID in shipIDs) {
				db.Ships[shipID].LoadFromRequest(APIName, data);
			}

			db.Fleet.LoadFromRequest(APIName, data);

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
