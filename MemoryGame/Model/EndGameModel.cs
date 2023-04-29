using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
    public class EndGameModel
    {
        private UserModel _player;
        private string _winrate;
        private int _gamesLost;
       
    
     

        public EndGameModel(UserModel player)
        {
            _player = new UserModel(player);
            _gamesLost = _player.GamesPlayed - _player.GamesWon;
            _winrate = ((double)_player.GamesWon / (double)_player.GamesPlayed * 100).ToString() + "%";
            
            

        }

        public EndGameModel()
        {
            _player = new UserModel(null);
            _winrate = "0%";
            _gamesLost = 0;
            
      
        }

        public UserModel Player
        {
            get { return _player; }
            set { _player = value; }
        }

        public string Winrate
        {
            get { return _winrate; }
            set { _winrate = value; }
        }

        public int GamesLost
        {
            get { return _gamesLost; }
            set { _gamesLost = value; }
        }

   

      

      
    }
}
