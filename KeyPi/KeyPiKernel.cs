using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPUPerfNet;

namespace KeyPi
{
    public class KeyPiKernel
    {
        public String Name
        {
            get;
            private set;
        }

        public Dictionary<string, KeyPiContext> Contexts
        {
            get;
            protected set;
        }

        internal KeyPiKernel(String name)
        {
            this.Name = name;
        }

        internal virtual void Open()
        {            
            foreach (KeyPiContext context in Contexts.Values)
                KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_OpenContext(context.ContextReference), "Cannot open the context " + context.Name);                
        }

        internal virtual void Close()
        {
            foreach (KeyPiContext context in Contexts.Values)
            {
                GPA_Status status = GPUPerfWrapper.GPA_SelectContext(context.ContextReference);
                if (status != GPA_Status.GPA_STATUS_OK)
                    throw new KeyPiException((KeyPiExceptionType)((int)(status)), "Exception raised");
                status = GPUPerfWrapper.GPA_CloseContext();
                if (status != GPA_Status.GPA_STATUS_OK)
                    throw new KeyPiException((KeyPiExceptionType)((int)(status)), "Exception raised");
            }
        }

        public override bool Equals(Object obj_kernel)
        {
            return this.Name == ((KeyPiKernel)obj_kernel).Name;
        }
    }
}
