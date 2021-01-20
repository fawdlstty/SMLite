using Fawdlstty.SMLite.ItemStruct;
using System;
using System.Collections.Generic;

namespace Fawdlstty.SMLite {
	public class SMLite<TState, TTrigger> {
		public SMLite (TState init_state) { State = init_state; }
		public _SMLite_ConfigState<TState, TTrigger> Configure (TState state) {

		}
		public bool AllowTriggering (TTrigger trigger) {
			if (m_states.ContainsKey (State))
				return 
		}
		public void Triggering (TTrigger trigger) {

		}
		public void Triggering (TTrigger trigger, params object[] args) {

		}

		public TState State { get; private set; }
		private Dictionary<TState, _SMLite_ConfigState<TState, TTrigger>> m_states;
	}
}
