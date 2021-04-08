'use strict';



let Assert = new Object();
Assert.IsTrue = (b) => { if (!b) aa = bb;/*throw 'Assert Failed';*/ };
Assert.IsFalse = (b) => { if (b) aa = bb;/*throw 'Assert Failed';*/ };
Assert.AreEqual = (a, b) => { if (a != b) aa = bb;/*throw 'Assert Failed';*/ };



const SMLite = require("./SMLite.js");
const SMLiteAsync = require("./SMLiteAsync.js");

const MyState = { Rest: 0, Ready: 1, Reading: 2, Writing: 3 };
const MyTrigger = { Run: 0, Close: 1, Read: 2, FinishRead: 3, Write: 4, FinishWrite: 5 };

function TestMethod1() {
	let n = 0;
	let entry_one = true;
	let _smb = new SMLite.SMLiteBuilder();
	_smb.Configure(MyState.Rest)
		.OnEntry(() => { Assert.IsFalse(entry_one); entry_one = true; n += 1; })
		.OnLeave(() => { Assert.IsTrue(entry_one); entry_one = false; n += 10; })
		.WhenChangeTo(MyTrigger.Run, MyState.Ready)
		.WhenIgnore(MyTrigger.Close);
	_smb.Configure(MyState.Ready)
		.OnEntry(() => { Assert.IsFalse(entry_one); entry_one = true; n += 100; })
		.OnLeave(() => { Assert.IsTrue(entry_one); entry_one = false; n += 1000; })
		.WhenChangeTo(MyTrigger.Read, MyState.Reading)
		.WhenChangeTo(MyTrigger.Write, MyState.Writing)
		.WhenChangeTo(MyTrigger.Close, MyState.Rest);
	_smb.Configure(MyState.Reading)
		.OnEntry(() => { Assert.IsFalse(entry_one); entry_one = true; n += 10000; })
		.OnLeave(() => { Assert.IsTrue(entry_one); entry_one = false; n += 100000; })
		.WhenChangeTo(MyTrigger.FinishRead, MyState.Ready)
		.WhenChangeTo(MyTrigger.Close, MyState.Rest);
	_smb.Configure(MyState.Writing)
		.OnEntry(() => { Assert.IsFalse(entry_one); entry_one = true; n += 1000000; })
		.OnLeave(() => { Assert.IsTrue(entry_one); entry_one = false; n += 10000000; })
		.WhenChangeTo(MyTrigger.FinishWrite, MyState.Ready)
		.WhenChangeTo(MyTrigger.Close, MyState.Rest);

	var _sm = _smb.Build(MyState.Rest);
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
	Assert.AreEqual(n, 0);
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Run));
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Close));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Read));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishRead));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Write));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishWrite));

	_sm.Triggering(MyTrigger.Close);
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
	Assert.AreEqual(n, 0);

	_sm.Triggering(MyTrigger.Close);
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
	Assert.AreEqual(n, 0);

	_sm.Triggering(MyTrigger.Close);
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
	Assert.AreEqual(n, 0);

	_sm.Triggering(MyTrigger.Run);
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(n, 110);
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Run));
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Close));
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Read));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishRead));
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Write));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishWrite));

	_sm.Triggering(MyTrigger.Close);
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
	Assert.AreEqual(n, 1111);
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Run));
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Close));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Read));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishRead));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Write));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishWrite));

	_sm.Triggering(MyTrigger.Run);
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(n, 1221);

	_sm.Triggering(MyTrigger.Read);
	Assert.AreEqual(_sm.GetState(), MyState.Reading);
	Assert.AreEqual(n, 12221);

	_sm.Triggering(MyTrigger.FinishRead);
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(n, 112321);

	_sm.Triggering(MyTrigger.Write);
	Assert.AreEqual(_sm.GetState(), MyState.Writing);
	Assert.AreEqual(n, 1113321);

	_sm.Triggering(MyTrigger.FinishWrite);
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(n, 11113421);

	_sm.SetState(MyState.Reading);
	_sm.SetState(MyState.Writing);
	_sm.SetState(MyState.Rest);
	Assert.AreEqual(n, 11113421);
}

