﻿using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.Record
{

	public sealed class RecordManager
	{

		#region Singleton

		private static readonly RecordManager instance = new RecordManager();

		public static RecordManager Instance => instance;

		#endregion

		public string MasterPath { get; private set; }

		public EnemyFleetRecord EnemyFleet { get; private set; }
		public ShipParameterRecord ShipParameter { get; private set; }
		public ConstructionRecord Construction { get; private set; }
		public ShipDropRecord ShipDrop { get; private set; }
		public DevelopmentRecord Development { get; private set; }
		public ResourceRecord Resource { get; private set; }
		public ResourceConsumptionRecord ResourceConsumption { get; private set; }
		public ResourcePreConsumptionRecord ResourcePreConsumption { get; private set; }

		private IEnumerable<RecordBase> Records
		{
			get
			{
				yield return EnemyFleet;
				yield return ShipParameter;
				yield return Construction;
				yield return ShipDrop;
				yield return Development;
				yield return Resource;
				yield return ResourceConsumption;
				yield return ResourcePreConsumption;
			}
		}


		private DateTime _prevTime;

		private RecordManager()
		{

			MasterPath = @"Record";
			EnemyFleet = new EnemyFleetRecord();
			ShipParameter = new ShipParameterRecord();
			Construction = new ConstructionRecord();
			ShipDrop = new ShipDropRecord();
			Development = new DevelopmentRecord();
			Resource = new ResourceRecord();
			ResourceConsumption = new ResourceConsumptionRecord();
			ResourcePreConsumption = new ResourcePreConsumptionRecord();

			foreach (var r in Records)
				r.RegisterEvents();


			if (!Directory.Exists(MasterPath))
			{
				Directory.CreateDirectory(MasterPath);
			}

			_prevTime = DateTime.Now;
			Observer.APIObserver.Instance["api_port/port"].ResponseReceived += TimerSave;
		}

		public bool Load(bool logging = true)
		{

			bool succeeded = true;

			ResourceManager.CopyDocumentFromArchive("Record/" + ShipParameter.FileName, MasterPath + "\\" + ShipParameter.FileName);

			foreach (var r in Records)
				succeeded &= r.Load(MasterPath);

			if (logging)
			{
				if (succeeded)
					Utility.Logger.Add(2, "", "已读取记录。");
				else
					Utility.Logger.Add(3, "", "读取记录失败。");
			}

			return succeeded;
		}


		public bool SaveAll(bool logging = true)
		{

			//api_start2がロード済みのときのみ
			if (KCDatabase.Instance.MasterShips.Count == 0) return false;

			bool succeeded = true;

			foreach (var r in Records)
				succeeded &= r.SaveAll(MasterPath);

			if (logging)
			{
				if (succeeded)
					Utility.Logger.Add(2, "", "已保存记录。");
				else
					Utility.Logger.Add(2, "", "保存记录失败。");
			}

			return succeeded;
		}

		public bool SavePartial(bool logging = true)
		{

			//api_start2がロード済みのときのみ
			if (KCDatabase.Instance.MasterShips.Count == 0) return false;

			bool succeeded = true;


			foreach (var r in Records)
			{
				if (!r.NeedToSave)
				{
					continue;
				}

				if (r.SupportsPartialSave)
					succeeded &= r.SavePartial(MasterPath);
				else
					succeeded &= r.SaveAll(MasterPath);

			}

			if (logging)
			{
				if (succeeded)
					Utility.Logger.Add(2, "", "已保存记录。");
				else
					Utility.Logger.Add(2, "", "保存记录失败。");
			}

			return succeeded;
		}


		void TimerSave(string apiname, dynamic data)
		{

			bool iscleared;

			switch (Utility.Configuration.Config.Control.RecordAutoSaving)
			{
				case 0:
				default:
					iscleared = false;
					break;
				case 1:
					iscleared = DateTimeHelper.IsCrossedHour(_prevTime);
					break;
				case 2:
					iscleared = DateTimeHelper.IsCrossedDay(_prevTime, 0, 0, 0);
					break;
				case 3:
					iscleared = true;
					break;
			}


			if (iscleared)
			{
				_prevTime = DateTime.Now;

				if (Records.Any(r => r.NeedToSave))
				{

					if (SavePartial(false))
					{
						Utility.Logger.Add(1, "", "已自动保存记录。");
					}
					else
					{
						Utility.Logger.Add(3, "", "自动保存记录失败。");
					}
				}
			}
		}

	}
}
