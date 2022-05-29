using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Decryptor.WinUI.UserControls
{
    public sealed partial class SuccessIndicator : UserControl
    {
        public static readonly DependencyProperty SuccessProperty =
            DependencyProperty.Register(nameof(Success), typeof(bool?), typeof(SuccessIndicator), new PropertyMetadata(null,
                new PropertyChangedCallback(OnSuccessPropertyChanged)));

        public SuccessIndicator()
        {
            InitializeComponent();
        }

        public bool? Success 
        { 
            get => (bool?)GetValue(SuccessProperty); 
            set => SetValue(SuccessProperty, value); 
        }

        private static void OnSuccessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SuccessIndicator successIndicator)
            {
                bool? success = e.NewValue as bool?;
                successIndicator.SetIndicator(success);
            }
        }

        private void SetIndicator(bool? success)
        {
            switch (success)
            {
                case true:
                    fiFailure.Visibility = Visibility.Collapsed;
                    fiSuccess.Visibility = Visibility.Visible;
                    break;

                case false:
                    fiFailure.Visibility = Visibility.Visible;
                    fiSuccess.Visibility = Visibility.Collapsed;
                    break;

                case null:
                default:
                    fiFailure.Visibility = Visibility.Collapsed;
                    fiSuccess.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}