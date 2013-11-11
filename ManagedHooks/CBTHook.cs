using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Kennedy.ManagedHooks
{
    public enum CBTEvents
    {

        HCBT_ACTIVATE = 5,

        HCBT_CLICKSKIPPED = 6,

        HCBT_CREATEWND = 3,

        HCBT_DESTROYWND = 4,

        HCBT_KEYSKIPPED = 7,

        HCBT_MINMAX = 1,

        HCBT_MOVESIZE = 0,

        HCBT_QS = 2,

        HCBT_SETFOCUS = 9,

        HCBT_SYSCOMMAND = 8
    }
    public class CBTHook : SystemHook
    {
        

        public delegate void CBTEventHandler(CBTEvents events,IntPtr hwnd);

		/// <include file='ManagedHooks.xml' path='Docs/MouseHook/MouseEvent/*'/>
		public event CBTEventHandler CBTEvent;

		/// <include file='ManagedHooks.xml' path='Docs/MouseHook/ctor/*'/>
		public CBTHook() : base(HookTypes.CBT)
		{
		}

		/// <include file='ManagedHooks.xml' path='Docs/MouseHook/HookCallback/*'/>
		protected override void HookCallback(int code, UIntPtr wparam, IntPtr lparam)
		{
			if (CBTEvent == null)
			{
				return;
			}

			//int x = 0, y = 0;
            
            //IntPtr hwnd = IntPtr.Zero;
			//CBTEvents mEvent = (CBTEvents)wparam.ToUInt32();

			if (code == (int)CBTEvents.HCBT_ACTIVATE)
			{
                CBTEvent(CBTEvents.HCBT_ACTIVATE, IntPtr.Zero);
			}//else if (code == (int)CBTEvents.HCBT_CLICKSKIPPED)
			//{
                //CBTEvent(CBTEvents.HCBT_CLICKSKIPPED, IntPtr.Zero);
			//}
            else if (code == (int)CBTEvents.HCBT_CREATEWND)
			{
                CBTEvent(CBTEvents.HCBT_CREATEWND, IntPtr.Zero);
			}else if (code == (int)CBTEvents.HCBT_DESTROYWND)
			{
                CBTEvent(CBTEvents.HCBT_DESTROYWND, IntPtr.Zero);
			}//else if (code == (int)CBTEvents.HCBT_KEYSKIPPED)
			//{
                //CBTEvent(CBTEvents.HCBT_KEYSKIPPED, IntPtr.Zero);
			//}
            else if (code == (int)CBTEvents.HCBT_MINMAX)
			{
                IntPtr hwnd = (IntPtr)wparam.ToUInt32();
                CBTEvent(CBTEvents.HCBT_MINMAX,hwnd);
                
			}else if (code == (int)CBTEvents.HCBT_MOVESIZE)
			{
                IntPtr hwnd = (IntPtr)wparam.ToUInt32();
                CBTEvent(CBTEvents.HCBT_MOVESIZE,hwnd);
			}else if (code == (int)CBTEvents.HCBT_QS)
			{
                CBTEvent(CBTEvents.HCBT_QS, IntPtr.Zero);
			}else if (code == (int)CBTEvents.HCBT_SETFOCUS)
			{
                CBTEvent(CBTEvents.HCBT_SETFOCUS, IntPtr.Zero);
			}else if (code == (int)CBTEvents.HCBT_SYSCOMMAND)
			{
                CBTEvent(CBTEvents.HCBT_SYSCOMMAND, IntPtr.Zero);
			}
			//MouseEvent(mEvent, new Point(x, y));
		}

		/// <include file='ManagedHooks.xml' path='Docs/MouseHook/FilterMessage/*'/>
        public void FilterMessage(CBTEvents eventType)
		{
			base.FilterMessage(this.HookType, (int)eventType);
		}

	
    }
}
