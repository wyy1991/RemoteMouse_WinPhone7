/* Jack Chiu did the function implementation of 2D mouse */
/* Yuye Wamg did the layout and integration */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;

namespace sdkAppBarCS
{
    public partial class Page2Dmouse : PhoneApplicationPage
    {
        /*
        Motion motion;
        DispatcherTimer timer;

        Double[] attitude = new double[3];
        Vector3 acceleration;*/
        bool canStart = false;
        String default_IP = sdkAppBarCS.PanoramaPage.default_IP;

        int default_Port = sdkAppBarCS.PanoramaPage.default_Port;

        public Page2Dmouse()
        {
            InitializeComponent();

            textBlock_IP.Text = default_IP;

            textBlock_Port.Text = default_Port.ToString();
        }

        private void start_Button_Click(object sender, RoutedEventArgs e)
        {
            #region initialize server name and port number

            if (String.IsNullOrWhiteSpace(txtServerName.Text))
            {
                //default IP
                (Application.Current as App).HostName = default_IP;
            }
            else
            {
                (Application.Current as App).HostName = txtServerName.Text;
            }

            if (String.IsNullOrWhiteSpace(txtPortNumber.Text))
            {
                //default Port Number
                (Application.Current as App).PortNumber = default_Port;
            }
            else
            {
                (Application.Current as App).PortNumber = Convert.ToInt32(txtPortNumber.Text);
            }

            #endregion

            #region use the button to control start and pause

            //let the connection start
            if (canStart == false)
            {
                canStart = true;
                NavigationService.Navigate(new Uri("/Page2DmouseControl.xaml", UriKind.Relative));
            }
            else
            {
                canStart = false;
                button1.Content = "Start";
            }

            #endregion

        }

        //**********************************************************
        // Below is page navigation

        private void Button1_Click(object sender, EventArgs e)
        {
            
            NavigationService.Navigate(new Uri("/PanoramaPage.xaml?goto=2", UriKind.Relative));
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button play works! ");
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PanoramaPage.xaml?goto=1", UriKind.Relative));
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
        }
    }
}