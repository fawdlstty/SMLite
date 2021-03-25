using System;
using System.Collections.Generic;

namespace Fawdlstty.SMLite.ItemStruct {
	public class _SMLite_ConfigState<TState, TTrigger> {
		private _SMLite_ConfigState<TState, TTrigger> _try_add_trigger (TTrigger _trigger, _SMLite_ConfigItem<TState, TTrigger> _item) {
			if (m_items.ContainsKey (_trigger))
				throw new Exception ("state is already has this trigger methods.");
			m_items[_trigger] = _item;
			return this;
        }

		public _SMLite_ConfigState<TState, TTrigger> WhenChangeTo (TTrigger trigger, TState new_state) {
			Func<TState> callback = () => new_state;
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> WhenIgnore (TTrigger trigger) {
			Action callback = () => {};
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> WhenFunc (TTrigger trigger, Func<TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1> (TTrigger trigger, Func<T1, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1, T2> (TTrigger trigger, Func<T1, T2, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1, T2, T3> (TTrigger trigger, Func<T1, T2, T3, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1, T2, T3, T4> (TTrigger trigger, Func<T1, T2, T3, T4, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc<T1, T2, T3, T4, T5> (TTrigger trigger, Func<T1, T2, T3, T4, T5, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_s (TTrigger trigger, Func<TState, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_s<T1> (TTrigger trigger, Func<TState, T1, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_s<T1, T2> (TTrigger trigger, Func<TState, T1, T2, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_s<T1, T2, T3> (TTrigger trigger, Func<TState, T1, T2, T3, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_s<T1, T2, T3, T4> (TTrigger trigger, Func<TState, T1, T2, T3, T4, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_s<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TState, T1, T2, T3, T4, T5, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_t (TTrigger trigger, Func<TTrigger, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_t<T1> (TTrigger trigger, Func<TTrigger, T1, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_t<T1, T2> (TTrigger trigger, Func<TTrigger, T1, T2, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_t<T1, T2, T3> (TTrigger trigger, Func<TTrigger, T1, T2, T3, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_t<T1, T2, T3, T4> (TTrigger trigger, Func<TTrigger, T1, T2, T3, T4, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_t<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TTrigger, T1, T2, T3, T4, T5, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_st (TTrigger trigger, Func<TState, TTrigger, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_st<T1> (TTrigger trigger, Func<TState, TTrigger, T1, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_st<T1, T2> (TTrigger trigger, Func<TState, TTrigger, T1, T2, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_st<T1, T2, T3> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_st<T1, T2, T3, T4> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenFunc_st<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> WhenAction (TTrigger trigger, Action callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1> (TTrigger trigger, Action<T1> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1, T2> (TTrigger trigger, Action<T1, T2> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1, T2, T3> (TTrigger trigger, Action<T1, T2, T3> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1, T2, T3, T4> (TTrigger trigger, Action<T1, T2, T3, T4> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction<T1, T2, T3, T4, T5> (TTrigger trigger, Action<T1, T2, T3, T4, T5> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> WhenAction_s (TTrigger trigger, Action<TState> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_s<T1> (TTrigger trigger, Action<TState, T1> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_s<T1, T2> (TTrigger trigger, Action<TState, T1, T2> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_s<T1, T2, T3> (TTrigger trigger, Action<TState, T1, T2, T3> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_s<T1, T2, T3, T4> (TTrigger trigger, Action<TState, T1, T2, T3, T4> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_s<T1, T2, T3, T4, T5> (TTrigger trigger, Action<TState, T1, T2, T3, T4, T5> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> WhenAction_t (TTrigger trigger, Action<TTrigger> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_t<T1> (TTrigger trigger, Action<TTrigger, T1> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_t<T1, T2> (TTrigger trigger, Action<TTrigger, T1, T2> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_t<T1, T2, T3> (TTrigger trigger, Action<TTrigger, T1, T2, T3> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_t<T1, T2, T3, T4> (TTrigger trigger, Action<TTrigger, T1, T2, T3, T4> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_t<T1, T2, T3, T4, T5> (TTrigger trigger, Action<TTrigger, T1, T2, T3, T4, T5> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> WhenAction_st (TTrigger trigger, Action<TState, TTrigger> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_st<T1> (TTrigger trigger, Action<TState, TTrigger, T1> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_st<T1, T2> (TTrigger trigger, Action<TState, TTrigger, T1, T2> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_st<T1, T2, T3> (TTrigger trigger, Action<TState, TTrigger, T1, T2, T3> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_st<T1, T2, T3, T4> (TTrigger trigger, Action<TState, TTrigger, T1, T2, T3, T4> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigState<TState, TTrigger> WhenAction_st<T1, T2, T3, T4, T5> (TTrigger trigger, Action<TState, TTrigger, T1, T2, T3, T4, T5> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigState<TState, TTrigger> OnEntry (Action callback) {
			if (m_on_entry != null)
				throw new Exception ("OnEntry is already have been set.");
			m_on_entry = callback;
			return this;
		}

		public _SMLite_ConfigState<TState, TTrigger> OnLeave (Action callback) {
			if (m_on_leave != null)
				throw new Exception ("OnLeave is already have been set.");
			m_on_leave = callback;
			return this;
		}

		internal bool _allow_trigger (TTrigger trigger) => m_items.ContainsKey (trigger);

		internal TState _trigger (TTrigger trigger, params object[] args) {
			if (_allow_trigger (trigger))
				return m_items[trigger]._call (args);
			throw new Exception ("not match function found.");
		}

		internal Action m_on_entry = null, m_on_leave = null;
		internal TState State { get; set; }
		Dictionary<TTrigger, _SMLite_ConfigItem<TState, TTrigger>> m_items = new Dictionary<TTrigger, _SMLite_ConfigItem<TState, TTrigger>> ();
	}
}
