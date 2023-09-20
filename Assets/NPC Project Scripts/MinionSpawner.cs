using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject[] army;
    [SerializeField] private GameObject soldier;
    [SerializeField] private int reinforcements;
    [SerializeField] private float spawnInterval;
    [SerializeField] private int spawnRadius;
    [SerializeField] private float lifetime;
    [SerializeField] private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        army = new GameObject[reinforcements];

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > spawnInterval)
        {
            int spawnPosition = Random.Range(0, army.Length - 1);
            if (army[spawnPosition] == null) {
                GameObject go = Instantiate(soldier, RandomPositionInWanderRadius(), transform.rotation);
                go.name = go.name + "(" + spawnPosition + ")";
                army[spawnPosition] = go;
                Destroy(go, Random.Range(lifetime/2, lifetime));
            }
            timer = 0;
        }
    }

    Vector3 RandomPositionInWanderRadius()
    {
        //randomly generate a number within radius limit of NPC, and add it to NPC's current position
        //radius is limited by starting point of NPC

        //Generates random point in a circle, then multiplies it by radius in order to get true length
        //then adds starting point in order to adjust for where the NPC is setup for movement.
        return Random.insideUnitCircle * spawnRadius + new Vector2(transform.position.x, transform.position.y);
    }
}
