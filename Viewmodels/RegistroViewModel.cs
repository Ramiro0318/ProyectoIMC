using CommunityToolkit.Mvvm.Input;
using ProyectoIMC.Models;
using ProyectoIMC.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoIMC.Viewmodels
{
    public enum Vistas { AgregarView, EliminarView, VerView }

    public enum Sexo { Hombre = 1, Mujer = 2 }
    public class RegistroViewModel : INotifyPropertyChanged
    {
        public Vistas Vista { get; set; } = Vistas.VerView;

        public RegistroRepository repos = new();
        public RegistroModel? Registro { get; set; }

        //public string Clasificacion; 
        public ObservableCollection<RegistroModel> Registros { get; set; } = new ObservableCollection<RegistroModel>();
        public long BajoPeso { set; get; }
        public long PesoNomral { set; get; }
        public long Sobrepeso { set; get; }
        public long Obesidad { set; get; }
        public double Ratio { set; get; }
        public double AvgPeso { set; get; }
        public double AvgEstatura { set; get; }

        private string error;
        public string Error { set { error = value; } get { return error; } }

        public string Tooltip { set; get; } = "Por debajo de 18.5 Bajo peso \n18.5 - 24.9 Peso normal \n25.0 - 29.9 Sobrepeso \n30.0 o más Obesidad";
        public string Porcentaje { set; get; }

        public ICommand IrAgregarCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand IrEliminarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }




        public RegistroViewModel()
        {
            Actualizar(null);   

            IrAgregarCommand = new RelayCommand(IrAgregar);
            AgregarCommand = new RelayCommand(Agregar);
            IrEliminarCommand = new RelayCommand<RegistroModel>(IrEliminar);
            EliminarCommand = new RelayCommand(Eliminar);
            CancelarCommand = new RelayCommand(Cancelar);
        }



        private void IrAgregar()
        {
            Vista = Vistas.AgregarView;
            Registro = new RegistroModel();
            Error = "";
            Actualizar(nameof(Vista));
            Actualizar(nameof(Error));
        }
        private void Agregar()
        {
            if (!repos.Validar(Registro, out error))
            {
                repos.Agregar(Registro);
                Cancelar();
            }
            Error = error;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Error));
        }


        private void IrEliminar(RegistroModel? r)
        {
            Registro = r;
            if (Registro != null)
            {
                Vista = Vistas.EliminarView;
                Actualizar(nameof(Vista));
            }
        }
        private void Eliminar()
        {
            if (Registro != null)
            {
                repos.Eliminar(Registro);
                Cancelar();
            }
        }

        private void Cancelar()
        {
            Vista = Vistas.VerView;
            Actualizar(nameof(Vista));
        }

        public void Actualizar(string? name)
        {
            Registros.Clear();

            foreach (RegistroModel r in repos.GetAllRegistros())
            {
                Registros.Add(r);
            }

            BajoPeso = repos.ContarClasificacion("Bajo peso");
            PesoNomral = repos.ContarClasificacion("Peso normal");
            Sobrepeso = repos.ContarClasificacion("Sobrepeso");
            Obesidad = repos.ContarClasificacion("Obesidad");
            AvgEstatura = repos.PromedioEstatura();
            AvgPeso = repos.PromedioPeso();
            Ratio = (int)repos.RatioHM();

            Porcentaje = $"{Ratio}% Mujeres \n{100 - Ratio}% Hombres";

            PropertyChanged?.Invoke(this, new(name));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
