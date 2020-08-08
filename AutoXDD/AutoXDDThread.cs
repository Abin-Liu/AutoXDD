using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Automation;
using MFGLib;

namespace AutoXDD
{
	class AutoXDDThread : AutomationThread
	{
		public enum BrowseMode { All, Articles, Videos }
		public const string WindowName = "学习强国 - MuMu模拟器";

		public BrowseMode Mode { get; set; }
		public Point ArticleStart { get; set; } // 第一篇文章起始位置
		public Point VideoStart { get; set; } // CCTV第一个视频起始位置
		public Point VideoButton { get; set; } // 右下角[电视台]按钮

		const int ArticleCount = 6; // 文章浏览篇数
		const int ArticleDuration = 120000; // 文章浏览时长（2分钟）
		const int ArticleHeight = 122; // 文章链接高度（像素）
		
		const int VideoCount = 7; // 视频观看次数		
		const int VideoDuration = 180000; // 视频浏览时长（4分钟）
		const int VideoHeight = 82; // 视频链接高度（像素）

		readonly Point BackButton = new Point(32, 90); // 顶端[<返回]按钮

		const int PageChangeoverTime = 2000; // 换页时间
		const int TaskOpenTime = 2000; // 增加到每个任务开头的时间用于等待鼠标点击
		const int TaskExtraTime = 2000; // 增加到每个任务开始和结尾的额外时间，用以抵消网络延迟和程序延迟等因素

		// 全流程预计耗时：
		public int TotalTime
		{
			get
			{
				int duration = 0;
				if (Mode == BrowseMode.All || Mode == BrowseMode.Articles)
				{
					// 浏览6篇文章
					duration += (ArticleDuration + TaskOpenTime * 2) * ArticleCount;
				}

				if (Mode == BrowseMode.All)
				{
					// 点击右下角[电视台]按钮
					duration += PageChangeoverTime;
				}

				if (Mode == BrowseMode.All || Mode == BrowseMode.Videos)
				{
					// 观看7个视频，其中新闻联播观看3倍时长	
					duration += (VideoDuration + TaskOpenTime * 2) * VideoCount + VideoDuration * 2 + TaskOpenTime * 2;
				}

				return duration;
			}
		}

		public void Load()
		{
			RegistryHelper reg = new RegistryHelper();
			reg.Open("Abin", Application.ProductName);
			ArticleStart = ParsePoint(reg.ReadString("ArticleStart", "207,279"));
			VideoStart = ParsePoint(reg.ReadString("VideoStart", "235,201"));
			VideoButton = ParsePoint(reg.ReadString("VideoButton", "375,953"));
			reg.Close();
		}

		public void Save()
		{
			RegistryHelper reg = new RegistryHelper();
			reg.Open("Abin", Application.ProductName, true);
			reg.WriteString("ArticleStart", ComposePoint(ArticleStart));
			reg.WriteString("VideoStart", ComposePoint(VideoStart));
			reg.WriteString("VideoButton", ComposePoint(VideoButton));
			reg.Close();
		}

		static Point ParsePoint(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return new Point();
			}

			string[] items = text.Split(',');
			if (items.Length != 2)
			{
				return new Point();
			}
			
			try
			{
				int x = Convert.ToInt32(items[0].Trim());
				int y = Convert.ToInt32(items[1].Trim());
				return new Point(x, y);
			}
			catch (Exception)
			{
				return new Point();
			}
		}

		static string ComposePoint(Point point)
		{
			return string.Format("{0},{1}", point.X, point.Y);
		}

		public override IntPtr FindTargetWnd()
		{
			// return FindWindow("Notepad", null);
			return FindWindow(null, WindowName);
		}	
		

		void ClickBack()
		{
			SetTargetWndForeground();
			MouseClick(BackButton.X, BackButton.Y);
		}		

		protected override void ThreadProc()
		{
			// 浏览6篇文章
			if (Mode == BrowseMode.All | Mode == BrowseMode.Articles)
			{
				Point point = ArticleStart;
				for (int i = 0; i < ArticleCount; i++)
				{
					SetTargetWndForeground();
					Sleep(TaskOpenTime);
					MouseClick(point.X, point.Y);
					Sleep(ArticleDuration);
					ClickBack();
					Sleep(TaskExtraTime);
					point.Y += ArticleHeight;
				}
			}

			// 切换到视频页
			if (Mode == BrowseMode.All)
			{
				// 点击右下角[电视台]按钮
				SetTargetWndForeground();
				MouseClick(VideoButton.X, VideoButton.Y);
				Sleep(PageChangeoverTime);				
			}			

			// 观看7个视频
			if (Mode == BrowseMode.All || Mode == BrowseMode.Videos)
			{
				Point point = VideoStart;				
				for (int i = 0; i < VideoCount; i++)
				{
					SetTargetWndForeground();
					Sleep(TaskOpenTime);
					MouseClick(point.X, point.Y);
					Sleep(VideoDuration);

					// 第一个视频（新闻联播）观看3倍时长
					if (i == 0)
					{
						Sleep(VideoDuration * 2);
					}

					ClickBack();
					Sleep(TaskExtraTime);
					point.Y += VideoHeight;
				}

				// 点开下一个视频后再结束线程
				SetTargetWndForeground();
				Sleep(TaskOpenTime);
				MouseClick(point.X, point.Y);
				Sleep(TaskExtraTime);
			}			
		}
	}
}
