using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyPi;
namespace Tester
{
    class Program
    {
        static KeyPiProfiler profiler;
        static void Main(string[] args)
        {
            //Args to initialized the DLL
            String[] dllArgs = new String[] {
		        "33554432", 
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
            
            //Create e profiler for a DLL
            profiler = new KeyPiProfiler(
                "Saxpy",
                "SaxpyLib.dll",
                dllArgs);

            //Open the kernel associated tot he profiler (so contexts are opened)
            profiler.OpenKernel();

            /* Setup sessions */
            //Enable all counters
            profiler.CurrentContext.EnableAllCounters();
            
            //One session, number fo passes = #passes needed to computer the counters enabled
            int session_id = profiler.BeginSession();
            for (int i = 0; i < profiler.CurrentContext.RequiredPassCount; i++)
            {
                profiler.BeginPass();
                profiler.BeginSample(0);
                profiler.Run();
                profiler.EndSample();
                profiler.EndPass();
            }
            profiler.EndSession();

            //Wait for the values of the counters to be ready
            while (!profiler.IsSampleReady(session_id, 0)) { }

            //Print the counters values to the screen
            for (int i = 0; i < profiler.CurrentContext.EnabledCountersCount; i++)
            {
                KeyPiCounter counter = profiler.CurrentContext.Counters(i);
                Console.WriteLine(counter.Name + "[TYPE " + counter.DataType + "] = " + counter.Value(session_id, 0));
            }
            profiler.CloseKernel();
        }

        static void Callback(string dll, KeyPiDllKernelExecutionInfo result)
        {
            Console.WriteLine(dll + ":" + result.Result);
            Console.Read();
        }
    }
}
