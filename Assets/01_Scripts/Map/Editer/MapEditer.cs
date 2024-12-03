using System;
using UnityEngine;

// 맵 에디터 사용 방법
// 1. 트리 깊이 설정(레이어 깊이)
// 2. 방의 갯수 설정 (roomNodeInfo 배열의 길이)
// 3. 방 정보 설정
// 3-1. 시작 방: parentID 빈칸으로 설정, roomType Start로 설정
// 3-2. 나머지 방: parentID를 이전 방의 이름으로 설정

// 맵 에디터
[RequireComponent(typeof(MapGenerator))]
public class MapEditer : MonoBehaviour
{
  [Header("트리 깊이")]
  public int treeDepth; // 트리 깊이

  [Header("트리 정보")]
  public RoomNodeInfo[] roomNodeInfo;

  // 맵 정보 전달 매서드
  public RoomNodeInfo[] GetMapData()
  {
    return roomNodeInfo;
  }

  // 트리 최대 깊이 전달 매서드
  public int GetTreeDepth()
  {
    return treeDepth;
  }
}