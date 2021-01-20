#include <cassert>
#include <functional>
#include <iostream>
#include <string>
#include "smlite.hpp"

enum class MyState { Rest, Ready, Reading, Writing };
enum class MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };

int main () {
	SMLite::SMLite<MyState, MyTrigger> _sm (MyState::Rest);
	_sm.Configure (MyState::Rest)
		->OnEntry ([] () { std::cout << "entry Rest\n"; })
		->OnLeave ([] () { std::cout << "leave Rest\n"; })
		->WhenFunc (MyTrigger::Run, std::function ([] (MyState _state, MyTrigger _trigger, std::string _param) -> MyState {
			std::cout << "trigger with param [" << _param << "]\n";
			return MyState::Ready;
		}))
		->WhenIgnore (MyTrigger::Close);
	_sm.Configure (MyState::Ready)
		->OnEntry ([] () { std::cout << "entry Ready\n"; })
		->OnLeave ([] () { std::cout << "leave Ready\n"; })
		->WhenChangeTo (MyTrigger::Read, MyState::Reading)
		->WhenChangeTo (MyTrigger::Write, MyState::Writing)
		->WhenChangeTo (MyTrigger::Close, MyState::Rest);
	_sm.Configure (MyState::Reading)
		->OnEntry ([] () { std::cout << "entry Reading\n"; })
		->OnLeave ([] () { std::cout << "leave Reading\n"; })
		->WhenChangeTo (MyTrigger::FinishRead, MyState::Ready)
		->WhenChangeTo (MyTrigger::Close, MyState::Rest);
	_sm.Configure (MyState::Writing)
		->OnEntry ([] () { std::cout << "entry Writing\n"; })
		->OnLeave ([] () { std::cout << "leave Writing\n"; })
		->WhenChangeTo (MyTrigger::FinishWrite, MyState::Ready)
		->WhenChangeTo (MyTrigger::Close, MyState::Rest);

	assert (_sm.GetState () == MyState::Rest);
	assert (_sm.AllowTriggering (MyTrigger::Run));
	assert (!_sm.AllowTriggering (MyTrigger::Read));
	assert (!_sm.AllowTriggering (MyTrigger::FinishRead));
	assert (!_sm.AllowTriggering (MyTrigger::Write));
	assert (!_sm.AllowTriggering (MyTrigger::FinishWrite));
	assert (_sm.AllowTriggering (MyTrigger::Close));

	_sm.Triggering (MyTrigger::Run, std::string ("hello"));
	assert (_sm.GetState () == MyState::Ready);

	_sm.Triggering (MyTrigger::Read);
	assert (_sm.GetState () == MyState::Reading);

	_sm.Triggering (MyTrigger::FinishRead);
	assert (_sm.GetState () == MyState::Ready);

	_sm.Triggering (MyTrigger::Write);
	assert (_sm.GetState () == MyState::Writing);

	_sm.Triggering (MyTrigger::FinishWrite);
	assert (_sm.GetState () == MyState::Ready);

	_sm.Triggering (MyTrigger::Close);
	assert (_sm.GetState () == MyState::Rest);

	std::cout << "Hello World!\n";
	return 0;
}
