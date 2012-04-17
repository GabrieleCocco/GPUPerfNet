using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPUPerfNet;

namespace KeyPi
{
    public enum KeyPiExceptionType
    {
        KEYPI_STATUS_ERROR_NULL_POINTER = 1,
        KEYPI_STATUS_ERROR_COUNTERS_NOT_OPEN,
        KEYPI_STATUS_ERROR_COUNTERS_ALREADY_OPEN,
        KEYPI_STATUS_ERROR_INDEX_OUT_OF_RANGE,
        KEYPI_STATUS_ERROR_NOT_FOUND,
        KEYPI_STATUS_ERROR_ALREADY_ENABLED,
        KEYPI_STATUS_ERROR_NO_COUNTERS_ENABLED,
        KEYPI_STATUS_ERROR_NOT_ENABLED,
        KEYPI_STATUS_ERROR_SAMPLING_NOT_STARTED,
        KEYPI_STATUS_ERROR_SAMPLING_ALREADY_STARTED,
        KEYPI_STATUS_ERROR_SAMPLING_NOT_ENDED,
        KEYPI_STATUS_ERROR_NOT_ENOUGH_PASSES,
        KEYPI_STATUS_ERROR_PASS_NOT_ENDED,
        KEYPI_STATUS_ERROR_PASS_NOT_STARTED,
        KEYPI_STATUS_ERROR_PASS_ALREADY_STARTED,
        KEYPI_STATUS_ERROR_SAMPLE_NOT_STARTED,
        KEYPI_STATUS_ERROR_SAMPLE_ALREADY_STARTED,
        KEYPI_STATUS_ERROR_SAMPLE_NOT_ENDED,
        KEYPI_STATUS_ERROR_CANNOT_CHANGE_COUNTERS_WHEN_SAMPLING,
        KEYPI_STATUS_ERROR_SESSION_NOT_FOUND,
        KEYPI_STATUS_ERROR_SAMPLE_NOT_FOUND,
        KEYPI_STATUS_ERROR_SAMPLE_NOT_FOUND_IN_ALL_PASSES,
        KEYPI_STATUS_ERROR_COUNTER_NOT_OF_SPECIFIED_TYPE,
        KEYPI_STATUS_ERROR_READING_COUNTER_RESULT,
        KEYPI_STATUS_ERROR_VARIABLE_NUMBER_OF_SAMPLES_IN_PASSES,
        KEYPI_STATUS_ERROR_FAILED,
        KEYPI_STATUS_ERROR_HARDWARE_NOT_SUPPORTED
    }

    public class KeyPiException : Exception
    {
        public KeyPiExceptionType Type
        {
            get;
            private set;
        }

        public KeyPiException(KeyPiExceptionType type, string message)
            : base(message)
        {
            this.Type = type;
        }

        internal static void CheckOrThrow(GPA_Status status, String message)
        {
            if (status != GPA_Status.GPA_STATUS_OK)
                throw new KeyPiException((KeyPiExceptionType)((int)status), message); 
        }
    }
}
