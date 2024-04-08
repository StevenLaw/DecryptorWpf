using System.Windows;
using System.Windows.Controls;

namespace Decryptor.UserControls;

/// <summary>
/// Interaction logic for SuccessIndicator.xaml
/// </summary>
public partial class SuccessIndicator : UserControl
{

    public bool? Success
    {
        get { return (bool?)GetValue(SuccessProperty); }
        set { SetValue(SuccessProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Success.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SuccessProperty =
        DependencyProperty.Register("Success",
                                    typeof(bool?),
                                    typeof(SuccessIndicator),
                                    new PropertyMetadata(null, new PropertyChangedCallback(OnSuccessPropertyChanged)));

    private static void OnSuccessPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is SuccessIndicator si)
        {
            bool? success = e.NewValue as bool?;
            si.SetIndicator(success);
        }
    }

    private void SetIndicator(bool? success)
    {
        if (success == null)
        {
            imgFail.Visibility = Visibility.Collapsed;
            imgSuccess.Visibility = Visibility.Collapsed;
        }
        else if (success == true)
        {
            imgFail.Visibility = Visibility.Collapsed;
            imgSuccess.Visibility = Visibility.Visible;
        }
        else if (success == false)
        {
            imgFail.Visibility = Visibility.Visible;
            imgSuccess.Visibility = Visibility.Collapsed;
        }
    }

    public SuccessIndicator()
    {
        InitializeComponent();
        SetIndicator(Success);
    }
}
