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

namespace prakt_13._1
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
            pass.Focus();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (pass.Password == "123") Close();
            else
            {
                MessageBox.Show("Введён неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                pass.Focus();
            }
        }

        private void Esc_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Close();
        }
    }
}
