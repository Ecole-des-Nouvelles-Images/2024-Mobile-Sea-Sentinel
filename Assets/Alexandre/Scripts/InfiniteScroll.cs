using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    public ScrollRect ScrollRect;
    public RectTransform ViewPortTransform;
    public RectTransform ContentTransform;
    public VerticalLayoutGroup VLG;

    public RectTransform[] ItemList;

    private void Start()
    {
        int ItemsToAdd = Mathf.CeilToInt(ViewPortTransform.rect.height / (ItemList[0].rect.height + VLG.spacing));
        
        for (int i = 0; i < ItemsToAdd; i++)
        {
            RectTransform rT = Instantiate(ItemList[i % ItemList.Length], ContentTransform);
            rT.SetAsLastSibling();
        }
        for (int i = 0; i < ItemsToAdd; i++)
        {
            int num = ItemList.Length - i -1;
            while (num < 0)
            {
                num += ItemList.Length;
            }
            RectTransform rT = Instantiate(ItemList[num], ContentTransform);
            rT.SetAsFirstSibling();
        }
        ContentTransform.localPosition = new Vector3((0 - (ItemList[0].rect.width+VLG.spacing)*ItemsToAdd), ContentTransform.localPosition.y, ContentTransform.localPosition.z);
    }

    private void Update()
    {
        if (ContentTransform.localPosition.y > 0)
        {
            Canvas.ForceUpdateCanvases();
            ContentTransform.localPosition -=
                new Vector3( 0,ItemList.Length * (ItemList[0].rect.height + VLG.spacing), 0);
        }

        if (ContentTransform.localPosition.y < 0 - (ItemList.Length * (ItemList[0].rect.height + VLG.spacing)))
        {
            Canvas.ForceUpdateCanvases();
            ContentTransform.localPosition +=
                new Vector3(0,ItemList.Length * (ItemList[0].rect.width + VLG.spacing), 0);
        }
}
}
