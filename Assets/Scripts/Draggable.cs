using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Draggable : MonoBehaviour
{

    public delegate void DragEndedDelegate(Draggable draggables);
    public DragEndedDelegate dragEndedCallback;

    private bool isDragged = false;
    private Vector3 mouseDragStartPosition;
    private Vector3 spriteDragStartPosition;
    private Vector3 startPos;

    private int b, l, r, t;

    private List<Transform> objs = new List<Transform>();
    private Transform obj;

    private void OnMouseDown()
    {
        startPos = gameObject.transform.position;
        obj = transform;
        bool stop = false;
        for(int i = 0; stop == false; i++)
        {
            objs.Add(obj);
            obj.GetComponent<Draggable>().isDragged = true;
            if(obj == transform)
            {
                gameObject.transform.GetComponent<BlockManager>().RemovePar(gameObject.transform);
            }
            if(obj.GetComponent<BlockManager>().childBlock)
            {
                obj = obj.GetComponent<BlockManager>().childBlock;
            }
            else
            {
                gameObject.transform.GetComponent<BlockManager>().RemoveCh(gameObject.transform);
                stop = true;
            }
        }
        mouseDragStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spriteDragStartPosition = transform.localPosition;
        b = gameObject.transform.Find("base").GetComponent<SortingGroup>().sortingOrder;
        l = gameObject.transform.Find("left").GetComponent<SortingGroup>().sortingOrder;
        r = gameObject.transform.Find("right").GetComponent<SortingGroup>().sortingOrder;
        t = gameObject.transform.Find("base").Find("Canvas").GetComponent<Canvas>().sortingOrder;

        gameObject.transform.Find("base").GetComponent<SortingGroup>().sortingOrder = 99;
        gameObject.transform.Find("left").GetComponent<SortingGroup>().sortingOrder = 98;
        gameObject.transform.Find("right").GetComponent<SortingGroup>().sortingOrder = 98;
        gameObject.transform.Find("base").Find("Canvas").GetComponent<Canvas>().sortingOrder = 100;
    }

    private void OnMouseDrag()
    {
        if(isDragged)
        {
            transform.localPosition = spriteDragStartPosition + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseDragStartPosition);
            for(int i = 0; i < objs.Count; i++)
            {
               objs[i].localPosition = transform.localPosition - new Vector3(0, i - objs[i].localScale.y*i, 0); 
            }
        }
    }

    private void OnMouseUp()
    {
        for(int i = 0; i < objs.Count; i++)
        {
            obj.GetComponent<Draggable>().isDragged = false;
        }
        objs.Clear();
        dragEndedCallback(this);
        gameObject.transform.Find("base").GetComponent<SortingGroup>().sortingOrder = b;
        gameObject.transform.Find("left").GetComponent<SortingGroup>().sortingOrder = l;
        gameObject.transform.Find("right").GetComponent<SortingGroup>().sortingOrder = r;
        gameObject.transform.Find("base").Find("Canvas").GetComponent<Canvas>().sortingOrder = t;
        
    }

    public void CancelDrag()
    {
        print("cancelled");
        gameObject.transform.position = startPos;
        gameObject.transform.GetComponent<BlockManager>().Remove(gameObject.transform);
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if(coll.transform.tag == "Delete")
        {
            if (!isDragged)
            {
                Destroy(gameObject);
            }
        }
    }

}
