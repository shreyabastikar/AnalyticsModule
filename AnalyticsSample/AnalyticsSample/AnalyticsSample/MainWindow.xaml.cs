using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using mcMath;
namespace AnalyticsSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void baseWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
            }
        }

        int leftSessionID = 0;
        bool left = false;
        bool right = false;
        LogRecord obj;
        AnalyticsLog clsobj = new AnalyticsLog();
        private void butLeft_Click(object sender, RoutedEventArgs e)
        {
            grdLeft.Visibility = Visibility.Visible;

            long lRes = clsobj.Add(23, 40);
            clsobj.Extra = false;

            Console.WriteLine(lRes.ToString());
            left = true;
            obj = clsobj.PostSessionStart(1, butLeft.Name, "table", DateTime.Now);


            // Register a selection of this item and save a session token 
            // so that we can track duration that the left panel is open.
            // Pseudo code:

            //leftSessionID = AnalyticsModule.PostSessionStart(butLeft.Name);

            // In this pseudo code, two things happen:
            // 1) The selection of an object is recorded in the analytics database.
            // 2) Also recorded at that moment is a begin time. 
            //A unique identifier is passed back so that when the panel closes, 
            // the analytics module can record the end time so that we get a duration.


            // Since we may not always want to get the duration, we could break this into two calls to the analytics module 
            // so that either of them may be optional:

            //AnalyticsModule.PostSelection("object", 419);
            //leftSessionID = AnalyticsModule.StartSession("object", 419);

            // The parameters "object" and 419 would be information that comes from a database.
        }

        private void butRight_Click(object sender, RoutedEventArgs e)
        {
            grdRight.Visibility = Visibility.Visible;
            right = true;
            // Pseudo code:
            //AnalyticsModule.PostSessionEnd(leftSessionID);
        }

        int rightSessionID;
        private void butLeftClose_Click(object sender, RoutedEventArgs e)
        {
            if (left)
            {
                left = false;


            }
            obj.EndTime = DateTime.Now;
            int ret = clsobj.PostSessionEnd(obj);
            grdLeft.Visibility = Visibility.Hidden;
            Console.WriteLine("***************** {0}", e.Source);
            // Pseudo code:
            //rightSessionID = AnalyticsModule.PostSessionStart(butRight.Name);
        }

        private void butRightClose_Click(object sender, RoutedEventArgs e)
        {
            right = false;
            grdRight.Visibility = Visibility.Hidden;

            // Pseudo code:
            //AnalyticsModule.PostSessionEnd(rightSessionID);
        }
    }
}
