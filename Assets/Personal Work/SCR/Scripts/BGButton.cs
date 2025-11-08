using System;
using UnityEngine;

public class BGButton : Gimmic
{
    [SerializeField] GameObject OnButtonObj;
    [SerializeField] GameObject OffButtonObj;
    private int playerNum = 0;

    void Start()
    {
        isOn = false;
        SetButton();
    }

    public void SetButton()
    {
        isOn = playerNum > 0;
        OnButtonObj.SetActive(isOn);
        OffButtonObj.SetActive(!isOn);
        OnGimmic?.Invoke(isOn);
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        SetButton();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            playerNum++;
            SetButton();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            playerNum--;
            SetButton();
        }
    }
}
