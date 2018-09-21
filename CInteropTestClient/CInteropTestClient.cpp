//A basic test implementation of Netcore in C++/CLR

#include <string>  
#include <iostream>
#include <msclr/marshal_cppstd.h>
//#include "tasks/tasks_internal.h"

#include "CInteropTestClient.h"

using namespace cli;
using namespace System;
using namespace RTCV;
using namespace RTCV::NetCore;
using namespace System::Runtime::InteropServices;
using namespace System::Diagnostics;

#using <system.dll>


/*
Trace::Listeners->Add(gcnew TextWriterTraceListener(Console::Out));
Trace::AutoFlush = true;
Trace::WriteLine(filename);
*/

//Create our NetcoreClient
extern "C" void TestInitialize()
{
	NetcoreClientInitializer::Initialize();
}

public ref class MemoryDomain
{
public:
	int size = 0;
	int offset = 0;
	bool bigendian = false;
	String ^ name = "NULL";
};


//Define this in here as it's managed
public ref class NetcoreClient
{
public:
	static RTCV::NetCore::NetCoreSpec ^ spec;
	static RTCV::NetCore::NetCoreConnector ^ connector;

	void OnMessageReceived(Object^  sender, RTCV::NetCore::NetCoreEventArgs^  e);

	void NetcoreClient::StartClient();
	void NetcoreClient::RestartClient();

	void LoadState(String^ filename);
	void NetcoreClient::SaveState(String^ filename, bool wait);
	void NetcoreClient::PokeByte(long long address, Byte ^ value, MemoryDomain ^ domain);
	Byte NetcoreClient::PeekByte(long long address, MemoryDomain ^ domain);
	void NetcoreClient::PokeBytes(long long address, array<Byte>^ value, int range, MemoryDomain ^ domain);
	array<Byte>^ NetcoreClient::PeekBytes(long long address, int range, MemoryDomain ^ domain);
	array<Byte>^ NetcoreClient::PeekAddresses(array<long long>^ addresses);
	void NetcoreClient::PokeAddresses(array<long long>^ addresses, array<Byte>^ values);

	MemoryDomain^ NetcoreClient::GetDomain(long long address, int range, MemoryDomain ^ domain);


	System::Random^ rand = gcnew Random();
};


//Create our NetcoreClient
void NetcoreClientInitializer::Initialize()
{
	NetcoreClient^ client = gcnew NetcoreClient;
	client->StartClient();
}

//Initialize it 
void NetcoreClient::StartClient() 
{
	NetcoreClient::spec = gcnew RTCV::NetCore::NetCoreSpec();
	NetcoreClient::spec->Side = NetworkSide::CLIENT;
	NetcoreClient::spec->MessageReceived += gcnew EventHandler<NetCoreEventArgs^>(this, &NetcoreClient::OnMessageReceived);

	NetcoreClient::connector = gcnew RTCV::NetCore::NetCoreConnector(spec);
	
}

void NetcoreClient::RestartClient() 
{
	NetcoreClient::connector->Kill();
	NetcoreClient::connector = nullptr;
	StartClient();
}


/* IMPLEMENT YOUR COMMANDS HERE */
void NetcoreClient::LoadState(String^ filename) 
{
	//Assuming your emulator uses std::string, you need to convert from System.String. This does that for you.
	std::string converted_filename = msclr::interop::marshal_as< std::string >(filename);
	//content_load_state(converted_filename.c_str(), false, true);
}

void NetcoreClient::SaveState(String^ filename, bool wait) 
{	
	//Assuming your emulator uses std::string, you need to convert from System.String. This does that for you.
	std::string converted_filename = msclr::interop::marshal_as< std::string >(filename);
}

void NetcoreClient::PokeByte(long long address, Byte ^ value, MemoryDomain ^ domain) 
{
	//IMPLEMENT YOUR POKE BYTE FUNCTION HERE
}

Byte NetcoreClient::PeekByte(long long address, MemoryDomain ^ domain) 
{
	//IMPLEMENT YOUR PEEK BYTE FUNCTION HERE
	return -1;
}

void NetcoreClient::PokeBytes(long long address, array<Byte>^ value, int range, MemoryDomain ^ domain) 
{
	for (int i = 0; i < range; i++)
		PokeByte(address + i, value[i], domain);
}


array<Byte>^ NetcoreClient::PeekBytes(long long address, int range, MemoryDomain ^ domain) 
{
	array<Byte>^ byte = gcnew array<Byte>(range);
	for (int i = 0; i < range; i++)
		byte[i] = PeekByte(address + i, domain);

	return byte;
}

array<Byte>^ NetcoreClient::PeekAddresses(array<long long>^ addresses) 
{
	MemoryDomain ^ domain = gcnew MemoryDomain;
	array<Byte>^ bytes = gcnew array<Byte>(sizeof(addresses));

	for (int i = 0; i < sizeof(addresses); i++) 
	{
		domain = GetDomain(addresses[i], 1, domain);
		bytes[i] = PeekByte(addresses[i], domain);
	}
	return bytes;
}

