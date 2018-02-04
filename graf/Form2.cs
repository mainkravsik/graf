using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace graf
{
    public partial class Form2 : Form
    {
        private Button but1; // эта переменная будет содержать ссылку на кнопку button1 из формы Form1
        private PictureBox pic1;
        public Form2(Button but,PictureBox pic) // получаем ссылку на кнопку в переменную but
        {
            pic1 = pic;
            but1 = but; // теперь but1 будет ссылкой на кнопку button1
            InitializeComponent();
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Close(object sender, MouseEventArgs e)
        {
            pic1.Enabled = true;
            but1.Enabled = true;
            Close();
        }

        private void Close(object sender, EventArgs e)
        {
            pic1.Enabled = true;
            but1.Enabled = true;
            Close();
        }
    }
}
