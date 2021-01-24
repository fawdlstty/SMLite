# SMLite

[![license](https://img.shields.io/github/license/fawdlstty/SMLite?color=09f)](./LICENSE)
[![cpp](https://img.shields.io/lgtm/grade/cpp/github/fawdlstty/SMLite)](https://lgtm.com/projects/g/fawdlstty/SMLite)
[![csharp](https://img.shields.io/lgtm/grade/csharp/github/fawdlstty/SMLite)](https://lgtm.com/projects/g/fawdlstty/SMLite)
[![nuget](https://img.shields.io/nuget/dt/Fawdlstty.SMLite?label=nuget%20downloads)](https://www.nuget.org/packages/Fawdlstty.SMLite)

English | [简体中文](./README.zh.md)

Suitable for `C++` & `C#` and easy to use state machine library

[TODO](./TODO.md)

## Support Environments

<table><tr><td>

| C++ Version | Support |
| :---: | :---: |
| C++ 98 | × |
| C++ 03 | × |
| C++ 11 | √ |
| C++ 14 | √ |
| C++ 17 | √ |
| C++ 20 | √ |

</td><td>

| C# Runtime | Support |
| :---: | :---: |
| .NET Framework 3.5 | × |
| .NET Framework 4.0 | × |
| .NET Framework 4.5 | √ |
| .NET Standard 2.0 | √ |
| .NET Standard 2.1 | √ |
| .NET 5.0 | √ |

</td></tr></table>

## Tutorials

### C++

Step 1. Download the repository, copy the `SMLite/SMLite.hpp` file in the repository into your solution project, and include it

```cpp
#include "SMLite.hpp"
```

Step 2. Define two enum class that represent all states and all triggers

```cpp
enum class MyState { Rest, Ready, Reading, Writing };
enum class MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };
```

Step 3. Define state machine builder with templates as two enum class and parameters as initial values

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

    // If MyTrigger::Read is fired, the callback function is called (triggering this method callback does not adjust the return value)
    ->WhenAction (MyTrigger::Read, std::function ([] (MyState _state, MyTrigger _trigger) {
        std::cout << "call WhenAction callback\n";
    }))

    // If MyTrigger::FinishRead is fired, the callback function is called (triggering this method callback does not adjust the return value).
    // Note that an argument must be passed when firing, and the number and type must match exactly, otherwise an exception is thrown
    ->WhenAction (MyTrigger::FinishRead, std::function ([] (MyState _state, MyTrigger _trigger, std::string _param) {
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

### C\#

Step 1. Download the `Fawdlstty.SMLite` library via NuGet.

Step 2. Define two enumerated classes that represent all states and all triggers

```csharp
enum MyState { Rest, Ready, Reading, Writing };
enum MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };
```

Step 3. Define state machine builder, templates as two enumeration classes, and parameters as initial values

```csharp
var _smb = new SMLiteBuilder<MyState, MyTrigger> ();
```

Step 4. Rules that define a state machine, specifying what triggers are allowed to be fired for a specific state

```csharp
_smb.Configure (MyState.Rest)
    // This method is fired if the state changes from another state to myState.rest state, not by the initial value specified when the state machine is initialized
    .OnEntry (() => Console.WriteLine ("entry Rest"))

    // This method is fired if the state changes from the myState.rest state to another state
    .OnLeave (() => Console.WriteLine ("leave Rest"))

    // If myTrigger.Run is triggered, change the status to MyState.ready
    .WhenChangeTo (MyTrigger.Run, MyState.Ready)

    // If MyTrigger.Run is triggered, ignore it
    .WhenIgnore (MyTrigger.Close)

    // If MyTrigger.read is triggered, the callback function is called and the state is adjusted to the return value
    .WhenFunc (MyTrigger.Read, (MyState _state, MyTrigger _trigger) => {
        Console.WriteLine ("call WhenFunc callback");
        return MyState.Ready;
    })

    // If MyTrigger.FinishRead is fired, the callback function is called and the state is adjusted to the return value
    // Note that an argument must be passed when firing, and the number and type must match exactly, otherwise an exception is thrown
    .WhenFunc (MyTrigger.FinishRead, (MyState _state, MyTrigger _trigger, string _param) => {
        Console.WriteLine ($"call WhenFunc callback with param [{_param}]");
        return MyState.Ready;
    })

    // If MyTrigger.Read is fired, the callback function is called (triggering this method callback does not adjust the return value)
    .WhenAction (MyTrigger.Read, (MyState _state, MyTrigger _trigger) => {
        Console.WriteLine ("call WhenAction callback");
    })

    // If MyTrigger.FinishRead is fired, the callback function is called (triggering this method callback does not adjust the return value)
    // Note that an argument must be passed when firing, and the number and type must match exactly, otherwise an exception is thrown
    .WhenAction (MyTrigger.FinishRead, (MyState _state, MyTrigger _trigger, string _param) => {
        Console.WriteLine ($"call WhenAction callback with param [{_param}]");
    });
```

If you encounter the same trigger in the same state, you are allowed to define at most one way to handle it. The code above explains the defined trigger in detail.If a trigger is not defined but is encountered, the exception is thrown.

Step 5. Now let's get to the actual use of the state machine

```csharp
// Build state machine
var _sm = _smb.Build (MyState.Rest);

// Get current status
assert (_sm.State == MyState.Rest);

// Determine whether an trigger is allowed to fire
_sm.AllowTriggering (MyTrigger.Run);

// Fire an triggered
_sm.Triggering (MyTrigger.Run);

// Fires an trigger and passes in the specified parameters
_sm.Triggering (MyTrigger.Run, "hello");

// Forced to modify the current state, this code will not trigger OnEntry and OnLeave methods
_sm.State = MyState.Rest;
```

Step 6. If you use asynchrony

Much like the above, the following is to specify the asynchronous trigger callback function

```csharp
_smb.Configure (MyState.Ready)

    // Same effect as onEntry, except this function specifies an asynchronous method and cannot be called at the same time as OnEntry
    .OnEntryAsync (async () => {
        await Task.Yield ();
        Console.WriteLine ("entry Ready");
    })

    // This function specifies an asynchronous method and cannot be called at the same time as OnLeave
    .OnLeaveAsync (async () => {
        await Task.Yield ();
        Console.WriteLine ("leave Ready");
    })

    // The effect is identical to WhenFunc, but this function specifies an asynchronous method
    .WhenFuncAsync (MyTrigger.Read, async (MyState _state, MyTrigger _trigger) => {
        await Task.Yield ();
        Console.WriteLine ("call WhenFunc callback");
        return MyState.Ready;
    })

    // The effect is identical to WhenFunc, but this function specifies an asynchronous method
    .WhenFuncAsync (MyTrigger.FinishRead, async (MyState _state, MyTrigger _trigger, string _param) => {
        await Task.Yield ();
        Console.WriteLine ($"call WhenFunc callback with param [{_param}]");
        return MyState.Ready;
    })

    // The effect is identical to WhenAction, but this function specifies an asynchronous method
    .WhenActionAsync (MyTrigger.Read, async (MyState _state, MyTrigger _trigger) => {
        await Task.Yield ();
        Console.WriteLine ("call WhenAction callback");
    })

    // The effect is identical to WhenAction, but this function specifies an asynchronous method
    .WhenActionAsync (MyTrigger.FinishRead, async (MyState _state, MyTrigger _trigger, string _param) => {
        await Task.Yield ();
        Console.WriteLine ($"call WhenAction callback with param [{_param}]");
    });
```

Then there is the firing of an event:

```csharp
// An event is fired asynchronously, passing in the specified parameters
await _sm.TriggeringAsync (MyTrigger.Run, "hello");
```

Await asynchronously fired events will be returned after all functions have finished executing.In addition, it is important to note that synchronous and asynchronous should not be used together. If not used properly, it will easily lead to deadlock. The best practice is to use uniform synchronous or uniform asynchronous.
