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
		        "1", 
		        "1", 
		        "2.0", 
		        "0", 
		        "1", 
		        "4", 
		        "2", 
		        @"C:\Users\gabriele\APUBenchmarks\Debug\saxpy.cl",
		        "",
		        "0", "128", "0", "0", "saxpy4",
		        "1", "128", "0", "0", "saxpy4"
            };
            
            //Create e profiler for a DLL

            //Open the kernel associated tot he profiler (so contexts are opened)

            /* Setup sessions */
            //Enable all counters
            
            //One session, number fo passes = #passes needed to computer the counters enabled
            for (int i = 0; i < 20; i++)
            {
                profiler = new KeyPiProfiler(
                    "Saxpy",
                    "SaxpyLib.dll",
                    dllArgs);

                profiler.OpenKernel();
                profiler.CurrentContext.Counters(i).IsEnable = true;

                Console.WriteLine("Sampling counters " + profiler.CurrentContext.EnabledCountersCount + ": " + profiler.CurrentContext.Counters(i).Name);
                int session_id = profiler.BeginSession();
                profiler.BeginPass();
                profiler.BeginSample(i);
                profiler.Run();
                profiler.EndSample();
                profiler.EndPass();
                profiler.EndSession();


                while (!profiler.IsSampleReady(session_id, i)) { }
                KeyPiCounter counter = profiler.CurrentContext.Counters(i);
                Console.WriteLine(counter.Name + "[TYPE " + counter.DataType + "] = " + counter.Value(session_id, i));

                profiler.CurrentContext.Counters(i).IsEnable = false;

                profiler.CloseKernel();
                System.Threading.Thread.Sleep(500);
            }
            Console.Read();
        }

        static void Callback(string dll, KeyPiDllKernelExecutionInfo result)
        {
            Console.WriteLine(dll + ":" + result.Result);
            Console.Read();
        }
    }
}
