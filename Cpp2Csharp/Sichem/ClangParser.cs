using ClangSharp.Interop;
using System;
using System.Collections.Generic;

namespace Sichem
{
	public class ClangParser
	{
		public BaseProcessor Processor { get; private set; }

		public string StringResult
		{
			get; set;
		}
		public static readonly string[] DefaultClangCommandLineArgs = new string[]
	  {
			"-std=c++17",                           // The input files should be compiled for C++ 17
            "-xc++",                                // The input files are C++
            "-Wno-pragma-once-outside-header"       // We are processing files which may be header files
	  };
		public void Process(ConversionParameters parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}

			var arr = new List<string>();

			foreach (var d in parameters.Defines)
			{
				arr.Add("-D" + d);
			}
			arr.AddRange(DefaultClangCommandLineArgs);
			//			arr.Add("-I" + @"D:\Develop\Microsoft Visual Studio 12.0\VC\include");

			var createIndex = CXIndex.Create();


			CXTranslationUnit tu;

			tu = CXTranslationUnit.Parse(createIndex,
				parameters.InputPath,
				arr.ToArray(), null, CXTranslationUnit_Flags.CXTranslationUnit_DetailedPreprocessingRecord | CXTranslationUnit_Flags.CXTranslationUnit_IncludeBriefCommentsInCodeCompletion | CXTranslationUnit_Flags.CXTranslationUnit_CXXChainedPCH);

			var numDiagnostics = tu.NumDiagnostics;
			for (uint i = 0; i < numDiagnostics; ++i)
			{
				var diag = tu.GetDiagnostic(i);
				var str = diag.Format(

							CXDiagnosticDisplayOptions.CXDiagnostic_DisplaySourceLocation |
							 CXDiagnosticDisplayOptions.CXDiagnostic_DisplaySourceRanges).ToString();
				Logger.LogLine(str);
				diag.Dispose();
				//clang.disposeDiagnostic(diag);
			}


			// Process
			var cw = new ConversionProcessor(parameters, tu);
			var io = new System.IO.StreamWriter("./dump.txt");
			var cw2 = new DumpProcessor(tu, io);
			cw2.Run();

			io.Close();
			Processor = cw;
			Processor.Run();
			/*			using (var tw = new StreamWriter(Path.Combine(parameters.OutputPath, "dump.txt")))
						{
							Processor = new DumpProcessor(tu, tw);
							Processor.Run();
						}*/

			if (cw.StringWriter != null)
			{
				StringResult = cw.StringWriter.ToString();
			}
			tu.Dispose();
			createIndex.Dispose();
			//.disposeTranslationUnit(tu);
			//clang.disposeIndex(createIndex);
		}
	}
}
