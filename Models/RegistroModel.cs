using MySql.Data.MySqlClient;
using ProyectoIMC.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIMC.Models
{
    public class RegistroModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public Sexo Sexo { get; set; }
        public double Estatura { get; set; }
        public double Peso { get; set; }

        public double IMC { get; set; }

        public string Clasificacion { set; get; } = "";
    }
}
