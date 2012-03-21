using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPUPerfNet;
namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            GPA_Status status = GPUPerfCore.GPA_Initialize();
            status = GPUPerfCore.GPA_Destroy();

        }
    }
}
