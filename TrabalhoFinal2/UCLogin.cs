using MySql.Data.MySqlClient;
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
    public partial class UCLogin : UserControl
    {

        public event EventHandler EventoIrParaCadastro;

        public UCLogin()
        {
            InitializeComponent();
        }

        public event EventHandler LoginSucesso;
        private void botaoEntrar_Click(object sender, EventArgs e)
        {


                ConexaoBanco bd = new ConexaoBanco();
                try
                {
                    bd.AbrirConexao();
                    
                    string sql = "SELECT COUNT(*) FROM usuarios WHERE LOWER(usuario) = LOWER(@user) AND senha = @pass";

                    using (MySqlCommand cmd = new MySqlCommand(sql, bd.conectar))
                    {
                        
                        cmd.Parameters.AddWithValue("@user", txtUsuario.Text.ToLower().Trim());
                        cmd.Parameters.AddWithValue("@pass", txtSenha.Text);

                        int resultado = Convert.ToInt32(cmd.ExecuteScalar());

                        if (resultado > 0)
                        {
                            
                            LoginSucesso?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            MessageBox.Show("Usuário ou senha incorretos.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }

        }

        
        private void botaoCadastro_Click(object sender, EventArgs e)
        {
            EventoIrParaCadastro?.Invoke(this, EventArgs.Empty);
        }
    }
}
