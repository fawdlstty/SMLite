#include <malloc.h>
#include <stdio.h>
#include <stdlib.h>

#include "libsmlite.h"



psmlite_t smlite_create (psmlite_builder_t builder, int32_t init_state) {
	psmlite_t _sm;
	if (!builder) {
		printf ("parameter connot be null.\n");
		return 0;
	}
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
	if (!sm) {
		printf ("parameter connot be null.\n");
		return -1;
	}
	return sm->m_state;
}

void smlite_set_state (psmlite_t sm, int32_t new_state) {
	if (!sm) {
		printf ("parameter connot be null.\n");
		return;
	}
	sm->m_state = new_state;
}

int smlite_allow_triggering (psmlite_t sm, int32_t trigger) {
	c_iterator _iter, _end;
	psmlite_configstate_t _cfgstate;
	if (!sm) {
		printf ("parameter connot be null.\n");
		return 0;
	}
	_iter = c_map_find (&sm->m_builder->m_states, (value_type) sm->m_state);
	_end = c_map_end (&sm->m_builder->m_states);
	if (!ITER_EQUAL (_iter, _end)) {
		_cfgstate = (psmlite_configstate_t) ((c_ppair) ITER_REF (_iter))->second;
		_iter = c_map_find (&_cfgstate->m_items, (value_type) trigger);
		_end = c_map_end (&_cfgstate->m_items);
		if (!ITER_EQUAL (_iter, _end))
			return 1;
	}
	return 0;
}

//#define smlite_triggering(sm,trigger,...) {															\
//	c_iterator _iter, _end;																			\
//	psmlite_configstate_t _cfgstate;																\
//	psmlite_configitem_t _cfgitem;																	\
//	int32_t _state;																					\
//	if (!sm) {																						\
//		printf ("parameter connot be null.\n");														\
//		return;																						\
//	}																								\
//	if (!smlite_allow_triggering (sm, trigger)) {													\
//		printf ("current state cannot launch this trigger.\n");										\
//		return;																						\
//	}																								\
//	_iter = c_map_find (&sm->m_builder->m_states, (value_type) sm->m_state);						\
//	_end = c_map_end (&sm->m_builder->m_states);													\
//	if (!ITER_EQUAL (_iter, _end)) {																\
//		_cfgstate = (psmlite_configstate_t) ((c_ppair) ITER_REF (_iter))->second;					\
//		_iter = c_map_find (&_cfgstate->m_items, (value_type) trigger);								\
//		_end = c_map_end (&_cfgstate->m_items);														\
//		if (!ITER_EQUAL (_iter, _end)) {															\
//			_cfgitem = (psmlite_configitem_t) ((c_ppair) ITER_REF (_iter))->second;					\
//			_state = smlite_configitem_call (_cfgitem);												\
//			if (_state != sm->m_state) {															\
//				smlite_configstate_on_leave (_cfgstate);											\
//				sm->m_state = _state;																\
//				_iter = c_map_find (&sm->m_builder->m_states, (value_type) sm->m_state);			\
//				_cfgstate = (psmlite_configstate_t) ((c_ppair) ITER_REF (_iter))->second;			\
//				smlite_configstate_on_entry (_cfgstate);											\
//			}																						\
//		}																							\
//	}																								\
//}
