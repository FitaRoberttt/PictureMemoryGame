using MemoryGame.Model;
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
using System.Windows.Media;
using System.Windows.Threading;

namespace MemoryGame.ViewModel
{
    public class SlideCollectionViewModel : INotifyPropertyChanged
    {
        //Collection of picture slides
        public ObservableCollection<PictureViewModel> MemorySlides { get; private set; }

        //Selected slides for matching
        private PictureViewModel SelectedSlide1;
        private PictureViewModel SelectedSlide2;

        //Timers for peeking at slides and initial display for memorizing
        private DispatcherTimer _peekTimer;
        private DispatcherTimer _openingTimer;

        //Interval for how long a user peeks at selections
        private const int _peekSeconds = 1;
        //Interval for how long a user has to memorize slides
        private const int _openSeconds = 3;



        //Are selected slides still being displayed
        public bool areSlidesActive
        {
            get
            {
                if (SelectedSlide1 == null || SelectedSlide2 == null)
                    return true;

                return false;
            }
        }

        //Have all slides been matched
        public bool AllSlidesMatched
        {
            get
            {
                foreach (var slide in MemorySlides)
                {
                    if (!slide.isMatched)
                        return false;
                }

                return true;
            }
        }

        //Can user select a slide
        public bool canSelect { get; private set; }


        public SlideCollectionViewModel()
        {
            _peekTimer = new DispatcherTimer();
            _peekTimer.Interval = new TimeSpan(0, 0, _peekSeconds);
            _peekTimer.Tick += PeekTimer_Tick;

            _openingTimer = new DispatcherTimer();
            _openingTimer.Interval = new TimeSpan(0, 0, _openSeconds);
            _openingTimer.Tick += OpeningTimer_Tick;

        }

        //Create slides from images in file directory
        public void CreateSlides(int gameSizeRow, int gameSizeColumn)
        {
            //New list of slides
            int size = (gameSizeRow * gameSizeColumn)/2;
            MemorySlides = new ObservableCollection<PictureViewModel>();
            var models = GetModelsFrom();

            //Create slides with matching pairs from models
            for (int i = 0; i < size; i++)
            {
                //Create 2 matching slides
                var newSlide = new PictureViewModel(models[i]);
                var newSlideMatch = new PictureViewModel(models[i]);
                //Add new slides to collection
                MemorySlides.Add(newSlide);
                MemorySlides.Add(newSlideMatch);
                //Initially display images for user
                newSlide.PeekAtImage();
                newSlideMatch.PeekAtImage();
            }

            ShuffleSlides();
            OnPropertyRaised("MemorySlides");
        }

        //Select a slide to be matched
        public void SelectSlide(PictureViewModel slide)
        {
            slide.PeekAtImage();

            if (SelectedSlide1 == null)
            {
                SelectedSlide1 = slide;
            }
            else if (SelectedSlide2 == null)
            {
                SelectedSlide2 = slide;
                HideUnmatched();
            }

            //SoundManager.PlayCardFlip();
            OnPropertyRaised("areSlidesActive");
        }

        //Are the selected slides a match
        public bool CheckIfMatched()
        {
            if (SelectedSlide1.Id == SelectedSlide2.Id)
            {
                MatchCorrect();
                return true;
            }
            else
            {
                MatchFailed();
                return false;
            }
        }

        //Selected slides did not match
        private void MatchFailed()
        {
            SelectedSlide1.MarkFailed();
            SelectedSlide2.MarkFailed();
            ClearSelected();
        }

        //Selected slides matched
        private void MatchCorrect()
        {
            SelectedSlide1.MarkMatched();
            SelectedSlide2.MarkMatched();
            ClearSelected();
        }

        //Clear selected slides
        private void ClearSelected()
        {
            SelectedSlide1 = null;
            SelectedSlide2 = null;
            canSelect = false;
        }

        //Reveal all unmatched slides
        public void RevealUnmatched()
        {
            foreach (var slide in MemorySlides)
            {
                if (!slide.isMatched)
                {
                    _peekTimer.Stop();
                    slide.MarkFailed();
                    slide.PeekAtImage();
                }
            }
        }

        //Hide all slides that are unmatched
        public void HideUnmatched()
        {
            _peekTimer.Start();
        }

        //Display slides for memorizing
        public void Memorize()
        {
            _openingTimer.Start();
        }

        //Get slide picture models for creating picture views
        private List<PictureModel> GetModelsFrom()
        {
            //List of models for picture slides
            var models = new List<PictureModel>();
            //Get all image URIs in folder
            string[] images = Directory.GetFiles(@"C:/Facultate/An2Sem2/MVP/Tema1/MemoryGame/SlideShow");
            //Slide id begin at 0
            var id = 0;

            foreach (string i in images)
            {
                models.Add(new PictureModel() { Id = id, ImageSource = i });
                id++;
            }

            return models;
        }

        //Randomize the location of the slides in collection
        private void ShuffleSlides()
        {
            //Randomizing slide indexes
            var rnd = new Random();
            //Shuffle memory slides
            for (int i = 0; i < 64; i++)
            {
                MemorySlides.Reverse();
                MemorySlides.Move(rnd.Next(0, MemorySlides.Count), rnd.Next(0, MemorySlides.Count));
            }
        }
        //Close slides being memorized
        private void OpeningTimer_Tick(object sender, EventArgs e)
        {
            foreach (var slide in MemorySlides)
            {
                slide.ClosePeek();
                canSelect = true;
            }
            OnPropertyRaised("areSlidesActive");
            _openingTimer.Stop();
        }

        //Display selected card
        private void PeekTimer_Tick(object sender, EventArgs e)
        {
            foreach (var slide in MemorySlides)
            {
                if (!slide.isMatched)
                {
                    slide.ClosePeek();
                    canSelect = true;
                }
            }
            OnPropertyRaised("areSlidesActive");
            _peekTimer.Stop();
        }

        public List<Button> ChangeGridSize(Grid gameGrid, int gameSizeRow, int gameSizeColumn)
        {
            // Eliminați toate butoanele existente
            List<Button> buttons = new List<Button>();
            int rows = gameSizeRow;
            int columns = gameSizeColumn;

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
            int index = 0;
            // Adăugați butoane noi
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {

                    Button button = new Button();
                    button.Name = "button" + ((i * columns) + j + 1);
                    Image image = new Image { Stretch = Stretch.Fill };
                    Binding binding = new Binding("SlideImage");
                    binding.Source = MemorySlides[(i * columns) + j];
                    BindingOperations.SetBinding(image, Image.SourceProperty, binding);
                    button.Content = image;
                    button.IsEnabled = true; // setează valoarea inițială a proprietății IsEnabled
                    button.DataContext = MemorySlides[(i * columns) + j]; // setează DataContext-ul butonului
                    //button.Click += Slide_Clicked; // adaugă un handler pentru evenimentul Click
                    Binding bindingEnabled = new Binding("isSelectable");
                    bindingEnabled.Source = MemorySlides[(i * columns) + j];
                    BindingOperations.SetBinding(button, Button.IsEnabledProperty, bindingEnabled);
                    buttons.Add(button);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    gameGrid.Children.Add(button);
                    
                }
            }

            return buttons;


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
