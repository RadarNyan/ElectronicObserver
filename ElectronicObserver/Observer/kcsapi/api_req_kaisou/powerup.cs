﻿using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou {

	public class powerup : APIBase {


		public override void OnRequestReceived( Dictionary<string, string> data ) {

			KCDatabase db = KCDatabase.Instance;


			db.Fleet.LoadFromRequest( APIName, data );


			foreach ( string id in data["api_id_items"].Split( ",".ToCharArray() ) ) {

				int shipID = int.Parse( id );


				ShipData ship = db.Ships[shipID];
				for ( int i = 0; i < ship.Slot.Count; i++ ) {
					if ( ship.Slot[i] != -1 )
						db.Equipments.Remove( ship.Slot[i] );
				}

				Utility.Logger.Add(2, "", "已解体 : ", ship.NameWithLevel);
				db.Ships.Remove( shipID );

			}

			base.OnRequestReceived( data );
		}


		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			var ship = db.Ships[(int)data.api_ship.api_id];


			if ( ship != null ) {

				if ( Utility.Configuration.Config.Log.ShowSpoiler ) {
					if ( (int)data.api_powerup_flag == 0 ) {
						Utility.Logger.Add(2, "", "对 ", ship.NameWithLevel, " 的近代化改修失败。");

					} else {
						var updated_ship = new ShipData();
						updated_ship.LoadFromResponse( APIName, data.api_ship );

						StringBuilder sb = new StringBuilder();

						var contents = new LinkedList<string>();

						int firepower = updated_ship.FirepowerBase - ship.FirepowerBase;
						if ( firepower > 0 )
							contents.AddLast(string.Format("火力 +{0} ({1}/{2})",
								firepower,
								updated_ship.FirepowerBase,
								updated_ship.MasterShip.FirepowerMax));
						int torpedo = updated_ship.TorpedoBase - ship.TorpedoBase;
						if ( torpedo > 0 )
							contents.AddLast(string.Format("雷装 +{0} ({1}/{2})",
								torpedo,
								updated_ship.TorpedoBase,
								updated_ship.MasterShip.TorpedoMax));
						int aa = updated_ship.AABase - ship.AABase;
						if ( aa > 0 )
							contents.AddLast(string.Format("对空 +{0} ({1}/{2})",
								aa,
								updated_ship.AABase,
								updated_ship.MasterShip.AAMax));
						int armor = updated_ship.ArmorBase - ship.ArmorBase;
						if ( armor > 0 )
							contents.AddLast(string.Format("装甲 +{0} ({1}/{2})",
								armor,
								updated_ship.ArmorBase,
								updated_ship.MasterShip.ArmorMax));
						int luck = updated_ship.LuckBase - ship.LuckBase;
						if ( luck > 0 )
							contents.AddLast(string.Format("运 +{0} ({1}/{2})",
								luck,
								updated_ship.LuckBase,
								updated_ship.MasterShip.LuckMax));

						sb.AppendFormat(string.Join(", ", contents));
						Utility.Logger.Add(2, "", "成功对 ", ship.NameWithLevel, " 进行了近代化改修 : " + sb.ToString());
					}
				}
				ship.LoadFromResponse( APIName, data.api_ship );
			}

			db.Fleet.LoadFromResponse( APIName, data.api_deck );


			base.OnResponseReceived( (object)data );
		}



		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return true; } }


		public override string APIName {
			get { return "api_req_kaisou/powerup"; }
		}

	}


}
