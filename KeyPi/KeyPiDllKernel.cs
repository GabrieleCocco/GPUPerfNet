using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using GPUPerfNet;

namespace KeyPi
{
    public class KeyPiDllKernelExecutionInfo
    {
        public DateTime StartTime
        {
            get;
            private set;
        }
        public DateTime EndTime
        {
            get;
            private set;
        }
        public int Result
        {
            get;
            private set;
        }

        public KeyPiDllKernelExecutionInfo(DateTime start_time, DateTime end_time, int result)
        {
            this.StartTime = start_time;
            this.EndTime = end_time;
            this.Result = result;
        }
    }

    internal static class KeyPiDllKernelUtil
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        internal static extern bool FreeLibrary(IntPtr hModule);
    }

    public class KeyPiDllKernel : KeyPiKernel
    {  
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void KeyPiDllInternalRun(out SYSTEMTIME start, out SYSTEMTIME end, out int result);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void KeyPiDllInternalInit(
            int argc, 
            [In] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)] String[] argv,
            out IntPtr queues,
            out IntPtr queues_names, 
            out int queues_count);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void KeyPiDllInternalRelease();

        public string DllPath
        {
            get;
            private set;
        }

        private IntPtr DllPointer;
        private KeyPiDllInternalInit InternalInit;
        private KeyPiDllInternalRun InternalRun;
        private KeyPiDllInternalRelease InternalRelease;

        internal KeyPiDllKernel(string name, string dll_path, string[] args) : base(name)
        {
            DllPath = dll_path;
            DllPointer = KeyPiDllKernelUtil.LoadLibrary(dll_path);
            if (DllPointer == IntPtr.Zero)
                throw new KeyPiException(KeyPiExceptionType.KEYPI_STATUS_ERROR_FAILED, "Cannot load KeyPi DLL " + dll_path);
            IntPtr pAddressOfFunctionToCall = KeyPiDllKernelUtil.GetProcAddress(DllPointer, "init");
            if (DllPointer == IntPtr.Zero)
                throw new KeyPiException(KeyPiExceptionType.KEYPI_STATUS_ERROR_FAILED, "Cannot find init function in KeyPi DLL " + dll_path);

            InternalInit = (KeyPiDllInternalInit)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(KeyPiDllInternalInit));

            pAddressOfFunctionToCall = KeyPiDllKernelUtil.GetProcAddress(DllPointer, "run");
            if (DllPointer == IntPtr.Zero)
                throw new KeyPiException(KeyPiExceptionType.KEYPI_STATUS_ERROR_FAILED, "Cannot find run function in KeyPi DLL " + dll_path);
            InternalRun = (KeyPiDllInternalRun)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(KeyPiDllInternalRun));

            pAddressOfFunctionToCall = KeyPiDllKernelUtil.GetProcAddress(DllPointer, "release");
            if (DllPointer == IntPtr.Zero)
                throw new KeyPiException(KeyPiExceptionType.KEYPI_STATUS_ERROR_FAILED, "Cannot find release function in KeyPi DLL " + dll_path);
            InternalRelease = (KeyPiDllInternalRelease)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(KeyPiDllInternalRelease));

            IntPtr queues_pointer = IntPtr.Zero;
            int queues_count = 0;
            IntPtr queues_names_pointer = IntPtr.Zero;
            InternalInit(args.Length, args, out queues_pointer, out queues_names_pointer, out queues_count);

            IntPtr[] queues_names = new IntPtr[queues_count];
            IntPtr[] queues = new IntPtr[queues_count];
            Marshal.Copy(queues_names_pointer, queues_names, 0, queues_count);
            Marshal.Copy(queues_pointer, queues, 0, queues_count);
            List<KeyPiContext> contexts = new List<KeyPiContext>();
            for (int i = 0; i < queues_count; i++) 
                contexts.Add(new KeyPiContext(Marshal.PtrToStringAnsi(queues_names[i]), queues[i]));

            this.Contexts = new Dictionary<string, KeyPiContext>();
            foreach (KeyPiContext context in contexts)
            {
                if (this.Contexts.ContainsKey(context.Name))
                    throw new KeyPiException(KeyPiExceptionType.KEYPI_STATUS_ERROR_FAILED, "A context called " + context.Name + " has just been added to the kernel " + this.Name);
                this.Contexts.Add(context.Name, context);
            }
        }

        internal KeyPiDllKernelExecutionInfo Run()
        {
            SYSTEMTIME start, end;
            int result;
            InternalRun(out start, out end, out result);
            
            DateTime start_time = new DateTime(start.Year, start.Month, start.Day, start.Hour, start.Minute, start.Second, start.Milliseconds);
            DateTime end_time = new DateTime(end.Year, end.Month, end.Day, end.Hour, end.Minute, end.Second, end.Milliseconds);
            return new KeyPiDllKernelExecutionInfo(start_time, end_time, result);
        }

        internal override void Close()
        {
            base.Close();
            InternalRelease();
            KeyPiDllKernelUtil.FreeLibrary(DllPointer);
        }
    }
}
