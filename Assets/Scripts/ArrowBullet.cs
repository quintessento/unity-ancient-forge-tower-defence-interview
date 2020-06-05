namespace AFSInterview
{
    using UnityEngine;

    public class ArrowBullet : Bullet
    {
        [SerializeField] private float drag = 0.01f;

        protected override void UpdateDirection()
        {
            //don't update the direction, because we want this bullet to maintain its initial direction
        }

        protected override void UpdatePosition()
        {
            velocity += Physics.gravity * Time.deltaTime;
            velocity -= velocity * drag;
            transform.position += velocity * Time.deltaTime;
        }
    }
}