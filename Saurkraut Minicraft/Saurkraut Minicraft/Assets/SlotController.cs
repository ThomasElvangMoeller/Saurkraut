using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotController : MonoBehaviour, IDropHandler
{
    public GameObject item {
        get {
            if(transform.childCount > 0) {
                return transform.GetChild(0).gameObject;
            } else
            {
                return null;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(!item)
        {
            SlotedItemController.itemBeingDragged.transform.SetParent(transform);
        }
    }
}
