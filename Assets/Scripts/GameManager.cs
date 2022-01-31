using System;
using GD.MinMaxSlider;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Player player;
    [SerializeField] private GameObject killer, bigger, smaller;
    [SerializeField] private Transform enemySpawnTransformsHolder;

    [SerializeField] [NamedArray(new string[] {"Level 1", "Level 2", "Level 3"})] [MinMaxSlider(0, 1)]
    private Vector2[] enemySpawnChance;

    private Transform[] _enemySpawnTransforms;
    private AudioSource _audio;

    [SerializeField] private float spawnDeltaTime;

    private int _level = 0;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _enemySpawnTransforms =
            enemySpawnTransformsHolder.GetChild(0).GetComponentsInChildren<Transform>()[Range.StartAt(1)];
        StartCoroutine(BallSpawner());
    }

    public void PlaySound()
    {
        _audio.Play();
    }

    public void SelectLevel(int numOfLevel)
    {
        _enemySpawnTransforms =
            enemySpawnTransformsHolder.GetChild(numOfLevel-1).GetComponentsInChildren<Transform>()[Range.StartAt(1)];
    }

    public void Lose()
    {
    }

    public void Win()
    {
        Debug.Log("Win!!!");
    }

    private IEnumerator BallSpawner()
    {
        while (true)
        {
            Transform spawnTransform = _enemySpawnTransforms[Random.Range(0, _enemySpawnTransforms.Length - 1)];;
            RaycastHit hit;
            Physics.Raycast(spawnTransform.position, Vector3.down, out hit);
            
            while (hit.collider.CompareTag("Player"))
            {
                spawnTransform = _enemySpawnTransforms[Random.Range(0, _enemySpawnTransforms.Length - 1)];;
                Physics.Raycast(spawnTransform.position, Vector3.down, out hit); 
            }

            var ball = RandomBall();
            Instantiate(ball, spawnTransform.position, spawnTransform.rotation);

            yield return new WaitForSeconds(spawnDeltaTime);
        }
    }

    private GameObject RandomBall()
    {
        var val = Random.value;

        if (val < enemySpawnChance[_level].x)
            return killer;

        if (val < enemySpawnChance[_level].y)
            return bigger;

        return smaller;
    }
}