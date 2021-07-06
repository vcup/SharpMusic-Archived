using System;
using ManagedBass;

namespace SharpMusic.Backend.Play.BassManaged
{
    public class Channel
    {
        private Sound _sound;
        private Device _device = Device.GetDefaultDevice;

        public Channel()
        {
            Bass.ChannelSetDevice(0, _device.Handle);
        }
        
        public Channel(Sound sound)
        {
            Sound = sound;
            Bass.ChannelSetDevice(_sound.Handle, _device.Handle);
        }

        public Channel(Sound sound, Device device)
        {
            Sound = sound;
            Device = device;
            Bass.ChannelSetDevice(sound.Handle, device.Handle);
        }

        #region PlayFunc
        
        public void Play()
        {
            if (!Bass.ChannelPlay(_sound.Handle))
            {
                throw new BassException();
            }
            Bass.ChannelPlay(_sound.Handle);
        }

        public void Stop()
        {
            if (!Bass.ChannelStop(_sound.Handle))
            {
                throw new BassException();
            }
        }

        public void Pause()
        {
            if (!Bass.ChannelPause(_sound.Handle))
            {
                throw new BassException();
            }
        }

        public void Resume()
        {
            if (State != PlaybackState.Paused)
                return;
            Play();

        }

        #endregion
        
        #region PlaybackState
        
        public PlaybackState State
        {
            get => _sound != null? Bass.ChannelIsActive(_sound.Handle) : PlaybackState.Stopped;
            set
            {
                switch (value)
                {
                    case PlaybackState.Paused:
                        Pause();
                        break;
                    case PlaybackState.Stopped:
                        Stop();
                        break;
                    case PlaybackState.Playing:
                        Stop();
                        break;
                    case PlaybackState.Stalled:
                        throw new ArgumentException("Can't Set State to Stalled");
                    default:
                        throw new ArgumentException($"Invalid value:{value}");
                }
            }
        }

        public double Volume
        {
            get => Bass.ChannelGetAttribute(_sound.Handle, ChannelAttribute.Volume);
            set => Bass.ChannelSetAttribute(_sound.Handle, ChannelAttribute.Volume, value);
        }

        public TimeSpan Position
        {
            get => TimeSpan.FromSeconds(
                Bass.ChannelBytes2Seconds(_sound.Handle, Bass.ChannelGetPosition(_sound.Handle)));
            set => Bass.ChannelSetPosition(_sound.Handle, Bass.ChannelSeconds2Bytes(_sound.Handle, value.TotalSeconds));
        }

        public TimeSpan PlayTime
        {
            get => TimeSpan.FromSeconds(
                Bass.ChannelBytes2Seconds(_sound.Handle, Bass.ChannelGetLength(_sound.Handle)));
            set => throw new NotImplementedException("This Property is Non-ImplementedException");
        }

        public Device Device
        {
            get => _device;
            set
            {
                if (Bass.ChannelSetDevice(_sound.Handle, value.Handle))
                {
                    _device = value;
                }
                else
                    throw new BassException();
            }
        }
            
        public Sound Sound
        {
            get => _sound;
            set
            {
                if (State != PlaybackState.Stopped)
                {
                    Stop();
                }
                _sound = value;
                GC.Collect();
            }
        }
        
        #endregion

        #region Event

        public void SetEndedEvent(Action action)
        {
            SyncProcedure proc = new((handle, channel, data, user) => action());
            Bass.ChannelSetSync(_sound.Handle, SyncFlags.End, 0, proc);
        }

        #endregion

    }
}