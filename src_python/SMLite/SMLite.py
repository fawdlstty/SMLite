# -*- coding: utf-8 -*-

# SMLite
# State machine library for C, C++, C#, Python
# Author: Fawdlstty
# Version 0.1.5
# 
# Source Repository            <https://github.com/fawdlstty/SMLite>
# Report                       <https://github.com/fawdlstty/SMLite/issues>
# MIT License                  <https://opensource.org/licenses/MIT>
# Copyright (C) 2021 Fawdlstty <https://www.fawdlstty.com>



from ItemStruct._SMLite_ConfigState import _SMLite_ConfigState

class SMLite (object):
	@staticmethod
	def print_info ():
		print ("SMLite")
		print ("State machine library for C++ & C# & Python")
		print ("Author: Fawdlstty")
		print ("Version 0.1.5")
		print ("")
		print ("Source Repository            <https://github.com/fawdlstty/SMLite>")
		print ("Report                       <https://github.com/fawdlstty/SMLite/issues>")
		print ("MIT License                  <https://opensource.org/licenses/MIT>")
		print ("Copyright (C) 2021 Fawdlstty <https://www.fawdlstty.com>")
		print ("")

	def __init__ (self, init_state, states):
		self.__state = init_state
		self.__states = states

	def AllowTriggering (self, trigger):
		if self.__state in self.__states:
			return self.__states [self.__state]._allow_trigger (trigger)
		return False

	def Triggering (self, trigger, *args):
		if self.AllowTriggering (trigger):
			_new_state = self.__states[self.__state]._trigger (trigger, *args)
			if _new_state != self.__state:
				if self.__states[self.__state].m__on_leave != None:
					self.__states[self.__state].m__on_leave ()
				self.__state = _new_state
				if self.__states[self.__state].m__on_entry != None:
					self.__states[self.__state].m__on_entry ()
			return
		raise Exception ("not match function found.")

	def GetState (self):
		return self.__state

	def SetState (self, state):
		self.__state = state

if __name__ == '__main__':
	SMLite.print_info ()
