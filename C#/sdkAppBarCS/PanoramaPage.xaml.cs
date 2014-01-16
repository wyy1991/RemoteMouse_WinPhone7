/* Yuye Wamg did the layout and integration */
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using Microsoft.Devices;
using System.Windows.Threading;

namespace sdkAppBarCS
{
    public partial class PanoramaPage : PhoneApplicationPage
    {

        public bool connected = false;
        public static String default_IP = "216.26.110.119";
        public static int default_Port = 13001;
        public static String connectIP = default_IP;
        public static int connectPort = 13001;
        bool canStart = false;

        DispatcherTimer timer;
        //Microsoft.Phone.Controls.Panorama MyPanorama = new Panorama();
        public PanoramaPage()
        {
            InitializeComponent();

            //Set the initial values for the Application Bar properties by checking the radio buttons.
            ApplicationBar.Opacity = 0.5;
            //Set the background image for the panorama page, depending on the current theme.
            //----------------------------------------------------------------------------------------
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("Images/background6.jpg", UriKind.Relative));
           
            //Microsoft.Phone.Controls.PanoramaItem;
            PanoControl.Background = imageBrush;
            PanoControl.DefaultItem = Item1;


            // Initialize the timer and add Tick event handler, but don't start it yet.
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick_check_connection);
            timer.Start();

            /*
            if (!connected)
            {
                button2DMouse.IsEnabled = false;
                button3DMouse.IsEnabled = false;
                buttonClicker.IsEnabled = false;
                buttonFoot.IsEnabled = false;
                buttonTouchPad.IsEnabled = false;

            }
            */

        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string strItemIndex;
            if (NavigationContext.QueryString.TryGetValue("goto", out strItemIndex)) {
                PanoControl.DefaultItem = PanoControl.Items[Convert.ToInt32(strItemIndex)];
            }
            base.OnNavigatedTo(e);
              
        }

        #region application bar
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
        #endregion
        #region buttons for mode

        private void buttonClicker_Click(object sender, RoutedEventArgs e)
        {
            if (!connected)
            {
                NavigationService.Navigate(new Uri("/PanoramaPage.xaml?goto=1", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/PageClicker.xaml", UriKind.Relative));
            }
            //NavigationService.Navigate(new Uri("/PageClicker.xaml", UriKind.Relative));
        }

        private void buttonFoot_Click(object sender, RoutedEventArgs e)
        {
            if (!connected)
            {
                NavigationService.Navigate(new Uri("/PanoramaPage.xaml?goto=1", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/PageLegControl.xaml", UriKind.Relative));
            }
            //NavigationService.Navigate(new Uri("/PageLegControl.xaml", UriKind.Relative));
        }

        private void button2DMouse_Click(object sender, RoutedEventArgs e)
        {
            if (!connected)
            {
                NavigationService.Navigate(new Uri("/PanoramaPage.xaml?goto=1", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/Page2DmouseControl.xaml", UriKind.Relative));
            }
            //NavigationService.Navigate(new Uri("/Page2Dmouse.xaml", UriKind.Relative));
        }

        private void button3DMouse_Click(object sender, RoutedEventArgs e)
        {
            if (!connected)
            {
                NavigationService.Navigate(new Uri("/PanoramaPage.xaml?goto=1", UriKind.Relative));
            }
            else 
            {
                NavigationService.Navigate(new Uri("/Page3Dmouse.xaml", UriKind.Relative));
            }
            
        }

        private void buttonTouchPad_Click(object sender, RoutedEventArgs e)
        {
            if (!connected)
            {
                NavigationService.Navigate(new Uri("/PanoramaPage.xaml?goto=1", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/TouchPad.xaml", UriKind.Relative));
            }
            //NavigationService.Navigate(new Uri("/TouchPad.xaml", UriKind.Relative));
        }

        #endregion 

        public void timer_Tick_check_connection(object sender, EventArgs e) 
        {
            if (canStart == true) 
            {
                AsynchronousClient client = new AsynchronousClient((Application.Current as App).HostName, 13001);
                client.ResponseReceived += new ResponseReceivedEventHandler(receive_Data);
                String stringToSend = ("0" + "|" + "0");
                client.SendData(stringToSend);
            }
            
        }

        void receive_Data(object sender, ResponseReceivedEventArgs e)
        {
            //receive data from the server, display it
            txtIPmsg.Text = e.response;
            int resp = Convert.ToInt32("8888");

            if (resp == 8888)
            {
                //VibrateController vibrate = VibrateController.Default;
                //vibrate.Start(TimeSpan.FromMilliseconds(30));
                buttonConnect.Content = "GOOD";
                if (connected == false) 
                {
                    setConnected(true);
                }
            }
            else 
            {
                buttonConnect.Content = "FAIL";
                if (connected == true) 
                {
                    setConnected(false);
                }
                
            }
            if (connected)
            {
                button2DMouse.IsEnabled = true;
                button3DMouse.IsEnabled = true;
                buttonClicker.IsEnabled = true;
                buttonFoot.IsEnabled = true;
                buttonTouchPad.IsEnabled = true;

            }
            
        }

        private void setConnected(bool con) {
            connected = con;
        }
        //
        private void buttonConnect_Click(object sender, RoutedEventArgs e)
        {
            if (canStart == true)
            {
                    canStart = false;
                    buttonConnect.Content = "CONNECTED";
               
            }
            else 
            {
                #region initialize server name and port number

                if (String.IsNullOrWhiteSpace(textBoxIP.Text))
                {
                    //txtIPmsg.Text = "Please fill in IP address.";

                    NavigationService.Navigate(new Uri("/PanoramaPage.xaml?goto=1", UriKind.Relative));
                    //default IP
                    (Application.Current as App).HostName = default_IP;
                }
                else
                {
                    (Application.Current as App).HostName = textBoxIP.Text;
                    connectIP = (Application.Current as App).HostName;
                }

                (Application.Current as App).PortNumber = Convert.ToInt32("13001");
                connectPort = (Application.Current as App).PortNumber;


                #endregion

                #region use the button to control start and pause


                //let the connection start
                if (canStart == false)
                {
                    canStart = true;
                    //buttonConnect.Content = "Pause";
                }

                #endregion
            }
          

        }
        

    }//page class
}//namespace
