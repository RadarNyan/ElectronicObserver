﻿using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility
{


	public delegate void LogAddedEventHandler(Logger.LogData data);


	public sealed class Logger
	{

		#region Singleton

		private static readonly Logger instance = new Logger();

		public static Logger Instance => instance;

		#endregion


		/// <summary>
		/// ログが追加された時に発生します。
		/// </summary>
		public event LogAddedEventHandler LogAdded = delegate { };


		public class LogData
		{

			/// <summary>
			/// 書き込み時刻
			/// </summary>
			public readonly DateTime Time;

			/// <summary>
			/// 優先度
			/// 基本的には 0=非表示(ログファイルにのみ記載, など), 1=内部情報(動作ログ, API受信情報など), 2=重要な情報(入渠完了など, ユーザーに表示する必要があるもの), 3=緊急の情報(エラー等)
			/// </summary>
			public readonly int Priority;

			/// <summary>
			/// ログ内容
			/// </summary>
			public readonly string Message;
			public readonly string MsgChs1;
			public readonly string MsgJap2;
			public readonly string MsgChs2;
			public readonly string MsgJap3;
			public readonly string MsgChs3;

			public LogData(DateTime time, int priority, string message, string msgchs1, string msgjap2, string msgchs2, string msgjap3, string msgchs3)
			{
				Time = time;
				Priority = priority;
				Message = message;
				MsgChs1 = msgchs1;
				MsgJap2 = msgjap2;
				MsgChs2 = msgchs2;
				MsgJap3 = msgjap3;
				MsgChs3 = msgchs3;
			}


			public override string ToString() => $"[{DateTimeHelper.TimeToCSVString(Time)}][{Priority}] : {Message}{MsgChs1}{MsgJap2}{MsgChs2}{MsgJap3}{MsgChs3}";


		}



		private List<LogData> log;
		private bool toDebugConsole;
		private int lastSavedCount;


		private Logger()
		{
			log = new List<LogData>();
			toDebugConsole = true;
			lastSavedCount = 0;
		}


		public static IReadOnlyList<LogData> Log
		{
			get
			{
				lock (Logger.Instance)
				{
					return Logger.Instance.log.AsReadOnly();
				}
			}
		}


		/// <summary>
		/// ログを追加します。
		/// </summary>
		/// <param name="priority">優先度。</param>
		/// <param name="message">ログ内容。</param>
		public static void Add(int priority, string message, string msgchs1="", string msgjap2="", string msgchs2="", string msgjap3="", string msgchs3="")
		{

			LogData data = new LogData(DateTime.Now, priority, message, msgchs1, msgjap2, msgchs2, msgjap3, msgchs3);

			lock (Logger.Instance)
			{
				Logger.Instance.log.Add(data);
			}

			if (Configuration.Config.Log.SaveLogFlag && Configuration.Config.Log.SaveLogImmediately)
				Save();

			if (Configuration.Config.Log.LogLevel <= priority)
			{

				if (Logger.Instance.toDebugConsole)
				{
					System.Diagnostics.Debug.WriteLine(data.ToString());
				}


				try
				{
					Logger.Instance.LogAdded(data);

				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
				}

			}
		}

		/// <summary>
		/// ログをすべて消去します。
		/// </summary>
		public static void Clear()
		{
			lock (Logger.Instance)
			{
				Logger.instance.log.Clear();
				Logger.instance.lastSavedCount = 0;
			}
		}



		public static readonly string DefaultPath = @"eolog.log";


		public static void Save()
		{
			Save(DefaultPath);
		}

		/// <summary>
		/// ログを保存します。
		/// </summary>
		/// <param name="path">保存先のファイル。</param>
		public static void Save(string path)
		{

			try
			{
				lock (Logger.Instance)
				{

					var log = Logger.instance;

					using (StreamWriter sw = new StreamWriter(path, true, Utility.Configuration.Config.Log.FileEncoding))
					{

						int priority = Configuration.Config.Log.LogLevel;

						foreach (var l in log.log.Skip(log.lastSavedCount).Where(l => l.Priority >= priority))
						{
							sw.WriteLine(l.ToString());
						}

						log.lastSavedCount = log.log.Count;
					}
				}
			}
			catch (Exception)
			{

				// に ぎ り つ ぶ す
			}

		}

	}
}
