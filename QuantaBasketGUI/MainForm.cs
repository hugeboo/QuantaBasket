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
        private IBasketEngine _basketEngine;
        private object _selectedBasketTreeObject = null;
        private string _selectedBasketTreeNodeName = null;

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
                _basketEngine = BasketFactory.CreateBasket();
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
            
            saveConfigurationToolStripButton.Enabled = !(_basketEngine?.Started ?? false) && 
                (_selectedBasketTreeObject is IHaveConfiguration);

            var bt = _basketEngine?.Now?.ToString("dd.MM.yy HH.mm.ss") ?? "???";
            basketTimeToolStripStatusLabel.Text = "Basket Time: " + bt;
        }

        private void BasketTreeControl_NodeSelected(object sender, QuantaBasket.Core.Utils.EventArgs<string> e)
        {
            object selectedObject = null;

            switch (e.Data)
            {
                case "Basket":
                    selectedObject = _basketEngine;
                    break;
                case "L1QuotationProvider":
                    selectedObject = _basketEngine?.L1QuotationProvider;
                    break;
                case "L1QuotationStore":
                    selectedObject = _basketEngine?.L1QuotationStore;
                    break;
                case "Trader":
                    selectedObject = _basketEngine?.TradingEngine;
                    break;
                case "TradingSystem":
                    selectedObject = _basketEngine?.TradingEngine?.TradingSystem;
                    break;
                case "TradingStore":
                    selectedObject = _basketEngine?.TradingEngine?.TradingStore;
                    break;
                default:
                    if (!string.IsNullOrEmpty(e.Data) && e.Data.StartsWith("Quant: ") && e.Data.Length > "Quant: ".Length)
                    {
                        var quantName = e.Data.Substring("Quant: ".Length);
                        selectedObject = _basketEngine?.GetQuant(quantName);
                    }
                    break;
            }

            _selectedBasketTreeObject = selectedObject;
            _selectedBasketTreeNodeName = e.Data;

            var ihc = selectedObject as IHaveConfiguration;
            propertyGrid.SelectedObject = ihc != null ? ihc.GetConfiguration() : null;
        }

        private void saveConfigurationToolStripButton_Click(object sender, EventArgs e)
        {
            (_selectedBasketTreeObject as IHaveConfiguration)?.SaveConfiguration();
        }
    }
}
