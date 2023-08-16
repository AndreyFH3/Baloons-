using System.Collections;
using UnityEngine;

public delegate void Bonus();
public delegate void Points(int value);
[RequireComponent(typeof(Rigidbody2D))]
public class Baloon : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    [SerializeField, Range(0f, 50f)] private float _sideForceTime = 15f;
    [SerializeField, Range(0f, 50f)] private float _sideForce = 5f;
    private float _sideForceTimePrivate = 0;
    [Space]
    [SerializeField] private float _upSpeed = 10f;
    [SerializeField] private float _timeToDestroy = 3f;
    [SerializeField] private AnimationCurve _sideEffec;
    private Vector3 _startPosition = new Vector3();
    [SerializeField] private SpriteRenderer _sprite;
    private Bonus _bonus;
    private Points _points;

    // Start is called before the first frame update
    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(LifeTimeDestroy());
        _startPosition = transform.position;
    }

    private IEnumerator LifeTimeDestroy()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        Destroyed(false);
    }

    public void Destroyed(bool isClicked)
    {
        if (isClicked)
        {
            _bonus?.Invoke();
            _points?.Invoke(100);
        }
        _bonus = null;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        transform.position = _startPosition;
    }

    void FixedUpdate()
    {
        _sideForceTimePrivate += Time.fixedDeltaTime;
        float x = _sideEffec.Evaluate(_sideForceTimePrivate/_sideForceTime) * _sideForce;
        Vector2 dir = new Vector2(x, _upSpeed * Time.fixedDeltaTime);
        _rb2d.velocity = dir;
        if(_sideForceTimePrivate>_sideForceTime) _sideForceTimePrivate = 0;
    }

    public void SetBonus(Bonus b)
    {
        _bonus = b;
    }

    public void AddPoints(Points points)
    {
        _points = points;
    }

    public void SetColor(Color color)
    {
        _sprite.color = color;
    }
}
