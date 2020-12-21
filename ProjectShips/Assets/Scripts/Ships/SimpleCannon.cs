using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectShips.Ships
{
    public class SimpleCannon : MonoBehaviour
    {
        [SerializeField] float strenth = 100;
        [SerializeField] GameObject ballPrefab;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                Shoot();
        }

        private void Shoot()
        {
            var go = Instantiate(ballPrefab, transform.position, Quaternion.identity);
            var rigidbody = go.GetComponent<Rigidbody>();

            rigidbody.AddForce(transform.forward * strenth, ForceMode.Impulse);
        }
    }
}