async function TestMethod2() {
	let n = 0;
	let entry_one = true;
	let _smb = new SMLiteAsync.SMLiteBuilderAsync();
	_smb.Configure(MyState.Rest)
		.OnEntryAsync(async () => { Assert.IsFalse(entry_one); entry_one = true; n += 1; })
		.OnLeaveAsync(async () => { Assert.IsTrue(entry_one); entry_one = false; n += 10; })
		.WhenChangeTo(MyTrigger.Run, MyState.Ready)
		.WhenIgnore(MyTrigger.Close);
	_smb.Configure(MyState.Ready)
		.OnEntryAsync(async () => { Assert.IsFalse(entry_one); entry_one = true; n += 100; })
		.OnLeaveAsync(async () => { Assert.IsTrue(entry_one); entry_one = false; n += 1000; })
		.WhenChangeTo(MyTrigger.Read, MyState.Reading)
		.WhenChangeTo(MyTrigger.Write, MyState.Writing)
		.WhenChangeTo(MyTrigger.Close, MyState.Rest);
	_smb.Configure(MyState.Reading)
		.OnEntryAsync(async () => { Assert.IsFalse(entry_one); entry_one = true; n += 10000; })
		.OnLeaveAsync(async () => { Assert.IsTrue(entry_one); entry_one = false; n += 100000; })
		.WhenChangeTo(MyTrigger.FinishRead, MyState.Ready)
		.WhenChangeTo(MyTrigger.Close, MyState.Rest);
	_smb.Configure(MyState.Writing)
		.OnEntryAsync(async () => { Assert.IsFalse(entry_one); entry_one = true; n += 1000000; })
		.OnLeaveAsync(async () => { Assert.IsTrue(entry_one); entry_one = false; n += 10000000; })
		.WhenChangeTo(MyTrigger.FinishWrite, MyState.Ready)
		.WhenChangeTo(MyTrigger.Close, MyState.Rest);

	var _sm = _smb.Build(MyState.Rest);
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
	Assert.AreEqual(n, 0);
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Run));
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Close));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Read));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishRead));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Write));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishWrite));

	await _sm.TriggeringAsync(MyTrigger.Close);
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
	Assert.AreEqual(n, 0);

	await _sm.TriggeringAsync(MyTrigger.Close);
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
	Assert.AreEqual(n, 0);

	await _sm.TriggeringAsync(MyTrigger.Close);
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
	Assert.AreEqual(n, 0);

	await _sm.TriggeringAsync(MyTrigger.Run);
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(n, 110);
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Run));
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Close));
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Read));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishRead));
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Write));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishWrite));

	await _sm.TriggeringAsync(MyTrigger.Close);
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
	Assert.AreEqual(n, 1111);
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Run));
	Assert.IsTrue(_sm.AllowTriggering(MyTrigger.Close));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Read));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishRead));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.Write));
	Assert.IsFalse(_sm.AllowTriggering(MyTrigger.FinishWrite));

	await _sm.TriggeringAsync(MyTrigger.Run);
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(n, 1221);

	await _sm.TriggeringAsync(MyTrigger.Read);
	Assert.AreEqual(_sm.GetState(), MyState.Reading);
	Assert.AreEqual(n, 12221);

	await _sm.TriggeringAsync(MyTrigger.FinishRead);
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(n, 112321);

	await _sm.TriggeringAsync(MyTrigger.Write);
	Assert.AreEqual(_sm.GetState(), MyState.Writing);
	Assert.AreEqual(n, 1113321);

	await _sm.TriggeringAsync(MyTrigger.FinishWrite);
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(n, 11113421);

	_sm.SetState(MyState.Reading);
	_sm.SetState(MyState.Writing);
	_sm.SetState(MyState.Rest);
	Assert.AreEqual(n, 11113421);
}

