using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class GameRecord : MonoBehaviour
{
    
    public string fileName = "recordData.json";


    [SerializeField] TMP_Text easyRecordText;
    [SerializeField] TMP_Text normalRecordText;
    [SerializeField] TMP_Text hardRecordText;


    public float easyRecord;
    public float normalRecord;
    public float hardRecord;

    string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.dataPath, fileName);
        LoadData();
    }

    public void AddData(int x, float data)
    {
        if(x == 0)
        {
            if(data < easyRecord)
                easyRecord = data;
        }
        else if(x == 1)
        {
            if(data < normalRecord)
                normalRecord = data;
        }
        else if(x == 2)
        {
            if(data < hardRecord)
                hardRecord = data;
        }

        SaveData();
    }

    void SaveData()
    {
        GameRecordData data = new GameRecordData();
        data.easy = easyRecord;
        data.normal = normalRecord;
        data.hard = hardRecord;

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            GameRecordData data = JsonUtility.FromJson<GameRecordData>(jsonData);

            easyRecord = data.easy;
            normalRecord = data.normal;
            hardRecord = data.hard;

        }
        else
        {
            easyRecord = 3600;
            normalRecord = 3600;
            hardRecord = 3600;
            SaveData();
        }
        

        if(SceneManager.GetActiveScene().name == "Main")
            UpdateRecordText();
    }

    void UpdateRecordText()
    {
        easyRecordText.text = ConvertToText(easyRecord);
        normalRecordText.text = ConvertToText(normalRecord);
        hardRecordText.text = ConvertToText(hardRecord);
    }

    string ConvertToText(float time)
    {
        if(time > 3599)
            return "-";
            
        return string.Format("{0:D2}:{1:D2}", (int)time / 60, (int)time % 60);
    }


    public void StartGame(int x)
    {
        if(x == 0)
        {
            SceneManager.LoadScene("Easy");
        }
        else if(x == 1)
        {
            SceneManager.LoadScene("Normal");
        }
        else if(x == 2)
        {
            SceneManager.LoadScene("Hard");
        }
    }
}


[System.Serializable]
public class GameRecordData
{
    public float easy;
    public float normal;
    public float hard;
}