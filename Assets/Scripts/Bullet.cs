namespace AFSInterview
{
    using UnityEngine;

    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float autoDestroyDelay = 2f;

        private GameObject targetObject;

        private Vector3 direction;

        public void Initialize(GameObject target)
        {
            targetObject = target;
        }

        private void Update()
        {
            if (targetObject != null)
            {
                direction = (targetObject.transform.position - transform.position).normalized;

                if ((transform.position - targetObject.transform.position).magnitude <= 0.2f)
                {
                    Destroy(gameObject);
                    Destroy(targetObject);
                }
            }
            else
            {
                //enemy has already been destroyed by another tower, initiate self-destruction based on timer or checking when we go unseen under the ground
                if(autoDestroyDelay > 0f && transform.position.y >= -0.1f)
                {
                    autoDestroyDelay -= Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            transform.position += direction * speed * Time.deltaTime;
        }
    }
}