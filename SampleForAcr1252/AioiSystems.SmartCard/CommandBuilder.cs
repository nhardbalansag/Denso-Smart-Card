/* 
 * Copyright(C)AIOI SYSTEMS CO., LTD.
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace AioiSystems.SmartCard
{
    public class CommandBuilder
    {
        public CommandBuilder()
        {
            _secCode1 = new byte[] { 0x30, 0x30, 0x30 };
            _secCode2 = new byte[] { 0x30, 0x30, 0x30 };
            _secCode3 = new byte[] { 0x30, 0x30, 0x30 };
        }

        public const byte SECURITY_CODE_TYPE1 = 1;
        public const byte SECURITY_CODE_TYPE2 = 2;
        public const byte SECURITY_CODE_TYPE3 = 3;

        public const byte COMMAND_DATA_WRITE = (byte)0xB0;
        public const byte COMMAND_DATA_READ = (byte)0xC0;
        public const byte COMMAND_CHECK_STATUS = (byte)0xD0;
        public const byte COMMAND_CLEAR = (byte)0xA1;
        public const byte COMMAND_SHOW_DISPLAY_OLD = (byte)0xA0;
        public const byte COMMAND_SAVE_LAYOUT = (byte)0xB2;
        public const byte COMMAND_SHOW_DISPLAY = (byte)0xA2;
        public const byte COMMAND_SHOW_DISPLAY3 = (byte)0xA3;
        public const byte COMMAND_CHANGE_SECURITY_CODE = (byte)0xBD;

	    private int _maxBlocks = 8;
	    private byte _seq = 1;
        private byte[] _secCode1;
        private byte[] _secCode2;
        private byte[] _secCode3;

	    public int getMaxBlocks() {
		    return _maxBlocks;
	    }

	    public void setMaxBlocks(int maxBlocks) {
		    this._maxBlocks = maxBlocks;
	    }

        public void SetSecurityCode1(byte[] code)
        {
            _secCode1 = code;
        }
        public byte[] GetSecurityCode1()
        {
            return _secCode1;
        }

        public void SetSecurityCode2(byte[] code)
        {
            _secCode2 = code;
        }
        public byte[] GetSecurityCode2()
        {
            return _secCode2;
        }

        public void SetSecurityCode3(byte[] code)
        {
            _secCode3 = code;
        }
        public byte[] GetSecurityCode3()
        {
            return _secCode3;
        }

        /// <summary>
        /// Creates a command when there is no function data.
        /// </summary>
        public byte[] BuildCommand(byte functionNo,
            byte[] paramData)
        {
            List<byte[]> list = BuildCommand(functionNo,
                    paramData, null);
            if (list.Count == 0)
            {
                return null;
            }
            else
            {
                return list[0];
            }
        }

        /// <summary>
        /// Creates smart-tag command.
        /// Divides command into some frames if necessary.
        /// </summary>
        public List<byte[]> BuildCommand(byte functionNo,
            byte[] paramData,
            byte[] functionData)
        {
            int dataBlocks;
            byte[] innerData = null;
            int splitCount = 0;
            if (functionData == null)
            {
                dataBlocks = 0;
                splitCount = 1;
            }
            else
            {
                dataBlocks = functionData.Length / 16;
                if (functionData.Length % 16 > 0)
                {
                    dataBlocks++;
                    innerData = new byte[dataBlocks * 16];
                    Array.Copy(functionData, 0, innerData, 0, functionData.Length);
                }
                else
                {
                    innerData = functionData;
                }
                splitCount = GetSplitCount(dataBlocks);
            }

            List<byte[]> result = new List<byte[]>();
            int offset = 0;
            int frameBlocks = 0;
            for (int i = 0; i < splitCount; i++)
            {
                int dataLen;
                if (i == splitCount - 1)
                {
                    frameBlocks = GetLastBlockCount(dataBlocks) + 1;
                    
                    if (innerData == null)
                    {
                        dataLen = 0;
                    }
                    else
                    {
                        dataLen = functionData.Length - offset;
                    }
                }
                else
                {
                    frameBlocks = _maxBlocks;
                    dataLen = (frameBlocks - 1) * 16;
                }

                byte[] cmd = new byte[frameBlocks * 16];
                cmd[0] = functionNo;
                cmd[1] = (byte)splitCount;
                cmd[2] = (byte)(i + 1);
                cmd[3] = (byte)dataLen;
                if (functionNo == (byte)0xd0)
                {
                    cmd[4] = 0;
                }
                else
                {
                    cmd[4] = GetNextSeq();
                }

                //security code
                byte[] secCode = GetSecurityCode(functionNo, paramData);
                if (secCode != null)
                {
                    cmd[5] = secCode[0];
                    cmd[6] = secCode[1];
                    cmd[7] = secCode[2];
                }
                else
                {
                    cmd[5] = 0x30;
                    cmd[6] = 0x30;
                    cmd[7] = 0x30;
                }
                Array.Copy(paramData, 0, cmd, 8, paramData.Length);

                if (innerData != null)
                {
                    Array.Copy(innerData, offset, cmd, 16, dataLen);
                }
                
                result.Add(cmd);
                offset += dataLen;
            }
            return result;
        }

        /// <summary>
        /// Creates a command to write user data.
        /// Divides command into some frames if necessary.
        /// </summary>
        public List<byte[]> BuildDataWriteCommand(int startAdress, byte[] functionData)
        {
            int unit = _maxBlocks * 16 - 16;
            int splitCount = (functionData.Length + unit - 1) / unit;

            List<byte[]> result = new List<byte[]>();
            int offset = 0;
            int dataLen = (_maxBlocks - 1) * 16;    //data size without header
            int frameBlocks = 0;
            for (int i = 0; i < splitCount; i++)
            {
                if (i == splitCount - 1)
                {
                    //last block
                    dataLen = functionData.Length - offset;
                    frameBlocks = (dataLen + 15) / 16;
                    frameBlocks++;
                }
                else
                {
                    frameBlocks = _maxBlocks;
                }

                byte[] cmd = new byte[frameBlocks * 16];
                cmd[0] = COMMAND_DATA_WRITE;
                cmd[1] = (byte)splitCount;
                cmd[2] = (byte)(i + 1);
                cmd[3] = (byte)dataLen;
                cmd[4] = GetNextSeq();
                
                //security code
                if (_secCode2 != null)
                {
                    cmd[5] = _secCode2[0];
                    cmd[6] = _secCode2[1];
                    cmd[7] = _secCode2[2];
                }
                else
                {
                    cmd[5] = 0x30;
                    cmd[6] = 0x30;
                    cmd[7] = 0x30;
                }

                //Adress
                int adress = startAdress + offset;
                byte hAByte = (byte)(adress >> 8);
                byte lAByte = (byte)(adress & 0x00FF);

                //Length
                byte hLByte = (byte)(dataLen >> 8);
                byte lLByte = (byte)(dataLen & 0x00FF);

                byte[] paramData = new byte[]{
					hAByte,lAByte, hLByte, lLByte,
					0, 0, 0, 0
			    };
                //set function parameter data.
                Array.Copy(paramData, 0, cmd, 8, paramData.Length);
                //set function data.
                if (functionData != null)
                {
                    Array.Copy(functionData, offset, cmd, 16, dataLen);
                }
                result.Add(cmd);

                offset += dataLen;
            }
            return result;
        }

        public void SetSeq(byte seq)
        {
            this._seq = seq;
        }

        private int GetSplitCount(int totalBlocks)
        {
            int unit = _maxBlocks - 1;
            return (totalBlocks + unit - 1) / unit;
        }

        private int GetLastBlockCount(int dataBlocks)
        {
            if (dataBlocks == 0) 
                return 0;

            int mod = dataBlocks % (_maxBlocks - 1);
            if (mod == 0)
                return _maxBlocks - 1;
            else
                return mod;
        }

        private byte GetNextSeq()
        {
            byte result = _seq;
            if (_seq == 255)
                _seq = 1;
            else
                _seq++;
            
            return result;
        }

        private byte[] GetSecurityCode(byte functionNo, byte[] paramData)
        {
            byte[] code = null;
            switch (functionNo)
            {
                case COMMAND_SHOW_DISPLAY_OLD:
                case COMMAND_SHOW_DISPLAY:
                case COMMAND_SHOW_DISPLAY3:
                case COMMAND_CLEAR:
                case COMMAND_SAVE_LAYOUT:
                    code = _secCode1;
                    break;
                case COMMAND_DATA_WRITE:
                    code = _secCode2;
                    break;
                case COMMAND_DATA_READ:
                    code = _secCode3;
                    break;
                case COMMAND_CHANGE_SECURITY_CODE:
                    code = GetSecurityCodeByType(paramData[0]);
                    break;
                default:
                    code = new byte[] { 0x30, 0x30, 0x30 };
                    break;
            }
            return code;
        }

        private byte[] GetSecurityCodeByType(byte type)
        {
            switch (type)
            {
                case SECURITY_CODE_TYPE1:
                    return _secCode1;
                case SECURITY_CODE_TYPE2:
                    return _secCode2;
                case SECURITY_CODE_TYPE3:
                    return _secCode3;
            }
            return null;
        }

    }
}
