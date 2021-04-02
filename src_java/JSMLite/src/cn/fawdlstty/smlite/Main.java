package cn.fawdlstty.smlite;

import java.util.concurrent.atomic.AtomicBoolean;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.concurrent.atomic.AtomicReference;

enum MyState { Rest, Ready, Reading, Writing };
enum MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };

class Assert {
    static void IsTrue (boolean b) throws Exception { if (!b) throw new Exception ("IsTrue"); }
    static void IsFalse (boolean b) throws Exception { if (b) throw new Exception ("IsFalse"); }
    static void AreEqual (Object a, Object b) throws Exception { if (!a.equals(b)) throw new Exception ("AreEqual"); }
}

public class Main {
    static void TestMethod1 () throws Exception {
        AtomicInteger n = new AtomicInteger(0);
        AtomicBoolean entry_one = new AtomicBoolean(true);
        SMLiteBuilder<MyState, MyTrigger> _smb = new SMLiteBuilder ();
        _smb.Configure (MyState.Rest)
                .OnEntry (() -> { Assert.IsFalse (entry_one.get()); entry_one.set(true); n.addAndGet(1); })
				.OnLeave (() -> { Assert.IsTrue (entry_one.get()); entry_one.set(false); n.addAndGet(10); })
				.WhenChangeTo (MyTrigger.Run, MyState.Ready)
                .WhenIgnore (MyTrigger.Close);
        _smb.Configure (MyState.Ready)
                .OnEntry (() -> { Assert.IsFalse (entry_one.get()); entry_one.set(true); n.addAndGet(100); })
				.OnLeave (() -> { Assert.IsTrue (entry_one.get()); entry_one.set(false); n.addAndGet(1000); })
				.WhenChangeTo (MyTrigger.Read, MyState.Reading)
                .WhenChangeTo (MyTrigger.Write, MyState.Writing)
                .WhenChangeTo (MyTrigger.Close, MyState.Rest);
        _smb.Configure (MyState.Reading)
                .OnEntry (() -> { Assert.IsFalse (entry_one.get()); entry_one.set(true); n.addAndGet(10000); })
				.OnLeave (() -> { Assert.IsTrue (entry_one.get()); entry_one.set(false); n.addAndGet(100000); })
				.WhenChangeTo (MyTrigger.FinishRead, MyState.Ready)
                .WhenChangeTo (MyTrigger.Close, MyState.Rest);
        _smb.Configure (MyState.Writing)
                .OnEntry (() -> { Assert.IsFalse (entry_one.get()); entry_one.set(true); n.addAndGet(1000000); })
				.OnLeave (() -> { Assert.IsTrue (entry_one.get()); entry_one.set(false); n.addAndGet(10000000); })
				.WhenChangeTo (MyTrigger.FinishWrite, MyState.Ready)
                .WhenChangeTo (MyTrigger.Close, MyState.Rest);

        SMLite<MyState, MyTrigger> _sm = _smb.Build (MyState.Rest);
        Assert.AreEqual (_sm.GetState (), MyState.Rest);
        Assert.AreEqual (n.get(), 0);
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Run));
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Read));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Write));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite));

        _sm.Triggering (MyTrigger.Close);
        Assert.AreEqual (_sm.GetState(), MyState.Rest);
        Assert.AreEqual (n.get(), 0);

        _sm.Triggering (MyTrigger.Close);
        Assert.AreEqual (_sm.GetState(), MyState.Rest);
        Assert.AreEqual (n.get(), 0);

        _sm.Triggering (MyTrigger.Close);
        Assert.AreEqual (_sm.GetState(), MyState.Rest);
        Assert.AreEqual (n.get(), 0);

        _sm.Triggering (MyTrigger.Run);
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (n.get(), 110);
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Run));
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close));
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Read));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead));
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Write));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite));

        _sm.Triggering (MyTrigger.Close);
        Assert.AreEqual (_sm.GetState(), MyState.Rest);
        Assert.AreEqual (n.get(), 1111);
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Run));
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Read));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Write));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite));

        _sm.Triggering (MyTrigger.Run);
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (n.get(), 1221);

        _sm.Triggering (MyTrigger.Read);
        Assert.AreEqual (_sm.GetState(), MyState.Reading);
        Assert.AreEqual (n.get(), 12221);

        _sm.Triggering (MyTrigger.FinishRead);
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (n.get(), 112321);

        _sm.Triggering (MyTrigger.Write);
        Assert.AreEqual (_sm.GetState(), MyState.Writing);
        Assert.AreEqual (n.get(), 1113321);

        _sm.Triggering (MyTrigger.FinishWrite);
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (n.get(), 11113421);

        _sm.SetState(MyState.Reading);
        _sm.SetState(MyState.Writing);
        _sm.SetState(MyState.Rest);
        Assert.AreEqual (n.get(), 11113421);
    }
    
    static void TestMethod3 () throws Exception {
        AtomicReference<String> s = new AtomicReference<>("");
        SMLiteBuilder<MyState, MyTrigger> _smb = new SMLiteBuilder ();
        _smb.Configure (MyState.Rest)
                .WhenFunc (MyTrigger.Run, () -> { s.set("WhenFunc_Run"); return MyState.Ready; })
				.WhenFunc (MyTrigger.Read, (Object[] _p) -> { s.set((String)_p[0]); return MyState.Ready; })
				.WhenFunc (MyTrigger.FinishRead, (Object[] _p) -> { s.set(String.format("%s%d", (String)_p[0], (int)_p[1])); return MyState.Ready; })
				.WhenAction (MyTrigger.Close, () -> s.set("WhenAction_Close"))
				.WhenAction (MyTrigger.Write, (Object[] _p) -> s.set((String)_p[0]))
				.WhenAction (MyTrigger.FinishWrite, (Object[] _p) -> s.set(String.format("%s%d", (String)_p[0], (int)_p[1])));
        _smb.Configure (MyState.Ready)
                .WhenChangeTo (MyTrigger.Close, MyState.Rest);

        SMLite<MyState, MyTrigger> _sm = _smb.Build (MyState.Rest);
        Assert.AreEqual (_sm.GetState(), MyState.Rest);
        Assert.AreEqual (s.get(), "");

        _sm.Triggering (MyTrigger.Run);
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (s.get(), "WhenFunc_Run");
        _sm.Triggering (MyTrigger.Close);

        _sm.Triggering (MyTrigger.Read, "hello");
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (s.get(), "hello");
        _sm.Triggering (MyTrigger.Close);

        _sm.Triggering (MyTrigger.FinishRead, "hello", 1);
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (s.get(), "hello1");
        _sm.Triggering (MyTrigger.Close);

        _sm.Triggering (MyTrigger.Close);
        Assert.AreEqual (s.get(), "WhenAction_Close");
        Assert.AreEqual (_sm.GetState(), MyState.Rest);

        _sm.Triggering (MyTrigger.Write, "world");
        Assert.AreEqual (s.get(), "world");
        Assert.AreEqual (_sm.GetState(), MyState.Rest);

        _sm.Triggering (MyTrigger.FinishWrite, "world", 1);
        Assert.AreEqual (s.get(), "world1");
        Assert.AreEqual (_sm.GetState(), MyState.Rest);
    }

    static void TestMethod5 () throws Exception {
        AtomicReference<String> s = new AtomicReference<>("");
        SMLiteBuilder<MyState, MyTrigger> _smb = new SMLiteBuilder ();
        _smb.Configure (MyState.Rest)
                .WhenFunc_s (MyTrigger.Run, (MyState _state) -> { s.set("WhenFunc_Run"); return MyState.Ready; })
				.WhenFunc_s (MyTrigger.Read, (MyState _state, Object[] _p) -> { s.set((String)_p[0]); return MyState.Ready; })
				.WhenFunc_s (MyTrigger.FinishRead, (MyState _state, Object[] _p) -> { s.set(String.format("%s%d", (String)_p[0], (int)_p[1])); return MyState.Ready; })
				.WhenAction_s (MyTrigger.Close, (MyState _state) -> s.set("WhenAction_Close"))
				.WhenAction_s (MyTrigger.Write, (MyState _state, Object[] _p) -> s.set((String)_p[0]))
				.WhenAction_s (MyTrigger.FinishWrite, (MyState _state, Object[] _p) -> s.set(String.format("%s%d", (String)_p[0], (int)_p[1])));
        _smb.Configure (MyState.Ready)
                .WhenChangeTo (MyTrigger.Close, MyState.Rest);

        SMLite<MyState, MyTrigger> _sm = _smb.Build (MyState.Rest);
        Assert.AreEqual (_sm.GetState(), MyState.Rest);
        Assert.AreEqual (s.get(), "");

        _sm.Triggering (MyTrigger.Run);
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (s.get(), "WhenFunc_Run");
        _sm.Triggering (MyTrigger.Close);

        _sm.Triggering (MyTrigger.Read, "hello");
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (s.get(), "hello");
        _sm.Triggering (MyTrigger.Close);

        _sm.Triggering (MyTrigger.FinishRead, "hello", 1);
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (s.get(), "hello1");
        _sm.Triggering (MyTrigger.Close);

        _sm.Triggering (MyTrigger.Close);
        Assert.AreEqual (s.get(), "WhenAction_Close");
        Assert.AreEqual (_sm.GetState(), MyState.Rest);

        _sm.Triggering (MyTrigger.Write, "world");
        Assert.AreEqual (s.get(), "world");
        Assert.AreEqual (_sm.GetState(), MyState.Rest);

        _sm.Triggering (MyTrigger.FinishWrite, "world", 1);
        Assert.AreEqual (s.get(), "world1");
        Assert.AreEqual (_sm.GetState(), MyState.Rest);
    }

    static void TestMethod7 () throws Exception {
        AtomicReference<String> s = new AtomicReference<>("");
        SMLiteBuilder<MyState, MyTrigger> _smb = new SMLiteBuilder ();
        _smb.Configure (MyState.Rest)
                .WhenFunc_t (MyTrigger.Run, (MyTrigger _trigger) -> { s.set("WhenFunc_Run"); return MyState.Ready; })
				.WhenFunc_t (MyTrigger.Read, (MyTrigger _trigger, Object[] _p) -> { s.set((String)_p[0]); return MyState.Ready; })
				.WhenFunc_t (MyTrigger.FinishRead, (MyTrigger _trigger, Object[] _p) -> { s.set(String.format("%s%d", (String)_p[0], (int)_p[1])); return MyState.Ready; })
				.WhenAction_t (MyTrigger.Close, (MyTrigger _trigger) -> s.set("WhenAction_Close"))
				.WhenAction_t (MyTrigger.Write, (MyTrigger _trigger, Object[] _p) -> s.set((String)_p[0]))
				.WhenAction_t (MyTrigger.FinishWrite, (MyTrigger _trigger, Object[] _p) -> s.set(String.format("%s%d", (String)_p[0], (int)_p[1])));
        _smb.Configure (MyState.Ready)
                .WhenChangeTo (MyTrigger.Close, MyState.Rest);

        SMLite<MyState, MyTrigger> _sm = _smb.Build (MyState.Rest);
        Assert.AreEqual (_sm.GetState(), MyState.Rest);
        Assert.AreEqual (s.get(), "");

        _sm.Triggering (MyTrigger.Run);
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (s.get(), "WhenFunc_Run");
        _sm.Triggering (MyTrigger.Close);

        _sm.Triggering (MyTrigger.Read, "hello");
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (s.get(), "hello");
        _sm.Triggering (MyTrigger.Close);

        _sm.Triggering (MyTrigger.FinishRead, "hello", 1);
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (s.get(), "hello1");
        _sm.Triggering (MyTrigger.Close);

        _sm.Triggering (MyTrigger.Close);
        Assert.AreEqual (s.get(), "WhenAction_Close");
        Assert.AreEqual (_sm.GetState(), MyState.Rest);

        _sm.Triggering (MyTrigger.Write, "world");
        Assert.AreEqual (s.get(), "world");
        Assert.AreEqual (_sm.GetState(), MyState.Rest);

        _sm.Triggering (MyTrigger.FinishWrite, "world", 1);
        Assert.AreEqual (s.get(), "world1");
        Assert.AreEqual (_sm.GetState(), MyState.Rest);
    }

    static void TestMethod9 () throws Exception {
        AtomicReference<String> s = new AtomicReference<>("");
        SMLiteBuilder<MyState, MyTrigger> _smb = new SMLiteBuilder ();
        _smb.Configure (MyState.Rest)
                .WhenFunc_st (MyTrigger.Run, (MyState _state, MyTrigger _trigger) -> { s.set("WhenFunc_Run"); return MyState.Ready; })
				.WhenFunc_st (MyTrigger.Read, (MyState _state, MyTrigger _trigger, Object[] _p) -> { s.set((String)_p[0]); return MyState.Ready; })
				.WhenFunc_st (MyTrigger.FinishRead, (MyState _state, MyTrigger _trigger, Object[] _p) -> { s.set(String.format("%s%d", (String)_p[0], (int)_p[1])); return MyState.Ready; })
				.WhenAction_st (MyTrigger.Close, (MyState _state, MyTrigger _trigger) -> s.set("WhenAction_Close"))
				.WhenAction_st (MyTrigger.Write, (MyState _state, MyTrigger _trigger, Object[] _p) -> s.set((String)_p[0]))
				.WhenAction_st (MyTrigger.FinishWrite, (MyState _state, MyTrigger _trigger, Object[] _p) -> s.set(String.format("%s%d", (String)_p[0], (int)_p[1])));
        _smb.Configure (MyState.Ready)
                .WhenChangeTo (MyTrigger.Close, MyState.Rest);

        SMLite<MyState, MyTrigger> _sm = _smb.Build (MyState.Rest);
        Assert.AreEqual (_sm.GetState(), MyState.Rest);
        Assert.AreEqual (s.get(), "");

        _sm.Triggering (MyTrigger.Run);
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (s.get(), "WhenFunc_Run");
        _sm.Triggering (MyTrigger.Close);

        _sm.Triggering (MyTrigger.Read, "hello");
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (s.get(), "hello");
        _sm.Triggering (MyTrigger.Close);

        _sm.Triggering (MyTrigger.FinishRead, "hello", 1);
        Assert.AreEqual (_sm.GetState(), MyState.Ready);
        Assert.AreEqual (s.get(), "hello1");
        _sm.Triggering (MyTrigger.Close);

        _sm.Triggering (MyTrigger.Close);
        Assert.AreEqual (s.get(), "WhenAction_Close");
        Assert.AreEqual (_sm.GetState(), MyState.Rest);

        _sm.Triggering (MyTrigger.Write, "world");
        Assert.AreEqual (s.get(), "world");
        Assert.AreEqual (_sm.GetState(), MyState.Rest);

        _sm.Triggering (MyTrigger.FinishWrite, "world", 1);
        Assert.AreEqual (s.get(), "world1");
        Assert.AreEqual (_sm.GetState(), MyState.Rest);
    }

    static void TestMethod11 () throws Exception {
        AtomicInteger n = new AtomicInteger(0);
        AtomicBoolean entry_one = new AtomicBoolean(true);
        SMLiteBuilder<Tuple2<MyState, MyState>, MyTrigger> _smb = new SMLiteBuilder ();
        _smb.Configure (new Tuple2 (MyState.Rest, MyState.Rest))
                .OnEntry (() -> { Assert.IsFalse (entry_one.get()); entry_one.set(true); n.addAndGet(1); })
                .OnLeave (() -> { Assert.IsTrue (entry_one.get()); entry_one.set(false); n.addAndGet(10); })
                .WhenChangeTo (MyTrigger.Run, new Tuple2 (MyState.Ready, MyState.Ready))
                .WhenIgnore (MyTrigger.Close);
        _smb.Configure (new Tuple2 (MyState.Ready, MyState.Ready))
                .OnEntry (() -> { Assert.IsFalse (entry_one.get()); entry_one.set(true); n.addAndGet(100); })
                .OnLeave (() -> { Assert.IsTrue (entry_one.get()); entry_one.set(false); n.addAndGet(1000); })
                .WhenChangeTo (MyTrigger.Read, new Tuple2 (MyState.Reading, MyState.Reading))
                .WhenChangeTo (MyTrigger.Write, new Tuple2 (MyState.Writing, MyState.Writing))
                .WhenChangeTo (MyTrigger.Close, new Tuple2 (MyState.Rest, MyState.Rest));
        _smb.Configure (new Tuple2 (MyState.Reading, MyState.Reading))
                .OnEntry (() -> { Assert.IsFalse (entry_one.get()); entry_one.set(true); n.addAndGet(10000); })
                .OnLeave (() -> { Assert.IsTrue (entry_one.get()); entry_one.set(false); n.addAndGet(100000); })
                .WhenChangeTo (MyTrigger.FinishRead, new Tuple2 (MyState.Ready, MyState.Ready))
                .WhenChangeTo (MyTrigger.Close, new Tuple2 (MyState.Rest, MyState.Rest));
        _smb.Configure (new Tuple2 (MyState.Writing, MyState.Writing))
                .OnEntry (() -> { Assert.IsFalse (entry_one.get()); entry_one.set(true); n.addAndGet(1000000); })
                .OnLeave (() -> { Assert.IsTrue (entry_one.get()); entry_one.set(false); n.addAndGet(10000000); })
                .WhenChangeTo (MyTrigger.FinishWrite, new Tuple2 (MyState.Ready, MyState.Ready))
                .WhenChangeTo (MyTrigger.Close, new Tuple2 (MyState.Rest, MyState.Rest));

        SMLite<Tuple2<MyState, MyState>, MyTrigger> _sm = _smb.Build (new Tuple2 (MyState.Rest, MyState.Rest));
        Assert.AreEqual (_sm.GetState (), new Tuple2 (MyState.Rest, MyState.Rest));
        Assert.AreEqual (n.get(), 0);
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Run));
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Read));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Write));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite));

        _sm.Triggering (MyTrigger.Close);
        Assert.AreEqual (_sm.GetState(), new Tuple2 (MyState.Rest, MyState.Rest));
        Assert.AreEqual (n.get(), 0);

        _sm.Triggering (MyTrigger.Close);
        Assert.AreEqual (_sm.GetState(), new Tuple2 (MyState.Rest, MyState.Rest));
        Assert.AreEqual (n.get(), 0);

        _sm.Triggering (MyTrigger.Close);
        Assert.AreEqual (_sm.GetState(), new Tuple2 (MyState.Rest, MyState.Rest));
        Assert.AreEqual (n.get(), 0);

        _sm.Triggering (MyTrigger.Run);
        Assert.AreEqual (_sm.GetState(), new Tuple2 (MyState.Ready, MyState.Ready));
        Assert.AreEqual (n.get(), 110);
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Run));
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close));
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Read));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead));
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Write));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite));

        _sm.Triggering (MyTrigger.Close);
        Assert.AreEqual (_sm.GetState(), new Tuple2 (MyState.Rest, MyState.Rest));
        Assert.AreEqual (n.get(), 1111);
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Run));
        Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Read));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Write));
        Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite));

        _sm.Triggering (MyTrigger.Run);
        Assert.AreEqual (_sm.GetState(), new Tuple2 (MyState.Ready, MyState.Ready));
        Assert.AreEqual (n.get(), 1221);

        _sm.Triggering (MyTrigger.Read);
        Assert.AreEqual (_sm.GetState(), new Tuple2 (MyState.Reading, MyState.Reading));
        Assert.AreEqual (n.get(), 12221);

        _sm.Triggering (MyTrigger.FinishRead);
        Assert.AreEqual (_sm.GetState(), new Tuple2 (MyState.Ready, MyState.Ready));
        Assert.AreEqual (n.get(), 112321);

        _sm.Triggering (MyTrigger.Write);
        Assert.AreEqual (_sm.GetState(), new Tuple2 (MyState.Writing, MyState.Writing));
        Assert.AreEqual (n.get(), 1113321);

        _sm.Triggering (MyTrigger.FinishWrite);
        Assert.AreEqual (_sm.GetState(), new Tuple2 (MyState.Ready, MyState.Ready));
        Assert.AreEqual (n.get(), 11113421);

        _sm.SetState(new Tuple2 (MyState.Reading, MyState.Reading));
        _sm.SetState(new Tuple2 (MyState.Writing, MyState.Writing));
        _sm.SetState(new Tuple2 (MyState.Rest, MyState.Rest));
        Assert.AreEqual (n.get(), 11113421);
    }

    public static void main(String[] args) throws Exception {
        TestMethod1 ();
        TestMethod3 ();
        TestMethod5 ();
        TestMethod7 ();
        TestMethod9 ();
        System.out.println ("Test Success");
    }
}
