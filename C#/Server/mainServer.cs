/* The Server side for all remote mouse functions
 * Author: Xiufeng Xie.
 * Modified: Chun Wang, Jack Chiu, Yuye Wang.
 */


using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;


namespace xxf_Server
{
    public class mainServer:System.Windows.Forms.Form
    {

        #region learning algorithm parameter

        public static bool learning = false;
        public static bool doOnce = false;
        public static double x_Left;
        public static double x_Right;
        public static double y_Upper;
        public static double y_Lower;
        public static double x_Acc_min;
        public static double x_Acc_max;
        public static double y_Acc_min;
        public static double y_Acc_max;

        #endregion

        #region allocate a console
        
        // Allocates a new console for current process.        
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole(); 

        #endregion

        #region 2D mouse special variables
        public static double x_Speed = 0;
        public static double y_Speed = 0;
        public static int current_X = Screen.PrimaryScreen.Bounds.Width;
        public static int current_Y = Screen.PrimaryScreen.Bounds.Height;
        //public static int xSpeed = 0;
        //public static int ySpeed = 0;
        public static int resetRound = 0;
        #endregion

        #region mouse click parameter

        public static int click_timer_left = 0;

        public static int click_timer_right = 0;

        public static bool mClicked_left = false;

        public static bool mClicked_right = false;

        public static int last_X = 0;

        public static int last_Y = 0;

        #endregion

        #region brake system parameter

        public static bool cursor_brake_X = false;

        public static bool cursor_brake_Y = false;

        #endregion

        #region mouse control parameters

        const int MOUSEEVENTF_LEFTDOWN = 0x2;
        const int MOUSEEVENTF_LEFTUP = 0x4;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        const int MOUSEEVENTF_MIDDLEUP = 0x40;
        const int MOUSEEVENTF_MOVE = 0x1;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        const int MOUSEEVENTF_RIGHTUP = 0x10;
        private Button button1;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button2;
        const int MOUSEEVENTF_WHEEL = 0x800;

        //mouse location        
        public struct PONITAPI
        {
            public int x, y;
        }

        [DllImport("user32.dll")]
        public static extern int GetCursorPos(ref PONITAPI p);

        [DllImport("user32.dll")]
        public static extern int SetCursorPos(int x, int y);      
        

        [DllImport("user32.dll")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);       
    
        #endregion        
       
        #region keyboard control parameters

        [DllImport("user32.dll")]
        static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        #endregion

        #region low pass filter related paramters

        //first order low-pass filter
        static double out_Yaw;
        static double out_Pitch;
        static double out_Roll;

        //used to store the output of the last time, used by the LPF
        public static double[] last_Output = { 0, 0, 0 };

        #endregion

        #region touch pad parameters
        public static int padcontrolclick = 0;
        public static double padXcurpos = 0;
        public static double padYcurpos = 0;
        public static double padXlastpos = 0;
        public static double padYlastpos = 0;
        public static int padXcursor = 0;
        public static int padYcursor = 0;

        #endregion

        static Int32 portNum = 13001;        

        //thread for the listening loop, use the start button to start, close button to close
        Thread thread = new Thread(new ThreadStart(StartListening));
       
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        //Constructor
        public mainServer()
        {
            InitializeComponent();
            Console.WriteLine("Initializing");

            #region display current ip address of the server

            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            //string myip = IpEntry.AddressList[0].ToString();
            string myip = string.Empty;

            foreach (IPAddress ip in IpEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    myip = ip.ToString();
                    break;
                }
            }

            textBox1.Text = "Current IP is " + myip;
            textBox2.Text = "Current Port is " + portNum.ToString();            

            #endregion
        }

        public static void StartListening()
        {
            Console.WriteLine("StartListening");
            // Establish the local endpoint for the socket.           
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, portNum);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(2048);              

