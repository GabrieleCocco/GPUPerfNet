// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the KEYPITESTDLL_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// KEYPITESTDLL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef KEYPITESTDLL_EXPORTS
#define KEYPITESTDLL_API __declspec(dllexport)
#else
#define KEYPITESTDLL_API __declspec(dllimport)
#endif

// This class is exported from the KeyPiTestDll.dll
class KEYPITESTDLL_API CKeyPiTestDll {
public:
	CKeyPiTestDll(void);
	// TODO: add your methods here.
};

extern KEYPITESTDLL_API int nKeyPiTestDll;

KEYPITESTDLL_API int fnKeyPiTestDll(void);
