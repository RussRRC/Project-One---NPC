using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject[] army;
    [SerializeField] private GameObject soldier, secondarySoldier;
    [SerializeField] private int reinforcements;
    [SerializeField] private float spawnInterval;
    [SerializeField] private int spawnRadius;
    [SerializeField] private float lifetime;
    [SerializeField] private float timer = 0, secondWave = 42, secondaryTimer = 0;
    [SerializeField] private int counter;
    [SerializeField] private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        army = new GameObject[reinforcements];
        target = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        counter = 0;
        timer += Time.deltaTime;
        secondaryTimer += Time.deltaTime;
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
        }if(secondWave < 0 && secondaryTimer > spawnInterval * 3)
        {
            GameObject go = Instantiate(secondarySoldier, leftOrRightOfPlayer(), transform.rotation);
            secondaryTimer = 0; 
        }
        for (int i = 0; i < army.Length; i++)
        {
            if (army[i] != null)
            {
                counter++;
            }
        }
        if(secondWave > 0)
        {
            secondWave -= Time.deltaTime;
        }
    }

    private Vector3 leftOrRightOfPlayer()
    {
        int side = Random.Range(1, 3);
        if (side == 1)
        {
            return target.transform.position + new Vector3(-30, 0);
        }
        else
        {
            return target.transform.position + new Vector3(30, 0);
        }
    }
    private Vector3 RandomPositionInWanderRadius()
    {
        //randomly generate a number within radius limit of NPC, and add it to NPC's current position
        //radius is limited by starting point of NPC

        //Generates random point in a circle, then multiplies it by radius in order to get true length
        //then adds starting point in order to adjust for where the NPC is setup for movement.
        return Random.insideUnitCircle * (spawnRadius * new Vector2(1, 0.2f)) + new Vector2(transform.position.x, transform.position.y);
    }
}