                // Start an asynchronous socket to listen for connections. 
                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);                    

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.UTF8.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.sb.ToString();
                if (content.IndexOf("<XXF>") > -1)
                {
                    // Control the cursor
                    cursor_Control(handler, content);
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void cursor_Control(Socket handler, String data)
        {
                        
            #region Get sensor data
            Console.WriteLine("Got data: " + data);

            data = data.Replace("<XXF>", "").Trim("\0".ToCharArray());            
            string[] pairs = data.Split('|'); 
            
            #endregion          

            //mode selection
            //0=test, 1=clicker, 2=2D, 3=3D, 4=leg, 5=touchpad
            int mode = int.Parse(pairs[0]);
            if (mode == 0)
            {
                #region connection 
                Console.WriteLine("0received");
                
                data = "8888";
                Console.WriteLine("Send data: " + data);
                #endregion
            }
            else if(mode == 1)
            {   
                #region clicker

                #region extract parameters

                double mYaw = 1;
                double mPitch = double.Parse(pairs[1]);
                double mRoll = double.Parse(pairs[2]);
                int control = int.Parse(pairs[3]);

                #endregion

                #region learning algorithm

                if (learning == true)
                {
                    if (doOnce == false)
                    {//initial value
                        x_Left = mRoll;
                        x_Right = mRoll;
                        y_Upper = mPitch;
                        y_Lower = mPitch;
                        //you can do it only for once
                        doOnce = true;
                    }
                    else
                    {
                        //update in every loop
                        x_Left = Math.Min(x_Left, mRoll);
                        x_Right = Math.Max(x_Right, mRoll);
                        y_Upper = Math.Max(y_Upper, mPitch);
                        y_Lower = Math.Min(y_Lower, mPitch);

                        Console.WriteLine("Left: {0} , Right: {1} , Upper: {2} , Lower: {3}",
                        x_Left, x_Right, y_Upper, y_Lower);
                    }

                }

                #endregion

                else
                {
                    //Low-pass filter to smooth the sensor data          
                    lowPass.lowPassFilter(ref last_Output, ref cursor_brake_X, ref cursor_brake_Y, ref mYaw, ref mPitch, ref mRoll, out out_Yaw, out out_Pitch, out out_Roll, out cursor_brake_X, out cursor_brake_Y, out last_Output);

                    #region Control the Cursor and Click

                    PONITAPI p = new PONITAPI();

                    //get the location of the cursor
                    GetCursorPos(ref p);

                    int current_X = p.x;
                    int current_Y = p.y;

                    #region PPT Clicker

                    //Left click and Next PPT Clicked
                    if(control == 1){
                        //bottundown
                        mouse_event(MOUSEEVENTF_LEFTDOWN, p.x, p.y, 0, 0);
                        //buttonup
                        mouse_event(MOUSEEVENTF_LEFTUP, p.x, p.y, 0, 0);
                        Console.WriteLine("Next PPT");
                        data = "Next PPT";
                    }
                    //Previous PPT Clicked
                    else if(control == 2){
                        //keydown
                        keybd_event((byte)System.Windows.Forms.Keys.Left, 0, 0, 0);
                        //keyup
                        keybd_event((byte)System.Windows.Forms.Keys.Left, 0, 0x7F, 0);
                        Console.WriteLine("Previous PPT");
                        data = "Previous PPT";
                    }
                    //Start PPT Clicked
                    else if (control == 3)
                    {
                        //keydown
                        keybd_event((byte)System.Windows.Forms.Keys.F5, 0, 0, 0);
                        //keyup
                        keybd_event((byte)System.Windows.Forms.Keys.F5, 0, 0x7F, 0);
                        Console.WriteLine("Start PPT");
                        data = "Start PPT";
                    }
                    //End PPT Clicked
                    if (control == 4)
                    {
                        //keydown
                        keybd_event((byte)System.Windows.Forms.Keys.Escape, 0, 0, 0);
                        //keyup
                        keybd_event((byte)System.Windows.Forms.Keys.Escape, 0, 0x7F, 0);
                        Console.WriteLine("End PPT");
                        data = "End PPT";
                    }

                    #endregion

                    #region mapping
                    else
                    {
                        //Linear Mapping
                        mappingAlgorithm.linearMapping(ref x_Left, ref x_Right, ref y_Upper, ref y_Lower, ref current_X, ref current_Y, ref out_Roll, ref out_Pitch, out p.x, out p.y);
                        //set the location of the cursor           
                        SetCursorPos(p.x, p.y);

                        Console.WriteLine("Yaw: {0} , Pitch: {1} , Roll: {2}",
                        mYaw, mPitch, mRoll);
                    }
                    #endregion

                    #endregion
                }
                #endregion
            }
            else if(mode == 2)
            {   
                #region 2D mouse

                #region extract parameters

                double accX = double.Parse(pairs[1]);
                double accY = double.Parse(pairs[2]);
                int controlclick = int.Parse(pairs[3]);
                int xSpeed = 0;
                int ySpeed = 0;
                //int resetRound = 0;

                #endregion

                #region learning algorithm

                if (learning == true)
                {
                    if (doOnce == false)
                    {//initial value
                        x_Acc_min = accX;
                        x_Acc_max = accX;
                        y_Acc_min = accY;
                        y_Acc_max = accY;
                        //you can do it only for once
                        doOnce = true;
                    }
                    else
                    {
                        //update in every loop
                        x_Acc_min = Math.Min(x_Acc_min, accX);
                        x_Acc_max = Math.Max(x_Acc_max, accX);
                        y_Acc_min = Math.Min(y_Acc_min, accY);
                        y_Acc_max = Math.Max(y_Acc_max, accY);

                        //x_Acc_min = -1;
                        //x_Acc_max = 1;
                        //y_Acc_min = -1;
                        //y_Acc_max = 1;

                        Console.WriteLine("X_min: {0} , X_max: {1} , Y_min: {2} , Y_max: {3}",
                        x_Acc_min, x_Acc_max, y_Acc_min, y_Acc_max);
                    }

                }

                #endregion

                #region Control the Cursor and Click
                else
                {
                    PONITAPI p = new PONITAPI();

                    //get the location of the cursor
                    GetCursorPos(ref p);

                    int current_X = p.x;
                    int current_Y = p.y;

                    //left click
                    if (controlclick == 1)
                    {
                        //control click
                        mouse_event(MOUSEEVENTF_LEFTDOWN, p.x, p.y, 0, 0);
                        Console.WriteLine("Left Pressed");
                        mouse_event(MOUSEEVENTF_LEFTUP, p.x, p.y, 0, 0);
                        mClicked_left = true;
                        //Thread.Sleep(3000);
                    }
                    //right click
                    else if (controlclick == 2)
                    {
                        //control click
                        mouse_event(MOUSEEVENTF_RIGHTDOWN, p.x, p.y, 0, 0);
                        Console.WriteLine("Right Pressed");
                        mouse_event(MOUSEEVENTF_RIGHTUP, p.x, p.y, 0, 0);
                        mClicked_right = true;
                        //Thread.Sleep(3000);
                    }
                    else
                    {
                        Console.WriteLine("xAcc: {0} , yAcc: {1}", accX, accY);
                        mappingAlgorithm.twoDimentionMapping(ref current_X, ref current_Y, ref xSpeed, ref ySpeed, ref accX, ref accY, ref x_Acc_min, ref x_Acc_max, ref y_Acc_min, ref y_Acc_max, ref resetRound, out xSpeed, out ySpeed, out p.x, out p.y, out resetRound);
                        Console.WriteLine("p.x: {0} , p.y: {1}", p.x, p.y);
                        //set the location of the cursor            
                        SetCursorPos(p.x, p.y);
                    }
                }
                #endregion
                
                #endregion
            }
            else if (mode == 3)
            {   
                #region 3D mouse

                #region extract parameters

                double mYaw = 1;
                double mPitch = double.Parse(pairs[1]);
                double mRoll = double.Parse(pairs[2]);
                int control1 = int.Parse(pairs[3]);

                #endregion

                #region learning algorithm

                if (learning == true)
                {
                    if (doOnce == false)
                    {//initial value
                        x_Left = mRoll;
                        x_Right = mRoll;
                        y_Upper = mPitch;
                        y_Lower = mPitch;
                        //you can do it only for once
                        doOnce = true;
                    }
                    else
                    {
                        //update in every loop
                        x_Left = Math.Min(x_Left, mRoll);
                        x_Right = Math.Max(x_Right, mRoll);
                        y_Upper = Math.Max(y_Upper, mPitch);
                        y_Lower = Math.Min(y_Lower, mPitch);

                        Console.WriteLine("Left: {0} , Right: {1} , Upper: {2} , Lower: {3}",
                        x_Left, x_Right, y_Upper, y_Lower);
                    }

                }

                #endregion

                else
                {
                    //Low-pass filter to smooth the sensor data          
                    lowPass.lowPassFilter(ref last_Output, ref cursor_brake_X, ref cursor_brake_Y, ref mYaw, ref mPitch, ref mRoll, out out_Yaw, out out_Pitch, out out_Roll, out cursor_brake_X, out cursor_brake_Y, out last_Output);
                    
                    #region Control the Cursor and Click

                    PONITAPI p = new PONITAPI();

                    //get the location of the cursor
                    GetCursorPos(ref p);

                    int current_X = p.x;
                    int current_Y = p.y;

                    #region control click

                    //left click
                    if (control1 == 1)
                    {
                        //control click
                        mouse_event(MOUSEEVENTF_LEFTDOWN, p.x, p.y, 0, 0);
                        //Console.WriteLine("Left Pressed");
                        mouse_event(MOUSEEVENTF_LEFTUP, p.x, p.y, 0, 0);
                        mClicked_left = true;
                        //Thread.Sleep(3000);
                    }

                    //right click
                    else if (control1 == 3)
                    {
                        //control click
                        mouse_event(MOUSEEVENTF_RIGHTDOWN, p.x, p.y, 0, 0);
                        //Console.WriteLine("Right Pressed");
                        mouse_event(MOUSEEVENTF_RIGHTUP, p.x, p.y, 0, 0);
                        mClicked_right = true;
                        //Thread.Sleep(3000);
                    }

                    //left double click
                    else if (control1 == 2)
                    {
                        //control click
                        mouse_event(MOUSEEVENTF_LEFTDOWN, p.x, p.y, 0, 0);
                        //Console.WriteLine("Left Pressed");
                        mouse_event(MOUSEEVENTF_LEFTUP, p.x, p.y, 0, 0);

                        //control click
                        mouse_event(MOUSEEVENTF_LEFTDOWN, p.x, p.y, 0, 0);
                        //Console.WriteLine("Left Pressed");
                        mouse_event(MOUSEEVENTF_LEFTUP, p.x, p.y, 0, 0);

                        mClicked_left = true;
                        //Thread.Sleep(3000);
                    }

                    //right double click
                    else if (control1 == 4)
                    {
                        //control click
                        mouse_event(MOUSEEVENTF_RIGHTDOWN, p.x, p.y, 0, 0);
                        //Console.WriteLine("Right Pressed");
                        mouse_event(MOUSEEVENTF_RIGHTUP, p.x, p.y, 0, 0);

                        //control click
                        mouse_event(MOUSEEVENTF_RIGHTDOWN, p.x, p.y, 0, 0);
                        //Console.WriteLine("Right Pressed");
                        mouse_event(MOUSEEVENTF_RIGHTUP, p.x, p.y, 0, 0);

                        mClicked_right = true;
                        //Thread.Sleep(3000);
                    }

                    #endregion


                    #region mapping

                    //Mapping
                    mappingAlgorithm.linearMapping(ref x_Left, ref x_Right, ref y_Upper, ref y_Lower, ref current_X, ref current_Y, ref out_Roll, ref out_Pitch, out p.x, out p.y);
                    //set the location of the cursor           
                    SetCursorPos(p.x, p.y);

                    Console.WriteLine("Yaw: {0} , Pitch: {1} , Roll: {2}",
                    mYaw, mPitch, mRoll);

                    #endregion

                    #endregion
                }

                #endregion
            }
            else if (mode == 4)
            {  
                #region Leg Control

                #region extract paramteres

                double mYaw = double.Parse(pairs[1]);
                double mPitch = double.Parse(pairs[2]);
                double mRoll = double.Parse(pairs[3]);
                double accX = double.Parse(pairs[4]);
                double accY = double.Parse(pairs[5]);
                double accZ = double.Parse(pairs[6]);

                #endregion

                #region learning algorithm

                if (learning == true)
                {
                    if (doOnce == false)
                    {//initial value
                        x_Left = mRoll;
                        x_Right = mRoll;
                        y_Upper = mPitch;
                        y_Lower = mPitch;
                        //you can do it only for once
                        doOnce = true;
                    }
                    else
                    {
                        //update in every loop
                        x_Left = Math.Min(x_Left, mRoll);
                        x_Right = Math.Max(x_Right, mRoll);
                        y_Upper = Math.Max(y_Upper, mPitch);
                        y_Lower = Math.Min(y_Lower, mPitch);

                        Console.WriteLine("Left: {0} , Right: {1} , Upper: {2} , Lower: {3}",
                        x_Left, x_Right, y_Upper, y_Lower);
                    }

                }

                #endregion

                else
                {
                    //Low-pass filter to smooth the sensor data          
                    lowPass.lowPassFilter(ref last_Output, ref cursor_brake_X, ref cursor_brake_Y, ref mYaw, ref mPitch, ref mRoll, out out_Yaw, out out_Pitch, out out_Roll, out cursor_brake_X, out cursor_brake_Y, out last_Output);

                    #region Control the Cursor and Click

                    PONITAPI p = new PONITAPI();

                    //get the location of the cursor
                    GetCursorPos(ref p);

                    int current_X = p.x;
                    int current_Y = p.y;

                    double click_threshold = 0.30;


                    //left click
                    if (Math.Abs(accZ) > click_threshold)
                    {
                        //control click
                        mouse_event(MOUSEEVENTF_LEFTDOWN, last_X, last_Y, 0, 0);
                        Console.WriteLine("Left Pressed");
                        mouse_event(MOUSEEVENTF_LEFTUP, last_X, last_Y, 0, 0);
                        mClicked_left = true;
                        //Thread.Sleep(2000);
                    }

                    else
                    {
                        //right click,lower priority
                        if (Math.Abs(accX) > click_threshold)
                        {
                            //control click
                            mouse_event(MOUSEEVENTF_RIGHTDOWN, last_X, last_Y, 0, 0);
                            Console.WriteLine("Right Pressed");
                            mouse_event(MOUSEEVENTF_RIGHTUP, last_X, last_Y, 0, 0);
                            mClicked_right = true;
                        }

                        else
                        {   //linear mapping
                            mappingAlgorithm.linearMapping(ref x_Left, ref x_Right, ref y_Upper, ref y_Lower, ref current_X, ref current_Y, ref out_Roll, ref out_Pitch, out p.x, out p.y);                            
                        }
                    }

                    //set the location of the cursor            
                    SetCursorPos(p.x, p.y);

                    //click on the previous place
                    last_X = p.x;
                    last_Y = p.y;

                    Console.WriteLine("Yaw: {0} , Pitch: {1} , Roll: {2}",
                    mYaw, mPitch, mRoll);

                    #endregion
                }

                #endregion
            }
            else if (mode == 5)
            {
                #region Touch Pad

                #region extract touch pad parameters
                padcontrolclick = int.Parse(pairs[1]);
                padXcurpos = double.Parse(pairs[2]);
                padYcurpos = double.Parse(pairs[3]);
                padXlastpos = double.Parse(pairs[4]);
                padYlastpos = double.Parse(pairs[5]);
                #endregion
                
                #region click and move cursor

                //left click
                if (padcontrolclick == 1)
                {
                    //control click
                    mouse_event(MOUSEEVENTF_LEFTDOWN, padXcursor, padYcursor, 0, 0);
                    Console.WriteLine("Left Pressed");
                    mouse_event(MOUSEEVENTF_LEFTUP, padXcursor, padYcursor, 0, 0);
                    mClicked_left = true;
                }
                //right click
                if (padcontrolclick == 2)
                {
                    //control click
                    mouse_event(MOUSEEVENTF_RIGHTDOWN, padXcursor, padYcursor, 0, 0);
                    Console.WriteLine("Right Pressed");
                    mouse_event(MOUSEEVENTF_RIGHTUP, padXcursor, padYcursor, 0, 0);
                    mClicked_right = true;
                }
                // move
                double xdiff = padXcurpos - padXlastpos;
                double ydiff = padYcurpos - padYlastpos;
                padXcursor += Convert.ToInt32(xdiff*3);
                padYcursor += Convert.ToInt32(ydiff*3);

                //set the location of the cursor            
                SetCursorPos(padXcursor, padYcursor);
                #endregion
                #endregion
            }
            else
            {
                #region Display error

                Console.WriteLine("ERROR: Unknown Mode Selection.");
                data = "ERROR: unknown Mode Selection.";

                #endregion
            }            
 

            #region Manage the data to transmit back

                    if (mClicked_left == true)
                    {
                        data = "1";
                        //block left click for a while
                        click_timer_left = 30;
                        mClicked_left = false;
                    }
                    else if (mClicked_right == true)
                    {
                        data = "1";
                        //block right click for a while
                        click_timer_right = 30;
                        mClicked_right = false;
                    }
                    else
                    {
                        data = "0";
                    }
                    // Convert the string data to byte data using ASCII encoding.
                    byte[] byteData = Encoding.UTF8.GetBytes(data);                

                    // Begin sending the data to the remote device.
                    handler.BeginSend(byteData, 0, byteData.Length, 0,
                        new AsyncCallback(SendCallback), handler);

                    //for safety
                    data = "Connected";

                #endregion

        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
 

        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new mainServer());  
    
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(24, 184);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start Learning";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 46);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(256, 21);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 74);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(256, 21);
            this.textBox2.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(190, 183);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // mainServer
            // 
            this.ClientSize = new System.Drawing.Size(295, 265);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "mainServer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //the second click to start working
            if (button1.Text == "Start")
            {
                button1.Text = "Working";
                learning = false;
            }

            //the first click to start learning
            if ((!thread.IsAlive) && (button1.Text == "Start Learning"))
            {
                thread.Start();
                button1.Text = "Start";
                learning = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        { 
            //close the form
            this.Close();

            //kill the thread for the listening loop
            if (thread.IsAlive)
            {
                thread.Abort();
            }           
        } 

    }

    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}
