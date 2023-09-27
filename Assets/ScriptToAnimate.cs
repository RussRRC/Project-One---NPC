using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptToAnimate : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] walk;
    public Sprite[] idle;
    public Sprite[] jump;
    void Start()
    {
        StartCoroutine(Idle());
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            StopAllCoroutines();
            StartCoroutine(Jump());
        }
        if (Input.GetButtonDown("Horizontal"))
        {
            StopAllCoroutines();
            StartCoroutine(Walk());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Idle());
        }
    }
    IEnumerator Idle()
    {
        int i;
        i = 0;
        while (i < idle.Length)
        {
            spriteRenderer.sprite = idle[i];
            i++;
            yield return new WaitForSeconds(0.2f);
            yield return 0;

        }
        StartCoroutine(Idle());
    }
    IEnumerator Walk()
    {
        int i;
        i = 0;
        while (i < walk.Length)
        {
            spriteRenderer.sprite = walk[i];
            i++;
            yield return new WaitForSeconds(0.2f);
            yield return 0;
        }
        StartCoroutine(Walk());
    }
    IEnumerator Jump()
    {
        int i;
        i = 0;
        while (i < jump.Length)
        {
            spriteRenderer.sprite = jump[i];
            i++;
            yield return new WaitForSeconds(0.2f);
            yield return 0;
        }
        StartCoroutine(Jump());
    }
}