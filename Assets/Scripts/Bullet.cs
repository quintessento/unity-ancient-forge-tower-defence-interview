namespace AFSInterview
{
    using UnityEngine;

    public abstract class Bullet : MonoBehaviour
    {
        [SerializeField] protected float speed;
        [SerializeField] private float autoDestroyDelay = 2f;

        protected Vector3 direction;
        protected Vector3 velocity;

        protected GameObject targetObject;

        public float Speed => speed;

        public void Initialize(GameObject target, Vector3 initialDirection)
        {
            targetObject = target;
            direction = initialDirection;
            velocity = direction * speed;
        }

        protected abstract void UpdateDirection();
        protected abstract void UpdatePosition();

        private void Update()
        {
            //check if the bullet has moved under ground, destroy it if that's the case
            if(transform.position.y < -0.1f)
            {
                Destroy(gameObject);
            }

            if (targetObject != null)
            {
                UpdateDirection();

                if ((transform.position - targetObject.transform.position).magnitude <= 0.2f)
                {
                    Destroy(gameObject);
                    Destroy(targetObject);
                }
            }
            else
            {
                //enemy has already been destroyed by another tower, initiate self-destruction based on timer
                if(autoDestroyDelay > 0f)
                {
                    autoDestroyDelay -= Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            UpdatePosition();
        }
    }
}