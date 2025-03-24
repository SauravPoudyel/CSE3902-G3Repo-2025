using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public static class AudioCommands
    {

        public class AudioMuteCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                AudioManager.setVolume(0f);
            }
        }
        public class AudioIncreaseCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                AudioManager.setVolume(MathHelper.Clamp(AudioManager.Volume + 0.1f, 0f, 1f));
            }
        }

        public class AudioDecreaseCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                AudioManager.setVolume(MathHelper.Clamp(AudioManager.Volume - 0.1f, 0f, 1f));
            }
        }
    }
}