using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AutoXDD
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{			
			bool createdNew = false;
			Mutex mutex = new Mutex(true, "{CF67187B-CC9B-49A1-98F3-0D54C7131969}", out createdNew);
			if (createdNew)
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm());
			}
		}
	}
}
