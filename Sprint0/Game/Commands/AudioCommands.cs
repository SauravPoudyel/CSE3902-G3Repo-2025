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
                AudioManager.SetVolume(MathHelper.Clamp(AudioManager.Volume - 0.2f, 0f, 1f));
            }
        }

        public class AudioDriveCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("currentSpeed", out object value) && value is float currentSpeed &&
                    parameters.TryGetValue("maxSpeed", out object value2) && value is float maxSpeed)
                {
                    AudioManager.AudioDrive(currentSpeed, maxSpeed);
                }
            }

        }
    }
}