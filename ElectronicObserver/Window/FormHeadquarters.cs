using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Support;
using ElectronicObserver.Resource.Record;

namespace ElectronicObserver.Window
{

	public partial class FormHeadquarters : DockContent
	{

		private Form _parentForm;

		public FormHeadquarters(FormMain parent)
		{
			InitializeComponent();

			_parentForm = parent;


			ImageList icons = ResourceManager.Instance.Icons;

			ShipCount.ImageList = icons;
			ShipCount.ImageIndex = (int)ResourceManager.IconContent.HeadQuartersShip;
			EquipmentCount.ImageList = icons;
			EquipmentCount.ImageIndex = (int)ResourceManager.IconContent.HeadQuartersEquipment;
			InstantRepair.ImageList = icons;
			InstantRepair.ImageIndex = (int)ResourceManager.IconContent.ItemInstantRepair;
			InstantConstruction.ImageList = icons;
			InstantConstruction.ImageIndex = (int)ResourceManager.IconContent.ItemInstantConstruction;
			DevelopmentMaterial.ImageList = icons;
			DevelopmentMaterial.ImageIndex = (int)ResourceManager.IconContent.ItemDevelopmentMaterial;
			ModdingMaterial.ImageList = icons;
			ModdingMaterial.ImageIndex = (int)ResourceManager.IconContent.ItemModdingMaterial;
			FurnitureCoin.ImageList = icons;
			FurnitureCoin.ImageIndex = (int)ResourceManager.IconContent.ItemFurnitureCoin;
			Fuel.ImageList = icons;
			Fuel.ImageIndex = (int)ResourceManager.IconContent.ResourceFuel;
			Ammo.ImageList = icons;
			Ammo.ImageIndex = (int)ResourceManager.IconContent.ResourceAmmo;
			Steel.ImageList = icons;
			Steel.ImageIndex = (int)ResourceManager.IconContent.ResourceSteel;
			Bauxite.ImageList = icons;
			Bauxite.ImageIndex = (int)ResourceManager.IconContent.ResourceBauxite;
			DisplayUseItem.ImageList = icons;
			DisplayUseItem.ImageIndex = (int)ResourceManager.IconContent.ItemPresentBox;


			ControlHelper.SetDoubleBuffered(FlowPanelMaster);
			ControlHelper.SetDoubleBuffered(FlowPanelAdmiral);
			ControlHelper.SetDoubleBuffered(FlowPanelFleet);
			ControlHelper.SetDoubleBuffered(FlowPanelUseItem);
			ControlHelper.SetDoubleBuffered(FlowPanelResource);


			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormHeadQuarters]);

		}


		private void FormHeadquarters_Load(object sender, EventArgs e)
		{

			APIObserver o = APIObserver.Instance;

			o.APIList["api_req_nyukyo/start"].RequestReceived += Updated;
			o.APIList["api_req_nyukyo/speedchange"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/createship"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/createship_speedchange"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/destroyitem2"].RequestReceived += Updated;
			o.APIList["api_req_member/updatecomment"].RequestReceived += Updated;

			o.APIList["api_get_member/basic"].ResponseReceived += Updated;
			o.APIList["api_get_member/slot_item"].ResponseReceived += Updated;
			o.APIList["api_port/port"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship2"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/getship"].ResponseReceived += Updated;
			o.APIList["api_req_hokyu/charge"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/destroyitem2"].ResponseReceived += Updated;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/createitem"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/remodel_slot"].ResponseReceived += Updated;
			o.APIList["api_get_member/material"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship_deck"].ResponseReceived += Updated;
			o.APIList["api_req_air_corps/set_plane"].ResponseReceived += Updated;
			o.APIList["api_req_air_corps/supply"].ResponseReceived += Updated;
			o.APIList["api_get_member/useitem"].ResponseReceived += Updated;


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
			Utility.SystemEvents.UpdateTimerTick += SystemEvents_UpdateTimerTick;

			FlowPanelResource.SetFlowBreak(Ammo, true);

			FlowPanelMaster.Visible = false;

		}



		void ConfigurationChanged()
		{

			Font = FlowPanelMaster.Font = Utility.Configuration.Config.UI.JapFont;
			HQLevel.MainFont = Utility.Configuration.Config.UI.JapFont;
			HQLevel.SubFont = Utility.Configuration.Config.UI.JapFont2;
			HQLevel.MainFontColor = Utility.Configuration.Config.UI.ForeColor;
			HQLevel.SubFontColor  = Utility.Configuration.Config.UI.SubForeColor;

			// 点滅しない設定にしたときに消灯状態で固定されるのを防ぐ
			if (!Utility.Configuration.Config.FormHeadquarters.BlinkAtMaximum)
			{
				if (ShipCount.Tag as bool? ?? false)
				{
					ShipCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
					ShipCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
				}

				if (EquipmentCount.Tag as bool? ?? false)
				{
					EquipmentCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
					EquipmentCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
				}
			}

			//visibility
			CheckVisibilityConfiguration();
			{
				var visibility = Utility.Configuration.Config.FormHeadquarters.Visibility.List;
				AdmiralName.Visible = visibility[0];
				AdmiralComment.Visible = visibility[1];
				HQLevel.Visible = visibility[2];
				ShipCount.Visible = visibility[3];
				EquipmentCount.Visible = visibility[4];
				InstantRepair.Visible = visibility[5];
				InstantConstruction.Visible = visibility[6];
				DevelopmentMaterial.Visible = visibility[7];
				ModdingMaterial.Visible = visibility[8];
				FurnitureCoin.Visible = visibility[9];
				Fuel.Visible = visibility[10];
				Ammo.Visible = visibility[11];
				Steel.Visible = visibility[12];
				Bauxite.Visible = visibility[13];
				DisplayUseItem.Visible = visibility[14];
			}

			UpdateDisplayUseItem();
		}


		/// <summary>
		/// VisibleFlags 設定をチェックし、不正な値だった場合は初期値に戻します。
		/// </summary>
		public static void CheckVisibilityConfiguration()
		{
			const int count = 15;
			var config = Utility.Configuration.Config.FormHeadquarters;

			if (config.Visibility == null)
				config.Visibility = new Utility.Storage.SerializableList<bool>(Enumerable.Repeat(true, count).ToList());

			for (int i = config.Visibility.List.Count; i < count; i++)
			{
				config.Visibility.List.Add(true);
			}

		}

		/// <summary>
		/// 各表示項目の名称を返します。
		/// </summary>
		public static IEnumerable<string> GetItemNames()
		{
			yield return "提督名";
			yield return "提督签名";
			yield return "司令部等级";
			yield return "舰船数";
			yield return "装备数";
			yield return "高速修复材";
			yield return "高速建造材";
			yield return "开发资材";
			yield return "改修资材";
			yield return "家具币";
			yield return "燃料";
			yield return "弹药";
			yield return "钢材";
			yield return "铝土";
			yield return "自定义物品";
		}


		void Updated(string apiname, dynamic data)
		{

			KCDatabase db = KCDatabase.Instance;

			var configUI = Utility.Configuration.Config.UI;

			if (!db.Admiral.IsAvailable)
				return;

			FlowPanelMaster.SuspendLayout();

			//Admiral
			FlowPanelAdmiral.SuspendLayout();
			AdmiralName.Text = string.Format("{0} {1}", db.Admiral.AdmiralName, Constants.GetAdmiralRank(db.Admiral.Rank));
			AdmiralComment.Text = db.Admiral.Comment;
			FlowPanelAdmiral.ResumeLayout();

			//HQ Level
			DateTime nowJST = DateTime.UtcNow.AddHours(9);
			HQLevel.Value = db.Admiral.Level;
			{
				StringBuilder tooltip = new StringBuilder();
				bool showRankingPointsTooltip = true;
				if (Utility.Configuration.Config.UI.ShowGrowthInsteadOfNextInHQ) {
					HQLevel.TextNext = "Growth:";
					int exp1 = RecordManager.Instance.Resource.GetExpHalfDay(nowJST);
					if (exp1 == -2013) { // 年末战果黑洞期，显示全年
						HQLevel.TextNext = string.Format("{0} :", nowJST.Year);
						HQLevel.TextValueNext = String.Format("{0:n2}", RecordManager.Instance.Resource.GetExpYear(nowJST.Year) * 7 / 10000.0);
						goto BuildToolTip;
					}
					int exp2 = RecordManager.Instance.Resource.GetExpDay(nowJST);
					if (exp1 >= 0 && exp2 >= 0) {
						HQLevel.TextValueNext = String.Format(
							"{0:n2} / {1:n2}",
							exp1 * 7 / 10000.0,
							exp2 * 7 / 10000.0
						);
					} else {
						HQLevel.TextValueNext = "N/A";
						showRankingPointsTooltip = false;
					}
				} else {
					if (db.Admiral.Level < ExpTable.AdmiralExp.Max(e => e.Key))
					{
						HQLevel.TextNext = "next:";
						HQLevel.ValueNext = ExpTable.GetNextExpAdmiral(db.Admiral.Exp);
					}
					else
					{
						HQLevel.TextNext = "exp:";
						HQLevel.ValueNext = db.Admiral.Exp;
					}
				}

				//戦果ツールチップ
				BuildToolTip:
				StringBuilder tooltipAppend = new StringBuilder();

				tooltip.AppendFormat("提督经验 : {0:n0}\r\n", db.Admiral.Exp);
				if (db.Admiral.Level < ExpTable.AdmiralExp.Max(e => e.Key)) {
					tooltip.AppendFormat("距离升级 : {0:n0} ( 进度 {1:0.00%} )\r\n", ExpTable.GetNextExpAdmiral(db.Admiral.Exp), db.Admiral.Exp / (float)ExpTable.AdmiralExp[db.Admiral.Level + 1].Total);
				} else if (db.Admiral.Exp < 180000000) {
					tooltip.AppendFormat("距离上限 : {0:n0} ( 进度 {1:0.00%} )\r\n", 180000000 - db.Admiral.Exp, db.Admiral.Exp / 180000000f);
				} else {
					tooltip.AppendFormat("( 已达到经验值上限 )\r\n");
					showRankingPointsTooltip = false;
				}

				if (showRankingPointsTooltip) {
					int diff = RecordManager.Instance.Resource.GetExpHalfDay(nowJST, true);
					double rankingPointsPreviousHalfDay = diff * 7 / 10000.0;
					tooltipAppend.AppendFormat("半日 : {0:n2}\t( {1} )\r\n", rankingPointsPreviousHalfDay, RecordManager.Instance.Resource.RankingPeriodString);
					diff = RecordManager.Instance.Resource.GetExpDay(nowJST, true);
					double rankingPointsPreviousDay = diff * 7 / 10000.0;
					tooltipAppend.AppendFormat("单日 : {0:n2}\t( {1} )\r\n", rankingPointsPreviousDay, RecordManager.Instance.Resource.RankingPeriodString);
					diff = RecordManager.Instance.Resource.GetExpMonth(nowJST, true);
					double rankingPointsPreviousMonth = diff * 7 / 10000.0;
					tooltipAppend.AppendFormat("单月 : {0:n2}\t( {1} )\r\n", rankingPointsPreviousMonth, RecordManager.Instance.Resource.RankingPeriodString);

					diff = RecordManager.Instance.Resource.GetExpHalfDay(nowJST);
					if (diff == -2013) {
						TimeSpan timeZoneOffset = DateTimeOffset.Now.Offset - new TimeSpan(9, 0, 0);
						DateTime ends = new DateTime(nowJST.Year, 12, 31, 0, 0, 0).AddDays(1) + timeZoneOffset;
						tooltip.AppendFormat("\r\n{0} 年末战果黑洞期 ( ~ {1} )\r\n", nowJST.Year, ends.ToString("yyyy'/'M'/'d HH':'mm"));
						tooltip.Append(tooltipAppend.ToString());
						diff = RecordManager.Instance.Resource.GetExpYear(nowJST.Year);
						double rankingPointsYear = diff * 7 / 10000.0;
						tooltip.AppendFormat("全年 : {0:n2}\t( {1} )\r\n", rankingPointsYear, RecordManager.Instance.Resource.RankingPeriodString);
					} else {
						tooltip.AppendFormat("\r\n{0}\r\n", RecordManager.Instance.Resource.MonthString);
						double rankingPointsHalfDay = diff * 7 / 10000.0;
						tooltip.AppendFormat("半日 : {0:n2}\t( {1} )\r\n", rankingPointsHalfDay, RecordManager.Instance.Resource.RankingPeriodString, rankingPointsHalfDay - rankingPointsPreviousHalfDay);
						diff = RecordManager.Instance.Resource.GetExpDay(nowJST);
						double rankingPointsDay = diff * 7 / 10000.0;
						tooltip.AppendFormat("今日 : {0:n2}\t( {1} )\r\n", rankingPointsDay, RecordManager.Instance.Resource.RankingPeriodString, rankingPointsDay - rankingPointsPreviousDay);
						diff = RecordManager.Instance.Resource.GetExpMonth(nowJST);
						double rankingPointsMonth = diff * 7 / 10000.0;
						tooltip.AppendFormat("本月 : {0:n2}\t( {1} )\r\n", rankingPointsMonth, RecordManager.Instance.Resource.RankingPeriodString, rankingPointsMonth - rankingPointsPreviousMonth);

						tooltip.Append("\r\n上次结算战果\r\n");
						tooltip.Append(tooltipAppend.ToString());
					}
				}

				ToolTipInfo.SetToolTip(HQLevel, tooltip.ToString());
			}

			//Fleet
			FlowPanelFleet.SuspendLayout();
			{

				ShipCount.Text = string.Format("{0}/{1}", RealShipCount, db.Admiral.MaxShipCount);
				if (RealShipCount > db.Admiral.MaxShipCount - 5)
				{
					ShipCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
					ShipCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
				}
				else
				{
					ShipCount.BackColor = Color.Transparent;
					ShipCount.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				}
				ShipCount.Tag = RealShipCount >= db.Admiral.MaxShipCount;

				EquipmentCount.Text = string.Format("{0}/{1}", RealEquipmentCount, db.Admiral.MaxEquipmentCount);
				if (RealEquipmentCount > db.Admiral.MaxEquipmentCount + 3 - 20)
				{
					EquipmentCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
					EquipmentCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
				}
				else
				{
					EquipmentCount.BackColor = Color.Transparent;
					EquipmentCount.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				}
				EquipmentCount.Tag = RealEquipmentCount >= db.Admiral.MaxEquipmentCount;

			}
			FlowPanelFleet.ResumeLayout();



			var resday = RecordManager.Instance.Resource.GetRecord(DateTime.Now.AddHours(-5).Date.AddHours(5));
			var resweek = RecordManager.Instance.Resource.GetRecord(DateTime.Now.AddHours(-5).Date.AddDays(-(((int)DateTime.Now.AddHours(-5).DayOfWeek + 6) % 7)).AddHours(5)); //月曜日起点
			var resmonth = RecordManager.Instance.Resource.GetRecord(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddHours(5));


			//UseItems
			FlowPanelUseItem.SuspendLayout();

			InstantRepair.Text = db.Material.InstantRepair.ToString();
			if (db.Material.InstantRepair >= 3000) {
				InstantRepair.ForeColor = configUI.Headquarters_MaterialMaxFG;
				InstantRepair.BackColor = configUI.Headquarters_MaterialMaxBG;
			} else if (db.Material.InstantRepair < (configUI.HqResLowAlertBucket == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertBucket)) {
				InstantRepair.ForeColor = configUI.Headquarters_ResourceLowFG;
				InstantRepair.BackColor = configUI.Headquarters_ResourceLowBG;
			} else {
				InstantRepair.ForeColor = configUI.ForeColor;
				InstantRepair.BackColor = Color.Transparent;
			}
			ToolTipInfo.SetToolTip( InstantRepair, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.InstantRepair - resday.InstantRepair ),
					resweek == null ? 0 : ( db.Material.InstantRepair - resweek.InstantRepair ),
					resmonth == null ? 0 : ( db.Material.InstantRepair - resmonth.InstantRepair ) ) );

			InstantConstruction.Text = db.Material.InstantConstruction.ToString();
			if (db.Material.InstantConstruction >= 3000) {
				InstantConstruction.ForeColor = configUI.Headquarters_MaterialMaxFG;
				InstantConstruction.BackColor = configUI.Headquarters_MaterialMaxBG;
			} else {
				InstantConstruction.ForeColor = configUI.ForeColor;
				InstantConstruction.BackColor = Color.Transparent;
			}
			ToolTipInfo.SetToolTip( InstantConstruction, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.InstantConstruction - resday.InstantConstruction ),
					resweek == null ? 0 : ( db.Material.InstantConstruction - resweek.InstantConstruction ),
					resmonth == null ? 0 : ( db.Material.InstantConstruction - resmonth.InstantConstruction ) ) );

			DevelopmentMaterial.Text = db.Material.DevelopmentMaterial.ToString();
			if (db.Material.DevelopmentMaterial >= 3000) {
				DevelopmentMaterial.ForeColor = configUI.Headquarters_MaterialMaxFG;
				DevelopmentMaterial.BackColor = configUI.Headquarters_MaterialMaxBG;
			} else {
				DevelopmentMaterial.ForeColor = configUI.ForeColor;
				DevelopmentMaterial.BackColor = Color.Transparent;
			}
			ToolTipInfo.SetToolTip( DevelopmentMaterial, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.DevelopmentMaterial - resday.DevelopmentMaterial ),
					resweek == null ? 0 : ( db.Material.DevelopmentMaterial - resweek.DevelopmentMaterial ),
					resmonth == null ? 0 : ( db.Material.DevelopmentMaterial - resmonth.DevelopmentMaterial ) ) );

			ModdingMaterial.Text = db.Material.ModdingMaterial.ToString();
			if (db.Material.ModdingMaterial >= 3000) {
				ModdingMaterial.ForeColor = configUI.Headquarters_MaterialMaxFG;
				ModdingMaterial.BackColor = configUI.Headquarters_MaterialMaxBG;
			} else {
				ModdingMaterial.ForeColor = configUI.ForeColor;
				ModdingMaterial.BackColor = Color.Transparent;
			}
			ToolTipInfo.SetToolTip( ModdingMaterial, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.ModdingMaterial - resday.ModdingMaterial ),
					resweek == null ? 0 : ( db.Material.ModdingMaterial - resweek.ModdingMaterial ),
					resmonth == null ? 0 : ( db.Material.ModdingMaterial - resmonth.ModdingMaterial ) ) );

			FurnitureCoin.Text = db.Admiral.FurnitureCoin.ToString();
			if (db.Admiral.FurnitureCoin >= 200000) {
				FurnitureCoin.ForeColor = configUI.Headquarters_CoinMaxFG;
				FurnitureCoin.BackColor = configUI.Headquarters_CoinMaxBG;
			} else {
				FurnitureCoin.ForeColor = configUI.ForeColor;
				FurnitureCoin.BackColor = Color.Transparent;
			}
			{
				int small = db.UseItems[10]?.Count ?? 0;
				int medium = db.UseItems[11]?.Count ?? 0;
				int large = db.UseItems[12]?.Count ?? 0;

				ToolTipInfo.SetToolTip(FurnitureCoin,
						string.Format("(小) x {0} ( +{1} )\r\n(中) x {2} ( +{3} )\r\n(大) x {4} ( +{5} )\r\n",
							small, small * 200,
							medium, medium * 400,
							large, large * 700));
			}
			UpdateDisplayUseItem();
			FlowPanelUseItem.ResumeLayout();


			//Resources
			FlowPanelResource.SuspendLayout();
			{

				Fuel.Text = db.Material.Fuel.ToString();

				if (db.Material.Fuel >= 300000) {
					Fuel.ForeColor = configUI.Headquarters_ResourceMaxFG;
					Fuel.BackColor = configUI.Headquarters_ResourceMaxBG;
				} else if (db.Material.Fuel < (configUI.HqResLowAlertFuel == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertFuel)) {
					Fuel.ForeColor = configUI.Headquarters_ResourceLowFG;
					Fuel.BackColor = configUI.Headquarters_ResourceLowBG;
				} else if (db.Material.Fuel > db.Admiral.MaxResourceRegenerationAmount) {
					Fuel.ForeColor = configUI.Headquarters_ResourceOverFG;
					Fuel.BackColor = configUI.Headquarters_ResourceOverBG;
				} else {
					Fuel.ForeColor = configUI.ForeColor;
					Fuel.BackColor = Color.Transparent;
				}
				ToolTipInfo.SetToolTip( Fuel, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.Fuel - resday.Fuel ),
					resweek == null ? 0 : ( db.Material.Fuel - resweek.Fuel ),
					resmonth == null ? 0 : ( db.Material.Fuel - resmonth.Fuel ) ) );

				Ammo.Text = db.Material.Ammo.ToString();
				if (db.Material.Ammo >= 300000) {
					Ammo.ForeColor = configUI.Headquarters_ResourceMaxFG;
					Ammo.BackColor = configUI.Headquarters_ResourceMaxBG;
				} else if (db.Material.Ammo < (configUI.HqResLowAlertAmmo == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertAmmo)) {
					Ammo.ForeColor = configUI.Headquarters_ResourceLowFG;
					Ammo.BackColor = configUI.Headquarters_ResourceLowBG;
				} else if (db.Material.Ammo > db.Admiral.MaxResourceRegenerationAmount) {
					Ammo.ForeColor = configUI.Headquarters_ResourceOverFG;
					Ammo.BackColor = configUI.Headquarters_ResourceOverBG;
				} else {
					Ammo.ForeColor = configUI.ForeColor;
					Ammo.BackColor = Color.Transparent;
				}
				ToolTipInfo.SetToolTip( Ammo, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.Ammo - resday.Ammo ),
					resweek == null ? 0 : ( db.Material.Ammo - resweek.Ammo ),
					resmonth == null ? 0 : ( db.Material.Ammo - resmonth.Ammo ) ) );

				Steel.Text = db.Material.Steel.ToString();
				if (db.Material.Steel >= 300000) {
					Steel.ForeColor = configUI.Headquarters_ResourceMaxFG;
					Steel.BackColor = configUI.Headquarters_ResourceMaxBG;
				} else if (db.Material.Steel < (configUI.HqResLowAlertSteel == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertSteel)) {
					Steel.ForeColor = configUI.Headquarters_ResourceLowFG;
					Steel.BackColor = configUI.Headquarters_ResourceLowBG;
				} else if (db.Material.Steel > db.Admiral.MaxResourceRegenerationAmount) {
					Steel.ForeColor = configUI.Headquarters_ResourceOverFG;
					Steel.BackColor = configUI.Headquarters_ResourceOverBG;
				} else {
					Steel.ForeColor = configUI.ForeColor;
					Steel.BackColor = Color.Transparent;
				}
				ToolTipInfo.SetToolTip( Steel, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.Steel - resday.Steel ),
					resweek == null ? 0 : ( db.Material.Steel - resweek.Steel ),
					resmonth == null ? 0 : ( db.Material.Steel - resmonth.Steel ) ) );

				Bauxite.Text = db.Material.Bauxite.ToString();
				if (db.Material.Bauxite >= 300000) {
					Bauxite.ForeColor = configUI.Headquarters_ResourceMaxFG;
					Bauxite.BackColor = configUI.Headquarters_ResourceMaxBG;
				} else if (db.Material.Bauxite < (configUI.HqResLowAlertBauxite == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertBauxite)) {
					Bauxite.ForeColor = configUI.Headquarters_ResourceLowFG;
					Bauxite.BackColor = configUI.Headquarters_ResourceLowBG;
				} else if (db.Material.Bauxite > db.Admiral.MaxResourceRegenerationAmount) {
					Bauxite.ForeColor = configUI.Headquarters_ResourceOverFG;
					Bauxite.BackColor = configUI.Headquarters_ResourceOverBG;
				} else {
					Bauxite.ForeColor = configUI.ForeColor;
					Bauxite.BackColor = Color.Transparent;
				}
				ToolTipInfo.SetToolTip( Bauxite, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.Bauxite - resday.Bauxite ),
					resweek == null ? 0 : ( db.Material.Bauxite - resweek.Bauxite ),
					resmonth == null ? 0 : ( db.Material.Bauxite - resmonth.Bauxite ) ) );

			}
			FlowPanelResource.ResumeLayout();

			FlowPanelMaster.ResumeLayout();
			if (!FlowPanelMaster.Visible)
				FlowPanelMaster.Visible = true;
			AdmiralName.Refresh();
			AdmiralComment.Refresh();

		}


		void SystemEvents_UpdateTimerTick()
		{

			KCDatabase db = KCDatabase.Instance;

			if (db.Ships.Count <= 0) return;

			if (Utility.Configuration.Config.FormHeadquarters.BlinkAtMaximum)
			{
				if (ShipCount.Tag as bool? ?? false)
				{
					ShipCount.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG : Color.Transparent;
					ShipCount.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG : Utility.Configuration.Config.UI.ForeColor;
				}

				if (EquipmentCount.Tag as bool? ?? false)
				{
					EquipmentCount.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG : Color.Transparent;
					EquipmentCount.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG : Utility.Configuration.Config.UI.ForeColor;
				}
			}
		}


		private void Resource_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
				new Dialog.DialogResourceChart().Show(_parentForm);
		}

		private void Resource_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				try
				{
					var mat = KCDatabase.Instance.Material;
					Clipboard.SetText($"{mat.Fuel}/{mat.Ammo}/{mat.Steel}/{mat.Bauxite}/修復{mat.InstantRepair}/開発{mat.DevelopmentMaterial}/建造{mat.InstantConstruction}/改修{mat.ModdingMaterial}");
				}
				catch (Exception ex)
				{
					Utility.Logger.Add(3, "資源のクリップボードへのコピーに失敗しました。" + ex.Message);
				}
			}
		}


		private void UpdateDisplayUseItem()
		{
			var db = KCDatabase.Instance;
			var item = db.UseItems[Utility.Configuration.Config.FormHeadquarters.DisplayUseItemID];
			var itemMaster = db.MasterUseItems[Utility.Configuration.Config.FormHeadquarters.DisplayUseItemID];
			string tail = "\r\n( 可在设置中修改 )";

			if (item != null)
			{
				DisplayUseItem.Text = item.Count.ToString();
				ToolTipInfo.SetToolTip(DisplayUseItem, itemMaster.Name + tail);

			}
			else if (itemMaster != null)
			{
				DisplayUseItem.Text = "0";
				ToolTipInfo.SetToolTip(DisplayUseItem, itemMaster.Name + tail);

			}
			else
			{
				DisplayUseItem.Text = "???";
				ToolTipInfo.SetToolTip(DisplayUseItem, "不明なアイテム (ID: " + Utility.Configuration.Config.FormHeadquarters.DisplayUseItemID + ")" + tail);
			}
		}

		private int RealShipCount
		{
			get
			{
				if (KCDatabase.Instance.Battle != null)
					return KCDatabase.Instance.Ships.Count + KCDatabase.Instance.Battle.DroppedShipCount;

				return KCDatabase.Instance.Ships.Count;
			}
		}

		private int RealEquipmentCount
		{
			get
			{
				if (KCDatabase.Instance.Battle != null)
					return KCDatabase.Instance.Equipments.Count + KCDatabase.Instance.Battle.DroppedEquipmentCount;

				return KCDatabase.Instance.Equipments.Count;
			}
		}


		protected override string GetPersistString()
		{
			return "HeadQuarters";
		}


	}

}
