using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<Player> players = new List<Player>();
    public Vector3[] spawnPoints;


    private void Start()
    {
        StartGame(spawnPoints[players.Count]);
    }

    public void StartGame(Vector3 spawnPoint)
    {
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint, Quaternion.identity);
        if(PhotonNetwork.IsMasterClient)
            photonView.RPC("RPCSetPlayer", RpcTarget.All, player.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    void RPCSetPlayer(int ViewID)
    {
        PhotonView photonView = PhotonNetwork.GetPhotonView(ViewID);
        Player player = photonView.GetComponent<Player>();
        players.Add(player);
    }

}
