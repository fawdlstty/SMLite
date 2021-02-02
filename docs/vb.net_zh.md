# VB.Net库用户手册

Step 1. 通过 NuGet 下载 `Fawdlstty.SMLite` 库

Step 2. 定义两个枚举类，分别代表所有的状态与所有的触发器

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

Step 3. 定义状态机变量，模板为两个枚举类，参数为初始值

```vb
Dim _smb As SMLiteBuilder(Of MyState, MyTrigger) = New SMLiteBuilder(Of MyState, MyTrigger)()
```

Step 4. 定义状态机的规则，指定具体的某个状态允许触发什么事件

```vb
With _smb.Configure(MyState.Rest)

    ' 如果状态由其他状态变成 MyState.Rest 状态，那么触发此方法，初始化状态机时指定的初始值不触发此方法
    .OnEntry(Sub()
                 Console.WriteLine ("entry Rest")
             End Sub)

    ' 如果状态由 MyState.Rest 状态变成其他状态，那么触发此方法
    .OnLeave(Sub()
                 Console.WriteLine ("leave Rest")
             End Sub)

    ' 如果触发 MyTrigger.Run，则将状态改为 MyState.Ready
    .WhenChangeTo(MyTrigger.Run, MyState.Ready)

    ' 如果触发 MyTrigger.Run，忽略
    .WhenIgnore(MyTrigger.Close)

    ' 如果触发 MyTrigger.Read，则调用回调函数，并将状态调整为返回值
    .WhenFunc(MyTrigger.Read, Function(_state As MyState, _trigger As MyTrigger) As MyState
                                  Console.WriteLine ("call WhenFunc callback")
                                  Return MyState.Ready
                              End Function)

    ' 如果触发 MyTrigger.FinishRead，则调用回调函数，并将状态调整为返回值
    ' 需注意，触发时候需传入参数，数量与类型必须完全匹配，否则抛异常
    .WhenFunc(MyTrigger.FinishRead, Function(_state As MyState, _trigger As MyTrigger, _param_ As String) As MyState
                                        Console.WriteLine ($"call WhenFunc callback with param [{_param}]")
                                        Return MyState.Ready
                                    End Function)

    ' 如果触发 MyTrigger.Write，则调用回调函数（触发此方法回调不调整返回值）
    .WhenAction(MyTrigger.Write, Sub(_state As MyState, _trigger As MyTrigger)
                                     Console.WriteLine ("call WhenAction callback")
                                 End Sub)

    ' 如果触发 MyTrigger.FinishWrite，则调用回调函数（触发此方法回调不调整返回值）
    ' 需注意，触发时候需传入参数，数量与类型必须完全匹配，否则抛异常
    .WhenAction(MyTrigger.FinishWrite, Sub(_state As MyState, _trigger As MyTrigger, _param As String)
                                           Console.WriteLine ($"call WhenAction callback with param [{_param}]")
                                       End Sub)
End With
```

同一个状态下，如果遇到同样的触发器，最多只允许定义一种处理方式，上面代码对定义的触发事件有详细解释。如果不定义触发事件但遇到触发，那么抛异常。

Step 5. 下面开始真正使用到状态机

```vb
' 生成状态机
Dim _sm = _smb.Build(MyState.Rest)

' 获取当前状态
Dim _state = _sm.State

' 判断是否允许触发某一个事件
Dim _allow = _sm.AllowTriggering(MyTrigger.Close)

' 触发一个事件
_sm.Triggering (MyTrigger.Run)

' 触发一个事件，并传入指定参数
_sm.Triggering (MyTrigger.Run, "hello")

' 强行修改当前状态，此操作将不会触发OnEntry、OnLeave事件
_sm.State = MyState.Rest
```

Step 6. 如果用到异步

使用与上面非常相似，下面是指定异步触发回调函数

```vb
With _smb.Configure(MyState.Rest)

    ' 与 OnEntry 效果一致，不过这函数指定异步方法，并且不能与 OnEntry 同时调用
    .OnEntryAsync(Async Function() As Task
                      Await Task.Yield()
                      Console.WriteLine ("entry Ready")
                  End Function)

    ' 与 OnLeave 效果一致，不过这函数指定异步方法，并且不能与 OnLeave 同时调用
    .OnLeaveAsync(Async Function() As Task
                      Await Task.Yield()
                      Console.WriteLine ("leave Ready")
                  End Function)

    ' 效果与 WhenFunc 一致，不过这函数指定异步方法
    .WhenFuncAsync(MyTrigger.Read, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken) As Task(Of MyState)
                                       Await Task.Yield()
                                       Console.WriteLine ("call WhenFunc callback")
                                       Return MyState.Ready
                                   End Function)

    ' 效果与 WhenFunc 一致，不过这函数指定异步方法
    .WhenFuncAsync(MyTrigger.FinishRead, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken, _param As String) As Task(Of MyState)
                                             Await Task.Yield()
                                             Console.WriteLine ($"call WhenFunc callback with param [{_param}]")
                                             Return MyState.Ready
                                         End Function)

    ' 效果与 WhenAction 一致，不过这函数指定异步方法
    .WhenActionAsync(MyTrigger.Write, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken) As Task
                                          Await Task.Yield()
                                          Console.WriteLine ("call WhenAction callback")
                                      End Function)

    ' 效果与 WhenAction 一致，不过这函数指定异步方法
    .WhenActionAsync(MyTrigger.FinishWrite, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken, _p1 As String) As Task
                                                Await Task.Yield()
                                                Console.WriteLine ($"call WhenAction callback with param [{_param}]")
                                            End Function)
End With
```

然后是触发一个事件：

```vb
' 异步触发一个事件，并传入指定参数
Await _sm.TriggeringAsync (MyTrigger.Run, "hello")

' 限定异步任务最长执行时间，超时取消
Dim _source = new CancellationTokenSource (TimeSpan.FromSeconds (10))
Await _sm.TriggeringAsync (MyTrigger.Run, _source.Token, "hello")
```

await异步触发的事件将在所有函数执行完毕之后返回。另外需要注意，同步与异步最好不要混用，使用的不好就很容易导致死锁，最佳实践是统一同步或统一异步。
