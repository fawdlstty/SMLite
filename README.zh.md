# SMLite

[English](./README.md) | 简体中文

适用于 `C++` (C++17)、`C#` (.NET5) 的简单易用的状态机库

## 使用SMLite

### C++

Step 1. 下载仓库，将仓库中的`SMLite/smlite.hpp`文件拷贝进自己的解决方案项目，并

```cpp
#include "smlite.hpp"
```

Step 2. 定义两个强枚举类，分别代表所有的状态与所有的触发器

```cpp
enum class MyState { Rest, Ready, Reading, Writing };
enum class MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };
```

Step 3. 定义状态机变量，模板为两个强枚举类，参数为初始值

```cpp
SMLite::SMLite<MyState, MyTrigger> _sm (MyState::Rest);
```

Step 4. 定义状态机的规则，指定具体的某个状态允许触发什么事件

```cpp
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
```

同一个状态下，如果遇到同样的触发器，最多只允许定义一种处理方式，上面代码对定义的触发事件有详细解释。如果不定义触发事件但遇到触发，那么抛异常。

Step 5. 下面开始真正使用到状态机

```cpp
// 获取当前状态
assert (_sm.GetState () == MyState::Rest);

// 判断是否允许触发某一个事件
_sm.AllowTriggering (MyTrigger::Run);

// 触发一个事件
_sm.Triggering (MyTrigger::Run);

// 触发一个事件，并传入指定参数
_sm.Triggering (MyTrigger::Run, std::string ("hello"));
```

### C#

Step 1. 下载仓库，拷贝出Fawdlstty.SMLite项目进自己的解决方案，并添加项目引用（注意，当前仅支持.NET5，暂不支持其他框架版本）

Step 2. 定义两个枚举类，分别代表所有的状态与所有的触发器

```csharp
enum MyState { Rest, Ready, Reading, Writing };
enum MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };
```

Step 3. 定义状态机变量，模板为两个枚举类，参数为初始值

```csharp
var _sm = new SMLite<MyState, MyTrigger> (MyState.Rest);
```

Step 4. 定义状态机的规则，指定具体的某个状态允许触发什么事件

```csharp
_sm.Configure (MyState.Rest)
    // 如果状态由其他状态变成 MyState.Rest 状态，那么触发此方法，初始化状态机时指定的初始值不触发此方法
    .OnEntry (() => Console.WriteLine ("entry Rest"))
    // 如果状态由 MyState.Rest 状态变成其他状态，那么触发此方法
    .OnLeave (() => Console.WriteLine ("leave Rest"))
    // 如果触发`MyTrigger.Run`，则将状态改为`MyState.Ready`
    .WhenChangeTo (MyTrigger.Run, MyState.Ready)
    // 如果触发`MyTrigger.Run`，忽略
    .WhenIgnore (MyTrigger.Close)
    // 如果触发`MyTrigger.Read`，则调用回调函数，并将状态调整为返回值
    .WhenFunc (MyTrigger.Read, (MyState _state, MyTrigger _trigger) => {
        Console.WriteLine ("call WhenFunc callback");
        return MyState.Ready;
    })
    // 如果触发`MyTrigger.FinishRead`，则调用回调函数，并将状态调整为返回值
    // 需注意，触发时候需传入参数，数量与类型必须完全匹配，否则抛异常
    .WhenFunc (MyTrigger.FinishRead, (MyState _state, MyTrigger _trigger, string _param) => {
        Console.WriteLine ($"call WhenFunc callback with param [{_param}]");
        return MyState.Ready;
    })
    // 如果触发`MyTrigger.Read`，则调用回调函数（触发此方法回调不调整返回值）
    .WhenAction (MyTrigger.Read, (MyState _state, MyTrigger _trigger) => {
        Console.WriteLine ("call WhenAction callback");
    })
    // 如果触发`MyTrigger.FinishRead`，则调用回调函数（触发此方法回调不调整返回值）
    // 需注意，触发时候需传入参数，数量与类型必须完全匹配，否则抛异常
    .WhenAction (MyTrigger.FinishRead, (MyState _state, MyTrigger _trigger, string _param) => {
        Console.WriteLine ($"call WhenAction callback with param [{_param}]");
    });
```

同一个状态下，如果遇到同样的触发器，最多只允许定义一种处理方式，上面代码对定义的触发事件有详细解释。如果不定义触发事件但遇到触发，那么抛异常。

Step 5. 下面开始真正使用到状态机

```cpp
// 获取当前状态
assert (_sm.State == MyState.Rest);

// 判断是否允许触发某一个事件
_sm.AllowTriggering (MyTrigger.Run);

// 触发一个事件
_sm.Triggering (MyTrigger.Run);

// 触发一个事件，并传入指定参数
_sm.Triggering (MyTrigger.Run, "hello");
```

Step 6. 如果用到异步

// TODO

## 开源协议

MIT License
