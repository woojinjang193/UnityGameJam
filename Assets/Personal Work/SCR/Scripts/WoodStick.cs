using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class WoodStick : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> renderers;
    [SerializeField] GameObject normalStick;
    [SerializeField] GameObject fireStick;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FlameOn()
    {
        normalStick.SetActive(false);
        fireStick.SetActive(true);
        gameObject.tag = "Fire";
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fire"))
        {
            animator.Play("FireOn");
        }
    }

    public void ResetFire()
    {
        normalStick.SetActive(true);
        fireStick.SetActive(false);
        gameObject.tag = "Box";
        animator.Play("Idle");
    }

    public void EchoStick()
    {
        foreach (SpriteRenderer sR in renderers)
        {
            Color es = sR.color;
            es.a = 0.3f;
            sR.color = es;
        }
    }
}
