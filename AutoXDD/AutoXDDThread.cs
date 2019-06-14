﻿using System;
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
		public int Duration { get; set; } = 0; // 毫秒
		public int Minutes { get { return Duration / 1000 / 60; } } // 分钟
		public int Scroll { get; set; } = 0;
	}

	class AutoXDDThread : AutomationThread
	{
		public const int DEFAULT_DURATION = 4; // 默认任务时间（分钟）
		public const int SCROLL_DELAY_TIME = 100; // 两次滚轮之间的间隔
		public const int TASK_OPEN_TIME = 1500; // 增加到每个任务开头的时间用于等待鼠标点击
		public const int TASK_EXTRA_TIME = 3000; // 增加到每个任务开始和结尾的额外时间，用以抵消网络延迟和程序延迟等因素

		public int Count { get { return m_tasks.Count; } }
		public TaskData this[int index] { get { return m_tasks[index]; } }

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

		public int SetTasks(string contents)
		{
			int duration = 0;
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
						duration += data.Scroll * SCROLL_DELAY_TIME;
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
						data.Duration = Convert.ToInt32(fields[2].Trim()) * 60 * 1000; // GUI上的任务时间一律是分钟
						m_tasks.Add(data);
						duration += TASK_OPEN_TIME + TASK_EXTRA_TIME + data.Duration + TASK_EXTRA_TIME;
					}
					catch
					{
						continue;
					}
				}
			}

			return duration;
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
				if (IsTargetWndForeground())
				{
					MouseWheel(false);
				}				
			}
		}

		void Back()
		{
			SetTargetWndForeground();
			MouseClick(32, 90); // 顶端<返回按钮
		}

		protected override void ThreadProc()
		{
			for (int i = 0; i < Count; i++)
			{
				TaskData data = m_tasks[i];

				SetTargetWndForeground();

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

				PostMessage(0, i + 1);
			}
		}		
	}
}
