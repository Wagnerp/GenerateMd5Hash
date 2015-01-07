using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace GenerateMd5Hash
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            #region Basic file- check

            if (args.Length <= 0)
            {
                Console.WriteLine("You really need to specify a file!");
                Environment.Exit(401);
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File not found!");
                Environment.Exit(404);
            }

            #endregion


            byte[] bHashBuffer;

            using (var md5 = MD5.Create())
            {

                using (var fs = File.OpenRead(args[0]))
                {
                    bHashBuffer = new byte[fs.Length];
                    bHashBuffer = md5.ComputeHash(fs);
                }
            }

            var strComputedHash = new StringBuilder(bHashBuffer.Length);
            for (var i = 0; i < bHashBuffer.Length; i++)
                strComputedHash.Append(bHashBuffer[i].ToString("X2"));

            Console.WriteLine("MD5 Hash is: " + strComputedHash);

            Console.WriteLine("Copy to Clipboard?<y/n>");
            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.Y)
            {
                Clipboard.SetText(strComputedHash.ToString());
                Console.WriteLine("Copied to Clipboard!");
                Console.WriteLine("");
            }
        }
    }
}
