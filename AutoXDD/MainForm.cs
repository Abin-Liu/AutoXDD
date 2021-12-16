using System;
using System.Windows.Forms;
using Automation;
using Win32API;
using ToolkitForms;

namespace AutoXDD
{
	public partial class MainForm : AutomationForm
	{
		AutoXDDThread m_thread = new AutoXDDThread();

		DateTime m_startTime; // 线程开始时间
		int m_totalTime = 0; // 任务总预计耗时

		public MainForm()
		{
			InitializeComponent();
			SetThread(m_thread);
			m_thread.EnableBeeps = true;			
			ThreadTickInterval = 0; // 本项目不需要ticker
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			m_thread.Load();
			comboBox1.SelectedIndex = 0;			
			txtArticlePos.Text = m_thread.ArticleStart.ToString();
			txtVideoPos.Text = m_thread.VideoStart.ToString();
			txtVideoButton.Text = m_thread.VideoButton.ToString();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			timer1.Enabled = false;
		}

		static string formatTime(int milliseconds)
		{
			int seconds = milliseconds / 1000;
			return string.Format("{0}:{1:D2}", seconds / 60, seconds % 60);
		}			

		protected override void OnThreadStart()
		{
			base.OnThreadStart();

			comboBox1.Enabled = false;
			txtArticlePos.Enabled = false;
			txtVideoPos.Enabled = false;
			txtVideoButton.Enabled = false;
			btnStart.Enabled = false;
			btnStop.Enabled = true;

			m_startTime = DateTime.Now;
			timer1.Enabled = true;
		}

		protected override void OnThreadStop()
		{
			base.OnThreadStop();

			timer1.Enabled = false;
			if (m_thread.Aborted)
			{
				txtTime.Text = "已取消";
			}
			else
			{
				txtTime.Text = "已完成";
				progressBar1.Value = progressBar1.Maximum;
				if (chkAutoShutdown.Checked)
				{
					Shutdown();
				}
			}

			comboBox1.Enabled = true;
			txtArticlePos.Enabled = true;
			txtVideoPos.Enabled = true;
			txtVideoButton.Enabled = true;
			btnStart.Enabled = true;
			btnStop.Enabled = false;
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			Close();
		}		

		private void btnStart_Click(object sender, EventArgs e)
		{
			if (IsAlive)
			{
				return;
			}

			IntPtr targetWnd = TestTargetWindow();
			if (targetWnd == IntPtr.Zero)
			{
				return;
			}

			m_thread.Mode = (AutoXDDThread.BrowseMode)comboBox1.SelectedIndex;
			m_thread.Save();

			m_totalTime = m_thread.TotalTime;
			txtTime.Text = formatTime(m_totalTime);

			progressBar1.Minimum = 0;
			progressBar1.Maximum = m_totalTime;
			progressBar1.Value = 0;

			StartThread();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			if (IsAlive)
			{
				StopThread();
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			int elapsed = (int)(DateTime.Now - m_startTime).TotalMilliseconds;
			elapsed = Math.Max(0, elapsed);
			elapsed = Math.Min(m_totalTime, elapsed);
			txtTime.Text = formatTime(m_totalTime - elapsed);
			progressBar1.Value = elapsed;
		}
		
		IntPtr TestTargetWindow()
		{
			IntPtr targetWnd = m_thread.FindTargetWnd();
			if (targetWnd == IntPtr.Zero)
			{
				MessageBox.Show(this, "未找到目标窗体: " + AutoXDDThread.WindowName, ProductName);
			}
			return targetWnd;
		}

		void CapturePos(string type)
		{
			IntPtr targetWnd = TestTargetWindow();
			if (targetWnd == IntPtr.Zero)
			{
				return;
			}

			FormCapture form = new FormCapture(targetWnd);
			if (form.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			switch (type)
			{
				case "ArticalPos":
					m_thread.ArticleStart = form.CursorPos;
					txtArticlePos.Text = m_thread.ArticleStart.ToString();
					break;

				case "VideoPos":
					m_thread.VideoStart = form.CursorPos;
					txtVideoPos.Text = m_thread.VideoStart.ToString();
					break;

				case "VideoButton":
					m_thread.VideoButton = form.CursorPos;
					txtVideoButton.Text = m_thread.VideoButton.ToString();
					break;

				default:
					break;
			}			

			Window.SetForegroundWindow(Handle);
		}

		private void txtArticlePos_Click(object sender, EventArgs e)
		{
			CapturePos("ArticalPos");
		}

		private void txtVideoPos_Click(object sender, EventArgs e)
		{
			CapturePos("VideoPos");
		}

		private void txtVideoButton_Click(object sender, EventArgs e)
		{
			CapturePos("VideoButton");
		}

		private void Shutdown()
		{
			ShutdownForm form = new ShutdownForm();
			form.CountDown = 180;

			if (form.ShowDialog(this) == DialogResult.OK)
			{
				Close();
			}
		}
	}
}
