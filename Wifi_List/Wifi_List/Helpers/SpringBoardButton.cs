using Xamarin.Forms;


namespace Wifi_List
{
    public class SpringBoardButton : ContentView
    {
        public Image SBIcon { get; set; }
        public Label SBLabel { get; set; }
        public ImageSource Icon

        {
            get { return SBIcon.Source; }
            set { SBIcon.Source = value; }
        }
        public string Label
        {
            get { return SBLabel.Text; }
            set { SBLabel.Text = value; }
        }
        public SpringBoardButton()
        {
            SBIcon = new Image();
            SBIcon.Source = "{Binding Icon}";
            SBIcon.HorizontalOptions = LayoutOptions.Center;

            SBLabel = new Label();
            SBLabel.Text = "{Binding Label}";
            SBLabel.TextColor = Color.Black;
            SBLabel.HorizontalOptions = LayoutOptions.Center;

            Content = new StackLayout
            {
                Spacing = 10,
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.White,
                Children = { SBIcon, SBLabel }
            };

        }
    }
}

