#ifndef __LIBSMLITE_H__
#define __LIBSMLITE_H__

#ifdef __cplusplus
extern "C" {
#endif

#include <stdarg.h>
#include <stdint.h>



#ifndef LIBSMLITE_IMPL
	typedef void *psmlite_builder_t, *psmlite_t, *psmlite_configstate_t;
#endif

	psmlite_builder_t smlite_builder_create ();
	void smlite_builder_delete (psmlite_builder_t *pbuilder);
	psmlite_configstate_t smlite_builder_configure (psmlite_builder_t builder, int32_t state);
	psmlite_t smlite_builder_build (psmlite_builder_t builder, int32_t init_state);



#ifdef __cplusplus
}
#endif

#endif //__LIBSMLITE_H__
