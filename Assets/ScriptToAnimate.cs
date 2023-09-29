using UnityEngine;
using System.Collections;
public class ScriptToAnimate : MonoBehaviour
{
    public Animator animator;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            animator.SetTrigger("Idle");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Kick");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetTrigger("Walk");
        }
    }
}