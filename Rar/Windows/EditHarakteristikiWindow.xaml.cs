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

using Newtonsoft.Json;

namespace Rar.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditHarakteristikiWindow.xaml
    /// </summary>
    public partial class EditHarakteristikiWindow : Window
    {
        class Harakteritiki
        {
            public string Harakteristika { get; set; }
            public string Value { get; set; }

            public Harakteritiki(string h, string v)
            {
                Harakteristika = h;
                Value = v;
            }
        }

        List<Harakteritiki> VideocardL = new List<Harakteritiki> { new Harakteritiki("Объем памяти", ""), new Harakteritiki("Разъемы", ""), new Harakteritiki("Частота графического процессора", ""), new Harakteritiki("Рекомендуемая мощность блока питания", ""), new Harakteritiki("Максимальное разрешение", ""), new Harakteritiki("Количество поддерживаемых мониторов", "") };

        List<Harakteritiki> ProcessorL = new List<Harakteritiki> { new Harakteritiki("Количество ядер", ""), new Harakteritiki("Количество потоков", ""), new Harakteritiki("Тактовая частота", ""), new Harakteritiki("Видеопроцессор", "") };

        List<Harakteritiki> l;
        public EditHarakteristikiWindow(string h, string kategory)
        {
            InitializeComponent();
            if (h != null && h != "" && h != "[]")
            { 
                l = JsonConvert.DeserializeObject<List<Harakteritiki>>(h);
            }
            else
            {
                if (kategory != null)
                {
                    switch (kategory)
                    {
                        case "Видеокарты":
                            l = VideocardL;
                            break;
                        case "Процессоры":
                            l = ProcessorL;
                            break;

                        default:
                            l = new List<Harakteritiki>();
                            break;
                    }
                }
                else
                    l = new List<Harakteritiki>();
            }
            DG.ItemsSource = null;
            DG.ItemsSource = l;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            l = (List<Harakteritiki>)DG.ItemsSource;
            DialogResult = true;
            Close();
        }

        public string GetHarakteristiki
        {
            get { return JsonConvert.SerializeObject(l); }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            l = (List<Harakteritiki>)DG.ItemsSource;
            l.Add(new Harakteritiki("", ""));
            DG.ItemsSource = null;
            DG.ItemsSource = l;
        }

        private void headerThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Left = Left + e.HorizontalChange;
            Top = Top + e.VerticalChange;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
