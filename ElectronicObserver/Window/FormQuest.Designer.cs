﻿namespace ElectronicObserver.Window {
	partial class FormQuest {
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.QuestView = new System.Windows.Forms.DataGridView();
			this.QuestView_State = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.QuestView_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.QuestView_Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.QuestView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.QuestView_Progress = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MenuProgress = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuProgress_Increment = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuProgress_Decrement = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuProgress_Reset = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuMain_ColumnFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain_ColumnFilter_State = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain_ColumnFilter_Type = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain_ColumnFilter_Category = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain_ColumnFilter_Name = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain_ColumnFilter_Progress = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuMain_Initialize = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.MenuMain_QuestFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain_ShowWeekly = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain_ShowDaily = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain_ShowOnce = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuMain_ShowRunningOnly = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain_ShowMonthly = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain_ShowOther = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMain_GoogleQuest = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			((System.ComponentModel.ISupportInitialize)(this.QuestView)).BeginInit();
			this.MenuProgress.SuspendLayout();
			this.MenuMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// QuestView
			// 
			this.QuestView.AllowUserToAddRows = false;
			this.QuestView.AllowUserToDeleteRows = false;
			this.QuestView.AllowUserToResizeRows = false;
			this.QuestView.BackgroundColor = System.Drawing.SystemColors.Control;
			this.QuestView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.QuestView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.QuestView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.QuestView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.QuestView_State,
            this.QuestView_Type,
            this.QuestView_Category,
            this.QuestView_Name,
            this.QuestView_Progress});
			this.QuestView.ContextMenuStrip = this.MenuMain;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.QuestView.DefaultCellStyle = dataGridViewCellStyle4;
			this.QuestView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.QuestView.Location = new System.Drawing.Point(0, 0);
			this.QuestView.MultiSelect = false;
			this.QuestView.Name = "QuestView";
			this.QuestView.ReadOnly = true;
			this.QuestView.RowHeadersVisible = false;
			this.QuestView.RowTemplate.Height = 21;
			this.QuestView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.QuestView.ShowCellToolTips = false;
			this.QuestView.Size = new System.Drawing.Size(300, 200);
			this.QuestView.TabIndex = 0;
			this.QuestView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.QuestView_CellFormatting);
			this.QuestView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.QuestView_CellMouseDown);
			this.QuestView.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.QuestView_CellMouseEnter);
			this.QuestView.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.QuestView_CellMouseLeave);
			this.QuestView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.QuestView_CellPainting);
			this.QuestView.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.QuestView_ColumnWidthChanged);
			this.QuestView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.QuestView_SortCompare);
			this.QuestView.Sorted += new System.EventHandler(this.QuestView_Sorted);
			// 
			// QuestView_State
			// 
			this.QuestView_State.FalseValue = "";
			this.QuestView_State.HeaderText = "";
			this.QuestView_State.IndeterminateValue = "";
			this.QuestView_State.Name = "QuestView_State";
			this.QuestView_State.ReadOnly = true;
			this.QuestView_State.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.QuestView_State.ThreeState = true;
			this.QuestView_State.TrueValue = "";
			this.QuestView_State.Width = 24;
			// 
			// QuestView_Type
			// 
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.QuestView_Type.DefaultCellStyle = dataGridViewCellStyle2;
			this.QuestView_Type.HeaderText = "种";
			this.QuestView_Type.Name = "QuestView_Type";
			this.QuestView_Type.ReadOnly = true;
			this.QuestView_Type.Width = 20;
			// 
			// QuestView_Category
			// 
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.QuestView_Category.DefaultCellStyle = dataGridViewCellStyle3;
			this.QuestView_Category.HeaderText = "分类";
			this.QuestView_Category.Name = "QuestView_Category";
			this.QuestView_Category.ReadOnly = true;
			this.QuestView_Category.Width = 40;
			// 
			// QuestView_Name
			// 
			this.QuestView_Name.FillWeight = 200F;
			this.QuestView_Name.HeaderText = "任务名";
			this.QuestView_Name.Name = "QuestView_Name";
			this.QuestView_Name.ReadOnly = true;
			this.QuestView_Name.Width = 143;
			// 
			// QuestView_Progress
			// 
			this.QuestView_Progress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.QuestView_Progress.ContextMenuStrip = this.MenuProgress;
			this.QuestView_Progress.HeaderText = "进度";
			this.QuestView_Progress.Name = "QuestView_Progress";
			this.QuestView_Progress.ReadOnly = true;
			this.QuestView_Progress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// MenuProgress
			// 
			this.MenuProgress.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuProgress_Increment,
            this.MenuProgress_Decrement,
            this.toolStripSeparator3,
            this.MenuProgress_Reset});
			this.MenuProgress.Name = "MenuProgress";
			this.MenuProgress.Size = new System.Drawing.Size(149, 76);
			// 
			// MenuProgress_Increment
			// 
			this.MenuProgress_Increment.Name = "MenuProgress_Increment";
			this.MenuProgress_Increment.Size = new System.Drawing.Size(148, 22);
			this.MenuProgress_Increment.Text = "进度 +1(&I)";
			this.MenuProgress_Increment.Click += new System.EventHandler(this.MenuProgress_Increment_Click);
			// 
			// MenuProgress_Decrement
			// 
			this.MenuProgress_Decrement.Name = "MenuProgress_Decrement";
			this.MenuProgress_Decrement.Size = new System.Drawing.Size(148, 22);
			this.MenuProgress_Decrement.Text = "进度 -1(&D)";
			this.MenuProgress_Decrement.Click += new System.EventHandler(this.MenuProgress_Decrement_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(145, 6);
			// 
			// MenuProgress_Reset
			// 
			this.MenuProgress_Reset.Name = "MenuProgress_Reset";
			this.MenuProgress_Reset.Size = new System.Drawing.Size(148, 22);
			this.MenuProgress_Reset.Text = "重置进度(&R)";
			this.MenuProgress_Reset.Click += new System.EventHandler(this.MenuProgress_Reset_Click);
			// 
			// MenuMain
			// 
			this.MenuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuMain_QuestFilter,
            this.MenuMain_ColumnFilter,
            this.toolStripSeparator1,
            this.MenuMain_GoogleQuest,
            this.toolStripSeparator4,
            this.MenuMain_Initialize});
			this.MenuMain.Name = "MenuMain";
			this.MenuMain.Size = new System.Drawing.Size(204, 126);
			this.MenuMain.Opening += new System.ComponentModel.CancelEventHandler(this.MenuMain_Opening);
			// 
			// MenuMain_ColumnFilter
			// 
			this.MenuMain_ColumnFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuMain_ColumnFilter_State,
            this.MenuMain_ColumnFilter_Type,
            this.MenuMain_ColumnFilter_Category,
            this.MenuMain_ColumnFilter_Name,
            this.MenuMain_ColumnFilter_Progress});
			this.MenuMain_ColumnFilter.Name = "MenuMain_ColumnFilter";
			this.MenuMain_ColumnFilter.Size = new System.Drawing.Size(203, 22);
			this.MenuMain_ColumnFilter.Text = "显示列(&C)";
			// 
			// MenuMain_ColumnFilter_State
			// 
			this.MenuMain_ColumnFilter_State.CheckOnClick = true;
			this.MenuMain_ColumnFilter_State.Name = "MenuMain_ColumnFilter_State";
			this.MenuMain_ColumnFilter_State.Size = new System.Drawing.Size(152, 22);
			this.MenuMain_ColumnFilter_State.Text = "进行状态(&S)";
			this.MenuMain_ColumnFilter_State.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
			// 
			// MenuMain_ColumnFilter_Type
			// 
			this.MenuMain_ColumnFilter_Type.CheckOnClick = true;
			this.MenuMain_ColumnFilter_Type.Name = "MenuMain_ColumnFilter_Type";
			this.MenuMain_ColumnFilter_Type.Size = new System.Drawing.Size(152, 22);
			this.MenuMain_ColumnFilter_Type.Text = "任务种类(&T)";
			this.MenuMain_ColumnFilter_Type.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
			// 
			// MenuMain_ColumnFilter_Category
			// 
			this.MenuMain_ColumnFilter_Category.CheckOnClick = true;
			this.MenuMain_ColumnFilter_Category.Name = "MenuMain_ColumnFilter_Category";
			this.MenuMain_ColumnFilter_Category.Size = new System.Drawing.Size(152, 22);
			this.MenuMain_ColumnFilter_Category.Text = "任务分类(&C)";
			this.MenuMain_ColumnFilter_Category.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
			// 
			// MenuMain_ColumnFilter_Name
			// 
			this.MenuMain_ColumnFilter_Name.CheckOnClick = true;
			this.MenuMain_ColumnFilter_Name.Name = "MenuMain_ColumnFilter_Name";
			this.MenuMain_ColumnFilter_Name.Size = new System.Drawing.Size(152, 22);
			this.MenuMain_ColumnFilter_Name.Text = "任务名(&N)";
			this.MenuMain_ColumnFilter_Name.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
			// 
			// MenuMain_ColumnFilter_Progress
			// 
			this.MenuMain_ColumnFilter_Progress.CheckOnClick = true;
			this.MenuMain_ColumnFilter_Progress.Name = "MenuMain_ColumnFilter_Progress";
			this.MenuMain_ColumnFilter_Progress.Size = new System.Drawing.Size(152, 22);
			this.MenuMain_ColumnFilter_Progress.Text = "进度(&P)";
			this.MenuMain_ColumnFilter_Progress.Click += new System.EventHandler(this.MenuMain_ColumnFilter_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(200, 6);
			// 
			// MenuMain_Initialize
			// 
			this.MenuMain_Initialize.Name = "MenuMain_Initialize";
			this.MenuMain_Initialize.Size = new System.Drawing.Size(203, 22);
			this.MenuMain_Initialize.Text = "初始化(&I)";
			this.MenuMain_Initialize.Click += new System.EventHandler(this.MenuMain_Initialize_Click);
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 30000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
			// 
			// MenuMain_QuestFilter
			// 
			this.MenuMain_QuestFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuMain_ShowRunningOnly,
            this.toolStripSeparator2,
            this.MenuMain_ShowOnce,
            this.MenuMain_ShowDaily,
            this.MenuMain_ShowWeekly,
            this.MenuMain_ShowMonthly,
            this.MenuMain_ShowOther});
			this.MenuMain_QuestFilter.Name = "MenuMain_QuestFilter";
			this.MenuMain_QuestFilter.Size = new System.Drawing.Size(203, 22);
			this.MenuMain_QuestFilter.Text = "显示筛选(&Q)";
			// 
			// MenuMain_ShowWeekly
			// 
			this.MenuMain_ShowWeekly.CheckOnClick = true;
			this.MenuMain_ShowWeekly.Name = "MenuMain_ShowWeekly";
			this.MenuMain_ShowWeekly.Size = new System.Drawing.Size(204, 22);
			this.MenuMain_ShowWeekly.Text = "显示周常任务(&W)";
			this.MenuMain_ShowWeekly.Click += new System.EventHandler(this.MenuMain_ShowWeekly_Click);
			// 
			// MenuMain_ShowDaily
			// 
			this.MenuMain_ShowDaily.CheckOnClick = true;
			this.MenuMain_ShowDaily.Name = "MenuMain_ShowDaily";
			this.MenuMain_ShowDaily.Size = new System.Drawing.Size(204, 22);
			this.MenuMain_ShowDaily.Text = "显示日常任务(&D)";
			this.MenuMain_ShowDaily.Click += new System.EventHandler(this.MenuMain_ShowDaily_Click);
			// 
			// MenuMain_ShowOnce
			// 
			this.MenuMain_ShowOnce.CheckOnClick = true;
			this.MenuMain_ShowOnce.Name = "MenuMain_ShowOnce";
			this.MenuMain_ShowOnce.Size = new System.Drawing.Size(204, 22);
			this.MenuMain_ShowOnce.Text = "显示单次任务(&O)";
			this.MenuMain_ShowOnce.Click += new System.EventHandler(this.MenuMain_ShowOnce_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(201, 6);
			// 
			// MenuMain_ShowRunningOnly
			// 
			this.MenuMain_ShowRunningOnly.CheckOnClick = true;
			this.MenuMain_ShowRunningOnly.Name = "MenuMain_ShowRunningOnly";
			this.MenuMain_ShowRunningOnly.Size = new System.Drawing.Size(204, 22);
			this.MenuMain_ShowRunningOnly.Text = "只显示进行中任务(&R)";
			this.MenuMain_ShowRunningOnly.Click += new System.EventHandler(this.MenuMain_ShowRunningOnly_Click);
			// 
			// MenuMain_ShowMonthly
			// 
			this.MenuMain_ShowMonthly.CheckOnClick = true;
			this.MenuMain_ShowMonthly.Name = "MenuMain_ShowMonthly";
			this.MenuMain_ShowMonthly.Size = new System.Drawing.Size(204, 22);
			this.MenuMain_ShowMonthly.Text = "显示月常任务(&M)";
			this.MenuMain_ShowMonthly.Click += new System.EventHandler(this.MenuMain_ShowMonthly_Click);
			// 
			// MenuMain_ShowOther
			// 
			this.MenuMain_ShowOther.CheckOnClick = true;
			this.MenuMain_ShowOther.Name = "MenuMain_ShowOther";
			this.MenuMain_ShowOther.Size = new System.Drawing.Size(204, 22);
			this.MenuMain_ShowOther.Text = "显示其他任务(&R)";
			this.MenuMain_ShowOther.Click += new System.EventHandler(this.MenuMain_ShowOther_Click);
			// 
			// MenuMain_GoogleQuest
			// 
			this.MenuMain_GoogleQuest.Name = "MenuMain_GoogleQuest";
			this.MenuMain_GoogleQuest.Size = new System.Drawing.Size(203, 22);
			this.MenuMain_GoogleQuest.Text = "用 Google 搜索任务名(&G)";
			this.MenuMain_GoogleQuest.Click += new System.EventHandler(this.MenuMain_GoogleQuest_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(200, 6);
			// 
			// FormQuest
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.QuestView);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormQuest";
			this.Text = "任务";
			this.Load += new System.EventHandler(this.FormQuest_Load);
			((System.ComponentModel.ISupportInitialize)(this.QuestView)).EndInit();
			this.MenuProgress.ResumeLayout(false);
			this.MenuMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView QuestView;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.ContextMenuStrip MenuMain;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_Initialize;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_State;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_Type;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_Category;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_Name;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ColumnFilter_Progress;
		private System.Windows.Forms.ContextMenuStrip MenuProgress;
		private System.Windows.Forms.ToolStripMenuItem MenuProgress_Increment;
		private System.Windows.Forms.ToolStripMenuItem MenuProgress_Decrement;
		private System.Windows.Forms.DataGridViewCheckBoxColumn QuestView_State;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Type;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Category;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuestView_Progress;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem MenuProgress_Reset;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_QuestFilter;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowRunningOnly;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowOnce;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowDaily;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowWeekly;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowMonthly;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_ShowOther;
		private System.Windows.Forms.ToolStripMenuItem MenuMain_GoogleQuest;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
	}
}