package cn.fawdlstty.smlite;

import java.util.HashMap;

public class SMLite<TState extends Enum, TTrigger extends Enum> {
    public SMLite (TState init_state, HashMap<TState, _SMLite_ConfigState<TState, TTrigger>> _states) {
        m_state = init_state;
        m_cfg_states = _states;
    }

    public boolean AllowTriggering (TTrigger trigger) {
        if (m_cfg_states.containsKey (m_state))
            return m_cfg_states.get (m_state)._allow_trigger (trigger);
        return false;
    }

    public void Triggering (TTrigger trigger) throws Exception {
        synchronized (m_cfg_states) {
            if (m_cfg_states.containsKey (m_state)) {
                if (m_cfg_states.get (m_state)._allow_trigger (trigger)) {
                    TState _new_state = m_cfg_states.get (m_state)._trigger (trigger);
                    if (_new_state.compareTo (m_state) != 0) {
                        if (m_cfg_states.get (m_state).m_on_leave != null)
                            m_cfg_states.get (m_state).m_on_leave.call ();
                        m_state = _new_state;
                        if (m_cfg_states.get (m_state).m_on_entry != null)
                            m_cfg_states.get (m_state).m_on_entry.call ();
                    }
                    return;
                }
            }
            throw new Exception ("not match function found.");
        }
    }

    public void Triggering (TTrigger trigger, Object... args) throws Exception {
        synchronized (m_cfg_states) {
            if (m_cfg_states.containsKey (m_state)) {
                if (m_cfg_states.get (m_state)._allow_trigger (trigger)) {
                    TState _new_state = m_cfg_states.get (m_state)._trigger (trigger, args);
                    if (_new_state.compareTo (m_state) != 0) {
                        if (m_cfg_states.get (m_state).m_on_leave != null)
                            m_cfg_states.get (m_state).m_on_leave.call ();
                        m_state = _new_state;
                        if (m_cfg_states.get (m_state).m_on_entry != null)
                            m_cfg_states.get (m_state).m_on_entry.call ();
                    }
                    return;
                }
            }
            throw new Exception ("not match function found.");
        }
    }

    public TState GetState () { return m_state; }
    public void SetState (TState state) { synchronized (m_cfg_states) { m_state = state; } }
    private TState m_state;
    private HashMap<TState, _SMLite_ConfigState<TState, TTrigger>> m_cfg_states = null;
}
