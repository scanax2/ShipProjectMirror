using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectShips.Ships
{
    public class ShipPart : MonoBehaviour
    {
        public GameObject FloaterPrefab;
        private bool isFloater;

        public List<ShipPart> Next = new List<ShipPart>();
        public float MinMomentumToBreak = 3f;
        [SerializeField] float _mass = 10f;

        Rigidbody _rb;

        public float Mass
        {
            get => _mass;
            set
            {
                _mass = value;

                if (_rb != null)
                    _rb.mass = _mass;
            }
        }

        private void Awake()
        {
            if (!TryGetComponent(out _rb))
            {
                _rb = gameObject.AddComponent<Rigidbody>();
                _rb.drag = 0.05f;
            }

            _rb.isKinematic = true;
            _rb.mass = Mass;

            if (!TryGetComponent<Collider>(out _))
            {
                gameObject.AddComponent<MeshCollider>().convex = true;
            }
        }

        /// <summary>
        /// Shatters object into next parts if velocity was high enough.
        /// </summary>
        public List<ShipPart> Shatter(float momentum, Vector3 point)
        {
            if (momentum / (1 + (transform.position - point).sqrMagnitude) > MinMomentumToBreak)
            {
                // Turn on interaction with water
                SetWaterInteraction();

                if (Next == null || Next.Count == 0)
                {
                    _rb.isKinematic = false;
                    return new List<ShipPart>() { this };
                }
                else
                {
                    foreach (var part in Next)
                        part.gameObject.SetActive(true);

                    gameObject.SetActive(false);
                    return new List<ShipPart>(Next);
                }
            }

            return new List<ShipPart>();
        }

        void SetWaterInteraction()
        {
            if (FloaterPrefab && !isFloater)
            {
                transform.parent = null;
                GameObject floater = Instantiate(FloaterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                floater.transform.parent = transform;
                floater.transform.localPosition = new Vector3(0, 0, 0);
                floater.GetComponent<Floater>().rigidBody = GetComponent<Rigidbody>();
                isFloater = true;
                GetComponent<Rigidbody>().isKinematic = false;
                if (gameObject.active)
                {
                    StartCoroutine(TurnIsKinematic(5));
                }

            }
        }

        /// <summary>
        /// Shatters object into next parts if velocity was high enough.
        /// </summary>
        public List<ShipPart> Shatter(float momentum)
        {
            return Shatter(momentum, transform.position);
        }

        private IEnumerator TurnIsKinematic(float time)
        {
            yield return new WaitForSeconds(time);
            GetComponent<Rigidbody>().isKinematic = false;
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody == null || collision.transform.tag != "Projectile")
                return;
               

            var collRb = collision.rigidbody;
            Shatter(CalculateMomentum(collRb.velocity, collRb.mass, collision.contacts[0].normal));
        }

        /// <summary>
        /// Calculates momentum relative to plane with specified normal.
        /// </summary>
        public static float CalculateMomentum(Vector3 velocity, float mass, Vector3 normal)
        {
            var dotProduct = Mathf.Abs(Vector3.Dot(normal, velocity.normalized));
            return (velocity * dotProduct).magnitude * mass;
        }
    }
}