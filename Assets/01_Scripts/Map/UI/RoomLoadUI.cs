using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 방 이동에 필요한 UI를 동적 생성
[RequireComponent(typeof(TestRoomManager))]
public class RoomLoadUI : MonoBehaviour
{
  [Header("UI 부모 객체")]
  public GameObject uiParent; // 동적 UI가 생성될 부모 객체

  [Header("버튼 프리팹")]
  public GameObject buttonPrefab; // 버튼 프리팹

  RoomNode currentRoom; // 현재 방 노드
  TestRoomManager roomManager;

  // 초기화 매서드
  public void Initialize(RoomNode roomNode)
  {
    currentRoom = roomNode;
    roomManager = gameObject.GetComponent<TestRoomManager>();
  }

  // 방 로드 UI 표시 매서드
  public void DisplayUI()
  {
    if(currentRoom == null)
    {
      Debug.Log("방을 찾을 수 없음");
      return;
    }

    if(buttonPrefab == null)
    {
      Debug.Log("버튼 프리팹을 찾을 수 없음");
      return;
    }

    // 기존 동적 UI 제거
    foreach(Transform child in uiParent.transform) Destroy(child.gameObject);

    float spacing = 400f; // 버튼 간 간격
    float start = 0f; // 시작 위치
    int childCount = currentRoom.children.Count; // 자식 노드 갯수

    // 자식 노드가 여러개일 경우 중심으로 정렬
    if(childCount > 1) start = -(spacing * (childCount - 1)) / 2f;

    for(int i = 0; i < childCount; i++)
    {
      RoomNode childNode = currentRoom.children[i];

      // 버튼 생성
      GameObject newButton = Instantiate(buttonPrefab, uiParent.transform);

      // 버튼 위치
      RectTransform rectTransform = newButton.GetComponentInChildren<RectTransform>();
      rectTransform.anchoredPosition = new Vector2(start + i * spacing, 0);

      // 버튼 텍스트
      TextMeshProUGUI text = newButton.GetComponentInChildren<TextMeshProUGUI>();
      text.text = $"Go to \n {childNode.roomID}";

      // 버튼 클릭 이벤트
      Button button = newButton.GetComponent<Button>();
      button.onClick.AddListener(() => MoveToRoom(childNode));
    }

    uiParent.SetActive(false);
  }

  // 방 이동 메서드
  private void MoveToRoom(RoomNode targetRoom)
  {
    roomManager.LoadRoom(targetRoom); // 방 이동
    Initialize(targetRoom); // 현재 방 갱신
    DisplayUI(); // UI 갱신
  }
}
