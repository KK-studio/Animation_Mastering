using UnityEngine;
using UnityEngine.AI;

namespace NetOpt.NetOptDemo
{
    public class DemoEntity : MonoBehaviour
    {
        private Bounds bounds = new Bounds(Vector3.zero, new Vector3(19, 1, 19));

        public float speed = 3f;
    
        public Vector3 serializedPosition;
        public Quaternion serializedRotation;
        public Vector3 serializedScale;
    
        public Vector3 logicalPosition;
        public Quaternion logicalRotation;
        public Vector3 logicalScale;

        private Vector3 currentTarget;

        // Start is called before the first frame update
        void Start()
        {
            logicalPosition = transform.position;
            logicalRotation = transform.rotation;
            logicalScale = transform.localScale;

            logicalPosition = RandomPosition();
            serializedPosition = logicalPosition;
        }

        Vector3 RandomPosition()
        {
            Vector3 position = new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                0.5f, Random.Range(bounds.min.z, bounds.max.z));
            return position;
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(logicalPosition, currentTarget) <= 1f)
            {
                currentTarget = RandomPosition();
            }

            Vector3 direction = (currentTarget - logicalPosition).normalized;

            logicalPosition += direction * (speed * Time.deltaTime);

            logicalRotation = Quaternion.LookRotation(direction);

            logicalScale = Vector3.one;

            transform.position = Vector3.Lerp(transform.position, serializedPosition, Time.deltaTime * 6f);
            transform.rotation = Quaternion.Lerp(transform.rotation, serializedRotation, Time.deltaTime * 6f);
        }
    }
}