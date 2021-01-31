#include "CppUnitTest.h"
#include "../libsmlite/libsmlite.h"

#include <sstream>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

enum MyState { MyState_Rest, MyState_Ready, MyState_Reading, MyState_Writing };
enum MyTrigger { MyTrigger_Run, MyTrigger_Close, MyTrigger_Read, MyTrigger_FinishRead, MyTrigger_Write, MyTrigger_FinishWrite };



std::wstring _wstr (MyState _state) {
	std::wstringstream _ss;
	_ss << (int) _state;
	std::wstring _ret;
	_ss >> _ret;
	return _ret;
}

std::string _append (std::string _p1, int _p2) {
	std::stringstream _ss;
	_ss << _p1 << _p2;
	std::string _ret;
	_ss >> _ret;
	return _ret;
}

namespace Microsoft {
	namespace VisualStudio {
		namespace CppUnitTestFramework {
			template<> static std::wstring ToString<MyState> (const MyState& t) {
				return _wstr (t);
			}
			template<> static std::wstring ToString<MyState> (const MyState* t) {
				return _wstr (*t);
			}
			template<> static std::wstring ToString<MyState> (MyState* t) {
				return _wstr (*t);
			}
		}
	}
}

#ifdef _DEBUG
#	pragma comment (lib, "../../Debug/libsmlite.lib")
#else
#	pragma comment (lib, "../../Release/libsmlite.lib")
#endif



int n = 0;
bool entry_one = true;

void _rest_entry () { Assert::IsFalse (entry_one); entry_one = true; n += 1; }
void _rest_leave () { Assert::IsTrue (entry_one); entry_one = false; n += 10; }
void _ready_entry () { Assert::IsFalse (entry_one); entry_one = true; n += 100; }
void _ready_leave () { Assert::IsTrue (entry_one); entry_one = false; n += 1000; }
void _reading_entry () { Assert::IsFalse (entry_one); entry_one = true; n += 10000; }
void _reading_leave () { Assert::IsTrue (entry_one); entry_one = false; n += 100000; }
void _writing_entry () { Assert::IsFalse (entry_one); entry_one = true; n += 1000000; }
void _writing_leave () { Assert::IsTrue (entry_one); entry_one = false; n += 10000000; }



