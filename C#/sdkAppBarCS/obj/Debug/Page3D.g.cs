﻿#pragma checksum "E:\2012fall\ece454\RmouseUI1\C#\sdkAppBarCS\Page3D.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C5E74EE4326A46BBB0DAE37D89E9BAF0"
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
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox txtServerName;
        
        internal System.Windows.Controls.TextBox txtPortNumber;
        
        internal System.Windows.Controls.Button button1;
        
        internal System.Windows.Controls.TextBlock textBlock1;
        
        internal System.Windows.Controls.TextBlock textBlock_IP;
        
        internal System.Windows.Controls.TextBlock textBlock2;
        
        internal System.Windows.Controls.TextBlock textBlock_Port;
        
        internal System.Windows.Controls.Button button2;
        
        internal System.Windows.Controls.Button button3;
        
        internal System.Windows.Controls.TextBlock textBlock_Test;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/sdkAppBarCS;component/Page3D.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.txtServerName = ((System.Windows.Controls.TextBox)(this.FindName("txtServerName")));
            this.txtPortNumber = ((System.Windows.Controls.TextBox)(this.FindName("txtPortNumber")));
            this.button1 = ((System.Windows.Controls.Button)(this.FindName("button1")));
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock1")));
            this.textBlock_IP = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock_IP")));
            this.textBlock2 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock2")));
            this.textBlock_Port = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock_Port")));
            this.button2 = ((System.Windows.Controls.Button)(this.FindName("button2")));
            this.button3 = ((System.Windows.Controls.Button)(this.FindName("button3")));
            this.textBlock_Test = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock_Test")));
        }
    }
}

