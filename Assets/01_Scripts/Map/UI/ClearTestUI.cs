using UnityEngine;

// 스테이지 클리어 테스트 UI
public class ClearTestUI : MonoBehaviour
{
  public GameObject roomLoadUI;

  // 스테이지 클리어 테스트 버튼 
  public void onButtonClick()
  {
    roomLoadUI.SetActive(true);
  }
}
