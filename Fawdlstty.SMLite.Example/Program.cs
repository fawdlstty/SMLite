using System;
using System.Threading.Tasks;
using Fawdlstty.SMLite;

namespace Fawdlstty.SMLite.Example {
	enum MyState { Rest, Ready, Reading, Writing };
	enum MyTrigger { Run, Close, Read, FinishRead, Write, FinishWrite };

	class Program {
        static void Main (string[] args) {
			var _sm = new SMLite<MyState, MyTrigger> (MyState.Rest);
			_sm.Configure (MyState.Rest)
				.OnEntry (() => Console.WriteLine ("entry Rest"))
				.OnLeave (() => Console.WriteLine ("leave Rest"))
				.WhenChangeTo (MyTrigger.Run, MyState.Ready)
				.WhenIgnore (MyTrigger.Close);
			_sm.Configure (MyState.Ready)
				.WhenChangeTo (MyTrigger.Read, MyState.Reading)
				.WhenChangeTo (MyTrigger.Write, MyState.Writing)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);
			_sm.Configure (MyState.Reading)
				.WhenChangeTo (MyTrigger.FinishRead, MyState.Ready)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);
			_sm.Configure (MyState.Writing)
				.WhenChangeTo (MyTrigger.FinishWrite, MyState.Ready)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);

			assert (_sm.State == MyState.Rest);
			assert (_sm.AllowTriggering (MyTrigger.Run));
			assert (!_sm.AllowTriggering (MyTrigger.Read));
			assert (!_sm.AllowTriggering (MyTrigger.FinishRead));
			assert (!_sm.AllowTriggering (MyTrigger.Write));
			assert (!_sm.AllowTriggering (MyTrigger.FinishWrite));
			assert (_sm.AllowTriggering (MyTrigger.Close));

			_sm.Triggering (MyTrigger.Run);
			assert (_sm.State == MyState.Ready);

			_sm.Triggering (MyTrigger.Read);
			assert (_sm.State == MyState.Reading);

			_sm.Triggering (MyTrigger.FinishRead);
			assert (_sm.State == MyState.Ready);

			_sm.Triggering (MyTrigger.Write);
			assert (_sm.State == MyState.Writing);

			_sm.Triggering (MyTrigger.FinishWrite);
			assert (_sm.State == MyState.Ready);

			_sm.Triggering (MyTrigger.Close);
			assert (_sm.State == MyState.Rest);

			Console.WriteLine ("Hello World!");
			Console.ReadKey ();
		}

        private static void assert (bool v) {
			if (!v)
				throw new Exception ("error");
        }
    }
}
