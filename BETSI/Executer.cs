using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BETSI
{
    class Executer
    {
        // Basic speech methods

        public static void GetHours()
        {
            OutputSound.Speak(DateTime.Now.ToShortTimeString()); // OutPut Sound execute the Local Now Time
        }

        public static void GetDate()
        {
            OutputSound.Speak(DateTime.Now.ToShortDateString()); // OutPut Sound execute the Local Now Date
        }

        public static void System_Stats_Calc()
        {
            OutputSound.Speak("Teste::::speak");
        }

    }
}
