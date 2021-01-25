# SMLite

[![license](https://img.shields.io/github/license/fawdlstty/SMLite?color=09f)](./LICENSE)
[![cpp](https://img.shields.io/lgtm/grade/cpp/github/fawdlstty/SMLite)](https://lgtm.com/projects/g/fawdlstty/SMLite)
[![csharp](https://img.shields.io/lgtm/grade/csharp/github/fawdlstty/SMLite)](https://lgtm.com/projects/g/fawdlstty/SMLite)
[![nuget](https://img.shields.io/nuget/dt/Fawdlstty.SMLite?label=nuget%20downloads)](https://www.nuget.org/packages/Fawdlstty.SMLite)
[![Total alerts](https://img.shields.io/lgtm/alerts/g/fawdlstty/SMLite.svg?logo=lgtm)](https://lgtm.com/projects/g/fawdlstty/SMLite/alerts/)
[![AppVeyor Build](https://img.shields.io/appveyor/build/fawdlstty/SMLite)](https://ci.appveyor.com/project/fawdlstty/SMLite)
[![Coverage Status](https://coveralls.io/repos/github/fawdlstty/SMLite/badge.svg)](https://coveralls.io/github/fawdlstty/SMLite)

[English](./README.md) | 简体中文

`C++`、`C#` 的状态机库

[TODO](./TODO.md)

## 支持的环境

<table><tr><td>

| C++ 版本 | 是否支持 |
| :---: | :---: |
| C++ 98 | × |
| C++ 03 | × |
| C++ 11 | √ |
| C++ 14 | √ |
| C++ 17 | √ |
| C++ 20 | √ |

</td><td>

| C# 运行时 | 是否支持 |
| :---: | :---: |
| .NET Framework 3.5 | × |
| .NET Framework 4.0 | × |
| .NET Framework 4.5 | √ |
| .NET Standard 2.0 | √ |
| .NET Standard 2.1 | √ |
| .NET 5.0 | √ |

</td></tr></table>

## 如何使用

### C++

Step 1. 下载仓库，将仓库中的`SMLite/SMLite.hpp`文件拷贝进自己的解决方案项目，并引用

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

### C\#

Step 1. 通过 NuGet 下载 `Fawdlstty.SMLite` 库

Step 2. 定义两个枚举类，分别代表所有的状态与所有的触发器

```csharp
enum MyState { Rest, Ready, Reading, Writing };
enum MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };
```

Step 3. 定义状态机变量，模板为两个枚举类，参数为初始值

```csharp
var _smb = new SMLiteBuilder<MyState, MyTrigger> ();
```

Step 4. 定义状态机的规则，指定具体的某个状态允许触发什么事件

```csharp
_smb.Configure (MyState.Rest)

    // 如果状态由其他状态变成 MyState.Rest 状态，那么触发此方法，初始化状态机时指定的初始值不触发此方法
    .OnEntry (() => Console.WriteLine ("entry Rest"))

    // 如果状态由 MyState.Rest 状态变成其他状态，那么触发此方法
    .OnLeave (() => Console.WriteLine ("leave Rest"))

    // 如果触发 MyTrigger.Run，则将状态改为 MyState.Ready
    .WhenChangeTo (MyTrigger.Run, MyState.Ready)

    // 如果触发 MyTrigger.Run，忽略
    .WhenIgnore (MyTrigger.Close)

    // 如果触发 MyTrigger.Read，则调用回调函数，并将状态调整为返回值
    .WhenFunc (MyTrigger.Read, (MyState _state, MyTrigger _trigger) => {
        Console.WriteLine ("call WhenFunc callback");
        return MyState.Ready;
    })

    // 如果触发 MyTrigger.FinishRead，则调用回调函数，并将状态调整为返回值
    // 需注意，触发时候需传入参数，数量与类型必须完全匹配，否则抛异常
    .WhenFunc (MyTrigger.FinishRead, (MyState _state, MyTrigger _trigger, string _param) => {
        Console.WriteLine ($"call WhenFunc callback with param [{_param}]");
        return MyState.Ready;
    })

    // 如果触发 MyTrigger.Read，则调用回调函数（触发此方法回调不调整返回值）
    .WhenAction (MyTrigger.Read, (MyState _state, MyTrigger _trigger) => {
        Console.WriteLine ("call WhenAction callback");
    })

    // 如果触发 MyTrigger.FinishRead，则调用回调函数（触发此方法回调不调整返回值）
    // 需注意，触发时候需传入参数，数量与类型必须完全匹配，否则抛异常
    .WhenAction (MyTrigger.FinishRead, (MyState _state, MyTrigger _trigger, string _param) => {
        Console.WriteLine ($"call WhenAction callback with param [{_param}]");
    });
```

同一个状态下，如果遇到同样的触发器，最多只允许定义一种处理方式，上面代码对定义的触发事件有详细解释。如果不定义触发事件但遇到触发，那么抛异常。

Step 5. 下面开始真正使用到状态机

```csharp
// 生成状态机
auto _sm = _smb.Build (MyState.Rest);

// 获取当前状态
assert (_sm.State == MyState.Rest);

// 判断是否允许触发某一个事件
_sm.AllowTriggering (MyTrigger.Run);

// 触发一个事件
_sm.Triggering (MyTrigger.Run);

// 触发一个事件，并传入指定参数
_sm.Triggering (MyTrigger.Run, "hello");

// 强行修改当前状态，此操作将不会触发OnEntry、OnLeave事件
_sm.State = MyState.Rest;
```

Step 6. 如果用到异步

使用与上面非常相似，下面是指定异步触发回调函数

```csharp
_smb.Configure (MyState.Ready)

    // 与 OnEntry 效果一致，不过这函数指定异步方法，并且不能与 OnEntry 同时调用
    .OnEntryAsync (async () => {
        await Task.Yield ();
        Console.WriteLine ("entry Ready");
    })

    // 与 OnLeave 效果一致，不过这函数指定异步方法，并且不能与 OnLeave 同时调用
    .OnLeaveAsync (async () => {
        await Task.Yield ();
        Console.WriteLine ("leave Ready");
    })

    // 效果与 WhenFunc 一致，不过这函数指定异步方法
    .WhenFuncAsync (MyTrigger.Read, async (MyState _state, MyTrigger _trigger, CancellationToken _token) => {
        await Task.Yield ();
        Console.WriteLine ("call WhenFunc callback");
        return MyState.Ready;
    })

    // 效果与 WhenFunc 一致，不过这函数指定异步方法
    .WhenFuncAsync (MyTrigger.FinishRead, async (MyState _state, MyTrigger _trigger, CancellationToken _token, string _param) => {
        await Task.Yield ();
        Console.WriteLine ($"call WhenFunc callback with param [{_param}]");
        return MyState.Ready;
    })

    // 效果与 WhenAction 一致，不过这函数指定异步方法
    .WhenActionAsync (MyTrigger.Read, async (MyState _state, MyTrigger _trigger, CancellationToken _token) => {
        await Task.Yield ();
        Console.WriteLine ("call WhenAction callback");
    })

    // 效果与 WhenAction 一致，不过这函数指定异步方法
    .WhenActionAsync (MyTrigger.FinishRead, async (MyState _state, MyTrigger _trigger, CancellationToken _token, string _param) => {
        await Task.Yield ();
        Console.WriteLine ($"call WhenAction callback with param [{_param}]");
    });
```

然后是触发一个事件：

```csharp
// 异步触发一个事件，并传入指定参数
await _sm.TriggeringAsync (MyTrigger.Run, "hello");

// 限定异步任务最长执行时间，超时取消
var _source = new CancellationTokenSource (TimeSpan.FromSeconds (10));
await _sm.TriggeringAsync (MyTrigger.Run, _source.Token, "hello");
```

await异步触发的事件将在所有函数执行完毕之后返回。另外需要注意，同步与异步最好不要混用，使用的不好就很容易导致死锁，最佳实践是统一同步或统一异步。
