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
			m_state = init_state;
			m_cfg_states = _states;
		}

		public bool AllowTriggering (TTrigger trigger) {
			if (m_cfg_states.ContainsKey (m_state))
				return m_cfg_states[m_state]._allow_trigger (trigger);
			return false;
		}

		public void Triggering (TTrigger trigger, params object[] args) {
			lock (m_cfg_states) {
				if (m_cfg_states.ContainsKey (m_state)) {
					if (m_cfg_states[m_state]._allow_trigger (trigger)) {
						var _new_state = m_cfg_states[m_state]._trigger (trigger, args);
						if (_new_state.CompareTo (m_state) != 0) {
							if (m_cfg_states[m_state].m_on_leave != null)
								m_cfg_states[m_state].m_on_leave ();
							m_state = _new_state;
							if (m_cfg_states[m_state].m_on_entry != null)
								m_cfg_states[m_state].m_on_entry ();
						}
						return;
					}
				}
				throw new Exception ("not match function found.");
			}
		}

		public TState State {
			get { return m_state; }
			set { lock (m_cfg_states) m_state = value; }
		}
		private TState m_state;
		private Dictionary<TState, _SMLite_ConfigState<TState, TTrigger>> m_cfg_states = null;
	}
}
