// /////////////////////////////////////////////////////////////
// File: SystemHookCore.cpp  Class: none
// Date: 2/25/2004			 Author: Michael Kennedy
// Language: C++			 Framework: Win32
//
// Copyright: Copyright (c) Michael Kennedy, 2004-2005
// /////////////////////////////////////////////////////////////
// License: See License.txt file included with application.
// Description: See compiled documentation (Managed Hooks.chm)
// /////////////////////////////////////////////////////////////

#include "stdafx.h"
#include <windows.h>
#include "SystemHookCore.h"
#include "MessageFilter.h"

HookProc UserMouseHookCallback = NULL;
HookProc UserKeyboardHookCallback = NULL;
HookProc UserCBTHookCallback = NULL;
#pragma data_seg("TestData")
	HHOOK hookMouse = NULL;
	HHOOK hookKeyboard = NULL;
	HHOOK hookCBT = NULL;
#pragma data_seg()

#pragma comment(linker,"/section:.TestData,rws")
//
// Store the application instance of this module to pass to
// hook initialization. This is set in DLLMain().
//
HINSTANCE g_appInstance = NULL;

MessageFilter mouseFilter;
MessageFilter keyboardFilter;
MessageFilter cbtFilter;

static LRESULT CALLBACK InternalKeyboardHookCallback(int code, WPARAM wparam, LPARAM lparam);
static LRESULT CALLBACK InternalMouseHookCallback(int code, WPARAM wparam, LPARAM lparam);
static LRESULT CALLBACK InternalCBTHookCallback(int code, WPARAM wparam, LPARAM lparam);

int SetUserHookCallback(HookProc userProc, UINT hookID)
{	
	if (userProc == NULL)
	{
		return HookCoreErrors::SetCallBack::ARGUMENT_ERROR;
	}

	if (hookID == WH_KEYBOARD_LL)
	{
		if (UserKeyboardHookCallback != NULL)
		{
			return HookCoreErrors::SetCallBack::ALREADY_SET;
		}

		UserKeyboardHookCallback = userProc;
		keyboardFilter.Clear();
		return HookCoreErrors::SetCallBack::SUCCESS;
	}
	else if (hookID == WH_MOUSE_LL)
	{
		if (UserMouseHookCallback != NULL)
		{
			return HookCoreErrors::SetCallBack::ALREADY_SET;
		}

		UserMouseHookCallback = userProc;
		mouseFilter.Clear();
		return HookCoreErrors::SetCallBack::SUCCESS;
	}
	else if (hookID == WH_CBT)
	{
		if (UserCBTHookCallback != NULL)
		{
			return HookCoreErrors::SetCallBack::ALREADY_SET;
		}
		UserCBTHookCallback = userProc;
		cbtFilter.Clear();
		return HookCoreErrors::SetCallBack::SUCCESS;
	}
	return HookCoreErrors::SetCallBack::NOT_IMPLEMENTED;
}

bool InitializeHook(UINT hookID, int threadID,HWND destination)
{
	if (g_appInstance == NULL)
	{
		return false;
	}

	if (hookID == WH_KEYBOARD_LL)
	{
		if (UserKeyboardHookCallback == NULL)
		{
			return false;
		}

		hookKeyboard = SetWindowsHookEx(hookID, (HOOKPROC)InternalKeyboardHookCallback, g_appInstance, threadID);
		return hookKeyboard != NULL;
	}
	else if (hookID == WH_MOUSE_LL)
	{
		if (UserMouseHookCallback == NULL)
		{
			return false;
		}

		hookMouse = SetWindowsHookEx(hookID, (HOOKPROC)InternalMouseHookCallback, g_appInstance, threadID);
		return hookMouse != NULL;
	}
	else if (hookID == WH_CBT)
	{
		if (UserCBTHookCallback==NULL)
		{
			return false;
		}

		/*if (GetProp(GetDesktopWindow(), "WILSON_HOOK_HWND_CBT") != NULL)
		{
		SendNotifyMessage((HWND)GetProp(GetDesktopWindow(), "WILSON_HOOK_HWND_CBT"), RegisterWindowMessage("WILSON_HOOK_CBT_REPLACED"), 0, 0);
		}
		SetProp(GetDesktopWindow(), "WILSON_HOOK_HWND_KEYBOARD", destination);*/
		
		hookCBT = SetWindowsHookEx(hookID,(HOOKPROC)InternalCBTHookCallback,g_appInstance,threadID);
		return hookCBT != NULL;
	}
	return false;
}

