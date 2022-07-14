using System;
using System.IO;

namespace SF.Unpacker
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("GameLoft GLA2 Unpacker");
            Console.WriteLine("(c) 2022 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    GF.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of GLA2 archive file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    GF.Unpacker E:\\Games\\SF\\effects.gla2 D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_Gla2File = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_Gla2File))
            {
                Utils.iSetError("[ERROR]: Input GLA2 file -> " + m_Gla2File + " <- does not exist");
                return;
            }

            Gla2Unpack.iDoIt(m_Gla2File, m_Output);
        }
    }
}
