using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Fawdlstty.SMLite.ItemStruct {
    [Flags]
    enum _SMLite_BuildItem {
        None = 0,
        State = 1,
        Trigger = 2,
        CancellationToken = 4,
	}

    class _SMLite_ConfigItem<TState, TTrigger> {
        internal _SMLite_ConfigItem (_SMLite_BuildItem build_item, TState state, TTrigger trigger, object callback, MethodInfo callback_info) {
            BuildItem = build_item;
            State = state;
            Trigger = trigger;
            Callback = callback;
            CallbackInfo = callback_info;
        }

        internal TState _call (params object[] args) {
            var _v = new List<object> ();
            if ((BuildItem & _SMLite_BuildItem.State) > 0)
                _v.Add (State);
            if ((BuildItem & _SMLite_BuildItem.Trigger) > 0)
                _v.Add (Trigger);
            if (args?.Length > 0)
                _v.AddRange (args);
            var _state = Callback.GetType ().InvokeMember ("Invoke", BindingFlags.InvokeMethod, null, Callback, _v.ToArray ());
            return _state != null ? (TState) _state : State;
        }

        internal async Task<TState> _call_async (CancellationToken token, params object[] args) {
            var _v = new List<object> ();
            if ((BuildItem & _SMLite_BuildItem.State) > 0)
                _v.Add (State);
            if ((BuildItem & _SMLite_BuildItem.Trigger) > 0)
                _v.Add (Trigger);
            if ((BuildItem & _SMLite_BuildItem.CancellationToken) > 0) {
                _v.Add (token);
            } else if (token != CancellationToken.None) {
                throw new Exception ("State machine method does not support the CancellationToken parameter, but the parameter 'token' is not 'CancellationToken.None'");
			}
            if (args?.Length > 0)
                _v.AddRange (args);
            var _state = Callback.GetType ().InvokeMember ("Invoke", BindingFlags.InvokeMethod, null, Callback, _v.ToArray ());
            if (_state is Task<TState> _t0) {
                return await _t0;
            } else if (_state is Task _t1) {
                await _t1;
                return State;
            } else {
                return _state != null ? (TState) _state : State;
            }
        }

        internal _SMLite_BuildItem BuildItem { get; private set; }
        internal TState State { get; private set; }
        internal TTrigger Trigger { get; private set; }
        internal object Callback { get; private set; }
        internal MethodInfo CallbackInfo { get; private set; }
    }
}