void UninitializeHook(UINT hookID)
{
	if (hookID == WH_KEYBOARD_LL)
	{
		if (hookKeyboard != NULL)
		{
			UnhookWindowsHookEx(hookKeyboard);
		}
		hookKeyboard = NULL;
	}
	else if (hookID == WH_MOUSE_LL)
	{
		if (hookMouse != NULL)
		{
			UnhookWindowsHookEx(hookMouse);
		}
		hookMouse = NULL;
	}
	else if (hookID == WH_CBT)
	{
		if (hookCBT != NULL)
		{
			UnhookWindowsHookEx(hookCBT);
		}
		hookCBT = NULL;
	}
}

void Dispose(UINT hookID)
{
	if (hookID == WH_KEYBOARD_LL)
	{
		UserKeyboardHookCallback = NULL;
	}
	else if (hookID == WH_MOUSE_LL)
	{
		UserMouseHookCallback = NULL;
	}
	else if (hookID == WH_CBT)
	{
		UserCBTHookCallback = NULL;
	}
}

int FilterMessage(UINT hookID, int message)
{
	if (hookID == WH_KEYBOARD_LL)
	{
		if (keyboardFilter.AddMessage(message))
		{
			return HookCoreErrors::FilterMessage::SUCCESS;
		}
		else
		{
			return HookCoreErrors::FilterMessage::FAILED;
		}
	}
	else if (hookID == WH_MOUSE_LL)
	{
		if(mouseFilter.AddMessage(message))
		{
			return HookCoreErrors::FilterMessage::SUCCESS;
		}
		else
		{
			return HookCoreErrors::FilterMessage::FAILED;
		}
	}
	else if (hookID == WH_CBT)
	{
		if (cbtFilter.AddMessage(message))
		{
			return HookCoreErrors::FilterMessage::SUCCESS;
		}
		else
		{
			return HookCoreErrors::FilterMessage::FAILED;
		}
	}

	return HookCoreErrors::FilterMessage::NOT_IMPLEMENTED;
}

static LRESULT CALLBACK InternalMouseHookCallback(int code, WPARAM wparam, LPARAM lparam)
{
	if (code < 0)
	{
		return CallNextHookEx(hookMouse, code, wparam, lparam);
	}

	if (UserMouseHookCallback != NULL && !mouseFilter.IsFiltered((int)wparam))
	{
		UserMouseHookCallback(code, wparam, lparam);
	}

	return CallNextHookEx(hookMouse, code, wparam, lparam);
}

static LRESULT CALLBACK InternalKeyboardHookCallback(int code, WPARAM wparam, LPARAM lparam)
{
	if (code < 0)
	{
		return CallNextHookEx(hookKeyboard, code, wparam, lparam);
	}

	if (UserKeyboardHookCallback != NULL && !keyboardFilter.IsFiltered((int)wparam))
	{
		UserKeyboardHookCallback(code, wparam, lparam);
	}

	return CallNextHookEx(hookKeyboard, code, wparam, lparam);
}

static LRESULT CALLBACK InternalCBTHookCallback(int code, WPARAM wparam, LPARAM lparam)
{
	if(code < 0)
	{
		return CallNextHookEx(hookCBT,code,wparam,lparam);
	}

	if (UserCBTHookCallback!=NULL&&!cbtFilter.IsFiltered((int)wparam))
	{
		UserCBTHookCallback(code, wparam, lparam);
	}

	return CallNextHookEx(hookCBT,code,wparam,lparam);
}