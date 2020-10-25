namespace QuantaBasketGUI
{
    partial class BasketTreeControl
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("L1QuotationStore");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("L1QuotationProvider", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("TradingSystem");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("TradingStore");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Trader", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Quantas");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Basket", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode5,
            treeNode6});
            this.treeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.FullRowSelect = true;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            treeNode1.Name = "L1QuotationStore";
            treeNode1.Text = "L1QuotationStore";
            treeNode2.Name = "L1QuotationProvider";
            treeNode2.Text = "L1QuotationProvider";
            treeNode3.Name = "TradingSystem";
            treeNode3.Text = "TradingSystem";
            treeNode4.Name = "TradingStore";
            treeNode4.Text = "TradingStore";
            treeNode5.Name = "Trader";
            treeNode5.Text = "Trader";
            treeNode6.Name = "Quantas";
            treeNode6.Text = "Quantas";
            treeNode7.Name = "Basket";
            treeNode7.Text = "Basket";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7});
            this.treeView.Size = new System.Drawing.Size(213, 305);
            this.treeView.TabIndex = 1;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // BasketTreeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView);
            this.Name = "BasketTreeControl";
            this.Size = new System.Drawing.Size(213, 305);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
    }
}
