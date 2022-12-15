using Labirint;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Forms;
using Labirint.LabirintsClasses;
using static System.Net.Mime.MediaTypeNames;

namespace Labirint
{
    public partial class Form1 : Form
    {
        LabirintBase labirint;
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "Идеальный")
            {
                labirint = new PerfectLabirint(Convert.ToInt32(comboBox1.SelectedItem), Convert.ToInt32(comboBox3.SelectedItem), 10, pictureBox1, checkBox1.Checked, checkBox2.Checked);
            }
            else if (comboBox2.SelectedItem.ToString() == "Плетенный")
            {
                labirint = new WickerLabirint(Convert.ToInt32(comboBox1.SelectedItem), Convert.ToInt32(comboBox3.SelectedItem), 10, pictureBox1, checkBox1.Checked, checkBox2.Checked);
            }
            else if (comboBox2.SelectedItem.ToString() == "Одномаршрутный")
            {
                labirint = new SingleRouteLabirint(Convert.ToInt32(comboBox1.SelectedItem), Convert.ToInt32(comboBox3.SelectedItem), 10, pictureBox1, checkBox1.Checked, checkBox2.Checked);
            }
            else if (comboBox2.SelectedItem.ToString() == "Рандомные проходы")
            {
                labirint = new RandomLabirint(Convert.ToInt32(comboBox1.SelectedItem), Convert.ToInt32(comboBox3.SelectedItem), 10, pictureBox1, checkBox1.Checked,checkBox2.Checked);
            }
            labirint.Generate();
            labirint.Draw();
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            labirint.Save();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "Идеальный")
            {
                checkBox1.Checked = true;
                checkBox1.Enabled = true;
                checkBox2.Checked = true;
                checkBox2.Enabled = true;
            }
            else if (comboBox2.SelectedItem.ToString() == "Плетенный")
            {
                checkBox1.Checked = true;
                checkBox1.Enabled = true;
                checkBox2.Checked = true;
                checkBox2.Enabled = true;
            }
            else if (comboBox2.SelectedItem.ToString() == "Одномаршрутный")
            {
                checkBox1.Checked = true;
                checkBox1.Enabled = true;
                checkBox2.Checked = true;
                checkBox2.Enabled = true;
            }
            else if (comboBox2.SelectedItem.ToString() == "Рандомные проходы")
            {
                checkBox1.Checked = true;
                checkBox1.Enabled = true;
                checkBox2.Checked = false;
                checkBox2.Enabled = false;
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if(labirint != null)
            {
                labirint.MouseClickOnLabirint(pictureBox1, e);
            }
        }
    }
}