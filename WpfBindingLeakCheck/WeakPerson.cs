using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Mvvm;

namespace WpfBindingLeakCheck
{
// ModelのPropertyChangedを弱いイベントパターンでリッスンする
    public class WeakPropertyChangedViewModelBase<TModel> : NotificationObject
        where TModel : INotifyPropertyChanged
    {
        protected TModel Model { get; private set; }

        // イベントのリスナ保持のためのフィールド
        private IWeakEventListener propertyChangedListener;

        public WeakPropertyChangedViewModelBase(TModel model)
        {
            this.Model = model;
            // ここが超ポイント！！
            // IWeakEventListenerを作成してEventManagerに渡す。
            // このときListenerのインスタンスは、フィールドなどで管理して
            // ViewModelがGCの対象になるまで破棄されないようにする
            this.propertyChangedListener = new PropertyChangedWeakEventListener(
                base.RaisePropertyChanged);
            PropertyChangedEventManager.AddListener(
                this.Model,
                this.propertyChangedListener,
                string.Empty);
        }

        // IWeakEventListenerの実装
        class PropertyChangedWeakEventListener : IWeakEventListener
        {
            private Action<string> raisePropertyChangedAction;

            public PropertyChangedWeakEventListener(Action<string> raisePropertyChangedAction)
            {
                this.raisePropertyChangedAction = raisePropertyChangedAction;
            }

            public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
            {
                // PropertyChangedEventManagerじゃないと処理しない
                if (typeof(PropertyChangedEventManager) != managerType)
                {
                    return false;
                }

                // PropertyChangedEventArgsじゃないと処理しない
                var evt = e as PropertyChangedEventArgs;
                if (evt == null)
                {
                    return false;
                }

                // コンストラクタで渡されたコールバックを呼び出す
                this.raisePropertyChangedAction(evt.PropertyName);
                return true;
            }
        }
    }

    public class WeakPersonViewModel : WeakPropertyChangedViewModelBase<Person>
    {

        public WeakPersonViewModel(Person model)
            : base(model)
        {
        }

        public string Name
        {
            get { return base.Model.Name; }

            set { base.Model.Name = value; }
        }
    }
}
