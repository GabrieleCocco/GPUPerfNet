using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyPi;
namespace Tester
{
    class Program
    {
            static KeyPiProfiler profiler = new KeyPiProfiler();
        static void Main(string[] args)
        {

            String[] dllArgs = new String[] {
		        "16777216", 
		        "10", 
		        "1", 
		        "2.0", 
		        "0", 
		        "TEST_MODE_DEVICE", 
		        "4", 
		        "1", 
		        @"C:\Users\gabriele\APUBenchmarks\Debug\saxpy.cl",
		        "",
		        "0", "128", "MEMORY_MODE_HOST_MAP", "MEMORY_MODE_HOST_MAP", "saxpy4",
		        "test_saxpy.csv",
		        "0" };
            profiler.OpenDllKernel(
                "KeyPiTestDll.dll",
                dllArgs);
            
            /* Setup sessions */
            profiler.RunAll(Callback);
        }

        static void Callback(string dll, KeyPiDllKernelExecutionInfo result)
        {
            Console.WriteLine(dll + ":" + result.Result);
            Console.Read();
        }

























        }
    }
}
