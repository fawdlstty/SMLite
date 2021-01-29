#include <malloc.h>
#include <stdio.h>
#include <stdlib.h>

#include "local.h"



psmlite_t smlite_create (psmlite_builder_t builder, int32_t init_state) {
	psmlite_t _sm;
	_sm = (psmlite_t) malloc (sizeof (smlite_t));
	if (!_sm) {
		printf ("malloc failed.\0");
		return 0;
	}
	memset (_sm, 0, sizeof (smlite_t));
	_sm->m_state = init_state;
	_sm->m_builder = builder;
	builder->m_ref_count += 1;
	return _sm;
}

void smlite_delete (psmlite_t *psm) {
	if ((!psm) || (!*psm))
		return;
	(*psm)->m_builder->m_ref_count -= 1;
	if ((*psm)->m_builder->m_ref_count == 0) {
		c_map_destroy (&((*psm)->m_builder->m_states));
		free ((*psm)->m_builder);
	}
	free (*psm);
	*psm = 0;
}

int32_t smlite_get_state (psmlite_t sm) {
	return sm->m_state;
}

void smlite_set_state (psmlite_t sm, int32_t new_state) {
	sm->m_state = new_state;
}

int smlite_allow_triggering (psmlite_t sm, int32_t trigger) {
	c_iterator _iter, _end;
	psmlite_configstate_t _cfgstate;
	_iter = c_map_find (&sm->m_builder->m_states, sm->m_state);
	_end = c_map_end (&sm->m_builder->m_states);
	if (!ITER_EQUAL (_iter, _end)) {
		_cfgstate = (psmlite_configstate_t) ((c_ppair) ITER_REF (_iter))->second;
		return psmlite_configstate_allow_trigger (_cfgstate, trigger);
	}
	return 0;
}

void smlite_triggering (psmlite_t sm, int32_t trigger, ...) {
	if (!smlite_allow_triggering (sm, trigger)) {
		printf ("current state cannot launch this trigger.\n");
		return;
	}
	c_iterator _iter, _end;
	psmlite_configstate_t _cfgstate;
	int32_t _state;
	_iter = c_map_find (&sm->m_builder->m_states, sm->m_state);
	_end = c_map_end (&sm->m_builder->m_states);
	if (!ITER_EQUAL (_iter, _end)) {
		_cfgstate = (psmlite_configstate_t) ((c_ppair) ITER_REF (_iter))->second;
		_state = psmlite_configstate_trigger (_cfgstate, trigger);
		if (_state != sm->m_state) {
			smlite_configstate_on_leave (_cfgstate);
			sm->m_state = _state;
			_iter = c_map_find (&sm->m_builder->m_states, sm->m_state);
			_cfgstate = (psmlite_configstate_t) ((c_ppair) ITER_REF (_iter))->second;
			smlite_configstate_on_entry (_cfgstate);
		}
	}
}
