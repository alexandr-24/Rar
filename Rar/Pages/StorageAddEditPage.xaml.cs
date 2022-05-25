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
    /// Логика взаимодействия для StorageAddEditPage.xaml
    /// </summary>
    public partial class StorageAddEditPage : Page
    {
        Склад currentStorage;
        Frame currentFrame;
        int id;
        TextBox Adres;
        TextBox Vmestitelnost;
        TextBox Zav;
        public StorageAddEditPage(int i, Frame f)
        {
            InitializeComponent();
            id = i;
            currentFrame = f;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData(id);
        }
        public void LoadData(int id)
        {
            Adres = (TextBox)AdresTB.Template.FindName("InputTB", AdresTB);
            Vmestitelnost = (TextBox)VmestitelnostTB.Template.FindName("InputTB", VmestitelnostTB);
            Zav = (TextBox)ZavTB.Template.FindName("InputTB", ZavTB);

            if (id != -1)
            {
                using (RarEntities context = new RarEntities())
                {
                    currentStorage = context.Склад.Where(s => s.Номер_склада == id).FirstOrDefault<Склад>();

                    Adres.Text = currentStorage.Адрес_склада;
                    Vmestitelnost.Text = currentStorage.Вместительность.Remove(currentStorage.Вместительность.Length - 2, 2);
                    Zav.Text = currentStorage.Зав_складом;
                    Image.ImageSource = new BitmapImage(new Uri("pack://application:,,," + currentStorage.Фото, UriKind.Absolute));
                }
            }
            else
            {
                currentStorage = new Склад();
                /*
                using (RarEntities context = new RarEntities())
                {
                    currentStorage.Номер_склада = context.Склад.LastOrDefault().Номер_склада;
                }
                */
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (RarEntities context = new RarEntities())
            {
                if (id != -1)
                    currentStorage = context.Склад.Where(s => s.Номер_склада == id).FirstOrDefault<Склад>();
                currentStorage.Адрес_склада = Adres.Text;
                currentStorage.Вместительность = Vmestitelnost.Text + "м²";
                currentStorage.Зав_складом = Zav.Text;

                if (id == -1)
                {
                    context.Склад.Add(currentStorage);
                }


                context.SaveChanges();
            }

            currentFrame.Navigate(new StoragePage(currentFrame));
        }

        
    }
}
