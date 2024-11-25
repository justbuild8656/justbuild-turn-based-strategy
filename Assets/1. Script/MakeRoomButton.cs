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
            Debug.Log("방 이름을 적어주세요.");
            return;
        }

        loadingPanel.SetActive(true);
        PhotonMgr.Instance.CreateRoom(roomNameInputField.text, maxPlayers);
    }
}
