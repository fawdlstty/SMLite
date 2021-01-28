# C# Library Tutorials

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

    // If MyTrigger.Write is fired, the callback function is called (triggering this method callback does not adjust the return value)
    .WhenAction (MyTrigger.Write, (MyState _state, MyTrigger _trigger) => {
        Console.WriteLine ("call WhenAction callback");
    })

    // If MyTrigger.FinishWrite is fired, the callback function is called (triggering this method callback does not adjust the return value)
    // Note that an argument must be passed when firing, and the number and type must match exactly, otherwise an exception is thrown
    .WhenAction (MyTrigger.FinishWrite, (MyState _state, MyTrigger _trigger, string _param) => {
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
    .WhenFuncAsync (MyTrigger.Read, async (MyState _state, MyTrigger _trigger, CancellationToken _token) => {
        await Task.Yield ();
        Console.WriteLine ("call WhenFunc callback");
        return MyState.Ready;
    })

    // The effect is identical to WhenFunc, but this function specifies an asynchronous method
    .WhenFuncAsync (MyTrigger.FinishRead, async (MyState _state, MyTrigger _trigger, CancellationToken _token, string _param) => {
        await Task.Yield ();
        Console.WriteLine ($"call WhenFunc callback with param [{_param}]");
        return MyState.Ready;
    })

    // The effect is identical to WhenAction, but this function specifies an asynchronous method
    .WhenActionAsync (MyTrigger.Read, async (MyState _state, MyTrigger _trigger, CancellationToken _token) => {
        await Task.Yield ();
        Console.WriteLine ("call WhenAction callback");
    })

    // The effect is identical to WhenAction, but this function specifies an asynchronous method
    .WhenActionAsync (MyTrigger.FinishRead, async (MyState _state, MyTrigger _trigger, CancellationToken _token, string _param) => {
        await Task.Yield ();
        Console.WriteLine ($"call WhenAction callback with param [{_param}]");
    });
```

Then there is the firing of an event:

```csharp
// An event is fired asynchronously, passing in the specified parameters
await _sm.TriggeringAsync (MyTrigger.Run, "hello");

// Limit the maximum execution time of an asynchronous task, timeout to cancel
var _source = new CancellationTokenSource (TimeSpan.FromSeconds (10));
await _sm.TriggeringAsync (MyTrigger.Run, _source.Token, "hello");
```

Await asynchronously fired events will be returned after all functions have finished executing.In addition, it is important to note that synchronous and asynchronous should not be used together. If not used properly, it will easily lead to deadlock. The best practice is to use uniform synchronous or uniform asynchronous.
