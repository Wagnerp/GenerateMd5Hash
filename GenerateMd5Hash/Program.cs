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
            var bIsFile = false;
            var text = String.Empty;

            #region Basic checks...

            if (args.Length <= 0)
            {
                Console.WriteLine("You really need to specify a file/ string!");
                PrintHelp();
                Environment.Exit(401);
            }

            if (args[0].StartsWith("--?"))
            {
                PrintHelp();
                Environment.Exit(40);
            }

            if (!File.Exists(args[0]))
            {
                bIsFile = false;
                text = args[0].StartsWith("@") ? args[0].Substring(1) : args[0];
            }

            else
            {
                bIsFile = true;
                
            }

            #endregion


            byte[] bHashBuffer = new byte[0];

            using (var md5 = MD5.Create())
            {
                if (bIsFile)
                {
                    using (var fs = File.OpenRead(args[0]))
                    {
                        bHashBuffer = md5.ComputeHash(fs);
                    }
                }

                else
                {
                    bHashBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
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
                Console.WriteLine("\nCopied to Clipboard!");
                Console.WriteLine("");
            }
        }

        static void PrintHelp()
        {
            const int iPosition = 30;

            FillConsoleLineWithChar("-");
            Console.Write("| --help");
            Console.SetCursorPosition(iPosition, Console.CursorTop);
            Console.Write("Prints helptext");
            Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop);
            Console.Write("|");
            FillConsoleLineWithChar("-");

            Console.Write("| Generate md5sum of File:");
            Console.SetCursorPosition(iPosition, Console.CursorTop);
            Console.Write("[PATH_TO_FILE]");
            Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop);
            Console.Write("|");
            Console.Write("| Generate md5sum of text:");
            Console.SetCursorPosition(iPosition, Console.CursorTop);
            Console.Write("@[TEXT]");
            Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop);
            Console.Write("|");
            FillConsoleLineWithChar("-");
        }

        static void FillConsoleLineWithChar(string character)
        {
            for (var i = 0; i < Console.BufferWidth; i++)
            {
                Console.Write(character);
            }
        }
    }
}
