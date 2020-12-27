using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRenderer : MonoBehaviour
{
    private bool isCreated = false;

    //To create new water instance
    private void OnTriggerEnter(Collider other)
    {
        if (!isCreated && other.tag == "Player")
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Instantiate(transform, new Vector3(transform.position.x + meshFilter.mesh.bounds.size.x - 15,  transform.position.y, transform.position.z), Quaternion.identity);
            isCreated = true;
        }
    }

    //To delete old water instance
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
           Destroy(gameObject, 2f);
        }
    }
}
