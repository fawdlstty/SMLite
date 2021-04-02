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

'use strict';

const _SMLite_BuildItem = {
	NONE: 0,
	STATE: 1,
	TRIGGER: 2,
}

class _SMLite_ConfigItem {
	constructor(build_item, state, trigger, callback) {
		this.__build_item = build_item;
		this.__state = state;
		this.__trigger = trigger;
		this.__callback = callback;
	}

	_call(...args) {
		// let args = [].slice.call(arguments);
		let _ret = null;
		if (this.__build_item == _SMLite_BuildItem.NONE) {
			_ret = this.__callback(...args);
		} else if (this.__build_item == _SMLite_BuildItem.STATE) {
			_ret = this.__callback(this.__state, ...args);
		} else if (this.__build_item == _SMLite_BuildItem.TRIGGER) {
			_ret = this.__callback(this.__trigger, ...args);
		} else {
			_ret = this.__callback(this.__state, this.__trigger, ...args);
		}
		if (_ret === null)
			_ret = this.__state;
		return _ret;
	}
}

class _SMLite_ConfigState {
	constructor(state) {
		this.f__on_entry = null;
		this.f__on_leave = null;
		this.__state = state;
		this.__items = {};
	}

	_try_add_trigger(trigger, item) {
		if (trigger in this.__items)
			throw "state is already has this trigger methods.";
		this.__items[trigger] = item;
		return this;
	}

	WhenChangeTo(trigger, new_state) {
		let _callback = () => new_state;
		let _item = new _SMLite_ConfigItem(_SMLite_BuildItem.NONE, this.__state, trigger, _callback, false);
		return this._try_add_trigger(trigger, _item);
	}

	WhenIgnore(trigger) {
		let _callback = () => null;
		let _item = new _SMLite_ConfigItem(_SMLite_BuildItem.NONE, this.__state, trigger, _callback, false);
		return this._try_add_trigger(trigger, _item);
	}

	WhenFunc(trigger, callback) {
		let _item = new _SMLite_ConfigItem(_SMLite_BuildItem.STATE | _SMLite_BuildItem.TRIGGER, this.__state, trigger, callback, false);
		return this._try_add_trigger(trigger, _item);
	}

	WhenAction(trigger, callback) {
		let _callback = (...args) => { callback(...args); return null; };
		let _item = new _SMLite_ConfigItem(_SMLite_BuildItem.STATE | _SMLite_BuildItem.TRIGGER, this.__state, trigger, _callback, false);
		return this._try_add_trigger(trigger, _item);
	}

	OnEntry(callback) {
		if (this.f__on_entry != null)
			throw "OnEntry is already have been set.";
		this.f__on_entry = callback;
		return this;
	}

	OnLeave(callback) {
		if (this.f__on_leave != null)
			throw "OnLeave is already have been set.";
		this.f__on_leave = callback;
		return this;
	}

	_allow_trigger(trigger) {
		return trigger in this.__items;
	}

	_trigger(trigger, ...args) {
		if (this._allow_trigger(trigger))
			return this.__items[trigger]._call(...args);
		throw "not match function found.";
	}
}

class SMLite {
	constructor(init_state, states) {
		this.__state = init_state
		this.__states = states
	}

	AllowTriggering(trigger) {
		if (this.__state in this.__states)
			return this.__states[this.__state]._allow_trigger(trigger);
		return false;
	}

	Triggering(trigger, ...args) {
		if (this.AllowTriggering(trigger)) {
			let _cfgstate = this.__states[this.__state]
			let _new_state = _cfgstate._trigger(trigger, ...args)
			if (_new_state != this.__state) {
				if (_cfgstate.f__on_leave != null)
					_cfgstate.f__on_leave();
				this.__state = _new_state;
				_cfgstate = this.__states[this.__state];
				if (_cfgstate.f__on_entry != null)
					_cfgstate.f__on_entry();
			}
			return;
		}
		throw "not match function found.";
	}

	GetState() {
		return this.__state;
	}

	SetState(state) {
		this.__state = state;
	}
}

class SMLiteBuilder {
	constructor() {
		this.__states = {};
		this.__builded = false;
	}

	Configure(state) {
		if (this.__builded)
			throw "shouldn't configure builder after builded.";
		if (state in this.__states)
			throw "state is already exists.";
		let _state = new _SMLite_ConfigState(state);
		this.__states[state] = _state;
		return _state;
	}

	Build(init_state) {
		this.__builded = true;
		return new SMLite(init_state, this.__states);
	}
}

module.exports = { SMLiteBuilder };
