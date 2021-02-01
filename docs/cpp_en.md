# C++ Library Tutorials

Step 1. Download the repository, copy the `src_cpp/SMLite/SMLite.hpp` file in the repository into your solution project, and include it

```cpp
#include "SMLite.hpp"
```

Step 2. Define two enum class that represent all states and all triggers

```cpp
enum class MyState { Rest, Ready, Reading, Writing };
enum class MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };
```

Step 3. Define state machine builder with templates as two enum class

```cpp
Fawdlstty::SMLiteBuilder<MyState, MyTrigger> _smb {};
```

Step 4. Rules that define a state machine, specifying what triggers are allowed to be fired for a specific state

```cpp
// If the current state of the state machine is MyState::Rest
_smb.Configure (MyState::Rest)

    // This method is fired if the state changes from another state to MyState::Rest state, not by the initial value specified when the state machine is initialized
    ->OnEntry ([] () { std::cout << "entry Rest\n"; })

    // This method is fired if the state changes from the MyState::Rest state to another state
    ->OnLeave ([] () { std::cout << "leave Rest\n"; })

    // If MyTrigger::Run is triggered, the state is changed to MyState::Ready
    ->WhenChangeTo (MyTrigger::Run, MyState::Ready)

    // If MyTrigger::Close is triggered, ignore it
    ->WhenIgnore (MyTrigger::Close)

    // If MyTrigger::Read is fired, the callback function is called and the state is adjusted to the return value
    ->WhenFunc (MyTrigger::Read, std::function ([] (MyState _state, MyTrigger _trigger) -> MyState {
    std::cout << "call WhenFunc callback\n";
        return MyState::Ready;
    }))

    // If MyTrigger::FinishRead is fired, the callback function is called and the state is adjusted to the return value
    // Note that an argument must be passed when firing, and the number and type must match exactly, otherwise an exception is thrown
    ->WhenFunc (MyTrigger::FinishRead, std::function ([] (MyState _state, MyTrigger _trigger, std::string _param) -> MyState {
        std::cout << "call WhenFunc callback with param [" << _param << "]\n";
        return MyState::Ready;
    }))

    // If MyTrigger::Write is fired, the callback function is called (triggering this method callback does not adjust the return value)
    ->WhenAction (MyTrigger::Write, std::function ([] (MyState _state, MyTrigger _trigger) {
        std::cout << "call WhenAction callback\n";
    }))

    // If MyTrigger::FinishWrite is fired, the callback function is called (triggering this method callback does not adjust the return value).
    // Note that an argument must be passed when firing, and the number and type must match exactly, otherwise an exception is thrown
    ->WhenAction (MyTrigger::FinishWrite, std::function ([] (MyState _state, MyTrigger _trigger, std::string _param) {
        std::cout << "call WhenAction callback with param [" << _param << "]\n";
    }));
```

If you encounter the same trigger in the same state, you are allowed to define at most one way to handle it. The code above explains the defined trigger in detail.If a trigger is not defined but is encountered, the exception is thrown.

Step 5. Now let's get to the actual use of the state machine

```cpp
// Build state machine
auto _sm = _smb.Build (MyState::Rest);

// Get current status
assert (_sm->GetState () == MyState::Rest);

// Determine whether an trigger is allowed to fire
_sm->AllowTriggering (MyTrigger::Run);

// Fire an trigger
_sm->Triggering (MyTrigger::Run);

// Fires an trigger and passes in the specified parameters
_sm->Triggering (MyTrigger::Run, std::string ("hello"));

// Forced to modify the current state, this code will not trigger OnEntry and OnLeave methods
_sm->SetState (MyState::Ready);
```
