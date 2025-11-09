using System.Collections.Generic;
using UnityEngine;

public class CoinBox : MonoBehaviour
{
    [SerializeField] GameObject CoinObj;
    [SerializeField] private bool canFire;
    [SerializeField] private bool isOpen;
    [SerializeField] private List<bool> isOn;
    [SerializeField] private List<Gimmic> gimmics;

    void Start()
    {
        isOpen = false;
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
        if (isOpen)
        {
            Destroy(gameObject);
            Instantiate(CoinObj, gameObject.transform.position, Quaternion.identity);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fire"))
        {
            if (canFire)
            {
                Destroy(gameObject);
                Instantiate(CoinObj, gameObject.transform.position, Quaternion.identity);
            }
        }
    }
}
