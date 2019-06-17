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
			RegisterHotKey(2, Keys.F6);
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
		
		void AddDataRow(int x, int y)
		{
			int index = dataGridView1.Rows.Add();
			DataGridViewCellCollection cells = dataGridView1.Rows[index].Cells;
			if (x == -1)
			{
				cells[0].Value = "Scroll";
				cells[1].Value = "Down";
				cells[2].Value = AutoXDDThread.SCROLL_COUNT;
				cells[2].ReadOnly = true;
			}
			else
			{
				cells[0].Value = x;
				cells[1].Value = y;
				cells[2].Value = AutoXDDThread.DEFAULT_DURATION;
				cells[2].ReadOnly = false;
			}
		}		

		protected override void OnHotKey(int id)
		{
			base.OnHotKey(id);
			if (IsAlive)
			{
				return;
			}

			if (id == 1)
			{
				Point cursor = m_thread.GetCursorClientPos();
				AddDataRow(cursor.X, cursor.Y);				
			}
			else if (id == 2)
			{				
				AddDataRow(-1, -1);
				AutoXDDThread.ScrollDown();
			}			
		}

		protected override void OnThreadStart()
		{
			base.OnThreadStart();

			btnStart.Text = "■ 停止";
			dataGridView1.Enabled = false;
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
				dataGridView1.Rows.Clear();
			}
			
			btnStart.Text = "▶ 开始";
			dataGridView1.Enabled = true;
			btnClear.Enabled = true;
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		protected override void OnThreadMessage(int wParam, int lParam)
		{
			base.OnThreadMessage(wParam, lParam);
			dataGridView1.Rows.RemoveAt(0);
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			if (IsAlive)
			{
				StopThread();
				return;
			}

			m_thread.Clear();
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				m_thread.AddTask(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
			}

			m_totalDuration = m_thread.TotalRuntime;
			if (m_totalDuration == 0)
			{
				SetLabel("没有任务数据");
				return;
			}

			SetLabel(m_totalDuration);
			progressBar1.Maximum = m_totalDuration;
			progressBar1.Value = 0;
			StartThread(0);
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
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
