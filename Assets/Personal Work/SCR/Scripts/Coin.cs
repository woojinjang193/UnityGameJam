using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Manager.Game.Coin++;
            Debug.Log("코인 먹음");
            Debug.Log($"{Manager.Game.Coin}");
            Destroy(gameObject);
        }
    }
}
