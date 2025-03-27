using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace Sprint0
{
    public static class AudioCommands
    {

        public class AudioMuteCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                AudioManager.SetVolume(0f);
            }
        }
        public class AudioIncreaseCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                AudioManager.SetVolume(MathHelper.Clamp(AudioManager.Volume + 0.1f, 0f, 1f));
            }
        }

        public class AudioDecreaseCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                AudioManager.SetVolume(MathHelper.Clamp(AudioManager.Volume - 0.1f, 0f, 1f));
            }
        }

        public class AudioDriveCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("currentSpeed") && parameters["currentSpeed"] is float currentSpeed &&
                    parameters.ContainsKey("maxSpeed") && parameters["maxSpeed"] is float maxSpeed)
                {
                    AudioManager.AudioDrive(currentSpeed, maxSpeed);
                }
            }

        }
    }
}