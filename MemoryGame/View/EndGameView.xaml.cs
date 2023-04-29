using MemoryGame.Model;
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

namespace MemoryGame.View
{
    /// <summary>
    /// Interaction logic for EndGameView.xaml
    /// </summary>
    public partial class EndGameView : Window
    {
        public EndGameView()
        {
            InitializeComponent();
        }

        public EndGameView(UserModel player)
        {
            DataContext = new EndGameModel(player);
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow menu = new MainWindow();
            this.Visibility = Visibility.Hidden;
            menu.Show();
        }
    }
}
