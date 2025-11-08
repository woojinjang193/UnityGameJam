using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject openDoorObj;
    [SerializeField] GameObject closeDoorObj;
    [SerializeField] private bool isOpen;
    [SerializeField] private List<bool> isOn;
    [SerializeField] private List<Gimmic> gimmics;

    void Start()
    {
        isOpen = false;
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
        isOpen = true;
        foreach (bool on in isOn)
        {
            if (!on)
            {
                isOpen = false;
                break;
            }
        }
        SetDoor();
    }


    public void SetDoor()
    {
        openDoorObj.SetActive(isOpen);
        closeDoorObj.SetActive(!isOpen);
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        SetDoor();
    }


}
