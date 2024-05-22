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

namespace Touchin_Grass
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Window;
        public MainWindow()
        {
            InitializeComponent();
            Window = this;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        { 
            Keyboard.ClearFocus();
            TboxLogin_LostFocus(TBoxLogin, e);
            PBoxPassword_LostFocus(PBoxPassword, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var LoginList = App.Garden.Authorization.ToList();
            Authorization Iser;
            Iser = LoginList.Where(x => x.Login == TBoxLogin.Text && x.Password == PBoxPassword.Password).FirstOrDefault();
            { if (Iser != null)
                {
                    if (Iser.Role == 1)
                    {
                        UserWindow Dictionar = new UserWindow();
                        Dictionar.Show();
                        this.Close();
                    }
                    else if (Iser.Role == 0)
                    { 
                        AdminWindow Admin = new AdminWindow();
                        Admin.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль", "Ошибка");
                }
            }
        }

        private void PBoxPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            LabelHintPassword.Visibility = Visibility.Hidden;
        }

        private void PBoxPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PBoxPassword.Password == string.Empty)
            {
                LabelHintPassword.Visibility = Visibility.Visible;
            }
        }
        private void  TboxLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            LabelHintLogin.Visibility = Visibility.Hidden;
        }

        private void TboxLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TBoxLogin.Text == string.Empty)
            {
                LabelHintLogin.Visibility = Visibility.Visible;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Закрыть программу?", "Выход", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            { 
             this.Close();
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                MainWindow.Window.DragMove();
            }
        }
    }
}
