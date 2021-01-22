#include <cassert>
#include <functional>
#include <iostream>
#include <string>
#include "smlite.hpp"

enum class MyState { Rest, Ready, Reading, Writing };
enum class MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };

int main () {
	Fawdlstty::SMLiteBuilder<MyState, MyTrigger> _smb {};
	_smb.Configure (MyState::Rest)
		->OnEntry ([] () { std::cout << "entry Rest\n"; })
		->OnLeave ([] () { std::cout << "leave Rest\n"; })
		->WhenChangeTo (MyTrigger::Run, MyState::Ready)
		->WhenIgnore (MyTrigger::Close);
	_smb.Configure (MyState::Ready)
		->WhenChangeTo (MyTrigger::Read, MyState::Reading)
		->WhenChangeTo (MyTrigger::Write, MyState::Writing)
		->WhenChangeTo (MyTrigger::Close, MyState::Rest);
	_smb.Configure (MyState::Reading)
		->WhenChangeTo (MyTrigger::FinishRead, MyState::Ready)
		->WhenChangeTo (MyTrigger::Close, MyState::Rest);
	_smb.Configure (MyState::Writing)
		->WhenChangeTo (MyTrigger::FinishWrite, MyState::Ready)
		->WhenChangeTo (MyTrigger::Close, MyState::Rest);

	auto _sm = _smb.Build (MyState::Rest);
	assert (_sm->GetState () == MyState::Rest);

	assert (_sm->AllowTriggering (MyTrigger::Run));
	assert (!_sm->AllowTriggering (MyTrigger::Read));
	assert (!_sm->AllowTriggering (MyTrigger::FinishRead));
	assert (!_sm->AllowTriggering (MyTrigger::Write));
	assert (!_sm->AllowTriggering (MyTrigger::FinishWrite));
	assert (_sm->AllowTriggering (MyTrigger::Close));

	_sm->Triggering (MyTrigger::Run);
	assert (_sm->GetState () == MyState::Ready);

	_sm->Triggering (MyTrigger::Read);
	assert (_sm->GetState () == MyState::Reading);

	_sm->Triggering (MyTrigger::FinishRead);
	assert (_sm->GetState () == MyState::Ready);

	_sm->Triggering (MyTrigger::Write);
	assert (_sm->GetState () == MyState::Writing);

	_sm->Triggering (MyTrigger::FinishWrite);
	assert (_sm->GetState () == MyState::Ready);

	_sm->Triggering (MyTrigger::Close);
	assert (_sm->GetState () == MyState::Rest);

	std::cout << "Hello World!\n";
	return 0;
}
