/*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Runtime.InteropServices;

namespace Tizen.Tapi
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MiscVersionInfoStruct
    {
        internal byte Mask;
        [MarshalAs(UnmanagedType.LPStr)]
        internal string SwVersion;
        [MarshalAs(UnmanagedType.LPStr)]
        internal string HwVersion;
        [MarshalAs(UnmanagedType.LPStr)]
        internal string CalDate;
        [MarshalAs(UnmanagedType.LPStr)]
        internal string ProductCode;
        [MarshalAs(UnmanagedType.LPStr)]
        internal string ModelId;
        internal byte PrlNam;
        [MarshalAs(UnmanagedType.LPStr)]
        internal string PrlVersion;
        internal byte EriNam;
        [MarshalAs(UnmanagedType.LPStr)]
        internal string EriVersion;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MiscSerialNumInfoStruct
    {
        [MarshalAs(UnmanagedType.LPStr)]
        internal string Esn;
        [MarshalAs(UnmanagedType.LPStr)]
        internal string MeId;
        [MarshalAs(UnmanagedType.LPStr)]
        internal string Imei;
        [MarshalAs(UnmanagedType.LPStr)]
        internal string ImeiSv;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MiscDeviceInfoStruct
    {
        [MarshalAs(UnmanagedType.LPStr)]
        internal string Vendor;
        [MarshalAs(UnmanagedType.LPStr)]
        internal string Device;
    }

    internal class ModemStructConversions
    {
        internal static MiscVersionInformation ConvertVersionStruct(MiscVersionInfoStruct infoStruct)
        {
            MiscVersionInformation versionInfo = new MiscVersionInformation();
            versionInfo.CalcDate = infoStruct.CalDate;
            versionInfo.EriNamNum = infoStruct.EriNam;
            versionInfo.EriVers = infoStruct.EriVersion;
            versionInfo.HwVers = infoStruct.HwVersion;
            versionInfo.SwVers = infoStruct.SwVersion;
            versionInfo.PrlNamNum = infoStruct.PrlNam;
            versionInfo.PrlVers = infoStruct.PrlVersion;
            versionInfo.ProdCode = infoStruct.ProductCode;
            versionInfo.Version = infoStruct.Mask;
            versionInfo.Model = infoStruct.ModelId;
            return versionInfo;
        }

        internal static MiscSerialNumberInformation ConvertSerialNumberStruct(MiscSerialNumInfoStruct infoStruct)
        {
            MiscSerialNumberInformation serialNumberInfo = new MiscSerialNumberInformation();
            serialNumberInfo.SzEsn = infoStruct.Esn;
            serialNumberInfo.SzImei = infoStruct.Imei;
            serialNumberInfo.SzImeiSv = infoStruct.ImeiSv;
            serialNumberInfo.SzMeid = infoStruct.MeId;
            return serialNumberInfo;
        }

        internal static MiscDeviceInfo ConvertMiscInfoStruct(MiscDeviceInfoStruct infoStruct)
        {
            MiscDeviceInfo deviceInfo = new MiscDeviceInfo();
            deviceInfo.Vendor = infoStruct.Vendor;
            deviceInfo.Device = infoStruct.Device;
            return deviceInfo;
        }
    }
}
