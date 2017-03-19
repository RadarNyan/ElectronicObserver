﻿namespace ElectronicObserver.Window.Dialog {
	partial class DialogResourceChart {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_File_SaveImage = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Graph = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Graph_Resource = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Graph_ResourceDiff = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.Menu_Graph_Material = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Graph_MaterialDiff = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.Menu_Graph_Experience = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Graph_ExperienceDiff = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Span = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Span_Day = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Span_Week = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Span_Month = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Span_Season = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Span_Year = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Span_All = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Option = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Option_ShowAllData = new System.Windows.Forms.ToolStripMenuItem();
			this.Menu_Option_DivideByDay = new System.Windows.Forms.ToolStripMenuItem();
			this.ResourceChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.SaveImageDialog = new System.Windows.Forms.SaveFileDialog();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ResourceChart)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Graph,
            this.Menu_Span,
            this.Menu_Option});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(774, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// Menu_File
			// 
			this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File_SaveImage});
			this.Menu_File.Name = "Menu_File";
			this.Menu_File.Size = new System.Drawing.Size(70, 20);
			this.Menu_File.Text = "文件(&F)";
			// 
			// Menu_File_SaveImage
			// 
			this.Menu_File_SaveImage.Name = "Menu_File_SaveImage";
			this.Menu_File_SaveImage.Size = new System.Drawing.Size(196, 22);
			this.Menu_File_SaveImage.Text = "保存图表图像(&S)...";
			this.Menu_File_SaveImage.Click += new System.EventHandler(this.Menu_File_SaveImage_Click);
			// 
			// Menu_Graph
			// 
			this.Menu_Graph.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Graph_Resource,
            this.Menu_Graph_ResourceDiff,
            this.toolStripSeparator1,
            this.Menu_Graph_Material,
            this.Menu_Graph_MaterialDiff,
            this.toolStripSeparator2,
            this.Menu_Graph_Experience,
            this.Menu_Graph_ExperienceDiff});
			this.Menu_Graph.Name = "Menu_Graph";
			this.Menu_Graph.Size = new System.Drawing.Size(97, 20);
			this.Menu_Graph.Text = "选择分类(&G)";
			// 
			// Menu_Graph_Resource
			// 
			this.Menu_Graph_Resource.Name = "Menu_Graph_Resource";
			this.Menu_Graph_Resource.Size = new System.Drawing.Size(162, 22);
			this.Menu_Graph_Resource.Text = "资源(&R)";
			this.Menu_Graph_Resource.Click += new System.EventHandler(this.Menu_Graph_Resource_Click);
			// 
			// Menu_Graph_ResourceDiff
			// 
			this.Menu_Graph_ResourceDiff.Name = "Menu_Graph_ResourceDiff";
			this.Menu_Graph_ResourceDiff.Size = new System.Drawing.Size(162, 22);
			this.Menu_Graph_ResourceDiff.Text = "资源差(&E)";
			this.Menu_Graph_ResourceDiff.Click += new System.EventHandler(this.Menu_Graph_ResourceDiff_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
			// 
			// Menu_Graph_Material
			// 
			this.Menu_Graph_Material.Name = "Menu_Graph_Material";
			this.Menu_Graph_Material.Size = new System.Drawing.Size(162, 22);
			this.Menu_Graph_Material.Text = "资材(&M)";
			this.Menu_Graph_Material.Click += new System.EventHandler(this.Menu_Graph_Material_Click);
			// 
			// Menu_Graph_MaterialDiff
			// 
			this.Menu_Graph_MaterialDiff.Name = "Menu_Graph_MaterialDiff";
			this.Menu_Graph_MaterialDiff.Size = new System.Drawing.Size(162, 22);
			this.Menu_Graph_MaterialDiff.Text = "资材差(&A)";
			this.Menu_Graph_MaterialDiff.Click += new System.EventHandler(this.Menu_Graph_MaterialDiff_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(159, 6);
			// 
			// Menu_Graph_Experience
			// 
			this.Menu_Graph_Experience.Name = "Menu_Graph_Experience";
			this.Menu_Graph_Experience.Size = new System.Drawing.Size(162, 22);
			this.Menu_Graph_Experience.Text = "经验值(&E)";
			this.Menu_Graph_Experience.Click += new System.EventHandler(this.Menu_Graph_Experience_Click);
			// 
			// Menu_Graph_ExperienceDiff
			// 
			this.Menu_Graph_ExperienceDiff.Name = "Menu_Graph_ExperienceDiff";
			this.Menu_Graph_ExperienceDiff.Size = new System.Drawing.Size(162, 22);
			this.Menu_Graph_ExperienceDiff.Text = "经验值差(&X)";
			this.Menu_Graph_ExperienceDiff.Click += new System.EventHandler(this.Menu_Graph_ExperienceDiff_Click);
			// 
			// Menu_Span
			// 
			this.Menu_Span.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Span_Day,
            this.Menu_Span_Week,
            this.Menu_Span_Month,
            this.Menu_Span_Season,
            this.Menu_Span_Year,
            this.Menu_Span_All});
			this.Menu_Span.Name = "Menu_Span";
			this.Menu_Span.Size = new System.Drawing.Size(61, 20);
			this.Menu_Span.Text = "范围(&S)";
			// 
			// Menu_Span_Day
			// 
			this.Menu_Span_Day.Name = "Menu_Span_Day";
			this.Menu_Span_Day.Size = new System.Drawing.Size(121, 22);
			this.Menu_Span_Day.Text = "日(&D)";
			this.Menu_Span_Day.Click += new System.EventHandler(this.Menu_Span_Day_Click);
			// 
			// Menu_Span_Week
			// 
			this.Menu_Span_Week.Name = "Menu_Span_Week";
			this.Menu_Span_Week.Size = new System.Drawing.Size(121, 22);
			this.Menu_Span_Week.Text = "周(&W)";
			this.Menu_Span_Week.Click += new System.EventHandler(this.Menu_Span_Week_Click);
			// 
			// Menu_Span_Month
			// 
			this.Menu_Span_Month.Name = "Menu_Span_Month";
			this.Menu_Span_Month.Size = new System.Drawing.Size(121, 22);
			this.Menu_Span_Month.Text = "月(&M)";
			this.Menu_Span_Month.Click += new System.EventHandler(this.Menu_Span_Month_Click);
			// 
			// Menu_Span_Season
			// 
			this.Menu_Span_Season.Name = "Menu_Span_Season";
			this.Menu_Span_Season.Size = new System.Drawing.Size(121, 22);
			this.Menu_Span_Season.Text = "三个月(&S)";
			this.Menu_Span_Season.Click += new System.EventHandler(this.Menu_Span_Season_Click);
			// 
			// Menu_Span_Year
			// 
			this.Menu_Span_Year.Name = "Menu_Span_Year";
			this.Menu_Span_Year.Size = new System.Drawing.Size(121, 22);
			this.Menu_Span_Year.Text = "年(&Y)";
			this.Menu_Span_Year.Click += new System.EventHandler(this.Menu_Span_Year_Click);
			// 
			// Menu_Span_All
			// 
			this.Menu_Span_All.Name = "Menu_Span_All";
			this.Menu_Span_All.Size = new System.Drawing.Size(121, 22);
			this.Menu_Span_All.Text = "全部(&A)";
			this.Menu_Span_All.Click += new System.EventHandler(this.Menu_Span_All_Click);
			// 
			// Menu_Option
			// 
			this.Menu_Option.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Option_ShowAllData,
            this.Menu_Option_DivideByDay});
			this.Menu_Option.Name = "Menu_Option";
			this.Menu_Option.Size = new System.Drawing.Size(81, 20);
			this.Menu_Option.Text = "选项(&O)";
			// 
			// Menu_Option_ShowAllData
			// 
			this.Menu_Option_ShowAllData.CheckOnClick = true;
			this.Menu_Option_ShowAllData.Name = "Menu_Option_ShowAllData";
			this.Menu_Option_ShowAllData.Size = new System.Drawing.Size(185, 22);
			this.Menu_Option_ShowAllData.Text = "显示所有数据";
			this.Menu_Option_ShowAllData.Click += new System.EventHandler(this.Menu_Option_ShowAllData_Click);
			// 
			// Menu_Option_DivideByDay
			// 
			this.Menu_Option_DivideByDay.CheckOnClick = true;
			this.Menu_Option_DivideByDay.Name = "Menu_Option_DivideByDay";
			this.Menu_Option_DivideByDay.Size = new System.Drawing.Size(185, 22);
			this.Menu_Option_DivideByDay.Text = "以一天为单位平均计算";
			this.Menu_Option_DivideByDay.Click += new System.EventHandler(this.Menu_Option_DivideByDay_Click);
			// 
			// ResourceChart
			// 
			this.ResourceChart.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ResourceChart.Location = new System.Drawing.Point(0, 24);
			this.ResourceChart.Name = "ResourceChart";
			this.ResourceChart.Size = new System.Drawing.Size(774, 505);
			this.ResourceChart.TabIndex = 1;
			this.ResourceChart.Text = "資源チャート";
			this.ResourceChart.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.ResourceChart_GetToolTipText);
			this.ResourceChart.CustomizeLegend += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.CustomizeLegendEventArgs>(this.ResourceChart_CustomizeLegend);
			this.ResourceChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ResourceChart_MouseDown);
			// 
			// SaveImageDialog
			// 
			this.SaveImageDialog.Filter = "PNG|*.png|File|*";
			this.SaveImageDialog.Title = "保存图表图像";
			// 
			// DialogResourceChart
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(774, 529);
			this.Controls.Add(this.ResourceChart);
			this.Controls.Add(this.menuStrip1);
			this.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "DialogResourceChart";
			this.Text = "资源图表";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogResourceChart_FormClosed);
			this.Load += new System.EventHandler(this.DialogResourceChart_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ResourceChart)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem Menu_Graph;
		private System.Windows.Forms.ToolStripMenuItem Menu_Graph_Resource;
		private System.Windows.Forms.ToolStripMenuItem Menu_Graph_Material;
		private System.Windows.Forms.ToolStripMenuItem Menu_Graph_Experience;
		private System.Windows.Forms.DataVisualization.Charting.Chart ResourceChart;
		private System.Windows.Forms.ToolStripMenuItem Menu_Graph_ResourceDiff;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem Menu_Graph_MaterialDiff;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem Menu_Graph_ExperienceDiff;
		private System.Windows.Forms.ToolStripMenuItem Menu_Span;
		private System.Windows.Forms.ToolStripMenuItem Menu_Span_Day;
		private System.Windows.Forms.ToolStripMenuItem Menu_Span_Month;
		private System.Windows.Forms.ToolStripMenuItem Menu_Span_Week;
		private System.Windows.Forms.ToolStripMenuItem Menu_Span_Year;
		private System.Windows.Forms.ToolStripMenuItem Menu_Span_All;
		private System.Windows.Forms.ToolStripMenuItem Menu_Span_Season;
		private System.Windows.Forms.ToolStripMenuItem Menu_Option;
		private System.Windows.Forms.ToolStripMenuItem Menu_Option_ShowAllData;
		private System.Windows.Forms.ToolStripMenuItem Menu_Option_DivideByDay;
		private System.Windows.Forms.ToolStripMenuItem Menu_File;
		private System.Windows.Forms.ToolStripMenuItem Menu_File_SaveImage;
		private System.Windows.Forms.SaveFileDialog SaveImageDialog;
	}
}