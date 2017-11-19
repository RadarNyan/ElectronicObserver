﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BrowserLib
{
	/// <summary>
	/// FormBrowserのインターフェス
	/// WCFでプロセス間通信用
	/// </summary>
	[ServiceContract]
	public interface IBrowser
	{
		[OperationContract]
		void ConfigurationChanged(BrowserConfiguration conf);

		[OperationContract]
		void InitialAPIReceived();

		[OperationContract]
		void SaveScreenShot();

		[OperationContract]
		void RefreshBrowser();

		[OperationContract]
		void ApplyZoom();

		[OperationContract]
		void Navigate(string url);

		/// <summary>
		/// プロキシをセット
		/// </summary>
		[OperationContract]
		void SetProxy(string proxy);

		[OperationContract]
		void ApplyStyleSheet();

		[OperationContract]
		void DestroyDMMreloadDialog();

		[OperationContract]
		void CloseBrowser();

		[OperationContract]
		void SetIconResource(byte[] canvas);

	}
}
