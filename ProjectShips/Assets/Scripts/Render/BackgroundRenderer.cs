using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRenderer : MonoBehaviour
{
    private bool isCreated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isCreated && other.tag == "Player")
        {
            SpriteRenderer[] sprites;
            sprites = GetComponentsInChildren<SpriteRenderer>();
            float OffsetX = 0;
            foreach (SpriteRenderer sprite in sprites)
            {
                OffsetX += 2*sprite.bounds.extents.x;
            }

            Instantiate(transform, new Vector3(transform.position.x + OffsetX, transform.position.y, transform.position.z), Quaternion.identity);
            isCreated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject, 2f);
        }
    }
}
