using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Diagnostics;

namespace EasySave
{
    class Program
    {
        static void lireFichier(string cheminFichier)
        {
            StreamReader Lire = new StreamReader(cheminFichier);
            string line;
            while ((line = Lire.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
            Lire.Close();
        }
        static void ecrireFichier(string cheminFichier, string aEcrire, bool ecraser)
        {
            //la fonction peut être utilisée pour créer un fichier en laissant aEcrire vide
            if (ecraser == false)
            {
                StreamWriter Ecrire = new StreamWriter(cheminFichier, true);
                Ecrire.WriteLine(aEcrire);
                Ecrire.Close();
            }
            else
            {
                StreamWriter Ecrire = new StreamWriter(cheminFichier, false);
                Ecrire.WriteLine(aEcrire);
                Ecrire.Close();
            }
        }
        static void multiBackup(string cheminFichier, string formatLogs) //prends en paramètre un fichier contenant une liste de backup à exécuter
        {
            StreamReader Lire = new StreamReader(cheminFichier);
            string line;
            while ((line = Lire.ReadLine()) != null)
            {
                executeBackup(@"C:\Easysave\Profils\" + line + ".txt", formatLogs);
            }
            Lire.Close();
        }
        static void executeBackup(string cheminProfil, string formatLogs)
        {
            string nomProfil = File.ReadLines(cheminProfil).Skip(0).Take(1).First(); //la 2e ligne du profil est le dossier source
            string cheminAvancement = @"C:\Easysave\avancement.txt";
            string dossierSource = File.ReadLines(cheminProfil).Skip(1).Take(1).First(); //la 2e ligne du profil est le dossier source
            string dossierCible = File.ReadLines(cheminProfil).Skip(2).Take(1).First(); //la 3e ligne du profil est le dossier cible

            string cheminLogs = @"C:\Easysave\logs."+formatLogs;
            if (!Directory.Exists(dossierSource))// Vérifie si les répertoires existent
            {
                Directory.CreateDirectory(dossierSource);
                return;
            }

            if (!Directory.Exists(dossierCible))
            {
                Directory.CreateDirectory(dossierCible);
                return;
            }

            var stopwatch = new Stopwatch(); //début de la stopwatch
            stopwatch.Start();
            long totalBytes = 0;

            foreach (var file in Directory.GetFiles(dossierSource, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = file.Replace(dossierSource, "");
                string targetPath = dossierCible + relativePath;
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                var fileInfo = new FileInfo(file);
                totalBytes += fileInfo.Length;
                File.Copy(file, targetPath, true);
                ecrireFichier(cheminAvancement, DateTime.Now + " Copie du fichier " + targetPath + " vers " + dossierCible, false);
            }

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed; //fin de stopwatch

            ecrireFichier(cheminLogs, "{", false);
            ecrireFichier(cheminLogs, "Nom : " + nomProfil + ",", false);
            ecrireFichier(cheminLogs, "Dossier source : " + dossierSource + ",", false);
            ecrireFichier(cheminLogs, "Dossier source : " + dossierCible + ",", false);
            ecrireFichier(cheminLogs, "Copie terminée avec succès.", false);
            ecrireFichier(cheminLogs, "Temps écoulé : " + elapsedTime, false);
            ecrireFichier(cheminLogs, "Taille totale des fichiers transférés : " + totalBytes + " octets", false);
            ecrireFichier(cheminLogs, "Vitesse de transfert : " + (totalBytes / 1024 / elapsedTime.TotalSeconds).ToString("0.##") + " Ko/s", false);
            ecrireFichier(cheminLogs, "}", false);
        }
        static void Main(string[] args)
        {
            bool quit = false;
            string langue = "fr";
            string formatLogs = File.ReadLines(@"C:\Easysave\choixformat.txt").Skip(0).Take(1).First();
            string dossierLangue = @"C:\Easysave\Langues\fr";
            string dossierProfils = @"C:\Easysave\Profils\";
            string cheminLogs = @"C:\Easysave\logs."+formatLogs;
            string cheminListeProfils = @"C:\Easysave\Profils\listeprofils.txt";     
            int nbProfils = Directory.GetFiles(dossierProfils, "*", SearchOption.TopDirectoryOnly).Length;

            while (quit == false)
            {
                if (langue == "fr")
                {
                    dossierLangue = @"C:\Easysave\Langues\fr\";
                }
                else if (langue == "eng")
                {
                    dossierLangue = @"C:\Easysave\Langues\eng\";
                }

                lireFichier(dossierLangue + "menu.txt");
                int choix = Convert.ToInt32(Console.ReadLine());
                switch (choix)
                {
                    case 1: //cette fonction sert à créer un profil, sauvegardé dans son propre fichier txt
                        if (nbProfils > 4)
                        {
                            Console.WriteLine(File.ReadLines(dossierLangue + "profil.txt").Skip(0).Take(1).First());
                        }
                        else
                        {
                            Console.WriteLine(File.ReadLines(dossierLangue + "profil.txt").Skip(1).Take(1).First());
                            string nomProfil = Console.ReadLine();
                            ecrireFichier(dossierProfils + nomProfil + ".txt", nomProfil, false);

                            Console.WriteLine(File.ReadLines(dossierLangue + "profil.txt").Skip(2).Take(1).First());
                            string dossierSource = Console.ReadLine();
                            ecrireFichier(dossierProfils + nomProfil + ".txt", dossierSource, false);

                            Console.WriteLine(File.ReadLines(dossierLangue + "profil.txt").Skip(3).Take(1).First());
                            string dossierCible = Console.ReadLine();
                            ecrireFichier(dossierProfils + nomProfil + ".txt", dossierCible, false);

                            Console.WriteLine(File.ReadLines(dossierLangue + "profil.txt").Skip(4).Take(1).First());
                            string typeSave = Console.ReadLine();
                            if ((typeSave != "d") & typeSave != "c")
                            {
                                while ((typeSave != "d") & typeSave != "c")
                                {
                                    Console.WriteLine(File.ReadLines(dossierLangue + "profil.txt").Skip(4).Take(1).First());
                                    typeSave = Console.ReadLine();
                                }
                            }
                            ecrireFichier(dossierProfils + nomProfil + ".txt", typeSave, false);
                            ecrireFichier(dossierProfils + "listeprofils.txt", nomProfil, false); //ajout du profil dans la liste des profils
                        }

                        break;

                    case 2: //exécuter backup
                        Console.WriteLine(File.ReadLines(dossierLangue + "backup.txt").Skip(2).Take(1).First()); //un ou tous les profils ?
                        int choixBackup = Convert.ToInt32(Console.ReadLine()); //1 pour un profil, 2 pour tous les profils à la suite
                        if ((choixBackup != 1) & (choixBackup != 2))
                        {
                            while ((choixBackup != 1) & (choixBackup != 2))
                            {
                                Console.WriteLine(File.ReadLines(dossierLangue + "backup.txt").Skip(2).Take(1).First());
                                choixBackup = Convert.ToInt32(Console.ReadLine());
                            }
                        }
                        if (choixBackup == 1)
                        {
                            Console.WriteLine(File.ReadLines(dossierLangue + "backup.txt").Skip(0).Take(1).First());
                            lireFichier(cheminListeProfils);
                            string choixProfil = Console.ReadLine();
                            Console.WriteLine(File.ReadLines(dossierLangue + "backup.txt").Skip(1).Take(1).First() + choixProfil + ".txt ? [y/n]");
                            string choixYN = Console.ReadLine();
                            if ((choixYN != "y") & choixYN != "n")
                            {
                                while ((choixYN != "y") & choixYN != "n")
                                {
                                    Console.WriteLine(File.ReadLines(dossierLangue + "backup.txt").Skip(1).Take(1).First() + choixProfil + ".txt ? [y/n]");
                                    choixYN = Console.ReadLine();
                                }
                            }
                            if (choixYN == "y")
                            {
                                string profilAExecuter = dossierProfils + choixProfil + ".txt";
                                executeBackup(profilAExecuter, formatLogs);
                                Console.WriteLine(File.ReadLines(dossierLangue + "backup.txt").Skip(4).Take(1).First()); //la sauvegarde s'est bien passé
                            }
                            else
                            {
                                break;
                            }
                        }
                        else if (choixBackup == 2)
                        {
                            Console.WriteLine(File.ReadLines(dossierLangue + "backup.txt").Skip(3).Take(1).First());
                            string choixYN = Console.ReadLine();
                            if ((choixYN != "y") & choixYN != "n")
                            {
                                while ((choixYN != "y") & choixYN != "n")
                                {
                                    Console.WriteLine(File.ReadLines(dossierLangue + "backup.txt").Skip(3).Take(1).First());
                                    choixYN = Console.ReadLine();
                                }
                            }
                            if (choixYN == "y")
                            {
                                //récupérer tous les profils dans une boucle et exécuter le backup
                                multiBackup(cheminListeProfils, formatLogs);
                                Console.WriteLine(File.ReadLines(dossierLangue + "backup.txt").Skip(5).Take(1).First()); //la sauvegarde s'est bien passé
                            }
                            else
                            {
                                break;
                            }

                        }

                        break;

                    case 3: //changer format logs
                        Console.WriteLine(File.ReadLines(dossierLangue + "profil.txt").Skip(6).Take(1).First());
                        formatLogs = Console.ReadLine();

                        if ((formatLogs != "xml") & formatLogs != "json")
                        {
                            while ((formatLogs != "xml") & formatLogs != "json")
                            {
                                Console.WriteLine(File.ReadLines(dossierLangue + "profil.txt").Skip(6).Take(1).First());
                                formatLogs = Console.ReadLine();
                            }
                        }

                        ecrireFichier(@"C:\Easysave\choixformat.txt", formatLogs, true);
                        break;

                    case 4:
                        lireFichier(cheminLogs);
                        break;

                    case 5:
                        if (langue == "fr")
                            langue = "eng";
                        else
                            langue = "fr";
                        break;

                    case 6:
                        quit = true;
                        break;
                }
            }
        }
    }
}