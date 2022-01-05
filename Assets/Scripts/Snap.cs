using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Snap : MonoBehaviour
{

    public List<Transform> snapPoints;
    public List<Draggable> draggables;
    public float snapRange = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Draggable draggable in draggables)
        {
            draggable.dragEndedCallback = OnDragEnded;
        }
    }


    // Update is called once per frame
    private void OnDragEnded(Draggable draggable)
    {
        float closestDistance = -1;
        Transform closestSnapPoint = null;

        foreach(Transform snapPoint in snapPoints)
        {
            if (snapPoint != draggable.transform.Find("btm") && snapPoint != draggable.transform.Find("top"))
            {
                if (draggable.transform && snapPoint)
                {
                    float currentDistance = Vector2.Distance(draggable.transform.position, snapPoint.position);
                    if (closestSnapPoint == null || currentDistance < closestDistance)
                    {

                        closestSnapPoint = snapPoint;
                        closestDistance = currentDistance;

                    }
                }
                
            }
        }

        if(closestSnapPoint != null && closestDistance <= snapRange && closestSnapPoint != draggable.transform.Find("btm"))
        {
            if(closestSnapPoint.name == "btm")
            {
                draggable.transform.position = closestSnapPoint.position - new Vector3(0, draggable.transform.localScale.y * 1.5f, 0);
                if (closestSnapPoint.parent.GetComponent<BlockManager>().childBlock != null)
                {
                    draggable.CancelDrag();
                } else
                {
                    draggable.transform.GetComponent<BlockManager>().parentBlock = closestSnapPoint.parent;
                    closestSnapPoint.parent.GetComponent<BlockManager>().childBlock = draggable.transform;
                    print(closestSnapPoint + " <- " + closestSnapPoint.parent);
                }

            }
            if (closestSnapPoint.name == "top")
            {
                draggable.transform.position = closestSnapPoint.position;
                if (closestSnapPoint.parent.GetComponent<BlockManager>().parentBlock != null)
                {
                    draggable.CancelDrag();
                }
                else
                {
                    draggable.transform.GetComponent<BlockManager>().childBlock = closestSnapPoint.parent;
                    closestSnapPoint.parent.GetComponent<BlockManager>().parentBlock = draggable.transform;
                    print(closestSnapPoint + " <- " + closestSnapPoint.parent);
                }

            }
        }
    }
}
