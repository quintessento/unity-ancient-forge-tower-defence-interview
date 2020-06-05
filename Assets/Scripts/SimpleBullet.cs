namespace AFSInterview
{
    using UnityEngine;

    public class SimpleBullet : Bullet
    {
        protected override void UpdateDirection()
        {
            direction = (targetObject.transform.position - transform.position).normalized;
        }

        protected override void UpdatePosition()
        {
            velocity = direction * speed;
            transform.position += velocity * Time.deltaTime;
        }
    }
}