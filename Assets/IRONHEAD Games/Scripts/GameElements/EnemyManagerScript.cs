using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour
{
    public List<GameObject> SpawnPoints = new List<GameObject>();           //list of potential spawn points

    public GameObject enemyUnit;        //physical manifestation of the bug units
    public GameObject shooterUnit;      //physical manifestation of the bug units
    public GameObject playerHealthBox;

    public AudioSource SeekerDeathEffect; //death SFX for the 'bug' enemies (TODO: should just be a generic "DeathEffect" for all enemies)

    private float totalGameTime = 0.0f;     //time since the last enemy spawned
    private float spawnDelay = 5f;

    //Test flags, not used in release
    private bool onlySpawnOne = false;
    private bool spawned = false;

    public int EnemiesSpawned = 0;      //number of enemies used in game so far

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStateManagerScript.Instance.GameActive == false)
        {
            return; //game stopped, possibly a pause
        }

        float localspawnDelay = spawnDelay;

        totalGameTime += Time.deltaTime;

        if(totalGameTime > localspawnDelay)
        {
            localspawnDelay -= 0.1f;
            if(localspawnDelay <= 0.1f)
            {
                localspawnDelay = 0.1f;
            }

            totalGameTime = 0;

            SpawnEnemy();
        }
    }


    /// <summary>
    /// CReates a new enemy in the arena
    /// </summary>
    public void SpawnEnemy()
    {
        //Test routine. Spawn 1 and only 1 enemy
        /*if(onlySpawnOne && spawned)
        {
            return;
        }

        spawned = true;*/

        //update total number of enemies used so far
        EnemiesSpawned += 1;

        //chose spawn location from list
        int maxIndex = this.SpawnPoints.Count - 1;
        int spawnSelected = Random.Range(0, maxIndex);
        GameObject spawnLocation = this.SpawnPoints[spawnSelected];

        //Chose what kind of enemy to spawn
        //Every 5th enemy is a 'shooter'
        if (EnemiesSpawned % 5 == 0)
        {
            Instantiate(shooterUnit, spawnLocation.transform);
        }
        else
        {
            Instantiate(enemyUnit, spawnLocation.transform);
        }

        //every 10th enemy, spawn a heal powerup
        if (EnemiesSpawned % 10 == 0)
        {
            Instantiate(playerHealthBox, spawnLocation.transform);
        }
  
    }
}
