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

namespace Rar.Pages
{
    /// <summary>
    /// Логика взаимодействия для StoragePage.xaml
    /// </summary>
    /// 

    
    public partial class StoragePage : Page
    {
        Frame CurrentFrame;
        public StoragePage(Frame frame)
        {
            InitializeComponent();
            CurrentFrame = frame;
            using (RarEntities context = new RarEntities())
            {
                ListViewStorage.ItemsSource = context.Склад.ToList();
            }
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox x = (TextBox)SearchTB.Template.FindName("SearchTB", SearchTB);
            if (x.Text == "")
            {
                using (RarEntities context = new RarEntities())
                {
                    ListViewStorage.ItemsSource = context.Склад.ToList();
                }
            }
            else
            {
                using (RarEntities context = new RarEntities())
                {
                
                    ListViewStorage.ItemsSource = context.Database.SqlQuery<Склад>("SELECT * FROM Склад WHERE Адрес_склада LIKE '%" + x.Text + "%'").ToList();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CurrentFrame.Navigate(new StorageAddEditPage(Convert.ToInt32(((Button)e.OriginalSource).Tag.ToString())));
        }
    }
}
