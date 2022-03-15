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

namespace V1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            MainStuff w_MainStuff = new MainStuff();
            w_MainStuff.Show();
            this.Hide();
        }

        private void tb_Username_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void pb_Password_TextInput(object sender, TextCompositionEventArgs e)
        {
        }
    }
}
