using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using System.ComponentModel;

namespace WpfBindingLeakCheck
{

    // INotifyPropertyChangedの実装クラス
    public class NotificationObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var h = this.PropertyChanged;
            if (h != null)
            {
                h(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    // Modelクラス
    public class Person : NotificationObject
    {
        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                base.RaisePropertyChanged("Name");
            }
        }
    }

    // PersonのViewModel
    public class PersonViewModel : NotificationObject
    {
        private Person model;
        public PersonViewModel(Person model)
        {
            this.model = model;
            this.model.PropertyChanged += ModelPropertyChanged;
        }

        public string Name
        {
            get
            {
                return this.model.Name;
            }

            set
            {
                this.model.Name = value;
            }
        }

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // ModelとViewModelのプロパティ名一緒だからそのまま使おう
            base.RaisePropertyChanged(e.PropertyName);
        }
    }
}