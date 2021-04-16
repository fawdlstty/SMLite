/*
* SMLite
* State machine library for C, C++, C#, Java, JavaScript, Python, VB.Net
* Author: Fawdlstty
* Version 0.1.7
*
* Source Repository            <https://github.com/fawdlstty/SMLite>
* Report                       <https://github.com/fawdlstty/SMLite/issues>
* MIT License                  <https://opensource.org/licenses/MIT>
* Copyright (C) 2021 Fawdlstty <https://www.fawdlstty.com>
*/

using Fawdlstty.SMLite.ItemStruct;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Fawdlstty.SMLite {
    public class SMLite<TState, TTrigger> where TState : IComparable where TTrigger : Enum {
		internal SMLite (TState init_state, int _cfg_state_index) {
			m_state = init_state;
			m_cfg_state_index = _cfg_state_index;
		}

		public bool AllowTriggering (TTrigger trigger) {
			var _cfg_states = s_cfg_states_group[m_cfg_state_index];
			if (_cfg_states.ContainsKey (m_state))
				return _cfg_states[m_state]._allow_trigger (trigger);
			return false;
		}

		public void Triggering (TTrigger trigger, params object[] args) {
			lock (m_locker) {
				var _cfg_states = s_cfg_states_group[m_cfg_state_index];
				if (_cfg_states.ContainsKey (m_state)) {
					if (_cfg_states[m_state]._allow_trigger (trigger)) {
						var _new_state = _cfg_states[m_state]._trigger (trigger, args);
						if (_new_state.CompareTo (m_state) != 0) {
							if (_cfg_states[m_state].m_on_leave != null)
								_cfg_states[m_state].m_on_leave ();
							m_state = _new_state;
							if (_cfg_states[m_state].m_on_entry != null)
								_cfg_states[m_state].m_on_entry ();
						}
						return;
					}
				}
				throw new Exception ("not match function found.");
			}
		}

		public TState State {
			get { return m_state; }
			set { lock (m_locker) m_state = value; }
		}
		private TState m_state;
		private object m_locker = new object { };

		public void SetUserData (string _key, object _value) {
			lock (m_locker)
				m_user_data[_key] = _value;
		}
		public T GetUserData<T> (string _key) {
			lock (m_locker)
				return (T) m_user_data[_key];
		}
		public void ClearUserDataItem (string _key) {
			lock (m_locker)
				m_user_data.Remove (_key);
		}
		public void ClearUserData () {
			lock (m_locker)
				m_user_data.Clear ();
		}
		private Dictionary<string, object> m_user_data = new Dictionary<string, object> ();

		public string Serialize () {
			lock (m_locker) {
				return JsonConvert.SerializeObject (new {
					type = "SMLite",
					stype = typeof (TState).FullName,
					ttype = typeof (TTrigger).FullName,
					state = m_state,
					cfg_state_idx = m_cfg_state_index
				});
			}
		}
		public static SMLite<TState, TTrigger> Deserialize (string _ser) {
			JObject _o = JObject.Parse (_ser);
			if ($"{_o["type"]}" != "SMLite")
				throw new Exception ($"You must deserialize by {_o["type"]}<>.Deserialize ()");
			if (typeof (TState).FullName != $"{_o["stype"]}" || typeof (TTrigger).FullName != $"{_o["ttype"]}")
				throw new Exception ("TState or TTrigger not match");
			return new SMLite<TState, TTrigger> (_o["state"].ToObject<TState> (), _o["cfg_state_idx"].ToObject<int> ());
		}

		private int m_cfg_state_index = 0;
		internal static int s_cfg_states_group_index = 0;
		internal static Dictionary<int, Dictionary<TState, _SMLite_ConfigState<TState, TTrigger>>> s_cfg_states_group = new Dictionary<int, Dictionary<TState, _SMLite_ConfigState<TState, TTrigger>>> ();
	}
}
