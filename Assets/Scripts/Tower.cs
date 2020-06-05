namespace AFSInterview
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Tower : MonoBehaviour
    {
        [SerializeField] protected Bullet bulletPrefab;
        [SerializeField] protected Transform bulletSpawnPoint;
        [SerializeField] private float firingRate;
        [SerializeField] private float firingRange;

        protected Enemy targetEnemy;

        private float fireTimer;

        private IReadOnlyList<Enemy> enemies;

        public void Initialize(IReadOnlyList<Enemy> enemies)
        {
            this.enemies = enemies;
            fireTimer = firingRate;
        }

        protected abstract void Fire();

        protected virtual void FaceTarget()
        {
            if (targetEnemy != null)
            {
                TurnToTarget(targetEnemy.transform.position);
            }
        }

        protected void TurnToTarget(Vector3 targetPosition)
        {
            var lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }

        private void Update()
        {
            targetEnemy = FindClosestEnemy();

            FaceTarget();

            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                if (targetEnemy != null)
                {
                    Fire();
                }

                fireTimer = firingRate;
            }
        }

        private Enemy FindClosestEnemy()
        {
            Enemy closestEnemy = null;
            var closestDistance = float.MaxValue;

            foreach (var enemy in enemies)
            {
                var distance = (enemy.transform.position - transform.position).magnitude;
                if (distance <= firingRange && distance <= closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distance;
                }
            }

            return closestEnemy;
        }
    }
}
