using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Automation;

namespace AutoXDD
{
	class TaskData
	{
		public int X { get; set; } = 0;
		public int Y { get; set; } = 0;
		public int Duration { get; set; } = 0;
		public int Scroll { get; set; } = 0;
	}

	class AutoXDDThread : AutomationThread
	{
		List<TaskData> m_tasks = new List<TaskData>();

		public AutoXDDThread()
		{
			TargetWndName = "学习强国 - MuMu模拟器";
		}	
		
		public void Start(Form1 form, string[] tasks)
		{
			m_tasks.Clear();
			foreach (string line in tasks)
			{
				TaskData data = new TaskData();
				string[] fields = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
				if (fields.Length == 1)
				{
					try
					{
						data.Scroll = Convert.ToInt32(fields[0].Replace("/", "").Trim());
						m_tasks.Add(data);
					}
					catch
					{
						continue;
					}					
				}
				else if (fields.Length == 3)
				{
					try
					{
						data.X = Convert.ToInt32(fields[0].Trim());
						data.Y = Convert.ToInt32(fields[1].Trim());
						string[] timeFields = fields[2].Split(':');
						data.Duration = (Convert.ToInt32(timeFields[0]) * 60 + Convert.ToInt32(timeFields[1])) * 1000;
						m_tasks.Add(data);
					}
					catch
					{
						continue;
					}
				}				
			}

			base.Start(form);
		}
		
		void ScrollDown(int count)
		{
			for (int i = 0; i < count; i++)
			{				
				MouseWheel(false);
				Sleep(100);
			}
		}

		void AntiIdle(int milliseconds)
		{
			for (int i = 0; i < 10; i++)
			{
				Sleep(milliseconds / 10);
				MouseWheel(false);
			}
		}

		protected override void ThreadProc()
		{
			SetTargetWndForeground();			
			foreach (TaskData data in m_tasks)
			{
				if (data.Scroll > 0)
				{
					ScrollDown(data.Scroll);
				}
				else
				{
					Sleep(1500);
					MouseClick(data.X, data.Y);
					Sleep(3000);
					AntiIdle(data.Duration);
					MouseClick(32, 90); // 顶端<返回按钮
				}				
			}
		}		
	}
}
