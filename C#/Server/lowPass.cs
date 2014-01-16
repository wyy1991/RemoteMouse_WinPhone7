/* low pass filter to smooth sensor data
 * Author:Xiufeng Xie
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xxf_Server
{
    class lowPass
    {
        //linear Low Pass Filter
        public static void lowPassFilter(ref double[] last_Output, ref bool cursor_brake_X, ref bool cursor_brake_Y, ref double in_Yaw, ref double in_Pitch, ref double in_Roll, out double out_Yaw, out double out_Pitch, out double out_Roll, out bool cursor_brake_X_out, out bool cursor_brake_Y_out, out double[] last_Output_out)
        {
            cursor_brake_X_out = cursor_brake_X;

            cursor_brake_Y_out = cursor_brake_X;

            //coefficient of the first-order low pass filter
            const double alpha = 0.08;

            //first order low-pass filter
            out_Yaw = (1 - alpha) * last_Output[0] + alpha * in_Yaw;
            out_Pitch = (1 - alpha) * last_Output[1] + alpha * in_Pitch;
            out_Roll = (1 - alpha) * last_Output[2] + alpha * in_Roll;

            /*brake system
             *   if the absolute roll or pitch value become smaller, maybe the user wanted to stop the cursor
             */

            //X axis
            if (Math.Abs(out_Roll) < Math.Abs(last_Output[2]))
            {
                cursor_brake_X_out = true;
            }
            else
            {
                cursor_brake_X_out = false;
            }
            //Y axis
            if (Math.Abs(out_Pitch) < Math.Abs(last_Output[1]))
            {
                cursor_brake_Y_out = true;
            }
            else
            {
                cursor_brake_Y_out = false;
            }

            //store the output for the LPF formula
            last_Output_out = last_Output;
            last_Output_out[0] = out_Yaw;
            last_Output_out[1] = out_Pitch;
            last_Output_out[2] = out_Roll;
        }  

    }
}
