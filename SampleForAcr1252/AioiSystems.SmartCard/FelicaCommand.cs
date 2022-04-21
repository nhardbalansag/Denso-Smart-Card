/* 
 * Copyright(C)AIOI SYSTEMS CO., LTD.
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace AioiSystems.SmartCard
{
    public static class FelicaCommand
    {
        private static readonly int BLOCK_SIZE = 16;

        public static byte[] CreatePacketForWrite(byte[] idm, byte[] blockData)
        {
            return CreatePacketForWrite(idm, blockData, true, null);
        }

        /// <summary>
        /// Creates a FeliCa command for writing.
        /// </summary>
        /// <param name="blocks"></param>
        /// <param name="blockData"></param>
        /// <param name="addLength"></param>
        /// <returns></returns>
        public static byte[] CreatePacketForWrite(byte[] idm, byte[] blockData, bool addLength, byte[] blockList)
        {
            int blocks = (blockData.Length + BLOCK_SIZE - 1) / BLOCK_SIZE;
            
            //3-bytes block list
            if (blockList == null)
            {
                blockList = new byte[blocks * 3];
                for (int i = 0; i < blocks; i++)
                {
                    blockList[i * 3] = (byte)0x00;
                    blockList[i * 3 + 1] = (byte)i;
                    blockList[i * 3 + 2] = (byte)0x04;
                }
            }
            int len = 13 + blockList.Length + BLOCK_SIZE * blocks;

            if (addLength)
                len++;
            byte[] packet = new byte[len];
            int pos = 0;
            if (addLength)
            {
                packet[0] = (byte)len;
                pos = 1;
            }
            packet[pos] = (byte)0x08;
            pos++;

            //IDm
            Array.Copy(idm, 0, packet, pos, 8);
            pos += 8;
            //service count
            packet[pos] = 0x01;
            pos++;
            //service code
            packet[pos] = 0x09;
            packet[pos + 1] = 0x00;
            pos += 2;
            //block count
            packet[pos] = (byte)blocks;
            pos++;

            //block list
            for (int i = 0; i < blockList.Length; i++)
            {
                packet[pos] = blockList[i];
                pos ++;
            }
            
            //block data
            Array.Copy(blockData, 0, packet, pos, blockData.Length);
            return packet;
        }

        public static byte[] CreatePacketForRead(byte[] idm, int blocks)
        {
            return CreatePacketForRead(idm, blocks, true, null);
        }

        /// <summary>
        /// Creates a FeliCa command for reading.
        /// </summary>
        /// <param name="blocks"></param>
        /// <param name="addLength"></param>
        /// <returns></returns>
        public static byte[] CreatePacketForRead(byte[] idm, int blocks, bool addLength, byte[] blockList)
        {
            //3-bytes block list
            if (blockList == null)
            {
                blockList = new byte[blocks * 3];
                for (int i = 0; i < blocks; i++)
                {
                    blockList[i * 3] = (byte)0x00;
                    blockList[i * 3 + 1] = (byte)i;
                    blockList[i * 3 + 2] = (byte)0x04;
                }
            }
            int len = 13 + blockList.Length;
            if (addLength)
                len++;
            byte[] packet = new byte[len];
            int pos = 0;
            if (addLength)
            {
                packet[0] = (byte)len;
                pos = 1;
            }
            packet[pos] = (byte)0x06;
            pos++;
            //IDm
            Array.Copy(idm, 0, packet, pos, 8);
            pos += 8;
            //service count
            packet[pos] = 0x01;
            pos++;
            //service code
            packet[pos] = 0x09;
            packet[pos + 1] = 0x00;
            pos += 2;
            //block count
            packet[pos] = (byte)blocks;
            pos++;
            //block list
            for (int i = 0; i < blockList.Length; i++)
            {
                packet[pos] = blockList[i];
                pos++;
            }

            return packet;
        }

        public static byte[] GetBlockData(byte[] response)
        {
            return GetBlockData(response, true);
        }

        /// <summary>
        /// Gets a block-data from a FeliCa response packet.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="withLength"></param>
        /// <returns></returns>
        public static byte[] GetBlockData(byte[] response, bool withLength)
        {
            int minLen = 12;
            if (withLength)
                minLen = 13;

            if (response.Length < minLen)
                return null;

            int blockCount = response[minLen - 1];
            byte[] blockData = new byte[blockCount * BLOCK_SIZE];
            if (response.Length < minLen + blockData.Length)
                return null;

            Array.Copy(response, minLen, blockData, 0, blockData.Length);

            return blockData;
        }

        public static byte[] GetPollingCommand()
        {
            return GetPollingCommand(true);
        }

        /// <summary>
        /// Gets a polling command for FeliCa.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetPollingCommand(bool addLength)
        {
            byte[] command = null;
            if (addLength)
            {
                command = new byte[]{
                    0x06, 0x00, 0xff, 0xff, 0x00, 0x00
                };
            }
            else
            {
                command = new byte[]{
                    0x00, 0xff, 0xff, 0x00, 0x00
                };
            }
            
            return command;
        }
    }
}
