namespace AFSInterview
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class GameplayManager : MonoBehaviour
    {
        [Header("Prefabs")] 
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject[] towerPrefabs;

        [Header("Settings")] 
        [SerializeField] private Vector2 boundsMin;
        [SerializeField] private Vector2 boundsMax;
        [SerializeField] private float enemySpawnRate;

        [Header("UI")] 
        [SerializeField] private TextMeshProUGUI enemiesCountText;
        [SerializeField] private TextMeshProUGUI scoreText;
        
        private List<Enemy> enemies;
        private float enemySpawnTimer;
        private int score;

        private void Awake()
        {
            enemies = new List<Enemy>();
        }

        private void Start()
        {
            UpdateScoreCounters();
        }

        private void Update()
        {
            enemySpawnTimer -= Time.deltaTime;

            if (enemySpawnTimer <= 0f)
            {
                SpawnEnemy();
                enemySpawnTimer = enemySpawnRate;
            }

            if (Input.GetMouseButtonDown(0))
            {
                TrySpawnTowerOnGround(0);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                TrySpawnTowerOnGround(1);
            }
        }

        private void SpawnEnemy()
        {
            var position = new Vector3(Random.Range(boundsMin.x, boundsMax.x), enemyPrefab.transform.position.y, Random.Range(boundsMin.y, boundsMax.y));
            
            var enemy = Instantiate(enemyPrefab, position, Quaternion.identity).GetComponent<Enemy>();
            enemy.OnEnemyDied += Enemy_OnEnemyDied;
            enemy.Initialize(boundsMin, boundsMax);

            enemies.Add(enemy);

            UpdateScoreCounters();
        }

        private void Enemy_OnEnemyDied(Enemy enemy)
        {
            enemies.Remove(enemy);
            score++;

            UpdateScoreCounters();
        }

        private void UpdateScoreCounters()
        {
            scoreText.text = "Score: " + score;
            enemiesCountText.text = "Enemies: " + enemies.Count;
        }

        private bool TrySpawnTowerOnGround(int towerPrefabIndex)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, LayerMask.GetMask("Ground")))
            {
                var spawnPosition = hit.point;
                spawnPosition.y = towerPrefabs[towerPrefabIndex].transform.position.y;

                SpawnTower(towerPrefabIndex, spawnPosition);
                return true;
            }

            return false;
        }

        private void SpawnTower(int towerPrefabIndex, Vector3 position)
        {
            var tower = Instantiate(towerPrefabs[towerPrefabIndex], position, Quaternion.identity).GetComponent<Tower>();
            tower.Initialize(enemies);
        }
    }
}