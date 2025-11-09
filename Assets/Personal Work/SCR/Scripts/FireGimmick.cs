using System.Collections.Generic;
using UnityEngine;

public class FireGimmick : MonoBehaviour
{
    [SerializeField] GameObject openDoorObj;
    [SerializeField] GameObject closeDoorObj;
    [SerializeField] private bool isFlame;
    [SerializeField] private List<bool> isOn;
    [SerializeField] private List<Gimmic> gimmics;

    void Start()
    {
        isFlame = false;
        SetDoor();
        for (int i = 0; i < gimmics.Count; i++)
        {
            int index = i;
            gimmics[i].OnGimmic += (signal) => SendSignal(signal, index);
            isOn.Add(false);
        }
    }

    public void SendSignal(bool signal, int index)
    {
        isOn[index] = signal;
        SetOpen();
    }

    private void SetOpen()
    {
        isFlame = true;
        foreach (bool on in isOn)
        {
            if (!on)
            {
                isFlame = false;
                break;
            }
        }
        SetDoor();
    }


    public void SetDoor()
    {
        openDoorObj.SetActive(isFlame);
        closeDoorObj.SetActive(!isFlame);
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        SetDoor();
    }

}
