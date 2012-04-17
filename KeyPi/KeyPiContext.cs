using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPUPerfNet;

namespace KeyPi
{
    public class KeyPiContext
    {
        public String Name
        {
            get;
            private set;
        }

        public IntPtr ContextReference
        {
            get;
            private set;
        }

        public KeyPiCounter Counters(int index)
        {
            return new KeyPiCounter(index);
        }

        public void EnableAllCounters()
        {
            KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_EnableAllCounters(), "Cannot enable all the counters");
        }

        public void DisableAllCounters()
        {
            KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_DisableAllCounters(), "Cannot disable all the counters");
        }

        public int RequiredPassCount
        {
            get
            {
                uint count;
                KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_GetPassCount(out count), "Cannot get number of required pass");
                return (int)count;
            }
        }

        public int EnabledCountersCount
        {
            get
            {
                uint count;
                KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_GetEnabledCount(out count), "Cannot get number of enabled counters");
                return (int)count;
            }
        }

        public int CountersCount
        {
            get
            {
                uint count;
                KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_GetNumCounters(out count), "Cannot get number of counters");
                return (int)count;
            }
        }

        public KeyPiContext(String name, IntPtr context)
        {
            this.Name = name;
            this.ContextReference = context;
        }

        public override bool Equals(Object obj_context)
        {
            KeyPiContext context = (KeyPiContext)obj_context;
            if ((this.Name != null) && (context.Name != null) && (this.Name != context.Name))
                return false;
            return this.ContextReference.ToInt32() == context.ContextReference.ToInt32();
        }
    }
}