namespace libsmliteTest {
	TEST_CLASS (libsmliteTest) {
public:
		TEST_METHOD (TestMethod1) {
			psmlite_builder_t _smb = smlite_builder_create ();
			{
				psmlite_configstate_t _state = smlite_builder_configure (_smb, MyState_Rest);
				smlite_configstate_on_entry (_state, _rest_entry);
				smlite_configstate_on_leave (_state, _rest_leave);
				smlite_configstate_when_change_to (_state, MyTrigger_Run, MyState_Ready);
				smlite_configstate_when_ignore (_state, MyTrigger_Close);
			}
			{
				psmlite_configstate_t _state = smlite_builder_configure (_smb, MyState_Ready);
				smlite_configstate_on_entry (_state, _ready_entry);
				smlite_configstate_on_leave (_state, _ready_leave);
				smlite_configstate_when_change_to (_state, MyTrigger_Read, MyState_Reading);
				smlite_configstate_when_change_to (_state, MyTrigger_Write, MyState_Writing);
				smlite_configstate_when_change_to (_state, MyTrigger_Close, MyState_Rest);
			}
			{
				psmlite_configstate_t _state = smlite_builder_configure (_smb, MyState_Reading);
				smlite_configstate_on_entry (_state, _reading_entry);
				smlite_configstate_on_leave (_state, _reading_leave);
				smlite_configstate_when_change_to (_state, MyTrigger_FinishRead, MyState_Ready);
				smlite_configstate_when_change_to (_state, MyTrigger_Close, MyState_Rest);
			}
			{
				psmlite_configstate_t _state = smlite_builder_configure (_smb, MyState_Writing);
				smlite_configstate_on_entry (_state, _writing_entry);
				smlite_configstate_on_leave (_state, _writing_leave);
				smlite_configstate_when_change_to (_state, MyTrigger_FinishWrite, MyState_Ready);
				smlite_configstate_when_change_to (_state, MyTrigger_Close, MyState_Rest);
			}

			psmlite_t _sm = smlite_builder_build (_smb, MyState_Rest);
			Assert::AreEqual ((MyState) smlite_get_state (_sm), MyState_Rest);
			Assert::AreEqual (n, 0);
			Assert::IsTrue (smlite_allow_triggering (_sm, MyTrigger_Run));
			Assert::IsTrue (smlite_allow_triggering (_sm, MyTrigger_Close));
			Assert::IsFalse (smlite_allow_triggering (_sm, MyTrigger_Read));
			Assert::IsFalse (smlite_allow_triggering (_sm, MyTrigger_FinishRead));
			Assert::IsFalse (smlite_allow_triggering (_sm, MyTrigger_Write));
			Assert::IsFalse (smlite_allow_triggering (_sm, MyTrigger_FinishWrite));

			smlite_triggering (_sm, MyTrigger_Close);
			Assert::AreEqual ((MyState) smlite_get_state (_sm), MyState_Rest);
			Assert::AreEqual (n, 0);

			smlite_triggering (_sm, MyTrigger_Close);
			Assert::AreEqual ((MyState) smlite_get_state (_sm), MyState_Rest);
			Assert::AreEqual (n, 0);

			smlite_triggering (_sm, MyTrigger_Close);
			Assert::AreEqual ((MyState) smlite_get_state (_sm), MyState_Rest);
			Assert::AreEqual (n, 0);

			smlite_triggering (_sm, MyTrigger_Run);
			Assert::AreEqual ((MyState) smlite_get_state (_sm), MyState_Ready);
			Assert::AreEqual (n, 110);
			Assert::IsFalse (smlite_allow_triggering (_sm, MyTrigger_Run));
			Assert::IsTrue (smlite_allow_triggering (_sm, MyTrigger_Close));
			Assert::IsTrue (smlite_allow_triggering (_sm, MyTrigger_Read));
			Assert::IsFalse (smlite_allow_triggering (_sm, MyTrigger_FinishRead));
			Assert::IsTrue (smlite_allow_triggering (_sm, MyTrigger_Write));
			Assert::IsFalse (smlite_allow_triggering (_sm, MyTrigger_FinishWrite));

			smlite_triggering (_sm, MyTrigger_Close);
			Assert::AreEqual ((MyState) smlite_get_state (_sm), MyState_Rest);
			Assert::AreEqual (n, 1111);
			Assert::IsTrue (smlite_allow_triggering (_sm, MyTrigger_Run));
			Assert::IsTrue (smlite_allow_triggering (_sm, MyTrigger_Close));
			Assert::IsFalse (smlite_allow_triggering (_sm, MyTrigger_Read));
			Assert::IsFalse (smlite_allow_triggering (_sm, MyTrigger_FinishRead));
			Assert::IsFalse (smlite_allow_triggering (_sm, MyTrigger_Write));
			Assert::IsFalse (smlite_allow_triggering (_sm, MyTrigger_FinishWrite));

			smlite_triggering (_sm, MyTrigger_Run);
			Assert::AreEqual ((MyState) smlite_get_state (_sm), MyState_Ready);
			Assert::AreEqual (n, 1221);

			smlite_triggering (_sm, MyTrigger_Read);
			Assert::AreEqual ((MyState) smlite_get_state (_sm), MyState_Reading);
			Assert::AreEqual (n, 12221);

			smlite_triggering (_sm, MyTrigger_FinishRead);
			Assert::AreEqual ((MyState) smlite_get_state (_sm), MyState_Ready);
			Assert::AreEqual (n, 112321);

			smlite_triggering (_sm, MyTrigger_Write);
			Assert::AreEqual ((MyState) smlite_get_state (_sm), MyState_Writing);
			Assert::AreEqual (n, 1113321);

			smlite_triggering (_sm, MyTrigger_FinishWrite);
			Assert::AreEqual ((MyState) smlite_get_state (_sm), MyState_Ready);
			Assert::AreEqual (n, 11113421);

			smlite_set_state (_sm, MyState_Reading);
			smlite_set_state (_sm, MyState_Writing);
			smlite_set_state (_sm, MyState_Rest);
			Assert::AreEqual (n, 11113421);
		}

		TEST_METHOD (TestMethod3) {
			// TODO
			Assert::IsTrue (false);
		}
	};
}
