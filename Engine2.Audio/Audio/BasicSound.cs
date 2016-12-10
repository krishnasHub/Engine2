using Engine2.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Audio
{
    public class BasicSound : GameActor
    {
        protected string soundFile;

        public bool IsGlobal = false;
        public SoundState CurrentState;

        public BasicSound(string soundFile)
        {
            this.soundFile = soundFile;

            this.canInit = true;
            this.IsCollidable = false;
        }

        public override void Init()
        {
            // Init the sound file
            CurrentState = SoundState.Stop;
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

            switch(CurrentState)
            {
                case SoundState.Play:
                    break;

                case SoundState.Pause:
                    break;

                case SoundState.Stop:
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
