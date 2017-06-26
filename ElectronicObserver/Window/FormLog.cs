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

namespace ElectronicObserver.Window {

	public partial class FormLog : DockContent {


		public FormLog( FormMain parent ) {
			InitializeComponent();
			ConfigurationChanged();
		}
		
		private void FormLog_Load( object sender, EventArgs e ) {

			foreach ( var log in Utility.Logger.Log ) {
				if ( log.Priority >= Utility.Configuration.Config.Log.LogLevel )
					AddLogLine(log);
			}

			Utility.Logger.Instance.LogAdded += new Utility.LogAddedEventHandler( ( Utility.Logger.LogData data ) => {
				if ( InvokeRequired ) {
					// Invokeはメッセージキューにジョブを投げて待つので、別のBeginInvokeされたジョブが既にキューにあると、
					// それを実行してしまい、BeginInvokeされたジョブの順番が保てなくなる
					// GUIスレッドによる処理は、順番が重要なことがあるので、GUIスレッドからInvokeを呼び出してはいけない
					Invoke( new Utility.LogAddedEventHandler( Logger_LogAdded ), data );
				} else {
					Logger_LogAdded( data );
				}
			} );

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormLog] );
		}


		void ConfigurationChanged() {
			LogTextBox.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			LogTextBox.BackColor = Utility.Configuration.Config.UI.BackColor;
			LogTextBox.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
			LogTextBox.Font = Utility.Configuration.Config.UI.JapFont;
			ContextMenuLog_WrapText.Checked = Utility.Configuration.Config.UI.TextWrapInLogWindow;
			ContextMenuLog_CompactMode.Checked = Utility.Configuration.Config.UI.CompactModeLogWindow;
		}


		private void AddLogLine(Utility.Logger.LogData data) {
			if (LogTextBox.Lines.Length > 0) {
				if (LogTextBox.Lines.Length > 299) { // Delete first 100 lines once Log reaches 300 lines
					SuspendDrawLog();
					LogTextBox.ReadOnly = false;
					LogTextBox.Select(0, LogTextBox.GetFirstCharIndexFromLine(100));
					LogTextBox.SelectedText = "";
					LogTextBox.ReadOnly = true;
					SuspendDrawLog(false);
				}
				LogTextBox.AppendText("\r\n");
				LogTextBox.ScrollToCaret();
			}
			LogTextBox.SelectionFont = Utility.Configuration.Config.UI.JapFont;
			if (ContextMenuLog_CompactMode.Checked) {
				LogTextBox.AppendText(string.Format("[{0:HH:mm:ss}] {1}", data.Time, data.Message));
			} else {
				LogTextBox.AppendText(string.Format("[{0}][{1}] : {2}", Utility.Mathematics.DateTimeHelper.TimeToCSVString(data.Time), data.Priority, data.Message));
			}
			LogTextBox.SelectionFont = Utility.Configuration.Config.UI.MainFont;
			LogTextBox.AppendText(data.MsgChs1);
			LogTextBox.SelectionFont = Utility.Configuration.Config.UI.JapFont;
			LogTextBox.AppendText(data.MsgJap2);
			LogTextBox.SelectionFont = Utility.Configuration.Config.UI.MainFont;
			LogTextBox.AppendText(data.MsgChs2);
			LogTextBox.SelectionFont = Utility.Configuration.Config.UI.JapFont;
			LogTextBox.AppendText(data.MsgJap3);
			LogTextBox.SelectionFont = Utility.Configuration.Config.UI.MainFont;
			LogTextBox.AppendText(data.MsgChs3);
			HideCaret(LogTextBox.Handle);
		}


		void Logger_LogAdded( Utility.Logger.LogData data ) {
			AddLogLine(data);
		}



		private void ContextMenuLog_Clear_Click( object sender, EventArgs e ) {

			LogTextBox.Text = "";

		}

		protected override string GetPersistString() {
			return "Log";
		}


		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern int HideCaret (IntPtr hwnd);

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

		private const int WM_SETREDRAW = 11;


		private void HideCaret(object sender, EventArgs e)
		{
			HideCaret(LogTextBox.Handle);
		}


		private void SuspendDrawLog(bool suspend = true)
		{
			if (suspend) {
				SendMessage(LogTextBox.Handle, WM_SETREDRAW, false, 0);
			} else {
				SendMessage(LogTextBox.Handle, WM_SETREDRAW, true, 0);
				LogTextBox.Refresh();
			}
		}


		private void ContextMenuLog_WrapText_CheckedChanged(object sender, EventArgs e)
		{
			LogTextBox.WordWrap = ContextMenuLog_WrapText.Checked;
			Utility.Configuration.Config.UI.TextWrapInLogWindow = LogTextBox.WordWrap;
			ReloadLog();
		}


		private void ContextMenuLog_CompactMode_CheckedChanged(object sender, EventArgs e)
		{
			LogTextBox.ScrollBars = ContextMenuLog_CompactMode.Checked ? RichTextBoxScrollBars.None : RichTextBoxScrollBars.Both;
			Utility.Configuration.Config.UI.CompactModeLogWindow = ContextMenuLog_CompactMode.Checked;
			ReloadLog();
		}


		private void ReloadLog()
		{
			LogTextBox.Text = "";
			foreach (var log in Utility.Logger.Log) {
				if (log.Priority >= Utility.Configuration.Config.Log.LogLevel)
					AddLogLine(log);
			}
		}
	}
}
