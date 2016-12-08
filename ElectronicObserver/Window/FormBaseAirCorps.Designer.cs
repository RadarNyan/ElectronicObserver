﻿namespace ElectronicObserver.Window {
	partial class FormBaseAirCorps {
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
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.TableMember = new System.Windows.Forms.TableLayoutPanel();
			this.ContextMenuBaseAirCorps = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ContextMenuBaseAirCorps_CopyOrganization = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.ContextMenuBaseAirCorps_DisplayRelocatedEquipments = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextMenuBaseAirCorps.SuspendLayout();
			this.SuspendLayout();
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 60000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
			// 
			// TableMember
			// 
			this.TableMember.AutoSize = true;
			this.TableMember.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableMember.ColumnCount = 5;
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TableMember.Location = new System.Drawing.Point(0, 0);
			this.TableMember.Name = "TableMember";
			this.TableMember.RowCount = 1;
			this.TableMember.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TableMember.Size = new System.Drawing.Size(0, 21);
			this.TableMember.TabIndex = 0;
			this.TableMember.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableMember_CellPaint);
			// 
			// ContextMenuBaseAirCorps
			// 
			this.ContextMenuBaseAirCorps.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenuBaseAirCorps_CopyOrganization,
            this.toolStripSeparator1,
            this.ContextMenuBaseAirCorps_DisplayRelocatedEquipments});
			this.ContextMenuBaseAirCorps.Name = "ContextMenuBaseAirCorps";
			this.ContextMenuBaseAirCorps.Size = new System.Drawing.Size(220, 54);
			this.ContextMenuBaseAirCorps.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuBaseAirCorps_Opening);
			// 
			// ContextMenuBaseAirCorps_CopyOrganization
			// 
			this.ContextMenuBaseAirCorps_CopyOrganization.Name = "ContextMenuBaseAirCorps_CopyOrganization";
			this.ContextMenuBaseAirCorps_CopyOrganization.Size = new System.Drawing.Size(219, 22);
			this.ContextMenuBaseAirCorps_CopyOrganization.Text = "复制到剪贴板(&C)";
			this.ContextMenuBaseAirCorps_CopyOrganization.Click += new System.EventHandler(this.ContextMenuBaseAirCorps_CopyOrganization_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(216, 6);
			// 
			// ContextMenuBaseAirCorps_DisplayRelocatedEquipments
			// 
			this.ContextMenuBaseAirCorps_DisplayRelocatedEquipments.Name = "ContextMenuBaseAirCorps_DisplayRelocatedEquipments";
			this.ContextMenuBaseAirCorps_DisplayRelocatedEquipments.Size = new System.Drawing.Size(219, 22);
			this.ContextMenuBaseAirCorps_DisplayRelocatedEquipments.Text = "确认配置转换中的装备(&R)";
			this.ContextMenuBaseAirCorps_DisplayRelocatedEquipments.Click += new System.EventHandler(this.ContextMenuBaseAirCorps_DisplayRelocatedEquipments_Click);
			// 
			// FormBaseAirCorps
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.ContextMenuStrip = this.ContextMenuBaseAirCorps;
			this.Controls.Add(this.TableMember);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "FormBaseAirCorps";
			this.Text = "基地航空队";
			this.Load += new System.EventHandler(this.FormBaseAirCorps_Load);
			this.ContextMenuBaseAirCorps.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.TableLayoutPanel TableMember;
		private System.Windows.Forms.ContextMenuStrip ContextMenuBaseAirCorps;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuBaseAirCorps_CopyOrganization;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuBaseAirCorps_DisplayRelocatedEquipments;

	}
}