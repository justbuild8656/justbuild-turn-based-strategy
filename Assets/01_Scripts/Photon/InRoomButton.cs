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
            Debug.Log("�� �̸��� �Է��ϼ���.");
            return;
        }
        PhotonManager.Instance.TryToJoinRoom(roomName, OnJoinRoomFailed);
    }

    private void OnJoinRoomFailed()
    {
        Debug.Log($"'{inputField.text}' �濡 ������ �� �����ϴ�. ���� �������� �ʽ��ϴ�.");
        for(int i = 0; i < PhotonManager.Instance.CurRoomInfos.Count; i++)
        {
            Debug.Log($"�� �̸�: {PhotonManager.Instance.CurRoomInfos[i].Name}");
        }
    }
}
