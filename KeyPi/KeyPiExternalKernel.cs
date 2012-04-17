using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPUPerfNet;

namespace KeyPi
{
    public class KeyPiExternalKernel : KeyPiKernel
    {        
        internal KeyPiExternalKernel(String name, IEnumerable<KeyPiContext> contexts)
            : base(name)
        {
            this.Contexts = new Dictionary<string, KeyPiContext>();
            foreach (KeyPiContext context in contexts)
            {
                if (this.Contexts.ContainsKey(context.Name))
                    throw new KeyPiException(KeyPiExceptionType.KEYPI_STATUS_ERROR_FAILED, "A context called " + context.Name + " has just been added to the kernel " + this.Name);
                this.Contexts.Add(context.Name, context);
            }
        }
    }
}
