using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProjectShips.Ships
{
    [CustomEditor(typeof(Ship))]
    public class ShipEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var ship = (Ship)target;

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_subpartsSuffix"));

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_minMomentumToBreak"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_mass"));
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Hide shattered"))
                    ship.FindParts();
                if (GUILayout.Button("Show shattered"))
                    ship.FindParts(false);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}