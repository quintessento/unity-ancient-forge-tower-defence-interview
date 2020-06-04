namespace AFSInterview
{
    using System.Collections;
    using UnityEngine;

    public class ArrowTower : Tower
    { 
        [SerializeField] private int numShots = 3;
        [SerializeField] private float shotInterval = 0.25f;

        protected override void Fire()
        {
            StopAllCoroutines();
            StartCoroutine(FireCoroutine());
        }

        private IEnumerator FireCoroutine()
        {
            for (int i = 0; i < numShots; i++)
            {
                var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.Initialize(targetEnemy.gameObject);

                yield return new WaitForSeconds(shotInterval);
            }
        }
    }
}
