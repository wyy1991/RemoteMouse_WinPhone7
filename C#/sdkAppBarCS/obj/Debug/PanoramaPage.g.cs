﻿#pragma checksum "C:\Users\Chun\Desktop\RmouseV14_combined\C#\sdkAppBarCS\PanoramaPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C5D280C510B3E0FC20F907C74FF515A0"
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.296
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
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
    
    
    public partial class PanoramaPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Panorama PanoControl;
        
        internal Microsoft.Phone.Controls.PanoramaItem Item1;
        
        internal System.Windows.Controls.ScrollViewer ContentPanel;
        
        internal Microsoft.Phone.Controls.PanoramaItem Item2;
        
        internal System.Windows.Controls.TextBlock txtIPmsg;
        
        internal System.Windows.Controls.TextBox textBoxIP;
        
        internal System.Windows.Controls.Button buttonConnect;
        
        internal Microsoft.Phone.Controls.PanoramaItem Item3;
        
        internal System.Windows.Controls.Button buttonClicker;
        
        internal System.Windows.Controls.Button button2DMouse;
        
        internal System.Windows.Controls.Button button3DMouse;
        
        internal System.Windows.Controls.Button buttonFoot;
        
        internal System.Windows.Controls.Button buttonTouchPad;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/sdkAppBarCS;component/PanoramaPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.PanoControl = ((Microsoft.Phone.Controls.Panorama)(this.FindName("PanoControl")));
            this.Item1 = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("Item1")));
            this.ContentPanel = ((System.Windows.Controls.ScrollViewer)(this.FindName("ContentPanel")));
            this.Item2 = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("Item2")));
            this.txtIPmsg = ((System.Windows.Controls.TextBlock)(this.FindName("txtIPmsg")));
            this.textBoxIP = ((System.Windows.Controls.TextBox)(this.FindName("textBoxIP")));
            this.buttonConnect = ((System.Windows.Controls.Button)(this.FindName("buttonConnect")));
            this.Item3 = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("Item3")));
            this.buttonClicker = ((System.Windows.Controls.Button)(this.FindName("buttonClicker")));
            this.button2DMouse = ((System.Windows.Controls.Button)(this.FindName("button2DMouse")));
            this.button3DMouse = ((System.Windows.Controls.Button)(this.FindName("button3DMouse")));
            this.buttonFoot = ((System.Windows.Controls.Button)(this.FindName("buttonFoot")));
            this.buttonTouchPad = ((System.Windows.Controls.Button)(this.FindName("buttonTouchPad")));
        }
    }
}

