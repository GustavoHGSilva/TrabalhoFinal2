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
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void CarregarUserControl(UserControl uc)
        {
            
            panelLogin.Controls.Clear();

            uc.Dock = DockStyle.Fill;

            panelLogin.Controls.Add(uc);
        }


        public event EventHandler EventoIrParaCadastro;

        private void Form1_Load(object sender, EventArgs e)
        {
            ExibirLogin();
        }

        public void ExibirLogin()
        {
            UCLogin login = new UCLogin();

            login.LoginSucesso += (s, args) => {

                Form2 principal = new Form2();

                
                principal.Show();

                
                principal.FormClosed += (s2, args2) => this.Close();

                
                this.Hide();
            };

            
            login.EventoIrParaCadastro += (s, args) => ExibirCadastro();

            CarregarUserControl(login);
        }


        public void ExibirCadastro()
        {
            UCCadastro cadastro = new UCCadastro();

            
            cadastro.EventoVoltarParaLogin += (s, args) => ExibirLogin();

            CarregarUserControl(cadastro);
        }

        
    }
}
