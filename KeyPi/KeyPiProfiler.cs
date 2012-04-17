using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPUPerfNet;
using System.Runtime.Remoting.Messaging;

namespace KeyPi
{
    public class KeyPiProfiler
    {
        private String current_context;

        private delegate KeyPiDllKernelExecutionInfo KeyPiDllKernelRun();
        public delegate void KeyPiDllKernelCallback(string dll_path, KeyPiDllKernelExecutionInfo info);

        public KeyPiContext CurrentContext
        {
            get
            {
                if (current_context == null)
                    throw new KeyPiException((KeyPiExceptionType)((int)(GPA_Status.GPA_STATUS_ERROR_INDEX_OUT_OF_RANGE)), "Invalid context name");
                return Kernel.Contexts[current_context];
            }
        }

        public KeyPiKernel Kernel
        {
            get;
            private set;
        }

        public KeyPiProfiler(string name, IEnumerable<KeyPiContext> contexts)
        {
            GPA_Status status = GPUPerfWrapper.GPA_Initialize();
            if (status != GPA_Status.GPA_STATUS_OK)
                throw new KeyPiException((KeyPiExceptionType)((int)(status)), "Exception raised");
            this.Kernel = new KeyPiExternalKernel(name, contexts);
            this.current_context = null;
        }

        public KeyPiProfiler(string name, string path, string[] args)
        {
            GPA_Status status = GPUPerfWrapper.GPA_Initialize();
            if (status != GPA_Status.GPA_STATUS_OK)
                throw new KeyPiException((KeyPiExceptionType)((int)(status)), "Exception raised");
            this.Kernel = new KeyPiDllKernel(name, path, args);
        }

        ~KeyPiProfiler()
        {
            try
            {
                Kernel.Close();
            }
            catch (KeyPiException)
            {
            }
            GPA_Status status = GPUPerfWrapper.GPA_Destroy();
            if (status != GPA_Status.GPA_STATUS_OK)
                throw new KeyPiException((KeyPiExceptionType)((int)(status)), "Exception raised");
        }

        public void OpenKernel()
        { 
            Kernel.Open();
            if (current_context == null)
                current_context = Kernel.Contexts.Keys.First();
        }
        
        public void CloseKernel()
        {
            Kernel.Close();
            current_context = null;
        }
        
        public void SelectKernelContext(string context_name)
        {
            if (context_name == null || !Kernel.Contexts.ContainsKey(context_name))
                throw new KeyPiException((KeyPiExceptionType)((int)(GPA_Status.GPA_STATUS_ERROR_INDEX_OUT_OF_RANGE)), "Invalid context name");
            current_context = context_name;

            GPA_Status status = GPUPerfWrapper.GPA_SelectContext(Kernel.Contexts[current_context].ContextReference);
            if (status != GPA_Status.GPA_STATUS_OK)
                throw new KeyPiException((KeyPiExceptionType)((int)(status)), "Exception raised");
        }

        private void RunCallback(IAsyncResult result)
        {
            KeyPiDllKernelRun run = (KeyPiDllKernelRun)((AsyncResult)result).AsyncDelegate;
            KeyPiDllKernelExecutionInfo info = run.EndInvoke(result);
            Tuple<KeyPiDllKernelCallback, string> state = (Tuple<KeyPiDllKernelCallback, string>)((AsyncResult)result).AsyncState;
            state.Item1(state.Item2, info);
        }

        public void RunAsync(KeyPiDllKernelCallback callback)
        {
            if (Kernel.GetType() == typeof(KeyPiDllKernel))
            {
                KeyPiDllKernelRun run = new KeyPiDllKernelRun(((KeyPiDllKernel)Kernel).Run);
                run.BeginInvoke(RunCallback, new Tuple<KeyPiDllKernelCallback, string>(callback, ((KeyPiDllKernel)Kernel).DllPath));
            }
        }

        public KeyPiDllKernelExecutionInfo Run()
        {
            if (Kernel.GetType() == typeof(KeyPiDllKernel))
                return ((KeyPiDllKernel)Kernel).Run();
            return null;
        }

        #region GPUPerf Mapping Functions

        public int BeginSession()
        {
            uint session_id;
            KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_BeginSession(out session_id), "Cannot begin session");
            return (int)session_id;
        }

        public void EndSession()
        {
            KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_EndSession(), "Cannot end session");
        }

        public void BeginPass()
        {
            KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_BeginPass(), "Cannot begin pass");
        }

        public void EndPass()
        {
            KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_EndPass(), "Cannot end pass");
        }

        public void BeginSample(int sample_id)
        {
            KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_BeginSample((uint)sample_id), "Cannot begin sample " + sample_id);
        }

        public void EndSample()
        {
            KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_EndSample(), "Cannot end sample");
        }
        
        public int SamplesCount(int session_id)
        {
            uint count;
            KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_GetSampleCount((uint)session_id, out count), "Cannot get samples count in session " + session_id);
            return (int)count;
        }
        
        public bool IsSampleReady(int session_id, int sample_id)
        {
            bool ready;
            KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_IsSampleReady(out ready, (uint)session_id, (uint)sample_id), "Cannot check if sample " + sample_id + " in session " + session_id + " is ready");
            return ready;
        }

        public bool IsSessionReady(int session_id)
        {
            bool ready;
            KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_IsSessionReady(out ready, (uint)session_id), "Cannot check if session " + session_id + " is ready");
            return ready;
        }

        #endregion
    }
}
