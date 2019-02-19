using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JetBrains.dotMemoryUnit;

namespace WpfBindingLeakCheck
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Person> _people;

        /// <inheritdoc />
        public MainWindow()
        {
            InitializeComponent();
            var nameArray = new[] {"Tom", "Jim"};
            _people = new List<Person>(nameArray.Select(i => new Person() {Name = i.ToString()}));            
        }

        private void ButtonBaseOnClick(object sender, RoutedEventArgs e)
        {
            var window = new PersonWindow {DataContext = new PersonViewModel(_people.FirstOrDefault())};
            window.ShowDialog();
        }
    }
}
