using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour
{

   // private List<EnemyScript> enemyManifest = new List<EnemyScript>();

    public GameStateManagerScript gameManager = null;

    public List<GameObject> SpawnPoints = new List<GameObject>();

    public GameObject enemyUnit;
    public GameObject shooterUnit;
    public GameObject playerHealthBox;

    public AudioSource SeekerDeathEffect;
    //public GameObject target;

    private float totalGameTime = 0.0f;
    private float spawnDelay = 5f;

    private bool onlySpawnOne = false;
    private bool spawned = false;

    public int EnemiesSpawned = 0;

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

    public void ChangeLevel()
    {
        gameManager.CurrentLevel++;
    
    
    }

    public void SpawnEnemy()
    {
        if(onlySpawnOne && spawned)
        {
            return;
        }

        spawned = true;

        int maxIndex = this.SpawnPoints.Count - 1;

        int spawnSelected = Random.Range(0, maxIndex);
        //int spawnSelected = 0;

        GameObject spawnLocation = this.SpawnPoints[spawnSelected];

        if (EnemiesSpawned > 0)
        {

            if (EnemiesSpawned % 5 == 0)
            {
                Instantiate(shooterUnit, spawnLocation.transform);
            }
            else
            {
                Instantiate(enemyUnit, spawnLocation.transform);
            }

            if (EnemiesSpawned % 10 == 0)
            {
                Instantiate(playerHealthBox, spawnLocation.transform);
            }
        }
        else
        {
            Instantiate(enemyUnit, spawnLocation.transform);
        }


        //DebugManagerScript.Instance.AddMessage("Enemy spawned at " + spawnLocation.transform.position.ToString());

        EnemiesSpawned += 1;

        
    }

    private void BuildEnemyManifest()
    {

    }
}
