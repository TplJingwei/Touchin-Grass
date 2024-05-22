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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            Update();
            updateComboBox();
            ChangeRecord.Visibility = Visibility.Hidden;
            ChangeRecord.IsEnabled = false;
        }
        public void ClearText()
        {
            TboxName.Text = "";
            RichDescription.Document.Blocks.Clear();
            TboxPhoto.Text = "";
            RichMoreInfo.Document.Blocks.Clear();
            RichRecipe.Document.Blocks.Clear();
            TboxPlace.Text = "";
            CBCategory.SelectedIndex = -1;
            IdSelectedPlant.Content = "";
            PicOfPlant.Source = null;
            Poison.IsChecked = false;

            ChangeRecord.Visibility = Visibility.Hidden;
            ChangeRecord.IsEnabled = false;
            AddRecord.IsEnabled = true;
            AddRecord.Visibility = Visibility.Visible;
        }
        #region Обновления
        public void Update()
        {
            var search = SearchPlant.Text;
            var list = App.Garden.Plant.ToList();
            if (search != null)
            { 
                list = App.Garden.Plant.Where(x => x.Name.Contains(search)).ToList(); 
            }
            DGridPlant.ItemsSource = list;
        }
        public void updateComboBox()
        {
            var list = App.Garden.Cat;
            List<string> listToExport = new List<string>();
            foreach (var i in list)
            {
                listToExport.Add(i.Category.ToString());
            }
            CBCategory.ItemsSource = listToExport;
        }
        #endregion

        #region Позже придумаю шутку
        private void DGridPlant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearText();
            try
            {
                var plant = (Plant)DGridPlant.SelectedItem;

                var MoreInfo = App.Garden.Plant.ToList();
                if (plant == null)
                {
                    this.DGridPlant.SelectedItem = null;
                    this.DGridPlant.IsReadOnly = true;
                }
                else
                {
                    if (plant.Picture != null)
                    {
                    }
                    ChangeRecord.Visibility = Visibility.Visible;
                    ChangeRecord.IsEnabled = true;
                    TboxName.Text = plant.Name;

                    FlowDocument pDesc = new FlowDocument();
                    Paragraph PDpar = new Paragraph();
                    PDpar.Inlines.Add(new Run(plant.Description.ToString()));
                    pDesc.Blocks.Add(PDpar);
                    RichDescription.Document = pDesc;

                    FlowDocument pMoI = new FlowDocument();
                    Paragraph PPMoI = new Paragraph();
                    PPMoI.Inlines.Add(new Run(plant.MoreInfo.ToString()));
                    pMoI.Blocks.Add(PPMoI);
                    RichMoreInfo.Document = pMoI;

                    FlowDocument PRecipe = new FlowDocument();
                    Paragraph PPRec = new Paragraph();
                    PPRec.Inlines.Add(new Run(plant.Recipe.ToString()));
                    PRecipe.Blocks.Add(PPRec);
                    RichRecipe.Document = PRecipe;

                    TboxPhoto.Text = plant.Picture;
                    TboxPlace.Text = MoreInfo[DGridPlant.SelectedIndex].Place;
                    CBCategory.SelectedIndex = Convert.ToInt32(plant.Category) - 1;
                    IdSelectedPlant.Content = plant.id;
                    if (plant.is_poisonous == true)
                    {
                        Poison.IsChecked = true;
                    }
                    try
                    {
                        PicOfPlant.Source = new BitmapImage(new Uri(plant.Picture));
                    }
                    catch (Exception ex)
                    { }
                }
            }
            catch (Exception ex1)
            { }

        }
        #endregion

        #region Фокусы с исчезновением 
        private void SeachPlant_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchLabel.Visibility = Visibility.Hidden;
        }

        private void SearchPlant_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchPlant.Text != string.Empty)
            { }
            else
            {
                SearchLabel.Visibility = Visibility.Visible;
            }
        }
        #endregion

        private void SearchPlant_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void ChangeRecordInDB(object sender, RoutedEventArgs e)
        {
            TextRange ObjDesc = new TextRange(RichDescription.Document.ContentStart, RichDescription.Document.ContentEnd);
            TextRange ObjMInfo = new TextRange(RichMoreInfo.Document.ContentStart, RichMoreInfo.Document.ContentEnd);
            TextRange ObjRec = new TextRange(RichRecipe.Document.ContentStart, RichRecipe.Document.ContentEnd);

            var Plant = (Plant)DGridPlant.SelectedItem;
            var Change = App.Garden.Plant.Where(x => x.id == Plant.id).FirstOrDefault();

            Change.Name= TboxName.Text;
            Change.Description = ObjDesc.Text;
            Change.Category = CBCategory.SelectedIndex+1;
            Change.Picture = TboxPhoto.Text;
            Change.MoreInfo = ObjMInfo.Text;
            Change.Recipe = ObjRec.Text;
            Change.Place = TboxPlace.Text;
            Change.is_poisonous = Poison.IsChecked;

            App.Garden.SaveChanges();
            Update();
            MessageBox.Show("Данные изменины", "Успех");
        }
        private void AddRecordToDB(object sender, RoutedEventArgs e)
        {

            TextRange ObjDesc = new TextRange(RichDescription.Document.ContentStart, RichDescription.Document.ContentEnd);
            TextRange ObjMInfo = new TextRange(RichMoreInfo.Document.ContentStart, RichMoreInfo.Document.ContentEnd);
            TextRange ObjRec = new TextRange(RichRecipe.Document.ContentStart, RichRecipe.Document.ContentEnd);

            var Plant = App.Garden.Plant.ToList();
            Plant P1 = new Plant
            { 
                Name = TboxName.Text,
                Description = ObjDesc.Text,
                Category = CBCategory.SelectedIndex+1,
                MoreInfo = ObjMInfo.Text,
                Picture = TboxPhoto.Text,
                Place = TboxPlace.Text,
                Recipe = ObjRec.Text,
                is_poisonous = Poison.IsChecked
            };

            if (TboxName.Text !=null ||  CBCategory.SelectedIndex != -1)
            {
                App.Garden.Plant.Add(P1);
                App.Garden.SaveChanges();
                Update();
                MessageBox.Show("Запись успешно добавлена", "Успех");
                ClearText();
            }
            else 
            {
                MessageBox.Show("Не указано имя/категория растения", "Ошибка");
            }
        }

        private void ButtonToDeleteTheWorld_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var pi = (Plant)DGridPlant.SelectedItem;
                App.Garden.Plant.Remove(pi);
                App.Garden.SaveChanges();
                Update();
                MessageBox.Show("Запись удалена");
                ClearText();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void TboxPhoto_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                PicOfPlant.Source = new BitmapImage(new Uri(TboxPhoto.Text));
            }
            catch (Exception ex)
            { }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Очитить поля?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ClearText();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Завершить сессию?", "Выход", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MainWindow MW = new MainWindow();
                MW.Show();
                this.Close();
            }
        }
    }
}
