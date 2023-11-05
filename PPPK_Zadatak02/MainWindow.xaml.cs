using PPPK_Zadatak02.DAL;
using PPPK_Zadatak02.Models;
using PPPK_Zadatak02.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace PPPK_Zadatak02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var connectionProtection = new ConnectionProtectionUtils();

            bool isEncrypted = connectionProtection.IsProtected();
            if (isEncrypted)
            {
                MessageBox.Show("The connection string is protected (encrypted).");
            }
            else
            {
                MessageBox.Show("The connection string is not protected (unencrypted).");
            }


            Frame.Navigate(new ListProductsPage(new ViewModels.ProductViewModel())
            {
                Frame = Frame
            });

        }
    }
}
