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
        Debug.Log("�� �ڵ� :" + roomCode);

        PhotonManager.Instance.CreateRoom(roomCode, 3);
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

    // �濡 �� �� �� �ڵ带 Ȯ���ϴ� �޼���
    public void JoinRoomByCode(string roomCode)
    {
        PhotonNetwork.JoinRoom(roomCode);
    }

    // �濡 �������� ��
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        string roomCode = (string)PhotonNetwork.CurrentRoom.CustomProperties["roomCode"];
        Debug.Log("������ ���� �ڵ�: " + roomCode);
    }
}
