using UnityEngine;
using UnityEngine.UI;
using System.Collections; // 코루틴을 사용하기 위해 꼭 필요합니다!

public class SliderValueControl : MonoBehaviour
{
    [Header("연결 대상")]
    [SerializeField] private Slider targetSlider;

    [Header("부드러운 이동 설정")]
    [SerializeField] private float changeAmount = 8f; // 한 번에 변경할 값
    [SerializeField] private float smoothDuration = 0.5f; // 값이 변하는 데 걸리는 시간 (초)

    // 현재 코루틴이 실행 중인지 확인하는 플래그
    // (버튼을 연타해도 코루틴이 중복 실행되지 않도록 방지)
    private bool isMoving = false;

    // --- 원본 코드 (즉시 변경) ---

    // '감소' 버튼의 OnClick() 이벤트에 이 함수를 연결하세요.
    public void DecreaseValue()
    {
        if (targetSlider != null)
        {
            targetSlider.value -= changeAmount;
        }
    }

    // (참고) '증가' 버튼에 연결할 수 있는 함수
    public void IncreaseValue()
    {
        if (targetSlider != null)
        {
            targetSlider.value += changeAmount;
        }
    }

    // --- 코루틴을 사용한 부드러운 이동 ---

    /// <summary>
    /// '부드럽게 감소' 버튼의 OnClick()에 이 함수를 연결하세요.
    /// </summary>
    public void DecreaseValueSmoothly()
    {
        // 이미 값이 변하는 중이라면 중복 실행하지 않음
        if (isMoving || targetSlider == null)
        {
            return;
        }

        // changeAmount만큼 *빼는* 코루틴을 시작시킵니다.
        StartCoroutine(SmoothMoveCoroutine(-changeAmount));
    }

    /// <summary>
    /// (참고) '부드럽게 증가' 버튼용 함수
    /// </summary>
    public void IncreaseValueSmoothly()
    {
        if (isMoving || targetSlider == null)
        {
            return;
        }

        // changeAmount만큼 *더하는* 코루틴을 시작시킵니다.
        StartCoroutine(SmoothMoveCoroutine(changeAmount));
    }


    /// <summary>
    /// 슬라이더의 값을 특정 시간(duration) 동안 부드럽게 변경하는 코루틴
    /// </summary>
    /// <param name="amount">변경할 총량 (예: -8f 또는 +8f)</param>
    private IEnumerator SmoothMoveCoroutine(float amount)
    {
        isMoving = true; // 이동 시작 플래그

        float elapsedTime = 0f;
        float startValue = targetSlider.value;

        // 목표 값 계산 (슬라이더의 최소/최대 값을 넘지 않도록 Clamp)
        float targetValue = Mathf.Clamp(startValue + amount, targetSlider.minValue, targetSlider.maxValue);

        // elapsedTime이 smoothDuration에 도달할 때까지 반복
        while (elapsedTime < smoothDuration)
        {
            // Lerp(시작값, 목표값, 진행률)
            // 진행률(elapsedTime / smoothDuration)은 0.0에서 1.0까지 증가합니다.
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / smoothDuration);

            targetSlider.value = newValue;

            // 다음 프레임까지 대기
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 루프가 끝나면 오차를 보정하기 위해 목표 값으로 정확히 설정
        targetSlider.value = targetValue;

        isMoving = false; // 이동 완료 플래그
    }
}