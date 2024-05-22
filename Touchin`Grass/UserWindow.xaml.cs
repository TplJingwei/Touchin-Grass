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

namespace Touchin_Grass
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public int plantid;
        public string less;
        public string more;
        public string PToBe;
        public string How2;
        public bool searchInWork =false;
        public UserWindow()
        {
            InitializeComponent();
            updateComboBox();
            update();
        }


        public void update()
        {
            var list = App.Garden.Plant.ToList();
            var list2 = App.Garden.Plant.ToList();

            
            List<string> listOfNames = new List<string>();

           
            if (CBCategory.SelectedIndex == 0)
            {
                list = App.Garden.Plant.ToList();
            }
            else
            {
                list = App.Garden.Plant.Where(x => x.Category.Equals(CBCategory.SelectedIndex)).ToList();
            }
            string stroka;

            foreach (Plant i in list)
            {
                stroka = i.Name;
                listOfNames.Add(stroka);
            }

            if (ListPrikolov != null)
            {
                ListPrikolov.ItemsSource = listOfNames;
            }
        }

        public void updateComboBox()
        {
            var list = App.Garden.Cat.ToList();
            int d = 0;
            foreach (var i in list)
            {
                string chk;
                chk = i.Category.ToString();
                CBCategory.Items.Add(chk);
            }
        }
        #region Ужасы
        private void ListPrikolov_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            GridM.Visibility = Visibility.Visible;
            PlantInfo.Visibility = Visibility.Visible;
            MoreAbout.IsEnabled = true;
            MoreAbout.Visibility = Visibility.Visible;
            PlaceAbout.IsEnabled = true;
            PlaceAbout.Visibility = Visibility.Visible;
            RecipeAbout.IsEnabled = true;
            RecipeAbout.Visibility = Visibility.Visible;
            if (MoreAbout.Visibility == Visibility.Visible)
            {
                LessAbout.Visibility = Visibility.Hidden;
            }


            Keyboard.ClearFocus();
            if (searchInWork == false)
            {
                var list = App.Garden.Plant.ToList();

                if (CBCategory.SelectedIndex != 0)
                {
                    list = App.Garden.Plant.Where(x => x.Category.Equals(CBCategory.SelectedIndex)).ToList();
                }

                try
                {
                    plantid = list[ListPrikolov.SelectedIndex].id;
                    PlantName.Text = list[ListPrikolov.SelectedIndex].Name;
                    less = list[ListPrikolov.SelectedIndex].Description;
                    more = list[ListPrikolov.SelectedIndex].MoreInfo;
                    PToBe = list[ListPrikolov.SelectedIndex].Place;
                    How2 = list[ListPrikolov.SelectedIndex].Recipe;
                    PlantInfo.Text = less;
                    PicOfPlant.Source = new BitmapImage(new Uri(list[ListPrikolov.SelectedIndex].Picture));
                    if (list[ListPrikolov.SelectedIndex].is_poisonous == true)
                    {
                        Warning.Visibility = Visibility.Visible;
                    }
                    else { Warning.Visibility = Visibility.Hidden; }
                }
                catch (Exception ex)
                { }
            }
            else
            {
                CBCategory.SelectedIndex = 0;
                var list = App.Garden.Plant.ToList();
                var search = PlantSearch.Text;
                string stroka;
                List<string> listOfNames = new List<string>();
                if (search != null)
                {
                    searchInWork = true;
                    list = list.Where(x => x.Name.Contains(search)).ToList();
                }
                else
                {
                    searchInWork = false;
                }
                foreach (var i in list)
                {
                    stroka = i.Name;
                    listOfNames.Add(stroka);
                }
                ListPrikolov.ItemsSource = listOfNames;
                try
                {
                    plantid = list[ListPrikolov.SelectedIndex].id;
                    PlantName.Text = list[ListPrikolov.SelectedIndex].Name;
                    less = list[ListPrikolov.SelectedIndex].Description;
                    more = list[ListPrikolov.SelectedIndex].MoreInfo;
                    PToBe = list[ListPrikolov.SelectedIndex].Place;
                    How2 = list[ListPrikolov.SelectedIndex].Recipe;
                    PlantInfo.Text = less;
                    PicOfPlant.Source = new BitmapImage(new Uri(list[ListPrikolov.SelectedIndex].Picture));
                    if (list[ListPrikolov.SelectedIndex].is_poisonous == true)
                    {
                        Warning.Visibility = Visibility.Visible;
                    }
                    else { Warning.Visibility = Visibility.Hidden; }
                }
                catch (Exception ex)
                { }
            }
        }
#endregion
        #region Фокусы
        private void PlantSearch_GotFocus(object sender, RoutedEventArgs e)
        { 
        SearchHint.Visibility = Visibility.Hidden;
        }
        private void PlantSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PlantSearch.Text == String.Empty)
            { 
            SearchHint.Visibility= Visibility.Visible;
            }
        }

        private void PlantSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var list = App.Garden.Plant.ToList();
            var search = PlantSearch.Text;
            string stroka;
            List<string> listOfNames = new List<string>();
            if (search != null)
            {
                searchInWork = true;
                list= list.Where(x => x.Name.Contains(search)).ToList();
            }
            else
            { 
                searchInWork=false;
            }
            foreach (var i in list)
            {
                stroka = i.Name;
                listOfNames.Add(stroka);
            }
            ListPrikolov.ItemsSource = listOfNames;
        }
        private void PlantSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
            PlantSearch_LostFocus(PlantSearch, e);
        }

        #endregion
        #region Тык-тык-тык
        private void MoreAbout_Click(object sender, RoutedEventArgs e)
        {
            if (ListPrikolov.SelectedIndex != -1)
            {
                PlantInfo.Text = more;
            }
            else 
            {
                PlantInfo.Text=more;
            }
            MoreAbout.Visibility = Visibility.Hidden;
            MoreAbout.IsEnabled = false;
            LessAbout.IsEnabled = true;
            LessAbout.Visibility = Visibility.Visible;
        }

        private void LessAbout_Click(object sender, RoutedEventArgs e)
        {

            if (ListPrikolov.SelectedIndex != -1)
            {
                PlantInfo.Text = less;
            }
            else
            {
                PlantInfo.Text=less;
            }
            LessAbout.Visibility = Visibility.Hidden;   
            LessAbout.IsEnabled = false;
            MoreAbout.IsEnabled = true;
            MoreAbout.Visibility = Visibility.Visible;
        }
        private void RecipeAbout_Click(object sender, RoutedEventArgs e)
        {
            if (ListPrikolov.SelectedIndex != -1)
            {
                PlantInfo.Text = How2;
            }
            else
            {
                PlantInfo.Text = How2;
            }
            MoreAbout.Visibility = Visibility.Hidden;
            MoreAbout.IsEnabled = false;
            LessAbout.IsEnabled = true;
            LessAbout.Visibility = Visibility.Visible;
        }
        private void PlaceAbout_Click(object sender, RoutedEventArgs e)
        {

            if (ListPrikolov.SelectedIndex != -1)
            {
                PlantInfo.Text = PToBe;
            }
            else
            {
               PlantInfo.Text = PToBe;
            }
            MoreAbout.Visibility = Visibility.Hidden;
            MoreAbout.IsEnabled = false;
            LessAbout.IsEnabled = true;
            LessAbout.Visibility = Visibility.Visible;
        }
        #endregion


        private void CBCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            update();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Завершить сеанс?", "Выход", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            { 
                MainWindow MW = new MainWindow();
                MW.Show();
                this.Close();
            }
        }
    }
} 