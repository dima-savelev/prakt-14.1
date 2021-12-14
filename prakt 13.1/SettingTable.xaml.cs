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
    /// Логика взаимодействия для SettingTable.xaml
    /// </summary>
    public partial class SettingTable : Window
    {
        public SettingTable()
        {
            InitializeComponent();
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            rowText.Text = Data.RowCount.ToString();
            columnText.Text = Data.ColumnCount.ToString();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(rowText.Text, out int row) && int.TryParse(columnText.Text, out int column) && row > 0 && column > 0)
            {
                Data.ColumnCount = column;
                Data.RowCount = row;
            }
            else
            {
                MessageBox.Show("Введены неверные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
        }
    }
}
