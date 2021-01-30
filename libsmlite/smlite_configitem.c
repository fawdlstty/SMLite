#include <malloc.h>
#include <stdio.h>
#include <stdlib.h>

#include "local.h"



psmlite_configitem_t smlite_configitem_create (int32_t state, int32_t trigger, void *callback) {
	psmlite_configitem_t _cfgitem;
	_cfgitem = (psmlite_configitem_t) malloc (sizeof (smlite_configitem_t));
	if (!_cfgitem) {
		printf ("malloc failed.\0");
		return 0;
	}
	memset (_cfgitem, 0, sizeof (smlite_configitem_t));
	_cfgitem->m_state = state;
	_cfgitem->m_trigger = trigger;
	_cfgitem->m_callback = callback;
	return _cfgitem;
}
