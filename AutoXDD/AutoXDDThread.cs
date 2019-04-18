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
		public const int DEFAULT_DURATION = 180000; // 默认任务时间
		public const int SCROLL_DELAY_TIME = 100; // 两次滚轮之间的间隔
		public const int TASK_OPEN_TIME = 1500; // 增加到每个任务开头的时间用于等待鼠标点击
		public const int TASK_EXTRA_TIME = 3000; // 增加到每个任务开始和结尾的额外时间，用以抵消网络延迟和程序延迟等因素

		public int Count { get { return m_tasks.Count; } }
		public TaskData this[int index] { get { return m_tasks[index]; } }

		List<TaskData> m_tasks = new List<TaskData>();
		DateTime m_startTime;

		public int TotalDuration { get; private set; } = 0;

		public int Elapsed
		{
			get
			{
				int value = (int)(DateTime.Now - m_startTime).TotalMilliseconds;
				value = Math.Max(0, value);
				value = Math.Min(TotalDuration, value);
				return value;				
			}
		}

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
			TotalDuration = 0;
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
						TotalDuration += data.Scroll * SCROLL_DELAY_TIME;
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
						if (timeFields.Length == 0)
						{
							data.Duration = DEFAULT_DURATION; // 默认
						}
						else if (timeFields.Length == 1)
						{
							data.Duration = Convert.ToInt32(timeFields[0]) * 1000; // 只有秒
						}
						else if (timeFields.Length == 2) // 分:秒
						{
							data.Duration = (Convert.ToInt32(timeFields[0]) * 60 + Convert.ToInt32(timeFields[1])) * 1000;
						}
						else // 时:分:秒
						{
							data.Duration = (Convert.ToInt32(timeFields[0]) * 3600 + Convert.ToInt32(timeFields[1]) * 60 + Convert.ToInt32(timeFields[2])) * 1000;
						}

						m_tasks.Add(data);
						TotalDuration += 1500 + 6000 + data.Duration;
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
				Sleep(SCROLL_DELAY_TIME);
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
			m_startTime = DateTime.Now;
			for (int i = 0; i < Count; i++)
			{
				TaskData data = m_tasks[i];

				if (data.Scroll > 0)
				{
					ScrollDown(data.Scroll);
				}
				else
				{
					Sleep(TASK_OPEN_TIME);
					MouseClick(data.X, data.Y);
					Sleep(TASK_EXTRA_TIME);
					AntiIdle(data.Duration);
					Sleep(TASK_EXTRA_TIME);

					// 最后一个任务不返回列表，正好蹭阅读时长
					if (i < Count - 1)
					{
						Back();
					}
				}

				PostMessage(1, i + 1);
			}
		}

		protected override void OnTick()
		{
			base.OnTick();
			PostMessage(0, 0);
		}
	}
}
