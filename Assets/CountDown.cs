using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] GameObject panel;
    [SerializeField] Image timeImage;
    [SerializeField] Text clock;
    [SerializeField] float duration, currentTime;
    // Start is called before the first frame update
    void Start()
    {
        boss.SetActive(true);
        panel.SetActive(false);
        currentTime = duration;
        clock.text = currentTime.ToString();
        StartCoroutine(TimeIEn());
    }

    IEnumerator TimeIEn()
    {
        while(currentTime >= 0)
        {
            timeImage.fillAmount = Mathf.InverseLerp(0, duration, currentTime);
            clock.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }
        OpenPanel();
    }

    void OpenPanel()
    {
        panel.SetActive(true);
        boss.SetActive(false);
    }

}
