using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class MakeRoomButton : MonoBehaviour
{
    public GameObject loadingPanel;

    public int maxPlayers;
    private void Start()
    {
        
    }

    public void OnClickedCreateRoom()
    {
        loadingPanel.SetActive(true);
        PhotonManager.Instance.CreateRoom(CreateCode(), maxPlayers);
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
}
