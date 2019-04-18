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
			RegisterHotKey(1, Keys.G, Win32API.ModKeys.Control);
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
					text += string.Format("{0}, {1}, {2}", data.X, data.Y, formatTime(data.Duration));
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
			text += string.Format("{0}, {1}, {2}", cursor.X, cursor.Y, formatTime(AutoXDDThread.DEFAULT_DURATION));
			txtTasks.Text = text;
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
			if (m_thread.Aborted)
			{
				SetLabel("已取消");
			}
			else
			{
				SetLabel("已完成");
				progressBar1.Value = progressBar1.Maximum;
			}
			
			btnStart.Text = "▶  开始";
			txtTasks.Enabled = true;
			txtTasks.Text = "";
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		protected override void OnThreadMessage(int wParam, int lParam)
		{
			base.OnThreadMessage(wParam, lParam);

			if (wParam == 0) // OnTick
			{
				if (!IsAlive)
				{
					return;
				}

				int elapsed = m_thread.Elapsed;
				progressBar1.Value = elapsed;
				SetLabel(m_thread.TotalDuration - elapsed);
			}
			else if (wParam == 1) // Progress
			{
				UpdateTaskText(lParam);
			}			
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			if (IsAlive)
			{
				StopThread();
				return;
			}

			if (!m_thread.SetTasks(txtTasks.Text))
			{
				SetLabel("没有任务数据");
				return;
			}

			UpdateTaskText(0);
			SetLabel(m_thread.TotalDuration);
			progressBar1.Maximum = m_thread.TotalDuration;
			progressBar1.Value = 0;
			StartThread();
		}
	}
}
