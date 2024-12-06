using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MapGenerator))]
public class MapGeneratorDebugger : MonoBehaviour
{
  MapGenerator mapGenerator;
  RoomTree tree;

  // 맵 생성기의 트리를 가져옴
  public void Start(){
    mapGenerator = GetComponent<MapGenerator>();

    if(mapGenerator != null){
      tree = mapGenerator.GetMapTree();

      if(tree == null) Debug.Log("맵 생성기의 트리가 존재하지 않음");
    }
    else Debug.Log("맵 생성기가 존재하지 않음");

  }

  void OnDrawGizmos()
  {
    // 트리와 루트 노드가 존재하는 경우
    if (tree != null && tree.root != null) DrawNode(tree.root, new Vector3(30f, 20f, 0), 0);
  }

  // 트리를 그리는 재귀 매서드
  void DrawNode(RoomNode node, Vector3 position, int depth)
  {
    // 방 종류별 기즈모 색상
    if(node.roomType == RoomType.Start) Gizmos.color = Color.blue;
    else if(node.roomType == RoomType.Normal) Gizmos.color = Color.white;
    else if(node.roomType == RoomType.End) Gizmos.color = Color.green;

    Gizmos.DrawSphere(position, 0.8f);

    // 자식 노드가 존재하는 경우
    if(node.children != null && node.children.Count > 0)
    {
      float spacing = 5f + (3f * depth); // 깊이에 따라 간격 증가

      for(int i = 0; i < node.children.Count; i++)
      {
        Vector3 childPosition = position + new Vector3((i - node.children.Count / 2f) * spacing, -5f, 0);

        // 부모 노드와 자식 노드 연결
        Gizmos.color = Color.white;
        Gizmos.DrawLine(position, childPosition);

        // 재귀
        DrawNode(node.children[i], childPosition, depth + 1);
      }
    }
  }
}