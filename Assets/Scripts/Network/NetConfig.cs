using System.Net;
using UnityEngine;
using System.Collections;

public class NetConfig {
    public static int PLAYER_MAX = 4;

    public static int SERVER_PORT = 50764;
    public static int GAME_PORT = 50765;
}

public class NetEventState {
    public NetEventType type;
    public NetEventResult result;
    public IPEndPoint endPoint;
}

public enum NetEventType {
    Connect = 0,
    Disconnect,
    SendError,
    ReceiveError,
}

public enum NetEventResult {
    Failure = -1,
    Success = 0,
}