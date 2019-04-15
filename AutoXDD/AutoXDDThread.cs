using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Automation;

namespace AutoXDD
{
	class AutoXDDThread : AutomationThread
	{
		public AutoXDDThread()
		{
			TargetWndName = "学习强国 - MuMu模拟器";
		}		

		bool WaitForMyUI()
		{
			return WaitForPixel(52, 459, MemDC.RGB(255, 156, 31), 0);
		}

		bool WaitForScoreUI()
		{
			return WaitForPixel(9, 257, MemDC.RGB(250, 86, 46), 0);
		}

		bool WaitForAvailable(int x, int y)
		{
			return WaitForPixel(x, y, MemDC.RGB(253, 247, 239), 1000);
		}

		protected override void ThreadProc()
		{
			SetTargetWndForeground();

			DelayBeforeAction();
			MouseClick(480, 954); // 右下角[我的]
			WaitForMyUI();

			DelayBeforeAction();
			MouseClick(56, 489); // 学习积分
			WaitForScoreUI();

			if (WaitForAvailable(458, 538))
			{
				ReadArticles();
			}

			if (WaitForAvailable(458, 640))
			{
				WatchVideos();
			}
		}		

		void ReadArticles()
		{
		}

		// 回到视频列表
		void WaitForVideoList()
		{
			WaitForPixel(373, 947, MemDC.RGB(227, 36, 22), 0);
		}		

		void WatchSingleVideo(int x, int y)
		{
			DelayBeforeAction();
			MouseClick(x, y);
			Sleep(1500);
			WaitForPixel(309, 216, MemDC.RGB(248, 248, 248), 300000); // 等待视频播放完毕后中间出现“重新播放”或超过5分钟
			DelayBeforeAction();
			MouseClick(25, 99); // 顶端白色<返回按钮
			WaitForVideoList();
		}

		void WatchVideos()
		{
			DelayBeforeAction();
			MouseClick(477, 637); // 观看视频 - [去看看]
			WaitForVideoList();

			// 顶屏
			for (int i = 0; i < 11; i++)
			{
				Sleep(100);
				MouseWheel(true);
			}

			WatchSingleVideo(261, 353); // 置顶大视频
			WatchSingleVideo(426, 614); // 视频1
			WatchSingleVideo(428, 762); // 视频2
			WatchSingleVideo(421, 889); // 视频3

			// 换屏
			for (int i = 0; i < 11; i++)
			{
				Sleep(100);
				MouseWheel(false);
			}

			WatchSingleVideo(443, 278); // 视频4
			WatchSingleVideo(465, 416); // 视频5
			WatchSingleVideo(446, 817); // 视频6
			WatchSingleVideo(464, 909); // 视频7
		}
	}
}
