#include <malloc.h>
#include <stdio.h>
#include <stdlib.h>

#include "local.h"



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
	c_map_destroy (&((*pcfgstate)->m_items));
	free (*pcfgstate);
	*pcfgstate = 0;
}

void smlite_configstate_whenfunc (psmlite_configstate_t cfgstate, int trigger, whenfunc_t callback) {

}

void smlite_configstate_whenfunc (psmlite_configstate_t cfgstate, int trigger, whenfunc_arg_t callback) {

}
