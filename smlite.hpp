#ifndef __SMLITE_HPP__
#define __SMLITE_HPP__

#include <execution>
#include <functional>
#include <map>
#include <memory>
#include <variant>
#include <vector>



namespace SMLite {
	template<typename TState, typename TTrigger> class _SMLite_ConfigItem;
	template<typename TState, typename TTrigger> class _SMLite_ConfigState;
	template<typename TState, typename TTrigger> class SMLite;



	template<typename TState, typename TTrigger>
	class _SMLite_ConfigItem {
	public:
		_SMLite_ConfigItem (TState old_state, TTrigger trigger, std::function<TState (TState old_state, TTrigger trigger)> action) : m_state (old_state), m_trigger (trigger), m_action (action) {}
		TState _call () { return m_action (m_state, m_trigger); }

	private:
		TState m_state;
		TTrigger m_trigger;
		std::function<TState (TState old_state, TTrigger trigger)> m_action;
	};



	template<typename TState, typename TTrigger>
	class _SMLite_ConfigState : public std::enable_shared_from_this<_SMLite_ConfigState<TState, TTrigger>> {
		friend class SMLite<TState, TTrigger>;
	public:
		_SMLite_ConfigState (TState state) : m_state (state) {}
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenFunc (TTrigger trigger, std::function<TState (TState state, TTrigger trigger)> action) {
			if (m_items.contains (trigger))
				throw std::exception ("state is already has this trigger methods.");
			m_items [trigger] = std::make_shared<_SMLite_ConfigItem<TState, TTrigger>> (m_state, trigger, action);
			return this->shared_from_this ();
		}
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenAction (TTrigger trigger, std::function<void (TState state, TTrigger trigger)> action) {
			if (m_items.contains (trigger))
				throw std::exception ("state is already has this trigger methods.");
			m_items [trigger] = std::make_shared<_SMLite_ConfigItem<TState, TTrigger>> (m_state, trigger, [action] (TState state, TTrigger trigger) {
				action (state, trigger);
				return state;
			});
			return this->shared_from_this ();
		}
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenChangeTo (TTrigger trigger, TState new_state) {
			if (m_items.contains (trigger))
				throw std::exception ("state is already has this trigger methods.");
			m_items [trigger] = std::make_shared<_SMLite_ConfigItem<TState, TTrigger>> (m_state, trigger, [new_state] (TState state, TTrigger trigger) {
				return new_state;
			});
			return this->shared_from_this ();
		}
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> WhenIgnore (TTrigger trigger) {
			if (m_items.contains (trigger))
				throw std::exception ("state is already has this trigger methods.");
			m_items [trigger] = std::make_shared<_SMLite_ConfigItem<TState, TTrigger>> (m_state, trigger, [] (TState state, TTrigger trigger) {
				return state;
			});
			return this->shared_from_this ();
		}

	protected:
		bool _allow_trigger (TTrigger trigger) { return m_items.contains (trigger); }
		TState _trigger (TTrigger trigger) { return m_items [trigger]->_call (); }

	private:
		TState m_state;
		std::map<TTrigger, std::shared_ptr<_SMLite_ConfigItem<TState, TTrigger>>> m_items;
	};



	template<typename TState, typename TTrigger>
	class SMLite {
	public:
		SMLite (TState init_state) : m_state (init_state) {}
		std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>> Configure (TState state) {
			if (m_states.contains (state))
				throw std::exception ("state is already exists.");
			auto _ptr = std::make_shared<_SMLite_ConfigState<TState, TTrigger>> (state);
			m_states [state] = _ptr;
			return _ptr;
		}
		TState GetState () { return m_state; }
		bool AllowTriggering (TTrigger trigger) {
			if (m_states.contains (m_state))
				return m_states [m_state]->_allow_trigger (trigger);
			return false;
		}
		void Triggering (TTrigger trigger) {
			if (!AllowTriggering (trigger))
				throw std::exception ("current state cannot launch this trigger.");
			m_state = m_states [m_state]->_trigger (trigger);
		}

	private:
		TState m_state;
		std::map<TState, std::shared_ptr<_SMLite_ConfigState<TState, TTrigger>>> m_states;
	};
}

#endif //__SMLITE_HPP__
