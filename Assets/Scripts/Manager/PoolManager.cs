using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;


    public GameObject[] bulletPrefabs;
    public List<GameObject>[] spawnedBullets;
    public Queue<GameObject>[] spawnedQueueBullets;

    void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        spawnedQueueBullets = new Queue<GameObject>[bulletPrefabs.Length];
        for (int i = 0; i < bulletPrefabs.Length; i++)
        {
            spawnedQueueBullets[i] = new Queue<GameObject>();
        }

        spawnedBullets = new List<GameObject>[bulletPrefabs.Length];
        for (int i = 0; i < bulletPrefabs.Length; i++)
        {
            spawnedBullets[i] = new List<GameObject>();
        }
    }

    public int GetNumToString(string name)
    {
        int n = 0;

        switch (name)
        {
            case "_Player__0":
                n = 0;
                break;

            case "_Normal__0_0":
                n = 1;
                break;

            case "_Normal__0_1":
                n = 2;
                break;

            case "_Normal__0_2":
                n = 3;
                break;

            case "_Normal__0_3":
                n = 4;
                break;
                
            case "_Spread__2_0":
                n = 5;
                break;
                
            case "_Spread__2_1":
                n = 6;
                break;

            case "_Spread__2_2":
                n = 7;
                break;
                
            case "_Spread__2_3":
                n = 8;
                break;

            // case "_Normal_1":
            //     n = 5;
            //     break;
                
            // case "_Normal_2":
            //     n = 6;
            //     break;

            // case "_Spread_0":
            //     n = 7;
            //     break;

            // case "_Spread_L":
            //     n = 8;
            //     break;

            // case "_Spread_R":
            //     n = 9;
            //     break;

            default:
                break;
        }


        return n;
    }

    public GameObject GetBullet1(int num) // int 대체
    {
        GameObject pool = null;
        
        if(spawnedQueueBullets[num].Count > 0)
            pool = spawnedQueueBullets[num].Dequeue();
        
        if(!pool)
        {
            pool = Instantiate(bulletPrefabs[num], transform);
            pool.name = bulletPrefabs[num].name;
        }

        pool.SetActive(true);

        return pool;
    }

    public GameObject GetBullet(int num) // int 대체
    {
        GameObject pool = null;

        foreach (GameObject item in spawnedBullets[num])
        {
            if(!item.activeSelf)
            {
                pool = item;
                break;
            }
        }
        
        if(!pool)
        {
            pool = Instantiate(bulletPrefabs[num], transform);
            spawnedBullets[num].Add(pool);
        }

        pool.SetActive(true);

        return pool;
    }

    public void SetBullet(int num, GameObject obj)
    {
        spawnedQueueBullets[num].Enqueue(obj);
    }
}
