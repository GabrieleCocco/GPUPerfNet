using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPUPerfNet;

namespace KeyPi
{
    public enum KeyPiCounterUsageType {
        KEYPI_USAGE_TYPE_RATIO,         ///< Result is a ratio of two different values or types
        KEYPI_USAGE_TYPE_PERCENTAGE,    ///< Result is a percentage, typically within [0,100] range, but may be higher for certain counters
        KEYPI_USAGE_TYPE_CYCLES,        ///< Result is in clock cycles
        KEYPI_USAGE_TYPE_MILLISECONDS,  ///< Result is in milliseconds
        KEYPI_USAGE_TYPE_BYTES,         ///< Result is in bytes
        KEYPI_USAGE_TYPE_ITEMS,         ///< Result is a count of items or objects (ie, vertices, triangles, threads, pixels, texels, etc)
        KEYPI_USAGE_TYPE_KILOBYTES,     ///< Result is in kilobytes
        KEYPI_USAGE_TYPE__LAST          ///< Marker indicating last element
    }

    public class KeyPiCounter
    {
        private bool enable;

        public int Index
        {
            get;
            private set;
        }

        public String Name
        {
            get
            {
                String name;
                KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_GetCounterName((uint)Index, out name), "Cannot get the name of the counter " + Index);
                return name;
            }
        }

        public String Description
        {
            get
            {
                String description;
                KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_GetCounterDescription((uint)Index, out description), "Cannot get the description of the counter " + Index);
                return description;
            }
        }

        public bool IsEnable
        {
            get
            {
                GPA_Status status = GPUPerfWrapper.GPA_IsCounterEnabled((uint)Index);
                if(status == GPA_Status.GPA_STATUS_OK)
                    return true;
                if(status == GPA_Status.GPA_STATUS_ERROR_NOT_FOUND)
                    return false;
                else
                    throw new KeyPiException((KeyPiExceptionType)(int)(status), "Cannot check is the counter " + Index + " is enabled");
            }
            set
            {
                if (value == true && !enable)
                {
                    KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_EnableCounter((uint)Index), "Cannot enable the counter " + Index);
                    enable = value;
                }
                if (value == false && enable)
                {
                    KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_DisableCounter((uint)Index), "Cannot siable the counter " + Index);
                    enable = value;
                }
            }
        }

        public Object Value(int session_id, int sample_id)
        {
            Type type = DataType;
            if (type == typeof(float))
            {
                float value;
                KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_GetSampleFloat32((uint)session_id, (uint)sample_id, (uint)Index, out value), "Cannot get the value of the counter " + Index + " for the sample " + sample_id + " in the session " + session_id);
                return value;
            }
            if (type == typeof(double))
            {
                double value;
                KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_GetSampleFloat64((uint)session_id, (uint)sample_id, (uint)Index, out value), "Cannot get the value of the counter " + Index + " for the sample " + sample_id + " in the session " + session_id);
                return value;
            }
            if (type == typeof(Int32) || type == typeof(UInt32))
            {
                uint value;
                KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_GetSampleUInt32((uint)session_id, (uint)sample_id, (uint)Index, out value), "Cannot get the value of the counter " + Index + " for the sample " + sample_id + " in the session " + session_id);
                return (Int32)value;
            }
            if (type == typeof(Int64) || type == typeof(UInt64))
            {
                long value;
                KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_GetSampleUInt64((uint)session_id, (uint)sample_id, (uint)Index, out value), "Cannot get the value of the counter " + Index + " for the sample " + sample_id + " in the session " + session_id);
                return (Int64)value;
            }
            return null;
        }

        public Type DataType
        {
            get
            {
                GPA_Type type;
                KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_GetCounterDataType((uint)Index, out type), "Cannot get the type of the counter " + Index);

                Type generic_type = typeof(Object);
                switch (type)
                {
                    case GPA_Type.GPA_TYPE_FLOAT32:
                        {
                            generic_type = typeof(float);
                        } break;
                    case GPA_Type.GPA_TYPE_FLOAT64:
                        {
                            generic_type = typeof(double);
                        } break;
                    case GPA_Type.GPA_TYPE_UINT32:
                        {
                            generic_type = typeof(UInt32);
                        } break;
                    case GPA_Type.GPA_TYPE_UINT64:
                        {
                            generic_type = typeof(UInt64);
                        } break;
                    case GPA_Type.GPA_TYPE_INT32:
                        {
                            generic_type = typeof(Int32);
                        } break;
                    case GPA_Type.GPA_TYPE_INT64:
                        {
                            generic_type = typeof(Int64);
                        } break;
                    default:
                        {
                            return null;
                        }
                }
                return generic_type;
            }
        }

        public KeyPiCounterUsageType UsageType
        {
            get
            {
                GPA_Usage_Type usage_type;
                KeyPiException.CheckOrThrow(GPUPerfWrapper.GPA_GetCounterUsageType((uint)Index, out usage_type), "Cannot get the usage type of the counter " + Index);
                return (KeyPiCounterUsageType)(int)usage_type;
            }
        }
            
        public KeyPiCounter(int index)
        {
            this.Index = index;
            this.enable = false;
        }
    }
}
