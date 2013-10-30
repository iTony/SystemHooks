using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Automation;

namespace Kennedy.ManagedHooks.SampleHookingApp
{
    class ElementClass
    {
        private AutomationElement elem;
        public AutomationElement Elem
        {
            get { return elem; }
            set { elem = value; }
        }

        private bool hasChild;
        public bool HasChild
        {
            get { return hasChild; }
            set { hasChild = value; }
        }
    }
}
