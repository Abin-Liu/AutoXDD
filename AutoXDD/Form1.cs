using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Automation;

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
			if (m_thread.IsAlive)
			{
				return;
			}

			StartThread();
		}		

		protected override void OnThreadStart()
		{
			base.OnThreadStart();
			btnStart.Enabled = false;
		}

		protected override void OnThreadStop()
		{
			base.OnThreadStop();
			Message("本次自动学习完成。", MessageBoxIcon.Information);
			btnStart.Enabled = true;
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
