# -*- coding: utf-8 -*-

from ._SMLite_ConfigItemAsync import _SMLite_ConfigItemAsync
from ._SMLite_BuildItem import _SMLite_BuildItem

class _SMLite_ConfigStateAsync (object):
	def __init__ (self, state):
		self.f__on_entry_async = None
		self.f__on_leave_async = None
		self.__state = state
		self.__items = {}

	def _try_add_trigger (self, trigger, item):
		if trigger in self.__items:
			raise Exception ("state is already has this trigger methods.")
		self.__items[trigger] = item
		return self

	def WhenChangeTo (self, trigger, new_state):
		_callback = lambda : new_state
		_item = _SMLite_ConfigItemAsync (_SMLite_BuildItem.NONE, self.__state, trigger, _callback, False)
		return self._try_add_trigger (trigger, _item)

	def WhenIgnore (self, trigger):
		_callback = lambda : None
		_item = _SMLite_ConfigItemAsync (_SMLite_BuildItem.NONE, self.__state, trigger, _callback, False)
		return self._try_add_trigger (trigger, _item)

	def WhenFuncAsync (self, trigger, callback):
		_item = _SMLite_ConfigItemAsync (_SMLite_BuildItem.STATE | _SMLite_BuildItem.TRIGGER, self.__state, trigger, callback, True)
		return self._try_add_trigger (trigger, _item)

	def WhenActionAsync (self, trigger, callback):
		_item = _SMLite_ConfigItemAsync (_SMLite_BuildItem.STATE | _SMLite_BuildItem.TRIGGER, self.__state, trigger, callback, True)
		return self._try_add_trigger (trigger, _item)

	def OnEntryAsync (self, callback):
		if self.f__on_entry_async != None:
			raise Exception ("OnEntry is already have been set.")
		self.f__on_entry_async = callback
		return self

	def OnLeaveAsync (self, callback):
		if self.f__on_leave_async != None:
			raise Exception ("OnLeave is already have been set.")
		self.f__on_leave_async = callback
		return self

	def _allow_trigger (self, trigger):
		return trigger in self.__items

	async def _trigger_async (self, trigger, *args):
		if self._allow_trigger (trigger):
			return await self.__items[trigger]._call_async (*args)
		raise Exception ("not match function found.")
