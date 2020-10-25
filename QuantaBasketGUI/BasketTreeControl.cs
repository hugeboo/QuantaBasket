using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuantaBasket.Core.Interfaces;
using QuantaBasket.Core.Utils;

namespace QuantaBasketGUI
{
    public partial class BasketTreeControl : UserControl
    {
        private IBasketEngine _basketEngine;

        public event EventHandler<EventArgs<string>> NodeSelected;

        public BasketTreeControl()
        {
            InitializeComponent();
        }

        public void SetDataSource(IBasketEngine basketEngine)
        {
            _basketEngine = basketEngine;

            var quantasNode = treeView.Nodes.Find("Quantas", true)[0];
            quantasNode.Nodes.Clear();
            var quantasNames = _basketEngine.GetQuantasNames();
            foreach(var qn in quantasNames)
            {
                quantasNode.Nodes.Add($"Quant: {qn}", qn);
            }

            treeView.ExpandAll();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            NodeSelected?.Invoke(this, new EventArgs<string>(e.Node?.Name ?? null));
        }
    }
}
