using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Win32API;

namespace AutoXDD
{
	partial class FormCapture : Form
	{
		public Point CursorPos { get; set; }

		IntPtr m_targetWnd = IntPtr.Zero;

		public FormCapture(IntPtr targetWnd)
		{
			InitializeComponent();
			m_targetWnd = targetWnd;
		}

		private void FormCapture_Load(object sender, EventArgs e)
		{
			Hotkey.RegisterHotKey(Handle, 0, Keys.F4);
		}

		private void FormCapture_FormClosing(object sender, FormClosingEventArgs e)
		{
			Hotkey.UnregisterHotKey(Handle, 0);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			int id = Hotkey.IsHotkeyEvent(ref m);
			if (id != 0)
			{
				return;
			}			

			Point point = Input.GetCursorPos();
			Point offset = Window.ScreenToClient(m_targetWnd);
			point.Offset(offset);
			CursorPos = point;
			DialogResult = DialogResult.OK;
			Close();
		}		
	}
}
