using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollView : MonoBehaviour
{
    [SerializeField]
    public float yScrollPosition {
        get { return m_yScrollPosition; }
        set { m_yScrollPosition = value;
        }
    }
    public float m_yScrollPosition;

}
