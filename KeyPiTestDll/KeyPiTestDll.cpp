// KeyPiTestDll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "KeyPiTestDll.h"
#include <CL/cl.hpp>
#include <stdlib.h>
#include <time.h>

typedef void (*Callback) (SYSTEMTIME start, SYSTEMTIME end, int result);

typedef struct CommandQueueInfo {
	cl_command_queue queue;
	char* name;
} CommandQueueInfo;

// This is an example of an exported function.
extern "C" KEYPITESTDLL_API void init(int argc, char* argv[], CommandQueueInfo** queues, int* queues_count)
{
	*queues_count = 4;
	(*queues) = (CommandQueueInfo*)malloc((*queues_count) * sizeof(CommandQueueInfo));
	for(int i = 0; i < (*queues_count); i++) {
		(*queues)[i].queue = (cl_command_queue)(i + 10);
		(*queues)[i].name = (char*)malloc(13);
		memset((*queues)[i].name, 0, 13);
		sprintf((*queues)[i].name, "Prova nome %d", i);
	}
}

// This is an example of an exported function.
extern "C" KEYPITESTDLL_API void run(SYSTEMTIME* start, SYSTEMTIME* end, int* result)
{
	start->wSecond = 1;
	end->wSecond = 2;
	*result = 10;
}

extern "C" KEYPITESTDLL_API void release()
{
}