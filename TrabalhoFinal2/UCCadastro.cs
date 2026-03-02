using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TrabalhoFinal2
{
    public partial class UCCadastro : UserControl
    {

        public event EventHandler EventoVoltarParaLogin;


        public UCCadastro()
        {
            InitializeComponent();
        }


        private void botaoVoltar_Click(object sender, EventArgs e)
        {
            EventoVoltarParaLogin?.Invoke(this, EventArgs.Empty);
        }


        private void LimparCampos()
        {
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Clear();
                }
            }
        }


        private void botaoFinalizar_Click(object sender, EventArgs e)
        {

            ConexaoBanco bd = new ConexaoBanco();
            try
            {
                bd.AbrirConexao();


                string sql = "INSERT INTO usuarios (nome, usuario, email, telefone, senha) " +
                             "VALUES (@nome, @usuario, @email, @telefone, @senha)";

                using (MySqlCommand cmd = new MySqlCommand(sql, bd.conectar))
                {

                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    cmd.Parameters.AddWithValue("@senha", txtSenha.Text);


                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Usuário cadastrado com sucesso!");


                    LimparCampos();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar ou salvar: " + ex.Message);
            }
        }
    }
}
