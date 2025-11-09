using UnityEngine;

[System.Serializable]
public class InterfaceVerifier<T> where T : class
{
    // Fields
    [SerializeField]
    private MonoBehaviour m_monoBehaviour;
    public MonoBehaviour MonoBehaviour
    {
        get => m_monoBehaviour;
        set => m_monoBehaviour = value;
    }

    /// <summary>
    /// Gets the value as type T if valid, or null if not
    /// </summary>
    public T Value
    {
        get
        {
            if (IsValid())
                return m_monoBehaviour as T;

            else
                return null;
        }
    }

    /// <summary>
    /// Checks if the stored MonoBehaviour implements interface T
    /// </summary>
    /// <returns>True if the MonoBehaviour is valid and implements T, false otherwise</returns>
    public bool IsValid()
    {
        if (m_monoBehaviour == null)
            return false;

        return m_monoBehaviour is T;
    }

    /// <summary>
    /// Tries to get the interface of type T
    /// </summary>
    /// <param name="result">The interface if valid, null otherwise</param>
    /// <returns>True if the interface is valid, false otherwise</returns>
    public bool TryGetInterface(out T result)
    {
        if (IsValid())
        {
            result = m_monoBehaviour as T;
            return true;
        }

        result = null;
        return false;
    }

    // Constructors
    /// <summary>
    /// Default constructor
    /// </summary>
    public InterfaceVerifier()
    {
        m_monoBehaviour = null;
    }

    /// <summary>
    /// Constructor with initial MonoBehaviour
    /// </summary>
    /// <param name="monoBehaviour">The MonoBehaviour to assign</param>
    public InterfaceVerifier(MonoBehaviour monoBehaviour)
    {
        m_monoBehaviour = monoBehaviour;
    }
}
