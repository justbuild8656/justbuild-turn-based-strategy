using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

// GameManager는 PhotonPun의 MonoBehaviourPunCallbacks를 상속받아 네트워크 기능을 관리하는 클래스
public class GameManager : MonoBehaviourPunCallbacks
{
    // 싱글톤 패턴을 위한 정적 Instance 변수
    public static GameManager Instance { get; private set; }

    // MonoBehaviour의 Awake 메서드. 게임 매니저 객체를 싱글톤으로 초기화
    private void Awake()
    {
        Instance = this; // 현재 인스턴스를 Singleton으로 설정
        DontDestroyOnLoad(gameObject); // 게임 씬이 변경되어도 이 객체를 파괴하지 않음
    }

    // 게임의 플레이어 리스트
    public List<Player> players = new List<Player>();

    // 플레이어가 스폰될 위치 배열
    public GameObject[] spawnPoints;

    // MonoBehaviour의 Start 메서드. 게임 시작 시 호출됨
    private void Start()
    {
        // 플레이어 수에 기반하여 스폰 포인트를 설정하고 게임 시작
        StartGame(spawnPoints[players.Count]);
    }

    // 게임을 시작하고 플레이어를 네트워크 상에서 생성
    public void StartGame(GameObject spawnPoint)
    {
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint.transform.position, Quaternion.identity);
        Debug.Log("Instantiated Player");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master client calling RPC...");
            photonView.RPC("RPCSetPlayer", RpcTarget.All, player.GetComponent<PhotonView>().ViewID);
        }

    }

    // RPC 메서드: 네트워크를 통해 ViewID를 받아 플레이어를 리스트에 추가
    [PunRPC]
    void RPCSetPlayer(int ViewID)
    {
        // PhotonView를 통해 ViewID에 해당하는 객체를 가져옴
        PhotonView photonView = PhotonNetwork.GetPhotonView(ViewID);

        // 가져온 객체에서 Player 컴포넌트를 추출
        Player player = photonView.GetComponent<Player>();

        // 플레이어를 리스트에 추가
        players.Add(player);
    }
}
