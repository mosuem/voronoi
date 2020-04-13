using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getClick : MonoBehaviour
{
    private ObjectAdmin objectAdmin;

    // Start is called before the first frame update
    void Start()
    {
        objectAdmin = Camera.main.GetComponent<ObjectAdmin>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseUp()
    {
        Debug.Log("Click on " + this.gameObject.name);
        objectAdmin.somethingWasPressed = true;
        if (objectAdmin.isPressed[this.gameObject] > 0f)
        {
            objectAdmin.stopRunning(this.gameObject);
        }
        else
        {
            objectAdmin.startRunning(this.gameObject);
        }
    }
}
