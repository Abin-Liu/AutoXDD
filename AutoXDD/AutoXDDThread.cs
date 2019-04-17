using System;
using System.Collections.Generic;
using System.Drawing;
using Automation;
using Win32API;

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

		public Point GetCursorClientPos()
		{
			if (TargetWnd == IntPtr.Zero)
			{
				TargetWnd = Window.FindWindow(null, TargetWndName);
			}

			Point point = Input.GetCursorPos();
			point.Offset(ScreenToClient);
			return point;
		}

		public bool SetTasks(string contents)
		{
			m_tasks.Clear();
			string[] lines = contents.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string line in lines)
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
						string[] timeFields = fields[2].Split(new char[] { ':', '.' }, StringSplitOptions.RemoveEmptyEntries);
						data.Duration = (Convert.ToInt32(timeFields[0]) * 60 + Convert.ToInt32(timeFields[1])) * 1000;
						m_tasks.Add(data);
					}
					catch
					{
						continue;
					}
				}
			}

			return m_tasks.Count > 0;
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

		void Back()
		{
			MouseClick(32, 90); // 顶端<返回按钮
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
					Sleep(3000);
					Back();
				}				
			}
		}		
	}
}
