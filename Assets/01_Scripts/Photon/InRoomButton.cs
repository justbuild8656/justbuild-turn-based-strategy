using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class InRoomButton : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputField;

    public void OnClickedInRoomButton()
    {
        string roomName = inputField.text;
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.Log("방 이름을 입력하세요.");
            return;
        }
        PhotonManager.Instance.TryToJoinRoom(roomName, OnJoinRoomFailed);
    }

    private void OnJoinRoomFailed()
    {
        Debug.Log($"'{inputField.text}' 방에 접속할 수 없습니다. 방이 존재하지 않습니다.");
        for(int i = 0; i < PhotonManager.Instance.CurRoomInfos.Count; i++)
        {
            Debug.Log($"방 이름: {PhotonManager.Instance.CurRoomInfos[i].Name}");
        }
    }
}
