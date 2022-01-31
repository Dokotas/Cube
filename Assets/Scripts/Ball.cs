using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManager _gameManager;
    private Rigidbody _rb;

    [SerializeField] private float speed;
    private Vector3 _direction;
    private bool _animationEnd;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(ColdDownAnimation(1f, 100));
    }

    private void FixedUpdate()
    {
        if (_animationEnd)
            _rb.AddForce(_direction * speed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<Ball>())
            _gameManager.PlaySound();

        if (!other.collider.CompareTag("Ground"))
            Destroy(gameObject);
    }

    private IEnumerator ColdDownAnimation(float time, int frames)
    {
        var delta = time / frames;
        var halfTime = time / 2;
        while (time > halfTime)
        {
            time -= delta;
            transform.localScale += Vector3.one * delta;
            yield return new WaitForSeconds(delta);
        }

        while (time > 0)
        {
            time -= delta;
            transform.localScale -= Vector3.one * delta;
            yield return new WaitForSeconds(delta);
        }

        _animationEnd = true;
        _direction = _gameManager.player.transform.position - transform.position;
    }
}