# -*- coding: utf-8 -*-

import asyncio
from ItemStruct._SMLite_ConfigState import _SMLite_ConfigState

class SMLite (object):
	"""
SMLite
State machine library for C, C++, C#, JavaScript, Python, VB.Net
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
				if _cfgstate.f__on_leave_async != None:
					asyncio.run (_cfgstate.f__on_leave_async ())
				self.__state = _new_state
				_cfgstate = self.__states[self.__state]
				if _cfgstate.f__on_entry != None:
					_cfgstate.f__on_entry ()
				if _cfgstate.f__on_entry_async != None:
					asyncio.run (_cfgstate.f__on_entry_async ())
			return
		raise Exception ("not match function found.")

	async def TriggeringAsync (self, trigger, *args):
		if self.AllowTriggering (trigger):
			_new_state = await self.__states[self.__state]._trigger_async (trigger, *args)
			if _new_state != self.__state:
				if self.__states[self.__state].f__on_leave != None:
					self.__states[self.__state].f__on_leave ()
				if self.__states[self.__state].f__on_leave_async != None:
					await self.__states[self.__state].f__on_leave_async ()
				self.__state = _new_state
				if self.__states[self.__state].f__on_entry != None:
					self.__states[self.__state].f__on_entry ()
				if self.__states[self.__state].f__on_entry_async != None:
					await self.__states[self.__state].f__on_entry_async ()
			return
		raise Exception ("not match function found.")

	def GetState (self):
		return self.__state

	def SetState (self, state):
		self.__state = state

if __name__ == '__main__':
	print (SMLite.__doc__)
