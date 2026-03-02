using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TrabalhoFinal2
{
    internal class ConexaoBanco
    {
        private string strConexao = "Server=localhost;Database=fastH;Uid=root;Pwd=23571113;";
        public MySqlConnection conectar = null;

        public void AbrirConexao()
        {
            try
            {
                conectar = new MySqlConnection(strConexao);
                conectar.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao conectar ao banco: " + ex.Message);
            }
        }

        public void FecharConexao()
        {
            try
            {
                if (conectar != null && conectar.State == System.Data.ConnectionState.Open)
                {
                    conectar.Close();
                    conectar.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao fechar conexão: " + ex.Message);
            }
        }
    }
}
