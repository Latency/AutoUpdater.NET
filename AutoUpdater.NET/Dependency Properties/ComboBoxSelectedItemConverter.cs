// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ComboBoxSelectedItemConverter.cs
// Author:   Latency McLaughlin
// Date:     01/15/2026
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using AutoUpdaterDotNET.Enums;
using kvp = System.Collections.Generic.KeyValuePair<ushort, AutoUpdaterDotNET.Enums.RemindLaterFormat>;

namespace AutoUpdaterDotNET.Dependency_Properties;

public class ComboBoxSelectedItemConverter : BaseConverter
{
    #region Fields
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private readonly DependencyProperty _remindLaterAtProperty;
    private readonly DependencyProperty _remindLaterTimeSpanProperty;
    private readonly ComboBox?          _control;
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="window"></param>
    /// <param name="cb"></param>
    public ComboBoxSelectedItemConverter(Window window, ComboBox? cb) : base(window)
    {
        _control = cb;
        _remindLaterAtProperty = DependencyProperty.Register(nameof(RemindLaterAt),
                                                             typeof(int),
                                                             window.GetType(),
                                                             new FrameworkPropertyMetadata(OnRemindLaterAtChanged)
        );
        _remindLaterTimeSpanProperty = DependencyProperty.Register(nameof(RemindLaterTimeSpan),
                                                                   typeof(RemindLaterFormat),
                                                                   window.GetType(),
                                                                   new FrameworkPropertyMetadata(OnRemindLaterTimeSpanChanged)
        );
    }

    #region Properties
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    public int RemindLaterAt
    {
        get => (int) GetValue(_remindLaterAtProperty);
        set => SetValue(_remindLaterAtProperty, value);
    }

    public RemindLaterFormat RemindLaterTimeSpan
    {
        get => (RemindLaterFormat) GetValue(_remindLaterTimeSpanProperty);
        set => SetValue(_remindLaterTimeSpanProperty, value);
    }

    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Methods
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    private void OnRemindLaterAtChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (_control is null)
            return;

        var timeVal = Convert.ToUInt16(e.NewValue);
        var (remindAt, remindTimeSpan) = (kvp) _control.SelectedItem;

        switch (remindTimeSpan)
        {
            case RemindLaterFormat.Seconds:
                switch (timeVal)
                {
                    case <= 5 * 60:
                        remindAt       = 5;
                        remindTimeSpan = RemindLaterFormat.Minutes;
                        break;
                    case <= 10 * 60:
                        remindAt       = 10;
                        remindTimeSpan = RemindLaterFormat.Minutes;
                        break;
                    case <= 15 * 60:
                        remindAt       = 15;
                        remindTimeSpan = RemindLaterFormat.Minutes;
                        break;
                    case <= 30 * 60:
                        remindAt       = 30;
                        remindTimeSpan = RemindLaterFormat.Minutes;
                        break;
                    default:
                        remindAt       = 1;
                        remindTimeSpan = RemindLaterFormat.Hours;
                        break;
                }
                break;
            case RemindLaterFormat.Minutes:
                switch (timeVal)
                {
                    case <= 5:
                        remindAt = 5;
                        break;
                    case <= 10:
                        remindAt = 10;
                        break;
                    case <= 15:
                        remindAt = 15;
                        break;
                    case <= 30:
                        remindAt = 30;
                        break;
                    default:
                        remindAt       = 1;
                        remindTimeSpan = RemindLaterFormat.Hours;
                        break;
                }
                break;
            case RemindLaterFormat.Hours:
                switch (timeVal)
                {
                    case <= 6:
                        remindAt = 6;
                        break;
                    case <= 12:
                        remindAt = 12;
                        break;
                    default:
                        remindAt       = 1;
                        remindTimeSpan = RemindLaterFormat.Days;
                        break;
                }
                break;
            case RemindLaterFormat.Days:
                switch (timeVal)
                {
                    case <= 2:
                        break;
                    case <= 4:
                        remindAt = 4;
                        break;
                    default:
                        remindAt       = 1;
                        remindTimeSpan = RemindLaterFormat.Weeks;
                        break;
                }
                break;
            case RemindLaterFormat.Weeks:
                break;
            default:
                throw new ArgumentOutOfRangeException(MethodBase.GetCurrentMethod()?.Name);
        }

        _control.SelectedItem = new kvp(remindAt, remindTimeSpan);
    }


    private void OnRemindLaterTimeSpanChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ;
    }


    /// <summary>
    /// Converter
    /// </summary>
    /// <remarks>
    /// This delegate is not being used!
    /// </remarks>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    protected override object? Converter(object? value, Type targetType, object? parameter, CultureInfo culture) => null;


    /// <summary>
    /// SetBindings
    /// </summary>
    public override void SetBindings(IViewModelConfig vm)
    {
        Parent.SetBinding(_remindLaterAtProperty, new Binding
        {
            Source              = vm,
            Path                = new PropertyPath(nameof(vm.RemindLaterAt)),
            Mode                = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });
        Parent.SetBinding(_remindLaterTimeSpanProperty, new Binding
        {
            Source              = vm,
            Path                = new PropertyPath(nameof(vm.RemindLaterTimeSpan)),
            Mode                = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });
    }

    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}