function TestMethod3() {
	let s = "";
	let _smb = new SMLite.SMLiteBuilder();
	_smb.Configure(MyState.Rest)
		.WhenFunc(MyTrigger.Run,(_state, _trigger) => { s = "WhenFunc_Run"; return MyState.Ready; })
		.WhenFunc(MyTrigger.Read,(_state, _trigger, _p1) => { s = _p1; return MyState.Ready; })
		.WhenFunc(MyTrigger.FinishRead, (_state, _trigger, _p1, _p2) => { s = _p1 + _p2; return MyState.Ready; })
		.WhenAction(MyTrigger.Close,(_state, _trigger) => s = "WhenAction_Close")
		.WhenAction(MyTrigger.Write,(_state, _trigger, _p1) => s = _p1)
		.WhenAction(MyTrigger.FinishWrite,(_state, _trigger, _p1, _p2) => s = _p1 + _p2);
	_smb.Configure(MyState.Ready)
		.WhenChangeTo(MyTrigger.Close, MyState.Rest);

	let _sm = _smb.Build(MyState.Rest);
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
	Assert.AreEqual(s, "");

	_sm.Triggering(MyTrigger.Run);
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(s, "WhenFunc_Run");
	_sm.Triggering(MyTrigger.Close);

	_sm.Triggering(MyTrigger.Read, "hello");
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(s, "hello");
	_sm.Triggering(MyTrigger.Close);

	_sm.Triggering(MyTrigger.FinishRead, "hello", 1);
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(s, "hello1");
	_sm.Triggering(MyTrigger.Close);

	_sm.Triggering(MyTrigger.Close);
	Assert.AreEqual(s, "WhenAction_Close");
	Assert.AreEqual(_sm.GetState(), MyState.Rest);

	_sm.Triggering(MyTrigger.Write, "world");
	Assert.AreEqual(s, "world");
	Assert.AreEqual(_sm.GetState(), MyState.Rest);

	_sm.Triggering(MyTrigger.FinishWrite, "world", 1);
	Assert.AreEqual(s, "world1");
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
}

async function TestMethod4() {
	let s = "";
	let _smb = new SMLiteAsync.SMLiteBuilderAsync();
	_smb.Configure(MyState.Rest)
		.WhenFuncAsync(MyTrigger.Run, async (_state, _trigger) => { s = "WhenFunc_Run"; return MyState.Ready; })
		.WhenFuncAsync(MyTrigger.Read, async (_state, _trigger, _p1) => { s = _p1; return MyState.Ready; })
		.WhenFuncAsync(MyTrigger.FinishRead, async (_state, _trigger, _p1, _p2) => { s = _p1 + _p2; return MyState.Ready; })
		.WhenActionAsync(MyTrigger.Close, async (_state, _trigger) => s = "WhenAction_Close")
		.WhenActionAsync(MyTrigger.Write, async (_state, _trigger, _p1) => s = _p1)
		.WhenActionAsync(MyTrigger.FinishWrite, async (_state, _trigger, _p1, _p2) => s = _p1 + _p2);
	_smb.Configure(MyState.Ready)
		.WhenChangeTo(MyTrigger.Close, MyState.Rest);

	let _sm = _smb.Build(MyState.Rest);
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
	Assert.AreEqual(s, "");

	await _sm.TriggeringAsync(MyTrigger.Run);
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(s, "WhenFunc_Run");
	await _sm.TriggeringAsync(MyTrigger.Close);

	await _sm.TriggeringAsync(MyTrigger.Read, "hello");
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(s, "hello");
	await _sm.TriggeringAsync(MyTrigger.Close);

	await _sm.TriggeringAsync(MyTrigger.FinishRead, "hello", 1);
	Assert.AreEqual(_sm.GetState(), MyState.Ready);
	Assert.AreEqual(s, "hello1");
	await _sm.TriggeringAsync(MyTrigger.Close);

	await _sm.TriggeringAsync(MyTrigger.Close);
	Assert.AreEqual(s, "WhenAction_Close");
	Assert.AreEqual(_sm.GetState(), MyState.Rest);

	await _sm.TriggeringAsync(MyTrigger.Write, "world");
	Assert.AreEqual(s, "world");
	Assert.AreEqual(_sm.GetState(), MyState.Rest);

	await _sm.TriggeringAsync(MyTrigger.FinishWrite, "world", 1);
	Assert.AreEqual(s, "world1");
	Assert.AreEqual(_sm.GetState(), MyState.Rest);
}



async function Test() {
	TestMethod1();
	await TestMethod2();
	TestMethod3();
	await TestMethod4();
	console.log("Test success.");
}

try {
	Test();
} catch(e) {
	console.log(e);
}
new Promise(resolve => setTimeout(resolve, 1000 * 60 * 60));
