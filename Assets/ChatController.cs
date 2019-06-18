using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatController : MonoBehaviour
{
    public GameObject scrollView_go;
    public GameObject chatScrollContainer;
    RectTransform chatScrollRectTransform;

    public GameObject questionChat_pf;
    public GameObject answerChat_pf;

    float chatDelay = 1;
    bool runCountdown = false;
    bool countdownFinished = false;
    bool animateScrollView = false;
    float yScrollPosition = -10f;
    float scrollViewHeight = 0f;
    //space in between messages
    float offset = 25f;

    //tmp string
    string[] answers = new string[3];

    // Start is called before the first frame update
    void Start()
    {
        answers[0] = "first answer";       
        answers[1] = "second answer";       
        answers[2] = "third answer";

        chatScrollRectTransform = chatScrollContainer.GetComponent<RectTransform>();
        scrollViewHeight = scrollView_go.GetComponent<RectTransform>().sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (runCountdown) {
            chatDelay -= Time.deltaTime * 0.1f;
            if(chatDelay <= 0) {
                countdownFinished = true;
                runCountdown = false;
            }
        }

        if (countdownFinished) {
            countdownFinished = false;
            PushQuestion();
        }

        if (animateScrollView) {
            AnimateScrollView();
        }
          
    }

    void PushQuestion() {

    }

    public void PushAnswer(int answerPosition) {
        Debug.Log("pushing answer");
        //unity buttons allow only one parameter
        bool isPlayerWriting = true;
        if (answerPosition == 3){
            isPlayerWriting = false;
            answerPosition = 2;
        }
           

        GameObject speechbubble_go;

        if(isPlayerWriting)
            speechbubble_go = Instantiate(questionChat_pf, chatScrollContainer.transform);
        else
            speechbubble_go = Instantiate(answerChat_pf, chatScrollContainer.transform);

        //place the answer below the last one
        RectTransform rectangleTransform = speechbubble_go.GetComponent<RectTransform>();
        rectangleTransform.anchoredPosition = new Vector2(0, yScrollPosition);
        //increase the content container
        chatScrollRectTransform.sizeDelta = new Vector2(chatScrollRectTransform.sizeDelta.x, chatScrollRectTransform.sizeDelta.y + rectangleTransform.sizeDelta.y + offset);
        //Scroll position plus offset
        yScrollPosition -= rectangleTransform.sizeDelta.y + offset;
        //TODO: set text 
        speechbubble_go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = answers[answerPosition];
        //TODO: time
        speechbubble_go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Time.time.ToString();

        animateScrollView = true;
    }

    void PushSpeechBubble( ) {

    }

    void AnimateScrollView() {
        //TODO: magic numbers
        if (scrollViewHeight < yScrollPosition * -1 && chatScrollRectTransform.anchoredPosition.y < (yScrollPosition+800) * -1) {
            chatScrollRectTransform.anchoredPosition = new Vector2(chatScrollRectTransform.anchoredPosition.x, chatScrollRectTransform.anchoredPosition.y + 5);

            if (chatScrollRectTransform.anchoredPosition.y > (yScrollPosition+800) * -1)
                animateScrollView = false;
        }
    }
}
