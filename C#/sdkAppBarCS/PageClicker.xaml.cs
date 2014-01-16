/* Chun Wang did the function implementation of PPT Clicker */
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
    public partial class Page1Clicker : PhoneApplicationPage
    {
        Motion motion;
      
        Double[] attitude = new double[3];
        Vector3 acceleration;

        String default_IP = "192.168.0.101"; //home testing
        //String default_IP = "72.33.244.186"; //Engineering hall testing

        int default_Port = 13001;

        bool canStart = false;
        //1 = next ppt, 2 = prev ppt, 3 = start ppt, 4 = end ppt
        int control = 0;

        public Page1Clicker()
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

        void receive_Data(object sender, ResponseReceivedEventArgs e){
            if (motion.IsDataValid & canStart) 
            {
                AsynchronousClient client = new AsynchronousClient((Application.Current as App).HostName, 13001);
                client.ResponseReceived += new ResponseReceivedEventHandler(receive_Data);
                String stringToSend = ("1" + "|" + attitude[1].ToString("0.00") + "|"
                    + attitude[2].ToString("0.00") 
                    + "|" + control.ToString("0"));

                client.SendData(stringToSend);
                //reset after button command is sent
                control = 0;
            }

            if (e.response == "1")
            {
                VibrateController vibrate = VibrateController.Default;
                vibrate.Start(TimeSpan.FromMilliseconds(30));
            }
        }

        #endregion


        private void previous_btn_Click(object sender, RoutedEventArgs e)
        {
            control = 2;
            textBlock_Status.Text = "Previous PPT";
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(30));
        }

        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            control = 1;
            textBlock_Status.Text = "Next PPT";
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(30));
        }




        //**********************************************************
        // Below is page navigation
        #region application bar
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
                //status_btn.Content = "Pause";
            }
            else
            {
                canStart = false;
                MessageBox.Show("Paused! ");
                //status_btn.Content = "Start";
            }
            #endregion

            #region transmit data for the first time
                      
            AsynchronousClient client = new AsynchronousClient((Application.Current as App).HostName, 13001);
            client.ResponseReceived += new ResponseReceivedEventHandler(receive_Data);
            String stringToSend = ("1" + "|" + attitude[1].ToString("0.00") + "|"
              + attitude[2].ToString("0.00")
              + "|" + control.ToString("0"));
            
            client.SendData(stringToSend);
            //reset after button command is sent
            control = 0;
            
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