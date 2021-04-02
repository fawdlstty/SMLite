/*
* SMLite
* State machine library for C, C++, C#, Java, JavaScript, Python, VB.Net
* Author: Fawdlstty
* Version 0.1.6
* 
* Source Repository            <https://github.com/fawdlstty/SMLite>
* Report                       <https://github.com/fawdlstty/SMLite/issues>
* MIT License                  <https://opensource.org/licenses/MIT>
* Copyright (C) 2021 Fawdlstty <https://www.fawdlstty.com>
*/

#ifndef __SMLITE_HPP__
#define __SMLITE_HPP__

#include <cstdio>
#include <exception>
#include <functional>
#include <map>
#include <memory>
#include <mutex>
#include <sstream>
#include <string>
#include <tuple>
#include <vector>



namespace Fawdlstty {

	class _SMLite_Exception: public std::exception {
	public:
		_SMLite_Exception (std::string _reason): m_reason (_reason) {}
		const char* what () const noexcept override { return m_reason.data (); }

	private:
		std::string m_reason;
	};

	template<typename TState, typename TTrigger>
	class _SMLite_ConfigItem {
	public:
		_SMLite_ConfigItem (TState _state, TTrigger _trigger): m_state (_state), m_trigger (_trigger) {}
		virtual ~_SMLite_ConfigItem () = default;
	protected:
		TState m_state;
		TTrigger m_trigger;
		virtual void _f () = 0;
	};

	template<typename TState, typename TTrigger, typename... Args>	class _SMLite_ConfigItem_A;
	template<typename TState, typename TTrigger, typename... Args>	class _SMLite_ConfigItem_SA;
	template<typename TState, typename TTrigger, typename... Args>	class _SMLite_ConfigItem_TA;
	template<typename TState, typename TTrigger, typename... Args>	class _SMLite_ConfigItem_STA;
	template<typename TState, typename TTrigger>					class _SMLite_ConfigState;
	template<typename TState, typename TTrigger>					class SMLite;
	template<typename TState, typename TTrigger>					class SMLiteBuilder;



	//
	// trigger item
	//

	template<typename TState, typename TTrigger, typename... Args>
	class _SMLite_ConfigItem_A: public _SMLite_ConfigItem<TState, TTrigger> {
	public:
		virtual ~_SMLite_ConfigItem_A () = default;
		_SMLite_ConfigItem_A (TState _state, TTrigger _trigger, std::function<TState (Args...)> _callback)
			: _SMLite_ConfigItem<TState, TTrigger> (_state, _trigger), m_callback (_callback) {}
		TState _call (Args... args) { return m_callback (args...); }
	protected:
		std::function<TState (Args...)> m_callback;
		void _f () override {}
	};

	template<typename TState, typename TTrigger, typename... Args>
	class _SMLite_ConfigItem_SA: public _SMLite_ConfigItem<TState, TTrigger> {
	public:
		virtual ~_SMLite_ConfigItem_SA () = default;
		_SMLite_ConfigItem_SA (TState _state, TTrigger _trigger, std::function<TState (TState, Args...)> _callback)
			: _SMLite_ConfigItem<TState, TTrigger> (_state, _trigger), m_callback (_callback) {}
		TState _call (Args... args) { return m_callback (this->m_state, args...); }
	protected:
		std::function<TState (TState, Args...)> m_callback;
		void _f () override {}
	};

	template<typename TState, typename TTrigger, typename... Args>
	class _SMLite_ConfigItem_TA: public _SMLite_ConfigItem<TState, TTrigger> {
	public:
		virtual ~_SMLite_ConfigItem_TA () = default;
		_SMLite_ConfigItem_TA (TState _state, TTrigger _trigger, std::function<TState (TTrigger, Args...)> _callback)
			: _SMLite_ConfigItem<TState, TTrigger> (_state, _trigger), m_callback (_callback) {}
		TState _call (Args... args) { return m_callback (this->m_trigger, args...); }
	protected:
		std::function<TState (TTrigger, Args...)> m_callback;
		void _f () override {}
	};

	template<typename TState, typename TTrigger, typename... Args>
	class _SMLite_ConfigItem_STA: public _SMLite_ConfigItem<TState, TTrigger> {
	public:
		virtual ~_SMLite_ConfigItem_STA () = default;
		_SMLite_ConfigItem_STA (TState _state, TTrigger _trigger, std::function<TState (TState, TTrigger, Args...)> _callback)
			: _SMLite_ConfigItem<TState, TTrigger> (_state, _trigger), m_callback (_callback) {}
		TState _call (Args... args) { return m_callback (this->m_state, this->m_trigger, args...); }
	protected:
		std::function<TState (TState, TTrigger, Args...)> m_callback;
		void _f () override {}
	};



	//
	// state item (include trigger groups)
	//

