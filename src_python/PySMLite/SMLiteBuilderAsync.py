# -*- coding: utf-8 -*-

from SMLiteAsync import SMLiteAsync
from ItemStruct._SMLite_ConfigStateAsync import _SMLite_ConfigStateAsync

class SMLiteBuilderAsync (object):
	def __init__ (self):
		self.__states = {}
		self.__builded = False

	def Configure (self, state):
		if self.__builded:
			raise Exception ("shouldn't configure builder after builded.")
		if state in self.__states:
			raise Exception ("state is already exists.")
		_state = _SMLite_ConfigStateAsync (state)
		self.__states [state] = _state
		return _state

	def Build (self, init_state):
		self.__builded = True
		return SMLiteAsync (init_state, self.__states)
