using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectShips.Ships
{
    public class ShipPart : MonoBehaviour
    {
        public List<ShipPart> Next = new List<ShipPart>();
        public float MinVelocityToBreak = 3f;
        [SerializeField] float _mass = 10f;
        public float Mass
        {
            get => _mass;
            set
            {
                _mass = value;

                if (rb != null)
                    rb.mass = _mass;
            }
        }

        Rigidbody rb;

        private void Awake()
        {
            if (!TryGetComponent(out rb))
            {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.drag = 0.05f;
            }

            rb.isKinematic = true;
            rb.mass = Mass;

            if (!TryGetComponent<Collider>(out _))
            {
                gameObject.AddComponent<MeshCollider>().convex = true;
            }
        }

        /// <summary>
        /// Shatters object into next parts if velocity was high enough.
        /// </summary>
        public List<ShipPart> Shatter(float velocity, Vector3 point)
        {
            if (velocity / (1 + (transform.position - point).sqrMagnitude) > MinVelocityToBreak)
            {
                if (Next == null || Next.Count == 0)
                {
                    rb.isKinematic = false;
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

        /// <summary>
        /// Shatters object into next parts if velocity was high enough.
        /// </summary>
        public List<ShipPart> Shatter(float velocity)
        {
            return Shatter(velocity, transform.position);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody == null)
                return;

            Shatter(collision.rigidbody.velocity.magnitude);
        }
    }
}