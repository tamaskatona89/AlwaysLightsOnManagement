﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
using AlwaysLightsOnDataModelsDLL;
using AlwaysLightsOnManagement;
using Microsoft.EntityFrameworkCore;

namespace Desktop_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DBServices DBServicesInstance { get; set; } = new DBServices();
        public MainWindow()
        {
            InitializeComponent();


        }

        private void year_textBox_Loaded(object sender, RoutedEventArgs e)
        {
            year_textBox.Text = DateTime.Now.Year.ToString();
        }

        private void queryButton_Click(object sender, RoutedEventArgs e)
        {
            //DBServicesInstance.ChangeTracker.LazyLoadingEnabled = true;

            int month_ComboBox_Value = month_comboBox.SelectedIndex + 1;
            int year_TextBox_Value = Int32.Parse(year_textBox.Text.ToString());
            List<ExportableWorkList> resultList = DBServicesInstance.GetWorkListByMonth(year_TextBox_Value, month_ComboBox_Value);

            dataGrid.ItemsSource = resultList;
        }

        private void worker_comboBox_Loaded(object sender, RoutedEventArgs e)
        {
            //GET Workers from DB and Display them in worker_comboBox
            List<Worker>? workerList = DBServicesInstance.GetWorkers();
            workerList?.Insert(0,new Worker { WorkerId=0, FullName="<< Összes Dolgozó >>"});

            worker_comboBox.DisplayMemberPath = "FullName";
            worker_comboBox.SelectedValuePath = "WorkerId";
            worker_comboBox.ItemsSource = workerList;
            worker_comboBox.SelectedIndex = 0;
        }
    }
}