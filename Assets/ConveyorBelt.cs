using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [System.Serializable]
    public class ConveyorBeltItem
    {
        public Transform item;
        [HideInInspector] public float currentLerp;
        [HideInInspector] public int endPoint = 1;
    }

    [SerializeField] private float itemSpacing;
    [SerializeField] private float speed;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private List<ConveyorBeltItem> _items;

    private void Update()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            ConveyorBeltItem beltItem = _items[i];
            Transform item = beltItem.item;

            if (i > 0)
            {
                if (Vector3.Distance(a: item.position, b: _items[i-1].item.position) <= itemSpacing)
                {
                    continue;
                }

            }

            item.transform.position = Vector3.Lerp(a: _lineRenderer.GetPosition(beltItem.endPoint - 1), b: _lineRenderer.GetPosition(beltItem.endPoint), beltItem.currentLerp);
            float distance = Vector3.Distance(a: _lineRenderer.GetPosition(beltItem.endPoint - 1), b: _lineRenderer.GetPosition(beltItem.endPoint));
            beltItem.currentLerp += speed * Time.deltaTime / distance;

            if (beltItem.currentLerp >= 1)
            {
                if (beltItem.endPoint + 1 < _lineRenderer.positionCount) //got more 
                {
                    beltItem.currentLerp = 0;
                    beltItem.endPoint += 1;
                }
                else
                {
                    beltItem.endPoint = 0;
                }
            }
        }

        
    }
}
