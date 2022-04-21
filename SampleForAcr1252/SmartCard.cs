/* 
 * Copyright(C)AIOI SYSTEMS CO., LTD.
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AioiSystems.SmartCard;
using AioiSystems.DotModule;

namespace SampleForAcr1252
{
    public class SmartCard
    {
        public SmartCard(NfcAdapter adapter)
        {
            _adapter = adapter;
            
            _builder = new CommandBuilder();
            _builder.setMaxBlocks(12);
        }

        public enum SmartCardFunctions
        {
            Nothing,
            ShowImage,
            ShowPartial,
            ClearDisplay,
            WriteUserArea,
            ReadUserArea,
            WriteToCardArea,
            ReadFromCardArea,
            SaveLayout,
            LoadLayout
        }

        public enum DisplayType
        {
            SmartTag20,
            SmartTag27,
            SmartCard29,
        }

        public static readonly byte TAG_STS_UNKNOWN = 0x70;
        public static readonly byte TAG_STS_INIT = 0;
        public static readonly byte TAG_STS_PROCESSED = 0xF0;
        public static readonly byte TAG_STS_BUSY = 0xF2;
        private static readonly int MAX_WRITE_SIZE = 512;
        private static readonly int BLOCK_SIZE = 16;

        protected CommandBuilder _builder = null;
        private NfcAdapter _adapter = null;
        private byte[] _idm = null;
        private byte _tagStatus = 0;
        private byte _battery = 0;
        private byte _version = 0;

        private ImageInfo _imageData = null;
        private byte[] _userData = null;

        private SmartCardFunctions _function;
        private DisplayType _targetDisplay;
        private int _layoutNo;

        public SmartCardFunctions SelectedFunction
        {
            get { return _function; }
            set { _function = value; }
        }

        public DisplayType TargetDisplay
        {
            get { return _targetDisplay; }
            set { _targetDisplay = value; }
        }

        public int LayoutNo
        {
            get { return _layoutNo; }
            set { _layoutNo = value; }
        }

        public NfcAdapter Adapter
        {
            get { return _adapter; }
        }

        public void SetIdm(byte[] idm)
        {
            _idm = idm;
        }

        public byte[] GetIdm()
        {
            return _idm;
        }

        public void SetImageData(ImageInfo imageData)
        {
            _imageData = imageData;
        }

        public void SetUserData(byte[] data)
        {
            _userData = data;
        }

        public byte[] GetUserData()
        {
            return _userData;
        }

        /// <summary>
        /// Indicates whether the smart-tag.
        /// </summary>
        /// <param name="idm"></param>
        /// <returns>True if the IDm is the smart-tag; otherwise, false.</returns>
        public static bool IsSmartCard(byte[] idm)
        {
            if (idm == null) return false;
            if (idm.Length < 8) return false;

            bool result = false;
            if (idm[0] == (byte)0x03
                    && idm[1] == (byte)0xFE
                    && idm[2] == (byte)0x00
                    && idm[3] == (byte)0x1D)    //SmartTag
            {
                result = true;
            }
            else if (idm[0] == (byte)0x02
                    && idm[1] == (byte)0xFE
                    && idm[2] == (byte)0x00
                    && idm[3] == (byte)0x00)    //SmartCard
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Starts the smart-tag process.
        /// </summary>
        public void StartProcess()
        {
            WaitForIdle();
            switch (this.SelectedFunction)
            {
                case SmartCardFunctions.ShowImage:
                    ShowImage(_imageData.GetImage(), 0, 0, _imageData.Width, _imageData.Height, 0, 0);
                    break;
                case SmartCardFunctions.ShowPartial:
                    ShowImage(_imageData.GetImage(), 65, 0, _imageData.Width, _imageData.Height, 3, 1); 
                    break;
                case SmartCardFunctions.ClearDisplay:
                    ClearDisplay();
                    break;
                case SmartCardFunctions.WriteUserArea:
                    WriteUserData(0, _userData);
                    break;
                case SmartCardFunctions.ReadUserArea:
                    _userData = ReadUserData(0, 3072);
                    break;
                case SmartCardFunctions.SaveLayout:
                    SaveLayout(this.LayoutNo);
                    break;
                case SmartCardFunctions.LoadLayout:
                    LoadLayout(this.LayoutNo);
                    break;
            }

            if (_targetDisplay == SmartCard.DisplayType.SmartCard29)
            {
                //wait for re-writing the display.
                WaitForIdle();

                //check the status.
                if (_tagStatus != TAG_STS_PROCESSED && _tagStatus != TAG_STS_BUSY)
                {
                    throw new System.IO.IOException();
                }
            }
        }

        /// <summary>
        /// Waits until smart-tag can do a next task.
        /// </summary>
        public void WaitForIdle()
        {
            for (int i = 0; i < 600; i++)
            {
                CheckStatus();
                
                if (_tagStatus != TAG_STS_BUSY
                        && _tagStatus != TAG_STS_UNKNOWN)
                {
                    return;
                }

                //Console.WriteLine("CheckStatus: retry");
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Confirms the smart-tag status.
        /// </summary>
        public void CheckStatus()
        {
            _tagStatus = TAG_STS_UNKNOWN;
            _version = 0x80;

            //send request
            byte[] paramData = new byte[]{
				0, 0, 0, 0,
				0, 0, 0, 0
		    };
            byte[] command = _builder.BuildCommand(CommandBuilder.COMMAND_CHECK_STATUS, paramData);
            SendCommand(command);

            Thread.Sleep(10); 

            //read block
            byte[] blockData = ReadData(2);
            if (blockData != null && blockData.Length > 15)
            {
                _tagStatus = blockData[3];
                _battery = blockData[5];
                _version = blockData[15];
                _builder.SetSeq((byte)(blockData[4] + 1));
            }
        }

        
        /// <summary>
        /// Sends a command to the smart-tag.
        /// </summary>
        private void SendCommand(byte[] command, byte[] blockList)
        {
            _adapter.Write(_idm, command, blockList);
        }

        private void SendCommand(byte[] command)
        {
            SendCommand(command, null);
        }

        /// <summary>
        /// Reads a command from the smart-tag.
        /// </summary>
        protected byte[] ReadData(int blocks, byte[] blockList)
        {
            return _adapter.Read(_idm, blocks, blockList);
        }

        protected byte[] ReadData(int blocks)
        {
            return ReadData(blocks, null);
        }

        /// <summary>
        /// Displays an image.
        /// </summary>
        /// <param name="imageData"></param>
        private void ShowImage(byte[] imageData,
            int x, int y, int width, int height,
            byte drawMode, byte layoutNo)
        {
            List<byte[]> list = null;
            byte[] paramData = null;

            if (this.TargetDisplay == DisplayType.SmartTag27
                || this.TargetDisplay == DisplayType.SmartCard29)
            {
                //2.7/2.9inch
                byte[] pos = ConvertTo3Bytes(x, y);
                byte[] size = ConvertTo3Bytes(width, height);
                byte mode = (byte)(drawMode << 4);
                mode |= 0x03;
                paramData = new byte[]{
                    pos[0], pos[1], pos[2], size[0], 
                    size[1], size[2], layoutNo, mode
                };
                list = _builder.BuildCommand(CommandBuilder.COMMAND_SHOW_DISPLAY3, paramData, imageData);
            }
            else
            {
                //2inch
                paramData = new byte[]{
                    (byte)x, (byte)y, (byte)width, (byte)height,
                    0, drawMode, layoutNo, 3
                };
                list = _builder.BuildCommand(CommandBuilder.COMMAND_SHOW_DISPLAY, paramData, imageData);
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    Thread.Sleep(40);
                }
                SendCommand(list[i]);
            }
        }

        /// <summary>
        /// Registers the last shown image to the smart-tag.
        /// </summary>
        private void SaveLayout(int layoutNo)
        {
            byte[] paramData = new byte[]{
				(byte)layoutNo, 0, 0, 0,
				0, 0, 0, 0
		    };

            byte[] command = _builder.BuildCommand(CommandBuilder.COMMAND_SAVE_LAYOUT, paramData);
            SendCommand(command);
        }

        /// <summary>
        /// Shows the registered image. 
        /// </summary>
        private void LoadLayout(int layoutNo)
        {
            byte[] paramData = new byte[]{
				0, 0, 0, 0,
				0, 3, (byte)layoutNo, 1
		    };
            byte[] command = _builder.BuildCommand(CommandBuilder.COMMAND_SHOW_DISPLAY, paramData);
            SendCommand(command);
        }

        /// <summary>
        /// Clears the display.
        /// </summary>
        private void ClearDisplay()
        {
            byte[] paramData = new byte[]{
				0, 0, 0, 0,
				0, 0, 0, 0
		    };
            byte[] command = _builder.BuildCommand(CommandBuilder.COMMAND_CLEAR, paramData);
            SendCommand(command);
        }

        /// <summary>
        /// Writes the user data to the free information area on the smart-tag.
        /// Divides and sends the command into some frames if necessary.
        /// </summary>
        private void WriteUserData(int startAddress, byte[] data)
        {
            int splitCount = (data.Length + MAX_WRITE_SIZE - 1) / MAX_WRITE_SIZE;

            int offset = 0;
            int dataLen = (data.Length <= MAX_WRITE_SIZE ? data.Length : MAX_WRITE_SIZE);

            for (int i = 0; i < splitCount; i++)
            {
                if (i == splitCount - 1)
                {   //last frame
                    dataLen = data.Length - offset;
                }
                byte[] framedata = new byte[dataLen];
                Array.Copy(data, offset, framedata, 0, dataLen);

                this.WriteUserDataByFrame(startAddress, framedata);

                offset += dataLen;
                startAddress += dataLen;

                Thread.Sleep(400);
                WaitForIdle();
            }
        }

        /// <summary>
        /// Writes the user data up to 512-bytes.
        /// </summary>
        private void WriteUserDataByFrame(int address, byte[] data)
        {
            List<byte[]> list = _builder.BuildDataWriteCommand(address, data);

            foreach (byte[] cmd in list)
            {
                SendCommand(cmd);
                Thread.Sleep(40);
            }
        }

        /// <summary>
        /// Reads the user data in the free information area on the smart-tag.
        /// </summary>
        public byte[] ReadUserData(int startAddress, int sizeToRead)
        {
            byte[] result = new byte[sizeToRead];
            int maxReadLength = _builder.getMaxBlocks() * BLOCK_SIZE - BLOCK_SIZE;
            int splitCount = (sizeToRead + maxReadLength - 1) / maxReadLength;
            int dataLen = (sizeToRead > maxReadLength ? maxReadLength : sizeToRead);
            int offset = 0;

            for (int i = 0; i < splitCount; i++)
            {
                if (i == splitCount - 1)
                {
                    dataLen = sizeToRead - offset; //last frame
                }
                
                byte[] data = ReadUserDataByBlock(startAddress, dataLen);
                Array.Copy(data, 0, result, offset, dataLen);

                offset += dataLen;
                startAddress += dataLen;
            }
            return result;
        }

        /// <summary>
        /// Reads the user data in the free information area on the smart-tag.
        /// (Maximum concurrent transfer block number below)
        /// </summary>
        private byte[] ReadUserDataByBlock(int readPos, int readSize)
        {
            //Address
            byte hAByte = (byte)(readPos >> 8);
            byte lAByte = (byte)(readPos & 0x00FF);

            //Length
            byte hLByte = (byte)(readSize >> 8);
            byte lLByte = (byte)(readSize & 0x00FF);

            //Sends request for read.
            byte[] paramData = new byte[]{
			    hAByte, lAByte, hLByte, lLByte,
			    0, 0, 0, 0
		    };

            byte[] command = _builder.BuildCommand(CommandBuilder.COMMAND_DATA_READ, paramData);

            SendCommand(command);
            Thread.Sleep(40);
            
            //Reads data
            int blocks = (readSize + BLOCK_SIZE - 1) / BLOCK_SIZE;

            byte[] data = ReadData(blocks + 1);

            byte[] userData = new byte[readSize];
            Array.Copy(data, 16, userData, 0, readSize);

            return userData;
        }

        /// <summary>
        /// Writes the data to card memory area.
        /// </summary>
        /// <param name="blockIndex">first block index (0-26)</param>
        /// <param name="blockData">data to write</param>
        public void WriteToCardArea(int blockIndex, byte[] data)
        {
            int maxBlocks = _builder.getMaxBlocks();
            int blocks = (data.Length + BLOCK_SIZE - 1) / BLOCK_SIZE;
            byte[] blockData = new byte[blocks * BLOCK_SIZE];
            Array.Copy(data, 0, blockData, 0, data.Length);

            int frames = (blocks + maxBlocks - 1) / maxBlocks;
            for (int i = 0; i < frames; i++)
            {
                int frameBlocks;    
                if (i == frames - 1)
                {
                    int mod = blocks % maxBlocks;
                    if (mod == 0)
                        frameBlocks = maxBlocks;
                    else
                        frameBlocks = mod;
                }
                else
                {
                    frameBlocks = maxBlocks;
                }
                byte[] command = new byte[frameBlocks * BLOCK_SIZE];
                Array.Copy(blockData, i * maxBlocks * 16, command, 0, command.Length);

                byte[] blockList = new byte[frameBlocks * 2];
                for (int j = 0; j < frameBlocks; j++)
                {
                    blockList[j * 2] = 0x80;
                    blockList[j * 2 + 1] = (byte)(j + i * maxBlocks + blockIndex);
                }
                SendCommand(command, blockList);
            }
        }

        /// <summary>
        /// Reads the data from card memory area.
        /// </summary>
        /// <param name="blockIndex">first block index (0-26)</param>
        /// <param name="blocks">block count (1～27)</param>
        /// <returns>read data</returns>
        public byte[] ReadFromCardArea(int blockIndex, int blocks)
        {
            byte[] readData = new byte[blocks * BLOCK_SIZE];
            int index = 0;
            int maxBlocks = _builder.getMaxBlocks();

            int frames = (blocks + maxBlocks - 1) / maxBlocks;
            for (int i = 0; i < frames; i++)
            {
                int frameBlocks;   
                if (i == frames - 1)
                {
                    int mod = blocks % maxBlocks;
                    if (mod == 0)
                        frameBlocks = maxBlocks;
                    else
                        frameBlocks = mod;
                }
                else
                {
                    frameBlocks = maxBlocks;
                }

                byte[] blockList = new byte[frameBlocks * 2];
                for (int j = 0; j < frameBlocks; j++)
                {
                    blockList[j * 2] = 0x80;
                    blockList[j * 2 + 1] = (byte)(j + i * maxBlocks + blockIndex);
                }
                byte[] data = ReadData(frameBlocks, blockList);
                if (data != null)
                {
                    Array.Copy(data, 0, readData, index, data.Length);
                    index += data.Length;
                }
            }
            return readData;
        }

        /// <summary>
        /// Converts two 12-bit numbers to the 3-bytes array.
        /// </summary>
        private static byte[] ConvertTo3Bytes(int a, int b)
        {
            byte[] result = new byte[3];
            result[0] = (byte)((a & 0x000FFF) >> 4);

            byte wk1 = (byte)((a & 0x0000000F) << 4); 
            wk1 |= (byte)((b & 0x00000F00) >> 8);
            result[1] = wk1;

            result[2] = (byte)(b & 0x000000FF);

            return result;
        }
    }
}
