using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataPlane : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _score;

    public void SetData(string name, int score)
    {
        _name.text = name;
        _score.text = score.ToString();
    }
}
