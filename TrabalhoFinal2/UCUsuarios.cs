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
    public partial class UCUsuarios : UserControl
    {
        public UCUsuarios()
        {
            InitializeComponent();
            CarregarDadosNoGrid();
        }


        private void CarregarDadosNoGrid()
        {

            ConexaoBanco bd = new ConexaoBanco();
            try
            {
                bd.AbrirConexao();


                string sql = "SELECT nome, usuario, email, telefone FROM usuarios";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, bd.conectar);
                DataTable dt = new DataTable();


                da.Fill(dt);

                dgvUsuarios.DataSource = dt;
                dgvUsuarios.Columns["nome"].HeaderText = "Nome Completo";
                dgvUsuarios.Columns["usuario"].HeaderText = "Login";
                dgvUsuarios.Columns["email"].HeaderText = "E-mail";
                dgvUsuarios.Columns["telefone"].HeaderText = "Telefone";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar lista: " + ex.Message);
            }
        }


        private void FiltrarUsuarios(string busca)
        {

            ConexaoBanco bd = new ConexaoBanco();
            try
            {
                bd.AbrirConexao();


                string sql = "SELECT nome, usuario, email, telefone FROM usuarios WHERE nome LIKE @busca";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, bd.conectar);


                da.SelectCommand.Parameters.AddWithValue("@busca", busca + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvUsuarios.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar: " + ex.Message);
            }
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {

            FiltrarUsuarios(txtPesquisa.Text);
        }

        private void ExecutarBusca()
        {

            string termo = txtPesquisa.Text.Trim();


            FiltrarUsuarios(termo);
        }

        private void pctLupa_Click(object sender, EventArgs e)
        {
            ExecutarBusca();
        }


        private void txtBusca_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                ExecutarBusca();


                e.SuppressKeyPress = true;
            }
        }


        private void UC_ListaUsuarios_Load(object sender, EventArgs e)
        {
            CarregarDadosNoGrid();
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsuarios.Rows[e.RowIndex];


                txtNome.Text = row.Cells["nome"].Value.ToString();
                txtLogin.Text = row.Cells["usuario"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();
                txtTelefone.Text = row.Cells["telefone"].Value.ToString();
            }
        }

        private void botaoApagar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLogin.Text))
            {
                MessageBox.Show("Selecione um usuário na lista primeiro!");
                return;
            }

            //Pergunta se o usuário tem certeza
            DialogResult confirmacao = MessageBox.Show("Tem certeza que deseja excluir este usuário?",
                "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacao == DialogResult.Yes)
            {
                ConexaoBanco bd = new ConexaoBanco();
                try
                {
                    bd.AbrirConexao();

                    string sql = "DELETE FROM usuarios WHERE usuario = @user";

                    using (MySqlCommand cmd = new MySqlCommand(sql, bd.conectar))
                    {
                        cmd.Parameters.AddWithValue("@user", txtLogin.Text);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Usuário removido com sucesso!");

                        LimparCampos();

                        CarregarDadosNoGrid();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao apagar: " + ex.Message);
                }
            }
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

        private void botaoAtualizar_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtLogin.Text))
            {
                MessageBox.Show("Selecione um usuário na lista para editar!");
                return;
            }

            ConexaoBanco bd = new ConexaoBanco();
            try
            {
                bd.AbrirConexao();


                string sql = "UPDATE usuarios SET nome = @nome, email = @email, telefone = @telefone " +
                             "WHERE usuario = @usuario";

                using (MySqlCommand cmd = new MySqlCommand(sql, bd.conectar))
                {

                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    cmd.Parameters.AddWithValue("@usuario", txtLogin.Text);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        MessageBox.Show("Dados atualizados com sucesso!");


                        CarregarDadosNoGrid();


                        LimparCampos();
                    }
                    else
                    {
                        MessageBox.Show("Nenhum usuário encontrado com esse login.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar: " + ex.Message);
            }

        }
    }
}
