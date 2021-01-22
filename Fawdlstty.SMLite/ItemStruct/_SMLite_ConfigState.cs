using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fawdlstty.SMLite.ItemStruct {
	public class _SMLite_ConfigState<TState, TTrigger> {
		private _SMLite_ConfigState<TState, TTrigger> _try_add_trigger (TTrigger _trigger, _SMLite_ConfigItem<TState, TTrigger> _item) {
			if (m_items.ContainsKey (_trigger))
				throw new Exception ("state is already has this trigger methods.");
			m_items[_trigger] = _item;
			return this;
        }

		public _SMLite_ConfigState<TState, TTrigger> WhenChangeTo (TTrigger trigger, TState new_state) {
			Func<TState, TTrigger, TState> callback = (_state, _trigger) => new_state;
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> WhenIgnore (TTrigger trigger) {
			Action<TState, TTrigger> callback = (_state, _trigger) => {};
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}

		#region Synchronized
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc (TTrigger trigger, Func<TState, TTrigger, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1> (TTrigger trigger, Func<TState, TTrigger, T1, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1, T2> (TTrigger trigger, Func<TState, TTrigger, T1, T2, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1, T2, T3> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1, T2, T3, T4> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1, T2, T3, T4, T5, T6> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, T6, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1, T2, T3, T4, T5, T6, T7> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, T6, T7, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1, T2, T3, T4, T5, T6, T7, T8> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, T6, T7, T8, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, T6, T7, T8, T9, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> WhenAction (TTrigger trigger, Action<TState, TTrigger> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1> (TTrigger trigger, Action<TState, TTrigger, T1> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1, T2> (TTrigger trigger, Action<TState, TTrigger, T1, T2> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1, T2, T3> (TTrigger trigger, Action<TState, TTrigger, T1, T2, T3> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1, T2, T3, T4> (TTrigger trigger, Action<TState, TTrigger, T1, T2, T3, T4> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1, T2, T3, T4, T5> (TTrigger trigger, Action<TState, TTrigger, T1, T2, T3, T4, T5> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1, T2, T3, T4, T5, T6> (TTrigger trigger, Action<TState, TTrigger, T1, T2, T3, T4, T5, T6> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1, T2, T3, T4, T5, T6, T7> (TTrigger trigger, Action<TState, TTrigger, T1, T2, T3, T4, T5, T6, T7> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1, T2, T3, T4, T5, T6, T7, T8> (TTrigger trigger, Action<TState, TTrigger, T1, T2, T3, T4, T5, T6, T7, T8> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> (TTrigger trigger, Action<TState, TTrigger, T1, T2, T3, T4, T5, T6, T7, T8, T9> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> OnEntry (Action callback) {
			if (m_on_entry != null || m_on_entry_async != null)
				throw new Exception ("OnEntry is already have been set.");
			m_on_entry = callback;
			return this;
		}

		public _SMLite_ConfigState<TState, TTrigger> OnLeave (Action callback) {
			if (m_on_leave != null || m_on_leave_async != null)
				throw new Exception ("OnLeave is already have been set.");
			m_on_leave = callback;
			return this;
		}
		#endregion

		#region Asynchronized
		public _SMLite_ConfigState<TState, TTrigger> WhenFuncAsync (TTrigger trigger, Func<TState, TTrigger, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFuncAsync<T1> (TTrigger trigger, Func<TState, TTrigger, T1, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFuncAsync<T1, T2> (TTrigger trigger, Func<TState, TTrigger, T1, T2, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFuncAsync<T1, T2, T3> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFuncAsync<T1, T2, T3, T4> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFuncAsync<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFuncAsync<T1, T2, T3, T4, T5, T6> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, T6, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFuncAsync<T1, T2, T3, T4, T5, T6, T7> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, T6, T7, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFuncAsync<T1, T2, T3, T4, T5, T6, T7, T8> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, T6, T7, T8, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFuncAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> WhenActionAsync (TTrigger trigger, Func<TState, TTrigger, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenActionAsync<T1> (TTrigger trigger, Func<TState, TTrigger, T1, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenActionAsync<T1, T2> (TTrigger trigger, Func<TState, TTrigger, T1, T2, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenActionAsync<T1, T2, T3> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenActionAsync<T1, T2, T3, T4> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenActionAsync<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenActionAsync<T1, T2, T3, T4, T5, T6> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, T6, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenActionAsync<T1, T2, T3, T4, T5, T6, T7> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, T6, T7, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenActionAsync<T1, T2, T3, T4, T5, T6, T7, T8> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, T6, T7, T8, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenActionAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> { State = State, Trigger = trigger, Callback = callback, CallbackInfo = callback.Method };
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> OnEntryAsync (Func<Task> callback) {
			if (m_on_entry != null || m_on_entry_async != null)
				throw new Exception ("OnEntry is already have been set.");
			m_on_entry_async = callback;
			return this;
		}

		public _SMLite_ConfigState<TState, TTrigger> OnLeaveAsync (Func<Task> callback) {
			if (m_on_leave != null || m_on_leave_async != null)
				throw new Exception ("OnLeave is already have been set.");
			m_on_leave_async = callback;
			return this;
		}
		#endregion

		internal bool _allow_trigger (TTrigger trigger) => m_items.ContainsKey (trigger);

		internal async Task<TState> _trigger_async (TTrigger trigger, params object[] args) {
			if (_allow_trigger (trigger))
				return await m_items[trigger]._call_async (args);
			throw new Exception ("not match function found.");
		}

		internal Action m_on_entry = null, m_on_leave = null;
		internal Func<Task> m_on_entry_async = null, m_on_leave_async = null;
		internal TState State { get; set; }
		Dictionary<TTrigger, _SMLite_ConfigItem<TState, TTrigger>> m_items = new Dictionary<TTrigger, _SMLite_ConfigItem<TState, TTrigger>> ();
	}
}
