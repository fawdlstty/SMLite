Imports System.Threading
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace Fawdlstty.SMLite.vb.Test
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

	<TestClass>
	Public Class UnitTest1
		<TestMethod>
		Sub TestSub1()
			Dim n As Integer = 0
			Dim entry_one As Boolean = True
			Dim _smb As SMLiteBuilder(Of MyState, MyTrigger) = New SMLiteBuilder(Of MyState, MyTrigger)()
			With _smb.Configure(MyState.Rest)
				.OnEntry(Sub()
							 Assert.IsFalse(entry_one)
							 entry_one = True
							 n += 1
						 End Sub)
				.OnLeave(Sub()
							 Assert.IsTrue(entry_one)
							 entry_one = False
							 n += 10
						 End Sub)
				.WhenChangeTo(MyTrigger.Run, MyState.Ready)
				.WhenIgnore(MyTrigger.Close)
			End With
			With _smb.Configure(MyState.Ready)
				.OnEntry(Sub()
							 Assert.IsFalse(entry_one)
							 entry_one = True
							 n += 100
						 End Sub)
				.OnLeave(Sub()
							 Assert.IsTrue(entry_one)
							 entry_one = False
							 n += 1000
						 End Sub)
				.WhenChangeTo(MyTrigger.Read, MyState.Reading)
				.WhenChangeTo(MyTrigger.Write, MyState.Writing)
				.WhenChangeTo(MyTrigger.Close, MyState.Rest)
			End With
			With _smb.Configure(MyState.Reading)
				.OnEntry(Sub()
							 Assert.IsFalse(entry_one)
							 entry_one = True
							 n += 10000
						 End Sub)
				.OnLeave(Sub()
							 Assert.IsTrue(entry_one)
							 entry_one = False
							 n += 100000
						 End Sub)
				.WhenChangeTo(MyTrigger.FinishRead, MyState.Ready)
				.WhenChangeTo(MyTrigger.Close, MyState.Rest)
			End With
			With _smb.Configure(MyState.Writing)
				.OnEntry(Sub()
							 Assert.IsFalse(entry_one)
							 entry_one = True
							 n += 1000000
						 End Sub)
				.OnLeave(Sub()
							 Assert.IsTrue(entry_one)
							 entry_one = False
							 n += 10000000
						 End Sub)
				.WhenChangeTo(MyTrigger.FinishWrite, MyState.Ready)
				.WhenChangeTo(MyTrigger.Close, MyState.Rest)
			End With

			Dim _sm = _smb.Build(MyState.Rest)
			Assert.AreEqual(_sm.State, MyState.Rest)
			Assert.AreEqual(n, 0)
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Run))
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Close))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Read))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishRead))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Write))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishWrite))

			_sm.Triggering(MyTrigger.Close)
			Assert.AreEqual(_sm.State, MyState.Rest)
			Assert.AreEqual(n, 0)

			_sm.Triggering(MyTrigger.Close)
			Assert.AreEqual(_sm.State, MyState.Rest)
			Assert.AreEqual(n, 0)

			_sm.Triggering(MyTrigger.Close)
			Assert.AreEqual(_sm.State, MyState.Rest)
			Assert.AreEqual(n, 0)

			_sm.Triggering(MyTrigger.Run)
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(n, 110)
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Run))
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Close))
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Read))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishRead))
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Write))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishWrite))

			_sm.Triggering(MyTrigger.Close)
			Assert.AreEqual(_sm.State, MyState.Rest)
			Assert.AreEqual(n, 1111)
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Run))
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Close))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Read))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishRead))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Write))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishWrite))

			_sm.Triggering(MyTrigger.Run)
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(n, 1221)

			_sm.Triggering(MyTrigger.Read)
			Assert.AreEqual(_sm.State, MyState.Reading)
			Assert.AreEqual(n, 12221)

			_sm.Triggering(MyTrigger.FinishRead)
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(n, 112321)

			_sm.Triggering(MyTrigger.Write)
			Assert.AreEqual(_sm.State, MyState.Writing)
			Assert.AreEqual(n, 1113321)

			_sm.Triggering(MyTrigger.FinishWrite)
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(n, 11113421)

			_sm.State = MyState.Reading
			_sm.State = MyState.Writing
			_sm.State = MyState.Rest
			Assert.AreEqual(n, 11113421)
		End Sub

		<TestMethod>
		Async Function TestSub2() As Task
			Dim n As Integer = 0
			Dim entry_one As Boolean = True
			Dim _smb As SMLiteBuilderAsync(Of MyState, MyTrigger) = New SMLiteBuilderAsync(Of MyState, MyTrigger)()
			With _smb.Configure(MyState.Rest)
				.OnEntryAsync(Async Function() As Task
								  Await Task.Yield()
								  Assert.IsFalse(entry_one)
								  entry_one = True
								  n += 1
							  End Function)
				.OnLeaveAsync(Async Function() As Task
								  Await Task.Yield()
								  Assert.IsTrue(entry_one)
								  entry_one = False
								  n += 10
							  End Function)
				.WhenChangeTo(MyTrigger.Run, MyState.Ready)
				.WhenIgnore(MyTrigger.Close)
			End With
			With _smb.Configure(MyState.Ready)
				.OnEntryAsync(Async Function() As Task
								  Await Task.Yield()
								  Assert.IsFalse(entry_one)
								  entry_one = True
								  n += 100
							  End Function)
				.OnLeaveAsync(Async Function() As Task
								  Await Task.Yield()
								  Assert.IsTrue(entry_one)
								  entry_one = False
								  n += 1000
							  End Function)
				.WhenChangeTo(MyTrigger.Read, MyState.Reading)
				.WhenChangeTo(MyTrigger.Write, MyState.Writing)
				.WhenChangeTo(MyTrigger.Close, MyState.Rest)
			End With
			With _smb.Configure(MyState.Reading)
				.OnEntryAsync(Async Function() As Task
								  Await Task.Yield()
								  Assert.IsFalse(entry_one)
								  entry_one = True
								  n += 10000
							  End Function)
				.OnLeaveAsync(Async Function() As Task
								  Await Task.Yield()
								  Assert.IsTrue(entry_one)
								  entry_one = False
								  n += 100000
							  End Function)
				.WhenChangeTo(MyTrigger.FinishRead, MyState.Ready)
				.WhenChangeTo(MyTrigger.Close, MyState.Rest)
			End With
			With _smb.Configure(MyState.Writing)
				.OnEntryAsync(Async Function() As Task
								  Await Task.Yield()
								  Assert.IsFalse(entry_one)
								  entry_one = True
								  n += 1000000
							  End Function)
				.OnLeaveAsync(Async Function() As Task
								  Await Task.Yield()
								  Assert.IsTrue(entry_one)
								  entry_one = False
								  n += 10000000
							  End Function)
				.WhenChangeTo(MyTrigger.FinishWrite, MyState.Ready)
				.WhenChangeTo(MyTrigger.Close, MyState.Rest)
			End With

			Dim _sm = _smb.Build(MyState.Rest)
			Assert.AreEqual(_sm.State, MyState.Rest)
			Assert.AreEqual(n, 0)
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Run))
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Close))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Read))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishRead))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Write))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishWrite))

			Await _sm.TriggeringAsync(MyTrigger.Close)
			Assert.AreEqual(_sm.State, MyState.Rest)
			Assert.AreEqual(n, 0)

			Await _sm.TriggeringAsync(MyTrigger.Close)
			Assert.AreEqual(_sm.State, MyState.Rest)
			Assert.AreEqual(n, 0)

			Await _sm.TriggeringAsync(MyTrigger.Close)
			Assert.AreEqual(_sm.State, MyState.Rest)
			Assert.AreEqual(n, 0)

			Await _sm.TriggeringAsync(MyTrigger.Run)
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(n, 110)
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Run))
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Close))
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Read))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishRead))
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Write))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishWrite))

			Await _sm.TriggeringAsync(MyTrigger.Close)
			Assert.AreEqual(_sm.State, MyState.Rest)
			Assert.AreEqual(n, 1111)
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Run))
			Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Close))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Read))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishRead))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Write))
			Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishWrite))

			Await _sm.TriggeringAsync(MyTrigger.Run)
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(n, 1221)

			Await _sm.TriggeringAsync(MyTrigger.Read)
			Assert.AreEqual(_sm.State, MyState.Reading)
			Assert.AreEqual(n, 12221)

			Await _sm.TriggeringAsync(MyTrigger.FinishRead)
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(n, 112321)

			Await _sm.TriggeringAsync(MyTrigger.Write)
			Assert.AreEqual(_sm.State, MyState.Writing)
			Assert.AreEqual(n, 1113321)

			Await _sm.TriggeringAsync(MyTrigger.FinishWrite)
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(n, 11113421)

			_sm.State = MyState.Reading
			_sm.State = MyState.Writing
			_sm.State = MyState.Rest
			Assert.AreEqual(n, 11113421)
		End Function

		<TestMethod>
		Sub TestSub9()
			Dim s As String = ""
			Dim _smb As SMLiteBuilder(Of MyState, MyTrigger) = New SMLiteBuilder(Of MyState, MyTrigger)()
			With _smb.Configure(MyState.Rest)
				.WhenFunc_st(MyTrigger.Run, Function(_state As MyState, _trigger As MyTrigger) As MyState
												s = "WhenFunc_Run"
												Return MyState.Ready
											End Function)
				.WhenFunc_st(MyTrigger.Read, Function(_state As MyState, _trigger As MyTrigger, _p1 As String) As MyState
												 s = _p1
												 Return MyState.Ready
											 End Function)
				.WhenFunc_st(MyTrigger.FinishRead, Function(_state As MyState, _trigger As MyTrigger, _p1 As String, _p2 As Integer) As MyState
													   s = $"{_p1}{_p2}"
													   Return MyState.Ready
												   End Function)
				.WhenAction_st(MyTrigger.Close, Sub(_state As MyState, _trigger As MyTrigger)
													s = "WhenAction_Close"
												End Sub)
				.WhenAction_st(MyTrigger.Write, Sub(_state As MyState, _trigger As MyTrigger, _p1 As String)
													s = _p1
												End Sub)
				.WhenAction_st(MyTrigger.FinishWrite, Sub(_state As MyState, _trigger As MyTrigger, _p1 As String, _p2 As Integer)
														  s = $"{_p1}{_p2}"
													  End Sub)
			End With
			With _smb.Configure(MyState.Ready)
				.WhenChangeTo(MyTrigger.Close, MyState.Rest)
			End With

			Dim _sm = _smb.Build(MyState.Rest)
			Assert.AreEqual(_sm.State, MyState.Rest)
			Assert.AreEqual(s, "")

			_sm.Triggering(MyTrigger.Run)
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(s, "WhenFunc_Run")
			_sm.Triggering(MyTrigger.Close)

			_sm.Triggering(MyTrigger.Read, "hello")
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(s, "hello")
			_sm.Triggering(MyTrigger.Close)

			_sm.Triggering(MyTrigger.FinishRead, "hello", 1)
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(s, "hello1")
			_sm.Triggering(MyTrigger.Close)

			_sm.Triggering(MyTrigger.Close)
			Assert.AreEqual(s, "WhenAction_Close")
			Assert.AreEqual(_sm.State, MyState.Rest)

			_sm.Triggering(MyTrigger.Write, "world")
			Assert.AreEqual(s, "world")
			Assert.AreEqual(_sm.State, MyState.Rest)

			_sm.Triggering(MyTrigger.FinishWrite, "world", 1)
			Assert.AreEqual(s, "world1")
			Assert.AreEqual(_sm.State, MyState.Rest)
		End Sub

		<TestMethod>
		Async Function TestSub10() As Task
			Dim s As String = ""
			Dim _smb As SMLiteBuilderAsync(Of MyState, MyTrigger) = New SMLiteBuilderAsync(Of MyState, MyTrigger)()
			With _smb.Configure(MyState.Rest)
				.WhenFuncAsync_st(MyTrigger.Run, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken) As Task(Of MyState)
													 Await Task.Yield()
													 s = "WhenFunc_Run"
													 Return MyState.Ready
												 End Function)
				.WhenFuncAsync_st(MyTrigger.Read, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken, _p1 As String) As Task(Of MyState)
													  Await Task.Yield()
													  s = _p1
													  Return MyState.Ready
												  End Function)
				.WhenFuncAsync_st(MyTrigger.FinishRead, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken, _p1 As String, _p2 As Integer) As Task(Of MyState)
															Await Task.Yield()
															s = $"{_p1}{_p2}"
															Return MyState.Ready
														End Function)
				.WhenActionAsync_st(MyTrigger.Close, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken) As Task
														 Await Task.Yield()
														 s = "WhenAction_Close"
													 End Function)
				.WhenActionAsync_st(MyTrigger.Write, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken, _p1 As String) As Task
														 Await Task.Yield()
														 s = _p1
													 End Function)
				.WhenActionAsync_st(MyTrigger.FinishWrite, Async Function(_state As MyState, _trigger As MyTrigger, _token As CancellationToken, _p1 As String, _p2 As Integer) As Task
															   Await Task.Yield()
															   s = $"{_p1}{_p2}"
														   End Function)
			End With
			With _smb.Configure(MyState.Ready)
				.WhenChangeTo(MyTrigger.Close, MyState.Rest)
			End With

			Dim _sm = _smb.Build(MyState.Rest)
			Assert.AreEqual(_sm.State, MyState.Rest)
			Assert.AreEqual(s, "")

			Await _sm.TriggeringAsync(MyTrigger.Run)
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(s, "WhenFunc_Run")
			Await _sm.TriggeringAsync(MyTrigger.Close)

			Await _sm.TriggeringAsync(MyTrigger.Read, "hello")
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(s, "hello")
			Await _sm.TriggeringAsync(MyTrigger.Close)

			Await _sm.TriggeringAsync(MyTrigger.FinishRead, "hello", 1)
			Assert.AreEqual(_sm.State, MyState.Ready)
			Assert.AreEqual(s, "hello1")
			Await _sm.TriggeringAsync(MyTrigger.Close)

			Await _sm.TriggeringAsync(MyTrigger.Close)
			Assert.AreEqual(s, "WhenAction_Close")
			Assert.AreEqual(_sm.State, MyState.Rest)

			Await _sm.TriggeringAsync(MyTrigger.Write, "world")
			Assert.AreEqual(s, "world")
			Assert.AreEqual(_sm.State, MyState.Rest)

			Await _sm.TriggeringAsync(MyTrigger.FinishWrite, "world", 1)
			Assert.AreEqual(s, "world1")
			Assert.AreEqual(_sm.State, MyState.Rest)
		End Function
	End Class
End Namespace
