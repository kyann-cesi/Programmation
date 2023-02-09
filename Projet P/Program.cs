using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace EasySave
{
    class Program
    {
        static void lireFichier(string cheminFichier)
        {
            StreamReader Lire = new StreamReader(cheminFichier);
            string line;
            while((line=Lire.ReadLine())!=null)
            {
                Console.WriteLine(line);
            }
            Lire.Close();
        }

        static void ecrireFichier(string cheminFichier, string aEcrire)
        {
            //la fonction peut être utilisée pour créer un fichier en laissant aEcrire vide
            StreamWriter Ecrire = new StreamWriter(cheminFichier,true); 
            //le true fait en sorte que la fonction fasse un append au lieu d'overwrite le fichier /!\

            Ecrire.WriteLine(aEcrire);
            Ecrire.Close();
        }

        static void copierDossier(string dossierSource, string dossierCible)
        {
            string cheminAvancement = @"C:\Easysave\avancement.txt";
            string cheminLogs = @"C:\Easysave\logs.txt";
            // Vérifiez si les répertoires existent
            if (!Directory.Exists(dossierSource))
            {
                Console.WriteLine("Le répertoire source n'existe pas");
                return;
            }

            if (!Directory.Exists(dossierCible))
            {
                Console.WriteLine("Le répertoire cible n'existe pas");
                return;
            }

            //le code ci-dessous copie tous les sous-dossiers du répertoire source à cible
            string[] allDirectories = Directory.GetDirectories(dossierSource, "*", SearchOption.AllDirectories);
            int numeroDossier = 1;
            int nbDossiersTotal = Directory.GetDirectories(dossierCible, "*", SearchOption.AllDirectories).Length;
            foreach (string dir in allDirectories)
            {
                string dirToCreate = dir.Replace(dossierSource, dossierCible);
                Directory.CreateDirectory(dirToCreate);
                ecrireFichier(cheminAvancement, DateTime.Now + " Copie du dossier " + numeroDossier + "/" + nbDossiersTotal + " " + dirToCreate + " vers " + dossierCible);
                numeroDossier++;
            }

            //ce code copie tous les fichiers contenu dans le dossier et dans les sous-dossiers
            string[] allFiles = Directory.GetFiles(dossierSource, "*.*", SearchOption.AllDirectories);
            int numeroFichier = 1;
            int nbFichiersTotal = Directory.GetFiles(dossierCible, "*", SearchOption.AllDirectories).Length;
            foreach (string newPath in allFiles)
            {
                File.Copy(newPath, newPath.Replace(dossierSource, dossierCible));
                ecrireFichier(cheminAvancement, DateTime.Now + " Copie du fichier " + numeroFichier + "/" + nbFichiersTotal + " " + newPath + " vers " + dossierCible);
                numeroFichier++;
            }

            Console.WriteLine("La copie a été effectuée avec succès");
        }



        static void Main(string[] args)
        {
            bool quit = false;
            string langue = "fr";
            string dossierLangue = @"C:\Easysave\Langues\fr";
            string cheminLogs = @"C:\Easysave\logs.txt";
            string dossierProfils = @"C:\Easysave\Profils\";
            int nbProfils = Directory.GetFiles(dossierProfils, "*", SearchOption.TopDirectoryOnly).Length;

            while (quit==false)
            {
                if (langue == "fr")
                {
                    dossierLangue = @"C:\Easysave\Langues\fr\";
                }
                else if (langue == "eng")
                {
                    dossierLangue = @"C:\Easysave\Langues\eng\";
                }

                lireFichier(dossierLangue+"menu.txt");
                int choix = Convert.ToInt32(Console.ReadLine());
                switch(choix)
                {
                    case 1:
                        if(nbProfils>4)
                        {
                            Console.WriteLine(File.ReadLines(dossierLangue + "profil.txt").Skip(0).Take(1).First());
                        }
                        else
                        {
                            Console.WriteLine(File.ReadLines(dossierLangue + "profil.txt").Skip(1).Take(1).First());
                            string nomProfil = Console.ReadLine();
                            ecrireFichier(dossierProfils + nomProfil + ".txt", nomProfil);

                            Console.WriteLine(File.ReadLines(dossierLangue + "profil.txt").Skip(2).Take(1).First());
                            string dossierSource = Console.ReadLine();
                            ecrireFichier(dossierProfils + nomProfil + ".txt", dossierSource);

                            Console.WriteLine(File.ReadLines(dossierLangue + "profil.txt").Skip(3).Take(1).First());
                            string dossierCible = Console.ReadLine();
                            ecrireFichier(dossierProfils + nomProfil + ".txt", dossierCible);

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
                            ecrireFichier(dossierProfils + nomProfil + ".txt", typeSave);  
                        }
                        break;

                    case 2: //exécuter backup
                        Console.WriteLine(File.ReadLines(dossierLangue + "backup.txt").Skip(0).Take(1).First());
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
                            string dossierSource = File.ReadLines(dossierProfils + choixProfil + ".txt").Skip(1).Take(1).First();
                            string dossierCible = File.ReadLines(dossierProfils + choixProfil + ".txt").Skip(2).Take(1).First();
                            copierDossier(dossierSource, dossierCible);
                        }
                        else
                        {
                            break;
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
