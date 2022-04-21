/* 
 * Copyright(C)AIOI SYSTEMS CO., LTD.
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace AioiSystems.SmartCard
{
    public class PcScModules
    {
        #region winscard.dll

        public const int SCARD_SCOPE_USER = 0;
        public const int SCARD_S_SUCCESS = 0;

        public const int SCARD_SHARE_EXCLUSIVE = 1;
        public const int SCARD_SHARE_SHARED = 2;
        public const int SCARD_SHARE_DIRECT = 3;

        public const int SCARD_LEAVE_CARD = 0;   
        public const int SCARD_RESET_CARD = 1;  
        public const int SCARD_UNPOWER_CARD = 2;   
        public const int SCARD_EJECT_CARD = 3;

        public const int SCARD_PROTOCOL_UNDEFINED = 0x00;          
        public const int SCARD_PROTOCOL_T0 = 0x01;                
        public const int SCARD_PROTOCOL_T1 = 0x02;                 
        public const int SCARD_PROTOCOL_RAW = 0x10000;            

        public const int SCARD_STATE_UNKNOWN = 0x04;
        public const int SCARD_STATE_UNAVAILABLE = 0x08;
        public const int SCARD_STATE_EMPTY = 0x10;
        public const int SCARD_STATE_PRESENT = 0x20;
        public const int SCARD_STATE_ATRMATCH = 0x40;
        public const int SCARD_STATE_EXCLUSIVE = 0x80;
        public const int SCARD_STATE_INUSE = 0x100;
        public const int SCARD_STATE_MUTE = 0x200;
        public const int SCARD_STATE_UNPOWERED = 0x400;

        [StructLayout(LayoutKind.Sequential)]
        public struct SCARD_IO_REQUEST
        {
            public int dwProtocol;
            public int cbPciLength;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct APDURec
        {
            public byte bCLA;
            public byte bINS;
            public byte bP1;
            public byte bP2;
            public byte bP3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] Data;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] SW;
            public bool IsSend;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SCARD_READERSTATE
        {
            public string RdrName;
            public int UserData;
            public int RdrCurrState;
            public int RdrEventState;
            public int ATRLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 37)]
            public byte[] ATRValue;
        }

        

        [DllImport("winscard.dll")]
        public static extern int SCardEstablishContext(int dwScope, int pvReserved1, int pvReserved2, ref int phContext);

        [DllImport("winscard.dll")]
        public static extern int SCardReleaseContext(int phContext);

        [DllImport("winscard.dll")]
        public static extern int SCardConnect(int hContext, string szReaderName, int dwShareMode, int dwPrefProtocol, ref int phCard, ref int ActiveProtocol);

        [DllImport("winscard.dll")]
        public static extern int SCardBeginTransaction(int hCard);

        [DllImport("winscard.dll")]
        public static extern int SCardDisconnect(int hCard, int Disposition);

        [DllImport("winscard.dll")]
        public static extern int SCardListReaderGroups(int hContext, ref string mzGroups, ref int pcchGroups);

        [DllImport("winscard.DLL", EntryPoint = "SCardListReadersA", CharSet = CharSet.Ansi)]
        public static extern int SCardListReaders(
            int hContext,
            byte[] Groups,
            byte[] Readers,
            ref int pcchReaders
            );

        [DllImport("winscard.dll")]
        public static extern int SCardStatus(int hCard, string szReaderName, ref int pcchReaderLen, ref int State, ref int Protocol, ref byte ATR, ref int ATRLen);

        [DllImport("winscard.dll")]
        public static extern int SCardEndTransaction(int hCard, int Disposition);

        [DllImport("winscard.dll")]
        public static extern int SCardState(int hCard, ref uint State, ref uint Protocol, ref byte ATR, ref uint ATRLen);

        [DllImport("winscard.dll")]
        public static extern int SCardTransmit(int hCard, ref SCARD_IO_REQUEST pioSendRequest, ref byte SendBuff, int SendBuffLen, ref SCARD_IO_REQUEST pioRecvRequest, ref byte RecvBuff, ref int RecvBuffLen);

        [DllImport("winscard.dll")]
        public static extern int SCardControl(int hCard, uint dwControlCode, ref byte SendBuff, int SendBuffLen, ref byte RecvBuff, int RecvBuffLen, ref int pcbBytesReturned);

        [DllImport("winscard.dll")]
        public static extern int SCardGetStatusChange(int hContext, int TimeOut, ref  SCARD_READERSTATE ReaderState, int ReaderCount);

        #endregion

        /// <summary>
        /// Returns the PC/SC context.
        /// </summary>
        public static int GetContext(out int context)
        {
            context = 0;
            return SCardEstablishContext(
                SCARD_SCOPE_USER, 0, 0, ref context);
        }

        /// <summary>
        /// Returns the device name list.
        /// </summary>
        public static string[] GetAdapters(int context)
        {
            int pcchReaders = 0;
            int ret = SCardListReaders(
                context, null, null, ref pcchReaders);

            if (ret != SCARD_S_SUCCESS)
                return null;

            byte[] buffer = new byte[pcchReaders];
            ret = SCardListReaders(
                context, null, buffer, ref pcchReaders);

            if (ret != SCARD_S_SUCCESS)
                return null;

            string nameSerial = Encoding.ASCII.GetString(buffer);
            return nameSerial.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Releases the context.
        /// </summary>
        public static void ReleaseContext(int context)
        {
            SCardReleaseContext(context);
        }

    }
}
