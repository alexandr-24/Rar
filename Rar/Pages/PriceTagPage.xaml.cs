using System;
using System.Collections.Generic;
using System.IO;
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

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;

namespace Rar.Pages
{
    /// <summary>
    /// Логика взаимодействия для PriceTagPage.xaml
    /// </summary>
    public partial class PriceTagPage : Page
    {
        public class ProductsToPrint
        {
            public bool isSelected { get; set; } = false;
            public string Count { get; set; } = "1";
            public int ID { get; set; }
            public string ProductName { get; set; }
            public string ProductCost { get; set; }
            public string ProductType { get; set; }
            public string ProductBarcode { get; set; }
        }
        public static List<ProductsToPrint> products = new List<ProductsToPrint>();

        public PriceTagPage()
        {
            InitializeComponent();
            LoadProductsList();
            updateDG();
        }

        private void LoadProductsList()
        {
            products = new List<ProductsToPrint>();
            using (RarEntities context = new RarEntities())
            {
                List<Товар> ProductsList = context.Товар.ToList();
                for (int i = 0; i < ProductsList.Count; i++)
                {
                    products.Add(new ProductsToPrint { isSelected = false, Count = "1", ID = ProductsList[i].Код_товара, ProductName = ProductsList[i].Название, ProductCost = ProductsList[i].Цена.ToString(), ProductType = "Цена за шт.", ProductBarcode = ProductsList[i].Штрих_код });
                }
            }
        }

        private void updateDG()
        {
            DG.ItemsSource = products.ToList();
        }

        private void checkAll_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < products.Count; i++)
            {
                products[i].isSelected = true;
            }
            updateDG();
        }

        private void checkAll_Unchecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < products.Count; i++)
            {
                products[i].isSelected = false;
            }
            updateDG();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            products.Clear();
            foreach (ProductsToPrint dr in DG.ItemsSource)
            {
                if (dr.isSelected == true)
                {
                    for (int i = 0; i < Convert.ToInt32(dr.Count); i++)
                    {
                        products.Add(dr);
                    }
                }
            }
            if (products.Count > 0)
            {
                Print(products);
            }
            else
            {
                MessageBox.Show("Товары не выбраны!");
            }
            LoadProductsList();
            updateDG();
        }

        private void Print(List<ProductsToPrint> productsToPrintsList)
        {
            try
            {
                string stream = Environment.CurrentDirectory + "/Tags";
                stream += "/" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".pdf";
                stream.Replace("/", @"\");

                int count = productsToPrintsList.Count;

                // 595 X 842 - A4
                // 208(4) X 166(5)
                FileStream fileStream = File.Create(stream);
                using (Document document = new Document(new iTextSharp.text.Rectangle(842, 595)))
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
                    document.Open();
                    PdfContentByte cb = writer.DirectContent;

                    string ttf = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
                    BaseFont bf = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    cb.SetColorFill(BaseColor.BLACK);
                    cb.SetFontAndSize(bf, 8);

                    // Price tag
                    int r = 0;
                    while (r < count)
                    {
                        if (r != 0) document.NewPage();
                        cb.SetColorFill(BaseColor.BLACK);
                        cb.SetFontAndSize(bf, 8);
                        for (int i = 0; i <= 3; i++)
                        {
                            for (int k = 0; k <= 2; k++)
                            {
                                if (r >= count) break;

                                cb.Rectangle(5 + 208 * i, document.PageSize.Height - 5 - 166 * k, 208, -166);
                                cb.Stroke();

                                BarcodeEAN codeEAN = new BarcodeEAN();
                                if (productsToPrintsList[r].ProductBarcode == null)
                                {
                                    throw new Exception("Не у всех товаров указан штрих-код!");
                                }
                                if (Convert.ToString(productsToPrintsList[r].ProductBarcode).Length == 13)
                                    codeEAN.CodeType = Barcode.EAN13;
                                else codeEAN.CodeType = Barcode.EAN8;
                                codeEAN.Code = Convert.ToString(productsToPrintsList[r].ProductBarcode);

                                iTextSharp.text.Image barcode = codeEAN.CreateImageWithBarcode(cb, null, null);
                                if (Convert.ToString(productsToPrintsList[r].ProductBarcode).Length == 13)
                                    barcode.SetAbsolutePosition(208 * i + 65, document.PageSize.Height - 155 - 166 * k);
                                else barcode.SetAbsolutePosition(208 * i + 85, document.PageSize.Height - 155 - 166 * k);
                                document.Add(barcode);

                                cb.SetFontAndSize(bf, 10);
                                string line1 = "";
                                string line2 = "";
                                if (productsToPrintsList[r].ProductName.Length > 30)
                                {
                                    int lastEnter = 0;
                                    for (int j = 0; j < 35; j++)
                                    {
                                        if (productsToPrintsList[r].ProductName[j] == ' ')
                                        {
                                            lastEnter = j;
                                        }
                                    }
                                    for (int j = 0; j < lastEnter; j++)
                                    {
                                        line1 += productsToPrintsList[r].ProductName[j];
                                    }
                                    while (lastEnter < productsToPrintsList[r].ProductName.Length)
                                    {
                                        line2 += productsToPrintsList[r].ProductName[lastEnter];
                                        lastEnter++;
                                    }
                                }
                                else
                                {
                                    line1 = productsToPrintsList[r].ProductName;
                                }

                                cb.ShowTextAligned(1, line1, 208 * i + 110, document.PageSize.Height - 35 - 166 * k, 0);
                                cb.ShowTextAligned(1, line2, 208 * i + 110, document.PageSize.Height - 50 - 166 * k, 0);
                                cb.SetFontAndSize(bf, 24);
                                cb.ShowTextAligned(1, productsToPrintsList[r].ProductCost + " руб.", 208 * i + 109, document.PageSize.Height - 83 - 166 * k, 0);
                                cb.SetFontAndSize(bf, 8);
                                cb.ShowTextAligned(1, productsToPrintsList[r].ProductType, 208 * i + 109, document.PageSize.Height - 100 - 166 * k, 0);
                                cb.ShowTextAligned(1, "Код: " + productsToPrintsList[r].ID, 208 * i + 109, document.PageSize.Height - 163 - 166 * k, 0);
                                cb.ShowTextAligned(1, System.DateTime.Now.ToString("dd.MM.yyyy"), 208 * i + 190, document.PageSize.Height - 15 - 166 * k, 0);

                                r++;
                            }
                            if (r >= count) break;
                        }
                    }
                    document.Close();
                    MessageBox.Show("Ценники напечатаны!");
                    Process.Start(Environment.CurrentDirectory + "/Tags");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
