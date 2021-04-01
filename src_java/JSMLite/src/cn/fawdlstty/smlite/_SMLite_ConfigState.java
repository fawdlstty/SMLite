package cn.fawdlstty.smlite;

import cn.fawdlstty.smlite.internface.*;

import java.util.EnumSet;
import java.util.HashMap;

public class _SMLite_ConfigState<TState extends Enum, TTrigger extends Enum> {
    private _SMLite_ConfigState<TState, TTrigger> _try_add_trigger (TTrigger _trigger, _SMLite_ConfigItem<TState, TTrigger> _item) throws Exception {
        if (m_items.containsKey (_trigger))
            throw new Exception ("state is already has this trigger methods.");
        m_items.put(_trigger, _item);
        return this;
    }

    public _SMLite_ConfigState<TState, TTrigger> WhenChangeTo (TTrigger trigger, TState new_state) throws Exception {
        IFunc_ callback = () -> new_state;
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem._ReturnState), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }

    public _SMLite_ConfigState<TState, TTrigger> WhenIgnore (TTrigger trigger) throws Exception {
        IAction_ callback = () -> {};
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.noneOf(_SMLite_BuildItem.class), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }

    public _SMLite_ConfigState<TState, TTrigger> WhenFunc (TTrigger trigger, IFunc_<TState> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem._ReturnState), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }
    public _SMLite_ConfigState<TState, TTrigger> WhenFunc (TTrigger trigger, IFunc_a<TState> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem._ReturnState), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }

    public _SMLite_ConfigState<TState, TTrigger> WhenFunc_s (TTrigger trigger, IFunc_s<TState> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem._ReturnState, _SMLite_BuildItem.State), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }
    public _SMLite_ConfigState<TState, TTrigger> WhenFunc_s (TTrigger trigger, IFunc_sa<TState> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem._ReturnState, _SMLite_BuildItem.State), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }

    public _SMLite_ConfigState<TState, TTrigger> WhenFunc_t (TTrigger trigger, IFunc_t<TState, TTrigger> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem._ReturnState, _SMLite_BuildItem.Trigger), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }
    public _SMLite_ConfigState<TState, TTrigger> WhenFunc_t (TTrigger trigger, IFunc_ta<TState, TTrigger> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem._ReturnState, _SMLite_BuildItem.Trigger), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }

    public _SMLite_ConfigState<TState, TTrigger> WhenFunc_st (TTrigger trigger, IFunc_st<TState, TTrigger> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem._ReturnState, _SMLite_BuildItem.State, _SMLite_BuildItem.Trigger), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }
    public _SMLite_ConfigState<TState, TTrigger> WhenFunc_st (TTrigger trigger, IFunc_sta<TState, TTrigger> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem._ReturnState, _SMLite_BuildItem.State, _SMLite_BuildItem.Trigger), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }

    public _SMLite_ConfigState<TState, TTrigger> WhenAction (TTrigger trigger, IAction_ callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.noneOf(_SMLite_BuildItem.class), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }
    public _SMLite_ConfigState<TState, TTrigger> WhenAction (TTrigger trigger, IAction_a callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.noneOf(_SMLite_BuildItem.class), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }

    public _SMLite_ConfigState<TState, TTrigger> WhenAction_s (TTrigger trigger, IAction_s<TState> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem.State), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }
    public _SMLite_ConfigState<TState, TTrigger> WhenAction_s (TTrigger trigger, IAction_sa<TState> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem.State), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }

    public _SMLite_ConfigState<TState, TTrigger> WhenAction_t (TTrigger trigger, IAction_t<TTrigger> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem.Trigger), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }
    public _SMLite_ConfigState<TState, TTrigger> WhenAction_t (TTrigger trigger, IAction_ta<TTrigger> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem.Trigger), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }

    public _SMLite_ConfigState<TState, TTrigger> WhenAction_st (TTrigger trigger, IAction_st<TState, TTrigger> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem.State, _SMLite_BuildItem.Trigger), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }
    public _SMLite_ConfigState<TState, TTrigger> WhenAction_st (TTrigger trigger, IAction_sta<TState, TTrigger> callback) throws Exception {
        _SMLite_ConfigItem<TState, TTrigger> _item = new _SMLite_ConfigItem (EnumSet.of (_SMLite_BuildItem.State, _SMLite_BuildItem.Trigger), m_state, trigger, callback);
        return _try_add_trigger (trigger, _item);
    }

    public _SMLite_ConfigState<TState, TTrigger> OnEntry (IAction_ callback) throws Exception {
        if (m_on_entry != null)
            throw new Exception ("OnEntry is already have been set.");
        m_on_entry = callback;
        return this;
    }

    public _SMLite_ConfigState<TState, TTrigger> OnLeave (IAction_ callback) throws Exception {
        if (m_on_leave != null)
            throw new Exception ("OnLeave is already have been set.");
        m_on_leave = callback;
        return this;
    }

    public boolean _allow_trigger (TTrigger trigger) { return m_items.containsKey (trigger); }

    public TState _trigger (TTrigger trigger) throws Exception {
        if (_allow_trigger (trigger))
            return m_items.get (trigger)._call ();
        throw new Exception ("not match function found.");
    }

    public TState _trigger (TTrigger trigger, Object[] args) throws Exception {
        if (_allow_trigger (trigger))
            return m_items.get (trigger)._call (args);
        throw new Exception ("not match function found.");
    }

    public IAction_ m_on_entry = null, m_on_leave = null;
    public TState m_state;
    private HashMap<TTrigger, _SMLite_ConfigItem<TState, TTrigger>> m_items = new HashMap();
}
