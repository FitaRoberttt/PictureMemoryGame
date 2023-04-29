using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Model
{
   public class UserModel
    {
        public UserModel()
        {
            _username = "";
            _gamesPlayed = 0;
            _gamesWon = 0;

        }

        public UserModel(string username, System.Uri profilePicture)
        {
            _username = username;
            _gamesPlayed = 0;
            _profilePicture = profilePicture;
            _gamesWon = 0;
        }

        public UserModel(UserModel user)
        {
            _gamesPlayed=user.GamesPlayed;
            _gamesWon   =user.GamesWon;
            _profilePicture = user.ProfilePicture;
            _username = user.UserName;
        }

     

        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        public int GamesPlayed
        {
            get { return _gamesPlayed; }
            set { _gamesPlayed = value; }
        }

        public int GamesWon
        {
            get { return _gamesWon; }
            set { _gamesWon = value; }
        }

        public System.Uri ProfilePicture
        {
            get { return _profilePicture; }
            set { _profilePicture = value; }    
        }

  

        private string _username;
        private int _gamesPlayed;
        private int _gamesWon;
        private System.Uri _profilePicture;
        public override string ToString()
        {
            return _username;
        }
      
    }
}
