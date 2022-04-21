/* 
 * Copyright(C)AIOI SYSTEMS CO., LTD.
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace AioiSystems.SmartCard
{
    public class NfcAdapter
    {
        /// <summary>
        /// Represents a reader/writer.
        /// </summary>
        public NfcAdapter()
        {
        }
        
        protected static readonly int RECV_BUFF_SIZE = 256;
        public static readonly int BLOCK_SIZE = 16;

        private int _protocol = 0;
        private int _cardHandle = 0;
        private PcScModules.SCARD_IO_REQUEST _pioSendRequest;
        private PcScModules.SCARD_READERSTATE _readerState;
        private int _context;
        private string _name;

        protected byte[] _idm = null;
        protected byte[] _recvBuff = new byte[RECV_BUFF_SIZE];

        private int _retryCount = 3;

        public int Context
        {
            get { return _context; }
            set { _context = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Sets or gets the retry count.
        /// </summary>
        public int RetryCount
        {
            get { return _retryCount; }
            set { _retryCount = value; }
        }


        /// <summary>
        /// Polls a smart tag.
        /// </summary>
        public bool Poll()
        {
            _readerState.RdrName = _name;
            _readerState.RdrCurrState = PcScModules.SCARD_STATE_UNAVAILABLE;

            int ret = PcScModules.SCardGetStatusChange(_context, 100, ref _readerState, 1);
            if (ret != PcScModules.SCARD_S_SUCCESS)
                return false;

            if ((_readerState.RdrEventState & PcScModules.SCARD_STATE_PRESENT) != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Opens the touched smart-tag.
        /// </summary>
        public void OpenCard()
        {
            int ret = PcScModules.SCardConnect(
                _context, _name, PcScModules.SCARD_SHARE_SHARED,
                PcScModules.SCARD_PROTOCOL_T0 | PcScModules.SCARD_PROTOCOL_T1,
                ref _cardHandle, ref _protocol);

            if (ret != PcScModules.SCARD_S_SUCCESS)
            {
                throw new Win32Exception(ret);
            }

            _pioSendRequest.dwProtocol = _protocol;
            _pioSendRequest.cbPciLength = 8;
        }

        /// <summary>
        /// Closes the smart-tag.
        /// </summary>
        public int Close()
        {
            return PcScModules.SCardDisconnect(_cardHandle, PcScModules.SCARD_LEAVE_CARD);
        }

        
        /// <summary>
        /// Sends the command to the r/w.
        /// </summary>
        public byte[] SendCommand(byte[] command)
        {
            int recvLen = 255; 

            int retryCount = _retryCount;
            for (int i = 0; i < retryCount + 1; i++)
            {
                Console.WriteLine("S> {0}", BitConverter.ToString(command));

                int ret = PcScModules.SCardTransmit(
                    _cardHandle, ref _pioSendRequest, ref command[0], command.Length,
                    ref _pioSendRequest, ref _recvBuff[0], ref recvLen);

                if (ret != PcScModules.SCARD_S_SUCCESS)
                {
                    if (i == retryCount)
                    {
                        throw new Win32Exception(ret);
                    }
                }
                else
                {
                    break;
                }
                System.Threading.Thread.Sleep(100);
                Console.WriteLine("send retry");
            }
            byte[] response = new byte[recvLen];
            Array.Copy(_recvBuff, response, recvLen);

            Console.WriteLine("R> {0}", BitConverter.ToString(response));
           
            return response;
        }

        /// <summary>
        /// Sends the command to write.(WWE)
        /// </summary>
        public void Write(byte[] idm, byte[] command, byte[] blockList)
        {
            byte[] cFelica = FelicaCommand.CreatePacketForWrite(idm, command, true, blockList);
            byte[] cApdu = CreateTransmitApdu(cFelica);
            byte[] res = SendCommand(cApdu);
            int len = CheckTransmitResponse(res);
        }

        /// <summary>
        /// Sends the command to read.(RWE)
        /// </summary>
        public byte[] Read(byte[] idm, int blocks, byte[] blockList)
        {
            byte[] cFelica = FelicaCommand.CreatePacketForRead(idm, blocks, true, blockList);
            byte[] cApdu = CreateTransmitApdu(cFelica);
            byte[] rApdu = SendCommand(cApdu);
            int len = CheckTransmitResponse(rApdu);

            byte[] blockData = null;
            if (rApdu.Length > 2)
            {
                byte[] rFelica = new byte[rApdu.Length - 2];
                Array.Copy(rApdu, 0, rFelica, 0, rApdu.Length - 2);

                blockData = FelicaCommand.GetBlockData(rFelica);
            }
            return blockData;
        }

        /// <summary>
        /// Reads the IDm from the smart-tag.
        /// </summary>
        public byte[] GetIdm()
        {
            byte[] apdu = new byte[]{
                0xFF, 0x00, 0x00, 0x00, 0x06,
                0x06, 0x00, 0xff, 0xff, 0x00, 0x00
            };

            byte[] res;
            int len;

            res = SendCommand(apdu);
            len = CheckTransmitResponse(res);

            if (res.Length >= 10 && res[1] == 0x01)
            {
                byte[] idm = new byte[8];
                Array.Copy(res, 2, idm, 0, idm.Length);
                return idm;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Opens the adapter.
        /// </summary>
        public void OpenAdapter()
        {
            ;
        }

        /// <summary>
        /// Closes the adapter.
        /// </summary>
        public void CloseAdapter()
        {
            ;
        }

        private byte[] CreateTransmitApdu(byte[] sendData)
        {
            byte[] cApdu = new byte[5 + sendData.Length];
            cApdu[0] = 0xFF;
            cApdu[1] = 0x00;
            cApdu[2] = 0x00;
            cApdu[3] = 0x00;
            cApdu[4] = (byte)(sendData.Length);
            Array.Copy(sendData, 0, cApdu, 5, sendData.Length);

            return cApdu;
        }

        protected int CheckTransmitResponse(byte[] response)
        {
            if (response == null)
            {
                throw new System.IO.IOException();
            }

            if (response.Length >= 2)
            {
                if (response[response.Length - 2] != 0x90
                    || response[response.Length - 1] != 0x00)
                    throw new System.IO.IOException();

                return response.Length;
            }
            else
            {
                throw new System.IO.IOException();
            }
        }
    }
}
