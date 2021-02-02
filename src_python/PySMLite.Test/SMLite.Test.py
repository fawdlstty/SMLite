# -*- coding: utf-8 -*-

import asyncio
import os
import sys
from enum import IntEnum
from SMLite import SMLite
from SMLiteBuilder import SMLiteBuilder

class MyState (IntEnum):
	Rest = 0
	Ready = 1
	Reading = 2
	Writing = 3

class MyTrigger (IntEnum):
	Run = 0
	Close = 1
	Read = 2
	FinishRead = 3
	Write = 4
	FinishWrite = 5



class Assert (object):
	@staticmethod
	def IsTrue (val):
		if not val:
			raise Exception ("Assert Failed")

	@staticmethod
	def IsFalse (val):
		if val:
			raise Exception ("Assert Failed")

	@staticmethod
	def AreEqual (val1, val2):
		if val1 != val2:
			raise Exception ("Assert Failed")



n = 0
entry_one = True
s = ""

def TestMethod1 ():
	global n, entry_one
	n = 0
	entry_one = True
	def _rest_entry ():
		global n, entry_one
		Assert.IsTrue (not entry_one)
		entry_one = True
		n += 1
	def _rest_leave ():
		global n, entry_one
		Assert.IsTrue (entry_one)
		entry_one = False
		n += 10
	def _ready_entry ():
		global n, entry_one
		Assert.IsTrue (not entry_one)
		entry_one = True
		n += 100
	def _ready_leave ():
		global n, entry_one
		Assert.IsTrue (entry_one)
		entry_one = False
		n += 1000
	def _reading_entry ():
		global n, entry_one
		Assert.IsTrue (not entry_one)
		entry_one = True
		n += 10000
	def _reading_leave ():
		global n, entry_one
		Assert.IsTrue (entry_one)
		entry_one = False
		n += 100000
	def _writing_entry ():
		global n, entry_one
		Assert.IsTrue (not entry_one)
		entry_one = True
		n += 1000000
	def _writing_leave ():
		global n, entry_one
		Assert.IsTrue (entry_one)
		entry_one = False
		n += 10000000

	_smb = SMLiteBuilder ()
	_smb.Configure (MyState.Rest)\
		.OnEntry (_rest_entry)\
		.OnLeave (_rest_leave)\
		.WhenChangeTo (MyTrigger.Run, MyState.Ready)\
		.WhenIgnore (MyTrigger.Close)
	_smb.Configure (MyState.Ready)\
		.OnEntry (_ready_entry)\
		.OnLeave (_ready_leave)\
		.WhenChangeTo (MyTrigger.Read, MyState.Reading)\
		.WhenChangeTo (MyTrigger.Write, MyState.Writing)\
		.WhenChangeTo (MyTrigger.Close, MyState.Rest)
	_smb.Configure (MyState.Reading)\
		.OnEntry (_reading_entry)\
		.OnLeave (_reading_leave)\
		.WhenChangeTo (MyTrigger.FinishRead, MyState.Ready)\
		.WhenChangeTo (MyTrigger.Close, MyState.Rest)
	_smb.Configure (MyState.Writing)\
		.OnEntry (_writing_entry)\
		.OnLeave (_writing_leave)\
		.WhenChangeTo (MyTrigger.FinishWrite, MyState.Ready)\
		.WhenChangeTo (MyTrigger.Close, MyState.Rest)

	_sm = _smb.Build (MyState.Rest)
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	Assert.AreEqual (n, 0)
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Run))
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Read))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Write))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite))

	_sm.Triggering (MyTrigger.Close)
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	Assert.AreEqual (n, 0)

	_sm.Triggering (MyTrigger.Close)
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	Assert.AreEqual (n, 0)

	_sm.Triggering (MyTrigger.Close)
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	Assert.AreEqual (n, 0)

	_sm.Triggering (MyTrigger.Run)
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (n, 110)
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Run))
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close))
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Read))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead))
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Write))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite))

	_sm.Triggering (MyTrigger.Close)
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	Assert.AreEqual (n, 1111)
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Run))
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Read))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Write))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite))

	_sm.Triggering (MyTrigger.Run)
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (n, 1221)

	_sm.Triggering (MyTrigger.Read)
	Assert.AreEqual (_sm.GetState (), MyState.Reading)
	Assert.AreEqual (n, 12221)

	_sm.Triggering (MyTrigger.FinishRead)
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (n, 112321)

	_sm.Triggering (MyTrigger.Write)
	Assert.AreEqual (_sm.GetState (), MyState.Writing)
	Assert.AreEqual (n, 1113321)

	_sm.Triggering (MyTrigger.FinishWrite)
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (n, 11113421)

	_sm.SetState (MyState.Reading)
	_sm.SetState (MyState.Writing)
	_sm.SetState (MyState.Rest)
	Assert.AreEqual (n, 11113421)
	print ("TestMethod1 Test Ok")

