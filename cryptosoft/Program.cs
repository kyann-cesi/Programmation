using System;
using System.IO;

namespace cryptosoft
{
    class Program
    {
        static void Main(string[] args)
        {
            // Dossier source et dossier de destination
            string sourceFolder = args[0];
            string destinationFolder = args[1];

            // Récupérer la liste des fichiers dans le dossier source
            string[] fileNames = Directory.GetFiles(sourceFolder);

            // Parcourir la liste des fichiers et les crypter et transférer vers le dossier de destination
            foreach (string fileName in fileNames)
            {
                // Crypter le fichier
                string encryptedFileName = Path.GetFileNameWithoutExtension(fileName) + ".encrypted" + Path.GetExtension(fileName);
                string encryptedFilePath = Path.Combine(sourceFolder, encryptedFileName);



                /////////////////////////////
                string FileName = fileName + "|" + encryptedFilePath;
                try
                {
                    a(FileName);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                /////////////////////////

                // Déplacer le fichier crypté vers le dossier de destination
                string destinationFilePath = Path.Combine(destinationFolder, Path.GetFileName(encryptedFileName));
                File.Move(encryptedFilePath, destinationFilePath);

                Console.WriteLine("Le fichier {0} a été transféré avec succès.", Path.GetFileName(encryptedFileName));
            }

            Console.WriteLine("Le transfert de fichiers est terminé.");


            static void a(string args)
            {
                char[] charSeparators = new char[] { '|' };
                string[] files_path = args.Split(charSeparators, StringSplitOptions.None);

                // Vérifier si le fichier à crypter a été fourni en argument
                if (args.Length == 0)
                {
                    Console.WriteLine("Aucun fichier spécifié.");
                    return;
                }

                // Lire le nom du fichier à crypter depuis les arguments


                // Ouvrir le fichier en lecture
                FileStream fs = new FileStream(files_path[0], FileMode.Open);

                // Créer un tableau de bytes pour stocker les données du fichier
                byte[] data = new byte[fs.Length];
                byte[] data2 = new byte[fs.Length];

                // Lire les données du fichier dans le tableau
                fs.Read(data, 0, data.Length);

                // Fermer le fichier
                fs.Close();

                // Crypter les données en utilisant la méthode XOR
                for (int i = 0; i < data.Length; i++)
                {
                    data2[i] = (byte)(data[i] ^ 0xFF); // XOR avec 0xFF
                }

                // Écrire les données cryptées dans le fichier
                if (data == data2)
                {
                    Console.WriteLine("Le fichier est equivalent, il n'est pas encrypté");
                }
                fs = new FileStream(files_path[1], FileMode.Create);
                fs.Write(data2, 0, data.Length);
                fs.Close();

                Console.WriteLine("Le fichier a été crypté avec succès.");
            }
        }
    }
}
