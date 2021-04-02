package cn.fawdlstty.smlite;

import cn.fawdlstty.smlite.internface.*;

import java.util.EnumSet;

public class _SMLite_ConfigItem<TState, TTrigger> {
    public _SMLite_ConfigItem (EnumSet<_SMLite_BuildItem> build_item, TState state, TTrigger trigger, Object callback) {
        m_build_item = build_item;
        m_state = state;
        m_trigger = trigger;
        Callback = callback;
    }

    public TState _call () throws Exception {
        if (m_build_item.isEmpty ()) {
            ((IAction_) Callback).call();
            return m_state;
        } else if (m_build_item.size() == 1 && m_build_item.contains(_SMLite_BuildItem._ReturnState)) {
            return ((IFunc_<TState>) Callback).call();
        } else if (m_build_item.size() == 1 && m_build_item.contains(_SMLite_BuildItem.State)) {
            ((IAction_s<TState>) Callback).call(m_state);
            return m_state;
        } else if (m_build_item.size() == 2 && m_build_item.contains(_SMLite_BuildItem._ReturnState) && m_build_item.contains(_SMLite_BuildItem.State)) {
            return ((IFunc_s<TState>) Callback).call(m_state);
        } else if (m_build_item.size() == 1 && m_build_item.contains(_SMLite_BuildItem.Trigger)) {
            ((IAction_t<TTrigger>) Callback).call(m_trigger);
            return m_state;
        } else if (m_build_item.size() == 2 && m_build_item.contains(_SMLite_BuildItem._ReturnState) && m_build_item.contains(_SMLite_BuildItem.Trigger)) {
            return ((IFunc_t<TState, TTrigger>) Callback).call(m_trigger);
        } else if (m_build_item.size() == 2 && m_build_item.contains(_SMLite_BuildItem.Trigger) && m_build_item.contains(_SMLite_BuildItem.State)) {
            ((IAction_st<TState, TTrigger>) Callback).call(m_state, m_trigger);
            return m_state;
        } else {
            return ((IFunc_st<TState, TTrigger>) Callback).call(m_state, m_trigger);
        }
    }

    public TState _call (Object[] args) throws Exception {
        if (m_build_item.isEmpty ()) {
            ((IAction_a) Callback).call(args);
            return m_state;
        } else if (m_build_item.size() == 1 && m_build_item.contains(_SMLite_BuildItem._ReturnState)) {
            return ((IFunc_a<TState>) Callback).call(args);
        } else if (m_build_item.size() == 1 && m_build_item.contains(_SMLite_BuildItem.State)) {
            ((IAction_sa<TState>) Callback).call(m_state, args);
            return m_state;
        } else if (m_build_item.size() == 2 && m_build_item.contains(_SMLite_BuildItem._ReturnState) && m_build_item.contains(_SMLite_BuildItem.State)) {
            return ((IFunc_sa<TState>) Callback).call(m_state, args);
        } else if (m_build_item.size() == 1 && m_build_item.contains(_SMLite_BuildItem.Trigger)) {
            ((IAction_ta<TTrigger>) Callback).call(m_trigger, args);
            return m_state;
        } else if (m_build_item.size() == 2 && m_build_item.contains(_SMLite_BuildItem._ReturnState) && m_build_item.contains(_SMLite_BuildItem.Trigger)) {
            return ((IFunc_ta<TState, TTrigger>) Callback).call(m_trigger, args);
        } else if (m_build_item.size() == 2 && m_build_item.contains(_SMLite_BuildItem.Trigger) && m_build_item.contains(_SMLite_BuildItem.State)) {
            ((IAction_sta<TState, TTrigger>) Callback).call(m_state, m_trigger, args);
            return m_state;
        } else {
            return ((IFunc_sta<TState, TTrigger>) Callback).call(m_state, m_trigger, args);
        }
    }

    private EnumSet<_SMLite_BuildItem> m_build_item;
    private TState m_state;
    private TTrigger m_trigger;
    private Object Callback;
}
