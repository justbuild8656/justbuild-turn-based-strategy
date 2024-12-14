using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

// InGameManager는 PhotonPun의 MonoBehaviourPunCallbacks를 상속받아 네트워크 기능을 관리하는 클래스
public class InGameManager : MonoBehaviourPunCallbacks
{
    public static InGameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Player playerMine;

    public List<Player> players = new List<Player>();

    public GameObject[] spawnPoints;

    public GameObject guildPanel;

    public GameObject watingPanel;

    public GameObject PlayerPane1;

    void Start()
    {
        PlayerPane1.SetActive(false);
        photonView.RPC("RPCEnteredPlayer", RpcTarget.All);
    }

    public void OnClickedStartButton()
    {
        StartInGame();  
    }

    // 게임 시작을 위한 함수
    public void StartInGame()
    {
        photonView.RPC("RPCGuildClose", RpcTarget.All);  // 모든 클라이언트에서 길드 패널을 닫음
        photonView.RPC("RPCStartGame", RpcTarget.All);   // 모든 클라이언트에서 게임 시작 RPC 호출
    }

    // 게임 시작 시 호출되는 RPC 함수
    [PunRPC]
    public void RPCStartGame()
    {
        var playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        GameObject player = PhotonNetwork.Instantiate("Player"+ playerCount, Vector3.zero, Quaternion.identity);
        photonView.RPC("RPCSetPlayer", RpcTarget.All, player.GetComponent<PhotonView>().ViewID);
        photonView.RPC("RPCActivatePlayerPane", RpcTarget.All);
    }

    // 플레이어가 네트워크에 들어왔을 때 호출되는 RPC 함수
    [PunRPC]
    public void RPCEnteredPlayer()
    {
        // 현재 방에 플레이어 수가 3명이 되면 대기 패널을 숨김
        if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            watingPanel.SetActive(false);
        }
    }

    // 플레이어 오브젝트의 정보를 설정하는 RPC 함수
    [PunRPC]
    void RPCSetPlayer(int ViewID)
    {
        Debug.Log("Player ID : " + ViewID);
        PhotonView photonView = PhotonNetwork.GetPhotonView(ViewID);

        Player player = photonView.GetComponent<Player>();

        if (photonView.IsMine)
        {
            playerMine = player;  
            Debug.Log(photonView.ViewID);  
        }

        // 플레이어의 스폰 위치를 설정
        int playerIndex = ViewID % spawnPoints.Length;
        Transform spawnPoint = spawnPoints[playerIndex].transform;
        player.transform.position = spawnPoint.position;

        players.Add(player);
    }

    [PunRPC]
    void RPCGuildClose()
    {
        guildPanel.SetActive(false);  
    }

    [PunRPC]
    void RPCActivatePlayerPane()
    {
        PlayerPane1.SetActive(true);  
    }
}
