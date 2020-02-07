using System;

namespace Runner
{
    class Program
    {

        static void Main(string[] args)
        {
            ZsPkg_Core.ClangCtx z = new ZsPkg_Core.ClangCtx();
            z.Run("TestHeader/ikcp.c", "./Gen/ikcp.cs");
            if (Zeus.Utilities.CurrentPlatform.OS == Zeus.Utilities.OS.Windows)
            {
                Console.ReadKey();
            }
            else
            {
                Console.Read();
            }

        }
    }
}
