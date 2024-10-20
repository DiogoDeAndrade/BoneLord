using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float        firstSpawnTime;
    public Vector2      nextSpawns;
    public GameObject[] spawnPrefab;
    public Transform[]  spawnPoints;

    float spawnTimer;
    int   spawnCount = 1;

    void Start()
    {
        spawnTimer = firstSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnEnemies();
            spawnTimer = nextSpawns.Random();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            SpawnEnemies();
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            int prefabIndex = Random.Range(0, spawnPrefab.Length);

            Instantiate(spawnPrefab[prefabIndex], spawnPoints[spawnPointIndex].position, Quaternion.identity);
        }

        spawnCount = Mathf.Clamp(spawnCount + 1, 0, spawnPoints.Length);
    }
}
