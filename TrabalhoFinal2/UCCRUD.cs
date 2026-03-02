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
    public partial class UCCRUD : UserControl
    {
        public UCCRUD()
        {
            InitializeComponent();
        }


        private void PreencherComboTecnicos()
        {
            string connectionString = "Server=localhost;Database=fastH;Uid=root;Pwd=23571113;";
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();
                    string sql = "SELECT usuario_ID, nome FROM usuarios"; 
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conexao);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbTecnico.DataSource = dt;
                    cmbTecnico.DisplayMember = "nome"; 
                    cmbTecnico.ValueMember = "usuario_ID";    

                    cmbTecnico.SelectedIndex = -1; 
                }
                catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
            }
        }


        private void CarregarDadosNoGrid()
        {

            string connectionString = "Server=localhost;Database=fastH;Uid=root;Pwd=23571113;";

            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();


                    string sql = "SELECT s.servico_id, u.nome as Tecnico, s.cliente, s.telefone, s.problema, s.statusP, s.tecnico_id " +
                    "FROM servicos s " +
                     "INNER JOIN usuarios u ON s.tecnico_id = u.usuario_ID";

                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conexao);
                    DataTable dt = new DataTable();
                    da.Fill(dt);


                    dgvAssistencia.DataSource = dt;


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar a lista de assistência: " + ex.Message);
                }
            }
        }

        private void LimparCampos()
        {

            txtCliente.Clear();
            txtTelefone.Clear();
            txtProblema.Clear();

            cmbTecnico.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;

            cmbTecnico.Focus();
        }


        private void UCCRUD_Load(object sender, EventArgs e)
        {
            PreencherComboTecnicos();
            CarregarDadosNoGrid();

        }

        private void botaoAdicionar_Click(object sender, EventArgs e)
        {
            if (cmbTecnico.SelectedValue == null)
            {
                MessageBox.Show("Selecione um técnico antes de adicionar!");
                return;
            }


            string connectionString = "Server=localhost;Database=fastH;Uid=root;Pwd=23571113;";
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();
                    string sql = "INSERT INTO servicos (tecnico_id, cliente, telefone, problema, statusP) " +
                                 "VALUES (@tecnico_id, @cliente, @telefone, @problema, @status)";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@tecnico_id", Convert.ToInt32(cmbTecnico.SelectedValue));
                        cmd.Parameters.AddWithValue("@cliente", txtCliente.Text);
                        cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                        cmd.Parameters.AddWithValue("@problema", txtProblema.Text);
                        cmd.Parameters.AddWithValue("@status", cmbStatus.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Ordem de serviço adicionada!");

                        CarregarDadosNoGrid();
                        LimparCampos();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
            }
        }



        private void dgvAssistencia_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAssistencia.Rows[e.RowIndex];

               
                txtIdServico.Text = row.Cells["servico_id"].Value.ToString();
                txtCliente.Text = row.Cells["cliente"].Value.ToString();
                txtTelefone.Text = row.Cells["telefone"].Value.ToString();
                txtProblema.Text = row.Cells["problema"].Value.ToString();
                cmbStatus.Text = row.Cells["statusP"].Value.ToString();

 
                cmbTecnico.SelectedValue = row.Cells["tecnico_id"].Value;
            }

        }

        private void botaoAtualizar_Click(object sender, EventArgs e)
        {


            if (cmbTecnico.SelectedValue == null || string.IsNullOrEmpty(txtIdServico.Text))
            {
                MessageBox.Show("Selecione um registro e um técnico válido!");
                return;
            }

            string connectionString = "Server=localhost;Database=fastH;Uid=root;Pwd=23571113;";
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();
                    string sql = "UPDATE servicos SET tecnico_id = @tecnico_id, cliente = @cliente, " +
                                 "telefone = @telefone, problema = @problema, statusP = @status " +
                                 "WHERE servico_id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                    {
                        
                        cmd.Parameters.AddWithValue("@tecnico_id", Convert.ToInt32(cmbTecnico.SelectedValue));
                        cmd.Parameters.AddWithValue("@cliente", txtCliente.Text);
                        cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                        cmd.Parameters.AddWithValue("@problema", txtProblema.Text);
                        cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                        cmd.Parameters.AddWithValue("@id", txtIdServico.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Registro atualizado com sucesso!");

                        CarregarDadosNoGrid();
                        LimparCampos();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao atualizar: " + ex.Message);
                }
            }
        }

        private void botaoApagar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdServico.Text))
            {
                MessageBox.Show("Por favor, selecione uma ordem de serviço na lista para excluir.");
                return;
            }

            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir esta ordem de serviço? Esta ação não pode ser desfeita!",
                "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                string connectionString = "Server=localhost;Database=fastH;Uid=root;Pwd=23571113;";

                using (MySqlConnection conexao = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conexao.Open();

  
                        string sql = "DELETE FROM servicos WHERE servico_id = @id";

                        using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                        {
                            cmd.Parameters.AddWithValue("@id", txtIdServico.Text);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Ordem de serviço excluída com sucesso!");

             
                            CarregarDadosNoGrid();
                            LimparCampos();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir: " + ex.Message);
                    }
                }
            }
        }
    }
}
