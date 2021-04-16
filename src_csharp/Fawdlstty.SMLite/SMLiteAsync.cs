using Fawdlstty.SMLite.ItemStruct;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fawdlstty.SMLite {
	public class SMLiteAsync<TState, TTrigger> where TState : IComparable where TTrigger : Enum {
		internal SMLiteAsync (TState init_state, int _cfg_state_index) {
			State = init_state;
			m_cfg_state_index = _cfg_state_index;
		}

		public bool AllowTriggering (TTrigger trigger) {
			var _cfg_states = s_cfg_states_group[m_cfg_state_index];
			if (_cfg_states.ContainsKey (State))
				return _cfg_states[State]._allow_trigger (trigger);
			return false;
		}

		public async Task TriggeringAsync (TTrigger trigger, params object[] args) {
			await TriggeringAsync (trigger, CancellationToken.None, args);
		}

		public async Task TriggeringAsync_c (TTrigger trigger, CancellationToken token, params object[] args) {
			using (var _guard = await m_locker.LockAsync ()) {
				var _cfg_states = s_cfg_states_group[m_cfg_state_index];
				if (_cfg_states.ContainsKey (m_state)) {
					if (_cfg_states[m_state]._allow_trigger (trigger)) {
						var _old_state = m_state;
						try {
							var _new_state = await _cfg_states[m_state]._triggerAsync (trigger, token, args);
							if (_new_state.CompareTo (m_state) != 0) {
								if (_cfg_states[m_state].m_on_leave_async != null) {
									if (token != CancellationToken.None)
										throw new Exception ("OnEntryAsync does not support the CancellationToken parameter, but the parameter 'token' is not 'CancellationToken.None'");
									await _cfg_states[m_state].m_on_leave_async ();
								} else if (_cfg_states[m_state].m_on_leave_cancel_async != null) {
									await _cfg_states[m_state].m_on_leave_cancel_async (token);
								}
								m_state = _new_state;
								if (_cfg_states[m_state].m_on_entry_async != null) {
									if (token != CancellationToken.None)
										throw new Exception ("OnLeaveAsync does not support the CancellationToken parameter, but the parameter 'token' is not 'CancellationToken.None'");
									await _cfg_states[m_state].m_on_entry_async ();
								} else if (_cfg_states[m_state].m_on_entry_cancel_async != null) {
									await _cfg_states[m_state].m_on_entry_cancel_async (token);
								}
							}
							return;
						} catch (Exception) {
							m_state = _old_state;
							throw;
						}
					}
				}
				throw new Exception ("not match function found.");
			}
		}

		public TState State {
			get { return m_state; }
			set { using (var _guard = m_locker.LockAsync ().Result) m_state = value; }
		}
		private TState m_state;
		private AsyncLock m_locker = new AsyncLock ();

		public async Task SetUserData (string _key, object _value) {
			using (var _guard = await m_locker.LockAsync ())
				m_user_data[_key] = _value;
		}
		public async Task<T> GetUserData<T> (string _key) {
			using (var _guard = await m_locker.LockAsync ())
				return (T) m_user_data[_key];
		}
		public async Task ClearUserDataItem (string _key) {
			using (var _guard = await m_locker.LockAsync ())
				m_user_data.Remove (_key);
		}
		public async Task ClearUserData () {
			using (var _guard = await m_locker.LockAsync ())
				m_user_data.Clear ();
		}
		private Dictionary<string, object> m_user_data = new Dictionary<string, object> ();

		public async Task<string> SerializeAsync () {
			using (var _guard = await m_locker.LockAsync ()) {
				return JsonConvert.SerializeObject (new {
					type = "SMLiteAsync",
					stype = typeof (TState).FullName,
					ttype = typeof (TTrigger).FullName,
					state = m_state,
					cfg_state_idx = m_cfg_state_index
				});
			}
		}
		public static SMLiteAsync<TState, TTrigger> Deserialize (string _ser) {
			JObject _o = JObject.Parse (_ser);
			if ($"{_o["type"]}" != "SMLiteAsync")
				throw new Exception ($"You must deserialize by {_o["type"]}.Deserialize ()");
			if (typeof (TState).FullName != $"{_o["stype"]}" || typeof (TTrigger).FullName != $"{_o["ttype"]}")
				throw new Exception ("TState or TTrigger not match");
			return new SMLiteAsync<TState, TTrigger> (_o["state"].ToObject<TState> (), _o["cfg_state_idx"].ToObject<int> ());
		}

		private int m_cfg_state_index = 0;
		internal static int s_cfg_states_group_index = 0;
		internal static Dictionary<int, Dictionary<TState, _SMLite_ConfigStateAsync<TState, TTrigger>>> s_cfg_states_group = new Dictionary<int, Dictionary<TState, _SMLite_ConfigStateAsync<TState, TTrigger>>> ();
	}
}
