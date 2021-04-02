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

#ifndef __LIBSMLITE_H__
#define __LIBSMLITE_H__

#ifdef __cplusplus
extern "C" {
#endif

#include <stdarg.h>
#include <stdint.h>

// https://sourceforge.net/projects/tstl2cl/
#include "tstl2cl/include/c_map.h"



typedef void (*notify_func_t) ();
typedef int32_t (*whenfunc_t) (int32_t, int32_t, ...);
typedef void (*whenaction_t) (int32_t, int32_t, ...);

typedef struct {
	c_map				m_states;
	int					m_builded;
	int					m_ref_count;
} smlite_builder_t, *psmlite_builder_t;

typedef struct {
	int32_t				m_state;
	psmlite_builder_t	m_builder;
} smlite_t, *psmlite_t;

typedef struct {
	notify_func_t		m_on_entry;
	notify_func_t		m_on_leave;
	int32_t				m_state;
	c_map				m_items;
} smlite_configstate_t, *psmlite_configstate_t;

typedef struct {
	int32_t				m_state;
	int32_t				m_trigger;
	void				*m_callback;
	int					m_has_ret;
} smlite_configitem_t, *psmlite_configitem_t;



int _int32_key_comparer (value_type x, value_type y);

// psmlite configitem
psmlite_configitem_t	smlite_configitem_create (int32_t state, int32_t trigger, void* callback, int has_ret);
void					smlite_configitem_delete (psmlite_configitem_t* pcfgitem);
#define					smlite_configitem_call(cfgitem,_ret,...) {			\
	if (!cfgitem) {															\
		printf ("parameter connot be null.\n");								\
		_ret = -1;															\
	} else if (cfgitem->m_has_ret) {										\
		whenfunc_t pfunc = (whenfunc_t) cfgitem->m_callback;				\
		_ret = pfunc (cfgitem->m_state, cfgitem->m_trigger, __VA_ARGS__);	\
	} else {																\
		whenaction_t pfunc = (whenaction_t) cfgitem->m_callback;			\
		pfunc (cfgitem->m_state, cfgitem->m_trigger, __VA_ARGS__);			\
		_ret = cfgitem->m_state;											\
	}																		\
}

// psmlite configstate
psmlite_configstate_t	smlite_configstate_create (int32_t init_state);
void					smlite_configstate_delete (psmlite_configstate_t* pcfgstate);
void					smlite_configstate_when_func (psmlite_configstate_t cfgstate, int32_t trigger, whenfunc_t callback);
void					smlite_configstate_when_action (psmlite_configstate_t cfgstate, int32_t trigger, whenaction_t callback);
void					smlite_configstate_when_change_to (psmlite_configstate_t cfgstate, int32_t trigger, int32_t new_state);
void					smlite_configstate_when_ignore (psmlite_configstate_t cfgstate, int32_t trigger);
void					smlite_configstate_on_entry (psmlite_configstate_t cfgstate, notify_func_t callback);
void					smlite_configstate_on_leave (psmlite_configstate_t cfgstate, notify_func_t callback);

// smlite
psmlite_t				smlite_create (psmlite_builder_t builder, int32_t init_state);
void					smlite_delete (psmlite_t *psm);
int32_t					smlite_get_state (psmlite_t sm);
void					smlite_set_state (psmlite_t sm, int32_t new_state);
int						smlite_allow_triggering (psmlite_t sm, int32_t trigger);
#define					smlite_triggering(sm,trigger,...) {											\
	c_iterator _iter, _end;																			\
	psmlite_configstate_t _cfgstate;																\
	psmlite_configitem_t _cfgitem;																	\
	int32_t _state;																					\
	if (!sm) {																						\
		printf ("parameter connot be null.\n");														\
		return;																						\
	}																								\
	if (!smlite_allow_triggering (sm, trigger)) {													\
		printf ("current state cannot launch this trigger.\n");										\
		return;																						\
	}																								\
	_iter = c_map_find (&sm->m_builder->m_states, (value_type) sm->m_state);						\
	_end = c_map_end (&sm->m_builder->m_states);													\
	if (!ITER_EQUAL (_iter, _end)) {																\
		_cfgstate = (psmlite_configstate_t) ((c_ppair) ITER_REF (_iter))->second;					\
		_iter = c_map_find (&_cfgstate->m_items, (value_type) trigger);								\
		_end = c_map_end (&_cfgstate->m_items);														\
		if (!ITER_EQUAL (_iter, _end)) {															\
			_cfgitem = (psmlite_configitem_t) ((c_ppair) ITER_REF (_iter))->second;					\
			smlite_configitem_call (_cfgitem, _state, __VA_ARGS__);									\
			if (_state != sm->m_state) {															\
				if (_cfgstate->m_on_leave)															\
					_cfgstate->m_on_leave ();														\
				sm->m_state = _state;																\
				_iter = c_map_find (&sm->m_builder->m_states, (value_type) sm->m_state);			\
				_cfgstate = (psmlite_configstate_t) ((c_ppair) ITER_REF (_iter))->second;			\
				if (_cfgstate->m_on_entry)															\
					_cfgstate->m_on_entry ();														\
			}																						\
		}																							\
	}																								\
}

// smlite builder
psmlite_builder_t		smlite_builder_create ();
void					smlite_builder_delete (psmlite_builder_t* pbuilder);
psmlite_configstate_t	smlite_builder_configure (psmlite_builder_t builder, int32_t state);
psmlite_t				smlite_builder_build (psmlite_builder_t builder, int32_t init_state);

#ifdef __cplusplus
}
#endif

#endif //__LIBSMLITE_H__