void NetcoreClient::PokeAddresses(array<long long>^ addresses, array<Byte>^ values) 
{

	MemoryDomain ^ domain = gcnew MemoryDomain;
	for (int i = 0; i < sizeof(addresses); i++) {
		domain = GetDomain(addresses[i], 1, domain);
		PokeByte(addresses[i], values[i], domain);
	}
}

MemoryDomain^ NetcoreClient::GetDomain(long long address, int range, MemoryDomain ^ domain)
{
	return domain;
}

/*ENUMS FOR THE SWITCH STATEMENT*/
enum COMMANDS 
{
	LOADSTATE,
	SAVESTATE,
	POKEBYTE,
	PEEKBYTE,
	POKEBYTES,
	PEEKBYTES,
	POKEADDRESSES,
	PEEKADDRESSES,
	UNKNOWN
};

COMMANDS CheckCommand(String^ inString) 
{
	if (inString == "LOADSTATE") return LOADSTATE;
	if (inString == "SAVESTATE") return SAVESTATE;
	if (inString == "POKEBYTE") return POKEBYTE;
	if (inString == "PEEKBYTE") return PEEKBYTE;
	if (inString == "POKEBYTES") return POKEBYTES;
	if (inString == "PEEKBYTES") return PEEKBYTES;
	if (inString == "POKEADDRESSES") return POKEADDRESSES;
	if (inString == "PEEKADDRESSES") return PEEKADDRESSES;
	return UNKNOWN;
}

/* THIS IS WHERE YOU HANDLE ANY RECEIVED MESSAGES */
void NetcoreClient::OnMessageReceived(Object^ sender, NetCoreEventArgs^ e)
{

	Trace::Listeners->Add(gcnew TextWriterTraceListener(Console::Out));
	Trace::AutoFlush = true;

	NetCoreMessage ^ message = e->message;

	NetCoreAdvancedMessage ^ advancedMessage = (NetCoreAdvancedMessage^)message;

	switch (CheckCommand(message->Type)) 
	{
		case LOADSTATE: 
		{
			LoadState((advancedMessage->objectValue)->ToString());
			break;
		}

		case SAVESTATE: 
		{
			SaveState((advancedMessage->objectValue)->ToString(), 0);
			break;
		}

		case POKEBYTE: 
		{

			long long address = Convert::ToInt64(((array<Object^>^)advancedMessage->objectValue)[0]);
			Byte value = Convert::ToByte(((array<Object^>^)advancedMessage->objectValue)[1]);

			MemoryDomain ^ domain = gcnew MemoryDomain;
			domain = GetDomain(address, 1, domain);

			PokeByte(address, value, domain);
			break;
		}

		case PEEKBYTE:
		{
			long long address = Convert::ToInt64(advancedMessage->objectValue);
			MemoryDomain ^ domain = gcnew MemoryDomain;

			domain = GetDomain(address, 1, domain);
			e->setReturnValue(PeekByte(address, domain));
			break;
		}
		case POKEBYTES: 
		{
			long long address = Convert::ToInt64(((array<Object^>^)advancedMessage->objectValue)[0]);
			int range = Convert::ToInt32(((array<Object^>^)advancedMessage->objectValue)[1]);
			array<Byte>^ value = (array<Byte>^)((array<Object^>^)advancedMessage->objectValue)[2];

			MemoryDomain ^ domain = gcnew MemoryDomain;
			domain = GetDomain(address, range, domain);

			PokeBytes(address, value, range, domain);
			break;
		}

		case PEEKBYTES:
		{
			long long address = Convert::ToInt64(((array<Object^>^)advancedMessage->objectValue)[0]);
			int range = Convert::ToByte(((array<Object^>^)advancedMessage->objectValue)[1]);

			MemoryDomain ^ domain = gcnew MemoryDomain;
			domain = GetDomain(address, range, domain);

			e->setReturnValue(PeekBytes(address, range, domain));
			break;
		}

		case POKEADDRESSES: 
		{
			array<long long>^ addresses = (array<long long>^)((array<Object^>^)advancedMessage->objectValue)[0];
			array<Byte>^ values = (array<Byte>^)((array<Object^>^)advancedMessage->objectValue)[1];
			PokeAddresses(addresses, values);
			break;
		}


		case PEEKADDRESSES:
		{
			array<long long>^ addresses = (array<long long>^)((array<Object^>^)advancedMessage->objectValue)[0];
			e->setReturnValue(PeekAddresses(addresses));
			break;
		}
		default:
			break;
	}
}


int main() {
	NetcoreClientInitializer::Initialize();

	//Since everything runs in another thread, we need the main thread to sleep forever.
	//This only matters for the test client. You'd call NetcoreClientInitializer::Initialize(); from the main thread of your emulator.
	System::Threading::Thread::Sleep(INT_MAX);
}