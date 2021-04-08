using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
    //서버 전용 변수들 (자주 사용함으로 만듬 )
    TrasportUDP transport;
    InputManager inputManager;

    //서버 클라이언트 편집용 
    public enum HostType
    {
        Server,
        Client,
    }

    //서버 상태 표지용 
    public enum SyncState
    {
        NotStarted = 0, // 키 데이터 송수신 하지 않음 
        WaitSynchronize, // 키 데이터 송신 혹은 수신
        Synchronized, // 동기화 완료 
    }

    private HostType hostType;
    private SyncState syncState = SyncState.NotStarted;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private static int playerNum = 2;
    private const int bufferNum = 4;

    private List<MouseData>[] inputBuffer = new List<MouseData>[playerNum];
    private MouseData[] mouseData = new MouseData[playerNum];

    // 서버 지연 처리용
    private int sendFrame = -1;
    private int recvFrame = -1;

    private bool isSynchronized = false;

    //서버에 접속 했는가 
    private bool isConnected = false;

    //서버 손절 
    private int suspendSync = 0;

    //통신환경이 나쁘면 중복 데이터 재송신 한다
    private int noSyncCount = 0;

    //게임 종료시 끓어질 떄까지의 기간
    private int disconnectCount = 0;

    //접속 확인용 더미 패킷
    private const string requestData = "Request Connection.";

    public NetworkController(string hostAddress, bool isHost)
    {
        isSynchronized = false;
        hostType = isHost ? HostType.Server : HostType.Client;

        GameObject nObj = GameObject.Find("Network");
        transport = nObj.GetComponent<TrasportUDP>();

        int remotePort = isHost ? NetConfig.GAME_PORT + 1 : NetConfig.GAME_PORT;
        transport.Connect(hostAddress, remotePort);

        transport.RegisterEventHandler(OnEventHandling);

        GameObject iObj = GameObject.Find("InputManager");
        inputManager = iObj.GetComponent<InputManager>();

        for (int i = 0; i < inputBuffer.Length; ++i)
        {
            inputBuffer[i] = new List<MouseData>();
        }
    }

    //네트워크 상태 가져오기 
    public bool IsConnected()
    {
        return (transport.IsConnected() && isConnected);
    }

    public SyncState GetSyncState()
    {
        return syncState;
    }

    public bool IsSuspend()
    {
        return (suspendSync == 0x03);
    }

    public HostType GetHostType()
    {
        return hostType;
    }

    public void SuspendSync()
    {
        if (suspendSync > 0)
        {
            return;
        }
        suspendSync = 0x01;
        Debug.Log("SUspendSync requested.");
    }

    public bool IsSync()
    {
        bool isSuspended = ((suspendSync & 0x02) == 0x02);
        bool frameSync = (syncState == SyncState.Synchronized) && isSynchronized;

        return (frameSync || !isConnected || isSuspended);
    }

    public void ClearSync()
    {
        isSynchronized = false;
    }

    //송수신 동기화
    public bool UpdateSync()
    {
        if (IsConnected() == false && syncState == SyncState.NotStarted)
        {
            byte[] request = System.Text.Encoding.UTF8.GetBytes(requestData);
            transport.Send(request, request.Length);
            return false;
        }

        bool update = EnqueueMouseData(); // 키 값을 버퍼에 저장한다

        if (update)
        {
            SendInputData(); // 키값을 송신한다
        }

        ReceiveInputData(); // 키값을 수신한다

        if (IsSync() == false) //동기화 중엔 아무것도 하지 않는다 
        {
            DequeueMouseData();
        }

        return IsSync();
    }

    //송신 
    public void SendInputData() {
        PlayerInfo info = PlayerInfo.GetInstance();
        int playerId = info.GetPlayerId();
        int count = inputBuffer[playerId].Count;

        InputData inputData = new InputData();
        inputData.count = count;
        inputData.flag = suspendSync;
        inputData.datum = new MouseData[bufferNum];

        for (int i = 0; i < count;++i) {
            inputData.datum[i] = inputBuffer[playerId][i];
        }

        InputSerializer serializer = new InputSerializer();
        bool ret = serializer.Serialize(inputData);
        if(ret) {
            byte[] data = serializer.GetSerializedData();
            transport.Send(data, data.Length);
        }

        if(syncState == SyncState.NotStarted) {
            syncState = SyncState.WaitSynchronize;
        }
    }

    //수신 
    public void ReceiveInputData() {
        byte[] data = new byte[1400];

        int recvSize = transport.Receive(ref data, data.Length);
        if(recvSize < 0) {
            return;
        }

        string str = System.Text.Encoding.UTF8.GetString(data);
        //접속 요청 패킷 
        if(requestData.CompareTo(str.Trim('\0')) == 0) {
            return;
        }

        InputData inputData = new InputData();
        InputSerializer serializer = new InputSerializer();
        serializer.Deserialize(data, ref inputData);

        PlayerInfo info = PlayerInfo.GetInstance();
        int playerId = info.GetPlayerId();
        int opponent = (playerId == 0) ? 1 : 0;

        for (int i = 0; i < inputData.count; ++i) {
            int frame = inputData.datum[i].frame;
            if(recvFrame + 1 == frame) {
                inputBuffer[opponent].Add(inputData.datum[i]);
                ++recvFrame;
            }
        }

        //접속 종료 플래그 감시
        if((inputData.flag & 0x03) == 0x03) {
            suspendSync = 0x03;
            Debug.Log("Receive SUspendSunc.");
        }

        if((inputData.flag & 1) > 0 && (suspendSync & 1) > 0) {
            suspendSync |= 0x02;
            Debug.Log("Receive SuspendSync." + inputData.flag);
        }

        if(isConnected && suspendSync == 0x03) {
            ++disconnectCount;
            if(disconnectCount > 10) {
                transport.Disconnect();
                Debug.Log("Disconnect becuase of suspendSync.");
            }
        }

        if(syncState == SyncState.NotStarted) {
            syncState = SyncState.WaitSynchronize;
        }
    }

    public bool EnqueueMouseData() {
        PlayerInfo info = PlayerInfo.GetInstance();
        int playerId = info.GetPlayerId();

        if(inputBuffer[playerId].Count >= bufferNum) {
            ++noSyncCount;
            if(noSyncCount >= bufferNum) {
                noSyncCount = 0;
                return true;
            }

            return false;
        }

        sendFrame++;
        MouseData mouseData = inputManager.GetLocalMouseData();
        mouseData.frame = sendFrame;
        inputBuffer[playerId].Add(mouseData);

        return true;
    }

    public void DequeueMouseData()
    {
        //Debug.Log("DequeueMouseData");
        // 양 단말의 데이터가 모였는지 체크합니다.
        for (int i = 0; i < playerNum; ++i)
        {
            if (inputBuffer[i].Count == 0)
            {
                return;     //입력값이 없을 때는 아무것도 하지 않습니다.
            }
        }

        // 데이터가 모였으므로.
        for (int i = 0; i < playerNum; ++i)
        {
            mouseData[i] = inputBuffer[i][0];
            inputBuffer[i].RemoveAt(0);

            //입력 관리자에게 동기된 데이터로서 전달합니다.
            inputManager.SetInputData(i, mouseData[i]);

#if false
            m_debugWriterSyncData.WriteLine(mouseData[i]);
#endif
        }
#if false
        m_debugWriterSyncData.Flush();
#endif

        //Debug.Log("DequeueMouseData:isSynchronized");

        // 상태 갱신.
        if (syncState != SyncState.Synchronized)
        {
            syncState = SyncState.Synchronized;
        }

        isSynchronized = true;
    }

    public void OnEventHandling(NetEventState state)
    {
        switch (state.type)
        {
            case NetEventType.Connect:
                isConnected = true;
                Debug.Log("[NetworkController] Connected.");
                break;

            case NetEventType.Disconnect:
                isConnected = false;
                Debug.Log("[NetworkController] Disconnected.");
                break;
        }
    }

    //debug code.
    void EmurateInput()
    {
        PlayerInfo info = PlayerInfo.GetInstance();
        int playerId = info.GetPlayerId();
        MouseData inputData = inputManager.GetLocalMouseData(); //m_inputManager.GetMouseData(playerId);

        //동기화된 입력값 위장(자신의 입력을 상대의 입력으로서 준다).
        int opponent = (playerId == 0) ? 1 : 0;
        inputManager.SetInputData(playerId, inputData);
        inputManager.SetInputData(opponent, inputData);

        // = SyncFlag.Synchronized;
        isSynchronized = true;
    }

}
