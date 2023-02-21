using System;
using System.IO;
using System.Collections;

namespace CryptoSoft
{
    class Program
    {
        //int Main
        static int Main(string[] args)
        {
            Values.Instance.EasySavePath = args[0];
            StreamReader lang = new StreamReader(Values.Instance.EasySavePath + "\\Config\\Lang.json");
            Values.Instance.Lang = lang.ReadLine();
            lang.Close();
            StreamReader ext = new StreamReader(Values.Instance.EasySavePath + "\\CryptoSoft\\Ext.json");
            Values.Instance.ExtValues = ext.ReadLine();
            ext.Close();
            StreamReader path = new StreamReader(Values.Instance.EasySavePath + "\\CryptoSoft\\PathToCrypt.json");
            Values.Instance.PathToCrypt = path.ReadLine();
            path.Close();
            var list = Values.Instance.ExtValues.Split(';').ToList();
            string choiceS = "";
            string choiceR = "";
            int nFiles = 0;
            int nFile = 1;
            bool B = false;

            //Get all files from the selected folder
            
            DirectoryInfo place = new DirectoryInfo(Values.Instance.PathToCrypt);
            FileInfo[] Files = place.GetFiles();

            
            //Print extension from json

            step.print0(Values.Instance.Lang);

            //Print all extension wrote in Ext.json
            
            foreach (var items in list)
            {
                step.print01(items.ToString());
            }

            step.print1(Values.Instance.Lang);
            choiceS = Console.ReadLine();
            
            //Press n to shutdown the console

            if (choiceS == "n" | choiceS == "N")
            {
                return -1;
            }
   
            /*if (choiceS == "y" & choiceS == "Y")
            {
                return 1;
            }*/
            
            step.print2(Values.Instance.Lang, Values.Instance.PathToCrypt);

            //Print files where extension file == extension wrote in Ext.json
            
            foreach (FileInfo i in Files)
            {
                string a = i.Name;
                string b = i.Extension;
                foreach (var items in list)
                {
                    if (b == items.ToString())
                    {
                        step.print21(a);
                        B = true;
                        nFiles++;
                    }
                }              
            }
            
            //If for all files extension file !== extension wrote in Ext.json then shutdown the console
            
            if (B == false)
            {
                step.print3(Values.Instance.Lang);
                return -1;
            }

            step.print4(Values.Instance.Lang);
            choiceR = Console.ReadLine();

            if (choiceR == "n" | choiceR == "N")
            {
                return -1;
            }

            /*if (choiceR == "y" & choiceR == "Y")
            {
                return 1;
            }*/

            //start process foreach files
            
            step.print5(Values.Instance.Lang);

            foreach (FileInfo i in Files)
            {
                string a = i.Name;
                string b = i.Extension;
                foreach (var items in list)
                {
                    if (b == items.ToString())
                    {
                        if(nFile < 10)
                        {
                            step.print6(Values.Instance.Lang, a, nFile, nFiles);
                        }
                        
                        else 
                        {
                            step.print61(Values.Instance.Lang, a, nFile, nFiles);
                        }

                        var process = new Crypt();
                        process.Xor(Values.Instance.PathToCrypt + "\\" + a, a, Values.Instance.Lang);
                        nFile++;
                    }
                }
            }

            step.print8(Values.Instance.Lang, Values.Instance.PathToCrypt);
            step.print9(Values.Instance.Lang);
            return 1;
        }
    }
}