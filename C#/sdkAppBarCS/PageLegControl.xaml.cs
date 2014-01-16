/* Xiufeng Xie did the function implementation of 3D mouse */
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
//using System.Windows.DependencyObject;



namespace sdkAppBarCS
{
    public partial class PageLegControl : PhoneApplicationPage
    {
        Motion motion;

        Double[] attitude = new double[3];
        Vector3 acceleration;
        bool canStart = false;
        String default_IP = "192.168.16.2";

        int default_Port = 13001; 

        public PageLegControl()
        {
            InitializeComponent();

            motion = new Motion();

            // Specify the desired time between updates. The sensor accepts
            // intervals in multiples of 1s.            
            motion.TimeBetweenUpdates = TimeSpan.FromMilliseconds(45);


            motion.CurrentValueChanged += new
            EventHandler<SensorReadingEventArgs<MotionReading>>(motion_CurrentValueChanged);

            motion.Start();

            (Application.Current as App).HostName = sdkAppBarCS.PanoramaPage.connectIP;

            (Application.Current as App).PortNumber = sdkAppBarCS.PanoramaPage.connectPort;       

        }



        #region Update sensor data

        void motion_CurrentValueChanged(object sender,
        SensorReadingEventArgs<MotionReading> e)
        {
            if (motion.IsDataValid)
            {
                attitude[0] = MathHelper.ToDegrees(e.SensorReading.Attitude.Yaw);
                attitude[1] = MathHelper.ToDegrees(e.SensorReading.Attitude.Pitch);
                attitude[2] = MathHelper.ToDegrees(e.SensorReading.Attitude.Roll);
                acceleration = e.SensorReading.DeviceAcceleration;
            }
        }

        #endregion

        #region send data receive feedback

        void receive_Data(object sender, ResponseReceivedEventArgs e)
        {
           if (motion.IsDataValid & canStart)
            {
                AsynchronousClient client = new AsynchronousClient((Application.Current as App).HostName, 13001);
                client.ResponseReceived += new ResponseReceivedEventHandler(receive_Data);
                String stringToSend = ("4" + "|" + attitude[0].ToString("0.0") + "|"
                    + attitude[1].ToString("0.0") + "|" + attitude[2].ToString("0.0")
                    + "|" + acceleration.X.ToString("0.0")
                    + "|" + acceleration.Y.ToString("0.0")
                    + "|" + acceleration.Z.ToString("0.0"));

                client.SendData(stringToSend);

            }

            if (e.response == "1")
            {
                VibrateController vibrate = VibrateController.Default;
                vibrate.Start(TimeSpan.FromMilliseconds(30));
            }
        }

        #endregion

        private void button3Dstart_Click(object sender, RoutedEventArgs e)
        {
            #region initialize server name and port number

            (Application.Current as App).HostName = sdkAppBarCS.PanoramaPage.connectIP;
            (Application.Current as App).PortNumber = sdkAppBarCS.PanoramaPage.connectPort;
          
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
            #region use the button to control start and pause

            //let the connection start
            if (canStart == false)
            {
                canStart = true;
            }
            else
            {
                canStart = false;
                MessageBox.Show("Paused! ");
            }
            #endregion

            #region transmit data for the first time

            AsynchronousClient client = new AsynchronousClient((Application.Current as App).HostName, 13001);
            client.ResponseReceived += new ResponseReceivedEventHandler(receive_Data);
            String stringToSend = ("4" + "|" + attitude[0].ToString("0.0") + "|"
                    + attitude[1].ToString("0.0") + "|" + attitude[2].ToString("0.0")
                    + "|" + acceleration.X.ToString("0.0")
                    + "|" + acceleration.Y.ToString("0.0")
                    + "|" + acceleration.Z.ToString("0.0"));

            client.SendData(stringToSend);

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

    }
}