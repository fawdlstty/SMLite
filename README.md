# SMLite

English | [简体中文](./README.zh.md)

Suitable for `C++` (C++17) & `C#` (.NET5) easy to use state machine library

A state machine is an object used to maintain state.For example, I assume a half-duplex network state machine that cannot write when reading data and cannot read when writing data. This state machine has four states: not running, on standby, reading, and writing. Then, let's assume that there are six kinds of events, start, close, write, read, write complete, read complete. OK, let's use if to define the rule to see what methods can be triggered by each state:

<details><summary><strong>Click to expand the C++ code</strong></summary>
<p>

```cpp
enum class MyState { Rest, Ready, Reading, Writing };
enum class MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };

// ...

if (_state == MyState::Rest) {
    if (_trigger == MyTrigger::Run) {
        _state = MyState::Ready;
    } else if (_trigger == MyTrigger::Close) {
        //
    } else {
        throw std::exception ();
    }
} else if (_state == MyState::Ready) {
    if (_trigger == MyTrigger::Read) {
        _state = MyState::Reading;
    } else if (_trigger == MyTrigger::Write) {
        _state = MyState::Writing;
    } else if (_trigger == MyTrigger::Close) {
        _state = MyState::Rest;
    } else {
        throw std::exception ();
    }
} else if (_state == MyState::Reading) {
    if (_trigger == MyTrigger::FinishRead) {
        _state = MyState::Ready;
    } else if (_trigger == MyTrigger::Close) {
        _state = MyState::Rest;
    } else {
        throw std::exception ();
    }
} else if (_state == MyState::Writing) {
    if (_trigger == MyTrigger::FinishWrite) {
        _state = MyState::Ready;
    } else if (_trigger == MyTrigger::Close) {
        _state = MyState::Rest;
    } else {
        throw std::exception ();
    }
}
```

</p>
</details>

<details><summary><strong>Click to expand the C# code</strong></summary>
<p>

```csharp
enum MyState { Rest, Ready, Reading, Writing };
enum MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };

// ...

if (_state == MyState.Rest) {
    if (_trigger == MyTrigger.Run) {
        _state = MyState.Ready;
    } else if (_trigger == MyTrigger.Close) {
        //
    } else {
        throw new Exception ();
    }
} else if (_state == MyState.Ready) {
    if (_trigger == MyTrigger.Read) {
        _state = MyState.Reading;
    } else if (_trigger == MyTrigger.Write) {
        _state = MyState.Writing;
    } else if (_trigger == MyTrigger.Close) {
        _state = MyState.Rest;
    } else {
        throw new Exception ();
    }
} else if (_state == MyState.Reading) {
    if (_trigger == MyTrigger.FinishRead) {
        _state = MyState.Ready;
    } else if (_trigger == MyTrigger.Close) {
        _state = MyState.Rest;
    } else {
        throw new Exception ();
    }
} else if (_state == MyState.Writing) {
    if (_trigger == MyTrigger.FinishWrite) {
        _state = MyState.Ready;
    } else if (_trigger == MyTrigger.Close) {
        _state = MyState.Rest;
    } else {
        throw new Exception ();
    }
}
```

</p>
</details>

There are not many states and events, only ten in total, I haven't started to write the implementation, just the rule code, it looks a bit cluttered, difficult to maintain, easy to cause problems. All right, now let's think about implementing this using a state machine.

```cpp
// C++
enum class MyState { Rest, Ready, Reading, Writing };
enum class MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };

// ...

SMLite::SMLite<MyState, MyTrigger> _sm (MyState::Rest);
_sm.Configure (MyState::Rest)
    ->WhenChangeTo (MyTrigger::Run, MyState::Ready)
    ->WhenIgnore (MyTrigger::Close);
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
```

```csharp
// C#
using Fawdlstty.SMLite;

enum MyState { Rest, Ready, Reading, Writing };
enum MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };

// ...

var _sm = new SMLite<MyState, MyTrigger> (MyState.Rest);
_sm.Configure (MyState.Rest)
    .WhenChangeTo (MyTrigger.Run, MyState.Ready)
    .WhenIgnore (MyTrigger.Close);
_sm.Configure (MyState.Ready)
    .WhenChangeTo (MyTrigger.Read, MyState.Reading)
    .WhenChangeTo (MyTrigger.Write, MyState.Writing)
    .WhenChangeTo (MyTrigger.Close, MyState.Rest);
_sm.Configure (MyState.Reading)
    .WhenChangeTo (MyTrigger.FinishRead, MyState.Ready)
    .WhenChangeTo (MyTrigger.Close, MyState.Rest);
_sm.Configure (MyState.Writing)
    .WhenChangeTo (MyTrigger.FinishWrite, MyState.Ready)
    .WhenChangeTo (MyTrigger.Close, MyState.Rest);
```

