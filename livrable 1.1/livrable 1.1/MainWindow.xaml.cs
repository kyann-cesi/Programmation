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


namespace livrable_1._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>



    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Switchlanguage("fr");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {



        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Switchlanguage("fr");
        }
        private void Switchlanguage(string languageCode)
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            switch (languageCode)
            {
                case "en":
                    dictionary.Source = new Uri("..\\Dictionary1.xaml", UriKind.Relative);
                    break;
                case "fr":
                    dictionary.Source = new Uri("..\\francais.xaml", UriKind.Relative);
                    break;
                default:
                    dictionary.Source = new Uri("..\\francais.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dictionary);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Switchlanguage("en");
        }
    
    }
}
