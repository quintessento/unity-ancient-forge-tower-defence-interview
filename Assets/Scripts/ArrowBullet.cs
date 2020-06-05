namespace AFSInterview
{
    using UnityEngine;

    public class ArrowBullet : Bullet
    {
        protected override void UpdateDirection()
        {
            //don't update the direction, because we want this bullet to maintain its initial direction
        }

        protected override void UpdatePosition()
        {
            velocity += Physics.gravity * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
        }
    }
}