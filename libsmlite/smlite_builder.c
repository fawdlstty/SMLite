#include <malloc.h>
#include <stdio.h>
#include <stdlib.h>

#include "local.h"



int _int32_key_comparer (value_type x, value_type y) {
	return (int32_t) x - (int32_t) y;
}



psmlite_builder_t smlite_builder_create () {
	psmlite_builder_t _builder;
	_builder = (psmlite_builder_t) malloc (sizeof (smlite_builder_t));
	if (!_builder) {
		printf ("malloc failed.\0");
		return 0;
	}
	memset (_builder, 0, sizeof (smlite_builder_t));
	c_map_create (&_builder->m_states, _int32_key_comparer);
	_builder->m_builded = 0;
	_builder->m_ref_count = 1;
	return _builder;
}

void smlite_builder_delete (psmlite_builder_t *pbuilder) {
	if ((!pbuilder) || (!*pbuilder))
		return;
	(*pbuilder)->m_ref_count -= 1;
	if ((*pbuilder)->m_ref_count == 0) {
		c_map_destroy (&((*pbuilder)->m_states));
		free (*pbuilder);
	}
	*pbuilder = 0;
}

psmlite_configstate_t smlite_builder_configure (psmlite_builder_t builder, int32_t state) {
	c_iterator _iter, _end;
	psmlite_configstate_t _ptr;
	c_pair *_pair;
	if (builder->m_builded) {
		printf ("shouldn't configure builder after builded.\n");
		return 0;
	}
	_iter = c_map_find (&builder->m_states, state);
	_end = c_map_end (&builder->m_states);
	if (!ITER_EQUAL (_iter, _end)) {
		printf ("state is already exists.\0");
		return 0;
	}
	_ptr = smlite_configstate_create (state);
	if (_ptr == 0) {
		return 0;
	}
	//_ptr = (psmlite_configstate_t) malloc (sizeof (smlite_configstate_t));
	//if (!_ptr) {
	//	printf ("malloc failed.\0");
	//	return 0;
	//}
	//memset (_ptr, 0, sizeof (smlite_configstate_t));
	//smlite_configstate_init (_ptr, state);
	_pair = (c_pair *) malloc (sizeof (c_pair));
	if (!_pair) {
		printf ("malloc failed.\0");
		return 0;
	}
	memset (_pair, 0, sizeof (c_pair));
	_pair->first = state;
	_pair->second = _ptr;
	c_map_insert (&builder->m_states, _pair);
	return _ptr;
}

psmlite_t smlite_builder_build (psmlite_builder_t builder, int32_t init_state) {
	builder->m_builded = true;
	return smlite_create (builder, init_state);
}
