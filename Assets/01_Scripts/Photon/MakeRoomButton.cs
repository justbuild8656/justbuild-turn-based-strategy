using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class MakeRoomButton : MonoBehaviourPunCallbacks
{
    public GameObject loadingPanel;
    public int maxPlayers;

    private void Start() { }

    public void OnClickedCreateRoom()
    {
        loadingPanel.SetActive(true);
        string roomCode = CreateCode();
        Debug.Log("방 코드 :" + roomCode);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)maxPlayers;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "roomCode", roomCode } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "roomCode" };

        PhotonNetwork.CreateRoom(roomCode, roomOptions);
    }

    string CreateCode()
    {
        int[] numbers = new int[5];
        string code = "";
        for (int i = 0; i < numbers.Length; i++)
        {
            code += Random.Range(0, 9).ToString();
        }
        return code;
    }

    // 방에 들어갈 때 방 코드를 확인하는 메서드
    public void JoinRoomByCode(string roomCode)
    {
        PhotonNetwork.JoinRoom(roomCode);
    }

    // 방에 접속했을 때
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        string roomCode = (string)PhotonNetwork.CurrentRoom.CustomProperties["roomCode"];
        Debug.Log("접속한 방의 코드: " + roomCode);
    }
}
