using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InterfaceGroupVerifier<>))]
internal class InterfaceGroupVerifierDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty verifiersProperty = property.FindPropertyRelative("m_verifiers");
        EditorGUI.PropertyField(position, verifiersProperty, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty verifiersProperty = property.FindPropertyRelative("m_verifiers");
        return EditorGUI.GetPropertyHeight(verifiersProperty, label, true);
    }
}