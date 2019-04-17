using System;
using System.Drawing;
using System.Windows.Forms;
using Automation;

namespace AutoXDD
{
	public partial class MainForm : AutomationForm
	{
		AutoXDDThread m_thread = new AutoXDDThread();

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
			txtTime.Text = "";			
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			base.Form_OnClosing(sender, e);
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			base.Form_OnClosed(sender, e);
		}

		private string formatTime(int milliseconds)
		{
			int seconds = milliseconds / 1000;
			return string.Format("{0}:{1:D2}", seconds / 60, seconds % 60);
		}

		private void btnArticles_Click(object sender, EventArgs e)
		{
			if (IsAlive)
			{
				StopThread();
			}
			else
			{
				if (!m_thread.SetTasks(txtTasks.Text))
				{
					Message("没有任务数据。");
				}
				else
				{
					txtTime.Text = formatTime(m_thread.TotalDuration);
					progressBar1.Maximum = m_thread.TotalDuration;
					progressBar1.Value = 0;
					StartThread();
				}
			}
		}

		protected override void OnHotKey(int id)
		{
			base.OnHotKey(id);
			if (id != 1 || IsAlive)
			{
				return;
			}

			Point cursor = m_thread.GetCursorClientPos();			
			string line = string.Format("{0}, {1}, 3:00\r\n", cursor.X, cursor.Y);
			txtTasks.Text += line;
		}

		protected override void OnThreadStart()
		{
			base.OnThreadStart();			
			btnStart.Text = "■  停止";
			txtTasks.Enabled = false;
		}

		protected override void OnThreadStop()
		{
			base.OnThreadStop();
			if (!m_thread.Aborted)			
			{
				txtTime.Text = formatTime(0);
				progressBar1.Value = progressBar1.Maximum;
				m_thread.Alerting = true;
				Message("本次自动学习完成。", MessageBoxIcon.Information);
				m_thread.Alerting = false;
			}
			
			btnStart.Text = "▶  开始";
			txtTasks.Enabled = true;
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		protected override void OnThreadMessage(int wParam, int lParam)
		{
			base.OnThreadMessage(wParam, lParam);
			int elapsed = m_thread.Elapsed;
			txtTime.Text = formatTime(m_thread.TotalDuration - elapsed);
			progressBar1.Value = elapsed;
		}
	}
}
