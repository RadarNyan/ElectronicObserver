using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou
{

	public class marriage : APIBase
	{

		public override void OnResponseReceived(dynamic data)
		{

			Utility.Logger.Add(2, "", "同 ", KCDatabase.Instance.Ships[(int)data.api_id].Name, " 进行了婚礼 ( 伪 ) 。", "おめでとうございます！");

			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_req_kaisou/marriage";
	}

}
