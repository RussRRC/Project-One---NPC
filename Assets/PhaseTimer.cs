using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTimer : MonoBehaviour
{
    public Animator animation;
    [SerializeField] float timer;
    private float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime <= timer)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            animation.SetBool("Phase 2", true);
        }
    }
}
