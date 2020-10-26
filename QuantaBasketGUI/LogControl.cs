using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuantaBasket.Core.Utils;

namespace QuantaBasketGUI
{
    public partial class LogControl : UserControl
    {
        private const int MAX_LINE_COUNT = 500;

        public LogControl()
        {
            InitializeComponent();
        }

        public void SetLogStore(ILogStore logStore)
        {
            logStore.NewMessage += LogStore_NewMessage;
        }

        private void LogStore_NewMessage(object sender, EventArgs<string> e)
        {
            this.BeginInvoke(new Action(() =>
            {
                var lines = textBox.Lines;
                if (textBox.Lines.Length > MAX_LINE_COUNT)
                {
                    var lst = lines.ToList();
                    lst.RemoveRange(0, lines.Length - MAX_LINE_COUNT);
                    textBox.Lines = lst.ToArray();
                }

                textBox.AppendText(e.Data + "\r\n\r\n");
            }));
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
