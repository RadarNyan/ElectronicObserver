﻿using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ElectronicObserver.Window.Dialog
{

	public partial class DialogResourceChart : Form
	{


		private enum ChartType
		{
			Resource,
			ResourceDiff,
			Material,
			MaterialDiff,
			Experience,
			ExperienceDiff,
		}

		private enum ChartSpan
		{
			Day,
			Week,
			Month,
			Season,
			Year,
			All,
			WeekFirst,
			MonthFirst,
			SeasonFirst,
			YearFirst,
		}



		private ChartType SelectedChartType => (ChartType)GetSelectedMenuStripIndex(Menu_Graph);

		private ChartSpan SelectedChartSpan => (ChartSpan)GetSelectedMenuStripIndex(Menu_Span);




		public DialogResourceChart()
		{
			InitializeComponent();
		}



		private void DialogResourceChart_Load(object sender, EventArgs e)
		{

			if (!RecordManager.Instance.Resource.Record.Any())
			{
				MessageBox.Show("レコード データが存在しません。\n一度母港に移動してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
				return;
			}


			{
				int i = 0;
				foreach (var span in Menu_Span.DropDownItems.OfType<ToolStripMenuItem>())
				{
					span.Tag = i;
					i++;
				}
			}


			SwitchMenuStrip(Menu_Graph, 0);
			SwitchMenuStrip(Menu_Span, 2);


			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormResourceChart]);

			UpdateChart();
		}



		private void SetResourceChart()
		{

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add("ResourceChartArea");
			area.AxisX = CreateAxisX(SelectedChartSpan);
			area.AxisY = CreateAxisY(2000);
			area.AxisY2 = CreateAxisY(200);

			ResourceChart.Legends.Clear();
			var legend = ResourceChart.Legends.Add("ResourceLegend");
			legend.Font = Font;


			ResourceChart.Series.Clear();

			var fuel = ResourceChart.Series.Add("ResourceSeries_Fuel");
			var ammo = ResourceChart.Series.Add("ResourceSeries_Ammo");
			var steel = ResourceChart.Series.Add("ResourceSeries_Steel");
			var bauxite = ResourceChart.Series.Add("ResourceSeries_Bauxite");
			var instantRepair = ResourceChart.Series.Add("ResourceSeries_InstantRepair");

			var setSeries = new Action<Series>(s =>
			{
				s.ChartType = SeriesChartType.Line;
				s.Font = Font;
				s.XValueType = ChartValueType.DateTime;
			});

			setSeries(fuel);
			fuel.Color = Color.FromArgb(0, 128, 0);
			fuel.LegendText = "燃料";

			setSeries(ammo);
			ammo.Color = Color.FromArgb(255, 128, 0);
			ammo.LegendText = "弹药";

			setSeries(steel);
			steel.Color = Color.FromArgb(64, 64, 64);
			steel.LegendText = "钢材";

			setSeries(bauxite);
			bauxite.Color = Color.FromArgb(255, 0, 0);
			bauxite.LegendText = "铝土";

			setSeries(instantRepair);
			instantRepair.Color = Color.FromArgb(32, 128, 255);
			instantRepair.LegendText = "高速修复材";
			instantRepair.YAxisType = AxisType.Secondary;


			//データ設定
			{
				var record = GetRecords();

				if (record.Any())
				{
					var prev = record.First();
					foreach (var r in record)
					{

						if (ShouldSkipRecord(r.Date - prev.Date))
							continue;

						fuel.Points.AddXY(r.Date.ToOADate(), r.Fuel);
						ammo.Points.AddXY(r.Date.ToOADate(), r.Ammo);
						steel.Points.AddXY(r.Date.ToOADate(), r.Steel);
						bauxite.Points.AddXY(r.Date.ToOADate(), r.Bauxite);
						instantRepair.Points.AddXY(r.Date.ToOADate(), r.InstantRepair);

						prev = r;
					}
				}
			}


			SetYBounds();
		}


		private void SetResourceDiffChart()
		{

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add("ResourceChartArea");
			area.AxisX = CreateAxisX(SelectedChartSpan);
			area.AxisY = CreateAxisY(200);
			area.AxisY2 = CreateAxisY(20);

			ResourceChart.Legends.Clear();
			var legend = ResourceChart.Legends.Add("ResourceLegend");
			legend.Font = Font;


			ResourceChart.Series.Clear();

			var fuel = ResourceChart.Series.Add("ResourceSeries_Fuel");
			var ammo = ResourceChart.Series.Add("ResourceSeries_Ammo");
			var steel = ResourceChart.Series.Add("ResourceSeries_Steel");
			var bauxite = ResourceChart.Series.Add("ResourceSeries_Bauxite");
			var instantRepair = ResourceChart.Series.Add("ResourceSeries_InstantRepair");

			var setSeries = new Action<Series>(s =>
			{
				s.ChartType = SeriesChartType.Area;
				//s.SetCustomProperty( "PointWidth", "1.0" );		//棒グラフの幅
				//s.Enabled = false;	//表示するか
				s.Font = Font;
				s.XValueType = ChartValueType.DateTime;
			});

			setSeries(fuel);
			fuel.Color = Color.FromArgb(64, 0, 128, 0);
			fuel.BorderColor = Color.FromArgb(255, 0, 128, 0);
			fuel.LegendText = "燃料";

			setSeries(ammo);
			ammo.Color = Color.FromArgb(64, 255, 128, 0);
			ammo.BorderColor = Color.FromArgb(255, 255, 128, 0);
			ammo.LegendText = "弹药";

			setSeries(steel);
			steel.Color = Color.FromArgb(64, 64, 64, 64);
			steel.BorderColor = Color.FromArgb(255, 64, 64, 64);
			steel.LegendText = "钢材";

			setSeries(bauxite);
			bauxite.Color = Color.FromArgb(64, 255, 0, 0);
			bauxite.BorderColor = Color.FromArgb(255, 255, 0, 0);
			bauxite.LegendText = "铝土";

			setSeries(instantRepair);
			instantRepair.Color = Color.FromArgb(64, 32, 128, 255);
			instantRepair.BorderColor = Color.FromArgb(255, 32, 128, 255);
			instantRepair.LegendText = "高速修复材";
			instantRepair.YAxisType = AxisType.Secondary;


			//データ設定
			{
				var record = GetRecords();

				ResourceRecord.ResourceElement prev = null;

				if (record.Any())
				{
					prev = record.First();
					foreach (var r in record)
					{

						if (ShouldSkipRecord(r.Date - prev.Date))
							continue;

						double[] ys = new double[] {
							r.Fuel - prev.Fuel,
							r.Ammo - prev.Ammo,
							r.Steel - prev.Steel,
							r.Bauxite - prev.Bauxite,
							r.InstantRepair - prev.InstantRepair };

						if (Menu_Option_DivideByDay.Checked)
						{
							for (int i = 0; i < 4; i++)
								ys[i] /= Math.Max((r.Date - prev.Date).TotalDays, 1.0 / 1440.0);
						}

						fuel.Points.AddXY(prev.Date.ToOADate(), ys[0]);
						ammo.Points.AddXY(prev.Date.ToOADate(), ys[1]);
						steel.Points.AddXY(prev.Date.ToOADate(), ys[2]);
						bauxite.Points.AddXY(prev.Date.ToOADate(), ys[3]);
						instantRepair.Points.AddXY(prev.Date.ToOADate(), ys[4]);

						fuel.Points.AddXY(r.Date.ToOADate(), ys[0]);
						ammo.Points.AddXY(r.Date.ToOADate(), ys[1]);
						steel.Points.AddXY(r.Date.ToOADate(), ys[2]);
						bauxite.Points.AddXY(r.Date.ToOADate(), ys[3]);
						instantRepair.Points.AddXY(r.Date.ToOADate(), ys[4]);


						prev = r;
					}
				}
			}


			SetYBounds();
		}



		private void SetMaterialChart()
		{

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add("ResourceChartArea");
			area.AxisX = CreateAxisX(SelectedChartSpan);
			area.AxisY = CreateAxisY(50, 200);

			ResourceChart.Legends.Clear();
			var legend = ResourceChart.Legends.Add("ResourceLegend");
			legend.Font = Font;


			ResourceChart.Series.Clear();

			var instantConstruction = ResourceChart.Series.Add("ResourceSeries_InstantConstruction");
			var instantRepair = ResourceChart.Series.Add("ResourceSeries_InstantRepair");
			var developmentMaterial = ResourceChart.Series.Add("ResourceSeries_DevelopmentMaterial");
			var moddingMaterial = ResourceChart.Series.Add("ResourceSeries_ModdingMaterial");

			var setSeries = new Action<Series>(s =>
			{
				s.ChartType = SeriesChartType.Line;
				s.Font = Font;
				s.XValueType = ChartValueType.DateTime;
			});

			setSeries(instantConstruction);
			instantConstruction.Color = Color.FromArgb(255, 128, 0);
			instantConstruction.LegendText = "高速建造材";

			setSeries(instantRepair);
			instantRepair.Color = Color.FromArgb(0, 128, 0);
			instantRepair.LegendText = "高速修复材";

			setSeries(developmentMaterial);
			developmentMaterial.Color = Color.FromArgb(0, 0, 255);
			developmentMaterial.LegendText = "开发资材";

			setSeries(moddingMaterial);
			moddingMaterial.Color = Color.FromArgb(64, 64, 64);
			moddingMaterial.LegendText = "改修资材";


			//データ設定
			{
				var record = GetRecords();

				if (record.Any())
				{
					var prev = record.First();
					foreach (var r in record)
					{

						if (ShouldSkipRecord(r.Date - prev.Date))
							continue;

						instantConstruction.Points.AddXY(r.Date.ToOADate(), r.InstantConstruction);
						instantRepair.Points.AddXY(r.Date.ToOADate(), r.InstantRepair);
						developmentMaterial.Points.AddXY(r.Date.ToOADate(), r.DevelopmentMaterial);
						moddingMaterial.Points.AddXY(r.Date.ToOADate(), r.ModdingMaterial);

						prev = r;
					}
				}


				if (instantConstruction.Points.Count > 0)
				{
					int min = (int)new[] { instantConstruction.Points.Min(p => p.YValues[0]), instantRepair.Points.Min(p => p.YValues[0]), developmentMaterial.Points.Min(p => p.YValues[0]), moddingMaterial.Points.Min(p => p.YValues[0]) }.Min();
					area.AxisY.Minimum = Math.Floor(min / 200.0) * 200;

					int max = (int)new[] { instantConstruction.Points.Max(p => p.YValues[0]), instantRepair.Points.Max(p => p.YValues[0]), developmentMaterial.Points.Max(p => p.YValues[0]), moddingMaterial.Points.Max(p => p.YValues[0]) }.Max();
					area.AxisY.Maximum = Math.Ceiling(max / 200.0) * 200;
				}
			}


			SetYBounds();
		}


		private void SetMateialDiffChart()
		{

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add("ResourceChartArea");
			area.AxisX = CreateAxisX(SelectedChartSpan);
			area.AxisY = CreateAxisY(5, 20);

			ResourceChart.Legends.Clear();
			var legend = ResourceChart.Legends.Add("ResourceLegend");
			legend.Font = Font;


			ResourceChart.Series.Clear();

			var instantConstruction = ResourceChart.Series.Add("ResourceSeries_InstantConstruction");
			var instantRepair = ResourceChart.Series.Add("ResourceSeries_InstantRepair");
			var developmentMaterial = ResourceChart.Series.Add("ResourceSeries_DevelopmentMaterial");
			var moddingMaterial = ResourceChart.Series.Add("ResourceSeries_ModdingMaterial");

			var setSeries = new Action<Series>(s =>
			{
				s.ChartType = SeriesChartType.Area;
				//s.SetCustomProperty( "PointWidth", "1.0" );		//棒グラフの幅
				//s.Enabled = false;	//表示するか
				s.Font = Font;
				s.XValueType = ChartValueType.DateTime;
			});

			setSeries(instantConstruction);
			instantConstruction.Color = Color.FromArgb(64, 255, 128, 0);
			instantConstruction.BorderColor = Color.FromArgb(255, 255, 128, 0);
			instantConstruction.LegendText = "高速建造材";

			setSeries(instantRepair);
			instantRepair.Color = Color.FromArgb(64, 0, 128, 0);
			instantRepair.BorderColor = Color.FromArgb(255, 0, 128, 0);
			instantRepair.LegendText = "高速修复材";

			setSeries(developmentMaterial);
			developmentMaterial.Color = Color.FromArgb(64, 0, 0, 255);
			developmentMaterial.BorderColor = Color.FromArgb(255, 0, 0, 255);
			developmentMaterial.LegendText = "开发资材";

			setSeries(moddingMaterial);
			moddingMaterial.Color = Color.FromArgb(64, 64, 64, 64);
			moddingMaterial.BorderColor = Color.FromArgb(255, 64, 64, 64);
			moddingMaterial.LegendText = "改修资材";


			//データ設定
			{
				var record = GetRecords();

				ResourceRecord.ResourceElement prev = null;

				if (record.Any())
				{
					prev = record.First();
					foreach (var r in record)
					{

						if (ShouldSkipRecord(r.Date - prev.Date))
							continue;

						double[] ys = new double[] {
							r.InstantConstruction - prev.InstantConstruction ,
							r.InstantRepair - prev.InstantRepair,
							r.DevelopmentMaterial - prev.DevelopmentMaterial ,
							r.ModdingMaterial - prev.ModdingMaterial };

						if (Menu_Option_DivideByDay.Checked)
						{
							for (int i = 0; i < 4; i++)
								ys[i] /= Math.Max((r.Date - prev.Date).TotalDays, 1.0 / 1440.0);
						}

						instantConstruction.Points.AddXY(prev.Date.ToOADate(), ys[0]);
						instantRepair.Points.AddXY(prev.Date.ToOADate(), ys[1]);
						developmentMaterial.Points.AddXY(prev.Date.ToOADate(), ys[2]);
						moddingMaterial.Points.AddXY(prev.Date.ToOADate(), ys[3]);

						instantConstruction.Points.AddXY(r.Date.ToOADate(), ys[0]);
						instantRepair.Points.AddXY(r.Date.ToOADate(), ys[1]);
						developmentMaterial.Points.AddXY(r.Date.ToOADate(), ys[2]);
						moddingMaterial.Points.AddXY(r.Date.ToOADate(), ys[3]);

						prev = r;
					}
				}


				if (instantConstruction.Points.Count > 0)
				{
					int min = (int)new[] { instantConstruction.Points.Min(p => p.YValues[0]), instantRepair.Points.Min(p => p.YValues[0]), developmentMaterial.Points.Min(p => p.YValues[0]), moddingMaterial.Points.Min(p => p.YValues[0]) }.Min();
					area.AxisY.Minimum = Math.Floor(min / 20.0) * 20;

					int max = (int)new[] { instantConstruction.Points.Max(p => p.YValues[0]), instantRepair.Points.Max(p => p.YValues[0]), developmentMaterial.Points.Max(p => p.YValues[0]), moddingMaterial.Points.Max(p => p.YValues[0]) }.Max();
					area.AxisY.Maximum = Math.Ceiling(max / 20.0) * 20;
				}
			}


			SetYBounds();
		}



		private void SetExperienceChart()
		{

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add("ResourceChartArea");
			area.AxisX = CreateAxisX(SelectedChartSpan);
			area.AxisY = CreateAxisY(20000);

			ResourceChart.Legends.Clear();
			var legend = ResourceChart.Legends.Add("ResourceLegend");
			legend.Font = Font;


			ResourceChart.Series.Clear();

			var exp = ResourceChart.Series.Add("ResourceSeries_Experience");

			var setSeries = new Action<Series>(s =>
			{
				s.ChartType = SeriesChartType.Line;
				s.Font = Font;
				s.XValueType = ChartValueType.DateTime;
			});

			setSeries(exp);
			exp.Color = Color.FromArgb(0, 0, 255);
			exp.LegendText = "提督经验值";


			//データ設定
			{
				var record = GetRecords();

				if (record.Any())
				{
					var prev = record.First();
					foreach (var r in record)
					{

						if (ShouldSkipRecord(r.Date - prev.Date))
							continue;

						exp.Points.AddXY(r.Date.ToOADate(), r.HQExp);
						prev = r;
					}
				}


				if (exp.Points.Count > 0)
				{
					int min = (int)exp.Points.Min(p => p.YValues[0]);
					area.AxisY.Minimum = Math.Floor(min / 100000.0) * 100000;

					int max = (int)exp.Points.Max(p => p.YValues[0]);
					area.AxisY.Maximum = Math.Ceiling(max / 100000.0) * 100000;
				}
			}


			SetYBounds();
		}


		private void SetExperienceDiffChart()
		{

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add("ResourceChartArea");
			area.AxisX = CreateAxisX(SelectedChartSpan);
			area.AxisY = CreateAxisY(2000);

			ResourceChart.Legends.Clear();
			var legend = ResourceChart.Legends.Add("ResourceLegend");
			legend.Font = Font;


			ResourceChart.Series.Clear();

			var exp = ResourceChart.Series.Add("ResourceSeries_Experience");


			var setSeries = new Action<Series>(s =>
			{
				s.ChartType = SeriesChartType.Area;
				//s.SetCustomProperty( "PointWidth", "1.0" );		//棒グラフの幅
				//s.Enabled = false;	//表示するか
				s.Font = Font;
				s.XValueType = ChartValueType.DateTime;
			});

			setSeries(exp);
			exp.Color = Color.FromArgb(64, 0, 0, 255);
			exp.BorderColor = Color.FromArgb(255, 0, 0, 255);
			exp.LegendText = "提督经验值";


			//データ設定
			{
				var record = GetRecords();

				ResourceRecord.ResourceElement prev = null;

				if (record.Any())
				{
					prev = record.First();
					foreach (var r in record)
					{

						if (ShouldSkipRecord(r.Date - prev.Date))
							continue;

						double ys = r.HQExp - prev.HQExp;

						if (Menu_Option_DivideByDay.Checked)
							ys /= Math.Max((r.Date - prev.Date).TotalDays, 1.0 / 1440.0);

						exp.Points.AddXY(prev.Date.ToOADate(), ys);

						exp.Points.AddXY(r.Date.ToOADate(), ys);

						prev = r;
					}
				}


				if (exp.Points.Count > 0)
				{
					int min = (int)exp.Points.Min(p => p.YValues[0]);
					area.AxisY.Minimum = Math.Floor(min / 10000.0) * 10000;

					int max = (int)exp.Points.Max(p => p.YValues[0]);
					area.AxisY.Maximum = Math.Ceiling(max / 10000.0) * 10000;
				}
			}


			SetYBounds();
		}


		private Axis CreateAxisX(ChartSpan span)
		{

			Axis axis = new Axis();

			switch (span)
			{
				case ChartSpan.Day:
					axis.Interval = 2;
					axis.IntervalOffsetType = DateTimeIntervalType.Hours;
					axis.IntervalType = DateTimeIntervalType.Hours;
					axis.LabelStyle.Format = "MM/dd HH:mm";
					break;
				case ChartSpan.Week:
				case ChartSpan.WeekFirst:
					axis.Interval = 12;
					axis.IntervalOffsetType = DateTimeIntervalType.Hours;
					axis.IntervalType = DateTimeIntervalType.Hours;
					axis.LabelStyle.Format = "MM/dd HH:mm";
					break;
				case ChartSpan.Month:
				case ChartSpan.MonthFirst:
					axis.Interval = 3;
					axis.IntervalOffsetType = DateTimeIntervalType.Days;
					axis.IntervalType = DateTimeIntervalType.Days;
					axis.LabelStyle.Format = "yyyy/MM/dd";
					break;
				case ChartSpan.Season:
				case ChartSpan.SeasonFirst:
					axis.Interval = 7;
					axis.IntervalOffsetType = DateTimeIntervalType.Days;
					axis.IntervalType = DateTimeIntervalType.Days;
					axis.LabelStyle.Format = "yyyy/MM/dd";
					break;
				case ChartSpan.Year:
				case ChartSpan.YearFirst:
				case ChartSpan.All:
					axis.Interval = 1;
					axis.IntervalOffsetType = DateTimeIntervalType.Months;
					axis.IntervalType = DateTimeIntervalType.Months;
					axis.LabelStyle.Format = "yyyy/MM/dd";
					break;
			}

			axis.LabelStyle.Font = Font;
			axis.MajorGrid.LineColor = Color.FromArgb(192, 192, 192);

			return axis;
		}


		private Axis CreateAxisY(int minorInterval, int majorInterval)
		{

			Axis axis = new Axis();

			axis.LabelStyle.Font = Font;
			axis.IsStartedFromZero = true;
			axis.Interval = majorInterval;
			axis.MajorGrid.LineColor = Color.FromArgb(192, 192, 192);
			axis.MinorGrid.Enabled = true;
			axis.MinorGrid.Interval = minorInterval;
			axis.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
			axis.MinorGrid.LineColor = Color.FromArgb(224, 224, 224);

			return axis;
		}

		private Axis CreateAxisY(int interval)
		{
			return CreateAxisY(interval, interval * 5);
		}



		private void ResourceChart_GetToolTipText(object sender, ToolTipEventArgs e)
		{

			if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
			{
				var dp = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];

				switch (SelectedChartType)
				{
					case ChartType.Resource:
					case ChartType.Material:
					case ChartType.Experience:
						e.Text = string.Format("{0:yyyy\\/MM\\/dd HH\\:mm}\n{1} {2:F0}",
							DateTime.FromOADate(dp.XValue),
							e.HitTestResult.Series.LegendText,
							dp.YValues[0]);
						break;
					case ChartType.ResourceDiff:
					case ChartType.MaterialDiff:
					case ChartType.ExperienceDiff:
						e.Text = string.Format("{0:yyyy\\/MM\\/dd HH\\:mm}\n{1} {2:+0;-0;±0}{3}",
							DateTime.FromOADate(dp.XValue),
							e.HitTestResult.Series.LegendText,
							dp.YValues[0],
							Menu_Option_DivideByDay.Checked ? " / day" : "");
						break;
				}
			}

		}


		private void SwitchMenuStrip(ToolStripMenuItem parent, int index)
		{

			//すべての子アイテムに対して
			var items = parent.DropDownItems.OfType<ToolStripMenuItem>();
			int c = 0;

			foreach (var item in items)
			{
				if (index == c)
				{

					item.Checked = true;

				}
				else
				{

					item.Checked = false;
				}

				c++;
			}

			parent.Tag = index;
		}


		private int GetSelectedMenuStripIndex(ToolStripMenuItem parent)
		{
			return parent.Tag as int? ?? -1;
		}


		private void UpdateChart()
		{

			switch (SelectedChartType)
			{
				case ChartType.Resource:
					SetResourceChart();
					break;
				case ChartType.ResourceDiff:
					SetResourceDiffChart();
					break;
				case ChartType.Material:
					SetMaterialChart();
					break;
				case ChartType.MaterialDiff:
					SetMateialDiffChart();
					break;
				case ChartType.Experience:
					SetExperienceChart();
					break;
				case ChartType.ExperienceDiff:
					SetExperienceDiffChart();
					break;
			}

		}

		private IEnumerable<ResourceRecord.ResourceElement> GetRecords()
		{

			var border = DateTime.MinValue;
			var now = DateTime.Now;

			switch (SelectedChartSpan)
			{
				case ChartSpan.Day:
					border = now.AddDays(-1);
					break;
				case ChartSpan.Week:
					border = now.AddDays(-7);
					break;
				case ChartSpan.Month:
					border = now.AddMonths(-1);
					break;
				case ChartSpan.Season:
					border = now.AddMonths(-3);
					break;
				case ChartSpan.Year:
					border = now.AddYears(-1);
					break;

				case ChartSpan.WeekFirst:
					border = now.AddDays(now.DayOfWeek == DayOfWeek.Sunday ? -6 : (1 - (int)now.DayOfWeek));
					break;
				case ChartSpan.MonthFirst:
					border = new DateTime(now.Year, now.Month, 1);
					break;
				case ChartSpan.SeasonFirst:
					{
						int m = now.Month / 3 * 3;
						if (m == 0)
							m = 12;
						border = new DateTime(now.Year - (now.Month < 3 ? 1 : 0), m, 1);
					}
					break;
				case ChartSpan.YearFirst:
					border = new DateTime(now.Year, 1, 1);
					break;
			}

			foreach (var r in RecordManager.Instance.Resource.Record)
			{
				if (r.Date >= border)
					yield return r;
			}

			var material = KCDatabase.Instance.Material;
			var admiral = KCDatabase.Instance.Admiral;
			if (material.IsAvailable && admiral.IsAvailable)
			{
				yield return new ResourceRecord.ResourceElement(
					material.Fuel, material.Ammo, material.Steel, material.Bauxite,
					material.InstantConstruction, material.InstantRepair, material.DevelopmentMaterial, material.ModdingMaterial,
					admiral.Level, admiral.Exp);
			}

		}


		private bool ShouldSkipRecord(TimeSpan span)
		{

			if (Menu_Option_ShowAllData.Checked)
				return false;

			if (span.Ticks == 0)        //初回のデータ( prev == First )は無視しない
				return false;

			switch (SelectedChartSpan)
			{
				case ChartSpan.Day:
				case ChartSpan.Week:
				case ChartSpan.WeekFirst:
				default:
					return false;

				case ChartSpan.Month:
				case ChartSpan.MonthFirst:
					return span.TotalHours < 12.0;

				case ChartSpan.Season:
				case ChartSpan.SeasonFirst:
				case ChartSpan.Year:
				case ChartSpan.YearFirst:
				case ChartSpan.All:
					return span.TotalDays < 1.0;
			}

		}



		private void SetYBounds(double min, double max)
		{

			int order = (int)Math.Log10(Math.Max(max - min, 1));
			double powered = Math.Pow(10, order);
			double unitbase = Math.Round((max - min) / powered);
			double unit = powered * (
				unitbase < 2 ? 0.2 :
				unitbase < 5 ? 0.5 :
				unitbase < 7 ? 1.0 : 2.0);

			ResourceChart.ChartAreas[0].AxisY.Minimum = Math.Floor(min / unit) * unit;
			ResourceChart.ChartAreas[0].AxisY.Maximum = Math.Ceiling(max / unit) * unit;

			ResourceChart.ChartAreas[0].AxisY.Interval = unit;
			ResourceChart.ChartAreas[0].AxisY.MinorGrid.Interval = unit / 2;

			if (ResourceChart.Series.Where(s => s.Enabled).Any(s => s.YAxisType == AxisType.Secondary)) {
				ResourceChart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
				if (ResourceChart.Series.Count(s => s.Enabled) == 1) {
					ResourceChart.ChartAreas[0].AxisY2.MajorGrid.Enabled = true;
					ResourceChart.ChartAreas[0].AxisY2.MinorGrid.Enabled = true;
				} else {
					ResourceChart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
					ResourceChart.ChartAreas[0].AxisY2.MinorGrid.Enabled = false;
				}
				ResourceChart.ChartAreas[0].AxisY2.Minimum = ResourceChart.ChartAreas[0].AxisY.Minimum / 100;
				ResourceChart.ChartAreas[0].AxisY2.Maximum = ResourceChart.ChartAreas[0].AxisY.Maximum / 100;
				ResourceChart.ChartAreas[0].AxisY2.Interval = unit / 100;
				ResourceChart.ChartAreas[0].AxisY2.MinorGrid.Interval = unit / 200;
			} else {
				ResourceChart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
			}

		}

		private void SetYBounds()
		{
			SetYBounds(
				!ResourceChart.Series.Any(s => s.Enabled) || SelectedChartType == ChartType.ExperienceDiff ? 0 : ResourceChart.Series.Where(s => s.Enabled).Select(s => s.YAxisType == AxisType.Secondary ? s.Points.Min(p => p.YValues[0] * 100) : s.Points.Min(p => p.YValues[0])).Min(),
				!ResourceChart.Series.Any(s => s.Enabled) ? 0 : ResourceChart.Series.Where(s => s.Enabled).Select(s => s.YAxisType == AxisType.Secondary ? s.Points.Max(p => p.YValues[0] * 100) : s.Points.Max(p => p.YValues[0])).Max()
				);
		}


		private void ResourceChart_CustomizeLegend(object sender, CustomizeLegendEventArgs e)
		{

			e.LegendItems.Clear();

			foreach (var series in ResourceChart.Series)
			{

				var legendItem = new LegendItem
				{
					SeriesName = series.Name,
					ImageStyle = LegendImageStyle.Rectangle,
					BorderColor = Color.Empty,
					Name = series.Name + "_legendItem"
				};

				legendItem.Cells.Add(LegendCellType.SeriesSymbol, "", ContentAlignment.MiddleCenter);
				legendItem.Cells.Add(LegendCellType.Text, series.LegendText, ContentAlignment.MiddleLeft);

				var col = series.BorderColor != Color.Empty ? series.BorderColor : series.Color;

				if (series.Enabled)
				{
					legendItem.Color = col;
					legendItem.Cells[1].ForeColor = SystemColors.ControlText;
				}
				else
				{
					legendItem.Color = Color.FromArgb(col.A / 4, col);
					legendItem.Cells[1].ForeColor = SystemColors.GrayText;
				}
				e.LegendItems.Add(legendItem);
			}
		}


		private void ResourceChart_MouseDown(object sender, MouseEventArgs e)
		{

			if (e.Button != System.Windows.Forms.MouseButtons.Left)
				return;

			var hittest = ResourceChart.HitTest(e.X, e.Y, ChartElementType.LegendItem);

			if (hittest.Object != null)
			{

				var legend = (LegendItem)hittest.Object;
				ResourceChart.Series[legend.SeriesName].Enabled ^= true;
			}

			SetYBounds();
		}






		private void Menu_Graph_Resource_Click(object sender, EventArgs e)
		{
			SwitchMenuStrip(Menu_Graph, 0);
			UpdateChart();
		}

		private void Menu_Graph_ResourceDiff_Click(object sender, EventArgs e)
		{
			SwitchMenuStrip(Menu_Graph, 1);
			UpdateChart();
		}

		private void Menu_Graph_Material_Click(object sender, EventArgs e)
		{
			SwitchMenuStrip(Menu_Graph, 2);
			UpdateChart();
		}

		private void Menu_Graph_MaterialDiff_Click(object sender, EventArgs e)
		{
			SwitchMenuStrip(Menu_Graph, 3);
			UpdateChart();
		}

		private void Menu_Graph_Experience_Click(object sender, EventArgs e)
		{
			SwitchMenuStrip(Menu_Graph, 4);
			UpdateChart();
		}

		private void Menu_Graph_ExperienceDiff_Click(object sender, EventArgs e)
		{
			SwitchMenuStrip(Menu_Graph, 5);
			UpdateChart();
		}


		private void Menu_Span_Menu_Click(object sender, EventArgs e)
		{
			SwitchMenuStrip(Menu_Span, (int)((ToolStripMenuItem)sender).Tag);
			UpdateChart();
		}


		private void Menu_Option_ShowAllData_Click(object sender, EventArgs e)
		{
			UpdateChart();
		}

		private void Menu_Option_DivideByDay_Click(object sender, EventArgs e)
		{
			UpdateChart();
		}



		private void Menu_File_SaveImage_Click(object sender, EventArgs e)
		{

			if (SaveImageDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				try
				{

					ResourceChart.SaveImage(SaveImageDialog.FileName, ChartImageFormat.Png);

				}
				catch (Exception ex)
				{
					Utility.ErrorReporter.SendErrorReport(ex, "資源チャート画像の保存に失敗しました。");
				}
			}
		}



		private void DialogResourceChart_FormClosed(object sender, FormClosedEventArgs e)
		{
			ResourceManager.DestroyIcon(Icon);
		}


	}

}
