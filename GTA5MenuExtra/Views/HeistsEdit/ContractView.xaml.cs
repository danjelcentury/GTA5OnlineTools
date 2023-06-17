﻿using GTA5Core.Features;

namespace GTA5MenuExtra.Views.HeistsEdit;

/// <summary>
/// ContractView.xaml 的交互逻辑
/// </summary>
public partial class ContractView : UserControl
{
    public ContractView()
    {
        InitializeComponent();
    }

    private async void WriteStatWithDelay(string hash, int value)
    {
        await Hacks.WriteIntStat(hash, value);
    }

    private void Button_FIXER_GENERAL_BS_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_FIXER_GENERAL_BS", -1);
        WriteStatWithDelay("_FIXER_STORY_BS", 4095);
    }

    private void Button_TUNER_CURRENT_Click(object sender, RoutedEventArgs e)
    {
        var index = ComboBox_TUNER_CURRENT.SelectedIndex;
        if (index != -1)
        {
            WriteStatWithDelay("_TUNER_CURRENT", index);
            WriteStatWithDelay("_TUNER_GEN_BS", 65535);
        }
    }
}
