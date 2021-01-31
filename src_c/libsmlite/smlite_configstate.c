#include <malloc.h>
#include <stdio.h>
#include <stdlib.h>

#include "libsmlite.h"



psmlite_configstate_t smlite_configstate_create (int32_t init_state) {
	psmlite_configstate_t _cfgstate;
	_cfgstate = (psmlite_configstate_t) malloc (sizeof (smlite_configstate_t));
	if (!_cfgstate) {
		printf ("malloc failed.\0");
		return 0;
	}
	memset (_cfgstate, 0, sizeof (smlite_configstate_t));
	_cfgstate->m_on_entry = 0;
	_cfgstate->m_on_leave = 0;
	_cfgstate->m_state = init_state;
	c_map_create (&_cfgstate->m_items, _int32_key_comparer);
	return _cfgstate;
}

void smlite_configstate_delete (psmlite_configstate_t *pcfgstate) {
	if ((!pcfgstate) || (!*pcfgstate))
		return;
	c_map_destroy (&((*pcfgstate)->m_items));
	free (*pcfgstate);
	*pcfgstate = 0;
}

void smlite_configstate_when_func (psmlite_configstate_t cfgstate, int32_t trigger, whenfunc_t callback) {
	c_iterator _iter, _end;
	psmlite_configitem_t _ptr;
	c_pair* _pair;
	if (!cfgstate) {
		printf ("parameter connot be null.\n");
		return;
	}
	_iter = c_map_find (&cfgstate->m_items, (value_type) trigger);
	_end = c_map_end (&cfgstate->m_items);
	if (!ITER_EQUAL (_iter, _end)) {
		printf ("trigger is already exists.\0");
		return;
	}
	_ptr = smlite_configitem_create (cfgstate->m_state, trigger, callback, true);
	_pair = (c_pair*) malloc (sizeof (c_pair));
	if (!_pair) {
		printf ("malloc failed.\0");
		return;
	}
	memset (_pair, 0, sizeof (c_pair));
	_pair->first = (value_type) trigger;
	_pair->second = (value_type) _ptr;
	c_map_insert (&cfgstate->m_items, _pair);
}

void smlite_configstate_when_action (psmlite_configstate_t cfgstate, int32_t trigger, whenaction_t callback) {
	c_iterator _iter, _end;
	psmlite_configitem_t _ptr;
	c_pair* _pair;
	if (!cfgstate) {
		printf ("parameter connot be null.\n");
		return;
	}
	_iter = c_map_find (&cfgstate->m_items, (value_type) trigger);
	_end = c_map_end (&cfgstate->m_items);
	if (!ITER_EQUAL (_iter, _end)) {
		printf ("trigger is already exists.\0");
		return;
	}
	_ptr = smlite_configitem_create (cfgstate->m_state, trigger, callback, false);
	_pair = (c_pair*) malloc (sizeof (c_pair));
	if (!_pair) {
		printf ("malloc failed.\0");
		return;
	}
	memset (_pair, 0, sizeof (c_pair));
	_pair->first = (value_type) trigger;
	_pair->second = (value_type) _ptr;
	c_map_insert (&cfgstate->m_items, _pair);
}

void _ignore_func (int32_t _state, int32_t _trigger) {}
void smlite_configstate_when_change_to (psmlite_configstate_t cfgstate, int32_t trigger, int32_t new_state) {
	if (!cfgstate) {
		printf ("parameter connot be null.\n");
		return;
	}
	if (!cfgstate) {
		printf ("parameter connot be null.\n");
		return;
	}
	c_iterator _iter, _end;
	psmlite_configitem_t _ptr;
	c_pair* _pair;
	if (!cfgstate) {
		printf ("parameter connot be null.\n");
		return;
	}
	_iter = c_map_find (&cfgstate->m_items, (value_type) trigger);
	_end = c_map_end (&cfgstate->m_items);
	if (!ITER_EQUAL (_iter, _end)) {
		printf ("trigger is already exists.\0");
		return;
	}
	_ptr = smlite_configitem_create (new_state, trigger, _ignore_func, false);
	_pair = (c_pair*) malloc (sizeof (c_pair));
	if (!_pair) {
		printf ("malloc failed.\0");
		return;
	}
	memset (_pair, 0, sizeof (c_pair));
	_pair->first = (value_type) trigger;
	_pair->second = (value_type) _ptr;
	c_map_insert (&cfgstate->m_items, _pair);
}

void smlite_configstate_when_ignore (psmlite_configstate_t cfgstate, int32_t trigger) {
	if (!cfgstate) {
		printf ("parameter connot be null.\n");
		return;
	}
	c_iterator _iter, _end;
	psmlite_configitem_t _ptr;
	c_pair* _pair;
	if (!cfgstate) {
		printf ("parameter connot be null.\n");
		return;
	}
	_iter = c_map_find (&cfgstate->m_items, (value_type) trigger);
	_end = c_map_end (&cfgstate->m_items);
	if (!ITER_EQUAL (_iter, _end)) {
		printf ("trigger is already exists.\0");
		return;
	}
	_ptr = smlite_configitem_create (cfgstate->m_state, trigger, _ignore_func, false);
	_pair = (c_pair*) malloc (sizeof (c_pair));
	if (!_pair) {
		printf ("malloc failed.\0");
		return;
	}
	memset (_pair, 0, sizeof (c_pair));
	_pair->first = (value_type) trigger;
	_pair->second = (value_type) _ptr;
	c_map_insert (&cfgstate->m_items, _pair);
}

void smlite_configstate_on_entry (psmlite_configstate_t cfgstate, notify_func_t callback) {
	if (!cfgstate) {
		printf ("parameter connot be null.\n");
		return;
	}
	cfgstate->m_on_entry = callback;
}

void smlite_configstate_on_leave (psmlite_configstate_t cfgstate, notify_func_t callback) {
	if (!cfgstate) {
		printf ("parameter connot be null.\n");
		return;
	}
	cfgstate->m_on_leave = callback;
}
