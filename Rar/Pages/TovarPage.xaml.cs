using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для TovarPage.xaml
    /// </summary>
    public partial class TovarPage : Page
    {
        public TovarPage()
        {
            InitializeComponent();
            using (RarEntities context = new RarEntities())
            {
                ListViewStorage.ItemsSource = context.Товар.ToList();
            }
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox x = (TextBox)SearchTB.Template.FindName("SearchTB", SearchTB);
            if (x.Text == "")
            {
                using (RarEntities context = new RarEntities())
                {
                    ListViewStorage.ItemsSource = context.Товар.ToList();
                }
            }
            else
            {
                using (RarEntities context = new RarEntities())
                {
                    ListViewStorage.ItemsSource = context.Database.SqlQuery<Товар>("SELECT * FROM Товар WHERE Название LIKE '%" + x.Text + "%'").ToList();
                }
            }
        }
    }
}
