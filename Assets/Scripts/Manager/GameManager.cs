using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameRecord record;

    [SerializeField] AbstractMapGenerator mapGenerator;


    public List<GameObject> monsters;

    float curTime = 0f;
    int curScene;
    
    void Awake()
    {
        if(!instance)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if(SceneManager.GetActiveScene().name == "Easy")
            curScene = 0;
        else if(SceneManager.GetActiveScene().name == "Normal")
            curScene = 1;
        else if(SceneManager.GetActiveScene().name == "Hard")
            curScene = 2;
            

        mapGenerator.GenerateMap();
    }

    public void AddMonster(GameObject _monster)
    {
        monsters.Add(_monster);
    }

    public void RemoveMonster(GameObject _monster)
    {
        monsters.Remove(_monster);

        if(monsters.Count <= 0)
        {
            record.AddData(curScene, curTime);
            SceneManager.LoadScene("Main");
        }
    }

    
    void Update()
    {
        curTime += Time.deltaTime;
    }
}
