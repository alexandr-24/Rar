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
        /*
        TextBox Adres;
        TextBox Vmestitelnost;
        TextBox Zav;
        */
        int id;
        public StorageAddEditPage(int i)
        {
            InitializeComponent();
            id = i;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData(id);
        }
        public void LoadData(int id)
        {
            TextBox Adres = (TextBox)AdresTB.Template.FindName("InputTB", AdresTB);
            TextBox Vmestitelnost = (TextBox)VmestitelnostTB.Template.FindName("InputTB", VmestitelnostTB);
            TextBox Zav = (TextBox)ZavTB.Template.FindName("InputTB", ZavTB);

            if (id != -1)
            {
                using (RarEntities context = new RarEntities())
                {
                    currentStorage = context.Склад.Where(s => s.Номер_склада == id).FirstOrDefault<Склад>();

                    Adres.Text = currentStorage.Адрес_склада;
                    Vmestitelnost.Text = currentStorage.Вместительность.Remove(currentStorage.Вместительность.Length - 2, 2);
                    Zav.Text = currentStorage.Зав_складом;
                }

            }
            else
            {
                currentStorage = new Склад();
                using (RarEntities context = new RarEntities())
                {
                    currentStorage.Номер_склада = context.Склад.LastOrDefault().Номер_склада;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        
    }
}
