    using MySql.Data.MySqlClient;
using ProyectoIMC.Models;
using ProyectoIMC.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIMC.Repositories
{
    public class RegistroRepository
    {
        RegistroContext context = new RegistroContext();

        MySqlCommand command = new MySqlCommand();
        MySqlDataReader lector;
        List<RegistroModel> Registos = new List<RegistroModel>();



        public IEnumerable<RegistroModel> GetAllRegistros()
        {
            Registos.Clear();

            context.Conectar();
            command.CommandText = "Select * from Registro order by Nombre";
            command.Connection = context.Connection;
            lector = command.ExecuteReader();

            while (lector.Read())
            {
                RegistroModel r = new RegistroModel()
                {

                    Id = lector.GetInt32("Id"),
                    Nombre = lector.GetString("Nombre"),
                    Sexo = lector.GetInt32("Sexo") == 1 ? Sexo.Hombre : Sexo.Mujer,
                    Estatura = lector.GetDouble("Estatura"),
                    Peso = lector.GetDouble("Peso"),
                    IMC = lector.GetDouble("IMC"),
                    Clasificacion = lector.GetString("Clasificacion")
                };
                Registos.Add(r);
            }
            lector.Close();

            context.Desonectar();
            return Registos;
        }

        public bool Validar(RegistroModel r, out string error)
        {
            List<string> Errores = new List<string>();
            if (string.IsNullOrEmpty(r.Nombre))
            {
                Errores.Add("Indique un nombre para poder guardar su registro");
            }

            if (r.Sexo != Sexo.Hombre & r.Sexo != Sexo.Mujer)
            {
                Errores.Add("Indique un sexo válido");
            }

            if (r.Estatura > 2.99 || r.Estatura < 0.90)
            {
                Errores.Add("Indique su estatura correctamente");
            }

            if (r.Peso < 40 || r.Peso > 300)
            {
                Errores.Add("Indique correctamente su peso");
            }

            error = string.Join(",\n", Errores);

            return Errores.Count != 0;
        }



        public long ContarClasificacion(string clasificacion)
        {
            context.Conectar();

            command.CommandText = $"Select Count(*) From Registro Where Clasificacion = '{clasificacion}'";
            command.Connection = context.Connection;
            long c = (long)command.ExecuteScalar();

            context.Desonectar();
            return c;
        }

        public decimal RatioHM()
        {
            context.Conectar();

            command.CommandText = $"SELECT (COUNT(CASE WHEN sexo = 2 THEN 1 END) * 100.0 / COUNT(*)) AS porcentaje_mujeres FROM Registro";
            command.Connection = context.Connection;
            decimal ratio = (decimal)command.ExecuteScalar();

            context.Desonectar();
            return ratio;
        }

        public double PromedioPeso()
        {
            context.Conectar();

            command.CommandText = $"Select AVG(Peso) fROM Registro";
            command.Connection = context.Connection;
            double Peso = (double)command.ExecuteScalar();

            context.Desonectar();

            return Math.Round(Peso,2);
        }

        public double PromedioEstatura()
        {
            context.Conectar();

            command.CommandText = $"Select AVG(Estatura) fROM Registro";
            command.Connection = context.Connection;
            double Estatura = (double)command.ExecuteScalar();

            context.Desonectar();

            return Math.Round(Estatura,2);
        }


        public void Eliminar(RegistroModel r)
        {
            context.Conectar();

            command.CommandText = $"DELETE from registro WHERE ID = {r.Id}";
            command.Connection = context.Connection;
            command.ExecuteNonQuery();

            context.Desonectar();

        }

        public void Agregar(RegistroModel r)
        {
            context.Conectar();

            double IMC = Math.Round( r.Peso / Math.Pow(r.Estatura, 2),2);

            string Clasificacion = "";
            switch (IMC)
            {
                case < 18.5:
                    Clasificacion = "Bajo peso";
                    break;
                case >= 18.5 and < 25:
                    Clasificacion = "Peso normal";
                    break;
                case >= 25 and < 30:
                    Clasificacion = "Sobrepeso";
                    break;
                default:
                    Clasificacion = "Obesidad";
                    break;
            }

            command.CommandText = $"Insert INTO registro (Nombre, Sexo, Estatura, Peso, IMC, Clasificacion) VALUES('{(r.Nombre)}', {(r.Sexo == Sexo.Hombre ? 1 : 2)}, {(r.Estatura)}, {(r.Peso)}, {(IMC)}, '{(Clasificacion)}');";
            command.Connection = context.Connection;
            command.ExecuteNonQuery();

            context.Desonectar();

        }
    }
}
