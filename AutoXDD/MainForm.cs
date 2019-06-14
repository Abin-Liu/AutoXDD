using System;
using System.Drawing;
using System.Windows.Forms;
using Automation;

namespace AutoXDD
{
	public partial class MainForm : AutomationForm
	{
		AutoXDDThread m_thread = new AutoXDDThread();
		DateTime m_startTime;
		int m_totalDuration = 0;

		public MainForm()
		{
			InitializeComponent();
			m_thread.EnableBeeps = true;
			SetThread(m_thread);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			base.Form_OnLoad(sender, e);
			RegisterHotKey(1, Keys.F4);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			base.Form_OnClosing(sender, e);
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			base.Form_OnClosed(sender, e);
		}		

		private void SetLabel(string text)
		{
			txtTime.Text = text;
		}

		private static string formatTime(int milliseconds)
		{
			int seconds = milliseconds / 1000;
			return string.Format("{0}:{1:D2}", seconds / 60, seconds % 60);
		}

		private void SetLabel(int milliseconds)
		{
			txtTime.Text = formatTime(milliseconds);
		}

		private void UpdateTaskText(int startIndex)
		{
			string text = "";
			for (int i = startIndex; i < m_thread.Count; i++)
			{
				TaskData data = m_thread[i];
				if (text != "")
				{
					text += "\r\n";
				}

				if (data.Scroll > 0)
				{
					text += string.Format("/{0}", data.Scroll);
				}
				else
				{
					text += string.Format("{0}, {1}, {2}", data.X, data.Y, data.Minutes);
				}				
			}

			txtTasks.Text = text;
		}		

		protected override void OnHotKey(int id)
		{
			base.OnHotKey(id);
			if (id != 1 || IsAlive)
			{
				return;
			}

			Point cursor = m_thread.GetCursorClientPos();
			string text = txtTasks.Text.Trim();
			if (text != "")
			{
				text += "\r\n";
			}
			text += string.Format("{0}, {1}, {2}", cursor.X, cursor.Y, AutoXDDThread.DEFAULT_DURATION);
			txtTasks.Text = text;
		}

		protected override void OnThreadStart()
		{
			base.OnThreadStart();

			btnStart.Text = "■ 停止";
			txtTasks.Enabled = false;
			btnClear.Enabled = false;
			m_startTime = DateTime.Now;
			timer1.Enabled = true;
		}

		protected override void OnThreadStop()
		{
			base.OnThreadStop();

			timer1.Enabled = false;
			if (m_thread.Aborted)
			{
				SetLabel("已取消");
			}
			else
			{
				SetLabel("已完成");
				progressBar1.Value = progressBar1.Maximum;
				txtTasks.Text = "";
			}
			
			btnStart.Text = "▶ 开始";
			txtTasks.Enabled = true;
			btnClear.Enabled = true;
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		protected override void OnThreadMessage(int wParam, int lParam)
		{
			base.OnThreadMessage(wParam, lParam);
			UpdateTaskText(lParam);
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			if (IsAlive)
			{
				StopThread();
				return;
			}

			m_totalDuration = m_thread.SetTasks(txtTasks.Text);
			if (m_totalDuration == 0)
			{
				SetLabel("没有任务数据");
				return;
			}

			UpdateTaskText(0);
			SetLabel(m_totalDuration);
			progressBar1.Maximum = m_totalDuration;
			progressBar1.Value = 0;
			StartThread(0);
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			txtTasks.Text = "";
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			int elapsed = (int)(DateTime.Now - m_startTime).TotalMilliseconds;
			elapsed = Math.Max(0, elapsed);
			elapsed = Math.Min(m_totalDuration, elapsed);
			SetLabel(m_totalDuration - elapsed);
			progressBar1.Value = elapsed;
		}		
	}
}
