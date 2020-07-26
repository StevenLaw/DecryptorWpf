using System.Windows;
using System.Windows.Controls;

namespace Decryptor.UserControls
{
    /// <summary>
    /// Interaction logic for Spinner.xaml
    /// </summary>
    public partial class Spinner : UserControl
    {


        public bool IsSpinning
        {
            get { return (bool)GetValue(IsSpinningProperty); }
            set { SetValue(IsSpinningProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSpinning.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSpinningProperty =
            DependencyProperty.Register("IsSpinning", typeof(bool), typeof(Spinner), new PropertyMetadata(false, new PropertyChangedCallback(OnPropertyChangedSuccess)));

        public static void OnPropertyChangedSuccess(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Spinner spinner && e.NewValue is bool boolean)
            {
                spinner.Visibility = boolean ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Spinner()
        {
            InitializeComponent();
            //Spinner starts as not visible
            Visibility = Visibility.Collapsed;
        }
    }
}
