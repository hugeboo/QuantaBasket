﻿using NLog;
using QuantaBasket.Basket;
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
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                _basketEngine = new BasketEngine();
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, $"Fatal error:\n{ex.Message}", "QuantaBasket", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _basketEngine?.Dispose();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                _basketEngine?.Start();
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
