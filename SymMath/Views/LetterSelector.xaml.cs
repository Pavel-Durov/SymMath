using SymMath.Keyboard;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace SymMath
{
    public partial class LetterSelector : Window
    {
        private readonly TextBox[] _textBoxes;
        private Tuple<Char[], Char[]> _letters;

        public readonly Boolean IsEmpty;

        public LetterSelector(Key key, Tuple<Char[], Char[]> letters)
        {
            if (letters == null) throw new ArgumentNullException("letters");
            if (letters.Item1 == null || letters.Item2 == null || letters.Item1.Length == 0 || letters.Item1.Length != letters.Item2.Length)
                throw new ArgumentException("Invalid letter definition, unequal lower and upper case sequence length or length 0.");

            InitializeComponent();

            var letterTemplate = Utils.CloneWPFObject(this.LetterPanel.Children.Cast<TextBox>().First());
            var width = 0.0;

            // Remove sample children.  
            this.LetterPanel.Children.Clear();

            _textBoxes = new TextBox[letters.Item1.Length];

            // Add letters in order of appearance.
            for (var i = 0; i < letters.Item1.Length; i++)
            {
                var letter = letters.Item1[i];
                var newLetter = Utils.CloneWPFObject(letterTemplate);

                // Todo: it seems our "clone" is not cloning events, so let's hook it here.
                newLetter.PreviewMouseUp += OnMouseUp;
                // Adjust border thickness. It'd be nice if we can (?) do this in xaml using style ala css pseudo selectors
                var borderThick = newLetter.BorderThickness;
                borderThick.Left = borderThick.Right = 1;
                if (i == 0)
                {
                    borderThick.Right = letters.Item1.Length > 1 ? 1 : 2;
                    borderThick.Left = 2;
                }
                else if (i == letters.Item1.Length - 1)
                {
                    borderThick.Left = letters.Item1.Length > 1 ? 1 : 2;
                    borderThick.Right = 2;
                }
                newLetter.BorderThickness = borderThick;

                newLetter.Text = letter.ToString();

                this.LetterPanel.Children.Add(newLetter);

                _textBoxes[i] = newLetter;
                width += newLetter.Width;
            }

            this._letters = letters;

            // Restrict window size to panel width.
            this.Width = width + LetterPanel.Margin.Left * 2;

            this.Key = key;

            this.Loaded += (_, __) => SelectNext();
        }

        Storyboard _currentStoryboard;
        TextBox _prevSelectedTextBox;

        private void SetAnimation(TextBox sender)
        {
            var focusedTextBox = sender as TextBox;
            if (focusedTextBox != null)
            {
                if (_currentStoryboard != null)
                {
                    _currentStoryboard.Stop();
                }

                var fade = new DoubleAnimation()
                {
                    From = 0.5,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5),
                };

                Storyboard.SetTarget(fade, focusedTextBox);
                Storyboard.SetTargetProperty(fade, new PropertyPath(Button.OpacityProperty));

                _currentStoryboard = new Storyboard();
                _currentStoryboard.Children.Add(fade);

                _currentStoryboard.Begin();
            }
        }

        public readonly Key Key;

        public char SelectedLetter =>  _textBoxes[_activeIndex].Text[0];

        private Boolean _isLowerCase = true;

        public void ToUpper()
        {
            if (!_isLowerCase) return;

            for (var i = 0; i < _textBoxes.Length; i++)
                _textBoxes[i].Text = _letters.Item2[i].ToString();

            _isLowerCase = false;
        }

        public void ToLower()
        {
            if (_isLowerCase) return;

            for (var i = 0; i < _textBoxes.Length; i++)
                _textBoxes[i].Text = _letters.Item1[i].ToString();

            _isLowerCase = true;
        }

        private int _activeIndex = -1;

        public void SelectNext()
        {
            var count = _textBoxes.Length;
            var index = (_activeIndex + 1) % count;
            SelectTextBox(index);
        }

        private void SelectTextBox(int index)
        {
            _activeIndex = index;

            _prevSelectedTextBox = _textBoxes[_activeIndex];
            _prevSelectedTextBox.Focus();
            SetAnimation(_prevSelectedTextBox);
        }

        public void SelectPrevious()
        {
            var count = _textBoxes.Length;
            var index = (count + _activeIndex - 1) % count;
            SelectTextBox(index);
        }

        private void OnMouseUp(Object sender, MouseButtonEventArgs e)
        {
            var textBox = e.Source as TextBox;
            if (textBox == null) return;
            _activeIndex = Array.IndexOf(_textBoxes, textBox);
            textBox.Focus();
            Handler.HandleMouseUp();
        }
    }
}
