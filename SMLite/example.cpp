#include <cassert>
#include <functional>
#include <iostream>
#include <string>
#include "smlite.hpp"

enum class MyState { Rest, Ready, Reading, Writing };
enum class MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };

int main () {
	SMLite::SMLite<MyState, MyTrigger> _sm (MyState::Rest);
	// 如果当状态机的当前状态是 MyState::Rest
	_sm.Configure (MyState::Rest)
		// 如果状态由其他状态变成 MyState::Rest 状态，那么触发此方法，初始化状态机时指定的初始值不触发此方法
		->OnEntry ([] () { std::cout << "entry Rest\n"; })
		// 如果状态由 MyState::Rest 状态变成其他状态，那么触发此方法
		->OnLeave ([] () { std::cout << "leave Rest\n"; })
		// 如果触发`MyTrigger::Run`，则将状态改为`MyState::Ready`
		->WhenChangeTo (MyTrigger::Run, MyState::Ready)
		// 如果触发`MyTrigger::Run`，忽略
		->WhenIgnore (MyTrigger::Close)
		// 如果触发`MyTrigger::Read`，则调用回调函数，并将状态调整为返回值
		->WhenFunc (MyTrigger::Read, std::function ([] (MyState _state, MyTrigger _trigger) -> MyState {
		std::cout << "call WhenFunc callback\n";
			return MyState::Ready;
		}))
		// 如果触发`MyTrigger::FinishRead`，则调用回调函数，并将状态调整为返回值
		// 需注意，触发时候需传入参数，数量与类型必须完全匹配，否则抛异常
		->WhenFunc (MyTrigger::FinishRead, std::function ([] (MyState _state, MyTrigger _trigger, std::string _param) -> MyState {
			std::cout << "call WhenFunc callback with param [" << _param << "]\n";
			return MyState::Ready;
		}))
		// 如果触发`MyTrigger::Read`，则调用回调函数（触发此方法回调不调整返回值）
		->WhenAction (MyTrigger::Read, std::function ([] (MyState _state, MyTrigger _trigger) {
			std::cout << "call WhenAction callback\n";
		}))
		// 如果触发`MyTrigger::FinishRead`，则调用回调函数（触发此方法回调不调整返回值）
		// 需注意，触发时候需传入参数，数量与类型必须完全匹配，否则抛异常
		->WhenAction (MyTrigger::FinishRead, std::function ([] (MyState _state, MyTrigger _trigger, std::string _param) {
			std::cout << "call WhenAction callback with param [" << _param << "]\n";
		}));
	_sm.Configure (MyState::Ready)
		->WhenChangeTo (MyTrigger::Read, MyState::Reading)
		->WhenChangeTo (MyTrigger::Write, MyState::Writing)
		->WhenChangeTo (MyTrigger::Close, MyState::Rest);
	_sm.Configure (MyState::Reading)
		->WhenChangeTo (MyTrigger::FinishRead, MyState::Ready)
		->WhenChangeTo (MyTrigger::Close, MyState::Rest);
	_sm.Configure (MyState::Writing)
		->WhenChangeTo (MyTrigger::FinishWrite, MyState::Ready)
		->WhenChangeTo (MyTrigger::Close, MyState::Rest);

	assert (_sm.GetState () == MyState::Rest);

	_sm.Triggering (MyTrigger::Run, std::string ("hello"));
	assert (_sm.AllowTriggering (MyTrigger::Run));
	assert (_sm.AllowTriggering (MyTrigger::Read));
	assert (_sm.AllowTriggering (MyTrigger::FinishRead));
	assert (_sm.AllowTriggering (MyTrigger::Write));
	assert (_sm.AllowTriggering (MyTrigger::FinishWrite));
	assert (_sm.AllowTriggering (MyTrigger::Close));

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