	template<typename TState, typename TTrigger>
	class _SMLite_ConfigState : public std::enable_shared_from_this<_SMLite_ConfigState<TState, TTrigger>> {
		friend class SMLite<TState, TTrigger>;
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> _try_add_trigger (TTrigger _trigger, _SMLite_ConfigItem<TState, TTrigger> *_ptr) {
			if (m_items.find (_trigger) != m_items.end ())
				throw _SMLite_Exception ("state is already has this trigger methods.");
			m_items [_trigger] = std::shared_ptr<_SMLite_ConfigItem<TState, TTrigger>> (_ptr);
			return this->shared_from_this ();
		}

	public:
		_SMLite_ConfigState (TState state) : m_state (state) {}
#pragma region WhenFunc/WhenAction
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenFunc (TTrigger trigger, std::function<TState ()> callback) {
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_A<TState, TTrigger> (m_state, trigger, callback));
		}
		template<typename... Args>
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenFunc (TTrigger trigger, std::function<TState (Args...)> callback) {
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_A<TState, TTrigger, Args...> (m_state, trigger, callback));
		}
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenAction (TTrigger trigger, std::function<void ()> callback) {
			std::function<TState (TState)> f = [callback] (TState state) -> TState { callback (); return state; };
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_SA<TState, TTrigger> (m_state, trigger, f));
		}
		template<typename... Args>
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenAction (TTrigger trigger, std::function<void (Args...)> callback) {
			std::function<TState (TState, Args...)> f = [callback] (TState state, Args... args) -> TState { callback (args...); return state; };
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_SA<TState, TTrigger, Args...> (m_state, trigger, f));
		}
#pragma endregion
#pragma region WhenFunc/WhenAction S
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenFunc_S (TTrigger trigger, std::function<TState (TState)> callback) {
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_SA<TState, TTrigger> (m_state, trigger, callback));
		}
		template<typename... Args>
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenFunc_S (TTrigger trigger, std::function<TState (TState, Args...)> callback) {
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_SA<TState, TTrigger, Args...> (m_state, trigger, callback));
		}
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenAction_S (TTrigger trigger, std::function<void (TState)> callback) {
			std::function<TState (TState)> f = [callback] (TState state) -> TState { callback (state); return state; };
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_SA<TState, TTrigger> (m_state, trigger, f));
		}
		template<typename... Args>
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenAction_S (TTrigger trigger, std::function<void (TState, Args...)> callback) {
			std::function<TState (TState, Args...)> f = [callback] (TState state, Args... args) -> TState { callback (state, args...); return state; };
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_SA<TState, TTrigger, Args...> (m_state, trigger, f));
		}
#pragma endregion
#pragma region WhenFunc/WhenAction T
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenFunc_T (TTrigger trigger, std::function<TState (TTrigger)> callback) {
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_TA<TState, TTrigger> (m_state, trigger, callback));
		}
		template<typename... Args>
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenFunc_T (TTrigger trigger, std::function<TState (TTrigger, Args...)> callback) {
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_TA<TState, TTrigger, Args...> (m_state, trigger, callback));
		}
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenAction_T (TTrigger trigger, std::function<void (TTrigger)> callback) {
			std::function<TState (TState, TTrigger)> f = [callback] (TState state, TTrigger trigger) -> TState { callback (trigger); return state; };
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_STA<TState, TTrigger> (m_state, trigger, f));
		}
		template<typename... Args>
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenAction_T (TTrigger trigger, std::function<void (TTrigger, Args...)> callback) {
			std::function<TState (TState, TTrigger, Args...)> f = [callback] (TState state, TTrigger trigger, Args... args) -> TState { callback (trigger, args...); return state; };
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_STA<TState, TTrigger, Args...> (m_state, trigger, f));
		}
#pragma endregion
#pragma region WhenFunc/WhenAction ST
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenFunc_ST (TTrigger trigger, std::function<TState (TState, TTrigger)> callback) {
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_STA<TState, TTrigger> (m_state, trigger, callback));
		}
		template<typename... Args>
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenFunc_ST (TTrigger trigger, std::function<TState (TState, TTrigger, Args...)> callback) {
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_STA<TState, TTrigger, Args...> (m_state, trigger, callback));
		}
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenAction_ST (TTrigger trigger, std::function<void (TState, TTrigger)> callback) {
			std::function<TState (TState, TTrigger)> f = [callback] (TState state, TTrigger trigger) -> TState { callback (state, trigger); return state; };
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_STA<TState, TTrigger> (m_state, trigger, f));
		}
		template<typename... Args>
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenAction_ST (TTrigger trigger, std::function<void (TState, TTrigger, Args...)> callback) {
			std::function<TState (TState, TTrigger, Args...)> f = [callback] (TState state, TTrigger trigger, Args... args) -> TState { callback (state, trigger, args...); return state; };
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_STA<TState, TTrigger, Args...> (m_state, trigger, f));
		}
