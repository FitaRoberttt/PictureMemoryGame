using MemoryGame.Model;
using MemoryGame.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MemoryGame.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public ICommand SaveGameCommand { get; private set; }
        public ICommand NewGameCommand { get; private set; }
        public ICommand HelpCommand { get; private set; }
        public SlideCollectionViewModel Slides { get; private set; }
        public TimerViewModel Timer { get; private set; }

        public List<Button> buttons;


        private GameModel _game;

        public GameModel Game
        {
            get { return _game; }
            set { _game = value; }
        }
        private Grid _grid;

        public Grid GameGrid
        {
            get { return _grid; }
            set { _grid = value; }
        }
        public GameViewModel()
        {
            Slides = new SlideCollectionViewModel();
            buttons = new List<Button>();
            

        }
        public GameViewModel(UserModel player, int gameSizeRow,int gameSizeColumn, Grid gameGrid)
        {
            player.GamesPlayed++;
            File.WriteAllText(@"C:\Facultate\An2Sem2\MVP\Tema1\MemoryGame\Database\" + player.UserName + ".json", JsonConvert.SerializeObject(player));
            Game = new GameModel(player, gameSizeRow, gameSizeColumn);
            GameGrid = gameGrid;
            
            SetupGame(gameGrid, gameSizeRow, gameSizeColumn);
            NewGameCommand = new RelayCommand(NewGame);
            SaveGameCommand = new RelayCommand(SaveGame);
          
        }
        public void SetupGame(Grid gameGrid, int gameSizeRow,int gameSizeColumn)
        {
            Slides = new SlideCollectionViewModel();
            Timer = new TimerViewModel(new TimeSpan(0, 0, 1));
            Slides.CreateSlides(gameSizeRow, gameSizeColumn);
            buttons = Slides.ChangeGridSize(gameGrid, gameSizeRow, gameSizeColumn);
            foreach (Button button in buttons)
            {
                button.Click += Slide_Clicked;
            }
            void Slide_Clicked(object sender, RoutedEventArgs e)
            {
                var button = sender as Button;
                ClickedSlide(button.DataContext);
            }
            Slides.Memorize();
            Game.ClearInfo();
            Timer.Start();

            OnPropertyRaised("Timer");
            OnPropertyRaised("Slides");
            OnPropertyRaised("Game");
        }

        public void NextLevel(Grid gameGrid, int gameSizeRow, int gameSizeColumn)
        {
            //Slides = new SlideCollectionViewModel();
            //Timer = new TimerViewModel(new TimeSpan(0, 0, 1));
            Slides.CreateSlides(gameSizeRow, gameSizeColumn);
            buttons = Slides.ChangeGridSize(gameGrid, gameSizeRow, gameSizeColumn);
            foreach (Button button in buttons)
            {
                button.Click += Slide_Clicked;
            }
            void Slide_Clicked(object sender, RoutedEventArgs e)
            {
                var button = sender as Button;
                ClickedSlide(button.DataContext);
            }
            Slides.Memorize();

        }
       
     
        public void ClickedSlide(object slide)
        {

            if (Slides.canSelect)
            {
                var selected = slide as PictureViewModel;
                Slides.SelectSlide(selected);
            }

            if (!Slides.areSlidesActive)
            {
                if (Slides.CheckIfMatched())
                    Game.Award(); //Correct match
                else
                    Game.Penalize();//Incorrect match
            }
            GameStatus();
        }
        private void GameStatus()
        {
            if (Game.MatchAttempts < 0)
            {
                Game.GameStatus(false);
                Timer.Stop();
                Slides.RevealUnmatched();        
                

            }
            if (Slides.AllSlidesMatched)
            {
                Game.GameLevel++;
                if(Game.GameLevel<3)
                {
                    NextLevel(GameGrid, Game.GameSizeRow, Game.GameSizeColumn);
                }
                else
                {
                    
                   
                    Game.GameStatus(true);
                    Timer.Stop();
                    Game.Player.GamesWon++;
                    File.WriteAllText(@"C:\Facultate\An2Sem2\MVP\Tema1\MemoryGame\Database\" + Game.Player.UserName + ".json", JsonConvert.SerializeObject(Game.Player));

                }
                
            }
        }
        public bool isGameWon()
        {
            if (Game.GameLevel==3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public void NewGame()
        {
            Game.Player.GamesPlayed++;
            File.WriteAllText(@"C:\Facultate\An2Sem2\MVP\Tema1\MemoryGame\Database\" + Game.Player.UserName + ".json", JsonConvert.SerializeObject(Game.Player));
            Game = new GameModel(Game.Player, Game.GameSizeRow, Game.GameSizeColumn);
            SetupGame(GameGrid, Game.GameSizeRow, Game.GameSizeColumn);
            NewGameCommand = new RelayCommand(NewGame);
            SaveGameCommand = new RelayCommand(SaveGame);
        }
      
        public void SaveGame()
        {
            DateTime now = new DateTime();
            File.WriteAllText(@"C:\Facultate\An2Sem2\MVP\Tema1\MemoryGame\Saves\save-" + DateTime.Now.ToString("MM.dd.yyyy-HH.mm.ss") + ".json", JsonConvert.SerializeObject(Game));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }




    }
}
