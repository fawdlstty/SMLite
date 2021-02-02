# Python Library Tutorials

Step 1. Download the repository, copy the `src_python/SMLite/` folder in the repository into your solution project, and reference it

```python
from SMLite import SMLite
from SMLiteBuilder import SMLiteBuilder
```

Step 2. Define two enum class that represent all states and all triggers

```python
from enum import IntEnum

class MyState (IntEnum):
    Rest = 0
    Ready = 1
    Reading = 2
    Writing = 3

class MyTrigger (IntEnum):
    Run = 0
    Close = 1
    Read = 2
    FinishRead = 3
    Write = 4
    FinishWrite = 5
```

Step 3. Define state machine builder with templates as two enum class and parameters as initial values

```python
_smb = SMLiteBuilder ()
```

Step 4. Rules that define a state machine, specifying what triggers are allowed to be fired for a specific state

```python
# Note: When actually used, delete the comment and blank line so that Python's line continuation character can work properly
# If the current state of the state machine is MyState.Rest
_smb.Configure (MyState.Rest)\

    # This method is fired if the state changes from another state to MyState.Rest state, not by the initial value specified when the state machine is initialized
    .OnEntry (lambda : print ("entry Rest"))\

    # This method is fired if the state changes from the MyState.Rest state to another state
    .OnLeave (lambda : print ("leave Rest"))\

    # If MyTrigger.Run is triggered, the state is changed to MyState.Ready
    .WhenChangeTo (MyTrigger.Run, MyState.Ready)\

    # If MyTrigger.Close is triggered, ignore it
    .WhenIgnore (MyTrigger.Close)\

    # If MyTrigger.Read is fired, the callback function is called and the state is adjusted to the return value
    # def _rest_read (_state, _trigger):
    #     print ("call WhenFunc callback")
    #     return MyState.Ready
    .WhenFunc (MyTrigger.Read, _rest_read)\

    # If MyTrigger.FinishRead is fired, the callback function is called and the state is adjusted to the return value
    # Note that an argument must be passed when firing, and the number and type must match exactly, otherwise an exception is thrown
    # def _rest_finishread (_state, _trigger, _param):
    #     print ("call WhenFunc callback with param [%s]", _param)
    #     return MyState.Ready
    .WhenFunc (MyTrigger.FinishRead, _rest_finishread)\

    # If MyTrigger.Write is fired, the callback function is called (triggering this method callback does not adjust the return value)
    # def _rest_write (_state, _trigger):
    #     print ("call WhenAction callback")
    .WhenAction (MyTrigger.Write, _rest_write)\

    # If MyTrigger.FinishWrite is fired, the callback function is called (triggering this method callback does not adjust the return value).
    # Note that an argument must be passed when firing, and the number and type must match exactly, otherwise an exception is thrown
    # def _rest_finishwrite (_state, _trigger, _param):
    #     print ("call WhenAction callback with param [%s]", _param)
    .WhenAction (MyTrigger.FinishWrite, _rest_finishwrite)
```

If you encounter the same trigger in the same state, you are allowed to define at most one way to handle it. The code above explains the defined trigger in detail.If a trigger is not defined but is encountered, the exception is thrown.

Step 5. Now let's get to the actual use of the state machine

```python
# Build state machine
_sm = _smb.Build (MyState.Rest)

# Get current status
Assert.AreEqual (_sm.GetState (), MyState.Rest)

# Determine whether an trigger is allowed to fire
Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Run))

# Fire an trigger
_sm.Triggering (MyTrigger.Run)

# Fires an trigger and passes in the specified parameters
_sm.Triggering (MyTrigger.Run, "hello")

# Forced to modify the current state, this code will not trigger OnEntry and OnLeave methods
_sm.SetState (MyState.Ready)
```

Step 6. If you use asynchrony

Much like the above, the following is to specify the asynchronous trigger callback function

```python
_smb.Configure (MyState.Ready)\

    # Same effect as onEntry, except this function specifies an asynchronous method and cannot be called at the same time as OnEntry
    # async def _ready_entry ():
    #     await asyncio.sleep (0.01)
    #     print ("entry Ready")
    .OnEntryAsync (_ready_entry)\

    # This function specifies an asynchronous method and cannot be called at the same time as OnLeave
    # async def _ready_leave ():
    #     await asyncio.sleep (0.01)
    #     print ("leave Ready")
   .OnLeaveAsync (_ready_leave)\

    # The effect is identical to WhenFunc, but this function specifies an asynchronous method
    # async def _ready_read (_state, _trigger):
    #     await asyncio.sleep (0.01)
    #     print ("call WhenFuncAsync callback")
    #     return MyState.Ready
    .WhenFuncAsync (MyTrigger.Read, _ready_read)\

    # The effect is identical to WhenFunc, but this function specifies an asynchronous method
    # async def _ready_finishread (_state, _trigger, _param):
    #     await asyncio.sleep (0.01)
    #     print ("call WhenFuncAsync callback with param %s", _param)
    #     return MyState.Ready
    .WhenFuncAsync (MyTrigger.FinishRead, _ready_finishread)\

    # The effect is identical to WhenAction, but this function specifies an asynchronous method
    # async def _ready_write (_state, _trigger):
    #     await asyncio.sleep (0.01)
    #     print ("call WhenActionAsync callback")
    .WhenActionAsync (MyTrigger.Write, _ready_write)\

    # The effect is identical to WhenAction, but this function specifies an asynchronous method
    # async def _ready_finishwrite (_state, _trigger, _param):
    #     await asyncio.sleep (0.01)
    #     print ("call WhenActionAsync callback with param %s", _param)
    .WhenActionAsync (MyTrigger.FinishWrite, _ready_finishwrite)\
```

Then there is the firing of an event:

```csharp
// An event is fired asynchronously, passing in the specified parameters
await _sm.TriggeringAsync (MyTrigger.Run, "hello")
```

Await asynchronously fired events will be returned after all functions have finished executing. In addition, it is important to note that synchronous and asynchronous should not be used together. If not used properly, it will easily lead to deadlock. The best practice is to use uniform synchronous or uniform asynchronous.
