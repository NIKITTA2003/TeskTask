using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReloadGame : MonoBehaviour
{
    public GameObject BackGround;
    public GameObject Button;
    GameManager GM;
    public float duration=0.5f;
    
    Tween Tw;

    public delegate void FadeInEvent();
    public event FadeInEvent OnFadeIn;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();

        GM.OnReload += HandlerReload;
        GM.OnReloadStart += HandlerReloadStart;
        GM.OnReloadComlete += HandlerReloadComlete;
        
    }

    private void HandlerReload()
    {

        Button.SetActive(true);
        BackGround.SetActive(true);
        BackGround.GetComponent<UnityEngine.UI.Image>().DOFade(0.7f, duration);
    }

    private void HandlerReloadComlete()
    {
        Tw = BackGround.GetComponent<UnityEngine.UI.Image>().DOFade(0, duration);
        TweenCallback MyCallback = new TweenCallback(onFadeInEnd);
        Tw.OnComplete(MyCallback);
        
    }

    
    private void HandlerReloadStart()
    {
        Tw = BackGround.GetComponent<UnityEngine.UI.Image>().DOFade(1, duration);
        TweenCallback MyCallback = new TweenCallback(onFadeInStart);
        Tw.OnComplete(MyCallback);

        Button.SetActive(false);

    }

    void onFadeInStart()
    {
        OnFadeIn();
    }
    void onFadeInEnd()
    {
        BackGround.SetActive(false);
    }

}
