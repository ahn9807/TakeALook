using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class TrasportUDP : MonoBehaviour
{
    private Socket socket;
    private Thread thread;
    private bool isStarted = false;
    private bool isServer = false;
    private bool isConnected = false;
    private IPEndPoint remoteEndPoint;
    private PacketQueue sendQueue;
    private PacketQueue recvQueue;
    //최대 패킷 사이즈
    private const int packetSize = 1400;
    //타임 아웃 변수들
    private const int timeOutSec = 5;
    private DateTime ticker;
    //이벤트 통지 
    public delegate void EventHandler(NetEventState state);
    private EventHandler handler;

    private void Start()
    {
        sendQueue = new PacketQueue();
        recvQueue = new PacketQueue();
    }

    private void OnApplicationQuit()
    {
        if(isStarted == true) {
            StopServer();
        }
    }

    public bool StartServer(int port) {
        Debug.Log("Start server called[Port:" + port + "]");

        try {
            if(socket == null) {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            }
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }
        catch {
            return false;
        }

        isServer = true;

        return LaunchThread();
    }

    public void StopServer() {
        isStarted = false;
        if(thread != null) {
            thread.Join();
            thread = null;
        }

        Disconnect();

        if(socket != null) {
            socket.Close();
            socket = null;
        }

        isServer = false;
        isStarted = false;

        Debug.Log("Server stopped.");
    }

    public bool Connect(string address, int port) {
        Debug.Log("TransportUdp::Connect called.[Port:" + port + "]");

        try {
            if(socket == null) {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            }
            socket.Connect(address, port);
        }
        catch {
            Debug.Log("TransportUdp::Connect failed");
            return false;
        }

        bool ret = true;
        if(isStarted == false) {
            ret = LaunchThread();
            if(ret == true) {
                isConnected = true;
            }
        }

        if(handler != null) {
            NetEventState state = new NetEventState();
            state.type = NetEventType.Connect;
            state.result = NetEventResult.Success;
            handler(state);
        }

        Debug.Log("TransportUdp::Connect success.");

        return ret;
    }

    public bool Disconnect() {
        if(socket != null) {
            socket.Close();
            socket = null;

            if(handler != null) {
                NetEventState state = new NetEventState();
                state.type = NetEventType.Disconnect;
                state.result = NetEventResult.Success;
                handler(state);
            }
        }

        isStarted = false;
        isConnected = false;
        Debug.Log("TransportTcp::Disconnect called.");

        return true;
    }

    public int Send(byte[] data, int size) {
        return sendQueue.Enqueue(data, size);
    }

    public int Receive(ref byte[] buffer, int size) {
        return recvQueue.Dequeue(ref buffer, size);
    }

    public bool IsConnected() {
        return isConnected;
    }

    public bool IsServer() {
        return isServer;
    }

    public IPEndPoint GetRemoteEndPoint() {
        return remoteEndPoint;
    }

    public void RegisterEventHandler(EventHandler handler) {
        handler += handler;
    }

    public void UnregisterEventHandler(EventHandler handler) {
        if (handler != null)
        {
            handler -= handler;
        }
    }

    bool LaunchThread() {
        try {
            thread = new Thread(new ThreadStart(Dispatch));
            thread.Start();
        }
        catch {
            Debug.Log("Cannot launch thread");
            return false;
        }

        isStarted = true;
        return true;
    }

    void Dispatch() {
        while(isStarted == true) {
            AcceptClient();

            if(socket != null) {
                // 송신 처리 
                DispatchSend();
                // 수신 처리 
                DispatchReceive();
                // 타임 아웃 처리
                CheckTimeout();
            }

            Thread.Sleep(3);
        }
    }

    void AcceptClient() {
        if(isConnected == false && socket != null && socket.Poll(0, SelectMode.SelectRead)) {
            isConnected = true;
            ticker = DateTime.Now;

            if(handler != null) {
                NetEventState state = new NetEventState();
                state.type = NetEventType.Connect;
                state.result = NetEventResult.Success;
                handler(state);
            }
        }
    }

    void DispatchSend() {
        if(socket == null) {
            return;
        }

        try {
            if(socket.Poll(0, SelectMode.SelectWrite)) {
                byte[] buffer = new byte[packetSize];

                int sendSize = sendQueue.Dequeue(ref buffer, buffer.Length);
                while(sendSize > 0) {
                    int res = socket.Send(buffer, sendSize, SocketFlags.None);
                    sendSize = sendQueue.Dequeue(ref buffer, buffer.Length);
                }
            }
        }
        catch {
            return;
        }
    }

    void DispatchReceive() {
        if(socket == null) {
            return;
        }

        try {
            while(socket.Poll(0,SelectMode.SelectRead)) {
                byte[] buffer = new byte[packetSize];

                int recvSize = socket.Receive(buffer, buffer.Length, SocketFlags.None);

                if(recvSize == 0) {
                    Disconnect();
                }
                else if(recvSize > 0) {
                    recvQueue.Enqueue(buffer, recvSize);
                    ticker = DateTime.Now;
                }
            }
        }
        catch {
            return;
        }
    }

    void CheckTimeout() {
        TimeSpan timeSpan = DateTime.Now - ticker;

        if(isConnected && timeSpan.Seconds > timeOutSec) {
            Debug.Log("DIsconncet bbecause of timeout.");
            Disconnect();
        }
    }
}