using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectShips.Ships
{
    [RequireComponent(typeof(Rigidbody))]
    public class CannonBall : MonoBehaviour
    {
        public float DeathDelay = 5f;
        public float ExplosionRadius = 2f;
        public float ExplosionPower = 1.2f;

        /// <summary>
        /// Event executed on ball collision with ship part.
        /// </summary>
        public UnityEvent<Vector3, CannonBall> HitShipPart;

        new Rigidbody rigidbody;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        float tDeath = 0;
        private void Update()
        {
            tDeath += Time.deltaTime;

            if (tDeath >= DeathDelay)
                Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Adding explosion force from cannon ball hit
            Vector3 explosionPos = collision.GetContact(0).point;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, ExplosionRadius);
            foreach (Collider hit in colliders)
            {
                if (!hit.TryGetComponent<ShipPart>(out var shipPart))
                    continue;

                var parts = shipPart.Shatter(ExplosionPower * rigidbody.velocity.magnitude, explosionPos);

                foreach (var part in parts)
                {
                    Rigidbody partRigidbody = part.GetComponent<Rigidbody>();

                    if (partRigidbody != null)
                        partRigidbody.AddExplosionForce(ExplosionPower * rigidbody.velocity.magnitude, explosionPos, ExplosionRadius, 2.0F);
                }
            }

            if(collision.gameObject.TryGetComponent<ShipPart>(out _))
                HitShipPart?.Invoke(rigidbody.velocity, this);
        }
    }
}