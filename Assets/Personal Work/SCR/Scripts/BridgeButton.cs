using System;
using UnityEngine;

public class BridgeButton : Gimmic
{
    [SerializeField] GameObject OnButtonObj;
    [SerializeField] GameObject OffButtonObj;
    [SerializeField] GameObject CreateBridgeObj;
    [SerializeField] GameObject DeleteBridgeObj;
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
        CreateBridgeObj.SetActive(isOn);
        OffButtonObj.SetActive(!isOn);
        DeleteBridgeObj.SetActive(!isOn);
        OnGimmic?.Invoke(isOn);
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        SetButton();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNum++;
            SetButton();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNum--;
            SetButton();
        }
    }
}
