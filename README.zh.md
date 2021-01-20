# SMLite

[English](./README.md) | 简体中文

快速、仅头文件、简单的状态机实现

状态机是一种用于维护状态的对象。比如我假设一种半双工网络状态机，这种状态机在读数据时不能写，写数据时不能读。这种状态机有四种状态：未运行、待命、读取中、写入中。然后，我们假设有六种事件，启动，关闭，写入，读取，写入完成，读取完成。好了，我们用if来定义规则看看，分别制定某个状态分别能够触发什么方法：

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

现在状态和事件还不多，加起来只有十种，我还没开始写实现呢，仅仅规则代码，看起来就有点杂乱了，不易维护，容易出问题。好了，现在来考虑使用状态机的方式来实现。

```cpp
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

规则代码行数瞬间减少三分之二。下面我详细解释一下这段代码含义。_sm变量定义这一行，含义就是定义一个状态机，并指定当前状态机的初始化状态。

后面的Configure函数这一行，含义是，指定一个状态，然后对这个状态进行配置，这个状态下允许触发什么什么事件，然后做什么操作等等。WhenChangeTo含义是，当遇到某种事件触发，那么修改状态为后者。下面我们来详细解释一下这段代码：

- 当状态为`MyState::Rest`时，如果遇到`MyTrigger::Run`事件，则修改状态为`MyState::Ready`
- 当状态为`MyState::Rest`时，如果遇到`MyTrigger::Close`事件，不做任何操作
- 当状态为`MyState::Ready`时，如果遇到`MyTrigger::Read`事件，则修改状态为`MyState::Reading`
- 当状态为`MyState::Ready`时，如果遇到`MyTrigger::Write`事件，则修改状态为`MyState::Writing`
- 当状态为`MyState::Ready`时，如果遇到`MyTrigger::Close`事件，则修改状态为`MyState::Rest`
- ...

好了，一个简单的状态机就完成了。当然，简单修改事件可能不太能满足我们的需要，我们想着，遇到某种状态后，调用一个回调函数，然后通过自己的代码来处理，决定将状态修改为某个值，或者让某个状态下，允许触发一个事件，但不做任何操作，等等……

SMLite现提供4个方法，分别是：

- `WhenFunc`：遇到某个事件时，调用回调函数闭包，通过回调函数决定状态的值
- `WhenAction`：遇到某个事件时，调用回调函数闭包
- `WhenChangeTo`：遇到某个事件时，将状态机修改为指定的状态
- `WhenIgnore`：遇到某个事件时，忽略

好了，通过上面，我们完成了状态及规则的定义，下面我们来看看，具体如何使用。首先，由于我们在定义状态机时，指定了初始状态，所以状态机的状态是初始值：

```cpp
assert (_sm.GetState () == MyState::Rest);
```

由于我们对这个状态定义了两个触发器，所以这个状态仅接受这两种触发器的触发：

```cpp
assert (_sm.AllowTriggering (MyTrigger::Run));
assert (_sm.AllowTriggering (MyTrigger::Close));
assert (!_sm.AllowTriggering (MyTrigger::Read));
assert (!_sm.AllowTriggering (MyTrigger::FinishRead));
assert (!_sm.AllowTriggering (MyTrigger::Write));
assert (!_sm.AllowTriggering (MyTrigger::FinishWrite));
```

下面我们来完成一次事件触发，根据逻辑，状态会变成我们指定的状态：

```cpp
_sm.Triggering (MyTrigger::Run);
assert (_sm.GetState () == MyState::Ready);
```

通过引申的方式还能实现更多功能，比如通过闭包将shared_ptr作为参打包进去，使得在触发事件后，还能接收自定义参数。
