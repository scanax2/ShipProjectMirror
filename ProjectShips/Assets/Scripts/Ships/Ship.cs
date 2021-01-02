using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectShips.Ships
{
    public class Ship : MonoBehaviour
    {
        public HashSet<ShipPart> MainParts = new HashSet<ShipPart>();
        [SerializeField] string subpartsSuffix = "_cell";

        [Header("Parts settings")]
        [SerializeField] float _minVelocityToBreak = 3f;
        [SerializeField] float _mass = 10f;

        private void Awake()
        {
            FindParts(subpartsSuffix);
        }

        /// <summary>
        /// Find parts and builds simple 2 level deep linked list out of them.
        /// </summary>
        private void FindParts(string subpartSuffix)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                int suffixStartIndex = child.name.IndexOf(subpartSuffix);

                // Don't change the order of the 2 lines below. ShipPart's Awake() method adds rigidbody if there was none,
                // so we want to know before that if that was the case.
                bool canOverrideMass = !child.TryGetComponent<Rigidbody>(out _);
                bool hadShipPartComponent = child.TryGetComponent<ShipPart>(out var shipPart);

                if (!hadShipPartComponent)
                {
                    shipPart = child.gameObject.AddComponent<ShipPart>();
                    shipPart.MinVelocityToBreak = _minVelocityToBreak;
                }

                // If someone already set mass for a part then we don't want to override it
                if (canOverrideMass)
                    shipPart.Mass = _mass;

                // Executed if part has a parent
                if (suffixStartIndex > 0)
                {
                    // Finding object's name without suffix | "side2_cell001" -> "side2"
                    var mainPartName = child.name.Substring(0, suffixStartIndex);
                    var mainPartGO = transform.Find(mainPartName).gameObject;

                    // Making sure that the parent will have ShipPart component
                    if (!mainPartGO.TryGetComponent<ShipPart>(out var mainPart))
                        mainPart = mainPartGO.GetComponent<ShipPart>();

                    if (!MainParts.Contains(mainPart))
                        MainParts.Add(mainPart);

                    mainPart.Next.Add(child.gameObject.GetComponent<ShipPart>());
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
