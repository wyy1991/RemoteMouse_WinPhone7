﻿#pragma checksum "E:\2012fall\ece454\windowsExample\Application Bar Sample\C#\sdkAppBarCS\PlainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DB633766F526B746B0ADB73AA6C8EA7A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace sdkAppBarCS {
    
    
    public partial class PlainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.ScrollViewer ContentPanel;
        
        internal System.Windows.Controls.RadioButton ForeNormal;
        
        internal System.Windows.Controls.RadioButton ForeAccent;
        
        internal System.Windows.Controls.RadioButton BackNormal;
        
        internal System.Windows.Controls.RadioButton BackAccent;
        
        internal System.Windows.Controls.RadioButton One;
        
        internal System.Windows.Controls.RadioButton Half;
        
        internal System.Windows.Controls.RadioButton Zero;
        
        internal System.Windows.Controls.RadioButton DefaultSize;
        
        internal System.Windows.Controls.RadioButton Mini;
        
        internal System.Windows.Controls.RadioButton Enabled;
        
        internal System.Windows.Controls.RadioButton Disabled;
        
        internal System.Windows.Controls.RadioButton Visible;
        
        internal System.Windows.Controls.RadioButton Hidden;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/sdkAppBarCS;component/PlainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.ScrollViewer)(this.FindName("ContentPanel")));
            this.ForeNormal = ((System.Windows.Controls.RadioButton)(this.FindName("ForeNormal")));
            this.ForeAccent = ((System.Windows.Controls.RadioButton)(this.FindName("ForeAccent")));
            this.BackNormal = ((System.Windows.Controls.RadioButton)(this.FindName("BackNormal")));
            this.BackAccent = ((System.Windows.Controls.RadioButton)(this.FindName("BackAccent")));
            this.One = ((System.Windows.Controls.RadioButton)(this.FindName("One")));
            this.Half = ((System.Windows.Controls.RadioButton)(this.FindName("Half")));
            this.Zero = ((System.Windows.Controls.RadioButton)(this.FindName("Zero")));
            this.DefaultSize = ((System.Windows.Controls.RadioButton)(this.FindName("DefaultSize")));
            this.Mini = ((System.Windows.Controls.RadioButton)(this.FindName("Mini")));
            this.Enabled = ((System.Windows.Controls.RadioButton)(this.FindName("Enabled")));
            this.Disabled = ((System.Windows.Controls.RadioButton)(this.FindName("Disabled")));
            this.Visible = ((System.Windows.Controls.RadioButton)(this.FindName("Visible")));
            this.Hidden = ((System.Windows.Controls.RadioButton)(this.FindName("Hidden")));
        }
    }
}

