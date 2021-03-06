﻿namespace AFSInterview
{
    using System.Collections;
    using UnityEngine;

    public class ArrowTower : Tower
    { 
        [SerializeField] private int numShots = 3;
        [SerializeField] private float shotInterval = 0.25f;

        private bool isTargetLocked;
        private Vector3 predictedEnemyPosition;

        protected override void Fire()
        {
            if (isTargetLocked)
            {
                StopAllCoroutines();
                StartCoroutine(FireCoroutine());
            }
        }

        protected override void FaceTarget()
        {
            if (targetEnemy != null)
            {
                isTargetLocked = GetInterceptPosition(out predictedEnemyPosition);
                if (isTargetLocked)
                {
                    TurnToTarget(predictedEnemyPosition);
                }
            }
            else
            {
                base.FaceTarget();
            }
        }

        private IEnumerator FireCoroutine()
        {
            for (int i = 0; i < numShots; i++)
            {
                if(targetEnemy == null)
                {
                    yield break;
                }

                var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity).GetComponent<Bullet>();

                float distanceToTarget = Vector3.Distance(targetEnemy.transform.position, bulletSpawnPoint.position);
                if (GetTrajectoryAngle(distanceToTarget, out float trajectoryAngle))
                {
                    float trajectoryHeight = Mathf.Tan(trajectoryAngle) * distanceToTarget;
                    //adjust the trajectory arc by modifying the target's y position
                    predictedEnemyPosition.y += trajectoryHeight;
                }

                bullet.Initialize(
                    target: targetEnemy.gameObject,
                    initialDirection: (predictedEnemyPosition - bulletSpawnPoint.position).normalized
                );

                yield return new WaitForSeconds(shotInterval);
            }
        }

        private bool GetInterceptPosition(out Vector3 predictedPosition)
        {
            Vector3 targetDeltaPos = targetEnemy.transform.position - bulletSpawnPoint.position;
            float t = GetInterceptTime(bulletPrefab.Speed, targetDeltaPos, targetEnemy.Velocity);

            //use displacement formula and intercept time to predict the position
            predictedPosition = targetEnemy.transform.position + t * (targetEnemy.Velocity);

            return t != 0f;
        }

        private float GetInterceptTime(float projectileSpeed, Vector3 targetDeltaPosition, Vector3 targetVelocity)
        {
            float velocitySquared = targetVelocity.sqrMagnitude;

            //find a, b, c of the quadratic at^2 + bt + c = 0,
            //where t is the intercept time
            float a = velocitySquared - projectileSpeed * projectileSpeed;
            if(a == 0f)
            {
                //make sure there is no division by zero later on
                return 0f;
            }

            float b = 2f * Vector3.Dot(targetVelocity, targetDeltaPosition);
            float c = targetDeltaPosition.sqrMagnitude;
            //determinant
            float D = b * b - 4f * a * c;

            if (D >= 0f)
            {
                //two or one solutions
                float rootD = Mathf.Sqrt(D);
                float t1 = (-b + rootD) / (2f * a);
                float t2 = (-b - rootD) / (2f * a);
                return Mathf.Max(t1, t2);
            }

            return 0f;
        }

        private bool GetTrajectoryAngle(float distanceToTarget, out float angle)
        {
            float projectileSpeed = bulletPrefab.Speed;
            if(projectileSpeed <= 0f)
            {
                angle = 0f;
                return false;
            }

            angle = Mathf.Asin((-Physics.gravity.y * distanceToTarget) / (projectileSpeed * projectileSpeed)) * 0.5f;
            return true;
        }

    }
}
