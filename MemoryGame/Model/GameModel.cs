using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MemoryGame.Model
{
    public class GameModel:INotifyPropertyChanged
    {
        private UserModel _player;
        private int _gameSizeRow;
        private int _gameSizeColumn;
        private int _gameLevel;

        private const int _maxAttempts = 5;
        private const int _pointAward = 75;
        private const int _pointDeduction = 15;

        private int _matchAttempts;
        private int _score;

        private bool _gameLost;
        private bool _gameWon;

        public GameModel()
        {
            _player = new UserModel();
            _gameSizeRow = 0;
            _gameSizeColumn = 0;
            _matchAttempts = 0;
            _score = 0;
            _gameLevel = 1;
            _gameLost = false;
            _gameWon = false;
        }

        public GameModel(GameModel model)
        {
            _player = new UserModel(model.Player);
            _gameSizeRow = model.GameSizeRow;
            _gameSizeColumn = model.GameSizeColumn;
            _matchAttempts = model.MatchAttempts;
            _score = model.Score;
            _gameLevel = model.GameLevel;
            

        }

        public GameModel(UserModel player, int gameSizeRow, int gameSizeColumn)
        {
            _player = new UserModel(player);
            _gameSizeRow = gameSizeRow;
            _gameSizeColumn = gameSizeColumn;
            _matchAttempts = _maxAttempts;
            _score = 0;
            _gameLost = false;
            _gameWon = false;
        }

        public UserModel Player
        {
            get { return _player; }
            set { _player = value; }
        }
        
        public int GameSizeRow
        {
            get { return _gameSizeRow; }
            set { _gameSizeRow = value; }
        }
        public int GameSizeColumn
        {
            get { return _gameSizeColumn; }
            set { _gameSizeColumn = value; }
        }
        public int Score
        {
            get
            {
                return _score;
            }
            private set
            {
                _score = value;
                OnPropertyRaised("Score");
            }
        }
        
        public int GameLevel
        {
            get { return _gameLevel; }
            set { _gameLevel = value;
                OnPropertyRaised("GameLevel");
            }
        }
        public void Award()
        {
            Score += _pointAward;

        }
        public int MatchAttempts
        {
            get
            {
                return _matchAttempts;
            }
            private set
            {
                _matchAttempts = value;
                OnPropertyRaised("MatchAttempts");
            }
        }

      

        public void Penalize()
        {
            Score -= _pointDeduction;
            MatchAttempts--;

        }

        public void ClearInfo()
        {
            Score = 0;
            Score = 0;
            MatchAttempts = _maxAttempts;
            _gameLost = false;
            _gameWon = false;
            OnPropertyRaised("LostMessage");
            OnPropertyRaised("WinMessage");
        }
        public void GameStatus(bool win)
        {
            if (!win)
            {
                _gameLost = true;
                OnPropertyRaised("LostMessage");
            }

            if (win)
            {
                _gameWon = true;
                OnPropertyRaised("WinMessage");
            }
        }
        public Visibility LostMessage
        {
            get
            {
                if (_gameLost)
                    return Visibility.Visible;

                return Visibility.Hidden;
            }
        }

        public Visibility WinMessage
        {
            get
            {
                if (_gameWon)
                    return Visibility.Visible;

                return Visibility.Hidden;
            }
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
