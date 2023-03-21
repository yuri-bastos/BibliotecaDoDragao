using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CEGI.Biblioteca {

    [CustomEditor(typeof(BookSpriteSelector))]
    public class BookDataInspector : Editor
    {
        SerializedProperty table;
        SerializedProperty shelf;

        private void OnEnable()
        {
            table = serializedObject.FindProperty("BookOnTable");
            shelf = serializedObject.FindProperty("BookOnShelf");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(table);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Table Sprites", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Shelf Sprites", EditorStyles.boldLabel);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            while (shelf.arraySize < table.arraySize)
                shelf.arraySize++;

            for (int i = 0; i < table.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(table.GetArrayElementAtIndex(i), new GUIContent());

                if(shelf.GetArrayElementAtIndex(i) != null)
                    EditorGUILayout.PropertyField(shelf.GetArrayElementAtIndex(i), new GUIContent());

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Add"))
            {
                table.arraySize++;
                while (shelf.arraySize < table.arraySize)
                    shelf.arraySize++;
            }

            if (GUILayout.Button("Remove"))
            {
                table.arraySize--;
                while (shelf.arraySize > table.arraySize)
                    shelf.arraySize--;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}