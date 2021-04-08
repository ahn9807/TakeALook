using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Collections;

public class GameNetwork : MonoBehaviour
{
    //네트워크 
    string hostAddress;
    NetworkController networkController = null;
    GameMode gameMode;
    float timeScale;

    public enum GameMode
    {
        Ready = 0,
        Game,
        Result,
    };

    private void Awake()
    {
        timeScale = 1;
        Time.timeScale = 0;
    }
    // Use this for initialization
    void Start()
    {
        gameMode = GameMode.Ready;
        string hostname = Dns.GetHostName();
        IPAddress[] adrList = Dns.GetHostAddresses(hostname);
        hostAddress = adrList[0].ToString();
    }

    private void FixedUpdate()
    {
        switch(gameMode) {
            case GameMode.Ready:
                UpdateReady();
                break;
            case GameMode.Game:
                UpdateGame();
                break;
            case GameMode.Result:
                UpdateResult();
                break;
        }

        //프레임 동기화 진행 체크
        if(networkController!=null && networkController.IsSync()) {
            //프레임 동기화가 완료 되어 플래그를 클리어 한다 
            networkController.ClearSync();
            Time.timeScale = 0;
        }
    }

    private void LateUpdate()
    {
        if(networkController != null) {
            //송수신 동기화 시도와 완료되었는지 확인한다 
            if(networkController.UpdateSync()) {
                Resume();
            }
            else {
                Suspend();
            }
        }
    }

    //접속 대기
    void UpdateReady() {
        if(networkController != null) {
            if(networkController.IsConnected() == true) {
                NetworkController.HostType hostType = networkController.GetHostType();
                GameStart(hostType == NetworkController.HostType.Server);
                gameMode = GameMode.Game;
            }
        }
    }

    //게임 중
    void UpdateGame() {
        if(GameController.instance.IsEnd()) {
            networkController.SuspendSync();
            if(networkController.IsSuspend() == true) {
                gameMode = GameMode.Result;
            }
        }
    }

    //결과를 표시하고 승부를 낸다 
    void UpdateResult() {
        
    }

    public void Resume() {
        Time.timeScale = timeScale;
    }

    public void Suspend() {
        Time.timeScale = 0;
    }

    public void ClientConnection(string hostaddr) {
        if(networkController == null) {
            PlayerInfo info = PlayerInfo.GetInstance();
            hostAddress = hostaddr;
            networkController = new NetworkController(hostAddress, false);
            info.SetPlayerId(1);
        }
    }

    public void ServerConnection(string hostaddr) {
        if(networkController == null) {
            PlayerInfo info = PlayerInfo.GetInstance();
            hostAddress = hostaddr;
            networkController = new NetworkController(hostAddress, true);
            info.SetPlayerId(0);
        }
    }

    public void DisconnectObservation() {
        if(networkController != null &&
           networkController.IsConnected() == false &&
           networkController.IsSuspend() == false &&
           networkController.GetSyncState() != NetworkController.SyncState.NotStarted) {
            NotifyDisconnection();
        }

    }

    void GameStart(bool isServer) {
        //서버와 클라이언트의 작업을 처리한다 
    }

    //서버와 클라이언트가 끓김을 알려준다 
    void NotifyDisconnection() {
        
    }
}
