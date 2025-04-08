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
        public enum SoundKey { Shoot, Explosion, PowerUp, SniperFire, Dialogue, Drive, FixDeath }
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
                    if (soundEffectPlayer.soundEffects.TryGetValue(key, out SoundEffect value))
                    {
                        dialogueInstance = value.CreateInstance();
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

            if (normalizedVolume <= 0.01f)
            {
                if (driveInstance != null && driveInstance.State == SoundState.Playing)
                {
                    driveInstance.Stop();
                    driveInstance.Dispose();
                    driveInstance = null;
                }
                return;
            }

            if (driveInstance == null || driveInstance.State != SoundState.Playing)
            {
                if (soundEffectPlayer.soundEffects.TryGetValue(SoundKey.Drive, out SoundEffect driveSound))
                {
                    driveInstance = driveSound.CreateInstance();
                    driveInstance.IsLooped = true;
                    driveInstance.Volume = normalizedVolume;
                    driveInstance.Play();
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
                LoadSound(SoundKey.FixDeath, "fix_death.wav");
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
                if (soundEffects.TryGetValue(key, out SoundEffect value))
                {
                    float playVolume = MathHelper.Clamp(Volume * volumeMod, 0f, 1f);
                    value.Play(playVolume, 0f, 0f);
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
                if (musicTracks.TryGetValue(key, out Song value))
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(value);
                }
            }

            public void Stop() => MediaPlayer.Stop();
        }
    }
}
