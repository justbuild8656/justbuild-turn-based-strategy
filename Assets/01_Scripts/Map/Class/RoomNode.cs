using UnityEngine;
using System.Collections.Generic;

public enum RoomType
{
  Start, // 루트 노드(시작)
  Normal, // 일반 노드
  End// 마지막 노드(클리어 방)
}

// 방 노드 클래스
public class RoomNode
{
  public string roomID; // 방 ID
  public int depth; // 노드 깊이(레이어)
  public RoomType roomType; // 노드 종류
  public List<RoomNode> children; // 자식 노드 리스트

  // 생성자
  public RoomNode(string roomID, int depth, RoomType roomType)
  {
    this.roomID = roomID;
    this.depth = depth;
    this.roomType = roomType;
    this.children = new List<RoomNode>();
  }

  // 자식 노드 추가 매서드
  public bool AddChild(RoomNode child)
  {
    // 깊이 검사(부모 노드 깊이 - 1 == 자식 노드 깊이)
    if(child.depth != this.depth + 1)
    {
      Debug.Log($"{child.roomID}의 깊이가 {this.depth + 1}이어야 함. 현재 깊이: {this.depth}");
      return false;
    }

    children.Add(child);
    return true;
  }
}