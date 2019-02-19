using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace WpfBindingLeakCheck
{
    public class BindablePerson : BindableBase
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { this.SetProperty(ref this.name, value); }
        }
    }
}
