using System;
using System.Runtime.InteropServices;

namespace EnhancedPasswordWarning
{

    public sealed class AdsiUtils
    {
        private AdsiUtils() { }

        [
            ComImport,
            Guid("9068270b-0939-11d1-8be1-00c04fd8d503"),
            InterfaceType(ComInterfaceType.InterfaceIsIDispatch)
        ]
        private interface IADsLargeInteger
        {
            [DispId(2)] int HighPart { get; set; }
            [DispId(3)] int LowPart { get; set; }
        }

        public static DateTime AdsDateValue(object value)
        {
            try
            {
                long dV = AdsTicksValue(value);
                return DateTime.FromFileTime(dV);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static long AdsTicksValue(object value)
        {
            if (value == null) return 0;

            IADsLargeInteger v = value as IADsLargeInteger;
            if (v == null) return 0;

            return ((long)v.HighPart << 32) + (long)v.LowPart;
        }
    }

}