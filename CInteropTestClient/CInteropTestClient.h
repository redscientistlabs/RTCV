// declaration in .h file
#ifdef _CPLUSPLUS
extern "C" void Initialize();
#else
extern "C" void TestInitialize();
#endif

//Wrapper class for calling from the C++ side
class NetcoreClientInitializer
{
public:
	static void Initialize();
};
