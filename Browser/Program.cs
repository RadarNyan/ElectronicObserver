using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Browser
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// FormBrowserHostから起動された時は引数に通信用URLが渡される
			if (args.Length == 0)
			{
				MessageBox.Show("这是七四式电子观测仪的子程序，无法独立启动。\r\n请通过本体调用本程序。",
					"信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormBrowser(args[0]));
		}
	}
}
