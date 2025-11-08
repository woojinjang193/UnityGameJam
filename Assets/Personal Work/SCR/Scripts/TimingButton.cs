using System;
using System.Collections;
using UnityEngine;

public class TimingButton : Gimmic
{
    [SerializeField] GameObject OnButtonObj;
    [SerializeField] GameObject OffButtonObj;
    private Coroutine pressRoutine;
    void Start()
    {
        isOn = false;
        SetButton(isOn);
    }

    public void SetButton(bool open)
    {
        OnButtonObj.SetActive(open);
        OffButtonObj.SetActive(!open);

    }

    public void SetTrigger()
    {
        if (pressRoutine != null) StopCoroutine(pressRoutine);

        if (isOn) pressRoutine = StartCoroutine(OnSignal());
        else OnGimmic?.Invoke(isOn);
    }

    public IEnumerator OnSignal()
    {
        OnGimmic?.Invoke(true);

        yield return new WaitForSeconds(0.1f);

        OnGimmic?.Invoke(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOn = true;
            SetTrigger();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOn = false;
            SetTrigger();
        }
    }
}
