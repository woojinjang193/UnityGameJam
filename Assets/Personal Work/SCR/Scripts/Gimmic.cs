using System;
using UnityEngine;

public class Gimmic : MonoBehaviour
{
    [SerializeField] protected bool isOn;
    public Action<bool> OnGimmic;

    void Start()
    {
        isOn = false;
    }
}
