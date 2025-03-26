using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public static class AudioManager
    {
        public enum SoundKey { Shoot, Explosion, PowerUp, SniperFire, Dialogue }
        public enum MusicKey { Background }

        private static SoundEffectPlayer soundEffectPlayer = new SoundEffectPlayer();
        private static MusicPlayer musicPlayer = new MusicPlayer();
        public static float Volume { get; private set; } = 1f;
        private static readonly string soundPath = Path.Combine(Globals.projectDirectory, "Content", "Sounds");

        private static SoundEffectInstance dialogueInstance;

        public static void LoadContent()
        {
            soundEffectPlayer.LoadContent();
            musicPlayer.LoadContent();
        }

        public static void setVolume(float volume)
        {
            Volume = Math.Clamp(volume, 0f, 1f);
            MediaPlayer.Volume = volume;
        }

        public static void PlaySound(SoundKey key)
        {
            if (key == SoundKey.Dialogue)
            {
                if (dialogueInstance == null || dialogueInstance.State != SoundState.Playing)
                {
                    if (soundEffectPlayer.soundEffects.ContainsKey(key))
                    {
                        dialogueInstance = soundEffectPlayer.soundEffects[key].CreateInstance();
                        dialogueInstance.IsLooped = true;
                        dialogueInstance.Volume = Volume;
                        dialogueInstance.Play();
                    }
                }
            }
            else
            {
                soundEffectPlayer.Play(key);
            }
        }

        public static void StopDialogue()
        {
            if (dialogueInstance != null && dialogueInstance.State == SoundState.Playing)
            {
                dialogueInstance.Stop();
                dialogueInstance.Dispose();
                dialogueInstance = null;
            }
        }

        public static void PlayMusic(MusicKey key) => musicPlayer.Play(key);
        public static void StopMusic() => musicPlayer.Stop();

        private class SoundEffectPlayer
        {
            public Dictionary<SoundKey, SoundEffect> soundEffects = new Dictionary<SoundKey, SoundEffect>();

            public void LoadContent()
            {
                LoadSound(SoundKey.Shoot, "default_fire.wav");
                LoadSound(SoundKey.Explosion, "explosion.wav");
                LoadSound(SoundKey.PowerUp, "powerup.wav");
                LoadSound(SoundKey.SniperFire, "sniper_fire.wav");
                LoadSound(SoundKey.Dialogue, "dialogue.wav");
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
                {
                    soundEffects[key].Play(Volume, 0f, 0f);
                }
            }
        }

        private class MusicPlayer
        {
            private Dictionary<MusicKey, Song> musicTracks = new Dictionary<MusicKey, Song>();

            public void LoadContent()
            {
                string musicFile = Path.Combine(soundPath, "music_loop.ogg");
                if (!File.Exists(musicFile))
                {
                    Console.WriteLine("Music file not found: " + musicFile);
                    return;
                }
                musicTracks[MusicKey.Background] = Song.FromUri("Background", new Uri(musicFile, UriKind.Absolute));
            }

            public void Play(MusicKey key)
            {
                if (musicTracks.ContainsKey(key))
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(musicTracks[key]);
                }
            }

            public void Stop() => MediaPlayer.Stop();
        }
    }
}
