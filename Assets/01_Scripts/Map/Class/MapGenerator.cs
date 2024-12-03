using UnityEngine;
using System.Collections.Generic;

// 맵 생성기 클래스
public class MapGenerator : MonoBehaviour
{
  public RoomTree mapTree; // 맵 트리

  // 트리 생성 매서드
  public RoomTree CreateTree(RoomNodeInfo[] mapData, int maxDepth)
  {
    if(mapData == null || mapData.Length == 0){
      Debug.Log("맵 데이터가 없음");
      return null;
    }

    RoomTree tree = new RoomTree(maxDepth);
    Dictionary<string, RoomNode> nodeDictionary = new Dictionary<string, RoomNode>();

    foreach(var nodeInfo in mapData)
    {
      // 부모 노드가 없다면 루트 노드
      if(string.IsNullOrEmpty(nodeInfo.parentID))
      {
        RoomNode newNode = new RoomNode(nodeInfo.roomID, 1, nodeInfo.roomType);
        if (!tree.SetRoot(newNode))
        {
          Debug.LogError("루트 노드 설정 실패");
          return null;
        }

        nodeDictionary[nodeInfo.roomID] = newNode;
      }
      else
      {
        // 부모 노드가 있다면 자식 노드
        if(nodeDictionary.TryGetValue(nodeInfo.parentID, out RoomNode parentNode))
        {
          RoomNode newNode = new RoomNode(nodeInfo.roomID, parentNode.depth + 1, nodeInfo.roomType);
          if(!tree.AddChild(parentNode, newNode))
          {
            Debug.Log($"자식 노드 추가 실패 {nodeInfo.roomID}");
            return null;
          }

          nodeDictionary[nodeInfo.roomID] = newNode;
        }
        // 부모 노드의 이름을 찾을 수 없음
        else
        {
          Debug.Log($"부모 노드를 찾을 수 없음 {nodeInfo.parentID}");
          return null;
        }
      }
    }

    return tree;
  }

  // 맵 트리 반환 매서드
  public RoomTree GetMapTree(){
    return mapTree;
  }
}