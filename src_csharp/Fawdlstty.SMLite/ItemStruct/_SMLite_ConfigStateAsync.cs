using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fawdlstty.SMLite.ItemStruct {
	public class _SMLite_ConfigStateAsync<TState, TTrigger> {
		private _SMLite_ConfigStateAsync<TState, TTrigger> _try_add_trigger (TTrigger _trigger, _SMLite_ConfigItem<TState, TTrigger> _item) {
			if (m_items.ContainsKey (_trigger))
				throw new Exception ("state is already has this trigger methods.");
			m_items[_trigger] = _item;
			return this;
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenChangeTo (TTrigger trigger, TState new_state) {
			Func<TState> callback = () => new_state;
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenIgnore (TTrigger trigger) {
			Action callback = () => {};
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		#region has_cancellationtoken
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync (TTrigger trigger, Func<Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync<T1> (TTrigger trigger, Func<T1, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync<T1, T2> (TTrigger trigger, Func<T1, T2, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync<T1, T2, T3> (TTrigger trigger, Func<T1, T2, T3, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync<T1, T2, T3, T4> (TTrigger trigger, Func<T1, T2, T3, T4, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync<T1, T2, T3, T4, T5> (TTrigger trigger, Func<T1, T2, T3, T4, T5, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_s (TTrigger trigger, Func<TState, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_s<T1> (TTrigger trigger, Func<TState, T1, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_s<T1, T2> (TTrigger trigger, Func<TState, T1, T2, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_s<T1, T2, T3> (TTrigger trigger, Func<TState, T1, T2, T3, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_s<T1, T2, T3, T4> (TTrigger trigger, Func<TState, T1, T2, T3, T4, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_s<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TState, T1, T2, T3, T4, T5, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_t (TTrigger trigger, Func<TTrigger, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_t<T1> (TTrigger trigger, Func<TTrigger, T1, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_t<T1, T2> (TTrigger trigger, Func<TTrigger, T1, T2, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_t<T1, T2, T3> (TTrigger trigger, Func<TTrigger, T1, T2, T3, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_t<T1, T2, T3, T4> (TTrigger trigger, Func<TTrigger, T1, T2, T3, T4, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_t<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TTrigger, T1, T2, T3, T4, T5, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_st (TTrigger trigger, Func<TState, TTrigger, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_st<T1> (TTrigger trigger, Func<TState, TTrigger, T1, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_st<T1, T2> (TTrigger trigger, Func<TState, TTrigger, T1, T2, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_st<T1, T2, T3> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_st<T1, T2, T3, T4> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_st<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync (TTrigger trigger, Func<Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync<T1> (TTrigger trigger, Func<T1, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync<T1, T2> (TTrigger trigger, Func<T1, T2, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync<T1, T2, T3> (TTrigger trigger, Func<T1, T2, T3, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync<T1, T2, T3, T4> (TTrigger trigger, Func<T1, T2, T3, T4, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync<T1, T2, T3, T4, T5> (TTrigger trigger, Func<T1, T2, T3, T4, T5, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_s (TTrigger trigger, Func<TState, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_s<T1> (TTrigger trigger, Func<TState, T1, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_s<T1, T2> (TTrigger trigger, Func<TState, T1, T2, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_s<T1, T2, T3> (TTrigger trigger, Func<TState, T1, T2, T3, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_s<T1, T2, T3, T4> (TTrigger trigger, Func<TState, T1, T2, T3, T4, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_s<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TState, T1, T2, T3, T4, T5, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_t (TTrigger trigger, Func<TTrigger, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_t<T1> (TTrigger trigger, Func<TTrigger, T1, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_t<T1, T2> (TTrigger trigger, Func<TTrigger, T1, T2, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_t<T1, T2, T3> (TTrigger trigger, Func<TTrigger, T1, T2, T3, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_t<T1, T2, T3, T4> (TTrigger trigger, Func<TTrigger, T1, T2, T3, T4, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_t<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TTrigger, T1, T2, T3, T4, T5, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_st (TTrigger trigger, Func<TState, TTrigger, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_st<T1> (TTrigger trigger, Func<TState, TTrigger, T1, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_st<T1, T2> (TTrigger trigger, Func<TState, TTrigger, T1, T2, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_st<T1, T2, T3> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_st<T1, T2, T3, T4> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_st<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TState, TTrigger, T1, T2, T3, T4, T5, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		#endregion

		#region no_cancellationtoken
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync (TTrigger trigger, Func<CancellationToken, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync<T1> (TTrigger trigger, Func<CancellationToken, T1, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync<T1, T2> (TTrigger trigger, Func<CancellationToken, T1, T2, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync<T1, T2, T3> (TTrigger trigger, Func<CancellationToken, T1, T2, T3, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync<T1, T2, T3, T4> (TTrigger trigger, Func<CancellationToken, T1, T2, T3, T4, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync<T1, T2, T3, T4, T5> (TTrigger trigger, Func<CancellationToken, T1, T2, T3, T4, T5, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.None | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_s (TTrigger trigger, Func<TState, CancellationToken, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_s<T1> (TTrigger trigger, Func<TState, CancellationToken, T1, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_s<T1, T2> (TTrigger trigger, Func<TState, CancellationToken, T1, T2, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_s<T1, T2, T3> (TTrigger trigger, Func<TState, CancellationToken, T1, T2, T3, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_s<T1, T2, T3, T4> (TTrigger trigger, Func<TState, CancellationToken, T1, T2, T3, T4, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_s<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TState, CancellationToken, T1, T2, T3, T4, T5, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_t (TTrigger trigger, Func<TTrigger, CancellationToken, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_t<T1> (TTrigger trigger, Func<TTrigger, CancellationToken, T1, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_t<T1, T2> (TTrigger trigger, Func<TTrigger, CancellationToken, T1, T2, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_t<T1, T2, T3> (TTrigger trigger, Func<TTrigger, CancellationToken, T1, T2, T3, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_t<T1, T2, T3, T4> (TTrigger trigger, Func<TTrigger, CancellationToken, T1, T2, T3, T4, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_t<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TTrigger, CancellationToken, T1, T2, T3, T4, T5, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_st (TTrigger trigger, Func<TState, TTrigger, CancellationToken, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_st<T1> (TTrigger trigger, Func<TState, TTrigger, CancellationToken, T1, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_st<T1, T2> (TTrigger trigger, Func<TState, TTrigger, CancellationToken, T1, T2, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_st<T1, T2, T3> (TTrigger trigger, Func<TState, TTrigger, CancellationToken, T1, T2, T3, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_st<T1, T2, T3, T4> (TTrigger trigger, Func<TState, TTrigger, CancellationToken, T1, T2, T3, T4, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenFuncAsync_st<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TState, TTrigger, CancellationToken, T1, T2, T3, T4, T5, Task<TState>> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync (TTrigger trigger, Func<CancellationToken, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync<T1> (TTrigger trigger, Func<CancellationToken, T1, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync<T1, T2> (TTrigger trigger, Func<CancellationToken, T1, T2, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync<T1, T2, T3> (TTrigger trigger, Func<CancellationToken, T1, T2, T3, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync<T1, T2, T3, T4> (TTrigger trigger, Func<CancellationToken, T1, T2, T3, T4, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync<T1, T2, T3, T4, T5> (TTrigger trigger, Func<CancellationToken, T1, T2, T3, T4, T5, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_s (TTrigger trigger, Func<TState, CancellationToken, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_s<T1> (TTrigger trigger, Func<TState, CancellationToken, T1, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_s<T1, T2> (TTrigger trigger, Func<TState, CancellationToken, T1, T2, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_s<T1, T2, T3> (TTrigger trigger, Func<TState, CancellationToken, T1, T2, T3, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_s<T1, T2, T3, T4> (TTrigger trigger, Func<TState, CancellationToken, T1, T2, T3, T4, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_s<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TState, CancellationToken, T1, T2, T3, T4, T5, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_t (TTrigger trigger, Func<TTrigger, CancellationToken, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_t<T1> (TTrigger trigger, Func<TTrigger, CancellationToken, T1, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_t<T1, T2> (TTrigger trigger, Func<TTrigger, CancellationToken, T1, T2, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_t<T1, T2, T3> (TTrigger trigger, Func<TTrigger, CancellationToken, T1, T2, T3, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_t<T1, T2, T3, T4> (TTrigger trigger, Func<TTrigger, CancellationToken, T1, T2, T3, T4, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_t<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TTrigger, CancellationToken, T1, T2, T3, T4, T5, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_st (TTrigger trigger, Func<TState, TTrigger, CancellationToken, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_st<T1> (TTrigger trigger, Func<TState, TTrigger, CancellationToken, T1, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_st<T1, T2> (TTrigger trigger, Func<TState, TTrigger, CancellationToken, T1, T2, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_st<T1, T2, T3> (TTrigger trigger, Func<TState, TTrigger, CancellationToken, T1, T2, T3, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_st<T1, T2, T3, T4> (TTrigger trigger, Func<TState, TTrigger, CancellationToken, T1, T2, T3, T4, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		public _SMLite_ConfigStateAsync<TState, TTrigger> WhenActionAsync_st<T1, T2, T3, T4, T5> (TTrigger trigger, Func<TState, TTrigger, CancellationToken, T1, T2, T3, T4, T5, Task> callback) {
			var _item = new _SMLite_ConfigItem<TState, TTrigger> (_SMLite_BuildItem.State | _SMLite_BuildItem.Trigger | _SMLite_BuildItem.CancellationToken, State, trigger, callback, callback.Method);
			return _try_add_trigger (trigger, _item);
		}
		#endregion

		public _SMLite_ConfigStateAsync<TState, TTrigger> OnEntryAsync (Func<Task> callback) {
			if (m_on_entry_async != null || m_on_entry_cancel_async != null)
				throw new Exception ("OnEntry is already have been set.");
			m_on_entry_async = callback;
			return this;
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> OnEntryAsync (Func<CancellationToken, Task> callback) {
			if (m_on_entry_async != null || m_on_entry_cancel_async != null)
				throw new Exception ("OnEntry is already have been set.");
			m_on_entry_cancel_async = callback;
			return this;
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> OnLeaveAsync (Func<Task> callback) {
			if (m_on_leave_async != null || m_on_leave_cancel_async != null)
				throw new Exception ("OnLeave is already have been set.");
			m_on_leave_async = callback;
			return this;
		}

		public _SMLite_ConfigStateAsync<TState, TTrigger> OnLeaveAsync (Func<CancellationToken, Task> callback) {
			if (m_on_leave_async != null || m_on_leave_cancel_async != null)
				throw new Exception ("OnLeave is already have been set.");
			m_on_leave_cancel_async = callback;
			return this;
		}

		internal bool _allow_trigger (TTrigger trigger) => m_items.ContainsKey (trigger);

		internal async Task<TState> _triggerAsync (TTrigger trigger, CancellationToken token, params object[] args) {
			if (_allow_trigger (trigger))
				return await m_items[trigger]._call_async (token, args);
			throw new Exception ("not match function found.");
		}

		internal Func<Task> m_on_entry_async = null, m_on_leave_async = null;
		internal Func<CancellationToken, Task> m_on_entry_cancel_async = null, m_on_leave_cancel_async = null;
		internal TState State { get; set; }
		Dictionary<TTrigger, _SMLite_ConfigItem<TState, TTrigger>> m_items = new Dictionary<TTrigger, _SMLite_ConfigItem<TState, TTrigger>> ();
	}
}
