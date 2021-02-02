# -*- coding: utf-8 -*-

from SMLite import SMLite
from ItemStruct._SMLite_ConfigState import _SMLite_ConfigState

class SMLiteBuilder (object):
	def __init__ (self):
		self.__states = {}
		self.__builded = False

	def Configure (self, state):
		if self.__builded:
			raise Exception ("shouldn't configure builder after builded.")
		if state in self.__states:
			raise Exception ("state is already exists.")
		_state = _SMLite_ConfigState (state)
		self.__states [state] = _state
		return _state

	def Build (self, init_state):
		self.__builded = True
		return SMLite (init_state, self.__states)

if __name__ == '__main__':
	print (SMLite.__doc__)
