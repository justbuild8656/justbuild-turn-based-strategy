using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance { get; private set; }

    public List<RoomInfo> CurRoomInfos;
    public bool IsReadyToJoinRoom => PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby;
    
    private Action _failCallBack;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 15;
    }

    private void Start()
    {
        TryToJoinServer();
    }
    
    public void TryToJoinServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            OnConnectedToMaster();
        }
    }
    
    public void CreateRoom(string roomTitle, int maxPlayer)
    {
        RoomOptions roomOptions = new()
        {
            MaxPlayers = maxPlayer
        };
        
        PhotonNetwork.CreateRoom(roomTitle, roomOptions);
    }
    
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        
        if (!PhotonNetwork.InLobby)
        {
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
        PhotonNetwork.LoadLevel("Lobby");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        CurRoomInfos = roomList;
    }
    
    public void TryToJoinRoom(string roomName = null, Action failCallBack = null)
    {
        _failCallBack = failCallBack;
        if (string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
        } 
        else
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        _failCallBack?.Invoke();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SceneManager.LoadScene("InGame");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {

    }
}