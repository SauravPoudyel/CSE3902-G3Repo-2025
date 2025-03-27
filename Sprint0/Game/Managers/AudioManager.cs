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
        public enum SoundKey { Shoot, Explosion, PowerUp, SniperFire, Dialogue, Drive }
        public enum MusicKey { Background }

        private static SoundEffectPlayer soundEffectPlayer = new SoundEffectPlayer();
        private static MusicPlayer musicPlayer = new MusicPlayer();
        public static float Volume { get; private set; } = 1f;
        private static readonly string soundPath = Path.Combine(Globals.projectDirectory, "Content", "Sounds");

        private static SoundEffectInstance dialogueInstance;
        private static SoundEffectInstance driveInstance;

        public static void LoadContent()
        {
            soundEffectPlayer.LoadContent();
            musicPlayer.LoadContent();
        }

        public static void SetVolume(float volume)
        {
            Volume = Math.Clamp(volume, 0f, 1f);
            MediaPlayer.Volume = Volume;

            if (driveInstance != null)
                driveInstance.Volume = driveInstance.Volume * Volume;
        }

        public static void PlaySound(SoundKey key, float volumeMod = 1)
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
                soundEffectPlayer.Play(key, volumeMod);
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

        public static void AudioDrive(float currentSpeed, float maxSpeed)
        {
            float normalizedVolume = MathHelper.Clamp(currentSpeed / maxSpeed, 0f, 1f) * Volume;

            if (driveInstance == null)
            {
                if (soundEffectPlayer.soundEffects.ContainsKey(SoundKey.Drive))
                {
                    driveInstance = soundEffectPlayer.soundEffects[SoundKey.Drive].CreateInstance();
                    driveInstance.IsLooped = true;
                    driveInstance.Volume = normalizedVolume;
                    driveInstance.Play();
                }
                else
                {
                    Console.WriteLine("Drive sound not loaded.");
                }
            }
            else
            {
                driveInstance.Volume = normalizedVolume;
            }
        }

        public static void StopDriveSound()
        {
            if (driveInstance != null)
            {
                driveInstance.Stop();
                driveInstance.Dispose();
                driveInstance = null;
            }
        }

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
                LoadSound(SoundKey.Drive, "driving.wav"); 
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
                    Console.WriteLine("Warning: Sound file missing - " + fullPath);
                }
            }

            public void Play(SoundKey key, float volumeMod = 1)
            {
                if (soundEffects.ContainsKey(key))
                {
                    float playVolume = MathHelper.Clamp(Volume * volumeMod, 0f, 1f);
                    soundEffects[key].Play(playVolume, 0f, 0f);
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
