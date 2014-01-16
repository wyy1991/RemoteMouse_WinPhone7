using System;
using System.Collections.Generic;
using System.Linq;
/* the mapping algorithms from sensor data to location on the screen
 * Author:Xiufeng Xie
 */

using System.Text;
using System.Windows.Forms;

namespace xxf_Server
{
    class mappingAlgorithm
    {
        
        //Transmit static variable inside as parameter

        //delta method
        public static void mapping_3Dmouse(ref bool cursor_brake_X, ref bool cursor_brake_Y, ref int current_X, ref int current_Y, ref double rollValue, ref double pitchValue, out int xlocation, out int ylocation, out bool cursor_brake_X_out, out bool cursor_brake_Y_out)
        {

            double trigger_angle_X = 5;            

            cursor_brake_X_out=cursor_brake_X;

            cursor_brake_Y_out = cursor_brake_Y;

            //delta value method

            #region X axis

            if (rollValue > trigger_angle_X)
            {
                xlocation = current_X + (int)(rollValue - trigger_angle_X);
            }
            else if (rollValue < -trigger_angle_X)
            {
                xlocation = current_X + (int)(rollValue + trigger_angle_X);
            }
            else
            {
                xlocation = current_X;
                //shut down the brake
                cursor_brake_X_out = false;
            }

            //if the brake is on, stop the cursor
            if (cursor_brake_X == true)
            {
                xlocation = current_X;
            }

            #endregion

            #region compute Y location

            //linear mapping parameter  
            int y1 = 20;
            int y2 = -20;
            double yk = Screen.PrimaryScreen.Bounds.Height / (y2 - y1);
            double yb = -yk * y1;
            //compute the y location
            ylocation = Math.Min(Screen.PrimaryScreen.Bounds.Height, Math.Max(0, (int)(yk * pitchValue + yb)));

            #endregion            
            
        }

        //linear Mapping
        public static void linearMapping(ref double x_Left, ref double x_Right, ref double y_Upper, ref double y_Lower, ref int current_X, ref int current_Y, ref double rollValue, ref double pitchValue, out int xlocation, out int ylocation)
        {

            #region compute X location

            //linear mapping parameter  
            double x1 = x_Left;
            double x2 = x_Right;
            double xk = Screen.PrimaryScreen.Bounds.Width / (x2 - x1);
            double xb = -xk * x1;
            //compute the x location
            xlocation = Math.Min(Screen.PrimaryScreen.Bounds.Width, Math.Max(0, (int)(xk * rollValue + xb)));

            #endregion

            #region compute Y location

            //linear mapping parameter  
            double y1 = y_Upper;
            double y2 = y_Lower;
            double yk = Screen.PrimaryScreen.Bounds.Height / (y2 - y1);
            double yb = -yk * y1;
            //compute the y location
            ylocation = Math.Min(Screen.PrimaryScreen.Bounds.Height, Math.Max(0, (int)(yk * pitchValue + yb)));

            #endregion

        }

        //linear Mapping Leg
        public static void linearMapping_Leg(ref int current_X, ref int current_Y, ref double rollValue, ref double pitchValue, out int xlocation, out int ylocation)
        {

            #region compute X location

            //linear mapping parameter  
            int x1 = -5;
            int x2 = 19;
            double xk = Screen.PrimaryScreen.Bounds.Width / (x2 - x1);
            double xb = -xk * x1;
            //compute the x location
            xlocation = Math.Min(Screen.PrimaryScreen.Bounds.Width, Math.Max(0, (int)(xk * rollValue + xb)));

            #endregion

            #region compute Y location

            //linear mapping parameter  
            int y1 = -2;
            int y2 = -12;
            double yk = Screen.PrimaryScreen.Bounds.Height / (y2 - y1);
            double yb = -yk * y1;
            //compute the y location
            ylocation = Math.Min(Screen.PrimaryScreen.Bounds.Height, Math.Max(0, (int)(yk * pitchValue + yb)));

            #endregion

        }
        
        //3D Mapping
        public static void pointMapping(ref int current_X, ref int current_Y, ref double rollValue, ref double pitchValue, out int xlocation, out int ylocation)
        {           
            double center_X = Screen.PrimaryScreen.Bounds.Width / 2;
            double center_Y = Screen.PrimaryScreen.Bounds.Height / 2;
            double disToScr = center_Y / Math.Tan(20);
            double mRadius = Math.Abs(disToScr * Math.Tan(pitchValue));
            double delta_X = mRadius * Math.Sin(rollValue);
            double delta_Y;
            if (pitchValue >= 0)
            {
                delta_Y = -mRadius * Math.Cos(rollValue);
            }
            else
            {
                delta_Y = mRadius * Math.Cos(rollValue);
            }

            xlocation = Math.Min(Screen.PrimaryScreen.Bounds.Width, Math.Max(0, (int)(center_X + delta_X)));
            ylocation = Math.Min(Screen.PrimaryScreen.Bounds.Height, Math.Max(0, (int)(center_Y + delta_Y)));
            
        }

