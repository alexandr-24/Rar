using Rar.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rar
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Пробное подключение к БД чтобы открытие других страниц не лагало
            using (RarEntities context = new RarEntities())
            {
                context.Товар.ToList();
            }
        }

        private void headerThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Left = Left + e.HorizontalChange;
            Top = Top + e.VerticalChange;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CollapseButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(SelectedButtonFrame, 1);
            MainFrame.Navigate(null);
        }
        private void GoodsButton_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(SelectedButtonFrame, 2);
            MainFrame.Navigate(new TovarPage(MainFrame));
        }
        private void StorageButton_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(SelectedButtonFrame, 3);
            MainFrame.Navigate(new StoragePage(MainFrame));
        }

        private void ClientButton_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(SelectedButtonFrame, 4);
            MainFrame.Navigate(null);
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(SelectedButtonFrame, 5);
            MainFrame.Navigate(null);
        }
    }
}
