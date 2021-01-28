# C++库用户手册

Step 1. 下载仓库，将仓库中的`src_cpp/SMLite/SMLite.hpp`文件拷贝进自己的解决方案项目，并引用

```cpp
#include "SMLite.hpp"
```

Step 2. 定义两个强枚举类，分别代表所有的状态与所有的触发器

```cpp
enum class MyState { Rest, Ready, Reading, Writing };
enum class MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };
```

Step 3. 定义状态机生成器，模板为两个强枚举类

```cpp
Fawdlstty::SMLiteBuilder<MyState, MyTrigger> _smb {};
```

Step 4. 定义状态机的规则，指定具体的某个状态允许触发什么事件

```cpp
// 如果当状态机的当前状态是 MyState::Rest
_smb.Configure (MyState::Rest)

    // 如果状态由其他状态变成 MyState::Rest 状态，那么触发此方法，初始化状态机时指定的初始值不触发此方法
    ->OnEntry ([] () { std::cout << "entry Rest\n"; })

    // 如果状态由 MyState::Rest 状态变成其他状态，那么触发此方法
    ->OnLeave ([] () { std::cout << "leave Rest\n"; })

    // 如果触发 MyTrigger::Run，则将状态改为 MyState::Ready
    ->WhenChangeTo (MyTrigger::Run, MyState::Ready)

    // 如果触发 MyTrigger::Close，忽略
    ->WhenIgnore (MyTrigger::Close)

    // 如果触发 MyTrigger::Read，则调用回调函数，并将状态调整为返回值
    ->WhenFunc (MyTrigger::Read, std::function ([] (MyState _state, MyTrigger _trigger) -> MyState {
    std::cout << "call WhenFunc callback\n";
        return MyState::Ready;
    }))

    // 如果触发 MyTrigger::FinishRead，则调用回调函数，并将状态调整为返回值
    // 需注意，触发时候需传入参数，数量与类型必须完全匹配，否则抛异常
    ->WhenFunc (MyTrigger::FinishRead, std::function ([] (MyState _state, MyTrigger _trigger, std::string _param) -> MyState {
        std::cout << "call WhenFunc callback with param [" << _param << "]\n";
        return MyState::Ready;
    }))

    // 如果触发 MyTrigger::Read，则调用回调函数（触发此方法回调不调整返回值）
    ->WhenAction (MyTrigger::Read, std::function ([] (MyState _state, MyTrigger _trigger) {
        std::cout << "call WhenAction callback\n";
    }))

    // 如果触发 MyTrigger::FinishRead，则调用回调函数（触发此方法回调不调整返回值）
    // 需注意，触发时候需传入参数，数量与类型必须完全匹配，否则抛异常
    ->WhenAction (MyTrigger::FinishRead, std::function ([] (MyState _state, MyTrigger _trigger, std::string _param) {
        std::cout << "call WhenAction callback with param [" << _param << "]\n";
    }));
```

同一个状态下，如果遇到同样的触发器，最多只允许定义一种处理方式，上面代码对定义的触发事件有详细解释。如果不定义触发事件但遇到触发，那么抛异常。

Step 5. 下面开始真正使用到状态机

```cpp
// 生成状态机
auto _sm = _smb.Build (MyState::Rest);

// 获取当前状态
assert (_sm->GetState () == MyState::Rest);

// 判断是否允许触发某一个事件
_sm->AllowTriggering (MyTrigger::Run);

// 触发一个事件
_sm->Triggering (MyTrigger::Run);

// 触发一个事件，并传入指定参数
_sm->Triggering (MyTrigger::Run, std::string ("hello"));

// 强行修改当前状态，此操作将不会触发OnEntry、OnLeave事件
_sm->SetState (MyState::Ready);
```
