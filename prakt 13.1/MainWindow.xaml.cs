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
using LibMas;
using Microsoft.Win32;
using System.IO;

namespace prakt_13._1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double[,] _matrixInitial;
        private double[,] _matrixResult;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginPage password = new LoginPage();
            password.Owner = this;
            password.ShowDialog();
            try
            {
                using (StreamReader open = new StreamReader("config.ini"))
                {
                    Data.RowCount = Convert.ToInt32(open.ReadLine());
                    Data.ColumnCount = Convert.ToInt32(open.ReadLine());
                }
                _matrixInitial = new double[Data.RowCount, Data.ColumnCount];
                initialTable.ItemsSource = VisualArray.ToDataTable(_matrixInitial).DefaultView;
                size.Text = string.Format("Размер таблицы: {0}х{1}", Data.RowCount, Data.ColumnCount);
                rowText.Text = Data.RowCount.ToString();
                columnText.Text = Data.ColumnCount.ToString();
            }
            catch
            {
                MessageBox.Show("Конфигурация таблицы не задана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            if (_matrixInitial == null)
            {
                MessageBox.Show("Сначала создайте таблицу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _matrixResult = new double[_matrixInitial.GetLength(0), _matrixInitial.GetLength(1)];
            for (int i = 0; i < _matrixInitial.GetLength(0); i++)
            {
                for (int j = 0; j < _matrixInitial.GetLength(1); j++)
                {
                    _matrixResult[i, j] = _matrixInitial[i, j];
                }
            }
            double[] array = new double[_matrixResult.GetLength(1)];
            int rowMin = 0;
            int rowMax = 0;
            double min = _matrixResult[0, 0];
            double max = _matrixResult[0, 0];
            for (int i = 0; i < _matrixResult.GetLength(0); i++)
            {
                for (int j = 0; j < _matrixResult.GetLength(1); j++)
                {
                    if (_matrixResult[i, j] < min)
                    {
                        min = _matrixResult[i, j];
                        rowMin = i;
                    }
                    if (_matrixResult[i, j] > max)
                    {
                        max = _matrixResult[i, j];
                        rowMax = i;
                    }
                }
            }
            for (int j = 0; j < _matrixResult.GetLength(1); j++)
            {
                array[j] = _matrixResult[rowMin, j];
                _matrixResult[rowMin, j] = _matrixResult[rowMax, j];
            }
            for (int j = 0; j < _matrixResult.GetLength(1); j++)
            {
                _matrixResult[rowMax, j] = array[j];
            }
            resultTable.ItemsSource = VisualArray.ToDataTable(_matrixResult).DefaultView;
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(rowText.Text, out int row) && int.TryParse(columnText.Text, out int column) && row > 0 && column > 0)
            {
                _matrixInitial = new double[row, column];
                MatrixOperation.FillRandomValues(_matrixInitial, 1, 100);
                initialTable.ItemsSource = VisualArray.ToDataTable(_matrixInitial).DefaultView;
                resultTable.ItemsSource = null;
                size.Text = string.Format("Размер таблицы: {0}х{1}", row, column);
            }
            else
            {
                MessageBox.Show("Введены неверные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            rowText.Clear();
            columnText.Clear();
            initialTable.ItemsSource = null;
            resultTable.ItemsSource = null;
            if (_matrixInitial != null)
            {
                MatrixOperation.ClearMatrix(_matrixInitial);
            }

            if (_matrixResult != null)
            {
                MatrixOperation.ClearMatrix(_matrixResult);
            }
            size.Text = string.Format("Размер таблицы: 0х0");
            selectedText.Text = string.Format("Выбранная ячейка: 0х0");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_matrixInitial == null)
            {
                MessageBox.Show("Сначала создайте таблицу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".txt";
            save.Filter = "Все файлы (*.*) | *.* | Текстовые файлы (*.txt*) | *.txt*";
            save.FilterIndex = 2;
            save.Title = "Сохранить Таблицы";
            if (save.ShowDialog() == true)
            {
                MatrixOperation.SaveMatrix(save.FileName, _matrixInitial);
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Все файлы (*.*)|*.*|Текстовые файлы|*.txt";
            open.FilterIndex = 2;
            open.Title = "Открытие таблицы";
            if (open.ShowDialog() == true)
            {
                if (open.FileName != string.Empty)
                {
                    MatrixOperation.OpenMatrix(open.FileName, out _matrixInitial);
                    rowText.Text = _matrixInitial.GetLength(0).ToString();
                    columnText.Text = _matrixInitial.GetLength(1).ToString();
                    initialTable.ItemsSource = VisualArray.ToDataTable(_matrixInitial).DefaultView;
                    size.Text = string.Format("Размер таблицы: {0}х{1}", _matrixInitial.GetLength(0), _matrixInitial.GetLength(1));
                    selectedText.Text = string.Format("Выбранная ячейка: 0х0");
                }
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Савельев Дмитрий Александрович В13\nПрактическая работа №13\nДана вещественная матрица А(M, N). Строку, содержащую максимальный элемент, поменять местами со строкой, содержащей минимальный элемент.", "Информация о программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Cick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите закрыть программу?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes) this.Close();
        }

        private void Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            resultTable.ItemsSource = null;
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //Опряделяем номер столбца
            int indexColumn = e.Column.DisplayIndex;
            //Определяем номер строки
            int indexRow = e.Row.GetIndex();
            //Проверяем правильное значение ввел пользователь
            if (!double.TryParse(((TextBox)e.EditingElement).Text.Replace('.', ','), out double value))
            {
                MessageBox.Show("Введены неверные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Заносим введенное значение в матрицу
            _matrixInitial[indexRow, indexColumn] = value;
            resultTable.ItemsSource = null;
        }
        private void InitialTable_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (initialTable.CurrentColumn == null) return;
            selectedText.Text = string.Format("Выбранная ячейка: {0}х{1}", initialTable.Items.IndexOf(initialTable.CurrentItem) + 1, initialTable.CurrentColumn.DisplayIndex + 1);
        }

        private void ResultTable_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (resultTable.CurrentColumn == null) return;
            selectedText.Text = string.Format("Выбранная ячейка: {0}х{1}", resultTable.Items.IndexOf(resultTable.CurrentItem) + 1, resultTable.CurrentColumn.DisplayIndex + 1);
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            if (_matrixInitial == null)
            {
                Data.ColumnCount = 0;
                Data.RowCount = 0;
            }
            else
            {
                Data.ColumnCount = Convert.ToInt32(columnText.Text);
                Data.RowCount = Convert.ToInt32(rowText.Text);
            }
            SettingTable setting = new SettingTable();
            setting.ShowDialog();
            _matrixInitial = new double[Data.RowCount, Data.ColumnCount];
            initialTable.ItemsSource = VisualArray.ToDataTable(_matrixInitial).DefaultView;
            size.Text = string.Format("Размер таблицы: {0}х{1}", Data.RowCount, Data.ColumnCount);
            rowText.Text = Data.RowCount.ToString();
            columnText.Text = Data.ColumnCount.ToString();
            using (StreamWriter save = new StreamWriter("config.ini"))
            {
                save.WriteLine(Data.RowCount);
                save.WriteLine(Data.ColumnCount);
            }
        }
    }
}
