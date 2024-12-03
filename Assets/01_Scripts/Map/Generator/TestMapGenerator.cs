using UnityEngine;

[RequireComponent(typeof(MapEditer))]
public class TestMapGenerator : MapGenerator
{
  public void Start(){
    MapEditer mapEditor = gameObject.GetComponent<MapEditer>();
    
    // 맵의 정보를 에디터에서 받아옴
    RoomNodeInfo[] mapData = mapEditor.GetMapData();
    int maxDepth = mapEditor.GetTreeDepth();

    // 맵 생성
    RoomTree tree = CreateTree(mapData, maxDepth);

    if(tree != null) mapTree = tree;
    else Debug.Log("트리 생성 실패");
  }
}
