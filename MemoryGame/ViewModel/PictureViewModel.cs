using MemoryGame.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MemoryGame.ViewModel
{
    public class PictureViewModel : INotifyPropertyChanged
    {
        //Model for this view
        private PictureModel _model;

        //ID of this slide
        public int Id { get; private set; }

        //Slide status
        private bool _isViewed;
        private bool _isMatched;
        private bool _isFailed;

        //Is being viewed by user
        public bool isViewed
        {
            get
            {
                return _isViewed;
            }
            private set
            {
                _isViewed = value;
                OnPropertyRaised("SlideImage");
                OnPropertyRaised("BorderBrush");
            }
        }

        //Has been matched
        public bool isMatched
        {
            get
            {
                return _isMatched;
            }
            private set
            {
                _isMatched = value;
                OnPropertyRaised("SlideImage");
                OnPropertyRaised("BorderBrush");
            }
        }

        //Has failed to be matched
        public bool isFailed
        {
            get
            {
                return _isFailed;
            }
            private set
            {
                _isFailed = value;
                OnPropertyRaised("SlideImage");
                OnPropertyRaised("BorderBrush");
            }
        }

        //User can select this slide
        public bool isSelectable
        {
            get
            {
                if (isMatched)
                    return false;
                if (isViewed)
                    return false;

                return true;
            }
        }

        //Image to be displayed
        public string SlideImage
        {
            get
            {
                if (isMatched)
                    return _model.ImageSource;
                if (isViewed)
                    return _model.ImageSource;

                return "C:/Users/fitar/Downloads/MemoryGame-master/MemoryGame-master/MemoryGame/Assets/mystery_image.jpg";
            }
        }

        //Brush color of border based on status
        public Brush BorderBrush
        {
            get
            {
                if (isFailed)
                    return Brushes.Red;
                if (isMatched)
                    return Brushes.Green;
                if (isViewed)
                    return Brushes.Yellow;

                return Brushes.Black;
            }
        }


        public PictureViewModel(PictureModel model)
        {
            _model = model;
            Id = model.Id;
        }



        //Has been matched
        public void MarkMatched()
        {
            isMatched = true;
        }

        //Has failed to match
        public void MarkFailed()
        {
            isFailed = true;
        }

        //No longer being viewed
        public void ClosePeek()
        {
            isViewed = false;
            isFailed = false;
            OnPropertyRaised("isSelectable");
            OnPropertyRaised("SlideImage");
        }

        //Let user view
        public void PeekAtImage()
        {
            isViewed = true;
            OnPropertyRaised("SlideImage");
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
