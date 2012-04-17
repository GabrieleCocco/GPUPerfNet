using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;

namespace GPUPerfNet
{
    public delegate void GPA_LoggingCallbackPtrType(GPA_Logging_Type messageType, String message);

    public class GPUPerfWrapper
    {
        private const string DLLName = "GPUPerfAPICL.dll";

        [DllImport(DLLName, EntryPoint = "GPA_RegisterLoggingCallback", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_RegisterLoggingCallback(GPA_Logging_Type loggingType, GPA_LoggingCallbackPtrType callbackFuncPtr );

        // Init / destroy GPA

        /// \brief Initializes the driver so that counters are exposed.
        ///
        /// This function must be called before the rendering context or device is created.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        /// 
        [DllImport(DLLName, EntryPoint = "GPA_Initialize", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_Initialize();

        /// \brief Undo any initialization to ensure proper behavior in applications that are not being profiled.
        ///
        /// This function must be called after the rendering context or device is released / destroyed.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_Destroy", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_Destroy();

        // Context Startup / Finish

        /// \brief Opens the counters in the specified context for reading.
        ///
        /// This function must be called before any other GPA functions.
        /// \param context The context to open counters for. Typically a device pointer. Refer to GPA API specific documentation for further details.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_OpenContext", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_OpenContext(IntPtr context);

        /// \brief Closes the counters in the currently active context.
        ///
        /// GPA functions should not be called again until the counters are reopened with GPA_OpenContext.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_CloseContext", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_CloseContext();

        /// \brief Select another context to be the currently active context.
        ///
        /// The selected context must have previously been opened with a call to GPA_OpenContext.
        /// If the call is successful, all GPA functions will act on the currently selected context.
        /// \param context The context to select. The same value that was passed to GPA_OpenContext.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_SelectContext", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_SelectContext(IntPtr context);


        // Counter Interrogation

        /// \brief Get the number of counters available.
        ///
        /// \param count The value which will hold the count upon successful execution.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetNumCounters", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetNumCounters(out uint count);

        /// \brief Get the name of a specific counter.
        ///
        /// \param index The index of the counter name to query. Must lie between 0 and (GPA_GetNumCounters result - 1).
        /// \param name The value which will hold the name upon successful execution.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetCounterName", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetCounterName(uint index, out String name);

        /// \brief Get description of the specified counter.
        ///
        /// \param index The index of the counter to query. Must lie between 0 and (GPA_GetNumCounters result - 1).
        /// \param description The value which will hold the description upon successful execution.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetCounterDescription", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetCounterDescription(uint index, out String name);

        /// \brief Get the counter data type of the specified counter.
        ///
        /// \param index The index of the counter. Must lie between 0 and (GPA_GetNumCounters result - 1).
        /// \param counterDataType The value which will hold the description upon successful execution.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetCounterDataType", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetCounterDataType(uint index, out GPA_Type counterDataType );

        /// \brief Get the counter usage type of the specified counter.
        ///
        /// \param index The index of the counter. Must lie between 0 and (GPA_GetNumCounters result - 1).
        /// \param counterUsageType The value which will hold the description upon successful execution.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetCounterUsageType", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetCounterUsageType(uint index, out GPA_Usage_Type counterUsageType );

        /// \brief Get a string with the name of the specified counter data type.
        ///
        /// Typically used to display counter types along with their name.
        /// E.g. counterDataType of GPA_TYPE_UINT64 would return "gpa_uint64".
        /// \param counterDataType The type to get the string for.
        /// \param typeStr The value that will be set to contain a reference to the name of the counter data type.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetDataTypeAsStr", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetDataTypeAsStr( GPA_Type counterDataType, out String name);

        /// \brief Get a string with the name of the specified counter usage type.
        ///
        /// Convertes the counter usage type to a string representation.
        /// E.g. counterUsageType of GPA_USAGE_TYPE_PERCENTAGE would return "percentage".
        /// \param counterUsageType The type to get the string for.
        /// \param usageTypeStr The value that will be set to contain a reference to the name of the counter usage type.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetUsageTypeAsStr", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetUsageTypeAsStr(GPA_Usage_Type counterUsageType, out String name);

        /// \brief Enable a specified counter.
        ///
        /// Subsequent sampling sessions will provide values for any enabled counters.
        /// Initially all counters are disabled, and must explicitly be enabled by calling this function.
        /// \param index The index of the counter to enable. Must lie between 0 and (GPA_GetNumCounters result - 1).
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_EnableCounter", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_EnableCounter(uint index);


        /// \brief Disable a specified counter.
        ///
        /// Subsequent sampling sessions will not provide values for any disabled counters.
        /// Initially all counters are disabled, and must explicitly be enabled.
        /// \param index The index of the counter to enable. Must lie between 0 and (GPA_GetNumCounters result - 1).
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_DisableCounter", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_DisableCounter(uint index);


        /// \brief Get the number of enabled counters.
        ///
        /// \param count The value that will be set to the number of counters that are currently enabled.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetEnabledCount", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetEnabledCount(out uint count);


        /// \brief Get the counter index for an enabled counter.
        ///
        /// For example, if GPA_GetEnabledIndex returns 3, and I wanted the counter index of the first enabled counter,
        /// I would call this function with enabledNumber equal to 0.
        /// \param enabledNumber The number of the enabled counter to get the counter index for. Must lie between 0 and (GPA_GetEnabledIndex result - 1).
        /// \param enabledCounterIndex The value that will contain the index of the counter.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetEnabledIndex", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetEnabledIndex(uint enabledNumber, out uint enabledCounterIndex);


        /// \brief Check that a counter is enabled.
        ///
        /// \param counterIndex The index of the counter. Must lie between 0 and (GPA_GetNumCounters result - 1).
        /// \return GPA_STATUS_OK is returned if the counter is enabled, GPA_STATUS_ERROR_NOT_FOUND otherwise.
        [DllImport(DLLName, EntryPoint = "GPA_IsCounterEnabled", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_IsCounterEnabled(uint counterIndex);


        /// \brief Enable a specified counter using the counter name (case insensitive).
        ///
        /// Subsequent sampling sessions will provide values for any enabled counters.
        /// Initially all counters are disabled, and must explicitly be enabled by calling this function.
        /// \param counter The name of the counter to enable.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_EnableCounterStr", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_EnableCounterStr(String counter);


        /// \brief Disable a specified counter using the counter name (case insensitive).
        ///
        /// Subsequent sampling sessions will not provide values for any disabled counters.
        /// Initially all counters are disabled, and must explicitly be enabled.
        /// \param counter The name of the counter to disable.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_DisableCounterStr", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_DisableCounterStr(String counter);


        /// \brief Enable all counters.
        ///
        /// Subsequent sampling sessions will provide values for all counters.
        /// Initially all counters are disabled, and must explicitly be enabled by calling a function which enables them.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_EnableAllCounters", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_EnableAllCounters();


        /// \brief Disable all counters.
        ///
        /// Subsequent sampling sessions will not provide values for any disabled counters.
        /// Initially all counters are disabled, and must explicitly be enabled.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_DisableAllCounters", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_DisableAllCounters();


        /// \brief Get index of a counter given its name (case insensitive).
        ///
        /// \param counter The name of the counter to get the index for.
        /// \param index The index of the requested counter.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetCounterIndex", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetCounterIndex(String counter, out uint index);


        /// \brief Get the number of passes required for the currently enabled set of counters.
        ///
        /// This represents the number of times the same sequence must be repeated to capture the counter data.
        /// On each pass a different (compatible) set of counters will be measured.
        /// \param numPasses The value of the number of passes.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetPassCount", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetPassCount(out uint numPasses);


        /// \brief Begin sampling with the currently enabled set of counters.
        ///
        /// This must be called to begin the counter sampling process.
        /// A unique sessionID will be returned which is later used to retrieve the counter values.
        /// Session Identifiers are integers and always start from 1 on a newly opened context, upwards in sequence.
        /// The set of enabled counters cannot be changed inside a BeginSession/EndSession sequence.
        /// \param sessionID The value that will be set to the session identifier.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_BeginSession", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_BeginSession(out uint sessionID);


        /// \brief End sampling with the currently enabled set of counters.
        ///
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_EndSession", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_EndSession();


        /// \brief Begin sampling pass.
        ///
        /// Between BeginPass and EndPass calls it is expected that a sequence of repeatable operations exist.
        /// If this is not the case only counters which execute in a single pass should be activated.
        /// The number of required passes can be determined by enabling a set of counters and then calling GPA_GetPassCount.
        /// The operations inside the BeginPass/EndPass calls should be looped over GPA_GetPassCount result number of times.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_BeginPass", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_BeginPass();


        /// \brief End sampling pass.
        ///
        /// Between BeginPass and EndPass calls it is expected that a sequence of repeatable operations exist.
        /// If this is not the case only counters which execute in a single pass should be activated.
        /// The number of required passes can be determined by enabling a set of counters and then calling GPA_GetPassCount.
        /// The operations inside the BeginPass/EndPass calls should be looped over GPA_GetPassCount result number of times.
        /// This is necessary to capture all counter values, since sometimes counter combinations cannot be captured simultaneously.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_EndPass", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_EndPass();


        /// \brief Begin a sample using the enabled counters.
        ///
        /// Multiple samples can be performed inside a BeginSession/EndSession sequence.
        /// Each sample computes the values of the counters between BeginSample and EndSample.
        /// To identify each sample the user must provide a unique sampleID as a paramter to this function.
        /// The number need only be unique within the same BeginSession/EndSession sequence.
        /// BeginSample must be followed by a call to EndSample before BeginSample is called again.
        /// \param sampleID Any integer, unique within the BeginSession/EndSession sequence, used to retrieve the sample results.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_BeginSample", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_BeginSample(uint sampleID );


        /// \brief End sampling using the enabled counters.
        ///
        /// BeginSample must be followed by a call to EndSample before BeginSample is called again.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_EndSample", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_EndSample();

        /// \brief Get the number of samples a specified session contains.
        ///
        /// This is useful if samples are conditionally created and a count is not kept.
        /// \param sessionID The session to get the number of samples for.
        /// \param samples The value that will be set to the number of samples contained within the session.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetSampleCount", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetSampleCount(uint sessionID, out uint samples);


        /// \brief Determine if an individual sample result is available.
        ///
        /// After a sampling session results may be available immediately or take a certain amount of time to become available.
        /// This function allows you to determine when a sample can be read. 
        /// The function does not block, permitting periodic polling.
        /// To block until a sample is ready use a GetSample* function instead of this.
        /// It can be more efficient to determine if a whole session's worth of data is available using GPA_IsSessionReady.
        /// \param readyResult The value that will contain the result of the sample being ready. True if ready.
        /// \param sessionID The session containing the sample to determine availability.
        /// \param sampleID The sample identifier of the sample to query availability for.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_IsSampleReady", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_IsSampleReady(out bool readyResult, uint sessionID, uint sampleID);


        /// \brief Determine if all samples within a session are available.
        ///
        /// After a sampling session results may be available immediately or take a certain amount of time to become available.
        /// This function allows you to determine when the results of a session can be read. 
        /// The function does not block, permitting periodic polling.
        /// To block until a sample is ready use a GetSample* function instead of this.
        /// \param readyResult The value that will contain the result of the session being ready. True if ready.
        /// \param sessionID The session to determine availability for.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_IsSessionReady", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_IsSessionReady(out bool readyResult, uint sessionID);


        /// \brief Get a sample of type 64-bit unsigned integer.
        ///
        /// This function will block until the value is available.
        /// Use GPA_IsSampleReady if you do not wish to block.
        /// \param sessionID The session identifier with the sample you wish to retreive the result of.
        /// \param sampleID The identifier of the sample to get the result for.
        /// \param counterID The counter index to get the result for.
        /// \param result The value which will contain the counter result upon successful execution.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetSampleUInt64", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetSampleUInt64(uint sessionID, uint sampleID, uint counterID, out long result);


        /// \brief Get a sample of type 32-bit unsigned integer.
        ///
        /// This function will block until the value is available.
        /// Use GPA_IsSampleReady if you do not wish to block.
        /// \param sessionID The session identifier with the sample you wish to retreive the result of.
        /// \param sampleID The identifier of the sample to get the result for.
        /// \param counterIndex The counter index to get the result for.
        /// \param result The value which will contain the counter result upon successful execution.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetSampleUInt32", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetSampleUInt32(uint sessionID, uint sampleID, uint counterIndex, out uint result);


        /// \brief Get a sample of type 64-bit float.
        ///
        /// This function will block until the value is available.
        /// Use GPA_IsSampleReady if you do not wish to block.
        /// \param sessionID The session identifier with the sample you wish to retreive the result of.
        /// \param sampleID The identifier of the sample to get the result for.
        /// \param counterIndex The counter index to get the result for.
        /// \param result The value which will contain the counter result upon successful execution.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetSampleFloat64", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetSampleFloat64(uint sessionID, uint sampleID, uint counterIndex, out double result );


        /// \brief Get a sample of type 32-bit float.
        ///
        /// This function will block until the value is available.
        /// Use GPA_IsSampleReady if you do not wish to block.
        /// \param sessionID The session identifier with the sample you wish to retreive the result of.
        /// \param sampleID The identifier of the sample to get the result for.
        /// \param counterIndex The counter index to get the result for.
        /// \param result The value which will contain the counter result upon successful execution.
        /// \return The GPA result status of the operation. GPA_STATUS_OK is returned if the operation is successful.
        [DllImport(DLLName, EntryPoint = "GPA_GetSampleFloat32", CallingConvention = CallingConvention.Cdecl)]      
        public extern static GPA_Status GPA_GetSampleFloat32(uint sessionID, uint sampleID, uint counterIndex, out float result);


        /// \brief Get a string translation of a GPA status value.
        ///
        /// Provides a simple method to convert a status enum value into a string which can be used to display log messages.
        /// \param status The status to convert into a string.
        /// \return A string which describes the supplied status.
        [DllImport(DLLName, EntryPoint = "GPA_GetStatusAsStr", CallingConvention = CallingConvention.Cdecl)]      
        public extern static String GPA_GetStatusAsStr(uint status);
    }
}
