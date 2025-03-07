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
                LoadSound(SoundKey.Shoot, "default_fire.wav");
                LoadSound(SoundKey.Explosion, "explosion.wav");
                LoadSound(SoundKey.PowerUp, "powerup.wav");
                LoadSound(SoundKey.SniperFire, "sniper_fire.wav");
            }

            private void LoadSound(SoundKey key, string fileName)
            {
                string fullPath = Path.Combine(soundPath, fileName);
                if (File.Exists(fullPath))
                {
                    soundEffects[key] = SoundEffect.FromFile(fullPath);
                }
                else
                {
                    Console.WriteLine($"Warning: Sound file missing - {fullPath}");
                }
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
                    Console.WriteLine("Music file not found", musicFile);
                    return; 
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
