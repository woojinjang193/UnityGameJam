using System.Collections.Generic;
using UnityEngine;

public class FireDoor : MonoBehaviour
{
    [SerializeField] GameObject door;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FlameOn()
    {
        door.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fire"))
        {
            animator.Play("FireOn");
        }
    }
}
