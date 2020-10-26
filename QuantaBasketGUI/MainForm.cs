using NLog;
using QuantaBasket.Basket;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuantaBasketGUI
{
    public partial class MainForm : Form
    {
        private BasketEngine _basketEngine;

        private readonly ILogger _logger = LogManager.GetLogger("MainForm");
        public MainForm()
        {
            InitializeComponent();

            logControl.SetLogStore(LogStore.Default);
            basketTreeControl.NodeSelected += BasketTreeControl_NodeSelected;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                _basketEngine = new BasketEngine();
                basketTreeControl.SetDataSource(_basketEngine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Fatal error:\n{ex.Message}", "QuantaBasket", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _basketEngine?.Dispose();
        }

        private void startBasketToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                _basketEngine?.Start();
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show(this, $"{ex.Message}", "QuantaBasket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void stopBasketToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                _basketEngine?.Stop();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show(this, $"{ex.Message}", "QuantaBasket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            startBasketToolStripButton.Enabled = !_basketEngine?.Started ?? false;
            stopBasketToolStripButton.Enabled = _basketEngine?.Started ?? false;
        }

        private void BasketTreeControl_NodeSelected(object sender, QuantaBasket.Core.Utils.EventArgs<string> e)
        {
            IHaveConfiguration ihc = null;
         
            switch (e.Data)
            {
                case "Basket":
                    ihc = _basketEngine as IHaveConfiguration;
                    break;
                case "L1QuotationProvider":
                    ihc = _basketEngine?.L1QuotationProvider as IHaveConfiguration;
                    break;
                case "L1QuotationStore":
                    ihc = _basketEngine?.L1QuotationStore as IHaveConfiguration;
                    break;
                case "Trader":
                    ihc = _basketEngine?.TradingEngine as IHaveConfiguration;
                    break;
                case "TradingSystem":
                    ihc = _basketEngine?.TradingEngine?.TradingSystem as IHaveConfiguration;
                    break;
                case "TradingStore":
                    ihc = _basketEngine?.TradingEngine?.TradingStore as IHaveConfiguration;
                    break;
                default:
                    if (!string.IsNullOrEmpty(e.Data) && e.Data.StartsWith("Quant: ") && e.Data.Length > "Quant: ".Length)
                    {
                        var quantName = e.Data.Substring("Quant: ".Length);
                        ihc = _basketEngine?.GetQuant(quantName) as IHaveConfiguration;
                    }
                    break;
            }

            propertyGrid.SelectedObject = ihc != null ? ihc.GetConfiguration() : null;
        }
    }
}
