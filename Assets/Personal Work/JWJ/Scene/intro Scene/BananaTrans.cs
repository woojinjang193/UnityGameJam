using UnityEngine;

public class BananaTrans : MonoBehaviour
{
    [SerializeField] private GameObject _banana;
    [SerializeField] private GameObject _bananaPixel;

    public void BananaOff()
    {
        _banana.SetActive(false);
    }

    public void PixelBananaOn()
    {
        _bananaPixel.SetActive(true);
    }
}
