using Rar.Windows;
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
    /// Логика взаимодействия для TovarAddEditPage.xaml
    /// </summary>
    public partial class TovarAddEditPage : Page
    {
        Товар currentTovar;
        Frame currentFrame;
        int id;
        TextBox Name;
        TextBox Barcode;
        ComboBox Category;
        TextBox Price;
        TextBox Proizvoditel;
        TextBox Harakteristiki;


        public TovarAddEditPage(int i, Frame f)
        {
            InitializeComponent();

            id = i;
            currentFrame = f;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData(id);
        }

        private void LoadData(int id)
        {
            Name = (TextBox)NameTB.Template.FindName("InputTB", NameTB);
            Barcode = (TextBox)BarcodeTB.Template.FindName("InputTB", BarcodeTB);
            Category = CategoryCB;
            Price = (TextBox)PriceTB.Template.FindName("InputTB", PriceTB);
            Proizvoditel = (TextBox)ProizvoditelTB.Template.FindName("InputTB", ProizvoditelTB);
            Harakteristiki = (TextBox)HarakteristikiTB.Template.FindName("InputTB", HarakteristikiTB);

            using (RarEntities context = new RarEntities())
            {
                foreach (var i in context.Категория.ToList())
                {
                    CategoryCB.Items.Add(i.Название);
                }
            }

            if (id != -1)
            {
                using (RarEntities context = new RarEntities())
                {
                    currentTovar = context.Товар.Where(s => s.Код_товара == id).FirstOrDefault<Товар>();
                    Name.Text = currentTovar.Название;
                    Barcode.Text = currentTovar.Штрих_код;
                    Price.Text = Convert.ToString (currentTovar.Цена);
                    Proizvoditel.Text = currentTovar.Производитель;
                    //Harakteristiki.Text = currentTovar.Характеристики;

                    CategoryCB.SelectedIndex = 0;
                    Категория category = context.Категория.FirstOrDefault(k => k.Название == (string)CategoryCB.SelectedItem);
                    while (category.Код_категории != currentTovar.Код_категории)
                    {
                        CategoryCB.SelectedIndex += 1;
                        category = context.Категория.FirstOrDefault(k => k.Название == (string)CategoryCB.SelectedItem);
                    }
                }
            }
            else
            {
                currentTovar = new Товар();
                DeleteButton.Visibility = Visibility.Hidden;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (RarEntities context = new RarEntities())
            {
                string h = currentTovar.Характеристики;
                if (id != -1)
                    currentTovar = context.Товар.Where(s => s.Код_товара == id).FirstOrDefault<Товар>();
                currentTovar.Название = Name.Text;
                currentTovar.Штрих_код = Barcode.Text;
                currentTovar.Категория = context.Категория.FirstOrDefault(k => k.Название == (string)CategoryCB.SelectedItem);
                currentTovar.Цена = Convert.ToDecimal(Price.Text.Replace('.', ','));
                currentTovar.Производитель = Proizvoditel.Text;
                currentTovar.Характеристики = h;
                //currentTovar.Характеристики = Harakteristiki.Text;

                if (id == -1)
                {
                    context.Товар.Add(currentTovar);
                }

                context.SaveChanges();
            }

            currentFrame.Navigate(new TovarPage(currentFrame));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (RarEntities context = new RarEntities())
            {
                currentTovar = context.Товар.Where(s => s.Код_товара == id).FirstOrDefault<Товар>();
                context.Товар.Remove(currentTovar);
                context.SaveChanges();
            }
            currentFrame.Navigate(new TovarPage(currentFrame));
        }

        private void HarakteristikiTB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            EditHarakteristikiWindow w = new EditHarakteristikiWindow(currentTovar.Характеристики, (string)CategoryCB.SelectedItem);
            
            if(w.ShowDialog() == true)
            {
                currentTovar.Характеристики = w.GetHarakteristiki;
            }
        }
    }
}
