namespace QuantaBasketGUI
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.logTabPage = new System.Windows.Forms.TabPage();
            this.logControl = new QuantaBasketGUI.LogControl();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.startBasketToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.stopBasketToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveConfigurationToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.basketTreeControl = new QuantaBasketGUI.BasketTreeControl();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.configTabPage = new System.Windows.Forms.TabPage();
            this.basketTimeToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl.SuspendLayout();
            this.logTabPage.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.configTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.logTabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(4, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(788, 154);
            this.tabControl.TabIndex = 0;
            // 
            // logTabPage
            // 
            this.logTabPage.Controls.Add(this.logControl);
            this.logTabPage.Location = new System.Drawing.Point(4, 22);
            this.logTabPage.Name = "logTabPage";
            this.logTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.logTabPage.Size = new System.Drawing.Size(780, 128);
            this.logTabPage.TabIndex = 0;
            this.logTabPage.Text = "Log";
            this.logTabPage.UseVisualStyleBackColor = true;
            // 
            // logControl
            // 
            this.logControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logControl.Location = new System.Drawing.Point(3, 3);
            this.logControl.Name = "logControl";
            this.logControl.Size = new System.Drawing.Size(774, 122);
            this.logControl.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.basketTimeToolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 421);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(796, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startBasketToolStripButton,
            this.stopBasketToolStripButton,
            this.saveConfigurationToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(796, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip";
            // 
            // startBasketToolStripButton
            // 
            this.startBasketToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.startBasketToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("startBasketToolStripButton.Image")));
            this.startBasketToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.startBasketToolStripButton.Name = "startBasketToolStripButton";
            this.startBasketToolStripButton.Size = new System.Drawing.Size(72, 22);
            this.startBasketToolStripButton.Text = "Start Basket";
            this.startBasketToolStripButton.Click += new System.EventHandler(this.startBasketToolStripButton_Click);
            // 
            // stopBasketToolStripButton
            // 
            this.stopBasketToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stopBasketToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("stopBasketToolStripButton.Image")));
            this.stopBasketToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopBasketToolStripButton.Name = "stopBasketToolStripButton";
            this.stopBasketToolStripButton.Size = new System.Drawing.Size(72, 22);
            this.stopBasketToolStripButton.Text = "Stop Basket";
            this.stopBasketToolStripButton.Click += new System.EventHandler(this.stopBasketToolStripButton_Click);
            // 
            // saveConfigurationToolStripButton
            // 
            this.saveConfigurationToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.saveConfigurationToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveConfigurationToolStripButton.Image")));
            this.saveConfigurationToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveConfigurationToolStripButton.Name = "saveConfigurationToolStripButton";
            this.saveConfigurationToolStripButton.Size = new System.Drawing.Size(112, 22);
            this.saveConfigurationToolStripButton.Text = "Save Configuration";
            this.saveConfigurationToolStripButton.Click += new System.EventHandler(this.saveConfigurationToolStripButton_Click);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 200;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(579, 208);
            this.propertyGrid.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 0);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.splitContainer1.Size = new System.Drawing.Size(796, 396);
            this.splitContainer1.SplitterDistance = 238;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(4, 4);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.basketTreeControl);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.mainTabControl);
            this.splitContainer2.Size = new System.Drawing.Size(788, 234);
            this.splitContainer2.SplitterDistance = 197;
            this.splitContainer2.TabIndex = 0;
            // 
            // basketTreeControl
            // 
            this.basketTreeControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basketTreeControl.Location = new System.Drawing.Point(0, 0);
            this.basketTreeControl.Name = "basketTreeControl";
            this.basketTreeControl.Size = new System.Drawing.Size(197, 234);
            this.basketTreeControl.TabIndex = 0;
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.configTabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(587, 234);
            this.mainTabControl.TabIndex = 1;
            // 
            // configTabPage
            // 
            this.configTabPage.Controls.Add(this.propertyGrid);
            this.configTabPage.Location = new System.Drawing.Point(4, 22);
            this.configTabPage.Name = "configTabPage";
            this.configTabPage.Size = new System.Drawing.Size(579, 208);
            this.configTabPage.TabIndex = 0;
            this.configTabPage.Text = "Configuration";
            this.configTabPage.UseVisualStyleBackColor = true;
            // 
            // basketTimeToolStripStatusLabel
            // 
            this.basketTimeToolStripStatusLabel.Name = "basketTimeToolStripStatusLabel";
            this.basketTimeToolStripStatusLabel.Size = new System.Drawing.Size(91, 17);
            this.basketTimeToolStripStatusLabel.Text = "Basket Time: ???";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 443);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QantaBasket";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl.ResumeLayout(false);
            this.logTabPage.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.mainTabControl.ResumeLayout(false);
            this.configTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LogControl logControl;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage logTabPage;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton startBasketToolStripButton;
        private System.Windows.Forms.ToolStripButton stopBasketToolStripButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage configTabPage;
        private BasketTreeControl basketTreeControl;
        private System.Windows.Forms.ToolStripButton saveConfigurationToolStripButton;
        private System.Windows.Forms.ToolStripStatusLabel basketTimeToolStripStatusLabel;
    }
}

