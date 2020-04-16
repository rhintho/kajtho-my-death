using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatUIController : MonoBehaviour {
    public GameObject scrollView_go;
    public GameObject chatScrollContainer;
    RectTransform m_chatScrollRectTransform;

    public GameObject questionChat_pf;
    public GameObject answerChat_pf;

    //animation flags
    float m_chatDelay = 1;
    bool m_isCountdownRunning = false;
    bool m_isCountdownFinished = false;
    bool m_isScrollViewAnimating = false;
    //magic animation numbers
    float m_yScrollPosition = -10f;
    float m_scrollViewHeight = 0f;
    float m_Offset = 25f;     //space in between messages

    // Start is called before the first frame update
    void Start() {

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

    public void PushSpeechbubble(string _message, bool isPlayer) {

        GameObject speechbubble_go;
        if(isPlayer)
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


    void UpdateRectangleTransform(RectTransform rectangle, float height) {
        RectTransform speechbubbleTransform = rectangle;
        speechbubbleTransform.anchoredPosition = new Vector2(0, m_yScrollPosition);     //increase the content container
        //TODO: get size from amound of text
        speechbubbleTransform.sizeDelta = new Vector2(speechbubbleTransform.sizeDelta.x, height);

        m_chatScrollRectTransform = chatScrollContainer.GetComponent<RectTransform>();
        m_chatScrollRectTransform.sizeDelta = new Vector2(m_chatScrollRectTransform.sizeDelta.x, m_chatScrollRectTransform.sizeDelta.y + speechbubbleTransform.sizeDelta.y + m_Offset);    //Scroll position plus offset
        m_yScrollPosition -= speechbubbleTransform.sizeDelta.y + m_Offset;     //add offset
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
