using UnityEngine;

public class StartFadeIn : MonoBehaviour
{
    [SerializeField] private GameObject _fadeIn;

    public void FadeIn()
    {
        _fadeIn.SetActive(true);
        gameObject.SetActive(false);
    }    
}
