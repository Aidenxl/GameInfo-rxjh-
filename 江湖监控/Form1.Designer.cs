namespace 江湖监控
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listView1 = new System.Windows.Forms.ListView();
            this.ListViewAccount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListViewName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListViewGrade = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListViewCurrentExperience = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListViewNeedExperience = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListViewExperienceRatio = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListViewWX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListViewCurrentGold = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListViewMinutesGold = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListViewMinutesExperience = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListViewUpGradeTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListViewLoginTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ListViewAccount,
            this.ListViewName,
            this.ListViewGrade,
            this.ListViewCurrentExperience,
            this.ListViewNeedExperience,
            this.ListViewExperienceRatio,
            this.ListViewWX,
            this.ListViewCurrentGold,
            this.ListViewMinutesGold,
            this.ListViewMinutesExperience,
            this.ListViewUpGradeTime,
            this.ListViewLoginTime});
            this.listView1.Location = new System.Drawing.Point(0, 1);
            this.listView1.Name = "listView1";
            this.listView1.Scrollable = false;
            this.listView1.Size = new System.Drawing.Size(884, 449);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.listView1_ColumnWidthChanging);
            // 
            // ListViewAccount
            // 
            this.ListViewAccount.Text = "账号";
            this.ListViewAccount.Width = 83;
            // 
            // ListViewName
            // 
            this.ListViewName.Text = "名字";
            this.ListViewName.Width = 87;
            // 
            // ListViewGrade
            // 
            this.ListViewGrade.Text = "等级";
            this.ListViewGrade.Width = 39;
            // 
            // ListViewCurrentExperience
            // 
            this.ListViewCurrentExperience.Text = "当前经验";
            this.ListViewCurrentExperience.Width = 79;
            // 
            // ListViewNeedExperience
            // 
            this.ListViewNeedExperience.Text = "需要经验";
            this.ListViewNeedExperience.Width = 76;
            // 
            // ListViewExperienceRatio
            // 
            this.ListViewExperienceRatio.Text = "经验百分比";
            this.ListViewExperienceRatio.Width = 74;
            // 
            // ListViewWX
            // 
            this.ListViewWX.Text = "武勋";
            this.ListViewWX.Width = 61;
            // 
            // ListViewCurrentGold
            // 
            this.ListViewCurrentGold.Text = "游戏币";
            this.ListViewCurrentGold.Width = 71;
            // 
            // ListViewMinutesGold
            // 
            this.ListViewMinutesGold.Text = "金币/分钟";
            this.ListViewMinutesGold.Width = 71;
            // 
            // ListViewMinutesExperience
            // 
            this.ListViewMinutesExperience.Text = "经验/分钟";
            this.ListViewMinutesExperience.Width = 67;
            // 
            // ListViewUpGradeTime
            // 
            this.ListViewUpGradeTime.Text = "剩余升级时间";
            this.ListViewUpGradeTime.Width = 84;
            // 
            // ListViewLoginTime
            // 
            this.ListViewLoginTime.Text = "在线时间";
            this.ListViewLoginTime.Width = 87;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(887, 27);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(132, 424);
            this.listBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(890, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "掉线账号列表：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 452);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.listView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "江湖挂机数据监控";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ListViewName;
        private System.Windows.Forms.ColumnHeader ListViewGrade;
        private System.Windows.Forms.ColumnHeader ListViewCurrentExperience;
        private System.Windows.Forms.ColumnHeader ListViewNeedExperience;
        private System.Windows.Forms.ColumnHeader ListViewExperienceRatio;
        private System.Windows.Forms.ColumnHeader ListViewWX;
        private System.Windows.Forms.ColumnHeader ListViewCurrentGold;
        private System.Windows.Forms.ColumnHeader ListViewAccount;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader ListViewMinutesExperience;
        private System.Windows.Forms.ColumnHeader ListViewUpGradeTime;
        private System.Windows.Forms.ColumnHeader ListViewMinutesGold;
        private System.Windows.Forms.ColumnHeader ListViewLoginTime;
    }
}

