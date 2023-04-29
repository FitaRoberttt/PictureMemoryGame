using MemoryGame.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MemoryGame.ViewModel
{
    public class MenuViewModel: INotifyPropertyChanged
    {
        string[] savedImages; // de bagat converter
        public ICommand NewUserCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }
        public ICommand NextImageCommand { get; private set; }
        public ICommand PreviousImageCommand { get; private set; }

        private int _imageIndex;
        public int ImageIndex
        {
            get { return _imageIndex; }
            set { _imageIndex = AdjustIndex(value); }
            
        }
        
        private System.Uri _imageUri;
        public System.Uri ImageUri
        {
            get { return _imageUri; }
            set { _imageUri = value; }
        }


        public BitmapImage _profilePicture;
        public BitmapImage ProfilePicture
        {
            get { return _profilePicture; }
            set
            {
                _profilePicture = value;
                OnPropertyRaised("ProfilePicture");
            }
        }

        public ObservableCollection<BitmapImage> _profilePictures;
        public ObservableCollection<BitmapImage> ProfilePictures
        {
            get { return _profilePictures; }
            set { _profilePictures = value; }
        }

        private ObservableCollection<UserModel> _users;
        public ObservableCollection<UserModel> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        private string _newUsername;
        public string NewUsername
        {
            get { return _newUsername; }
            set { _newUsername = value; }

        }

        public UserModel _selectedUser;
        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; }
        }

        public UserModel _newUser;
        public UserModel NewUser
        {
            get { return _newUser; }
            set { _newUser = value; }
        }
        

        private int _gameSizeRow;
        public int GameSizeRow
        {
            get { return  _gameSizeRow;}
            set { _gameSizeRow = value;}
        }

        private int _gameSizeColumn;
        public int GameSizeColumn
        {
            get { return _gameSizeColumn; }
            set { _gameSizeColumn = value; }
        }

        public MenuViewModel()
        {
            _newUsername = "";
            _imageIndex = 0;
            NewUserCommand = new RelayCommand(AddNewUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
            NextImageCommand = new RelayCommand(NextImage);
            PreviousImageCommand = new RelayCommand(PreviousImage);
            _selectedUser = new UserModel();

            _users = new ObservableCollection<UserModel>();

            string[] savedUsers = Directory.GetFiles(@"C:\Facultate\An2Sem2\MVP\Tema1\MemoryGame\Database");
            
            foreach (string user in savedUsers)
            {
                string json = File.ReadAllText(user);
                UserModel userFromFile = JsonConvert.DeserializeObject<UserModel>(json);
                _users.Add(userFromFile);
            }
            
            _profilePictures = new ObservableCollection<BitmapImage>();
            savedImages = Directory.GetFiles(@"C:\Facultate\An2Sem2\MVP\Tema1\MemoryGame\ProfileImages");
            _profilePicture = new BitmapImage(new System.Uri(@"C:\Facultate\An2Sem2\MVP\Tema1\MemoryGame\ProfileImages\1.jpg"));
            foreach (var imagePath in savedImages)
            {
                _profilePictures.Add(new BitmapImage(new System.Uri(imagePath)));

            }


        }
        public object PathToImageConvert(string value)
        {
            return new BitmapImage(new Uri((string)value));
        }
        public void AddNewUser()
        {
            if (_newUsername != null && _newUsername != "" && IsUsernameUnique(_newUsername) == true)
            {
                UserModel newUser = new UserModel(_newUsername, new Uri(savedImages[AdjustIndex(_imageIndex)]));
                Users.Add(newUser);
                File.WriteAllText(@"C:\Facultate\An2Sem2\MVP\Tema1\MemoryGame\Database\" + _newUsername + ".json", JsonConvert.SerializeObject(newUser));
                _newUsername = "";

            }

        }
        public void DeleteUser()
        {
            if (SelectedUser != null && _users.Count != 0)
            {
                string[] savedUsers = Directory.GetFiles(@"C:\Facultate\An2Sem2\MVP\Tema1\MemoryGame\Database", "*.json");
                foreach (var userFile in savedUsers)
                {
                    var savedUser = JsonConvert.DeserializeObject<UserModel>(File.ReadAllText(userFile));

                    if (savedUser.UserName == SelectedUser.UserName && savedUser != null)
                    {
                        File.Delete(userFile);
                    }
                }
                Users.Remove(SelectedUser);

                SelectedUser = new UserModel();
            }
        }

        public void NextImage()
        {
            ImageIndex++;
            ProfilePicture = new BitmapImage(new System.Uri(savedImages[ImageIndex]));
            _imageUri = new System.Uri(savedImages[ImageIndex]);

        }

        public void PreviousImage()
        {
            --_imageIndex;
            _imageIndex = AdjustIndex(_imageIndex);
            ProfilePicture = new BitmapImage(new System.Uri(savedImages[ImageIndex]));
            _imageUri = new System.Uri(savedImages[ImageIndex]);

        }
        private int AdjustIndex(int imageIndex)
        {
            if (imageIndex == -1)
            {
                imageIndex = savedImages.Count() - 1;

                return savedImages.Count() - 1;
            }

            if (imageIndex > savedImages.Count() - 1)
            {
                imageIndex = imageIndex % savedImages.Count();
                return imageIndex % savedImages.Count();
            }

            return imageIndex;
        }
        private bool IsUsernameUnique(string username)
        {
            foreach (var user in _users)
            {
                if (user.UserName == username)
                    return false;
            }
            return true;
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
