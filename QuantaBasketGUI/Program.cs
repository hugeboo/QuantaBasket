﻿using NLog.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuantaBasketGUI
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ConfigurationItemFactory
                      .Default
                      .Targets
                      .RegisterDefinition("QuantaBasketGUI", typeof(QuantaBasketGUI.NLogCustomTarget));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
