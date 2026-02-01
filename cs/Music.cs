using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace mask
{
    partial class Music
    {
        public static void startBackgroundMusic()
        {
            string audioPath = getAudioPath();
            audioPath += "\\audio\\background.wav";
            SoundPlayer sound = new SoundPlayer(audioPath);
            sound.PlayLooping();
        }

        public static void startIntroMusic()
        {
            string audioPath = getAudioPath();
            audioPath += "\\audio\\intro.wav";
            SoundPlayer sound = new SoundPlayer(audioPath);
            sound.PlayLooping();
        }

        public static string getAudioPath()
        {
            string dir = Directory.GetCurrentDirectory();
            string audioPath = dir;

#if DEBUG
            audioPath = dir + "\\..\\..\\..\\..";
#endif
            
            return audioPath;
        }
    }
}
