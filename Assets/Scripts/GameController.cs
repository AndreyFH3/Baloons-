using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float _timeToEndGame = 60f;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _pointsText;

    [SerializeField] private Baloon _baloon;

    [SerializeField] private int _amountOfEachTypebaloons = 5;
    [SerializeField] private GameObject _endGameobject;

    private List<Baloon> _generatedBaloons = new List<Baloon>();
    private IEnumerator activator;
    private float _currentTimeScale;
    private int points = 0;
    public int Points { 
        get => points; 
        set 
        { 
            points = value;
            _pointsText.text = $"Очков: {points}";
        } 
    }

    void Start()
    {
        Points = 0;
        for (int j = 0; j < _amountOfEachTypebaloons; j++)
        {
            _generatedBaloons.Add(Instantiate(_baloon, new Vector3(Random.Range(-2f, 2f), -6.7f, 0), Quaternion.identity, transform));
            _generatedBaloons[j].AddPoints(AddPoints);
            _generatedBaloons[j].gameObject.SetActive(false);
        }
        Shuffle(_generatedBaloons);
        StartCoroutine(LeftTimer());
        activator = ActivateBalls();
        StartCoroutine(activator);
    }

    private void AddPoints(int points)
    {
        Points += points;
    }

    private IEnumerator LeftTimer()
    {
        while (_timeToEndGame > 0)
        {
            _timeText.text = $"Осталось: {Mathf.RoundToInt(_timeToEndGame)} сек.";
            _timeToEndGame -= Time.deltaTime;
            yield return null;
        }
        EndGame();  
    }

    private void EndGame()
    {
        _endGameobject.SetActive(true);
        StopCoroutine(activator);
    }

    private IEnumerator ActivateBalls()
    {
        float waitForSeconds = 2f;
        while (true)
        {
            List<Baloon> baloons = _generatedBaloons.FindAll(x => x.gameObject.activeSelf == false);
            if(baloons.Count > 0)
            {
                Baloon b = baloons[Random.Range(0, baloons.Count)];
                b.SetColor(new Color(Random.Range(0, 1.0f), 
                    Random.Range(0, 1.0f), 
                    Random.Range(0, 1.0f), 1.0f ));
                int random = Random.Range(0, 40);
                switch (random)
                {
                    case 0:
                        b.SetBonus(AddTime);
                        break;
                    case 3:
                        goto case 0;
                    case 5:
                        b.SetBonus(AddExtraPoints);
                        break;
                    case 7:
                        goto case 5;

                }
                b.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(waitForSeconds);
            
            if(waitForSeconds > .5f)
                waitForSeconds -= 0.1f;
        }
    }

    public List<T> Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        System.Random rnd = new System.Random();
        while (n > 1)
        {
            int k = rnd.Next(n--);
            T temp = list[n];
            list[n] = list[k];
            list[k] = temp;
        }
        return list;
    }

    public void SetTime()
    {
        if(Time.timeScale >= 1)
        {
            _currentTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
            Time.timeScale = _currentTimeScale;
    }

    #region bonuses

    private void AddTime() => _timeToEndGame += 1f; 

    private void AddExtraPoints()
    {

    }
    #endregion
}
