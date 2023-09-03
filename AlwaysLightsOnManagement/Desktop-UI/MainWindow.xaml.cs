using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using System.Xml.Serialization;
using AlwaysLightsOnDataModelsDLL;
using AlwaysLightsOnManagement;
using Microsoft.EntityFrameworkCore;

namespace Desktop_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Serializable]
    [XmlRoot("ExportableWorkList")]
    public partial class MainWindow : Window
    {
        public DBServices DBServicesInstance { get; set; } = new DBServices();

        [XmlElement("ExportableWorkList")]
        List<ExportableWorkList> resultList = new();

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
            int worker_ComboBox_Value = Int32.Parse(worker_comboBox.SelectedValue.ToString()!);

            
            if (worker_ComboBox_Value == 0)
            {
                //NO WORKER SELECTED - Show Monthly list with ALL Workers
                resultList = DBServicesInstance.GetWorkListByTime(year_TextBox_Value, month_ComboBox_Value);
            }
            else
            {
                //GOT OTHER Existing WorkerID - Show Selected Worker's WorkList
                resultList = DBServicesInstance.GetWorkListByWorkerIDAndTime(worker_ComboBox_Value,year_TextBox_Value, month_ComboBox_Value);
            }

            //SET DataGrid Source to resultList
            dataGrid.ItemsSource = resultList;
            xmlExportButton.IsEnabled = true;
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

        private void month_comboBox_Loaded(object sender, RoutedEventArgs e)
        {
            month_comboBox.SelectedIndex = DateTime.Now.Month-1;
        }

        private void groupByButton_Click(object sender, RoutedEventArgs e)
        {
            int month_ComboBox_Value = month_comboBox.SelectedIndex + 1;
            int year_TextBox_Value = Int32.Parse(year_textBox.Text.ToString());

            resultList = DBServicesInstance.GetWorkListByMonth_GroupByWorkTypes(year_TextBox_Value, month_ComboBox_Value);
            
            //SET DataGrid Source to resultList
            dataGrid.ItemsSource = resultList;
            xmlExportButton.IsEnabled = true;
        }

        private void xmlExportButton_Click(object sender, RoutedEventArgs e)
        {
            //do XML Export Here
            //SAVE OUTPUT worklistXML_{DateStamp}_{TimeStamp}.xml
            string xmlFileName = $"worklistXML_{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}__{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}.xml";
            DBServicesInstance.CreateXML(xmlFileName,resultList);

            // DISPLAY POPUP - SAVED
            string messageBoxText = $"XML fájl mentésre került \"{xmlFileName}\" néven!";
            string caption = "XML Mentve";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;

            MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.No);

        }

       

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
