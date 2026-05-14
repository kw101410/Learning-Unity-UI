using UnityEngine;
using UnityEngine.UI; // Scrollbar를 사용하기 위해 필요합니다.

public class ScrollbarMover : MonoBehaviour
{
    [Header("연결 대상")]
    [SerializeField] private Scrollbar horizontalScrollbar;
    [SerializeField] private RectTransform contentPanel; // 움직일 콘텐츠 패널

    [Header("이동 범위 (X 좌표)")]
    [SerializeField] private float minXPosition = 0f;
    [SerializeField] private float maxXPosition = -500f; // 콘텐츠가 왼쪽으로 이동하므로 보통 음수입니다.

    void Start()
    {
        if (horizontalScrollbar != null)
        {
            // 스크롤바의 OnValueChanged 이벤트에 UpdateContentPosition 함수를 연결합니다.
            horizontalScrollbar.onValueChanged.AddListener(UpdateContentPosition);

            // 시작할 때 현재 스크롤바 값으로 위치 초기화
            UpdateContentPosition(horizontalScrollbar.value);
        }
    }

    /// <summary>
    /// Scrollbar의 value(0.0 ~ 1.0)를 받아
    /// contentPanel의 X 위치(minX ~ maxX)로 변환합니다.
    /// </summary>
    /// <param name="value">Scrollbar의 현재 값 (0.0 ~ 1.0)</param>
    public void UpdateContentPosition(float value)
    {
        if (contentPanel == null) return;

        // Mathf.Lerp (선형 보간)을 사용하여 0~1 사이의 값을 minX~maxX 사이의 값으로 변환
        float newX = Mathf.Lerp(minXPosition, maxXPosition, value);

        // contentPanel의 현재 anchoredPosition을 가져옵니다.
        Vector2 currentPos = contentPanel.anchoredPosition;

        // X 위치만 업데이트하고 Y 위치는 그대로 유지합니다.
        contentPanel.anchoredPosition = new Vector2(newX, currentPos.y);
    }
}