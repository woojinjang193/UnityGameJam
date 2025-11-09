using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isEat = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isEat)
            {
                isEat = true;
                Manager.Game.Coin++;
                Debug.Log("코인 먹음");
                Debug.Log($"{Manager.Game.Coin}");
                Destroy(gameObject);
            }


        }
    }
}
