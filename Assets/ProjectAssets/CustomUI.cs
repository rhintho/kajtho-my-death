using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUI : MonoBehaviour {
    float speed = 0.1f;
    public GameObject[] openedApps;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            if (touchDeltaPosition.x < -10) {
                SwipeRight();
            }
            if (touchDeltaPosition.x > 10) {
                SwipeRight();
            }

        }
    }

    void SwipeRight() {
        Debug.Log("swipe right");
    }

    void SwipeLeft() {
        Debug.Log("swipe left");
    }

    public void CloseApps() {
        openedApps = GameObject.FindGameObjectsWithTag("App");

        foreach(GameObject app in openedApps){
            app.SetActive(false);
        }

    }
}
