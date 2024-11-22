using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LoopingScrollRectWithCenterSelection : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public float itemHeight;
    public int itemCount;
    public int centerIndex;

    private float scrollPosition;
    private int currentIndex;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        content = scrollRect.content;
        itemHeight = content.GetChild(0).GetComponent<RectTransform>().rect.height;
        itemCount = content.childCount;
        centerIndex = itemCount / 2;
        RepositionItems();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        scrollPosition = content.anchoredPosition.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float delta = eventData.delta.y;
        scrollPosition += delta;

        if (scrollPosition > itemHeight)
        {
            scrollPosition -= itemHeight;
            currentIndex = (currentIndex - 1 + itemCount) % itemCount;
            RepositionItems();
        }
        else if (scrollPosition < -itemHeight)
        {
            scrollPosition += itemHeight;
            currentIndex = (currentIndex + 1) % itemCount;
            RepositionItems();
        }

        content.anchoredPosition = new Vector2(0, scrollPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Snap to the nearest item
        int nearestIndex = Mathf.RoundToInt(scrollPosition / itemHeight);
        currentIndex = (currentIndex + nearestIndex) % itemCount;
        scrollPosition = nearestIndex * itemHeight;
        content.anchoredPosition = new Vector2(0, scrollPosition);
        RepositionItems();
    }

    private void RepositionItems()
    {
        for (int i = 0; i < itemCount; i++)
        {
            RectTransform item = content.GetChild(i).GetComponent<RectTransform>();
            item.anchoredPosition = new Vector2(0, -itemHeight * ((i + currentIndex) % itemCount));
        }

        // Highlight the center item
        HighlightCenterItem();
    }

    private void HighlightCenterItem()
    {
        for (int i = 0; i < itemCount; i++)
        {
            RectTransform item = content.GetChild(i).GetComponent<RectTransform>();
            if (i == centerIndex)
            {
                // Highlight the center item
                item.GetComponent<TextMeshProUGUI>().color = Color.red;
            }
            else
            {
                // Reset the color of other items
                item.GetComponent<TextMeshProUGUI>().color = Color.green;
            }
        }
    }
}
