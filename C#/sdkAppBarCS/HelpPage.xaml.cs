/* Yuye Wamg did the layout and integration */
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace sdkAppBarCS
{
    public partial class PivotPage : PhoneApplicationPage
    {
        public PivotPage()
        {
            InitializeComponent();

            //Set the initial values for the Application Bar properties by checking the radio buttons.
           
        }



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

        
    }//page class
}//namespace
