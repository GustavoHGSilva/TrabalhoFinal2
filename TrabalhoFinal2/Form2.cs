using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrabalhoFinal2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void botaoSair_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }

        private void lblUsuario_Click(object sender, EventArgs e)
        {

            UCUsuarios uc = new UCUsuarios();

            panelPrincipal.Controls.Clear();


            uc.Dock = DockStyle.Fill;


            panelPrincipal.Controls.Add(uc);
        }

        private void lblCRUD_Click(object sender, EventArgs e)
        {
            UCCRUD uc = new UCCRUD();

            panelPrincipal.Controls.Clear();


            uc.Dock = DockStyle.Fill;


            panelPrincipal.Controls.Add(uc);
        }
    }
}
