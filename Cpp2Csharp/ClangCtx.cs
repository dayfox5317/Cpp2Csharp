using ShellProgressBar;
using Sichem;
using System;
using System.IO;

namespace ZsPkg_Core
{
    public class ClangCtx
    {




        public void Run(string in_file, string out_file, string module = "Zeus_Mod", params string[] df)
        {
            const int totalTicks = 0;
            var options = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Yellow,
                ForegroundColorDone = ConsoleColor.DarkGreen,
                BackgroundColor = ConsoleColor.DarkGray,
                ProgressBarOnBottom = true,
                BackgroundCharacter = '\u2593'
            };
            string temp = string.Empty;
            using (var pbar = new ProgressBar(totalTicks, "process=>", options))
            {

                Logger.LogFunction = (info) =>
                {

                    pbar.Tick(info);
                    temp += info;
                };


                var parameters = new ConversionParameters
                {
                    InputPath = in_file,
                    ConversionMode = ConversionMode.SingleString,
                    Defines = df,
                    Namespace = module,
                    Class = module,
                    OutputPath = "./",
                    SkipStructs = new string[]
                   {
                   },
                    SkipGlobalVariables = new string[]
                   {
                   },
                    SkipFunctions = new string[]
                   {

                   },
                    Classes = new string[]
                   {

                   },
                    GlobalArrays = new string[]
                   {
                   },
                    GenerateSafeCode = false,
                };

                var cp = new ClangParser();

                cp.Process(parameters);

                // Post processing
                Logger.Info("Post processing...");

                var data = cp.StringResult;
                data = Utility.ReplaceNativeCalls(data);

                var s = System.IO.Path.GetDirectoryName(out_file);
                if (!Directory.Exists(s))
                {
                    Directory.CreateDirectory(s);
                }
                File.WriteAllText(out_file, data);

            }
            Console.Write(temp);
        }



    }

}
