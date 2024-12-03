using UnityEngine;
using System.Collections.Generic;

// 방 트리
public class RoomTree
{
  public RoomNode root; // 루트 노드
  public int maxDepth; // 최대 깊이

  // 생성자
  public RoomTree(int maxDepth)
  {
    this.maxDepth = maxDepth;
    root = null;
  }

  // 루트 노드 설정 매서드
  public bool SetRoot(RoomNode room)
  {
    if(room.depth != 1)
    {
      Debug.Log($"{room.roomID}의 깊이가 1이어야 함");
      return false;
    }

    // 루트 노드 설정
    root = room;
    return true;
  }

  // 자식 노드 추가 메서드
  public bool AddChild(RoomNode parentNode, RoomNode childNode)
  {
    if(parentNode == null)
    {
      Debug.Log("부모 노드가 존재하지 않음");
      return false;
    }

    if(childNode.depth != parentNode.depth + 1)
    {
      Debug.LogError($"자식 노드({childNode.roomID})의 깊이는 {parentNode.depth + 1}이어야 함");
      return false;
    }

    if(childNode.depth > maxDepth)
    {
      Debug.Log("자식 노드의 깊이가 트리의 최대 깊이를 초과함");
      return false;
    }

    // 자식 노드를 부모에 연결
    parentNode.AddChild(childNode);

    return true;
  }
}