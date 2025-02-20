using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    

    [SerializeField] Transform target;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GunData[] gunDatas;

    
    [SerializeField] float minRange;
    [SerializeField] float maxRange;
    [SerializeField] float range
    {
        get { return Random.Range(minRange, maxRange); }
    }
    int spawnNum = 0;
    


    void Start()
    {
        // InvokeRepeating(nameof(SpawnEnemy), 1f, 1f);
        StartCoroutine(nameof(SpawnEnemy));
    }
    
    void Update()
    {
        
    }


    IEnumerator SpawnEnemy()
    // void SpawnEnemy()
    {
        while (true)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
            if(spawnPos == Vector3.zero)
                spawnPos = Vector3.up;

            spawnPos *= range;
            spawnPos += target.position;

            GameObject enemy = Instantiate(enemyPrefabs[spawnNum++ % enemyPrefabs.Length], transform);
            enemy.transform.position = spawnPos;
            
            enemy.GetComponent<EnemyPistol>().data = gunDatas[spawnNum++ % gunDatas.Length];

            yield return new WaitForSeconds(1f);
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.position, minRange);   
        Gizmos.DrawWireSphere(target.position, maxRange);
    }
}