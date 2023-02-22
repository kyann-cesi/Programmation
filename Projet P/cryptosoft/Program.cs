using System;
using System.IO;

class Program
{

    static void Main(string[] args)
    {
        string inputFile = args[0];
        string outputDirectory = args[1];
        string outputFileName = Path.GetFileNameWithoutExtension(inputFile) + "_encrypted" + Path.GetExtension(inputFile);
        string outputFile = Path.Combine(outputDirectory, outputFileName);

        string key = "mySecretKey"; // clé de cryptage

        byte[] buffer = new byte[4096];
        int bytesRead;

        using (FileStream input = new FileStream(inputFile, FileMode.Open))
        using (FileStream output = new FileStream(outputFile, FileMode.Create))
        {
            int keyIndex = 0;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    buffer[i] ^= (byte)key[keyIndex];
                    keyIndex = (keyIndex + 1) % key.Length;
                }
                output.Write(buffer, 0, bytesRead);
            }
        }

        Console.WriteLine("le fichier a été crypté");
    }
}

