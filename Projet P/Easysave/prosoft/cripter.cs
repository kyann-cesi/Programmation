using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasySave
{
    internal class Encrypt
    {
        public void encryptFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                if (Values.Instance.Lang == "en")
                {
                    MessageBox.Show("Path didn't exist !", "CryptoSoft", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (Values.Instance.Lang == "fr")
                {
                    MessageBox.Show("Dossier inexistant !", "CryptoSoft", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                StreamWriter decrypt = new StreamWriter(Values.Instance.PathConfig + "\\CryptoSoft\\PathToCrypt.json");
                decrypt.WriteLine(path);
                decrypt.Close();
                Process[] cname = Process.GetProcessesByName("CryptoSoft");
                if (cname.Length != 0)
                {
                    if (Values.Instance.Lang == "en")
                    {
                        MessageBox.Show("CryptoSoft is already running !", "CryptoSoft", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (Values.Instance.Lang == "fr")
                    {
                        MessageBox.Show("CryptoSoft déjà en cours d'exécution !", "CryptoSoft", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    StreamReader read = new StreamReader(Values.Instance.PathConfig + "\\CryptoSoft\\Ext.json");
                    string readFile = read.ReadToEnd();
                    read.Close();
                    if (readFile == "")
                    {
                        if (Values.Instance.Lang == "en")
                        {
                            MessageBox.Show("Extension file empty !", "CryptoSoft", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (Values.Instance.Lang == "fr")
                        {
                            MessageBox.Show("Fichier extension vide !", "CryptoSoft", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = "CryptoSoft";
                        process.StartInfo.Arguments = Values.Instance.PathConfig;
                        process.Start();
                    }
                }
            }
        }
    }
}