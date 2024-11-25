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
            return;
        }

        loadingPanel.SetActive(true);
        PhotonManager.Instance.CreateRoom(roomNameInputField.text, maxPlayers);
    }
}
