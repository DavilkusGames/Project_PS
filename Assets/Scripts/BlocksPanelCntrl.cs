using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlocksPanelCntrl : Singleton<BlocksPanelCntrl>, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (!eventData.pointerDrag.CompareTag("CommandBlock")) return;
            Destroy(eventData.pointerDrag.gameObject);
        }
    }
}
