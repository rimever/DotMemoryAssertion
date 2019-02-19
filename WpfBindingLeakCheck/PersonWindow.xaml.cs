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
using System.Windows.Shapes;

namespace WpfBindingLeakCheck
{
    /// <summary>
    /// PersonWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class PersonWindow : Window
    {
        /// <inheritdoc />
        public PersonWindow()
        {
            InitializeComponent();
        }

        private void ButtonCloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
