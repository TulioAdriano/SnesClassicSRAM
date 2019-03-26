using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SnesClassicSram
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length.Equals(0))
            {
                Console.WriteLine("Emulator SRAM to SNES Classic converter tool");
                Console.WriteLine();
                Console.WriteLine("Usage: SnesClassicSram.exe sramFilename");
                Console.WriteLine();
                Console.WriteLine("This program will create a cartridge.sram and cartridge.sram.hash files to be placed on the SNES Classic.");
                Console.WriteLine();
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine($"Error: Cannot find file \"{args[0]}\".");
                Console.WriteLine();
                return;
            }

            if (!(new FileInfo(args[0]).Length.Equals(8192)))
            {
                Console.Write($"Warning: Uncommon SRAM size. Do you wish to proceed (Y/N)? ");
                if (!Console.ReadKey().KeyChar.ToString().ToUpper().Equals("Y"))
                {
                    Console.WriteLine("\r\n");
                    return;
                } 

                Console.WriteLine();
            }

            SHA1Managed sHA1Managed = new SHA1Managed();
            byte[] hash = sHA1Managed.ComputeHash(File.ReadAllBytes(args[0]));

            BinaryWriter writer = new BinaryWriter(new FileStream("cartridge.sram", FileMode.Create));
            writer.Write(File.ReadAllBytes(args[0]));
            writer.Write(hash);
            writer.Close();

            File.WriteAllBytes("cartridge.sram.hash", hash);
            Console.WriteLine($"Success. SRAM Hash: {BitConverter.ToString(hash).Replace("-", "")}.");
            Console.WriteLine();
        }
    }
}
