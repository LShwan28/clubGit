using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;

    [HideInInspector] public Transform ParentAfterDrag;

    public Canvas canvas;

    void Awake()
    {
        // Canvas는 최상위 부모에서 찾아옵니다 (단, 한 씬에 하나만 있다고 가정)
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentAfterDrag = transform.parent;
        image.raycastTarget = false;

        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling(); // 항상 UI 최상단으로 보이게
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(ParentAfterDrag);
    }
}
