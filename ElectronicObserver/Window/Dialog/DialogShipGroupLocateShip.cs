using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogShipGroupLocateShip : Form {

		public void DataAdd(int masterID, bool r1, bool r2, bool r3, bool r4, bool r5)
		{
			var ship = KCDatabase.Instance.Ships[masterID];
			var shipOrders = KCDatabase.Instance.ShipsOrder;
			DataGridViewRow row = new DataGridViewRow();
			row.CreateCells(dataGridView1);
			row.Cells[0].Value = $"{ship.Name} Lv.{ship.Level}";
			row.Cells[0].Style.Font = Utility.Configuration.Config.UI.JapFont;
			row.Cells[1].Value = GetIndexString(shipOrders[masterID][0]);
			row.Cells[2].Value = GetIndexString(shipOrders[masterID][1]);
			row.Cells[3].Value = GetIndexString(shipOrders[masterID][2]);
			row.Cells[4].Value = GetIndexString(shipOrders[masterID][3]);
			row.Cells[5].Value = GetIndexString(shipOrders[masterID][4]);
			dataGridView1.Rows.Add(row);

			SortIndex1.Visible = r1;
			SortIndex2.Visible = r2;
			SortIndex3.Visible = r3;
			SortIndex4.Visible = r4;
			SortIndex5.Visible = r5;

			if (!SortIndex1.Visible &&
				!SortIndex2.Visible &&
				!SortIndex3.Visible &&
				!SortIndex4.Visible &&
				!SortIndex5.Visible)
				SortIndex2.Visible = true;

			if (!SortIndex1.Visible && SortIndex2.Visible)
				SortIndex2.HeaderText = SortIndex1.HeaderText;
			if (!SortIndex3.Visible && SortIndex4.Visible)
				SortIndex4.HeaderText = SortIndex3.HeaderText;
		}

		private string GetIndexString(int index)
		{
			if (index == 0)
				return "";
			int p = index / 10;
			int n = index % 10;
			return string.Format("{0}_{1:D2}",
				n == 0 ? p : p + 1,
				n == 0 ? 10 : n);
		}

		public void DataClear() {
			dataGridView1.Rows.Clear();
		}

		public DialogShipGroupLocateShip() {
			InitializeComponent();
			dataGridView1.RowsDefaultCellStyle.BackColor = SystemColors.Control;
			dataGridView1.RowsDefaultCellStyle.ForeColor = SystemColors.ControlText;
			dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(0xFF, 0xFF, 0xCC);
			dataGridView1.RowsDefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
			dataGridView1.Font = Utility.Configuration.Config.UI.MainFont;
			dataGridView1.SortCompare += new DataGridViewSortCompareEventHandler(this.dataGridView1_SortCompare);
		}

		private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			if (e.Column.Index == ShipName.Index) {
				string s1 = (string)e.CellValue1;
				string s2 = (string)e.CellValue2;
				int num1 = int.Parse(s1.Split(new[] { "Lv." }, StringSplitOptions.None)[1]);
				int num2 = int.Parse(s2.Split(new[] { "Lv." }, StringSplitOptions.None)[1]);
				e.SortResult = num1 - num2;
			} else {
				string s1 = (string)e.CellValue1;
				string s2 = (string)e.CellValue2;
				if (s1 == "") {
					if (s2 == "") {
						e.SortResult = 0;
					} else {
						if (dataGridView1.SortOrder == SortOrder.Ascending) {
							e.SortResult = 1;
						} else {
							e.SortResult = -1;
						}
					}
				} else {
					if (s2 == "") {
						if (dataGridView1.SortOrder == SortOrder.Ascending) {
							e.SortResult = -1;
						} else {
							e.SortResult = 1;
						}
					} else {
						int num1 = int.Parse(s1.Replace("_", ""));
						int num2 = int.Parse(s2.Replace("_", ""));
						e.SortResult = num1 - num2;
					}
				}
			}
			e.Handled = true;
		}

		private void DialogShipGroupLocateShip_Load( object sender, EventArgs e ) {
			this.Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormShipGroup] );
		}

		private void DialogShipGroupLocateShip_FormClosed( object sender, FormClosedEventArgs e ) {
			ResourceManager.DestroyIcon( Icon );
		}


		private void DialogShipGroupLocateShip_Shown( object sender, EventArgs e ) {
			dataGridView1.ClearSelection();

			ClientSize = new Size(
				dataGridView1.Columns.GetColumnsWidth(DataGridViewElementStates.Visible),
				Math.Min(
					dataGridView1.Rows.GetRowsHeight(DataGridViewElementStates.Visible),
					dataGridView1.RowTemplate.Height * 20
					) + dataGridView1.ColumnHeadersHeight
				);

			if (dataGridView1.RowCount > 1) {
				this.Height += 2; // workaround
			}

			if (dataGridView1.Controls.OfType<VScrollBar>().First().Visible) {
				this.Width += SystemInformation.VerticalScrollBarWidth;
			}

 			var workingScreen = Screen.GetWorkingArea( Location );
			var dialogRectangle = new Rectangle( Left, Top, Right, Bottom );

			if ( !workingScreen.Contains( dialogRectangle ) ) {

				if ( Right > workingScreen.Right && Bottom > workingScreen.Bottom ) {
					Location = new Point( workingScreen.Right - Width, workingScreen.Bottom - Height );
				} else if ( Right > workingScreen.Right ) {
					Location = new Point( workingScreen.Right - Width, Top );
				} else if ( Bottom > workingScreen.Bottom ) {
					Location = new Point( Left, workingScreen.Bottom - Height );
				} else {
					return; // モニターを Location で指定してあるので例外はないはず。
				}
			}
		}

		private void dataGridView1_Sorted(object sender, EventArgs e)
		{
			dataGridView1.ClearSelection();
		}
	}
}
