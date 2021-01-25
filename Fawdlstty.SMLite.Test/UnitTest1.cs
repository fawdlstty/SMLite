using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace Fawdlstty.SMLite.Test {
	enum MyState { Rest, Ready, Reading, Writing };
	enum MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };

	[TestClass]
	public class UnitTest1 {
		[TestMethod]
		public void TestMethod1 () {
			int n = 0;
			bool entry_one = true;
			var _smb = new SMLiteBuilder<MyState, MyTrigger> ();
			_smb.Configure (MyState.Rest)
				.OnEntry (() => { Assert.IsFalse (entry_one); entry_one = true; n += 1; })
				.OnLeave (() => { Assert.IsTrue (entry_one); entry_one = false; n += 10; })
				.WhenChangeTo (MyTrigger.Run, MyState.Ready)
				.WhenIgnore (MyTrigger.Close);
			_smb.Configure (MyState.Ready)
				.OnEntry (() => { Assert.IsFalse (entry_one); entry_one = true; n += 100; })
				.OnLeave (() => { Assert.IsTrue (entry_one); entry_one = false; n += 1000; })
				.WhenChangeTo (MyTrigger.Read, MyState.Reading)
				.WhenChangeTo (MyTrigger.Write, MyState.Writing)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);
			_smb.Configure (MyState.Reading)
				.OnEntry (() => { Assert.IsFalse (entry_one); entry_one = true; n += 10000; })
				.OnLeave (() => { Assert.IsTrue (entry_one); entry_one = false; n += 100000; })
				.WhenChangeTo (MyTrigger.FinishRead, MyState.Ready)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);
			_smb.Configure (MyState.Writing)
				.OnEntry (() => { Assert.IsFalse (entry_one); entry_one = true; n += 1000000; })
				.OnLeave (() => { Assert.IsTrue (entry_one); entry_one = false; n += 10000000; })
				.WhenChangeTo (MyTrigger.FinishWrite, MyState.Ready)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);

			var _sm = _smb.Build (MyState.Rest);
			Assert.AreEqual (_sm.State, MyState.Rest);
			Assert.AreEqual (n, 0);
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Run));
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Read));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Write));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite));

			_sm.Triggering (MyTrigger.Close);
			Assert.AreEqual (_sm.State, MyState.Rest);
			Assert.AreEqual (n, 0);

			_sm.Triggering (MyTrigger.Close);
			Assert.AreEqual (_sm.State, MyState.Rest);
			Assert.AreEqual (n, 0);

			_sm.Triggering (MyTrigger.Close);
			Assert.AreEqual (_sm.State, MyState.Rest);
			Assert.AreEqual (n, 0);

			_sm.Triggering (MyTrigger.Run);
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (n, 110);
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Run));
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close));
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Read));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead));
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Write));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite));

			_sm.Triggering (MyTrigger.Close);
			Assert.AreEqual (_sm.State, MyState.Rest);
			Assert.AreEqual (n, 1111);
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Run));
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Read));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Write));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite));

			_sm.Triggering (MyTrigger.Run);
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (n, 1221);

			_sm.Triggering (MyTrigger.Read);
			Assert.AreEqual (_sm.State, MyState.Reading);
			Assert.AreEqual (n, 12221);

			_sm.Triggering (MyTrigger.FinishRead);
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (n, 112321);

			_sm.Triggering (MyTrigger.Write);
			Assert.AreEqual (_sm.State, MyState.Writing);
			Assert.AreEqual (n, 1113321);

			_sm.Triggering (MyTrigger.FinishWrite);
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (n, 11113421);

			_sm.State = MyState.Reading;
			_sm.State = MyState.Writing;
			_sm.State = MyState.Rest;
			Assert.AreEqual (n, 11113421);
		}

		[TestMethod]
		public async Task TestMethod2 () {
			int n = 0;
			bool entry_one = true;
			var _smb = new SMLiteBuilder<MyState, MyTrigger> ();
			_smb.Configure (MyState.Rest)
				.OnEntryAsync (async () => { await Task.Yield (); Assert.IsFalse (entry_one); entry_one = true; n += 1; })
				.OnLeaveAsync (async () => { await Task.Yield (); Assert.IsTrue (entry_one); entry_one = false; n += 10; })
				.WhenChangeTo (MyTrigger.Run, MyState.Ready)
				.WhenIgnore (MyTrigger.Close);
			_smb.Configure (MyState.Ready)
				.OnEntryAsync (async () => { await Task.Yield (); Assert.IsFalse (entry_one); entry_one = true; n += 100; })
				.OnLeaveAsync (async () => { await Task.Yield (); Assert.IsTrue (entry_one); entry_one = false; n += 1000; })
				.WhenChangeTo (MyTrigger.Read, MyState.Reading)
				.WhenChangeTo (MyTrigger.Write, MyState.Writing)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);
			_smb.Configure (MyState.Reading)
				.OnEntryAsync (async () => { await Task.Yield (); Assert.IsFalse (entry_one); entry_one = true; n += 10000; })
				.OnLeaveAsync (async () => { await Task.Yield (); Assert.IsTrue (entry_one); entry_one = false; n += 100000; })
				.WhenChangeTo (MyTrigger.FinishRead, MyState.Ready)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);
			_smb.Configure (MyState.Writing)
				.OnEntryAsync (async () => { await Task.Yield (); Assert.IsFalse (entry_one); entry_one = true; n += 1000000; })
				.OnLeaveAsync (async () => { await Task.Yield (); Assert.IsTrue (entry_one); entry_one = false; n += 10000000; })
				.WhenChangeTo (MyTrigger.FinishWrite, MyState.Ready)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);

			var _sm = _smb.Build (MyState.Rest);
			Assert.AreEqual (_sm.State, MyState.Rest);
			Assert.AreEqual (n, 0);
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Run));
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Read));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Write));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite));

			await _sm.TriggeringAsync (MyTrigger.Close);
			Assert.AreEqual (_sm.State, MyState.Rest);
			Assert.AreEqual (n, 0);

			await _sm.TriggeringAsync (MyTrigger.Close);
			Assert.AreEqual (_sm.State, MyState.Rest);
			Assert.AreEqual (n, 0);

			await _sm.TriggeringAsync (MyTrigger.Close);
			Assert.AreEqual (_sm.State, MyState.Rest);
			Assert.AreEqual (n, 0);

			await _sm.TriggeringAsync (MyTrigger.Run);
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (n, 110);
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Run));
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close));
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Read));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead));
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Write));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite));

			await _sm.TriggeringAsync (MyTrigger.Close);
			Assert.AreEqual (_sm.State, MyState.Rest);
			Assert.AreEqual (n, 1111);
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Run));
			Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Read));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Write));
			Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite));

			await _sm.TriggeringAsync (MyTrigger.Run);
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (n, 1221);

			await _sm.TriggeringAsync (MyTrigger.Read);
			Assert.AreEqual (_sm.State, MyState.Reading);
			Assert.AreEqual (n, 12221);

			await _sm.TriggeringAsync (MyTrigger.FinishRead);
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (n, 112321);

			await _sm.TriggeringAsync (MyTrigger.Write);
			Assert.AreEqual (_sm.State, MyState.Writing);
			Assert.AreEqual (n, 1113321);

			await _sm.TriggeringAsync (MyTrigger.FinishWrite);
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (n, 11113421);
		}

		[TestMethod]
		public void TestMethod3 () {
			string s = "";
			var _smb = new SMLiteBuilder<MyState, MyTrigger> ();
			_smb.Configure (MyState.Rest)
				.WhenFunc (MyTrigger.Run, (MyState _state, MyTrigger _trigger) => { s = "WhenFunc_Run"; return MyState.Ready; })
				.WhenFunc (MyTrigger.Read, (MyState _state, MyTrigger _trigger, string _p1) => { s = _p1; return MyState.Ready; })
				.WhenFunc (MyTrigger.FinishRead, (MyState _state, MyTrigger _trigger, string _p1, int _p2) => { s = $"{_p1}{_p2}"; return MyState.Ready; })
				.WhenAction (MyTrigger.Close, (MyState _state, MyTrigger _trigger) => s = "WhenAction_Close")
				.WhenAction (MyTrigger.Write, (MyState _state, MyTrigger _trigger, string _p1) => s = _p1)
				.WhenAction (MyTrigger.FinishWrite, (MyState _state, MyTrigger _trigger, string _p1, int _p2) => s = $"{_p1}{_p2}");
			_smb.Configure (MyState.Ready)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);

			var _sm = _smb.Build (MyState.Rest);
			Assert.AreEqual (_sm.State, MyState.Rest);
			Assert.AreEqual (s, "");

			_sm.Triggering (MyTrigger.Run);
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (s, "WhenFunc_Run");
			_sm.Triggering (MyTrigger.Close);

			_sm.Triggering (MyTrigger.Read, "hello");
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (s, "hello");
			_sm.Triggering (MyTrigger.Close);

			_sm.Triggering (MyTrigger.FinishRead, "hello", 1);
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (s, "hello1");
			_sm.Triggering (MyTrigger.Close);

			_sm.Triggering (MyTrigger.Close);
			Assert.AreEqual (s, "WhenAction_Close");
			Assert.AreEqual (_sm.State, MyState.Rest);

			_sm.Triggering (MyTrigger.Write, "world");
			Assert.AreEqual (s, "world");
			Assert.AreEqual (_sm.State, MyState.Rest);

			_sm.Triggering (MyTrigger.FinishWrite, "world", 1);
			Assert.AreEqual (s, "world1");
			Assert.AreEqual (_sm.State, MyState.Rest);
		}

		[TestMethod]
		public async Task TestMethod4 () {
			string s = "";
			var _smb = new SMLiteBuilder<MyState, MyTrigger> ();
			_smb.Configure (MyState.Rest)
				.WhenFuncAsync (MyTrigger.Run, async (MyState _state, MyTrigger _trigger, CancellationToken _token) => { await Task.Yield (); s = "WhenFunc_Run"; return MyState.Ready; })
				.WhenFuncAsync (MyTrigger.Read, async (MyState _state, MyTrigger _trigger, CancellationToken _token, string _p1) => { await Task.Yield (); s = _p1; return MyState.Ready; })
				.WhenFuncAsync (MyTrigger.FinishRead, async (MyState _state, MyTrigger _trigger, CancellationToken _token, string _p1, int _p2) => { await Task.Yield (); s = $"{_p1}{_p2}"; return MyState.Ready; })
				.WhenActionAsync (MyTrigger.Close, async (MyState _state, MyTrigger _trigger, CancellationToken _token) => { await Task.Yield (); s = "WhenAction_Close"; })
				.WhenActionAsync (MyTrigger.Write, async (MyState _state, MyTrigger _trigger, CancellationToken _token, string _p1) => { await Task.Yield (); s = _p1; })
				.WhenActionAsync (MyTrigger.FinishWrite, async (MyState _state, MyTrigger _trigger, CancellationToken _token, string _p1, int _p2) => { await Task.Yield (); s = $"{_p1}{_p2}"; });
			_smb.Configure (MyState.Ready)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);

			var _sm = _smb.Build (MyState.Rest);
			Assert.AreEqual (_sm.State, MyState.Rest);
			Assert.AreEqual (s, "");

			await _sm.TriggeringAsync (MyTrigger.Run);
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (s, "WhenFunc_Run");
			await _sm.TriggeringAsync (MyTrigger.Close);

			await _sm.TriggeringAsync (MyTrigger.Read, "hello");
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (s, "hello");
			await _sm.TriggeringAsync (MyTrigger.Close);

			await _sm.TriggeringAsync (MyTrigger.FinishRead, "hello", 1);
			Assert.AreEqual (_sm.State, MyState.Ready);
			Assert.AreEqual (s, "hello1");
			await _sm.TriggeringAsync (MyTrigger.Close);

			await _sm.TriggeringAsync (MyTrigger.Close);
			Assert.AreEqual (s, "WhenAction_Close");
			Assert.AreEqual (_sm.State, MyState.Rest);

			await _sm.TriggeringAsync (MyTrigger.Write, "world");
			Assert.AreEqual (s, "world");
			Assert.AreEqual (_sm.State, MyState.Rest);

			await _sm.TriggeringAsync (MyTrigger.FinishWrite, "world", 1);
			Assert.AreEqual (s, "world1");
			Assert.AreEqual (_sm.State, MyState.Rest);
		}
	}
}
