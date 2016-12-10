using Engine2.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

using Engine2.Util;

namespace Engine2.Audio
{
    public class BasicSound : GameActor
    {
        protected string soundFile;

        public bool IsGlobal = false;
        public SoundState CurrentState;

        private SoundPlayer player;
        private bool isPlaying = false;

        public BasicSound(string soundFile)
        {
            this.soundFile = soundFile;

            this.canInit = true;
            this.IsCollidable = false;

            CurrentState = SoundState.Stop;
        }

        ~BasicSound()
        {
            if (player != null && isPlaying)
                player.Stop();
        }

        public override void Init()
        {
            player = new SoundPlayer(Constants.RootFolder + "/" + soundFile);
            player.Load();
        }

        public override void Render()
        {
            // Nothing to Render here..
        }

        /// <summary>
        /// Based on the Current State of the Actor, we continue playing it, stop or pause the music.
        /// </summary>
        public override void Tick()
        {
            // Check if we need to continue playing this audio. If not. kill it

            switch (CurrentState)
            {
                case SoundState.Play:
                    if(!isPlaying)
                    {
                        player.Play();
                        isPlaying = true;
                    }
                    break;

                case SoundState.Pause:
                    break;

                case SoundState.Stop:
                    if(isPlaying)
                    {
                        player.Stop();
                        isPlaying = false;
                    }                    
                    break;
            }
        }

        public override void onHit(GameActor otherActor)
        {
            // No collission for this!
        }

        public virtual void Play()
        {
            CurrentState = SoundState.Play;
        }

        public virtual void Pause()
        {
            CurrentState = SoundState.Pause;
        }

        public virtual void Stop()
        {
            CurrentState = SoundState.Stop;
        }

    }

    public enum SoundState
    {
        Play,
        Pause,
        Stop
    }
    
}
