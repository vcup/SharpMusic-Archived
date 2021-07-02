using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using ManagedBass;
using TagLib.Id3v2;

namespace SharpMusic.Backend.Play.BassManaged
{
    public class Device
    {
        #region Static Field

        private static readonly int DefaultDevice = 0;
        private static readonly int DefaultRecord = 0;
        private static readonly HashSet<Device> Devices = new();
        private static readonly HashSet<Device> RecordDevices = new();

        #endregion

        static Device()
        {
            for (int i = 0; Bass.GetDeviceInfo(i, out DeviceInfo info); i++)
            {
                Devices.Add(new(i));
                if (info.IsDefault)
                {
                    DefaultDevice = i;
                }
            }

            for (int i = 0; Bass.RecordGetDeviceInfo(i, out DeviceInfo info); i++)
            {
                RecordDevices.Add(new(i));
                if (info.IsDefault)
                {
                    DefaultRecord = i;
                }
            }
        }

        #region Static Method

        public static Device GetDefaultDevice => Devices.First(d => d._handle == DefaultDevice);
        
        public static Device GetDefaultRecord => Devices.First(d => d._handle == DefaultRecord);
        public static IEnumerable<Device> DeviceEnumerable() => Devices;

        public static IEnumerable<Device> RecordDevicesEnumerable() => RecordDevices;
        
        #endregion

        private bool _initialized; // default false;
        private readonly int _handle;
        private Device(int handle)
        {
            _handle = handle;
        }
        

        #region Property

        public int Handle
        {
            get
            {
                if (!_initialized)
                {
                    if (!(_initialized = Bass.Init(_handle)))
                    {
                        throw new BassException();
                    }
                }

                return _handle;
            }
        }
        private DeviceInfo DeviceInfo => Bass.GetDeviceInfo(Handle);

        public string Description => DeviceInfo.Name;
        public DeviceType Type => DeviceInfo.Type;
        public bool IsDefault => DeviceInfo.IsDefault;
        public string Driver => DeviceInfo.Driver;
        public bool IsEnable => DeviceInfo.IsEnabled;
        public bool IsLoop => DeviceInfo.IsLoopback;

        #endregion
    }
}