﻿using GTA5Core.Features;
using GTA5Shared.Helper;

namespace GTA5MenuExtra.Views.HeistsEdit;

/// <summary>
/// DoomsdayView.xaml 的交互逻辑
/// </summary>
public partial class DoomsdayView : UserControl
{
    public DoomsdayView()
    {
        InitializeComponent();
    }

    #region 前置任务
    private async void WriteStatWithDelay(string hash, int value)
    {
        await Hacks.WriteIntStat(hash, value);
    }

    ////////////////////////////////////////////////////
    
    private void Button_GANGOPS_FLOW_MISSION_PROG_1_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_GANGOPS_FLOW_MISSION_PROG", -1);
    }

    private void Button_GANGOPS_FLOW_MISSION_PROG_503_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_GANGOPS_FLOW_MISSION_PROG", 503);
        WriteStatWithDelay("_GANGOPS_HEIST_STATUS", 819193);
        WriteStatWithDelay("_GANGOPS_FLOW_NOTIFICATIONS", 1557);
    }

    private void Button_GANGOPS_FLOW_MISSION_PROG_240_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_GANGOPS_FLOW_MISSION_PROG", 240);
        WriteStatWithDelay("_GANGOPS_HEIST_STATUS", 819198);
        WriteStatWithDelay("_GANGOPS_FLOW_NOTIFICATIONS", 1557);
    }

    private void Button_GANGOPS_FLOW_MISSION_PROG_16368_Click(object sender, RoutedEventArgs e)
    {
        WriteStatWithDelay("_GANGOPS_FLOW_MISSION_PROG", 16368);
        WriteStatWithDelay("_GANGOPS_HEIST_STATUS", 819190);
        WriteStatWithDelay("_GANGOPS_FLOW_NOTIFICATIONS", 1557);
    }
    #endregion
}
