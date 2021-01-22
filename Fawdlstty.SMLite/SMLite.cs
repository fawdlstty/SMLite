using Fawdlstty.SMLite.ItemStruct;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fawdlstty.SMLite {
    public class SMLite<TState, TTrigger> where TState : Enum where TTrigger : Enum {
		public SMLite (TState init_state, Dictionary<TState, _SMLite_ConfigState<TState, TTrigger>> _states) {
			State = init_state;
			m_states = _states;
		}

		public bool AllowTriggering (TTrigger trigger) {
			if (m_states.ContainsKey (State))
				return m_states[State]._allow_trigger (trigger);
			return false;
		}

		public void Triggering (TTrigger trigger, params object[] args) {
			if (m_states.ContainsKey (State)) {
				if (m_states[State]._allow_trigger (trigger)) {
					var _new_state = m_states[State]._trigger_async (trigger, args).Result;
					if (_new_state.CompareTo (State) != 0) {
						if (m_states[State].m_on_leave != null)
							m_states[State].m_on_leave ();
						if (m_states[State].m_on_leave_async != null)
							m_states[State].m_on_leave_async ().Wait ();
						State = _new_state;
						if (m_states[State].m_on_entry != null)
							m_states[State].m_on_entry ();
						if (m_states[State].m_on_entry_async != null)
							m_states[State].m_on_entry_async ().Wait ();
					}
					return;
				}
            }
			throw new Exception ("not match function found.");
		}

		public async Task TriggeringAsync (TTrigger trigger, params object[] args) {
			if (m_states.ContainsKey (State)) {
				if (m_states[State]._allow_trigger (trigger)) {
					var _new_state = await m_states[State]._trigger_async (trigger, args);
					if (_new_state.CompareTo (State) != 0) {
						if (m_states[State].m_on_leave != null)
							m_states[State].m_on_leave ();
						if (m_states[State].m_on_leave_async != null)
							await m_states[State].m_on_leave_async ();
						State = _new_state;
						if (m_states[State].m_on_entry != null)
							m_states[State].m_on_entry ();
						if (m_states[State].m_on_entry_async != null)
							await m_states[State].m_on_entry_async ();
					}
					return;
				}
			}
			throw new Exception ("not match function found.");
		}

		public TState State { get; private set; }
		Dictionary<TState, _SMLite_ConfigState<TState, TTrigger>> m_states = null;
	}
}
