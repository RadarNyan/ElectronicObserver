namespace ElectronicObserver.Window.Dialog {
	partial class DialogShipGroupLocateShip {
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
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.ShipName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SortIndex1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SortIndex2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToOrderColumns = true;
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ShipName,
            this.SortIndex1,
            this.SortIndex2});
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Location = new System.Drawing.Point(0, 0);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dataGridView1.Size = new System.Drawing.Size(284, 261);
			this.dataGridView1.TabIndex = 0;
			this.dataGridView1.Sorted += new System.EventHandler(this.dataGridView1_Sorted);
			// 
			// ShipName
			// 
			this.ShipName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ShipName.HeaderText = "舰娘";
			this.ShipName.Name = "ShipName";
			this.ShipName.ReadOnly = true;
			this.ShipName.Width = 54;
			// 
			// SortIndex1
			// 
			this.SortIndex1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.SortIndex1.HeaderText = "编成序";
			this.SortIndex1.Name = "SortIndex1";
			this.SortIndex1.ReadOnly = true;
			this.SortIndex1.Width = 66;
			// 
			// SortIndex2
			// 
			this.SortIndex2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.SortIndex2.HeaderText = "改装序";
			this.SortIndex2.Name = "SortIndex2";
			this.SortIndex2.ReadOnly = true;
			this.SortIndex2.Width = 66;
			// 
			// DialogShipGroupLocateShip
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.dataGridView1);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogShipGroupLocateShip";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "定位舰娘";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogShipGroupLocateShip_FormClosed);
			this.Load += new System.EventHandler(this.DialogShipGroupLocateShip_Load);
			this.Shown += new System.EventHandler(this.DialogShipGroupLocateShip_Shown);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipName;
		private System.Windows.Forms.DataGridViewTextBoxColumn SortIndex1;
		private System.Windows.Forms.DataGridViewTextBoxColumn SortIndex2;
	}
}