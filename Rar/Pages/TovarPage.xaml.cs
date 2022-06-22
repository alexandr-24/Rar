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
        Frame CurrentFrame;
        public TovarPage(Frame frame)
        {
            InitializeComponent();
            CurrentFrame = frame;
            using (RarEntities context = new RarEntities())
            {              
                CategoryCB.Items.Add("Все");
                foreach(var i in context.Категория.ToList())
                {
                    CategoryCB.Items.Add(i.Название);
                }
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CategoryCB.SelectedIndex = 0;
        }

        private void UpdateListViewTovar()
        {
            TextBox x = (TextBox)SearchTB.Template.FindName("SearchTB", SearchTB);

            using (RarEntities context = new RarEntities())
            {
                Категория category = context.Категория.FirstOrDefault(k => k.Название == (string)CategoryCB.SelectedItem);

                if (category == null)
                {
                    if (x.Text == "")
                    {
                        ListViewTovar.ItemsSource = context.Database.SqlQuery<Товар>("SELECT * FROM Товар").ToList();
                    }
                    else
                    {
                        ListViewTovar.ItemsSource = context.Database.SqlQuery<Товар>("SELECT * FROM Товар WHERE Название LIKE '%" + x.Text + "%'").ToList();
                    }
                }
                else
                {
                    if (x.Text == "")
                    {
                        ListViewTovar.ItemsSource = context.Database.SqlQuery<Товар>("SELECT * FROM Товар WHERE Код_категории = " + category.Код_категории).ToList();
                    }
                    else
                    {
                        ListViewTovar.ItemsSource = context.Database.SqlQuery<Товар>("SELECT * FROM Товар WHERE Код_категории = " + category.Код_категории + " AND Название LIKE '%" + x.Text + "%'").ToList();
                    }
                }
            }
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateListViewTovar();
        }

        private void CategoryCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateListViewTovar();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentFrame.Navigate(new TovarAddEditPage(-1, CurrentFrame));
        }

        private void testbutton_Click(object sender, RoutedEventArgs e)
        {
            CurrentFrame.Navigate(new TovarAddEditPage(Convert.ToInt32(((Button)e.OriginalSource).Tag.ToString()), CurrentFrame));
        }
    }
}
