using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectShips.Ships;

namespace ProjectShips
{
    public class ShipAI : MonoBehaviour
    {
        [SerializeField]
        private Ship PlayerShip;

        [SerializeField]
        private CannonController Cannon;

        [Header("AI Options")]
        [SerializeField]
        private float shootCooldown = 5f;

        [Tooltip("How much can randomized cooldown differ from start option")]
        [SerializeField]
        private float shootCooldownDelta = 1f;

        [SerializeField]
        private float aimPrecision = 1f;

        float randomCooldown;

        private void Start()
        {
            randomCooldown = GetRandomCooldown(shootCooldown, shootCooldownDelta);
        }

        float cooldownUpdateTemp = 0;
        private void Update()
        {
            cooldownUpdateTemp += Time.deltaTime;

            if (cooldownUpdateTemp > randomCooldown)
            {
                ShootPlayer();
                cooldownUpdateTemp = 0f;
                randomCooldown = GetRandomCooldown(shootCooldown, shootCooldownDelta);
            }
        }

        private float GetRandomCooldown(float cooldown, float delta)
        {
            return cooldown + Random.Range(-delta, delta);
        }

        public void ShootPlayer()
        {
            var dist = Random.Range(-10f / aimPrecision, 10f / aimPrecision);
            var point = PlayerShip.transform.position + PlayerShip.transform.forward * dist + Vector3.up * Mathf.Abs(dist * 0.2f);
            targetRotation = Quaternion.LookRotation(point - transform.position);

            StartCoroutine(ShootAtPlayer());
        }

        Quaternion targetRotation;
        private IEnumerator ShootAtPlayer()
        {
            float dotProduct = 0f;
            float transitionValue = 0f;
            while (dotProduct < 0.99f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, transitionValue);
                dotProduct = Quaternion.Dot(targetRotation, transform.rotation);
                yield return null;
                transitionValue += Time.deltaTime;
            }

            Cannon.Shoot();
        }
    }
}