/*
* SMLite
* State machine library for C, C++, C#, JavaScript, Python, VB.Net
* Author: Fawdlstty
* Version 0.1.6
*
* Source Repository            <https://github.com/fawdlstty/SMLite>
* Report                       <https://github.com/fawdlstty/SMLite/issues>
* MIT License                  <https://opensource.org/licenses/MIT>
* Copyright (C) 2021 Fawdlstty <https://www.fawdlstty.com>
*/

using Fawdlstty.SMLite.ItemStruct;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fawdlstty.SMLite {
    public class SMLite<TState, TTrigger> where TState : Enum where TTrigger : Enum {
		internal SMLite (TState init_state, Dictionary<TState, _SMLite_ConfigState<TState, TTrigger>> _states) {
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
					var _new_state = m_states[State]._trigger (trigger, args);
					if (_new_state.CompareTo (State) != 0) {
						if (m_states[State].m_on_leave != null)
							m_states[State].m_on_leave ();
						State = _new_state;
						if (m_states[State].m_on_entry != null)
							m_states[State].m_on_entry ();
					}
					return;
				}
            }
			throw new Exception ("not match function found.");
		}

		public TState State { get; set; }
		Dictionary<TState, _SMLite_ConfigState<TState, TTrigger>> m_states = null;
	}
}
