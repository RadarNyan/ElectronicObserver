﻿using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_get_member
{

	//一応現役、ケッコンした時などに呼ばれる
	public class ship2 : APIBase
	{

		public override void OnResponseReceived(dynamic data)
		{

			KCDatabase db = KCDatabase.Instance;


			//api_data

			var ships = (dynamic[])data.api_data;

			if (ships.Length > 1)
				db.Ships.Clear();

			foreach (var elem in ships)
			{
				int id = (int)elem.api_id;
				var ship = db.Ships[id];

				if (ship != null)
					ship.LoadFromResponse(APIName, elem);
				else
				{
					var a = new ShipData();
					a.LoadFromResponse(APIName, elem);
					db.Ships.Add(a);
				}
			}


			//api_data_deck
			db.Fleet.LoadFromResponse(APIName, data.api_data_deck);

			base.OnResponseReceived((object)data);
		}

		public override string APIName => "api_get_member/ship2";
	}


}
