using ElectronicObserver.Resource;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogShipGroupLocateShip : Form {

		public void DataAdd(string ship, string shipLevel, string index1, string index2) {
			DataGridViewRow row = new DataGridViewRow();
			row.CreateCells(dataGridView1);
			row.Cells[0].Value = string.Format("{0} Lv.{1}", ship, shipLevel);
			row.Cells[0].Style.Font = Utility.Configuration.Config.UI.JapFont;
			row.Cells[1].Value = index1;
			row.Cells[2].Value = index2;
			dataGridView1.Rows.Add(row);
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
				/*
				string s1 = (string)e.CellValue1;
				string s2 = (string)e.CellValue2;
				int num1 = int.Parse(s1.Split(new [] {"Lv."}, StringSplitOptions.None)[1]);
				int num2 = int.Parse(s2.Split(new [] {"Lv."}, StringSplitOptions.None)[1]);
				e.SortResult = num1 - num2;
				*/
				e.SortResult = e.CellValue1.ToString().CompareTo(e.CellValue2.ToString());
			} else if (e.Column.Index == SortIndex1.Index || e.Column.Index == SortIndex2.Index) {
				string s1 = (string)e.CellValue1;
				string s2 = (string)e.CellValue2;
				if (s1 == "") {
					if (s2 == "") {
						e.SortResult = 0;
					} else {
						e.SortResult = 1;
					}
				} else {
					if (s2 == "") {
						e.SortResult = -1;
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
