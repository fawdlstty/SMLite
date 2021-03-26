# -*- coding: utf-8 -*-

from ItemStruct._SMLite_ConfigStateAsync import _SMLite_ConfigStateAsync

class SMLiteAsync (object):
	def __init__ (self, init_state, states):
		self.__state = init_state
		self.__states = states

	def AllowTriggering (self, trigger):
		if self.__state in self.__states:
			return self.__states [self.__state]._allow_trigger (trigger)
		return False

	async def TriggeringAsync (self, trigger, *args):
		if self.AllowTriggering (trigger):
			_new_state = await self.__states[self.__state]._trigger_async (trigger, *args)
			if _new_state != self.__state:
				if self.__states[self.__state].f__on_leave_async != None:
					await self.__states[self.__state].f__on_leave_async ()
				self.__state = _new_state
				if self.__states[self.__state].f__on_entry_async != None:
					await self.__states[self.__state].f__on_entry_async ()
			return
		raise Exception ("not match function found.")

	def GetState (self):
		return self.__state

	def SetState (self, state):
		self.__state = state
