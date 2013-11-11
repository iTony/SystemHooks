// /////////////////////////////////////////////////////////////
// File: MainForm.cs		Class: Kennedy.SampleHookingApp.MainForm
// Date: 2/25/2004			Author: Michael Kennedy
// Language: C#				Framework: .NET
//
// Copyright: Copyright (c) Michael Kennedy, 2004-2005
// /////////////////////////////////////////////////////////////
// License: See License.txt file included with application.
// Description: See compiled documentation (Managed Hooks.chm)
// /////////////////////////////////////////////////////////////

#region using ...
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Windows.Automation;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
#endregion

namespace Kennedy.ManagedHooks.SampleHookingApp
{
	public class MainForm : System.Windows.Forms.Form
	{
		#region Member Variables

		private System.Windows.Forms.Button buttonInstall;
		private System.Windows.Forms.Button buttonUninstall;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.TextBox textBoxMessages;
		private System.Windows.Forms.Button buttonAbout;
		
		#endregion

        string srcPath = "";
        static Process process = null;
        static int count = 0;
        //static AutomationElement mainElement = null;
        static List<AutomationElement> elementList = null;
        static List<AutomationElement> childElemList = null;
        private AutomationElementCollection elements = null;
		// EXAMPLE CODE SECTION
		private Kennedy.ManagedHooks.MouseHook mouseHook = null;
        private Button button1;
        private TextBox textBox1;
		private Kennedy.ManagedHooks.KeyboardHook keyboardHook = null;
        private Kennedy.ManagedHooks.CBTHook cbtHook = null;
        public delegate bool CallBack(IntPtr hwnd, int lParam);

        [DllImport("user32.dll")]
        public static extern int EnumWindows(CallBack lpfn, int lParam);
        public static CallBack callBackEnumWindows = new CallBack(WindowProcess);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);
		
        public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            
			//
			// Add any constructor code after InitializeComponent call
			//

			// EXAMPLE CODE SECTION
            elementList = new List<AutomationElement>();
            childElemList = new List<AutomationElement>();

           

			mouseHook = new Kennedy.ManagedHooks.MouseHook();
			mouseHook.MouseEvent += new Kennedy.ManagedHooks.MouseHook.MouseEventHandler(mouseHook_MouseEvent);
		
			keyboardHook = new Kennedy.ManagedHooks.KeyboardHook();
			keyboardHook.KeyboardEvent += new Kennedy.ManagedHooks.KeyboardHook.KeyboardEventHandler(keyboardHook_KeyboardEvent);

