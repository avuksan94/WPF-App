using PPPK_Zadatak02.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PPPK_Zadatak02
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ConnectionProtectionUtils _operations = new ConnectionProtectionUtils();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Encrypt the connection string if its not already encrypted
            if (!_operations.IsProtected())
            {
                _operations.EncryptFile();
            }
        }

        //get the connection string for use
        public static string GetConnectionString()
        {
            var operations = new ConnectionProtectionUtils();
            string mainConnection;

            if (operations.IsProtected())
            {
                operations.DecryptFile();
                mainConnection = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
                operations.EncryptFile();
            }
            else
            {
                mainConnection = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
            }

            return mainConnection;
        }
    }
}
