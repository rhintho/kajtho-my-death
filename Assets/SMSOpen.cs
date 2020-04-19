using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMSOpen : MonoBehaviour
{
    public GameObject smsMenuUser;
    // Start is called before the first frame update
    void Start()
    {
        if (smsMenuUser == null)
            Debug.Log("register sms menu user from SMS Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
