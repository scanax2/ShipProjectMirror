using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectShips.Ships
{ 
    public class Ship : MonoBehaviour
    {
        public List<ShipPart> MainParts = new List<ShipPart>();
        [SerializeField] string subpartsSuffix = "_cell";

        private void Awake()
        {
            FindSubparts("_cell");
        }

        private void FindSubparts(string subpartSuffix)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                int suffixStartIndex = child.name.IndexOf(subpartSuffix);

                if (suffixStartIndex > 0)
                {
                    var mainPartName = child.name.Substring(0, suffixStartIndex);
                    var mainPart = MainParts.Find(p => p.name == mainPartName);

                    if (mainPart != null)
                    {
                        mainPart.Next.Add(child.GetComponent<ShipPart>());
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
