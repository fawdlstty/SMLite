#include <malloc.h>
#include <stdio.h>
#include <stdlib.h>

#include "libsmlite.h"



psmlite_configitem_t smlite_configitem_create (int32_t state, int32_t trigger, void *callback, int has_ret) {
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
	_cfgitem->m_has_ret = has_ret;
	return _cfgitem;
}

void smlite_configitem_delete (psmlite_configitem_t *pcfgitem) {
	if ((!pcfgitem) || (!*pcfgitem))
		return;
	free (*pcfgitem);
	*pcfgitem = 0;
}

//#define smlite_configitem_call(cfgitem,...) {								\
//	if (!cfgitem) {															\
//		printf ("parameter connot be null.\n");								\
//		return -1;															\
//	}																		\
//	if (cfgitem->m_has_ret) {												\
//		whenfunc_t pfunc = (whenfunc_t) cfgitem->m_callback;				\
//		return pfunc (cfgitem->m_state, cfgitem->m_trigger, __VA_ARGS__);	\
//	} else {																\
//		whenaction_t pfunc = (whenaction_t) cfgitem->m_callback;			\
//		pfunc (cfgitem->m_state, cfgitem->m_trigger, __VA_ARGS__);			\
//		return cfgitem->m_state;											\
//	}																		\
//}