async def TestMethod2 ():
	global n, entry_one
	n = 0
	entry_one = True
	async def _rest_entry ():
		await asyncio.sleep (0.01)
		global n, entry_one
		Assert.IsTrue (not entry_one)
		entry_one = True
		n += 1
	async def _rest_leave ():
		await asyncio.sleep (0.01)
		global n, entry_one
		Assert.IsTrue (entry_one)
		entry_one = False
		n += 10
	async def _ready_entry ():
		await asyncio.sleep (0.01)
		global n, entry_one
		Assert.IsTrue (not entry_one)
		entry_one = True
		n += 100
	async def _ready_leave ():
		await asyncio.sleep (0.01)
		global n, entry_one
		Assert.IsTrue (entry_one)
		entry_one = False
		n += 1000
	async def _reading_entry ():
		await asyncio.sleep (0.01)
		global n, entry_one
		Assert.IsTrue (not entry_one)
		entry_one = True
		n += 10000
	async def _reading_leave ():
		await asyncio.sleep (0.01)
		global n, entry_one
		Assert.IsTrue (entry_one)
		entry_one = False
		n += 100000
	async def _writing_entry ():
		await asyncio.sleep (0.01)
		global n, entry_one
		Assert.IsTrue (not entry_one)
		entry_one = True
		n += 1000000
	async def _writing_leave ():
		await asyncio.sleep (0.01)
		global n, entry_one
		Assert.IsTrue (entry_one)
		entry_one = False
		n += 10000000

	_smb = SMLiteBuilder ()
	_smb.Configure (MyState.Rest)\
		.OnEntryAsync (_rest_entry)\
		.OnLeaveAsync (_rest_leave)\
		.WhenChangeTo (MyTrigger.Run, MyState.Ready)\
		.WhenIgnore (MyTrigger.Close)
	_smb.Configure (MyState.Ready)\
		.OnEntryAsync (_ready_entry)\
		.OnLeaveAsync (_ready_leave)\
		.WhenChangeTo (MyTrigger.Read, MyState.Reading)\
		.WhenChangeTo (MyTrigger.Write, MyState.Writing)\
		.WhenChangeTo (MyTrigger.Close, MyState.Rest)
	_smb.Configure (MyState.Reading)\
		.OnEntryAsync (_reading_entry)\
		.OnLeaveAsync (_reading_leave)\
		.WhenChangeTo (MyTrigger.FinishRead, MyState.Ready)\
		.WhenChangeTo (MyTrigger.Close, MyState.Rest)
	_smb.Configure (MyState.Writing)\
		.OnEntryAsync (_writing_entry)\
		.OnLeaveAsync (_writing_leave)\
		.WhenChangeTo (MyTrigger.FinishWrite, MyState.Ready)\
		.WhenChangeTo (MyTrigger.Close, MyState.Rest)

	_sm = _smb.Build (MyState.Rest)
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	Assert.AreEqual (n, 0)
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Run))
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Read))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Write))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite))

	await _sm.TriggeringAsync (MyTrigger.Close)
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	Assert.AreEqual (n, 0)

	await _sm.TriggeringAsync (MyTrigger.Close)
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	Assert.AreEqual (n, 0)

	await _sm.TriggeringAsync (MyTrigger.Close)
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	Assert.AreEqual (n, 0)

	await _sm.TriggeringAsync (MyTrigger.Run)
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (n, 110)
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Run))
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close))
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Read))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead))
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Write))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite))

	await _sm.TriggeringAsync (MyTrigger.Close)
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	Assert.AreEqual (n, 1111)
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Run))
	Assert.IsTrue (_sm.AllowTriggering (MyTrigger.Close))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Read))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishRead))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.Write))
	Assert.IsFalse (_sm.AllowTriggering (MyTrigger.FinishWrite))

	await _sm.TriggeringAsync (MyTrigger.Run)
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (n, 1221)

	await _sm.TriggeringAsync (MyTrigger.Read)
	Assert.AreEqual (_sm.GetState (), MyState.Reading)
	Assert.AreEqual (n, 12221)

	await _sm.TriggeringAsync (MyTrigger.FinishRead)
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (n, 112321)

	await _sm.TriggeringAsync (MyTrigger.Write)
	Assert.AreEqual (_sm.GetState (), MyState.Writing)
	Assert.AreEqual (n, 1113321)

	await _sm.TriggeringAsync (MyTrigger.FinishWrite)
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (n, 11113421)

	_sm.SetState (MyState.Reading)
	_sm.SetState (MyState.Writing)
	_sm.SetState (MyState.Rest)
	Assert.AreEqual (n, 11113421)
	print ("TestMethod2 Test Ok")

