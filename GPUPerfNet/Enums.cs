using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPUPerfNet
{
    public enum GPA_Status : int {
        GPA_STATUS_OK = 0,
        GPA_STATUS_ERROR_NULL_POINTER,
        GPA_STATUS_ERROR_COUNTERS_NOT_OPEN,
        GPA_STATUS_ERROR_COUNTERS_ALREADY_OPEN,
        GPA_STATUS_ERROR_INDEX_OUT_OF_RANGE,
        GPA_STATUS_ERROR_NOT_FOUND,
        GPA_STATUS_ERROR_ALREADY_ENABLED,
        GPA_STATUS_ERROR_NO_COUNTERS_ENABLED,
        GPA_STATUS_ERROR_NOT_ENABLED,
        GPA_STATUS_ERROR_SAMPLING_NOT_STARTED,
        GPA_STATUS_ERROR_SAMPLING_ALREADY_STARTED,
        GPA_STATUS_ERROR_SAMPLING_NOT_ENDED,
        GPA_STATUS_ERROR_NOT_ENOUGH_PASSES,
        GPA_STATUS_ERROR_PASS_NOT_ENDED,
        GPA_STATUS_ERROR_PASS_NOT_STARTED,
        GPA_STATUS_ERROR_PASS_ALREADY_STARTED,
        GPA_STATUS_ERROR_SAMPLE_NOT_STARTED,
        GPA_STATUS_ERROR_SAMPLE_ALREADY_STARTED,
        GPA_STATUS_ERROR_SAMPLE_NOT_ENDED,
        GPA_STATUS_ERROR_CANNOT_CHANGE_COUNTERS_WHEN_SAMPLING,
        GPA_STATUS_ERROR_SESSION_NOT_FOUND,
        GPA_STATUS_ERROR_SAMPLE_NOT_FOUND,
        GPA_STATUS_ERROR_SAMPLE_NOT_FOUND_IN_ALL_PASSES,
        GPA_STATUS_ERROR_COUNTER_NOT_OF_SPECIFIED_TYPE,
        GPA_STATUS_ERROR_READING_COUNTER_RESULT,
        GPA_STATUS_ERROR_VARIABLE_NUMBER_OF_SAMPLES_IN_PASSES,
        GPA_STATUS_ERROR_FAILED,
        GPA_STATUS_ERROR_HARDWARE_NOT_SUPPORTED
    }
    
    public enum GPA_Type {
       GPA_TYPE_FLOAT32 = 0,             ///< Result will be a 32-bit float
       GPA_TYPE_FLOAT64,             ///< Result will be a 64-bit float
       GPA_TYPE_UINT32,              ///< Result will be a 32-bit unsigned int
       GPA_TYPE_UINT64,              ///< Result will be a 64-bit unsigned int
       GPA_TYPE_INT32,               ///< Result will be a 32-bit int
       GPA_TYPE_INT64,               ///< Result will be a 64-bit int
       GPA_TYPE__LAST                ///< Marker indicating last element
    }

    public enum GPA_Usage_Type {
        GPA_USAGE_TYPE_RATIO,         ///< Result is a ratio of two different values or types
        GPA_USAGE_TYPE_PERCENTAGE,    ///< Result is a percentage, typically within [0,100] range, but may be higher for certain counters
        GPA_USAGE_TYPE_CYCLES,        ///< Result is in clock cycles
        GPA_USAGE_TYPE_MILLISECONDS,  ///< Result is in milliseconds
        GPA_USAGE_TYPE_BYTES,         ///< Result is in bytes
        GPA_USAGE_TYPE_ITEMS,         ///< Result is a count of items or objects (ie, vertices, triangles, threads, pixels, texels, etc)
        GPA_USAGE_TYPE_KILOBYTES,     ///< Result is in kilobytes
        GPA_USAGE_TYPE__LAST          ///< Marker indicating last element
    }

    public enum GPA_Logging_Type {
        GPA_LOGGING_NONE = 0,
        GPA_LOGGING_ERROR = 1,
        GPA_LOGGING_MESSAGE = 2,
        GPA_LOGGING_ERROR_AND_MESSAGE = 3,
        GPA_LOGGING_TRACE = 4,
        GPA_LOGGING_ERROR_AND_TRACE = 5,
        GPA_LOGGING_MESSAGE_AND_TRACE = 6,
        GPA_LOGGING_ERROR_MESSAGE_AND_TRACE = 7,
        GPA_LOGGING_ALL = 0xFF
    }
}
