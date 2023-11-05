using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://learn.microsoft.com/en-us/answers/questions/273839/how-to-encrypt-connection-string-in-app-config

namespace PPPK_Zadatak02.Utils
{
    public class ConnectionProtectionUtils
    {
        public string FilePath { get; set; }
      
        public ConnectionProtectionUtils()
        {
            var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var configFilePath = exePath + ".config";

            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException("Config file not found.", configFilePath);
            }

            FilePath = configFilePath;
        }

        private bool EncryptConnectionString(bool encrypt, string fileName)
        {
            bool success = true;
            Configuration configuration = null;

            try
            {
                configuration = ConfigurationManager.OpenExeConfiguration(fileName);
                var configSection = configuration.GetSection("connectionStrings") as ConnectionStringsSection;

                if ((!(configSection.ElementInformation.IsLocked)) && (!(configSection.SectionInformation.IsLocked)))
                {
                    if (encrypt && (!configSection.SectionInformation.IsProtected))
                    {
                        // encrypt the file  
                        configSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    }

                    if ((!encrypt) && configSection.SectionInformation.IsProtected) //encrypt is true so encrypt  
                    {
                        // decrypt the file.   
                        configSection.SectionInformation.UnprotectSection();
                    }

                    configSection.SectionInformation.ForceSave = true;
                    configuration.Save();

                    success = true;

                }
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;

        }
        public bool IsProtected()
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(FilePath);
            var configSection = configuration.GetSection("connectionStrings") as ConnectionStringsSection;
            return configSection.SectionInformation.IsProtected;
        }
        public bool EncryptFile()
        {
            if (File.Exists(FilePath))
            {
                return EncryptConnectionString(true, FilePath);
            }
            else
            {
                return false;
            }
        }
        public bool DecryptFile()
        {
            if (File.Exists(FilePath))
            {
                return EncryptConnectionString(false, FilePath);
            }
            else
            {
                return false;
            }
        }
    }
}
