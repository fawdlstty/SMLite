using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fawdlstty.SMLite.ItemStruct {
    class _SMLite_ConfigItem<TState, TTrigger> {
        internal async Task<TState> _call_async (params object[] args) {
            var _v = new List<object> () { State, Trigger };
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

        internal TState State { get; init; }
        internal TTrigger Trigger { get; init; }
        internal object Callback { get; init; }
        internal MethodInfo CallbackInfo { get; init; }
    }
}
