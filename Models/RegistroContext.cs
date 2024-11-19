using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIMC.Models
{
    public class RegistroContext
    {
        public MySqlConnection Connection = new MySqlConnection("Server=localhost;User=root;Password=root;Database=indicemasa;Port=3306");

        public void Conectar()
        {
            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }
        }

        public void Desonectar()
        {
            if (Connection.State == System.Data.ConnectionState.Open)
            {
                Connection.Close();
            }
        }

    }
}
