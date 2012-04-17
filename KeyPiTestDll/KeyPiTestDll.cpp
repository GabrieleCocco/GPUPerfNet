// KeyPiTestDll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "KeyPiTestDll.h"
#include <CL/cl.hpp>
#include <stdlib.h>
#include <time.h>

typedef void (*Callback) (SYSTEMTIME start, SYSTEMTIME end, int result);

// This is an example of an exported function.
extern "C" KEYPITESTDLL_API void setup(int argc, char* argv[], cl_command_queue** queues, int* queues_count)
{
	*queues_count = 4;
}

// This is an example of an exported function.
extern "C" KEYPITESTDLL_API void run(SYSTEMTIME* start, SYSTEMTIME* end, int* result)
{
	start->wSecond = 1;
	end->wSecond = 2;
	*result = 10;
}