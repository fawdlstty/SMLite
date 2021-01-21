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
			_sm.Configure (MyState.Ready)
				.OnEntry (() => Console.WriteLine ("entry Ready"))
				.OnLeave (() => Console.WriteLine ("leave Ready"))
				.WhenChangeTo (MyTrigger.Read, MyState.Reading)
				.WhenChangeTo (MyTrigger.Write, MyState.Writing)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);
			_sm.Configure (MyState.Reading)
				.OnEntry(() => Console.WriteLine ("entry Reading"))
				.OnLeave (() => Console.WriteLine ("leave Reading"))
				.WhenChangeTo (MyTrigger.FinishRead, MyState.Ready)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);
			_sm.Configure (MyState.Writing)
				.OnEntry (() => Console.WriteLine ("entry Writing"))
				.OnLeave (() => Console.WriteLine ("leave Writing"))
				.WhenChangeTo (MyTrigger.FinishWrite, MyState.Ready)
				.WhenChangeTo (MyTrigger.Close, MyState.Rest);

			assert (_sm.State == MyState.Rest);
			assert (_sm.AllowTriggering (MyTrigger.Run));
			assert (!_sm.AllowTriggering (MyTrigger.Read));
			assert (!_sm.AllowTriggering (MyTrigger.FinishRead));
			assert (!_sm.AllowTriggering (MyTrigger.Write));
			assert (!_sm.AllowTriggering (MyTrigger.FinishWrite));
			assert (_sm.AllowTriggering (MyTrigger.Close));

			_sm.Triggering (MyTrigger.Run, "hello");
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
