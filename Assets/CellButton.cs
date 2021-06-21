using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CellButton : MonoBehaviour
{
    public string Value;
    public Sprite Media;
    public float Rotation;
    //public GameObject gameManager;
    public float duration = 0.2f;

    public Color[] palette;

    GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = Media;
        transform.GetChild(1).Rotate(new Vector3(0, 0, Rotation));
        transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = palette[Random.Range(0, palette.Length)];
        if (gameManager.isAnimate)
            DoBounce(transform);
    }
    
    
    private Sequence DoBounce(Transform t)
    {
        Sequence Bounce = DOTween.Sequence();
        
        Bounce.Append(t.DOScale(0, 0));
        Bounce.Append(t.DOScale(t.localScale*1.2f, duration));
        Bounce.Append(t.DOScale(t.localScale*0.95f, duration));
        Bounce.Append(t.DOScale(t.localScale*1, duration));

        return Bounce;
    }

    private Sequence easeInBounce(Transform t)
    {
        Sequence Bounce = DOTween.Sequence();

        Bounce.Append(t.DOMoveX(t.position.x+2, duration/2));
        Bounce.Append(t.DOMoveX(t.position.x - 2, duration / 2));
        Bounce.Append(t.DOMoveX(t.position.x + 1, duration / 2));
        Bounce.Append(t.DOMoveX(t.position.x - 1, duration / 2));
        Bounce.Append(t.DOMoveX(t.position.x + 0.5f, duration / 2));
        Bounce.Append(t.DOMoveX(t.position.x - 0.5f, duration / 2));
        Bounce.Append(t.DOMoveX(t.position.x + 0.25f, duration / 2));
        Bounce.Append(t.DOMoveX(t.position.x - 0.25f, duration / 2));
        Bounce.Append(t.DOMoveX(t.position.x + 0.05f, duration / 2));
        Bounce.Append(t.DOMoveX(t.position.x - 0.05f, duration / 2));

        return Bounce;
    }

    public void Click()
    {
        if (gameManager.ClickCell(Value))
        {
            TweenCallback MyCallback = new TweenCallback(gameManager.NextLevel);
            DoBounce(transform.GetChild(1)).OnComplete(MyCallback);
            transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        }
        else
            easeInBounce(transform.GetChild(1));
    }
}
