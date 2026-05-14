using UnityEngine;
using UnityEngine.UI; // ScrollRect를 사용하기 위해 필요합니다.

public class ScrollControl : MonoBehaviour
{
    // 인스펙터에서 Scroll View 오브젝트를 연결
    [SerializeField] private ScrollRect myScrollRect;

    // '맨 앞' 버튼에 연결할 함수
    public void JumpToStart()
    {
        if (myScrollRect != null)
        {
            // 수평 스크롤 위치를 0 (맨 왼쪽)으로 설정
            myScrollRect.horizontalNormalizedPosition = 0f;
        }
    }

    // '맨 뒤' 버튼에 연결할 함수
    public void JumpToEnd()
    {
        if (myScrollRect != null)
        {
            // 수평 스크롤 위치를 1 (맨 오른쪽)으로 설정
            myScrollRect.horizontalNormalizedPosition = 1f;
        }
    }
}