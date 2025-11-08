using System;
using System.Collections.Generic;
using UnityEngine;

public class TimingTrigger : Gimmic
{
    [SerializeField] private List<bool> timingSignal;
    [SerializeField] private List<TimingButton> timingButtons;

    void Start()
    {
        isOn = false;
        for (int i = 0; i < timingButtons.Count; i++)
        {
            int index = i;
            timingButtons[i].OnGimmic += (signal) => SendSignal(signal, index);
            timingSignal.Add(false);
        }
    }

    public void SendSignal(bool signal, int index)
    {
        timingSignal[index] = signal;
        SetTiming();
    }

    private void SetTiming()
    {
        isOn = true;
        foreach (bool on in timingSignal)
        {
            if (!on)
            {
                isOn = false;
                break;
            }
        }
        if (isOn) SetTrigger();
    }

    public void SetTrigger()
    {
        foreach (TimingButton btn in timingButtons)
        {
            btn.SetButton(true);
        }
        OnGimmic?.Invoke(isOn);
    }
}
