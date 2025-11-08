using System;
using UnityEngine;

public class BridgeButton : Gimmic
{
    [SerializeField] GameObject OnButtonObj;
    [SerializeField] GameObject OffButtonObj;
    [SerializeField] GameObject CreateBridgeObj;
    [SerializeField] GameObject DeleteBridgeObj;

    void Start()
    {
        isOn = false;
        SetButton();
    }

    public void SetButton()
    {
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
            isOn = true;
            SetButton();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOn = false;
            SetButton();
        }
    }
}