def TestMethod3 ():
	global s
	s = ""
	def _rest_run (_state, _trigger):
		global s
		s = "WhenFunc_Run"
		return MyState.Ready
	def _rest_read (_state, _trigger, _p1):
		global s
		s = _p1
		return MyState.Ready
	def _rest_finishread (_state, _trigger, _p1, _p2):
		global s
		s = _p1 + str (_p2)
		return MyState.Ready
	def _rest_close (_state, _trigger):
		global s
		s = "WhenAction_Close"
	def _rest_write (_state, _trigger, _p1):
		global s
		s = _p1
	def _rest_finishwrite (_state, _trigger, _p1, _p2):
		global s
		s = _p1 + str (_p2)

	_smb = SMLiteBuilder ()
	_smb.Configure (MyState.Rest)\
		.WhenFunc (MyTrigger.Run, _rest_run)\
		.WhenFunc (MyTrigger.Read, _rest_read)\
		.WhenFunc (MyTrigger.FinishRead, _rest_finishread)\
		.WhenAction (MyTrigger.Close, _rest_close)\
		.WhenAction (MyTrigger.Write, _rest_write)\
		.WhenAction (MyTrigger.FinishWrite, _rest_finishwrite)
	_smb.Configure (MyState.Ready)\
		.WhenChangeTo (MyTrigger.Close, MyState.Rest)

	_sm = _smb.Build (MyState.Rest)
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	Assert.AreEqual (s, "")

	_sm.Triggering (MyTrigger.Run)
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (s, "WhenFunc_Run")
	_sm.Triggering (MyTrigger.Close)

	_sm.Triggering (MyTrigger.Read, "hello")
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (s, "hello")
	_sm.Triggering (MyTrigger.Close)

	_sm.Triggering (MyTrigger.FinishRead, "hello", 1)
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (s, "hello1")
	_sm.Triggering (MyTrigger.Close)

	_sm.Triggering (MyTrigger.Close)
	Assert.AreEqual (s, "WhenAction_Close")
	Assert.AreEqual (_sm.GetState (), MyState.Rest)

	_sm.Triggering (MyTrigger.Write, "world")
	Assert.AreEqual (s, "world")
	Assert.AreEqual (_sm.GetState (), MyState.Rest)

	_sm.Triggering (MyTrigger.FinishWrite, "world", 1)
	Assert.AreEqual (s, "world1")
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	print ("TestMethod3 Test Ok")

async def TestMethod4 ():
	global s
	s = ""
	async def _rest_run (_state, _trigger):
		await asyncio.sleep (0.01)
		global s
		s = "WhenFunc_Run"
		return MyState.Ready
	async def _rest_read (_state, _trigger, _p1):
		await asyncio.sleep (0.01)
		global s
		s = _p1
		return MyState.Ready
	async def _rest_finishread (_state, _trigger, _p1, _p2):
		await asyncio.sleep (0.01)
		global s
		s = _p1 + str (_p2)
		return MyState.Ready
	async def _rest_close (_state, _trigger):
		await asyncio.sleep (0.01)
		global s
		s = "WhenAction_Close"
	async def _rest_write (_state, _trigger, _p1):
		await asyncio.sleep (0.01)
		global s
		s = _p1
	async def _rest_finishwrite (_state, _trigger, _p1, _p2):
		await asyncio.sleep (0.01)
		global s
		s = _p1 + str (_p2)

	_smb = SMLiteBuilder ()
	_smb.Configure (MyState.Rest)\
		.WhenFuncAsync (MyTrigger.Run, _rest_run)\
		.WhenFuncAsync (MyTrigger.Read, _rest_read)\
		.WhenFuncAsync (MyTrigger.FinishRead, _rest_finishread)\
		.WhenActionAsync (MyTrigger.Close, _rest_close)\
		.WhenActionAsync (MyTrigger.Write, _rest_write)\
		.WhenActionAsync (MyTrigger.FinishWrite, _rest_finishwrite)
	_smb.Configure (MyState.Ready)\
		.WhenChangeTo (MyTrigger.Close, MyState.Rest)

	_sm = _smb.Build (MyState.Rest)
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	Assert.AreEqual (s, "")

	await _sm.TriggeringAsync (MyTrigger.Run)
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (s, "WhenFunc_Run")
	await _sm.TriggeringAsync (MyTrigger.Close)

	await _sm.TriggeringAsync (MyTrigger.Read, "hello")
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (s, "hello")
	await _sm.TriggeringAsync (MyTrigger.Close)

	await _sm.TriggeringAsync (MyTrigger.FinishRead, "hello", 1)
	Assert.AreEqual (_sm.GetState (), MyState.Ready)
	Assert.AreEqual (s, "hello1")
	await _sm.TriggeringAsync (MyTrigger.Close)

	await _sm.TriggeringAsync (MyTrigger.Close)
	Assert.AreEqual (s, "WhenAction_Close")
	Assert.AreEqual (_sm.GetState (), MyState.Rest)

	await _sm.TriggeringAsync (MyTrigger.Write, "world")
	Assert.AreEqual (s, "world")
	Assert.AreEqual (_sm.GetState (), MyState.Rest)

	await _sm.TriggeringAsync (MyTrigger.FinishWrite, "world", 1)
	Assert.AreEqual (s, "world1")
	Assert.AreEqual (_sm.GetState (), MyState.Rest)
	print ("TestMethod4 Test Ok")

if __name__ == '__main__':
	TestMethod1 ()
	asyncio.run (TestMethod2 ())
	TestMethod3 ()
	asyncio.run (TestMethod4 ())
	input ("Test success, press ENTER to exit.")
