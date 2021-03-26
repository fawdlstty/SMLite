# -*- coding: utf-8 -*-

from ._SMLite_BuildItem import _SMLite_BuildItem

class _SMLite_ConfigItemAsync (object):
	def __init__ (self, build_item, state, trigger, callback, is_async):
		self.__build_item = build_item
		self.__state = state
		self.__trigger = trigger
		self.__callback = callback
		self.__is_async = is_async

	async def _call_async (self, *args):
		_ret = self.__state
		if self.__is_async:
			if self.__build_item == _SMLite_BuildItem.NONE:
				_ret = await self.__callback (*args)
			elif self.__build_item == _SMLite_BuildItem.STATE:
				_ret = await self.__callback (self.__state, *args)
			elif self.__build_item == _SMLite_BuildItem.TRIGGER:
				_ret = await self.__callback (self.__trigger, *args)
			else:
				_ret = await self.__callback (self.__state, self.__trigger, *args)
		else:
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
