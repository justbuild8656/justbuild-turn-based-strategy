using System;
using UnityEngine;

// 노드 정보 클래스(에디터에서 사용)
[Serializable]
public class RoomNodeInfo
{
  public string roomID; // 방 ID
  public string parentID; // 부모 노드의 ID
  public RoomType roomType; // 방 종류
}