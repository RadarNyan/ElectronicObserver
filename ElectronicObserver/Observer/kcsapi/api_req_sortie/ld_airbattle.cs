﻿using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_sortie
{

	public class ld_airbattle : APIBase
	{

		public override void OnResponseReceived(dynamic data)
		{

			KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);


			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_req_sortie/ld_airbattle";
	}

}
