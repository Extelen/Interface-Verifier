using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(InterfaceVerifier<>))]
internal class InterfaceVerifierDrawer : PropertyDrawer
{
    // Fields
    private const float ErrorBoxHeight = 40f;
    private const float Spacing = 2f;

    // Methods
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Get the generic interface type
        Type interfaceType = GetInterfaceType(fieldInfo.FieldType);

        if (interfaceType == null)
        {
            EditorGUI.HelpBox(position, "Error: Could not determine interface type", MessageType.Error);
            EditorGUI.EndProperty();
            return;
        }

        // Get the serialized MonoBehaviour property
        SerializedProperty monoBehaviourProp = property.FindPropertyRelative("m_monoBehaviour");

        // Calculate rectangles
        Rect fieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect errorRect = new Rect(position.x, fieldRect.yMax + Spacing, position.width, ErrorBoxHeight);

        // Save current value
        MonoBehaviour currentValue = monoBehaviourProp.objectReferenceValue as MonoBehaviour;

        // Draw the object field (accepts both MonoBehaviour and GameObject)
        EditorGUI.BeginChangeCheck();
        UnityEngine.Object newObject = EditorGUI.ObjectField(fieldRect, label, currentValue, typeof(UnityEngine.Object), true);

        if (EditorGUI.EndChangeCheck())
        {
            if (newObject != null)
            {
                MonoBehaviour validComponent = FindValidComponent(newObject, interfaceType);

                if (validComponent != null)
                    monoBehaviourProp.objectReferenceValue = validComponent;

                // No valid component was found
                else
                {
                    if (newObject is GameObject)
                        Debug.LogWarning($"The GameObject does not have any component that implements interface '{interfaceType.Name}'");

                    else
                        Debug.LogWarning($"The MonoBehaviour does not implement interface '{interfaceType.Name}'");
                }
            }
            else
                monoBehaviourProp.objectReferenceValue = null;
        }

        // Show error if current value is not valid
        if (currentValue != null && !interfaceType.IsAssignableFrom(currentValue.GetType()))
        {
            string errorMessage = $"'{currentValue.GetType().Name}' does not implement interface '{interfaceType.Name}'";
            EditorGUI.HelpBox(errorRect, errorMessage, MessageType.Error);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Get the interface type
        Type interfaceType = GetInterfaceType(fieldInfo.FieldType);
        if (interfaceType == null)
            return EditorGUIUtility.singleLineHeight;

        // Get the current MonoBehaviour
        SerializedProperty monoBehaviourProp = property.FindPropertyRelative("m_monoBehaviour");
        MonoBehaviour currentValue = monoBehaviourProp.objectReferenceValue as MonoBehaviour;

        // If there's an invalid value, add space for the error message
        if (currentValue != null && !interfaceType.IsAssignableFrom(currentValue.GetType()))
            return EditorGUIUtility.singleLineHeight + Spacing + ErrorBoxHeight;

        return EditorGUIUtility.singleLineHeight;
    }

    /// <summary>
    /// Gets the interface type from the generic type InterfaceVerifier<T>
    /// </summary>
    private Type GetInterfaceType(Type fieldType)
    {
        if (fieldType.IsGenericType)
        {
            Type[] genericArgs = fieldType.GetGenericArguments();

            if (genericArgs.Length > 0)
                return genericArgs[0];
        }

        return null;
    }

    /// <summary>
    /// Finds a valid component that implements the specified interface
    /// </summary>
    /// <param name="obj">The dragged object (can be MonoBehaviour or GameObject)</param>
    /// <param name="interfaceType">The required interface type</param>
    /// <returns>The first MonoBehaviour that implements the interface, or null if not found</returns>
    private MonoBehaviour FindValidComponent(UnityEngine.Object obj, Type interfaceType)
    {
        // If it's a MonoBehaviour, check if implements the interface
        if (obj is MonoBehaviour monoBehaviour)
        {
            if (interfaceType.IsAssignableFrom(monoBehaviour.GetType()))
                return monoBehaviour;

            return null;
        }

        // If it's a GameObject, search through all its components
        if (obj is GameObject gameObject)
        {
            MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour component in components)
            {
                if (component != null && interfaceType.IsAssignableFrom(component.GetType()))
                    return component;
            }
        }

        return null;
    }
}
