﻿using System.Windows;
using System.Windows.Controls;

namespace WinVerMediaChecker;

/// <summary>
/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
///
/// Step 1a) Using this custom control in a XAML file that exists in the current project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:WinVerMediaChecker"
///
///
/// Step 1b) Using this custom control in a XAML file that exists in a different project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:WinVerMediaChecker;assembly=WinVerMediaChecker"
///
/// You will also need to add a project reference from the project where the XAML file lives
/// to this project and Rebuild to avoid compilation errors:
///
///     Right click on the target project in the Solution Explorer and
///     "Add Reference"->"Projects"->[Browse to and select this project]
///
///
/// Step 2)
/// Go ahead and use your control in the XAML file.
///
///     <MyNamespace:CustomButton/>
///
/// </summary>
public class CustomButton : Button
{
    #region Public Constructors

    static CustomButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomButton), new FrameworkPropertyMetadata(typeof(CustomButton)));
    }

    #endregion Public Constructors

    #region Public Fields

    // Using a DependencyProperty as the backing store for ButtonLabel.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ButtonLabelProperty =
        DependencyProperty.Register("ButtonLabel", typeof(string), typeof(CustomButton), new PropertyMetadata(string.Empty));

    #endregion Public Fields

    #region Public Properties

    public string ButtonLabel
    {
        get { return (string)GetValue(ButtonLabelProperty); }
        set { SetValue(ButtonLabelProperty, value); }
    }

    #endregion Public Properties
}