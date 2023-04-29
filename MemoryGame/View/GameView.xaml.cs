using MemoryGame.Model;
using MemoryGame.ViewModel;
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
using System.Windows.Threading;


namespace MemoryGame.View
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : Window
    {
        /*
        void ChangeGridSize(int gameSize)
        {
            // Eliminați toate butoanele existente

            int rows = gameSize;
            int columns = gameSize;



            gameGrid.Children.Clear();
            gameGrid.RowDefinitions.Clear();
            gameGrid.ColumnDefinitions.Clear();

            // Creați rânduri și coloane noi
            for (int i = 0; i < rows; i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < columns; i++)
            {
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }


            // Adăugați butoane noi
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Button button = new Button();
                    button.Name = "button" + ((i * columns) + j + 1);
                    Image image = new Image { Stretch = Stretch.Fill };
                    Binding binding = new Binding($"SlidePhoto[{i * gameSize + j}]");
                    binding.Source = DataContext;
                    BindingOperations.SetBinding(image, Image.SourceProperty, binding);
                    button.Content = image;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    gameGrid.Children.Add(button);

                }
            }

            // Atribuiți pozele corespunzătoare butoanelor
        }
        */

        public GameView(UserModel player, int gameSizeRow,int gameSizeColumn)
        {
            
            InitializeComponent();
            DataContext = new GameViewModel(player, gameSizeRow, gameSizeColumn, gameGrid);
            var viewmodel = (GameViewModel)DataContext;
           
        }

        private void Stats(object sender, RoutedEventArgs e)
        {

            var viewmodel = (GameViewModel)DataContext;
            EndGameView end = new EndGameView(viewmodel.Game.Player);
            this.Visibility = Visibility.Hidden;
            end.Show();

            
        }
    }
}
