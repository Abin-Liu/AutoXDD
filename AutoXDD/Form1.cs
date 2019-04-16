using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Automation;
using Win32API;

namespace AutoXDD
{
	public partial class Form1 : AutomationForm
	{
		AutoXDDThread m_thread = new AutoXDDThread();

		public Form1()
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

		private void btnArticles_Click(object sender, EventArgs e)
		{
			if (IsAlive)
			{
				StopThread();
			}
			else
			{
				string text = txtTasks.Text.Trim();
				string[] lines = text.Split(new char[] { '\n', '\n' }, StringSplitOptions.RemoveEmptyEntries);				
				m_thread.Start(this, lines);
			}
		}

		protected override void OnHotKey(int id)
		{
			base.OnHotKey(id);
			if (id != 1 || IsAlive)
			{
				return;
			}

			IntPtr hwnd = Window.FindWindow(null, m_thread.TargetWndName);
			Point cursor = Input.GetCursorPos();
			Point offset = Window.ScreenToClient(hwnd);
			cursor.Offset(offset);
			string line = string.Format("{0}, {1}, 5:00\r\n", cursor.X, cursor.Y);
			txtTasks.Text += line;
		}

		protected override void OnThreadStart()
		{
			base.OnThreadStart();			
			btnStart.Text = "停止";
			txtTasks.Enabled = false;
		}

		protected override void OnThreadStop()
		{
			base.OnThreadStop();
			Message("本次自动学习完成。", MessageBoxIcon.Information);
			btnStart.Text = "开始";
			txtTasks.Enabled = true;
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
