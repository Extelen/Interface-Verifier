# Interface Verifier for Unity

Generic system to validate that MonoBehaviours assigned in the Inspector implement specific interfaces.

## Features

- Works with any interface
- Automatic validation in editor and runtime
- Drag & drop GameObjects - automatically finds the right component

## Usage

### Define an interface

```csharp
public interface IInteractable
{
    void Interact();
}
```

### Implement it in a MonoBehaviour

```csharp
public class Door : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Door opened!");
    }
}
```

### Use InterfaceVerifier

```csharp
public class Player : MonoBehaviour
{
    public InterfaceVerifier<IInteractable> target;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Option 1: Check validity first
            if (target.IsValid())
            {
                target.Value.Interact();
            }

            // Option 2: Try pattern (recommended)
            if (target.TryGetInterface(out IInteractable interactable))
            {
                interactable.Interact();
            }

            // Option 3: Direct access with null check
            target.Value?.Interact();
        }
    }
}
```

## API

**Properties:**
- `MonoBehaviour MonoBehaviour` - Get/set the stored MonoBehaviour
- `T Value` - Get the value as type T (null if invalid)

**Methods:**
- `bool IsValid()` - Check if MonoBehaviour implements interface T
- `bool TryGetInterface(out T result)` - Try to get the interface

## Editor Behavior

You can drag:
- **MonoBehaviour** - Assigns if it implements the interface
- **GameObject** - Searches all components and assigns the first valid one

Invalid assignments show an error box in the Inspector.
