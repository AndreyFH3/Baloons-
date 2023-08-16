using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SavePoints : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _saveButton;
    [SerializeField] private GameController _controller;
    private List<Data> _players = new List<Data>();
    [SerializeField] private Transform _scoreResult;
    [SerializeField] private DataPlane _dataPlane;
    [SerializeField] private GameObject _secondEndGame;

    private void OnEnable()
    {
        GetJson();
        _inputField.onValueChanged.AddListener(CompareString);
        _saveButton.onClick.AddListener(AddData);
        _saveButton.onClick.AddListener(ShowInSecondScreen);
    }

    private void SaveJson()
    {

        string filepath = Application.dataPath + "/gameData.json";
        if (File.Exists(filepath))
        {
            File.Delete(filepath);

        }
        Data[] data = new Data[_players.Count];
        int counter = 0;
        foreach (Data item in _players)
        {
            data[counter] = new Data(item.name, item.score);
            counter++;
        }
        var json = JsonUtility.ToJson(new DataArray(data));
        File.WriteAllText(filepath, json);
    }
    
    private void ShowInSecondScreen()
    {
        _secondEndGame.SetActive(true);
        Sort(_players);
        _players.Reverse(); 
        foreach (Data item in _players)
        {
            DataPlane plane = Instantiate(_dataPlane, _scoreResult);
            plane.SetData(item.name, item.score);
        }
    }

    public void CompareString(string s)
    {
        if (s.Length > 8)
        {
            string res = "";
            for (int i = 0; i < 9; i++)
            {
                res += s[i].ToString();
            }
            _inputField.text = res;
        }
    }

    private void AddData()
    {
        string name = _inputField.text;
        Data d = _players.Find(x => x.name == name);
        if(d == null)
        {
            _players.Add(new Data(name, _controller.Points));
        }
        else
        {
            d.score = _controller.Points;
        }
        SaveJson();
    }
    private void Sort(List<Data> arr)
    {
        for (int write = 0; write < arr.Count; write++)
        {
            for (int sort = 0; sort < arr.Count - 1; sort++)
            {
                if (arr[sort].score > arr[sort + 1].score)
                {
                    Data temp = arr[sort + 1];
                    arr[sort + 1] = arr[sort];
                    arr[sort] = temp;
                }
            }
        }
    }
    private void GetJson()
    {
        string filepath = Application.dataPath + "/gameData.json";
        Debug.Log(filepath);
        if (File.Exists(filepath))
        {
            var readAll = File.ReadAllText(filepath);
            DataArray d = JsonUtility.FromJson<DataArray>(readAll);
            foreach (Data data in d.data)
            {
                _players.Add(new Data(data.name, data.score));
            }
        }
    }
}

[System.Serializable]
public class Data
{
    public string name;
    public int score;
    public Data(string name, int score)
    { 
        this.name = name;
        this.score = score;
    }
}
[System.Serializable]
public class DataArray
{
    public Data[] data;
    public DataArray(Data[] data) { this.data = data; }
}