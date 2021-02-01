# VB.Net Library Tutorials

Step 1. Download the `Fawdlstty.SMLite` library via NuGet.

Step 2. Define two enumerated classes that represent all states and all triggers

```vb
Enum MyState
    Rest
    Ready
    Reading
    Writing
End Enum
Enum MyTrigger
    Run
    Close
    Read
    FinishRead
    Write
    FinishWrite
End Enum
```

Step 3. Define state machine builder, templates as two enumeration classes, and parameters as initial values

```vb
Dim _smb As SMLiteBuilder(Of MyState, MyTrigger) = New SMLiteBuilder(Of MyState, MyTrigger)()
```

Step 4. Rules that define a state machine, specifying what triggers are allowed to be fired for a specific state

```vb
With _smb.Configure(MyState.Rest)
    ' This method is fired if the state changes from another state to myState.rest state, not by the initial value specified when the state machine is initialized
    .OnEntry(Sub()
                 Console.WriteLine ("entry Rest")
             End Sub)

    ' This method is fired if the state changes from the myState.rest state to another state
    .OnLeave(Sub()
                 Console.WriteLine ("leave Rest")
             End Sub)

    ' If myTrigger.Run is triggered, change the status to MyState.ready
    .WhenChangeTo (MyTrigger.Run, MyState.Ready)

    ' If MyTrigger.Run is triggered, ignore it
    .WhenIgnore (MyTrigger.Close)

    ' If MyTrigger.read is triggered, the callback function is called and the state is adjusted to the return value
    .WhenFunc(MyTrigger.Read, Function(_state As MyState, _trigger As MyTrigger) As MyState
                                  Console.WriteLine ("call WhenFunc callback")
                                  Return MyState.Ready
                              End Function)

    ' If MyTrigger.FinishRead is fired, the callback function is called and the state is adjusted to the return value
    ' Note that an argument must be passed when firing, and the number and type must match exactly, otherwise an exception is thrown
    .WhenFunc(MyTrigger.FinishRead, Function(_state As MyState, _trigger As MyTrigger, _param_ As String) As MyState
                                        Console.WriteLine ($"call WhenFunc callback with param [{_param}]")
                                        Return MyState.Ready
                                    End Function)

    ' If MyTrigger.Write is fired, the callback function is called (triggering this method callback does not adjust the return value)
    .WhenAction(MyTrigger.Write, Sub(_state As MyState, _trigger As MyTrigger)
                                     Console.WriteLine ("call WhenAction callback")
                                 End Sub)

    ' If MyTrigger.FinishWrite is fired, the callback function is called (triggering this method callback does not adjust the return value)
    ' Note that an argument must be passed when firing, and the number and type must match exactly, otherwise an exception is thrown
    .WhenAction(MyTrigger.FinishWrite, Sub(_state As MyState, _trigger As MyTrigger, _param As String)
                                           Console.WriteLine ($"call WhenAction callback with param [{_param}]")
                                       End Sub)
End With
```

If you encounter the same trigger in the same state, you are allowed to define at most one way to handle it. The code above explains the defined trigger in detail.If a trigger is not defined but is encountered, the exception is thrown.

Step 5. Now let's get to the actual use of the state machine

```vb
' Build state machine
Dim _sm = _smb.Build(MyState.Rest)

' Get current status
Dim _state = _sm.State

' Determine whether an trigger is allowed to fire
Dim _allow = _sm.AllowTriggering(MyTrigger.Close)

' Fire an triggered
_sm.Triggering (MyTrigger.Run)

' Fires an trigger and passes in the specified parameters
_sm.Triggering (MyTrigger.Run, "hello")

' Forced to modify the current state, this code will not trigger OnEntry and OnLeave methods
_sm.State = MyState.Rest
```

Step 6. If you use asynchrony

Much like the above, the following is to specify the asynchronous trigger callback function

```vb
With _smb.Configure(MyState.Rest)

    ' Same effect as onEntry, except this function specifies an asynchronous method and cannot be called at the same time as OnEntry
    .OnEntryAsync(Async Function() As Task
                            Await Task.Yield()
                            Console.WriteLine ("entry Ready")
                        End Function)

    ' This function specifies an asynchronous method and cannot be called at the same time as OnLeave
    .OnLeaveAsync(Async Function() As Task
                            Await Task.Yield()
                            Console.WriteLine ("leave Ready")
                        End Function)

    ' The effect is identical to WhenFunc, but this function specifies an asynchronous method
    .WhenFuncAsync(MyTrigger.Read, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken) As Task(Of MyState)
                                            Await Task.Yield()
                                            Console.WriteLine ("call WhenFunc callback")
                                            Return MyState.Ready
                                        End Function)

    ' The effect is identical to WhenFunc, but this function specifies an asynchronous method
    .WhenFuncAsync(MyTrigger.FinishRead, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken, _param As String) As Task(Of MyState)
                                             Await Task.Yield()
                                             Console.WriteLine ($"call WhenFunc callback with param [{_param}]")
                                             Return MyState.Ready
                                         End Function)

    ' The effect is identical to WhenAction, but this function specifies an asynchronous method
    .WhenActionAsync(MyTrigger.Write, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken) As Task
                                                Await Task.Yield()
                                                Console.WriteLine ("call WhenAction callback")
                                            End Function)

    ' The effect is identical to WhenAction, but this function specifies an asynchronous method
    .WhenActionAsync(MyTrigger.FinishWrite, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken, _p1 As String) As Task
                                                      Await Task.Yield()
                                                      Console.WriteLine ($"call WhenAction callback with param [{_param}]")
                                                  End Function)
End With
```

Then there is the firing of an event:

```vb
' An event is fired asynchronously, passing in the specified parameters
Await _sm.TriggeringAsync (MyTrigger.Run, "hello")

' Limit the maximum execution time of an asynchronous task, timeout to cancel
Dim _source = new CancellationTokenSource (TimeSpan.FromSeconds (10))
Await _sm.TriggeringAsync (MyTrigger.Run, _source.Token, "hello")
```

Await asynchronously fired events will be returned after all functions have finished executing.In addition, it is important to note that synchronous and asynchronous should not be used together. If not used properly, it will easily lead to deadlock. The best practice is to use uniform synchronous or uniform asynchronous.
