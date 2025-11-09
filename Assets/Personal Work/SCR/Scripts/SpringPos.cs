using System;
using System.Collections.Generic;
using UnityEngine;

public class SpringPos : MonoBehaviour
{
    [SerializeField] private float pushPower = 20f;
    [SerializeField] private Spring spring;
    [SerializeField] private SpringButton sb;
    [SerializeField] private Vector2 powerPos;

    private List<Collider2D> collisions = new List<Collider2D>();

    void Start()
    {
        spring.pushElse += PushObj;
        sb.OnGimmic += spring.PushButton;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") ||
        collision.CompareTag("Box"))
        {
            Debug.Log("스프링 오브젝트 추가");
            if (!collisions.Contains(collision))
            {
                collisions.Add(collision);
            }
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collisions.Contains(collision))
        {
            Debug.Log("스프링 오브젝트 제거");
            collisions.Remove(collision);
        }
    }

    private void PushObj()
    {
        foreach (Collider2D coll in collisions)
        {
            //Debug.Log("오브젝트 띠용~");
            if (coll == null) continue;
            Rigidbody2D rb = coll.attachedRigidbody;

            if (coll.gameObject.GetComponent<PlayerController>() != null)
            {
                coll.gameObject.GetComponent<PlayerController>().PushPlayer();
            }
            else if (coll.gameObject.GetComponent<BoxInteraction>() != null)
            {
                coll.gameObject.GetComponent<BoxInteraction>().PushBox();
            }

            if (rb != null)
            {
                Debug.Log("리지드 있음 힘을 내");
                rb.AddForce(pushPower * powerPos, ForceMode2D.Impulse);
            }
        }
        collisions.Clear();
    }
}