#pragma endregion
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenChangeTo (TTrigger trigger, TState new_state) {
			std::function<TState ()> f = [new_state] () -> TState { return new_state; };
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_A<TState, TTrigger> (m_state, trigger, f));
		}
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenIgnore (TTrigger trigger) {
			std::function<TState (TState)> f = [] (TState state) -> TState { return state; };
			return _try_add_trigger (trigger, new _SMLite_ConfigItem_SA<TState, TTrigger> (m_state, trigger, f));
		}
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> OnEntry (std::function<void ()> callback) {
			if (m_on_entry)
				throw _SMLite_Exception ("OnEntry is already have been set.");
			m_on_entry = callback;
			return this->shared_from_this ();
		}
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> OnLeave (std::function<void ()> callback) {
			if (m_on_leave)
				throw _SMLite_Exception ("OnLeave is already have been set.");
			m_on_leave = callback;
			return this->shared_from_this ();
		}

	private:
		bool _allow_trigger (TTrigger trigger) { return m_items.find (trigger) != m_items.end (); }
		TState _trigger (TTrigger trigger) {
			_SMLite_ConfigItem<TState, TTrigger> *_ptr = m_items [trigger].get ();
			if (_ptr) {
				auto _ptrA = dynamic_cast<_SMLite_ConfigItem_A<TState, TTrigger>*> (_ptr);
				if (_ptrA)
					return _ptrA->_call ();
				auto _ptrSA = dynamic_cast<_SMLite_ConfigItem_SA<TState, TTrigger>*> (_ptr);
				if (_ptrSA)
					return _ptrSA->_call ();
				auto _ptrTA = dynamic_cast<_SMLite_ConfigItem_TA<TState, TTrigger>*> (_ptr);
				if (_ptrTA)
					return _ptrTA->_call ();
				auto _ptrSTA = dynamic_cast<_SMLite_ConfigItem_STA<TState, TTrigger>*> (_ptr);
				if (_ptrSTA)
					return _ptrSTA->_call ();
			}
			throw _SMLite_Exception ("not match function found.");
		}
		template<typename... Args>
		TState _trigger (TTrigger trigger, Args... args) {
			_SMLite_ConfigItem<TState, TTrigger> *_ptr = m_items [trigger].get ();
			if (_ptr) {
				auto _ptrA = dynamic_cast<_SMLite_ConfigItem_A<TState, TTrigger, Args...>*> (_ptr);
				if (_ptrA)
					return _ptrA->_call (args...);
				auto _ptrSA = dynamic_cast<_SMLite_ConfigItem_SA<TState, TTrigger, Args...>*> (_ptr);
				if (_ptrSA)
					return _ptrSA->_call (args...);
				auto _ptrTA = dynamic_cast<_SMLite_ConfigItem_TA<TState, TTrigger, Args...>*> (_ptr);
				if (_ptrTA)
					return _ptrTA->_call (args...);
				auto _ptrSTA = dynamic_cast<_SMLite_ConfigItem_STA<TState, TTrigger, Args...>*> (_ptr);
				if (_ptrSTA)
					return _ptrSTA->_call (args...);
			}
			throw _SMLite_Exception ("not match function found.");
		}

		std::function<void ()> m_on_entry, m_on_leave;
		TState m_state;
		std::map<TTrigger, std::shared_ptr<_SMLite_ConfigItem<TState, TTrigger>>> m_items;
	};



	//
	// state machine (include state groups)
	//

	template<typename TState, typename TTrigger>
	class SMLite {
		friend class SMLiteBuilder<TState, TTrigger>;
	public:
		SMLite (TState init_state, int _cfg_state): m_state (init_state), m_cfg_state_index (_cfg_state) {}
		TState GetState () { return m_state; }
		void SetState (TState new_state) {
			std::unique_lock<std::recursive_mutex> ul (m_mtx);
			m_state = new_state;
		}
		bool AllowTriggering (TTrigger trigger) {
			std::unique_lock<std::recursive_mutex> ul (m_mtx);
			std::shared_ptr<std::map<TState, std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>>>> _states;
			_get_ref ([&] (std::map<int, std::shared_ptr<std::map<TState, std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>>>>> &s_cfg_states_group, int &s_cfg_states_group_index) {
				_states = s_cfg_states_group [m_cfg_state_index];
			});
			if (_states->find (m_state) != _states->end ())
				return (*_states) [m_state]->_allow_trigger (trigger);
			return false;
		}
		bool Triggering (TTrigger trigger) {
			std::unique_lock<std::recursive_mutex> ul (m_mtx);
			std::shared_ptr<std::map<TState, std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>>>> _states;
			_get_ref ([&] (std::map<int, std::shared_ptr<std::map<TState, std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>>>>> &s_cfg_states_group, int &s_cfg_states_group_index) {
				_states = s_cfg_states_group [m_cfg_state_index];
			});
			if (!AllowTriggering (trigger))
				return false;
			auto _p = (*_states) [m_state];
			auto _state = _p->_trigger (trigger);
			if (m_state != _state) {
				if (_p->m_on_leave)
					_p->m_on_leave ();
				m_state = _state;
				_p = (*_states) [m_state];
				if (_p->m_on_entry)
					_p->m_on_entry ();
			}
			return true;
		}
		template<typename... Args>
		bool Triggering (TTrigger trigger, Args... args) {
			std::unique_lock<std::recursive_mutex> ul (m_mtx);
			std::shared_ptr<std::map<TState, std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>>>> _states;
			_get_ref ([&] (std::map<int, std::shared_ptr<std::map<TState, std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>>>>> &s_cfg_states_group, int &s_cfg_states_group_index) {
				_states = s_cfg_states_group[m_cfg_state_index];
			});
			if (!AllowTriggering (trigger))
				return false;
			auto _p = (*_states) [m_state];
			auto _state = _p->_trigger (trigger, args...);
			if (m_state != _state) {
				if (_p->m_on_leave)
					_p->m_on_leave ();
				m_state = _state;
				_p = (*_states) [m_state];
				if (_p->m_on_entry)
					_p->m_on_entry ();
			}
			return true;
		}

	private:
		TState m_state;
		std::recursive_mutex m_mtx;

	public:
		std::string Serialize () {
			std::stringstream _ss;
			_ss << "SMLite|" << typeid (TState).name () << "|" << typeid (TTrigger).name () << "|" << (int) m_state << "|" << m_cfg_state_index;
			return _ss.str ();
		}

		static std::shared_ptr<SMLite<TState, TTrigger>> Deserialize (std::string _ser) {
			std::vector<std::string> _v;
			size_t _begin = 0;
			size_t _p = _ser.find ('|', _begin);
			while (_p != std::string::npos) {
				_v.push_back (_ser.substr (_begin, _p - _begin));
				_begin = _p + 1;
				_p = _ser.find ('|', _begin);
			}
			_v.push_back (_ser.substr (_begin));
			if (_v.size () != 5)
				throw _SMLite_Exception ("Serialize string format error.");
			if (_v [0] != "SMLite")
				throw _SMLite_Exception ("You must deserialize by " + _v [0] + "<>::Deserialize ()");
			if (typeid (TState).name () != _v [1] || typeid (TTrigger).name () != _v [2])
				throw new _SMLite_Exception ("TState or TTrigger not match");
			TState _state = (TState) std::stoi (_v [3]);
			int _state_idx = std::stoi (_v [4]);
			return std::make_shared<SMLite<TState, TTrigger>> (_state, _state_idx);
		}

	private:
		int m_cfg_state_index = 0;
	public:
		static void _get_ref (std::function<void (std::map<int, std::shared_ptr<std::map<TState, std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>>>>> &, int &)> _callback) {
			static std::map<int, std::shared_ptr<std::map<TState, std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>>>>> s_cfg_states_group;
			static int s_cfg_states_group_index = 0;
			static std::mutex s_mtx;
			std::unique_lock<std::mutex> _ul (s_mtx);
			_callback (s_cfg_states_group, s_cfg_states_group_index);
		}
	};

	template<typename TState, typename TTrigger>
	class SMLiteBuilder {
	public:
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> Configure (TState state) {
			if (m_builded_index > 0)
				throw _SMLite_Exception ("shouldn't configure builder after builded.");
			if (m_states->find (state) != m_states->end ())
				throw _SMLite_Exception ("state is already exists.");
			auto _ptr = std::make_shared<_SMLite_ConfigState<TState, TTrigger>> (state);
			(*m_states) [state] = _ptr;
			return _ptr;
		}
		std::shared_ptr<SMLite<TState, TTrigger>> Build (TState init_state) {
			if (m_builded_index == 0) {
				SMLite<TState, TTrigger>::_get_ref ([&] (std::map<int, std::shared_ptr<std::map<TState, std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>>>>> &s_cfg_states_group, int &s_cfg_states_group_index) {
					m_builded_index = ++s_cfg_states_group_index;
					s_cfg_states_group [m_builded_index] = m_states;
				});
			}
			return std::shared_ptr<SMLite<TState, TTrigger>> (new SMLite<TState, TTrigger> (init_state, m_builded_index));
		}

	private:
		std::shared_ptr<std::map<TState, std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>>>> m_states
			= std::make_shared<std::map<TState, std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>>>> ();
		int m_builded_index = 0;
	};
}

#endif //__SMLITE_HPP__
