# C Library Tutorials

Step 1. Download the repository, copy the `src_c/libsmlite/` project in the repository into your solution project, and include it

```c
#include "libsmlite.h"
#pragma comment (lib, "libsmlite.lib") // Or '-lsmlite' in GCC
```

Step 2. Define two enum class that represent all states and all triggers

```c
enum MyState { MyState_Rest, MyState_Ready, MyState_Reading, MyState_Writing };
enum MyTrigger { MyTrigger_Run, MyTrigger_Close, MyTrigger_Read, MyTrigger_FinishRead, MyTrigger_Write, MyTrigger_FinishWrite };
```

Step 3. Define state machine builder

```c
psmlite_builder_t _smb = smlite_builder_create ();
```

Step 4. Rules that define a state machine, specifying what triggers are allowed to be fired for a specific state

```c
{
    // If the current state of the state machine is MyState_Rest
    psmlite_configstate_t _state = smlite_builder_configure (_smb, MyState_Rest);

    // This method is fired if the state changes from another state to MyState_Rest state, not by the initial value specified when the state machine is initialized
    // void _rest_entry () {
    //     printf ("entry Rest\n");
    // }
    smlite_configstate_on_entry (_state, _rest_entry);

    // This method is fired if the state changes from the MyState_Rest state to another state
    // void _rest_leave () {
    //     printf ("leave Rest\n");
    // }
    smlite_configstate_on_leave (_state, _rest_leave);

    // If MyTrigger_Run is triggered, the state is changed to MyState_Ready
    smlite_configstate_when_change_to (_state, MyTrigger_Run, MyState_Ready);

    // If MyTrigger_Close is triggered, ignore it
    smlite_configstate_when_ignore (_state, MyTrigger_Close);

    // If MyTrigger_Read is fired, the callback function is called and the state is adjusted to the return value
    // int32_t _rest_read (int32_t _state, int32_t _trigger) {
    //     printf ("call _rest_read ()\n");
    //     return (int32_t) MyState_Ready;
    // }
    smlite_configstate_when_func (_state, MyTrigger_Read, (whenfunc_t) _rest_read);

    // If MyTrigger_FinishRead is fired, the callback function is called and the state is adjusted to the return value
    // Note that an argument must be passed when firing, and the number and type must match exactly, otherwise segment fault
    // int32_t _rest_finishread (int32_t _state, int32_t _trigger, const char *_param) {
    //     printf ("call _rest_finishread () with param [%s]\n", _param);
    //     return (int32_t) MyState_Ready;
    // }
    smlite_configstate_when_func (_state, MyTrigger_FinishRead, (whenfunc_t) _rest_finishread);

    // If MyTrigger_Write is fired, the callback function is called (triggering this method callback does not adjust the return value)
    // void _rest_write (int32_t _state, int32_t _trigger) {
    //     printf ("call _rest_write ()\n");
    // }
    smlite_configstate_when_action (_state, MyTrigger_Write, (whenaction_t) _rest_write);

    // If MyTrigger_FinishWrite is fired, the callback function is called (triggering this method callback does not adjust the return value).
    // Note that an argument must be passed when firing, and the number and type must match exactly, otherwise segment fault
    // int32_t _rest_finishwrite (int32_t _state, int32_t _trigger, const char *_param) {
    //     printf ("call _rest_finishwrite () with param [%s]\n", _param);
    // }
    smlite_configstate_when_func (_state, MyTrigger_FinishWrite, (whenfunc_t) _rest_finishwrite);
}
```

If you encounter the same trigger in the same state, you are allowed to define at most one way to handle it. The code above explains the defined trigger in detail.If a trigger is not defined but is encountered, print error message and ignore.

Step 5. Now let's get to the actual use of the state machine

```c
// Build state machine
psmlite_t _sm = smlite_builder_build (_smb, MyState_Rest);

// Get current status
MyState _state = (MyState) smlite_get_state (_sm);

// Determine whether an trigger is allowed to fire (1 allow 0 disallow)
int _allow = smlite_allow_triggering (_sm, MyTrigger_Run);

// Fire an trigger
smlite_triggering (_sm, MyTrigger_Run);

// Fires an trigger and passes in the specified parameters
smlite_triggering (_sm, MyTrigger_Run, (const char *) "hello");

// Forced to modify the current state, this code will not trigger OnEntry and OnLeave methods
smlite_set_state (_sm, MyState_Ready);
```
