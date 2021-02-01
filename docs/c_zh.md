# C库用户手册

Step 1. 下载仓库，将仓库中的`src_c/libsmlite/`项目拷贝进自己的解决方案项目，并引用

```c
#include "libsmlite.h"
#pragma comment (lib, "libsmlite.lib") // 或者gcc里通过 '-lsmlite' 链接
```

Step 2. 定义两个枚举类，分别代表所有的状态与所有的触发器

```c
enum MyState { MyState_Rest, MyState_Ready, MyState_Reading, MyState_Writing };
enum MyTrigger { MyTrigger_Run, MyTrigger_Close, MyTrigger_Read, MyTrigger_FinishRead, MyTrigger_Write, MyTrigger_FinishWrite };
```

Step 3. 定义状态机生成器

```c
psmlite_builder_t _smb = smlite_builder_create ();
```

Step 4. 定义状态机的规则，指定具体的某个状态允许触发什么事件

```c
{
    // 如果当状态机的当前状态是 MyState_Rest
    psmlite_configstate_t _state = smlite_builder_configure (_smb, MyState_Rest);

    // 如果状态由其他状态变成 MyState_Rest 状态，那么触发此方法，初始化状态机时指定的初始值不触发此方法
    // void _rest_entry () {
    //     printf ("entry Rest\n");
    // }
    smlite_configstate_on_entry (_state, _rest_entry);

    // 如果状态由 MyState_Rest 状态变成其他状态，那么触发此方法
    // void _rest_leave () {
    //     printf ("leave Rest\n");
    // }
    smlite_configstate_on_leave (_state, _rest_leave);

    // 如果触发 MyTrigger_Run，则将状态改为 MyState_Ready
    smlite_configstate_when_change_to (_state, MyTrigger_Run, MyState_Ready);

    // 如果触发 MyTrigger_Close，忽略
    smlite_configstate_when_ignore (_state, MyTrigger_Close);

    // 如果触发 MyTrigger_Read，则调用回调函数，并将状态调整为返回值
    // int32_t _rest_read (int32_t _state, int32_t _trigger) {
    //     printf ("call _rest_read ()\n");
    //     return (int32_t) MyState_Ready;
    // }
    smlite_configstate_when_func (_state, MyTrigger_Read, (whenfunc_t) _rest_read);

    // 如果触发 MyTrigger_FinishRead，则调用回调函数，并将状态调整为返回值
    // 需注意，触发时候需传入参数，数量与类型必须完全匹配，否则段错误
    // int32_t _rest_finishread (int32_t _state, int32_t _trigger, const char *_param) {
    //     printf ("call _rest_finishread () with param [%s]\n", _param);
    //     return (int32_t) MyState_Ready;
    // }
    smlite_configstate_when_func (_state, MyTrigger_FinishRead, (whenfunc_t) _rest_finishread);

    // 如果触发 MyTrigger_Write，则调用回调函数（触发此方法回调不调整返回值）
    // void _rest_write (int32_t _state, int32_t _trigger) {
    //     printf ("call _rest_write ()\n");
    // }
    smlite_configstate_when_action (_state, MyTrigger_Write, (whenaction_t) _rest_write);

    // 如果触发 MyTrigger_FinishWrite，则调用回调函数（触发此方法回调不调整返回值）
    // 需注意，触发时候需传入参数，数量与类型必须完全匹配，否则段错误
    // int32_t _rest_finishwrite (int32_t _state, int32_t _trigger, const char *_param) {
    //     printf ("call _rest_finishwrite () with param [%s]\n", _param);
    // }
    smlite_configstate_when_func (_state, MyTrigger_FinishWrite, (whenfunc_t) _rest_finishwrite);
}
```

同一个状态下，如果遇到同样的触发器，最多只允许定义一种处理方式，上面代码对定义的触发事件有详细解释。如果不定义触发事件但遇到触发，那么打印错误信息并忽略。

Step 5. 下面开始真正使用到状态机

```c
// 生成状态机
psmlite_t _sm = smlite_builder_build (_smb, MyState_Rest);

// 获取当前状态
MyState _state = (MyState) smlite_get_state (_sm);

// 判断是否允许触发某一个事件（1允许0不允许）
int _allow = smlite_allow_triggering (_sm, MyTrigger_Run);

// 触发一个事件
smlite_triggering (_sm, MyTrigger_Run);

// 触发一个事件，并传入指定参数
smlite_triggering (_sm, MyTrigger_Run, (const char *) "hello");

// 强行修改当前状态，此操作将不会触发OnEntry、OnLeave事件
smlite_set_state (_sm, MyState_Ready);
```
