// KeyPiTestDll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "KeyPiTestDll.h"
#include <CL/cl.hpp>
#include <stdlib.h>
#include <time.h>

typedef void (*Callback) (SYSTEMTIME start, SYSTEMTIME end, int result);

// This is an example of an exported function.
extern "C" KEYPITESTDLL_API void setup(int argc, char* argv[], cl_command_queue** queues, char*** queues_names, int* queues_count)
{
	*queues_count = 4;
	*queues = (cl_command_queue*)malloc(4 * sizeof(cl_command_queue));
	*queues_names = (char**)malloc(4 * sizeof(char*));
	for(int i = 0; i < 4; i++) {
		(*queues_names)[i] = (char*)malloc(2);
		memset((*queues_names)[i], 0, 2);
		sprintf((*queues_names)[i], "%d", i);
	}
}

// This is an example of an exported function.
extern "C" KEYPITESTDLL_API void run(SYSTEMTIME* start, SYSTEMTIME* end, int* result)
{
	start->wSecond = 1;
	end->wSecond = 2;
	*result = 10;
}