using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

// GameManager는 PhotonPun의 MonoBehaviourPunCallbacks를 상속받아 네트워크 기능을 관리하는 클래스
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

    void Start()
    {
        photonView.RPC("RPCEnteredPlayer", RpcTarget.All);
    }

    public void OnClickedStartButton()
    {
        StartInGame();
    }

    public void StartInGame()
    {
        photonView.RPC("RPCGuildClose", RpcTarget.All);
        photonView.RPC("RPCStartGame", RpcTarget.All);
    }
    [PunRPC]
    public void RPCStartGame()
    {
        GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        photonView.RPC("RPCSetPlayer", RpcTarget.All, player.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    public void RPCEnteredPlayer()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            watingPanel.SetActive(false);
        }
    }

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
}
