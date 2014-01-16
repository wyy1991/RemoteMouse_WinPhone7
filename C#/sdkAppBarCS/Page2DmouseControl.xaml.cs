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
    public partial class Page2DmouseControl : PhoneApplicationPage
    {
        Motion motion;
  
        bool canStart = false;

        //Double[] attitude = new double[3];
        Vector3 acceleration;
        /*
        bool clickSent = true;
        String default_IP = "216.26.110.119";

        int default_Port = 13001;
        */
        int controlClick = 0;

        public Page2DmouseControl()
        {
            InitializeComponent();

            motion = new Motion();

            // Specify the desired time between updates. The sensor accepts
            // intervals in multiples of 1s.            
            motion.TimeBetweenUpdates = TimeSpan.FromMilliseconds(30);

            motion.CurrentValueChanged += new
            EventHandler<SensorReadingEventArgs<MotionReading>>(motion_CurrentValueChanged);

            motion.Start();

            (Application.Current as App).HostName = sdkAppBarCS.PanoramaPage.connectIP;

            (Application.Current as App).PortNumber = sdkAppBarCS.PanoramaPage.connectPort;
        }

        void motion_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
        {
            if (motion.IsDataValid)
            {
                acceleration = e.SensorReading.DeviceAcceleration;
            }
        }
   
        #region send data receive feedback

        void receive_Data(object sender, ResponseReceivedEventArgs e)
        {
            if (motion.IsDataValid & canStart)
            {
                AsynchronousClient client = new AsynchronousClient((Application.Current as App).HostName, 13001);
                client.ResponseReceived += new ResponseReceivedEventHandler(receive_Data);
                String stringToSend = ("2" + "|" + acceleration.X.ToString("0.00")
                    + "|" + acceleration.Y.ToString("0.00")
                    + "|" + controlClick.ToString("0"));

                client.SendData(stringToSend);
                controlClick = 0;
            }

            if (e.response == "1")
            {
                VibrateController vibrate = VibrateController.Default;
                vibrate.Start(TimeSpan.FromMilliseconds(30));
            }
        }

        #endregion

        private void left_Button_Click(object sender, RoutedEventArgs e)
        {
            controlClick = 1;
        }

        private void right_Button_Click(object sender, RoutedEventArgs e)
        {
            controlClick = 2;
        }

        #region application bar
        private void Button1_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PanoramaPage.xaml?goto=2", UriKind.Relative));
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //let the connection start
            if (canStart == false)
            {
                canStart = true;
            }
            else
            {
                canStart = false;
                MessageBox.Show("Pause! ");
            }
            //MessageBox.Show("Button play works! ");
            #region transmit data for the first time

            AsynchronousClient client = new AsynchronousClient((Application.Current as App).HostName, 13001);
            client.ResponseReceived += new ResponseReceivedEventHandler(receive_Data);
            String stringToSend = ("2" + "|" + acceleration.X.ToString("0.00")
                + "|" + acceleration.Y.ToString("0.00")
                + "|" + controlClick.ToString("0"));

            client.SendData(stringToSend);
            controlClick = 0;

            #endregion
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PanoramaPage.xaml?goto=1", UriKind.Relative));
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
        }
        #endregion

    }
}