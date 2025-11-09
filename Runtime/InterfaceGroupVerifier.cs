using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InterfaceGroupVerifier<T> where T : class
{
    // Fields
    [SerializeField]
    private InterfaceVerifier<T>[] m_verifiers;
    public InterfaceVerifier<T>[] Verifiers
    {
        get
        {
            return m_verifiers;
        }
    }

    private List<T> m_cachedInterfaces;
    public IReadOnlyList<T> CachedInterfaces
    {
        get
        {
            if (m_cachedInterfaces == null)
                ResetCachedInterfaces();

            return m_cachedInterfaces;
        }
    }

    // Methods
    public void Verify()
    {
        foreach (var verifier in m_verifiers)
        {
            if (verifier.IsValid())
                continue;

            else
                Debug.LogError("Object doesn't implement the required interface");
        }
    }

    public void ResetCachedInterfaces()
    {
        if (m_cachedInterfaces == null)
            m_cachedInterfaces = new List<T>();

        else
            m_cachedInterfaces.Clear();

        foreach (var verifier in m_verifiers)
        {
            if (verifier.TryGetInterface(out var result))
                m_cachedInterfaces.Add(result);

            else
                Debug.LogError("Object doesn't implement the required interface");
        }
    }

    public void ForEach(System.Action<T> action)
    {
        foreach (var item in CachedInterfaces)
        {
            action?.Invoke(item);
        }
    }
}