            cbtHook = new Kennedy.ManagedHooks.CBTHook();
            cbtHook.CBTEvent += new Kennedy.ManagedHooks.CBTHook.CBTEventHandler(cbtHook_windowsEvent);
        }

        public static bool WindowProcess(IntPtr hwnd, int lParam)
        {
            //EnumChildWindows(hwnd, callBackEnumChildWindows, 0);
            StringBuilder title = new StringBuilder(200);
            int len;
            len = GetWindowText(hwnd, title, 200);
            count++;
            uint processId = 0;
            string name = process.MainWindowTitle;
            GetWindowThreadProcessId(hwnd, ref processId);
            if (processId==process.Id&&title.Length>0)
            {
                AutomationElement mainElement = AutomationElement.FromHandle(hwnd);
                if (mainElement!=null)
                {
                    elementList.Add(mainElement);
                }
                
                //elementList.Add(mainElement);
            }
            if (hwnd == process.MainWindowHandle||hwnd == process.Handle)
            {
                //mainElement = AutomationElement.FromHandle(hwnd);
                
            }
            return true;
        }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// EXAMPLE CODE SECTION
				if (mouseHook != null)
				{
					mouseHook.Dispose();
					mouseHook = null;
				}
				if (keyboardHook != null)
				{
					keyboardHook.Dispose();
					keyboardHook = null;
				}
			
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.buttonInstall = new System.Windows.Forms.Button();
            this.buttonUninstall = new System.Windows.Forms.Button();
            this.textBoxMessages = new System.Windows.Forms.TextBox();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonInstall
            // 
            this.buttonInstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInstall.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonInstall.Location = new System.Drawing.Point(13, 96);
            this.buttonInstall.Name = "buttonInstall";
            this.buttonInstall.Size = new System.Drawing.Size(127, 32);
            this.buttonInstall.TabIndex = 0;
            this.buttonInstall.Text = "Start";
            this.buttonInstall.Click += new System.EventHandler(this.buttonInstall_Click);
            // 
            // buttonUninstall
            // 
            this.buttonUninstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUninstall.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonUninstall.Location = new System.Drawing.Point(183, 96);
            this.buttonUninstall.Name = "buttonUninstall";
            this.buttonUninstall.Size = new System.Drawing.Size(116, 32);
            this.buttonUninstall.TabIndex = 1;
            this.buttonUninstall.Text = "Stop";
            this.buttonUninstall.Click += new System.EventHandler(this.buttonUninstall_Click);
            // 
            // textBoxMessages
            // 
            this.textBoxMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessages.BackColor = System.Drawing.Color.White;
            this.textBoxMessages.Location = new System.Drawing.Point(0, 154);
            this.textBoxMessages.Multiline = true;
            this.textBoxMessages.Name = "textBoxMessages";
            this.textBoxMessages.ReadOnly = true;
            this.textBoxMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxMessages.Size = new System.Drawing.Size(555, 369);
            this.textBoxMessages.TabIndex = 2;
            this.textBoxMessages.TabStop = false;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 523);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(603, 22);
            this.statusBar1.TabIndex = 3;
            // 
            // buttonAbout
            // 
            this.buttonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbout.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonAbout.Location = new System.Drawing.Point(512, 96);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(56, 36);
            this.buttonAbout.TabIndex = 4;
            this.buttonAbout.Text = "&About";
            this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(526, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(51, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Open";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 42);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(482, 20);
            this.textBox1.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(603, 545);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonAbout);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.textBoxMessages);
            this.Controls.Add(this.buttonUninstall);
            this.Controls.Add(this.buttonInstall);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Hook Test Application";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Main Method
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			//
			// Give our app an XP theme look.
			//
			Application.EnableVisualStyles();
			Application.DoEvents();

			Application.Run(new MainForm());
		}

		#endregion

        private void WalkEnabledElements(AutomationElement rootElement)
        {
            Condition condition1 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            Condition condition2 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            TreeWalker walker = new TreeWalker(new AndCondition(condition1, condition2));
            try
            {
                AutomationElement elementNode = walker.GetFirstChild(rootElement);

                //bool flag = true;
                while (elementNode != null)
                {
                    object controlTypeNoDefault = elementNode.GetCurrentPropertyValue(AutomationElement.ControlTypeProperty, true);
                    //string name = "";
                    if (controlTypeNoDefault != AutomationElement.NotSupported)
                    {
                        ControlType controlType = controlTypeNoDefault as ControlType;
                        #region ///////////////////////////////////////////
                        //if (true || controlType == ControlType.Button)
                        //{
                        //    if (elementNode != null && flag)
                        //    {
                        //        /////UIAeventHandler = new AutomationEventHandler(OnUIAutomationEvent);
                        //        //this.textBox1.Text = elementNode.GetCurrentPropertyValue(AutomationElement.NameProperty) as string;
                        //        //Automation.AddAutomationEventHandler(InvokePattern.InvokedEvent, elementNode, TreeScope.Element,
                        //        //   UIAeventHandler = new AutomationEventHandler(OnUIAutomationEvent));
                        //        //Automation.AddAutomationPropertyChangedEventHandler(elementNode,
                        //        //    TreeScope.Element,
                        //        //    propChangeHandler = new AutomationPropertyChangedEventHandler(OnPropertyChange),
                        //        //    AutomationElement.IsEnabledProperty);
                        //        //AutomationEventHandler selectionInvalidatedHandler =
                        //        //    new AutomationEventHandler(SelectionInvalidatedHandler);
                        //        //Automation.AddAutomationEventHandler(
                        //        //SelectionPattern.InvalidatedEvent,
                        //        //elementNode,
                        //        //TreeScope.Element,
                        //        //selectionInvalidatedHandler);
                        //        //ElementSubscribeButton = elementNode;
                        //        // flag = false;
                        //    }
                        //}
                        //name += controlType.LocalizedControlType + "   ";
                        // name += elementNode.GetCurrentPropertyValue(AutomationElement.NameProperty) as string;
                        #endregion
                        childElemList.Add(elementNode);

                    }

                    //TreeNode childTreeNode = treeNode.Nodes.Add(name);
                    WalkEnabledElements(elementNode);
                    elementNode = walker.GetNextSibling(elementNode);
                }
            }
            catch (System.Exception ex)
            {
                GC.Collect();
                Thread.Sleep(3000);
            }
            
        }
        private AutomationElement GetElementsByPoint(Point point)
        {
            AutomationElement targetElem = null;
            //Condition condition1 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            //Condition condition2 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            //TreeWalker walker = new TreeWalker(new AndCondition(condition1, condition2));
            //for (int i = 0; i < childElemList.Count;i++ )
            //{
            //    AutomationElement element = childElemList[i];
            //    System.Windows.Rect boundingRect = (System.Windows.Rect)element.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
            //    if (point.X > boundingRect.Left && point.X < boundingRect.Right && point.Y < boundingRect.Bottom && point.Y > boundingRect.Top)
            //    {

            //        targetElem = element;
                    
            //        //return element;
            //    }
            //}
            for (int i = 0; i < childElemList.Count;i++ )
            {
                try
                {
                    AutomationElement element = childElemList[i];
                    System.Windows.Rect boundingRect = (System.Windows.Rect)element.GetCachedPropertyValue(AutomationElement.BoundingRectangleProperty);
                    if (point.X > boundingRect.Left && point.X < boundingRect.Right && point.Y < boundingRect.Bottom && point.Y > boundingRect.Top)
                    {
                        targetElem = element;
                        //return element;
                    }
                }
                catch (System.Exception ex)
                {
                    string exception = ex.Message;
                }
                
            }
            return targetElem;
        }
        private void GetElementsByPoint(AutomationElement rootElement,Point point,ref AutomationElement targetElem)
        {
            Condition condition1 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            Condition condition2 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            TreeWalker walker = new TreeWalker(new AndCondition(condition1, condition2));
            AutomationElement elementNode = walker.GetFirstChild(rootElement);

            //bool flag = true;
            while (elementNode != null)
            {
                object controlTypeNoDefault = elementNode.GetCurrentPropertyValue(AutomationElement.ControlTypeProperty, true);
                //string name = "";
                if (controlTypeNoDefault != AutomationElement.NotSupported)
                {
                    ControlType controlType = controlTypeNoDefault as ControlType;
                    System.Windows.Rect boundingRect = (System.Windows.Rect)elementNode.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
                    if (point.X>boundingRect.Left&&point.X<boundingRect.Right&&point.Y<boundingRect.Bottom&&point.Y>boundingRect.Top&&walker.GetFirstChild(elementNode)==null)
                    {
                        targetElem = elementNode;
                        return;
                    }
                }

                //TreeNode childTreeNode = treeNode.Nodes.Add(name);
                WalkEnabledElements(elementNode);
                elementNode = walker.GetNextSibling(elementNode);
            }

            return;
        }

		private void buttonInstall_Click(object sender, System.EventArgs e)
		{
			// EXAMPLE CODE SECTION

            process = Process.Start(srcPath);
            AutomationElement aeDeskTop = AutomationElement.RootElement;
            Thread.Sleep(500);
            //AutomationElement aeForm = null;
            int times = 0;
            process.WaitForInputIdle();
            while (process.MainWindowHandle == null || process.MainWindowHandle == IntPtr.Zero)
            {
                Thread.Sleep(1000);
                if (times>5&&process.Handle!=IntPtr.Zero)
                {
                    break;
                }
                times++;
                
            }
            if (process.MainWindowHandle==IntPtr.Zero)
            {
                EnumWindows(callBackEnumWindows, 0);
            }
            else
            {
                elementList.Add(AutomationElement.FromHandle(process.MainWindowHandle));
            }

            Condition condition1 = new PropertyCondition(AutomationElement.IsControlElementProperty, true);
            Condition condition2 = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            Condition condition = new PropertyCondition(AutomationElement.ProcessIdProperty,process.Id);
            //AutomationElement aeForm = AutomationElement.FromHandle(process.MainWindowHandle);
            CacheRequest fetchRequest = new CacheRequest();
            fetchRequest.Add(AutomationElement.NameProperty);
            fetchRequest.Add(AutomationElement.AutomationIdProperty);
            fetchRequest.Add(AutomationElement.ControlTypeProperty);
            fetchRequest.Add(AutomationElement.BoundingRectangleProperty);

            using (fetchRequest.Activate())
            {
                foreach (AutomationElement aeForm in elementList)
                {
                    AutomationElementCollection tElemList = aeForm.FindAll(TreeScope.Subtree, new AndCondition(condition, condition1, condition2));
                    //elements = aeForm.FindAll(TreeScope.Subtree, new AndCondition(condition1, condition2, condition));
                    ///elements.CopyTo(temp, temp.Count);
                    for (int i = 0; i < tElemList.Count;i++ )
                    {
                        childElemList.Add(tElemList[i]);
                    }
                }
                //WalkEnabledElements(aeDeskTop);
               
            }
            //fetchRequest.Add(AutomationElement.IsContentElementProperty);
            //foreach (AutomationElement element in elementList)
            //{
            //    WalkEnabledElements(element);
            //}

            //elements = aeDeskTop.FindAll(TreeScope.Subtree, new AndCondition(condition1, condition2, condition));

			AddText("Adding mouse hook.");
			mouseHook.InstallHook(this.Handle);

			AddText("Adding keyboard hook.");
			keyboardHook.InstallHook(this.Handle);

            AddText("Adding cbt hook");
            cbtHook.InstallHook(this.Handle);

			buttonInstall.Enabled = false;
			buttonUninstall.Enabled = true;
            
		}

		private void buttonUninstall_Click(object sender, System.EventArgs e)
		{
			// EXAMPLE CODE SECTION

			mouseHook.UninstallHook();
			AddText("Mouse hook removed.");

			keyboardHook.UninstallHook();
			AddText("Keyboard hook removed.");

            cbtHook.UninstallHook();
            AddText("CBT hook removed.");

			buttonInstall.Enabled = true;
			buttonUninstall.Enabled = false;
		}

		// EXAMPLE CODE SECTION
		private void mouseHook_MouseEvent(Kennedy.ManagedHooks.MouseEvents mEvent, Point point,IntPtr hwnd)
		{
            System.Windows.Point wpt = new System.Windows.Point(point.X, point.Y);
            
            try
            {
                AutomationElement autoElem = AutomationElement.FromPoint(wpt);
                //autoElem.SetFocus();
                //AutomationElement autoElem = AutomationElement.FocusedElement;
                if (autoElem == null)
                {
                    AddText("Automation null");
                }
                string name = "[" + autoElem.Current.Name;
                object controlTypeNoDefault = autoElem.GetCurrentPropertyValue(AutomationElement.ControlTypeProperty, true);
                if (controlTypeNoDefault != AutomationElement.NotSupported)
                {
                    ControlType controlType = controlTypeNoDefault as ControlType;
                    name += "] [" + controlType.LocalizedControlType + "]";
                }
                //string name = "null";
                string msg = string.Format("Mouse event: {0}-->{1}: ({2},{3}).,{4}", mEvent.ToString(), name, point.X, point.Y, hwnd);
                AddText(msg);
            }
            catch (System.Exception ex)
            {
                AutomationElement targetElem = null;
                
                targetElem = GetElementsByPoint(point);
                if (targetElem==null)
                {
                    AddText("get element error");
                }
                string name = "["+ targetElem.Cached.Name;
                object controlTypeNoDefault = targetElem.GetCachedPropertyValue(AutomationElement.ControlTypeProperty, true);
                if (controlTypeNoDefault != AutomationElement.NotSupported)
                {
                    ControlType controlType = controlTypeNoDefault as ControlType;
                    name += "] [" + controlType.LocalizedControlType + "]";
                }
                string msg = string.Format("Mouse event: {0}-->{1}: ({2},{3}).,{4}", mEvent.ToString(), name, point.X, point.Y, hwnd);
                AddText(msg);
            }
           
		}

		// EXAMPLE CODE SECTION
		private void keyboardHook_KeyboardEvent(Kennedy.ManagedHooks.KeyboardEvents kEvent, Keys key)
		{
			string msg = string.Format("Keyboard event: {0}: {1}.", kEvent.ToString(), key);
			AddText(msg);
		}

        private void cbtHook_windowsEvent(Kennedy.ManagedHooks.CBTEvents cEvent,IntPtr hwnd)
        {
            string msg = string.Format("{0}-->{1}", cEvent.ToString(), hwnd);
            AddText(cEvent.ToString());
        }

		#region Utility Methods

		private void AddText(string message)
		{
			if (message == null)
			{
				return;
			}

			int length = textBoxMessages.Text.Length + message.Length;
			if (length >= textBoxMessages.MaxLength)
			{
				textBoxMessages.Text = "";
			}

			if (!message.EndsWith("\r\n"))
			{
				message += "\r\n";
			}

			textBoxMessages.Text = message + textBoxMessages.Text;
		}

		private void buttonAbout_Click(object sender, System.EventArgs e)
		{
			new AboutForm().ShowDialog();
		}

		#endregion

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "可执行文件|*.exe*|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = openFileDialog.FileName;
                srcPath = openFileDialog.FileName;
            }
        }

	}
}
