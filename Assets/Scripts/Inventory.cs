using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleInventoryActive : MonoBehaviour
{
    private GameObject[] inventoryObjects;
    private bool isHidden = false;

    void Start()
    {
        inventoryObjects = GameObject.FindGameObjectsWithTag("Inventory");

        foreach (GameObject obj in inventoryObjects)
        {
            obj.SetActive(isHidden);
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            isHidden = !isHidden;

            foreach (GameObject obj in inventoryObjects)
            {
                obj.SetActive(isHidden);
            }
        }
    }
}
