using System.Collections.Generic;
using System.Windows.Controls;

namespace Decryptor.View.SettingsViews;

/// <summary>
/// Interaction logic for ScryptView.xaml
/// </summary>
public partial class ScryptView : UserControl
{
    public ScryptView()
    {
        InitializeComponent();
        SetupIterationComboBox();
    }
    private void SetupIterationComboBox()
    {

        var iterationValues = new List<int>();
        int i = 2;
        while (i > 0)
        {
            iterationValues.Add(i);
            i *= 2;
        }
        cmbIterations.ItemsSource = iterationValues;
    }
}
