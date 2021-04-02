# -*- coding: utf-8 -*-

import asyncio
from ItemStruct._SMLite_ConfigState import _SMLite_ConfigState

class SMLite (object):
	"""
SMLite
State machine library for C, C++, C#, Java, JavaScript, Python, VB.Net
Author: Fawdlstty
Version 0.1.6

Source Repository            <https://github.com/fawdlstty/SMLite>
Report                       <https://github.com/fawdlstty/SMLite/issues>
MIT License                  <https://opensource.org/licenses/MIT>
Copyright (C) 2021 Fawdlstty <https://www.fawdlstty.com>
"""

	def __init__ (self, init_state, states):
		self.__state = init_state
		self.__states = states

	def AllowTriggering (self, trigger):
		if self.__state in self.__states:
			return self.__states [self.__state]._allow_trigger (trigger)
		return False

	def Triggering (self, trigger, *args):
		if self.AllowTriggering (trigger):
			_cfgstate = self.__states[self.__state]
			_new_state = _cfgstate._trigger (trigger, *args)
			if _new_state != self.__state:
				if _cfgstate.f__on_leave != None:
					_cfgstate.f__on_leave ()
				self.__state = _new_state
				_cfgstate = self.__states[self.__state]
				if _cfgstate.f__on_entry != None:
					_cfgstate.f__on_entry ()
			return
		raise Exception ("not match function found.")

	def GetState (self):
		return self.__state

	def SetState (self, state):
		self.__state = state

if __name__ == '__main__':
	print (SMLite.__doc__)
