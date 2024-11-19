﻿using ProyectoIMC.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoIMC.Views
{
    /// <summary>
    /// Interaction logic for AgregarView.xaml
    /// </summary>
    public partial class AgregarView : UserControl
    {
        public AgregarView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbSexo.ItemsSource = Enum.GetValues(typeof(Sexo));

        }
    }
}
