namespace AutoXDD
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.btnExit = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTime = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtArticlePos = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtVideoPos = new System.Windows.Forms.TextBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.txtVideoButton = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnExit
			// 
			this.btnExit.Location = new System.Drawing.Point(227, 247);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 28);
			this.btnExit.TabIndex = 13;
			this.btnExit.Text = "退出";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(11, 196);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 12);
			this.label2.TabIndex = 8;
			this.label2.Text = "剩余时间：";
			// 
			// txtTime
			// 
			this.txtTime.Location = new System.Drawing.Point(82, 196);
			this.txtTime.Name = "txtTime";
			this.txtTime.Size = new System.Drawing.Size(234, 12);
			this.txtTime.TabIndex = 9;
			this.txtTime.Text = "未开始";
			this.txtTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(14, 213);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(304, 18);
			this.progressBar1.TabIndex = 10;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "浏览内容：";
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "全部（文章+视频）",
            "仅文章",
            "仅视频"});
			this.comboBox1.Location = new System.Drawing.Point(86, 11);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(233, 20);
			this.comboBox1.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(11, 41);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(65, 12);
			this.label3.TabIndex = 2;
			this.label3.Text = "文章起始：";
			// 
			// txtArticlePos
			// 
			this.txtArticlePos.BackColor = System.Drawing.SystemColors.Window;
			this.txtArticlePos.Cursor = System.Windows.Forms.Cursors.Hand;
			this.txtArticlePos.Location = new System.Drawing.Point(86, 38);
			this.txtArticlePos.Name = "txtArticlePos";
			this.txtArticlePos.ReadOnly = true;
			this.txtArticlePos.Size = new System.Drawing.Size(233, 21);
			this.txtArticlePos.TabIndex = 3;
			this.txtArticlePos.Click += new System.EventHandler(this.txtArticlePos_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(11, 68);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(65, 12);
			this.label4.TabIndex = 5;
			this.label4.Text = "视频起始：";
			// 
			// txtVideoPos
			// 
			this.txtVideoPos.BackColor = System.Drawing.SystemColors.Window;
			this.txtVideoPos.Cursor = System.Windows.Forms.Cursors.Hand;
			this.txtVideoPos.Location = new System.Drawing.Point(86, 65);
			this.txtVideoPos.Name = "txtVideoPos";
			this.txtVideoPos.ReadOnly = true;
			this.txtVideoPos.Size = new System.Drawing.Size(233, 21);
			this.txtVideoPos.TabIndex = 6;
			this.txtVideoPos.Click += new System.EventHandler(this.txtVideoPos_Click);
			// 
			// btnStop
			// 
			this.btnStop.Enabled = false;
			this.btnStop.Image = global::AutoXDD.Properties.Resources.Stop;
			this.btnStop.Location = new System.Drawing.Point(128, 247);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 28);
			this.btnStop.TabIndex = 12;
			this.btnStop.Text = "停止";
			this.btnStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnStart
			// 
			this.btnStart.Image = global::AutoXDD.Properties.Resources.Start;
			this.btnStart.Location = new System.Drawing.Point(32, 247);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 28);
			this.btnStart.TabIndex = 11;
			this.btnStart.Text = "开始";
			this.btnStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(11, 95);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(65, 12);
			this.label5.TabIndex = 5;
			this.label5.Text = "视频按钮：";
			// 
			// txtVideoButton
			// 
			this.txtVideoButton.BackColor = System.Drawing.SystemColors.Window;
			this.txtVideoButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.txtVideoButton.Location = new System.Drawing.Point(86, 92);
			this.txtVideoButton.Name = "txtVideoButton";
			this.txtVideoButton.ReadOnly = true;
			this.txtVideoButton.Size = new System.Drawing.Size(233, 21);
			this.txtVideoButton.TabIndex = 6;
			this.txtVideoButton.Click += new System.EventHandler(this.txtVideoButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(333, 334);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.txtVideoButton);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtVideoPos);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtArticlePos);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.txtTime);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnStart);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "AutoXDD";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label txtTime;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtArticlePos;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtVideoPos;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtVideoButton;
	}
}

