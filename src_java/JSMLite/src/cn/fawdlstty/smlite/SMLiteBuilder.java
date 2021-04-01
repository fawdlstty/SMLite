package cn.fawdlstty.smlite;

import java.util.Dictionary;
import java.util.HashMap;

public class SMLiteBuilder<TState extends Enum, TTrigger extends Enum> {
    public _SMLite_ConfigState<TState, TTrigger> Configure (TState state) throws Exception {
        if (m_builded)
            throw new Exception ("shouldn't configure builder after builded.");
        if (m_states.containsKey (state))
            throw new Exception ("state is already exists.");
        _SMLite_ConfigState<TState, TTrigger> _state = new _SMLite_ConfigState ();
        _state.m_state = state;
        m_states.put (state, _state);
        return _state;
    }

    public SMLite<TState, TTrigger> Build (TState init_state) {
        m_builded = true;
        return new SMLite<TState, TTrigger> (init_state, m_states);
    }

    private HashMap<TState, _SMLite_ConfigState<TState, TTrigger>> m_states = new HashMap ();
    private boolean m_builded = false;
}
