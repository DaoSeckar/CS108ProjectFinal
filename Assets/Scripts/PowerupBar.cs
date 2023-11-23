using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PowerupBar : MonoBehaviour
{
    public Image powerupBar;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        powerupBar.fillAmount = 1;
        StartCoroutine(barStart(7f));
    }

    private void OnDisable() {

        StopAllCoroutines();
    }
   

    IEnumerator barStart(float seconds)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            powerupBar.fillAmount = Mathf.Lerp(1f, 0f, lerpValue);
            yield return null;
        }

        
    }
}