        public static void twoDimentionMapping(ref int current_X, ref int current_Y, ref int xSpeed, ref int ySpeed, ref double accX, ref double accY, ref double x_Acc_min, ref double x_Acc_max, ref double y_Acc_min, ref double y_Acc_max, ref int resetRound, out int new_X_Speed, out int new_Y_Speed, out int xlocation, out int ylocation, out int newResetRound)
        {
            new_X_Speed = xSpeed;
            new_Y_Speed = ySpeed;

            if (accX > 0)
            {
                if (accX > 0 && accX < x_Acc_max / 5)
                {
                    new_X_Speed = xSpeed + 10;
                }
                else if (accX >= x_Acc_max / 5 && accX < 2 * x_Acc_max / 5)
                {
                    new_X_Speed = xSpeed + 30;
                }
                else if (accX >= 2 * x_Acc_max / 5 && accX < 3 * x_Acc_max / 5)
                {
                    new_X_Speed = xSpeed + 90;
                }
                else if (accX >= 3 * x_Acc_max / 5 && accX < 4 * x_Acc_max / 5)
                {
                    new_X_Speed = xSpeed + 270;
                }
                else if (accX >= 4 * x_Acc_max / 5 && accX <= x_Acc_max)
                {
                    new_X_Speed = xSpeed + 810;
                }
            }
            else if (accX < 0)
            {
                if (accX < 0 && accX > x_Acc_min / 5)
                {
                    new_X_Speed = xSpeed - 10;
                }
                else if (accX <= x_Acc_min / 5 && accX > 2 * x_Acc_min / 5)
                {
                    new_X_Speed = xSpeed - 30;
                }
                else if (accX <= 2 * x_Acc_min / 5 && accX > 3 * x_Acc_min / 5)
                {
                    new_X_Speed = xSpeed - 90;
                }
                else if (accX <= 3 * x_Acc_min / 5 && accX > 4 * x_Acc_min / 5)
                {
                    new_X_Speed = xSpeed - 270;
                }
                else if (accX <= 4 * x_Acc_min / 5 && accX >= x_Acc_min)
                {
                    new_X_Speed = xSpeed - 810;
                }
            }

            if (accY > 0)
            {
                if (accY > 0 && accY < y_Acc_max / 5)
                {
                    new_Y_Speed = ySpeed + 10;
                }
                else if (accY >= y_Acc_max / 5 && accY < 2 * y_Acc_max / 5)
                {
                    new_Y_Speed = ySpeed + 30;
                }
                else if (accY >= 2 * y_Acc_max / 5 && accY < 3 * y_Acc_max / 5)
                {
                    new_Y_Speed = ySpeed + 90;
                }
                else if (accY >= 3 * y_Acc_max / 5 && accY < 4 * y_Acc_max / 5)
                {
                    new_Y_Speed = ySpeed + 270;
                }
                else if (accY >= 4 * y_Acc_max / 5 && accY <= y_Acc_max)
                {
                    new_Y_Speed = ySpeed + 810;
                }
            }
            else if (accY < 0)
            {
                if (accY < 0 && accY > y_Acc_min / 5)
                {
                    new_Y_Speed = ySpeed - 10;
                }
                else if (accY <= y_Acc_min / 5 && accY > 2 * y_Acc_min / 5)
                {
                    new_Y_Speed = ySpeed - 30;
                }
                else if (accY <= 2 * y_Acc_min / 5 && accY > 3 * y_Acc_min / 5)
                {
                    new_Y_Speed = ySpeed - 90;
                }
                else if (accY <= 3 * y_Acc_min / 5 && accY > 4 * y_Acc_min / 5)
                {
                    new_Y_Speed = ySpeed - 270;
                }
                else if (accY <= 4 * y_Acc_min / 5 && accY >= y_Acc_min)
                {
                    new_Y_Speed = ySpeed - 810;
                }
            }

            /*if (new_X_Speed == xSpeed || new_Y_Speed == ySpeed)
            {
                newResetRound = resetRound + 1;
            }
            else
            {
                newResetRound = 0;
            }*/

            newResetRound = resetRound + 1;
            if (newResetRound == 2)
            {
                new_X_Speed = 0;
                new_Y_Speed = 0;
                newResetRound = 0;
            }

            Console.WriteLine("X_min: {0} , X_max: {1} , Y_min: {2}, Y_max: {3}", x_Acc_min, x_Acc_max, y_Acc_min, y_Acc_max);
            Console.WriteLine("new_X_Speed: {0} , new_Y_Speed: {1} , newResetRound: {2}", new_X_Speed, new_Y_Speed, newResetRound);

            xlocation = (int)(current_X + new_X_Speed);
            ylocation = (int)(current_Y + new_Y_Speed);
        }

    }
}
