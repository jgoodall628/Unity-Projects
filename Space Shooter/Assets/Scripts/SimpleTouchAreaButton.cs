using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SimpleTouchAreaButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{


    private bool touched;
    private bool canFire;
    private int pointerID;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        touched = false;
    }
    public void OnPointerDown(PointerEventData data)
    {
        // Set our start point

        if (!touched)
        {
            touched = true;
            canFire = true;
            pointerID = data.pointerId;
        }

    }
    public void OnPointerUp(PointerEventData data)
    {
        // reset everything
        if (data.pointerId == pointerID)
        {
            touched = false;
            canFire = false;
        }

    }

    public bool CanFire()
    {
        return canFire;
    }

}
