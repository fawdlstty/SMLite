#ifndef __LOCAL_H__
#define __LOCAL_H__

#ifdef __cplusplus
extern "C" {
#endif

#include <stdarg.h>
#include <stdint.h>

// https://sourceforge.net/projects/tstl2cl/
#include "tstl2cl/include/c_map.h"



typedef void (*notify_func_t) ();
typedef int32_t (*whenfunc_t) (int32_t, int32_t);
typedef int32_t (*whenfunc_arg_t) (int32_t, int32_t, va_list);
typedef void (*whenaction_t) (int32_t, int32_t);
typedef void (*whenaction_arg_t) (int32_t, int32_t, va_list);

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
} smlite_configitem_t, *psmlite_configitem_t;

#define LIBSMLITE_IMPL 1
#include "libsmlite.h"



int _int32_key_comparer (value_type x, value_type y);

// smlite builder
psmlite_builder_t		smlite_builder_create ();
void					smlite_builder_delete (psmlite_builder_t *pbuilder);
psmlite_configstate_t	smlite_builder_configure (psmlite_builder_t builder, int32_t state);
psmlite_t				smlite_builder_build (psmlite_builder_t builder, int32_t init_state);

// smlite
psmlite_t				smlite_create (psmlite_builder_t builder, int32_t init_state);
void					smlite_delete (psmlite_t *psm);
int32_t					smlite_get_state (psmlite_t sm);
void					smlite_set_state (psmlite_t sm, int32_t new_state);
int						smlite_allow_triggering (psmlite_t sm, int32_t trigger);
void					smlite_triggering (psmlite_t sm, int32_t trigger, ...);



#ifdef __cplusplus
};
#endif

#endif //__LOCAL_H__
