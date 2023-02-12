using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
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

        static void multiBackup(string cheminFichier) //prends en paramètre un fichier contenant une liste de backup à exécuter
        {
            StreamReader Lire = new StreamReader(cheminFichier);
            string line;
            while ((line = Lire.ReadLine()) != null)
            {
                executeBackup(@"C:\Easysave\Profils\" + line + ".txt");
            }
            Lire.Close();
        }
        static void executeBackup(string cheminProfil)
        {
            string dossierSource = File.ReadLines(cheminProfil).Skip(1).Take(1).First(); //la 2e ligne du profil est le dossier source
            string dossierCible = File.ReadLines(cheminProfil).Skip(2).Take(1).First(); //la 3e ligne du profil est le dossier cible

            string cheminAvancement = @"C:\Easysave\avancement.txt";
            string cheminLogs = @"C:\Easysave\logs.txt";
            // Vérifiez si les répertoires existent
            if (!Directory.Exists(dossierSource))
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

            /*
            //le code ci-dessous copie tous les sous-dossiers du répertoire source à cible
            string[] allDirectories = Directory.GetDirectories(dossierSource, "*", SearchOption.AllDirectories);
            int numeroDossier = 1;
            int nbDossiersTotal = Directory.GetDirectories(dossierCible, "*", SearchOption.AllDirectories).Length;
            foreach (string dir in allDirectories)
            {
                string dirToCreate = dir.Replace(dossierSource, dossierCible);
                Directory.CreateDirectory(dirToCreate);
                ecrireFichier(cheminAvancement, DateTime.Now + " Copie du dossier " + numeroDossier + "/" + nbDossiersTotal + " " + dirToCreate + " vers " + dossierCible, false);
                numeroDossier++;
            }

            //ce code copie tous les fichiers contenu dans le dossier et dans les sous-dossiers
            string[] allFiles = Directory.GetFiles(dossierSource, "*.*", SearchOption.AllDirectories);
            int numeroFichier = 1;
            int nbFichiersTotal = Directory.GetFiles(dossierCible, "*", SearchOption.AllDirectories).Length;
            foreach (string newPath in allFiles)
            {
                File.Copy(newPath, newPath.Replace(dossierSource, dossierCible));
                ecrireFichier(cheminAvancement, DateTime.Now + " Copie du fichier " + numeroFichier + "/" + nbFichiersTotal + " " + newPath + " vers " + dossierCible, false);
                numeroFichier++;
            }
            */

            foreach (var file in Directory.GetFiles(dossierSource, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = file.Replace(dossierSource, "");
                string targetPath = Path.Combine(dossierCible, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                var fileInfo = new FileInfo(file);
                totalBytes += fileInfo.Length;
                File.Copy(file, targetPath, true);
            }

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed; //fin de stopwatch

            ecrireFichier(cheminLogs,"Copie terminée avec succès.",false);
            ecrireFichier(cheminLogs,"Temps écoulé : " + elapsedTime,false);
            ecrireFichier(cheminLogs,"Taille totale des fichiers transférés : " + totalBytes + " octets",false);
            ecrireFichier(cheminLogs,"Vitesse de transfert : " + (totalBytes / 1024 / 1024 / elapsedTime.TotalSeconds).ToString("0.##") + " Mo/s",false);
        }
        static void Main(string[] args)
        {
            bool quit = false;
            string langue = "fr";
            string dossierLangue = @"C:\Easysave\Langues\fr";
            string cheminLogs = @"C:\Easysave\logs.txt";
            string cheminListeProfils = @"C:\Easysave\Profils\listeprofils.txt";
            string dossierProfils = @"C:\Easysave\Profils\";
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
                                executeBackup(profilAExecuter);
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
                                multiBackup(cheminListeProfils);
                                Console.WriteLine(File.ReadLines(dossierLangue + "backup.txt").Skip(5).Take(1).First()); //la sauvegarde s'est bien passé
                            }
                            else
                            {
                                break;
                            }
                        }


                        break;

                    case 3:
                        lireFichier(cheminLogs);
                        break;

                    case 4:
                        if (langue == "fr")
                            langue = "eng";
                        else
                            langue = "fr";
                        break;

                    case 5:
                        quit = true;
                        break;
                }

            }
        }
    }
}