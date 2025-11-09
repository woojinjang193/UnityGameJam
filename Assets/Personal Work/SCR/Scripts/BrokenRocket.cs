using UnityEngine;

public class BrokenRocket : MonoBehaviour
{
    private Animator rocketAni;

    void Awake()
    {
        rocketAni = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            rocketAni.Play("Bomb");
        }
    }

    public void Bomb()
    {
        gameObject.SetActive(false);
    }
}
