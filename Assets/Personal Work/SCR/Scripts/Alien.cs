using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField] GameObject rocket;

    public void CheckCoin()
    {
        if (Manager.Game.Coin == 4)
        {
            rocket.SetActive(true);
            gameObject.SetActive(false);
        }

    }
}
