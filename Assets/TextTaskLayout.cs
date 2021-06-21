using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TextTaskLayout : MonoBehaviour
{
    GameManager GM;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TMPro.TextMeshProUGUI>().DOFade(1, 2);
    }

    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        GM.OnNextLevel += HandlerNextLevel;
        GM.OnReloadComlete += HandleReload;
        
    }
    private void HandlerNextLevel(string Task)
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = "FIND " + Task;
    }

    private void HandleReload()
    {
        GetComponent<TMPro.TextMeshProUGUI>().DOFade(0, 0);
        GetComponent<TMPro.TextMeshProUGUI>().DOFade(1, 2);
    }
}
