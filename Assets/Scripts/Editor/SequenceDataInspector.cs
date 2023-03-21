using UnityEditor;
using UnityEngine;

namespace CEGI.Biblioteca
{
    [CustomEditor(typeof(SequenceData))]
    public class SequenceDataInspector : Editor
    {
        SerializedProperty matrix;
        SerializedProperty missingBooks;

        GUIStyle intField;

        private void OnEnable()
        {
            matrix = serializedObject.FindProperty("SequenceMatrix");
            missingBooks = serializedObject.FindProperty("MissingBooks");

            while (matrix.arraySize < 4)
                matrix.arraySize++;

            intField = new GUIStyle(EditorStyles.numberField);
            intField.fixedWidth = 30;
            intField.fixedHeight = 20;

            intField.stretchHeight = true;
            intField.stretchWidth = true;

            intField.margin = new RectOffset(0, 0, 0, 0);
            intField.border = new RectOffset(0, 0, 0, 0);

            intField.alignment = TextAnchor.UpperCenter;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.FlexibleSpace();

            GUILayout.Label("SEQUÊNCIA: " + serializedObject.targetObject.name, EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // serializedObject.Update();

            for (int i=0; i < matrix.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                for(int j=0; j < 10; j++)
                {
                    SerializedProperty field = matrix.GetArrayElementAtIndex(i).FindPropertyRelative("sequence");

                    while (field.arraySize < j+1)
                    {
                        field.InsertArrayElementAtIndex(j);
                        field.GetArrayElementAtIndex(j).intValue = 0;
                    }

                    field.GetArrayElementAtIndex(j).intValue = EditorGUILayout.IntField(field.GetArrayElementAtIndex(j).intValue, intField);
                }

                EditorGUILayout.EndHorizontal(); 
                EditorGUILayout.Space();
            }

            if (matrix.arraySize < 5)
            {
                if (GUILayout.Button("Add Prateleira"))
                {
                    matrix.arraySize++;
                    missingBooks.arraySize++;
                }
            }
            else
            {
                if (GUILayout.Button("Remover Prateleira"))
                {
                    matrix.DeleteArrayElementAtIndex(4);
                    if (matrix.GetArrayElementAtIndex(4).objectReferenceValue != null)
                        matrix.DeleteArrayElementAtIndex(4);

                    missingBooks.DeleteArrayElementAtIndex(4);
                    if (missingBooks.GetArrayElementAtIndex(4) != null)
                        missingBooks.DeleteArrayElementAtIndex(4);
                }
            }

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.FlexibleSpace();

            GUILayout.Label("Livros faltando por prateleira", EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            for (int i=0; i < missingBooks.arraySize; i++)
                EditorGUILayout.PropertyField(missingBooks.GetArrayElementAtIndex(i), new GUIContent("Prateleira " + (i+1).ToString("00")));
            

            serializedObject.ApplyModifiedProperties();
        }
    }
}