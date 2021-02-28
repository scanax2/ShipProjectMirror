using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectShips.Ships
{
    [RequireComponent(typeof(Rigidbody))]
    public class CannonBall : MonoBehaviour
    {
        public float DeathDelay = 10f;
        public float ExplosionRadius = 1.5f;
        public float ExplosionPower = 1f;


        /// <summary>
        /// Event executed on ball collision with ship part.
        /// </summary>
        public UnityEvent<float, CannonBall> HitShipPart;

        Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
          
        }

        private void OnDestroy()
        {
            HitShipPart.RemoveAllListeners();
        }

        float _tDeath = 0;
        private void Update()
        {
            _tDeath += Time.deltaTime;

            if (_tDeath >= DeathDelay)
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

                var parts = shipPart.Shatter(ExplosionPower * _rigidbody.velocity.magnitude * _rigidbody.mass, explosionPos);

                foreach (var part in parts)
                {
                    Rigidbody partRigidbody = part.GetComponent<Rigidbody>();

                    if (partRigidbody != null)
                        partRigidbody.AddExplosionForce(ExplosionPower * _rigidbody.velocity.magnitude * _rigidbody.mass, explosionPos, ExplosionRadius, 0.0F);
                }
            }

            if(collision.gameObject.TryGetComponent<ShipPart>(out _))
                HitShipPart?.Invoke(
                    ShipPart.CalculateMomentum(_rigidbody.velocity, _rigidbody.mass, collision.contacts[0].normal),
                    this);
        }
    }
}