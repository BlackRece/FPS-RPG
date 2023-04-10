using System.Collections;
using System.Collections.Generic;
using BlackRece.ProjectilePooler;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _enemySpawnPoints;
    [SerializeField] private float _spawnDelay = 1.0f;
    
    private float _spawnTimer = 0f;
    private int _spawnLimit = 100;
    private ProjectilePooler _pooler;
    private List<Enemy> _enemies;

    private void Awake()
    {
        _pooler = GetComponent<ProjectilePooler>();
        _enemies = new List<Enemy>();
    }

    private void Start()
    {
        _pooler.Init();
    }

    private void Update()
    {
        var paused = !FirstPersonController.IsPlayerInArena();
        if (!paused)
        {
            if(Tick() && _enemies.Count < _spawnLimit)
                SpawnEnemy();
            
        }

        for (var i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].HandlePause(paused);
            
            if(!_enemies[i].gameObject.activeSelf)
                _enemies.RemoveAt(i);
        }
    }

    private bool Tick()
    {
        if (_spawnTimer > 0f)
            _spawnTimer -= Time.deltaTime;
        else
        {
            _spawnTimer = _spawnDelay;
            return true;
        }

        return false;
    }
    
    private void SpawnEnemy()
    {
        // get player position
        var pos = FirstPersonController.GetPlayerPosition();
        
        // get closest spawn point
        Transform tSpawnPoint = null;
        var fDistance = 0f;
        foreach (var spawnPoint in _enemySpawnPoints)
        {
            var distance = Vector3.Distance(pos, spawnPoint.position);
            if (tSpawnPoint == null || distance < fDistance)
            {
                tSpawnPoint = spawnPoint;
                fDistance = distance;
            }
        }
        
        if(tSpawnPoint == null)
            return;
        
        // spawn enemy
        var enemy = _pooler
            .GetGameObject()
            .GetComponent<Enemy>();
        enemy.Init(tSpawnPoint.position, tSpawnPoint.rotation.eulerAngles);
        _enemies.Add(enemy);
    }
}
