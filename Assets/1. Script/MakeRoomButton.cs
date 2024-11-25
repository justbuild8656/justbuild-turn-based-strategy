using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class MakeRoomButton : MonoBehaviour
{
    public TMP_InputField roomNameInputField;
    public int maxPlayers;
    public GameObject loadingPanel;

    public void OnClickedCreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            Debug.Log("�� �̸��� �����ּ���.");
            return;
        }

        loadingPanel.SetActive(true);
        PhotonMgr.Instance.CreateRoom(roomNameInputField.text, maxPlayers);
    }
}
