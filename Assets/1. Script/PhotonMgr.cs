using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public class PhotonMgr : MonoBehaviourPunCallbacks
{
    private static PhotonMgr instance;

    public static PhotonMgr Instance
    {
        get
        {
            return instance;
        }
    }
    public bool IsReadyToJoinRoom()
    {
        return PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby;
    }

    private void Awake()
    {
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 15;
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        TryToJoinServer();
    }
    public void TryToJoinServer()
    {
        Debug.Log("서버 연결 시도");

        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("연결이 안되어 있습니다.");
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            OnConnectedToMaster();
        }

    }
    public void CreateRoom(string roomTitle, int maxPlayer)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayer;
        PhotonNetwork.CreateRoom(roomTitle, roomOptions);
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("서버 접속 완료");
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("로비로 접속 시도합니다.");
            PhotonNetwork.JoinLobby();
        }
        else
        {
            OnJoinedLobby();

        }
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("로비 접속 완료");
        PhotonNetwork.LoadLevel("Lobby");
    }

    public List<RoomInfo> curRoomInfos;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        curRoomInfos = roomList;
    }

    Action failCallBack;
    public void TryToJoinRoom(string name = null, Action failCallBack = null)
    {
        this.failCallBack = failCallBack;
        if (string.IsNullOrEmpty(name))
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
        } 
        else
        {
            PhotonNetwork.JoinRoom(name);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        failCallBack?.Invoke();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("방 이름 : " + PhotonNetwork.CurrentRoom.Name);
        SceneManager.LoadScene("InGame");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {

    }


}
