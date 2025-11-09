using System;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private bool buttonpuess;

    public Action pushElse;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PushButton(bool signal)
    {
        buttonpuess = signal;
        if (buttonpuess) PressSpring();
        else BounceSpring();
    }

    public void PressSpring()
    {
        animator.SetBool("Press", true);
    }

    public void BounceSpring()
    {
        animator.SetBool("Press", false);
        pushElse?.Invoke();
    }
}
