using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou
{

	public class remodeling : APIBase
	{

		public override bool IsRequestSupported => true;
		public override bool IsResponseSupported => false;

		public override void OnRequestReceived(Dictionary<string, string> data)
		{

			int id = int.Parse(data["api_id"]);
			var ship = KCDatabase.Instance.Ships[id];
			Utility.Logger.Add(2, "", "已将 ", string.Format("{0}「{1}」 Lv. {2}", ship.MasterShip.ShipTypeName, ship.MasterShip.NameWithClass, ship.Level),
				" 改造为 : ", string.Format("{0}「{1}」", ship.MasterShip.RemodelAfterShip.ShipTypeName, ship.MasterShip.RemodelAfterShip.NameWithClass));

			KCDatabase.Instance.Fleet.LoadFromRequest(APIName, data);

			base.OnRequestReceived(data);
		}

		public override string APIName => "api_req_kaisou/remodeling";
	}

}
