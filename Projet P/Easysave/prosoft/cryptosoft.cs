using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Diagnostics;

namespace CryptoSoft
{
    class Crypt
    {
        public int Xor(string path, string file, string lang)
        {
            var step = new Menu();
            
            try
            {
                //start the process
            
                int B = 0;

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                //Create array of byte by decomposing the file
                
                byte[] byteToEncrypt = File.ReadAllBytes(path);
                BitArray bitToEncrypt = new BitArray(byteToEncrypt);

                //Create array of byte by decomposing the key
                
                byte[] byteKey = new byte[64] {9, 43, 102, 147, 8, 52, 157, 183, 9, 43, 102, 147, 8, 52, 157, 183, 9, 43, 102, 147, 8, 52, 157, 183, 9, 43, 102, 147, 8, 52, 157, 183, 9, 43, 102, 147, 8, 52, 157, 183, 9, 43, 102, 147, 8, 52, 157, 183, 9, 43, 102, 147, 8, 52, 157, 183, 9, 43, 102, 147, 8, 52, 157, 183};
                BitArray bitKey = new BitArray(byteKey);

                //Create array of byte by recomposing the file
                
                byte[] byteCrypted = new byte[byteToEncrypt.Length];
                BitArray bitCrypted = new BitArray(bitToEncrypt.Length);

                int j = 0;
                
                //start xor operator on file

                for (int i = 0; i < bitToEncrypt.Length; i++)
                {
                    j = i % byteKey.Length;
                    bitCrypted[i] = bitToEncrypt[i] ^ bitKey[j];
                    
                    int k = 100 * i / bitToEncrypt.Length;
                         
                    if (k == B)
                    {
                        step.print71(lang, file, k);
                        B = k + 1;
                    }
                }

                step.print72(lang, file);

                bitCrypted.CopyTo(byteCrypted, 0);
                File.WriteAllBytes(path, byteCrypted);

                //Get result
                
                stopWatch.Stop();
                
                //Get time by using stopWatch
                
                TimeSpan ts = stopWatch.Elapsed;
                int time = ts.Milliseconds;
                time = Math.Abs(time);

                step.print73(lang, time);
                return time;
            }

            //if error return
            
            catch
            {
                step.print74(lang, file);
                return -1;
            }
        }
    }
}