using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility
{

	/// <summary>
	/// ソフトウェアの情報を保持します。
	/// </summary>
	public static class SoftwareInformation
	{

		/// <summary>
		/// ソフトウェア名(日本語)
		/// </summary>
		public static string SoftwareNameJapanese => "七四式电子观测仪";


		/// <summary>
		/// ソフトウェア名(英語)
		/// </summary>
		public static string SoftwareNameEnglish => "ElectronicObserver";


		/// <summary>
		/// バージョン(日本語, ソフトウェア名を含みます)
		/// </summary>
		public static string VersionJapanese => SoftwareNameJapanese + "三〇型改七（战时改修）";


		/// <summary>
		/// バージョン(英語)
		/// </summary>
		public static string VersionEnglish => "3.0.7";


		/// <summary>
		/// 汉化版版本号
		/// </summary>
		public static string VersionRN => "m4";


		/// <summary>
		/// 更新日時
		/// </summary>
		public static DateTime UpdateTime => DateTimeHelper.CSVStringToTime("2018/08/22 10:05:00");




		private static System.Net.WebClient client;
		private static readonly Uri uri = new Uri("https://raw.githubusercontent.com/RadarNyan/ElectronicObserver/master/VERSION");

		public static void CheckUpdate()
		{

			if (!Utility.Configuration.Config.Life.CheckUpdateInformation)
				return;

			if (client == null)
			{
				client = new System.Net.WebClient
				{
					Encoding = new System.Text.UTF8Encoding(false)
				};
				client.DownloadStringCompleted += DownloadStringCompleted;
			}

			if (!client.IsBusy)
				client.DownloadStringAsync(uri);
		}

		private static void DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
		{

			if (e.Error != null)
			{

				Utility.ErrorReporter.SendErrorReport(e.Error, "取得版本信息失败。");
				return;

			}

			if (e.Result.StartsWith("<!DOCTYPE html>"))
			{

				Utility.Logger.Add(3, "アップデート情報の URI が無効です。");
				return;

			}


			try
			{

				using (var sr = new System.IO.StringReader(e.Result))
				{

					DateTime date = DateTimeHelper.CSVStringToTime(sr.ReadLine());
					string version = sr.ReadLine();
					string description = sr.ReadToEnd();

					if (UpdateTime < date)
					{

						Utility.Logger.Add(3, "", "发现新版本！ : " + version);

						var result = System.Windows.Forms.MessageBox.Show(
							string.Format("更新版本 : {0}\r\n\r\n更新内容 :\r\n{1}\r\n要打开下载页吗？\r\n( 选择「取消」将不再自动检查更新 )",
							version, description),
							"更新信息", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Information,
							System.Windows.Forms.MessageBoxDefaultButton.Button1);


						if (result == System.Windows.Forms.DialogResult.Yes)
						{

							System.Diagnostics.Process.Start("https://ci.appveyor.com/project/RadarNyan/electronicobserver-icfh9/build/artifacts");

						}
						else if (result == System.Windows.Forms.DialogResult.Cancel)
						{

							Utility.Configuration.Config.Life.CheckUpdateInformation = false;

						}

					}
					else
					{

						Utility.Logger.Add(1, "", "检查更新 : 已经是最新版。");

					}

				}

			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport(ex, "处理更新情报失败。");
			}

		}

	}

}
