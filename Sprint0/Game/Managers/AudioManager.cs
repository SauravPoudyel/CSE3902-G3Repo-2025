using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Sprint0
{
    public static class AudioManager
    {
        public enum SoundKey { Shoot, Explosion, PowerUp, SniperFire }
        public enum MusicKey { Background }

        private static SoundEffectPlayer soundEffectPlayer = new SoundEffectPlayer();
        private static MusicPlayer musicPlayer = new MusicPlayer();
        private static readonly string soundPath = Path.Combine(Globals.projectDirectory, "Content", "Sounds");

        public static void LoadContent()
        {
            soundEffectPlayer.LoadContent();
            musicPlayer.LoadContent();
        }

        public static void PlaySound(SoundKey key) => soundEffectPlayer.Play(key);
        public static void PlayMusic(MusicKey key) => musicPlayer.Play(key);
        public static void StopMusic() => musicPlayer.Stop();

        private class SoundEffectPlayer
        {
            private Dictionary<SoundKey, SoundEffect> soundEffects = new();

            public void LoadContent()
            {
                soundEffects[SoundKey.Shoot] = SoundEffect.FromFile(Path.Combine(soundPath, "default_fire.wav"));
                soundEffects[SoundKey.Explosion] = SoundEffect.FromFile(Path.Combine(soundPath, "explosion.wav"));
                soundEffects[SoundKey.PowerUp] = SoundEffect.FromFile(Path.Combine(soundPath, "powerup.wav"));
                soundEffects[SoundKey.SniperFire] = SoundEffect.FromFile(Path.Combine(soundPath, "sniper_fire.wav"));
            }

            public void Play(SoundKey key)
            {
                if (soundEffects.ContainsKey(key))
                    soundEffects[key].Play();
            }
        }

        private class MusicPlayer
        {
            private Dictionary<MusicKey, Song> musicTracks = new();

            public void LoadContent()
            {
                string musicFile = Path.Combine(soundPath, "music_loop.ogg");

                if (!File.Exists(musicFile))
                {
                    throw new FileNotFoundException("Music file not found", musicFile);
                }

                musicTracks[MusicKey.Background] = Song.FromUri("Background", new Uri(musicFile, UriKind.Absolute));
            }

            public void Play(MusicKey key)
            {
                if (musicTracks.ContainsKey(key))
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.5f;
                    MediaPlayer.Play(musicTracks[key]);
                }
            }

            public void Stop() => MediaPlayer.Stop();
        }
    }
}
