using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatUIController : MonoBehaviour {
    //TODO: get all objects. provide function for back button, make sure one object is always enabled
    // make sure main container gets disabled
    GameObject scrollView_go;
    GameObject chatScrollContainer;
    GameObject m_activeChat;
    RectTransform m_chatScrollRectTransform;

    public GameObject questionChat_pf;
    public GameObject answerChat_pf;

    //button texts. should update this everytime there is an interaction happening
    TextMeshProUGUI playerAnswerOne;
    TextMeshProUGUI playerAnswerTwo;
    TextMeshProUGUI playerAnswerThree;

    List<GameObject> allChats_List = new List<GameObject>();
    //animation flags
    float m_chatDelay = 1;
    bool m_isCountdownRunning = false;
    bool m_isCountdownFinished = false;
    bool m_isScrollViewAnimating = false;
    //magic animation numbers
    float m_yScrollPosition;
    float m_scrollViewHeight = 0f;
    float m_Offset = 25f;     //space in between messages

    // Start is called before the first frame update
    void Start() {
        Transform allChats = this.transform.GetChild(1);

        foreach (Transform child in allChats)
            allChats_List.Add(child.gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (m_isCountdownRunning) {
            m_chatDelay -= Time.deltaTime * 0.1f;
            if (m_chatDelay <= 0) {
                m_isCountdownFinished = true;
                m_isCountdownRunning = false;
            }
        }

        if (m_isCountdownFinished) {
            m_isCountdownFinished = false;
            //PushQuestion();
            m_chatDelay = 1;
        }

        if (m_isScrollViewAnimating) {
            AnimateScrollView();
        }

    }
    public void UpdateActiveChat(GameObject activeChat) {
        m_activeChat = activeChat;
        foreach (GameObject chat in allChats_List) {
            if (chat.Equals(m_activeChat))
                m_activeChat.SetActive(true);
            else {
                chat.SetActive(false);
            }
        }

        //get active transforms for animation purposes
        scrollView_go = m_activeChat.transform.GetChild(1).GetChild(0).gameObject;
        chatScrollContainer = scrollView_go.transform.GetChild(0).gameObject;
    }

    public void PushSpeechbubble(string _message, bool isPlayer) {
        GameObject speechbubble_go;
        if (isPlayer)
            speechbubble_go = Instantiate(questionChat_pf, chatScrollContainer.transform);
        else
            speechbubble_go = Instantiate(answerChat_pf, chatScrollContainer.transform);

        TextMeshProUGUI message = speechbubble_go.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI date = speechbubble_go.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        message.text = _message;
        date.text = System.DateTime.Now.ToLongTimeString();

        float lineCount = message.GetTextInfo(message.text).lineCount;
        TMP_LineInfo[] lineHeight = message.GetTextInfo(message.text).lineInfo;
        float height = lineCount * lineHeight[0].lineHeight + 110; //magic number

        RectTransform rectangleTransform = speechbubble_go.GetComponent<RectTransform>();
        UpdateRectangleTransform(rectangleTransform, height);

    }

    public void UpdateButtonText(string[] nodeDataComments, VIDE_Assign currentDialogue) {
        playerAnswerOne = m_activeChat.transform.GetChild(2).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        playerAnswerTwo = m_activeChat.transform.GetChild(3).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        playerAnswerThree = m_activeChat.transform.GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        if (nodeDataComments.Length == 1) {
            playerAnswerOne.text = nodeDataComments[0];
            playerAnswerTwo.text = "";
            playerAnswerThree.text = "";
        }
        else {
            playerAnswerOne.text = nodeDataComments[0];
            playerAnswerTwo.text = nodeDataComments[1];
            playerAnswerThree.text = nodeDataComments[2];
        }

    }

    public void DisableButtons() {
        playerAnswerOne.transform.parent.gameObject.SetActive(false);
        playerAnswerTwo.transform.parent.gameObject.SetActive(false);
        playerAnswerThree.transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// will change the size of the speech bubble according to content
    /// will update the size of the container of all chatitems
    /// will trigger the animation if the chat would be out of the viewportz
    /// </summary>
    void UpdateRectangleTransform(RectTransform rectangle, float height) {
        RectTransform speechbubbleTransform = rectangle;
        m_yScrollPosition = scrollView_go.transform.parent.GetComponent<ScrollView>().yScrollPosition;
        speechbubbleTransform.anchoredPosition = new Vector2(0, m_yScrollPosition);     //increase the content container
        speechbubbleTransform.sizeDelta = new Vector2(speechbubbleTransform.sizeDelta.x, height);

        m_chatScrollRectTransform = chatScrollContainer.GetComponent<RectTransform>();
        m_chatScrollRectTransform.sizeDelta = new Vector2(m_chatScrollRectTransform.sizeDelta.x, m_chatScrollRectTransform.sizeDelta.y + speechbubbleTransform.sizeDelta.y + m_Offset);    //Scroll position plus offset
        //Update ScrollPosition on Object
        m_yScrollPosition -= speechbubbleTransform.sizeDelta.y + m_Offset;     //add offset
        scrollView_go.transform.parent.GetComponent<ScrollView>().yScrollPosition = m_yScrollPosition;
        m_isScrollViewAnimating = true; //set animation flag
        m_scrollViewHeight = scrollView_go.GetComponent<RectTransform>().sizeDelta.y;
    }

    void AnimateScrollView() {
        //TODO: magic numbers
        if (m_scrollViewHeight < m_yScrollPosition * -1 && m_chatScrollRectTransform.anchoredPosition.y < (m_yScrollPosition + 800) * -1) {
            m_chatScrollRectTransform.anchoredPosition = new Vector2(m_chatScrollRectTransform.anchoredPosition.x, m_chatScrollRectTransform.anchoredPosition.y + 5);

            if (m_chatScrollRectTransform.anchoredPosition.y > (m_yScrollPosition + 800) * -1)
                m_isScrollViewAnimating = false;
        }
    }

}
