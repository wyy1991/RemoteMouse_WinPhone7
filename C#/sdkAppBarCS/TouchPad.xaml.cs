using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;
using System.Windows.Threading;
using Microsoft.Devices;

namespace sdkAppBarCS
{
    public partial class TouchPad : PhoneApplicationPage
    {
        int controlclick;
        double xcurrpos;
        double ycurrpos;
        double xlastpos;
        double ylastpos;
        int touchmode;

        String defaultIP = sdkAppBarCS.PanoramaPage.default_IP;   
        
        bool canStart = false;

        //preXArray and preYArray are used to store the start point 
        //for each touch point. currently silverlight support 4 muliti-touch
        //here declare as 4 points for further needs. 
        double[] preXArray = new double[4];
        double[] preYArray = new double[4];

        public TouchPad()
        {
            InitializeComponent();         

            Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);

            (Application.Current as App).HostName = sdkAppBarCS.PanoramaPage.connectIP;

            (Application.Current as App).PortNumber = sdkAppBarCS.PanoramaPage.connectPort;
        }



        /// <summary>
        /// Every touch action will rise this event handler. 
        /// </summary>
        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            int pointsNumber = e.GetTouchPoints(drawCanvas).Count;
            TouchPointCollection pointCollection = e.GetTouchPoints(drawCanvas);
            //TouchPoint ppoint = e.GetPrimaryTouchPoint(null);
           

            for (int i = 0; i < pointsNumber; i++)
            {
                if (pointCollection[i].Action == TouchAction.Down)
                {
                    touchmode = 1;
                    preXArray[i] = pointCollection[i].Position.X;
                    preYArray[i] = pointCollection[i].Position.Y;
                    textBlock1.Text = "down";
                }
                if ((pointCollection[i].Action == TouchAction.Move)&&(pointsNumber == 1))
                {
                    touchmode = 2;
                    Line line = new Line();

                    line.X1 = preXArray[i];
                    line.Y1 = preYArray[i];
                    xlastpos = preXArray[i];
                    ylastpos = preYArray[i];

                    line.X2 = pointCollection[i].Position.X;
                    line.Y2 = pointCollection[i].Position.Y;
                    xcurrpos = pointCollection[i].Position.X;
                    ycurrpos = pointCollection[i].Position.Y;

                    line.Stroke = new SolidColorBrush(Colors.Black);
                    line.Fill = new SolidColorBrush(Colors.Black);
                    drawCanvas.Children.Add(line);

                    preXArray[i] = pointCollection[i].Position.X;
                    preYArray[i] = pointCollection[i].Position.Y;

                    textBlock1.Text = "move";
                    //textBlock1.Text = "curr:" + xcurrpos.ToString() + "    " +  ycurrpos.ToString() + "\n last: "
                    //    +xlastpos.ToString()+ "    "+ylastpos.ToString() ;
                }
                if (pointCollection[i].Action == TouchAction.Up)
                {
                    touchmode = 3;
                    xlastpos = xcurrpos;
                    ylastpos = ycurrpos;
                    textBlock1.Text = "up";
                }
            }
        }
        
        #region send data receive feedback

        void receive_Data(object sender, ResponseReceivedEventArgs e)
        {
            if (canStart)
            {
                AsynchronousClient client = new AsynchronousClient((Application.Current as App).HostName, 13001);
                client.ResponseReceived += new ResponseReceivedEventHandler(receive_Data);
                String stringToSend = ("5" + "|" + controlclick.ToString()
                    + "|" + xcurrpos.ToString("0.00")
                    + "|" + ycurrpos.ToString("0.00")
                    + "|" + xlastpos.ToString("0.00")
                    + "|" + ylastpos.ToString("0.00"));

                client.SendData(stringToSend);
                //reset after button command is sent
                controlclick = 0;
            }
            if (e.response == "1")
            {
                VibrateController vibrate = VibrateController.Default;
                vibrate.Start(TimeSpan.FromMilliseconds(30));
            }
        }

        #endregion
       

        private void button_leftclick_Click(object sender, RoutedEventArgs e)
        {
            controlclick = 1;
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(30));
        }

        private void button_rightclick_Click(object sender, RoutedEventArgs e)
        {
            controlclick = 2;
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(30));
        }

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
            String stringToSend = ("5" + "|" + controlclick.ToString()
                + "|" + xcurrpos.ToString("0.00")
                + "|" + ycurrpos.ToString("0.00")
                + "|" + xlastpos.ToString("0.00")
                + "|" + ylastpos.ToString("0.00"));

            client.SendData(stringToSend);
            //reset after button command is sent
            controlclick = 0;

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