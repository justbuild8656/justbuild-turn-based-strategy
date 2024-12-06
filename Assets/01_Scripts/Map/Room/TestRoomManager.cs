using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(RoomLoadUI))]
public class TestRoomManager : MonoBehaviour
{
  [Header("맵 생성기")]
  public MapGenerator mapGenerator; // 맵 생성기

  [Header("방 로드 위치")]
  public Vector3 roomLoadpos; // 방 생성 위치

  [Header("방 프리팹")]
  public GameObject room;

  RoomTree tree;
  private GameObject currentRoomInstance; // 현재 생성된 방

  public void Start()
  {
    if(mapGenerator != null)
    {
      tree = mapGenerator.GetMapTree();

      if(tree != null)
      {
        if(tree.root != null) LoadRoom(tree.root);
        else Debug.Log("루트 노드가 없음");
      }
      else Debug.Log("맵 트리가 없음");
    }
    else Debug.Log("맵 생성기를 찾을 수 없음");
  }

  // 방 로드 매서드
  public void LoadRoom(RoomNode roomnode)
  {
    if(room == null)
    {
      Debug.Log("방 프리팹이 없음");
      return;
    }
    
    // 이전 방 제거
    if(currentRoomInstance != null) Destroy(currentRoomInstance);

    // 방 생성
    currentRoomInstance  = Instantiate(room, roomLoadpos, Quaternion.identity);

    // 방 이벤트
    RoomEvent(currentRoomInstance, roomnode);

    RoomLoadUI roomLoadUI = gameObject.GetComponent<RoomLoadUI>();

    if(roomLoadUI == null)
    {
      Debug.Log("roomLoadUI가 없음");
      return;
    }

    // UI 설정
    roomLoadUI.Initialize(roomnode);
    roomLoadUI.DisplayUI();
  }

  // 방 이벤트 설정(현재는 임시로 텍스트만 표시)
  public void RoomEvent(GameObject currentRoom, RoomNode roomNode)
  {
    TextMeshProUGUI text = currentRoom.GetComponentInChildren<Canvas>().
      GetComponentInChildren<TextMeshProUGUI>();

    text.text = $"Room ID: {roomNode.roomID} \n Room Type: {roomNode.roomType}";
  }
}