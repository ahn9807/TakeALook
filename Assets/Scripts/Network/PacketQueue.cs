using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

public class PacketQueue
{
    //패킷 저장 정보 
    struct PacketInfo {
        public int offset;
        public int size;
    }

    private MemoryStream streamBuffer;
    private List<PacketInfo> offsetList;
    private int offset = 0;

    public PacketQueue() {
        streamBuffer = new MemoryStream();
        offsetList = new List<PacketInfo>();
    }

    public int Enqueue(byte[] data, int size) {
        PacketInfo info = new PacketInfo();

        info.offset = offset;
        info.size = size;

        offsetList.Add(info);

        streamBuffer.Position = offset;
        streamBuffer.Write(data, 0, size);
        streamBuffer.Flush();
        offset += size;

        return size;
    }

    public int Dequeue(ref byte[] buffer, int size)
    {
        if (offsetList.Count <= 0)
        {
            return -1;
        }

        PacketInfo info = offsetList[0];

        int dataSize = Math.Min(size, info.size);
        streamBuffer.Position = info.offset;
        int recvSize = streamBuffer.Read(buffer, 0, dataSize);

        if (recvSize > 0)
        {
            offsetList.RemoveAt(0);
        }

        if (offsetList.Count == 0)
        {
            Clear();
            offset = 0;
        }

        return recvSize;

    }
    public void Clear() 
    {
        byte[] buffer = streamBuffer.GetBuffer();
        Array.Clear(buffer, 0, buffer.Length);

        streamBuffer.Position = 0;
        streamBuffer.SetLength(0);

    }
}