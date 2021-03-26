# -*- coding: utf-8 -*-

from ._SMLite_ConfigItem import _SMLite_ConfigItem
from ._SMLite_BuildItem import _SMLite_BuildItem

class _SMLite_ConfigState (object):
	def __init__ (self, state):
		self.f__on_entry = None
		self.f__on_leave = None
		self.__state = state
		self.__items = {}

	def _try_add_trigger (self, trigger, item):
		if trigger in self.__items:
			raise Exception ("state is already has this trigger methods.")
		self.__items[trigger] = item
		return self

	def WhenChangeTo (self, trigger, new_state):
		_callback = lambda : new_state
		_item = _SMLite_ConfigItem (_SMLite_BuildItem.NONE, self.__state, trigger, _callback)
		return self._try_add_trigger (trigger, _item)

	def WhenIgnore (self, trigger):
		_callback = lambda : None
		_item = _SMLite_ConfigItem (_SMLite_BuildItem.NONE, self.__state, trigger, _callback)
		return self._try_add_trigger (trigger, _item)

	def WhenFunc (self, trigger, callback):
		_item = _SMLite_ConfigItem (_SMLite_BuildItem.STATE | _SMLite_BuildItem.TRIGGER, self.__state, trigger, callback)
		return self._try_add_trigger (trigger, _item)

	def WhenAction (self, trigger, callback):
		_item = _SMLite_ConfigItem (_SMLite_BuildItem.STATE | _SMLite_BuildItem.TRIGGER, self.__state, trigger, callback)
		return self._try_add_trigger (trigger, _item)

	def OnEntry (self, callback):
		if self.f__on_entry != None:
			raise Exception ("OnEntry is already have been set.")
		self.f__on_entry = callback
		return self

	def OnLeave (self, callback):
		if self.f__on_leave != None:
			raise Exception ("OnLeave is already have been set.")
		self.f__on_leave = callback
		return self

	def _allow_trigger (self, trigger):
		return trigger in self.__items

	def _trigger (self, trigger, *args):
		if self._allow_trigger (trigger):
			return self.__items[trigger]._call (*args)
		raise Exception ("not match function found.")
