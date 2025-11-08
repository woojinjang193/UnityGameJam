using System;
using UnityEngine;

public class PGButton : Gimmic
{
    [SerializeField] GameObject OnButtonObj;
    [SerializeField] GameObject OffButtonObj;

    void Start()
    {
        isOn = false;
        SetButton();
    }

    public void SetButton()
    {
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
