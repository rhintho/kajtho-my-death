using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDialogueState : MonoBehaviour
{
    int m_currentNode = 0;
    [SerializeField]
    public int currentNode {
        get { return m_currentNode; }
        set { m_currentNode = value; }
    }

    bool m_isPaused = false;
    [SerializeField]
    public bool isPaused {
        get { return m_isPaused; }
        set { m_isPaused = value; }
    }

    bool m_hasStarted = false;
    [SerializeField]
    public bool hasStarted {
        get { return m_hasStarted; }
        set { m_hasStarted = value; }
    }

}
