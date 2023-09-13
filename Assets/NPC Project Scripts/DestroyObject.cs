using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{

    private int destroyTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitUntilDeath());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator waitUntilDeath()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