The number of rule lines decreased by two-thirds. Let me explain what this code means in more detail. The _sm variable defines this line, which means defining a state machine and specifying the initialization state of the current state machine.

The line that follows the Configure function means Specify a state, and then configure that state, what events are allowed to be fired from that state, and what actions are taken. WhenChangeTo means that when an event is triggered, the state is changed to the latter. Let's explain this code in detail:

- When the state is`MyState::Rest`, change the state to `MyState::Ready` if the `MyTrigger::Run` is encountered
- When the state is`MyState::Rest`, ignore it if the `MyTrigger::Close` is encountered
- When the state is`MyState::Ready`, change the state to `MyState::Reading` if the `MyTrigger::Read` is encountered
- When the state is`MyState::Ready`, change the state to `MyState::Writing` if the `MyTrigger::Write` is encountered
- When the state is`MyState::Ready`, change the state to `MyState::Rest` if the `MyTrigger::Close` is encountered
- ...

All right, so a simple state machine is done.Of course, simply changing the event may not satisfy our needs, we want to encounter a certain state, call a callback function, and then through our own code to handle, decide to change the state to a certain value, or let a certain state, allow an event to fire, but do nothing, etc...

SMLite now provides 6 synchronous methods, which are:

- `OnEntry`: Trigger when entering a state
- `OnLeave`: Trigger when leaving a state
- `WhenFunc`: When an trigger is encountered, the callback function closure is invoked to determine the value of the state through the callback function
- `WhenAction`: When an event is encountered, the callback function closure is invoked
- `WhenChangeTo`: When an event is encountered, changes the state machine to the specified state
- `WhenIgnore`: When an event is encountered, ignore it

The C# library provides 4 more asynchronous methods:

- `OnEntryAsync`: Asynchronous trigger when entering a state
- `OnLeaveAsync`: Asynchronous trigger when leaving a state
- `WhenFuncAsync`: When an event is encountered, the Asynchronous callback function closure is invoked, and the state value is determined by the callback function
- `WhenActionAsync`: When an event is encountered, the Asynchronous callback function closure is invoked

Okay, now that we've defined the states and the rules, let's look at how to use them.First, since we specified the initial state when we defined the state machine, the state of the state machine is the initial value:

```cpp
// C++
assert (_sm.GetState () == MyState::Rest);
```

```csharp
// C#
assert (_sm.State == MyState.Rest);
```

Since we define two triggers for this state, this state accepts only those triggers:

```cpp
// C++
assert (_sm.AllowTriggering (MyTrigger::Run));
assert (_sm.AllowTriggering (MyTrigger::Close));
assert (!_sm.AllowTriggering (MyTrigger::Read));
assert (!_sm.AllowTriggering (MyTrigger::FinishRead));
assert (!_sm.AllowTriggering (MyTrigger::Write));
assert (!_sm.AllowTriggering (MyTrigger::FinishWrite));
```

```csharp
// C#
assert (_sm.AllowTriggering (MyTrigger.Run));
assert (_sm.AllowTriggering (MyTrigger.Close));
assert (!_sm.AllowTriggering (MyTrigger.Read));
assert (!_sm.AllowTriggering (MyTrigger.FinishRead));
assert (!_sm.AllowTriggering (MyTrigger.Write));
assert (!_sm.AllowTriggering (MyTrigger.FinishWrite));
```

Now let's complete an event firing. According to the logic, the state will change to the state we specified:

```cpp
// C++
_sm.Triggering (MyTrigger::Run);
assert (_sm.GetState () == MyState::Ready);
```

```csharp
// C#
_sm.Triggering (MyTrigger.Run);
// Or trigger events asynchronously
//await _sm.TriggeringAsync (MyTrigger.Run);

assert (_sm.State == MyState.Ready);
```

C# Asynchronous Exposure:

Synchronous and asynchronous code can be mixed in the same state machine, such as triggering values with asynchronous triggering events that allow synchronous calls.Has a start value for the triggered event synchronously and also allows asynchronous invocation.Be aware, however, that the business layer code, especially synchronous method calls, will execute the Task.wait () method in the asynchronous code, which is likely to cause a deadlock.Unified synchronous or unified asynchronous mode is recommended.
