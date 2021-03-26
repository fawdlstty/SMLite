# -*- coding: utf-8 -*-

from ._SMLite_BuildItem import _SMLite_BuildItem

class _SMLite_ConfigItem (object):
	def __init__ (self, build_item, state, trigger, callback):
		self.__build_item = build_item
		self.__state = state
		self.__trigger = trigger
		self.__callback = callback

	def _call (self, *args):
		_ret = self.__state
		if self.__build_item == _SMLite_BuildItem.NONE:
			_ret = self.__callback (*args)
		elif self.__build_item == _SMLite_BuildItem.STATE:
			_ret = self.__callback (self.__state, *args)
		elif self.__build_item == _SMLite_BuildItem.TRIGGER:
			_ret = self.__callback (self.__trigger, *args)
		else:
			_ret = self.__callback (self.__state, self.__trigger, *args)

		if _ret == None:
			_ret = self.__state
		return _ret
