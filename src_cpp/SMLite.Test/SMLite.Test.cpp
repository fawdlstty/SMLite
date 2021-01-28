#include "CppUnitTest.h"
#include "../SMLite/SMLite.hpp"

#include <sstream>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

enum class MyState { Rest, Ready, Reading, Writing };
enum class MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };



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
			template<> static std::wstring ToString<MyState> (const MyState &t) { return _wstr (t); }
			template<> static std::wstring ToString<MyState> (const MyState *t) { return _wstr (*t); }
			template<> static std::wstring ToString<MyState> (MyState *t) { return _wstr (*t); }
		}
	}
}



namespace SMLiteTest {
	TEST_CLASS (SMLiteTest) {
	public:
		TEST_METHOD (TestMethod1) {
			int n = 0;
			bool entry_one = true;
			Fawdlstty::SMLiteBuilder<MyState, MyTrigger> _smb {};
			_smb.Configure (MyState::Rest)
				->OnEntry ([&] () { Assert::IsFalse (entry_one); entry_one = true; n += 1; })
				->OnLeave ([&] () { Assert::IsTrue (entry_one); entry_one = false; n += 10; })
				->WhenChangeTo (MyTrigger::Run, MyState::Ready)
				->WhenIgnore (MyTrigger::Close);
			_smb.Configure (MyState::Ready)
				->OnEntry ([&] () { Assert::IsFalse (entry_one); entry_one = true; n += 100; })
				->OnLeave ([&] () { Assert::IsTrue (entry_one); entry_one = false; n += 1000; })
				->WhenChangeTo (MyTrigger::Read, MyState::Reading)
				->WhenChangeTo (MyTrigger::Write, MyState::Writing)
				->WhenChangeTo (MyTrigger::Close, MyState::Rest);
			_smb.Configure (MyState::Reading)
				->OnEntry ([&] () { Assert::IsFalse (entry_one); entry_one = true; n += 10000; })
				->OnLeave ([&] () { Assert::IsTrue (entry_one); entry_one = false; n += 100000; })
				->WhenChangeTo (MyTrigger::FinishRead, MyState::Ready)
				->WhenChangeTo (MyTrigger::Close, MyState::Rest);
			_smb.Configure (MyState::Writing)
				->OnEntry ([&] () { Assert::IsFalse (entry_one); entry_one = true; n += 1000000; })
				->OnLeave ([&] () { Assert::IsTrue (entry_one); entry_one = false; n += 10000000; })
				->WhenChangeTo (MyTrigger::FinishWrite, MyState::Ready)
				->WhenChangeTo (MyTrigger::Close, MyState::Rest);

			auto _sm = _smb.Build (MyState::Rest);
			Assert::AreEqual (_sm->GetState (), MyState::Rest);
			Assert::AreEqual (n, 0);
			Assert::IsTrue (_sm->AllowTriggering (MyTrigger::Run));
			Assert::IsTrue (_sm->AllowTriggering (MyTrigger::Close));
			Assert::IsFalse (_sm->AllowTriggering (MyTrigger::Read));
			Assert::IsFalse (_sm->AllowTriggering (MyTrigger::FinishRead));
			Assert::IsFalse (_sm->AllowTriggering (MyTrigger::Write));
			Assert::IsFalse (_sm->AllowTriggering (MyTrigger::FinishWrite));

			_sm->Triggering (MyTrigger::Close);
			Assert::AreEqual (_sm->GetState (), MyState::Rest);
			Assert::AreEqual (n, 0);

			_sm->Triggering (MyTrigger::Close);
			Assert::AreEqual (_sm->GetState (), MyState::Rest);
			Assert::AreEqual (n, 0);

			_sm->Triggering (MyTrigger::Close);
			Assert::AreEqual (_sm->GetState (), MyState::Rest);
			Assert::AreEqual (n, 0);

			_sm->Triggering (MyTrigger::Run);
			Assert::AreEqual (_sm->GetState (), MyState::Ready);
			Assert::AreEqual (n, 110);
			Assert::IsFalse (_sm->AllowTriggering (MyTrigger::Run));
			Assert::IsTrue (_sm->AllowTriggering (MyTrigger::Close));
			Assert::IsTrue (_sm->AllowTriggering (MyTrigger::Read));
			Assert::IsFalse (_sm->AllowTriggering (MyTrigger::FinishRead));
			Assert::IsTrue (_sm->AllowTriggering (MyTrigger::Write));
			Assert::IsFalse (_sm->AllowTriggering (MyTrigger::FinishWrite));

			_sm->Triggering (MyTrigger::Close);
			Assert::AreEqual (_sm->GetState (), MyState::Rest);
			Assert::AreEqual (n, 1111);
			Assert::IsTrue (_sm->AllowTriggering (MyTrigger::Run));
			Assert::IsTrue (_sm->AllowTriggering (MyTrigger::Close));
			Assert::IsFalse (_sm->AllowTriggering (MyTrigger::Read));
			Assert::IsFalse (_sm->AllowTriggering (MyTrigger::FinishRead));
			Assert::IsFalse (_sm->AllowTriggering (MyTrigger::Write));
			Assert::IsFalse (_sm->AllowTriggering (MyTrigger::FinishWrite));

			_sm->Triggering (MyTrigger::Run);
			Assert::AreEqual (_sm->GetState (), MyState::Ready);
			Assert::AreEqual (n, 1221);

			_sm->Triggering (MyTrigger::Read);
			Assert::AreEqual (_sm->GetState (), MyState::Reading);
			Assert::AreEqual (n, 12221);

			_sm->Triggering (MyTrigger::FinishRead);
			Assert::AreEqual (_sm->GetState (), MyState::Ready);
			Assert::AreEqual (n, 112321);

			_sm->Triggering (MyTrigger::Write);
			Assert::AreEqual (_sm->GetState (), MyState::Writing);
			Assert::AreEqual (n, 1113321);

			_sm->Triggering (MyTrigger::FinishWrite);
			Assert::AreEqual (_sm->GetState (), MyState::Ready);
			Assert::AreEqual (n, 11113421);

			_sm->SetState (MyState::Reading);
			_sm->SetState (MyState::Writing);
			_sm->SetState (MyState::Rest);
			Assert::AreEqual (n, 11113421);
		}

		TEST_METHOD (TestMethod3) {
			std::string s = "";
			std::function<void (MyState, MyTrigger, std::string)> _f = [&] (MyState _state, MyTrigger _trigger, std::string _p1) { s = _p1; };
			Fawdlstty::SMLiteBuilder<MyState, MyTrigger> _smb {};
			_smb.Configure (MyState::Rest)
				->WhenFunc (MyTrigger::Run, std::function<MyState (MyState, MyTrigger)> (
					[&] (MyState _state, MyTrigger _trigger) { s = "WhenFunc_Run"; return MyState::Ready; }))
				->WhenFunc (MyTrigger::Read, std::function<MyState (MyState, MyTrigger, std::string)> (
					[&] (MyState _state, MyTrigger _trigger, std::string _p1) { s = _p1; return MyState::Ready; }))
				->WhenFunc (MyTrigger::FinishRead, std::function<MyState (MyState, MyTrigger, std::string, int)> (
					[&] (MyState _state, MyTrigger _trigger, std::string _p1, int _p2) { s = _append (_p1, _p2); return MyState::Ready; }))
				->WhenAction (MyTrigger::Close, std::function<void (MyState, MyTrigger)> (
					[&] (MyState _state, MyTrigger _trigger) { s = "WhenAction_Close"; }))
				->WhenAction (MyTrigger::Write, std::function<void (MyState, MyTrigger, std::string)> (
					[&] (MyState _state, MyTrigger _trigger, std::string _p1) { s = _p1; }))
				->WhenAction (MyTrigger::FinishWrite, std::function<void (MyState, MyTrigger, std::string, int)> (
					[&] (MyState _state, MyTrigger _trigger, std::string _p1, int _p2) { s = _append (_p1, _p2); }));
			_smb.Configure (MyState::Ready)
				->WhenChangeTo (MyTrigger::Close, MyState::Rest);

			auto _sm = _smb.Build (MyState::Rest);
			Assert::AreEqual (_sm->GetState (), MyState::Rest);
			Assert::AreEqual (s, std::string (""));

			_sm->Triggering (MyTrigger::Run);
			Assert::AreEqual (_sm->GetState (), MyState::Ready);
			Assert::AreEqual (s, std::string ("WhenFunc_Run"));
			_sm->Triggering (MyTrigger::Close);

			_sm->Triggering (MyTrigger::Read, std::string ("hello"));
			Assert::AreEqual (_sm->GetState (), MyState::Ready);
			Assert::AreEqual (s, std::string ("hello"));
			_sm->Triggering (MyTrigger::Close);

			_sm->Triggering (MyTrigger::FinishRead, std::string ("hello"), 1);
			Assert::AreEqual (_sm->GetState (), MyState::Ready);
			Assert::AreEqual (s, std::string ("hello1"));
			_sm->Triggering (MyTrigger::Close);

			_sm->Triggering (MyTrigger::Close);
			Assert::AreEqual (s, std::string ("WhenAction_Close"));
			Assert::AreEqual (_sm->GetState (), MyState::Rest);

			_sm->Triggering (MyTrigger::Write, std::string ("world"));
			Assert::AreEqual (s, std::string ("world"));
			Assert::AreEqual (_sm->GetState (), MyState::Rest);

			_sm->Triggering (MyTrigger::FinishWrite, std::string ("world"), 1);
			Assert::AreEqual (s, std::string ("world1"));
			Assert::AreEqual (_sm->GetState (), MyState::Rest);
		}
	};
}
