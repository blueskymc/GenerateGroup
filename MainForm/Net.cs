using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MainForm
{
    public class Net
    {
        public char[] HostName { get; set; }//20
        public char[] HostIpAddress { get; set; }//20
        public int NetNum { get; set; }
        public List<NetStruct> NetList;

        public Net(BinaryReader reader)
        {
            NetList = new List<NetStruct>();
            this.HostName = reader.ReadChars(20);
            this.HostIpAddress = reader.ReadChars(20);
            this.NetNum = reader.ReadInt32();
            for (int i = 0; i < this.NetNum; i++)
            {
                NetStruct net = new NetStruct();
                NetList.Add(net.ToNetStruct(reader.ReadBytes(36)));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            MemoryStream output = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(this.HostName);
            writer.Write(this.HostIpAddress);
            writer.Write(this.NetNum);
            foreach (NetStruct net in NetList)
            {
                writer.Write(net.GetBytes());
            }
            writer.Close();
            output.Close();
            return output.ToArray();
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public Net ToObject(BinaryReader reader)
        {
            NetList = new List<NetStruct>();
            this.HostName = reader.ReadChars(20);
            this.HostIpAddress = reader.ReadChars(20);
            this.NetNum = reader.ReadInt32();
            for (int i = 0; i < this.NetNum; i++)
            {
                NetStruct net = new NetStruct();
                NetList.Add(net.ToNetStruct(reader.ReadBytes(36)));
            }
            return this;
        }
    }
    public struct NetStruct
    {
        public char[] Type { get; set; }//4
        public byte[] Desc { get; set; }//16
        public int NetPort { get; set; }
        public int PvaSize { get; set; }
        public int PvdSize { get; set; }
        public int PviSize { get; set; }

        public byte[] GetBytes()
        {
            MemoryStream output = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(this.Type);
            writer.Write(this.Desc);
            writer.Write(this.NetPort);
            writer.Write(this.PvaSize);
            writer.Write(this.PvdSize);
            writer.Write(this.PviSize);
            return output.ToArray();
        }
        public NetStruct ToNetStruct(byte[] infos)
        {
            MemoryStream input = new MemoryStream(infos);
            BinaryReader reader = new BinaryReader(input);
            this.Type = reader.ReadChars(4);
            this.Desc = reader.ReadBytes(16);
            this.NetPort = reader.ReadInt32();
            this.PvaSize = reader.ReadInt32();
            this.PvdSize = reader.ReadInt32();
            this.PviSize = reader.ReadInt32();

            reader.Close();
            input.Close();
            return this;
        }
    };
}
