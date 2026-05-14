using UnityEngine;
using UnityEngine.UI; // 레거시 UI 컴포넌트를 위해 필수
using System.Collections;
using System.Collections.Generic; // Dictionary 사용을 위해 필요

public class ChannelChatWindow_Legacy : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private InputField chatInputField; // [변경] TMP_InputField -> InputField
    [SerializeField] private Button sendButton;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Dropdown channelDropdown; // [변경] TMP_Dropdown -> Dropdown

    [Header("메시지가 생성될 위치")]
    [SerializeField] private RectTransform content;

    [Header("생성할 메시지 프리팹")]
    [SerializeField] private GameObject messagePrefab; // (중요) 이 프리팹은 레거시 'Text' 컴포넌트를 가져야 합니다.

    // --- 데이터 저장소 ---
    private Dictionary<string, List<string>> chatHistories = new Dictionary<string, List<string>>();

    private string currentChannelKey;

    void Start()
    {
        // 1. 드롭다운 옵션을 기반으로 채팅 기록 사전을 초기화합니다.
        InitializeChatHistories();

        // 2. 드롭다운 변경 이벤트에 'OnChannelChanged' 함수 연결
        if (channelDropdown != null)
        {
            channelDropdown.onValueChanged.AddListener(OnChannelChanged);
        }

        // 3. 나머지 이벤트 연결 (이전과 동일)
        if (sendButton != null)
        {
            sendButton.onClick.AddListener(OnClickSend);
        }
        if (chatInputField != null)
        {
            chatInputField.onSubmit.AddListener(OnSubmitChat);
        }

        // 4. 앱 시작 시 첫 번째 채널을 기본값으로 로드
        currentChannelKey = channelDropdown.options[channelDropdown.value].text;
        LoadChannelHistory(currentChannelKey);
    }

    // 드롭다운의 모든 옵션에 대해 빈 메시지 리스트를 생성
    private void InitializeChatHistories()
    {
        // [변경] TMP_Dropdown.OptionData -> Dropdown.OptionData
        foreach (Dropdown.OptionData option in channelDropdown.options)
        {
            string channelKey = option.text;
            if (!chatHistories.ContainsKey(channelKey))
            {
                chatHistories.Add(channelKey, new List<string>());
            }
        }
    }

    // 드롭다운 값이 변경될 때 호출되는 함수
    private void OnChannelChanged(int index)
    {
        string newChannelKey = channelDropdown.options[index].text;

        if (newChannelKey == currentChannelKey)
        {
            return;
        }

        currentChannelKey = newChannelKey;
        LoadChannelHistory(currentChannelKey);
    }

    // 기존 채팅 UI를 모두 지우고, 선택된 채널의 기록을 새로 로드하는 함수
    private void LoadChannelHistory(string channelKey)
    {
        // 1. 현재 'Content'의 모든 자식(메시지)을 파괴
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // 2. chatHistories에서 'channelKey'에 해당하는 메시지 리스트를 가져옴
        List<string> history = chatHistories[channelKey];

        // 3. 해당 리스트의 모든 메시지를 UI로 다시 생성
        foreach (string message in history)
        {
            InstantiateMessage(message);
        }

        // 4. 로드 후 즉시 스크롤을 맨 아래로
        StartCoroutine(ForceScrollToBottom());
    }


    // --- 메시지 전송 로직 ---

    private void OnSubmitChat(string message)
    {
        TrySendMessage();
    }

    private void OnClickSend()
    {
        TrySendMessage();
    }

    private void TrySendMessage()
    {
        string message = chatInputField.text;
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        // 1. UI에 메시지 추가
        InstantiateMessage(message);

        // 2. *현재 채널*의 데이터 저장소(Dictionary)에 메시지 추가
        if (chatHistories.ContainsKey(currentChannelKey))
        {
            chatHistories[currentChannelKey].Add(message);
        }

        // 3. 입력 필드 초기화 및 포커스
        chatInputField.text = "";
        chatInputField.ActivateInputField(); // 레거시 InputField에서도 동일하게 작동

        // 4. 스크롤 맨 아래로
        StartCoroutine(ForceScrollToBottom());
    }

    // UI에 메시지 프리팹을 생성하는 함수
    private void InstantiateMessage(string messageText)
    {
        GameObject newMessageObj = Instantiate(messagePrefab, content);

        // [변경] TMP_Text -> Text
        Text textComponent = newMessageObj.GetComponent<Text>();
        if (textComponent != null)
        {
            textComponent.text = messageText;
        }
    }

    // 스크롤을 맨 아래로 내리는 코루틴 (이전과 동일)
    // 스크롤을 맨 아래로 내리는 코루틴
    private IEnumerator ForceScrollToBottom()
    {
        yield return new WaitForEndOfFrame();

        // 이 부분을 0f 대신 1f로 변경합니다.
        scrollRect.verticalNormalizedPosition = 1f; // 0f -> 1f
    }
}