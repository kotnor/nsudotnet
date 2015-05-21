using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Mikhailov.Nsudotnet.Enigma
{
    class Program
    {
        private static void EncryptFile (string inputFileName, string algorithm, string outputFileName)
        {
            var provider = GetProvider(algorithm);
            provider.GenerateKey();
            provider.GenerateIV();
            using (var inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            {
                using (var outputFileStream = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    WriteKey(provider.Key, provider.IV);              
                    ICryptoTransform encrypt = provider.CreateEncryptor();
                    using (var cryptoStream = new CryptoStream(outputFileStream, encrypt, CryptoStreamMode.Write))
                    {
                        inputFileStream.CopyTo(cryptoStream);
                    }
                }
            }
        }
        private static void DecryptFile(string encFileName, string algorithm, string keyFile, string inFile)
        {
            byte[] key, iv;

            using (var keyReader = new StreamReader(new FileStream(keyFile, FileMode.Open, FileAccess.Read)))
            {
                key = Convert.FromBase64String(keyReader.ReadLine());
                iv = Convert.FromBase64String(keyReader.ReadLine());
            }
            var provider = GetProvider(algorithm);
            provider.Key = key;
            provider.IV = iv;
            using (var instreamFile = new FileStream(encFileName, FileMode.Open, FileAccess.Read))
            {
                using (var outstreamFile = new FileStream(inFile, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    ICryptoTransform decrypt = provider.CreateDecryptor();
                    using (var cryptostream = new CryptoStream(instreamFile, decrypt, CryptoStreamMode.Read))
                    {
                        cryptostream.CopyTo(outstreamFile);
                    }
                }
            }
        }

        public static void WriteKey(byte[] key, byte[] iv)
        {
            string keys = "key.txt";
            using (var keyWriter = new StreamWriter(new FileStream(keys, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                keyWriter.WriteLine(Convert.ToBase64String(key));
                keyWriter.WriteLine(Convert.ToBase64String(iv));
            }
        }
        private static SymmetricAlgorithm GetProvider(string algorithm)
        {
            switch (algorithm)
            {
                case "aes":
                    return new AesCryptoServiceProvider();
                case "rc2":
                    return new RC2CryptoServiceProvider();
                case "des":
                    return new DESCryptoServiceProvider();
                case "rijndael":
                    return new RijndaelManaged();
                default:
                    throw new Exception("Incorrent type of algoritym");
            }
        }
        static void Main(string[] args)
        {
            if (args.Length == 4)
            {
                if (args[0] == "encrypt")
                    EncryptFile(args[1], args[2], args[3]);
            } else if (args.Length == 5)
            {
                if (args[0] == "decrypt")
                    DecryptFile(args[1], args[2], args[3], args[4]);
            }
            else
                Console.WriteLine("There was a mistake of arguments");
        }
    }
}
