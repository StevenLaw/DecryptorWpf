using Decryptor.Utilities;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Decryptor.UserControls;

/// <summary>
/// Interaction logic for ViewablePasswordBox.xaml
/// </summary>
public partial class ViewablePasswordBox : UserControl
{

    public SecureString Password
    {
        get { return (SecureString)GetValue(PasswordProperty); }
        set { SetValue(PasswordProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register("Password",
                                    typeof(SecureString),
                                    typeof(ViewablePasswordBox),
                                    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnPropertyChangedSuccess)));

    public int PasswordLength
    {
        get { return (int)GetValue(PasswordLengthProperty); }
        set { SetValue(PasswordLengthProperty, value); }
    }

    // Using a DependencyProperty as the backing store for PasswordLength.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PasswordLengthProperty =
        DependencyProperty.Register("PasswordLength", typeof(int), typeof(ViewablePasswordBox), new PropertyMetadata(0));



    public static void OnPropertyChangedSuccess(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is ViewablePasswordBox vpb && e.NewValue is SecureString secure)
        {
            vpb.pwd.Password = secure.ToInsecureString();
            if (vpb.txt.Visibility == Visibility.Visible)
                vpb.txt.Text = secure.ToInsecureString();
        }
    }

    public ViewablePasswordBox()
    {
        InitializeComponent();
        pwd.Password = Password.ToInsecureString();
    }

    public void ShowPassword(bool show)
    {
        if (show)
        {
            pwd.Visibility = Visibility.Collapsed;
            txt.Visibility = Visibility.Visible;
            txt.Text = Password.ToInsecureString();
        }
        else
        {
            pwd.Visibility = Visibility.Visible;
            txt.Visibility = Visibility.Collapsed;
            txt.Text = string.Empty;
        }
    }

    private void Pwd_LostFocus(object sender, RoutedEventArgs e)
    {
        UpdatePassword(sender);
    }

    private void Pwd_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter || e.Key == Key.Return)
            UpdatePassword(sender);

        switch (sender)
        {
            case PasswordBox pwd:
                PasswordLength = pwd.SecurePassword.Length;
                break;
            case TextBox txt:
                PasswordLength = txt.Text.Length;
                break;
        }
    }

    private void UpdatePassword(object sender)
    {
        switch (sender)
        {
            case PasswordBox pwd:
                Password = pwd.SecurePassword;
                break;
            case TextBox txt:
                Password = txt.Text.ToSecureString();
                break;
        }
    }
}
