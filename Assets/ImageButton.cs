using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageButton : MonoBehaviour
{
    Button m_button;
    Sprite m_image;
    public Image modalImage;
    // Start is called before the first frame update
    void Start()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(() => SetModalImage());

        m_image = transform.GetChild(0).GetComponent<Image>().sprite;
    }

    void SetModalImage() {
        modalImage.sprite = m_image;
    }

    public void OnDestroy() {
        m_button.onClick.RemoveListener(() => SetModalImage());
    }
}
