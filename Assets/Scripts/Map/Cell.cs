using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour
{
    private SpriteRenderer _image;
    private BoxCollider bx;
    [SerializeField]
    private Color _overColor;
    [SerializeField]
    private Color _green;

    private void Awake()
    {
       
        _image = GetComponent<SpriteRenderer>();
        _image.color = _green;
    }

    private void Start()
    {
        bx = gameObject.AddComponent<BoxCollider>();
        bx.isTrigger = true;
        bx.transform.localScale = new Vector3(bx.transform.localScale.x, bx.transform.localScale.y, 0.1f);
        _image.enabled = false;
        //_image.color = _green;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (!_image.enabled)
            {
                _image.enabled = true;
            }
            
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            if (_image.enabled)
            {
                _image.enabled = false;
            }
               
        }
    }


    //public void OnMouseOver()
    //{
    //    Debug.Log("OnMouseOver");
    //    _image.color = _overColor;
    //}

    private void OnMouseExit()
    {
        _image.color = _green;
    }

    //public void OnMouseDown()
    //{
    //    Debug.Log("OnMouseDown");
    //    _material = null;
    //}

    //public void OnMouseDrag()
    //{
    //    Debug.Log("OnMouseDrag");
    //    _material = null;
    //}

    public void OnMouseEnter()
    {
        _image.color = _overColor;
    }
}
