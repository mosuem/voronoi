using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAdmin : MonoBehaviour {
    public GameObject dot;
    public List<GameObject> objects;
    internal Dictionary<GameObject, float> isPressed = new Dictionary<GameObject, float> ();
    internal Dictionary<GameObject, float> advantages = new Dictionary<GameObject, float> ();
    public bool somethingWasPressed;

    // Start is called before the first frame update
    void Start () {
        foreach (var item in objects) {
            isPressed[item] = -1f;
        }
    }

    internal void stopRunning (GameObject gameObject) {
        isPressed[gameObject] = -1f;
    }

    internal void startRunning (GameObject gameObject) {
        isPressed[gameObject] = 0.0001f;
        advantages[gameObject] = Time.time;
    }

    // Update is called once per frame
    void LateUpdate () {
        if (Input.GetMouseButtonUp (0) && !somethingWasPressed) {
            var position = Input.mousePosition;
            position.z = 5f;
            GameObject dot1 = Instantiate (dot);
            dot1.transform.position = Camera.main.ScreenToWorldPoint (position);
            objects.Add (dot1);
            isPressed[dot1] = -1;
        }
        somethingWasPressed = false;
    }

    public void Reset () {
        somethingWasPressed = true;
        bool isRunning = false;
        foreach (var obj in objects) {
            if (isPressed[obj] >= 0) {
                isRunning = true;
                break;
            }
            // GameObject.Destroy(obj);
        }
        if (isRunning) {
            foreach (var obj in objects) {
                stopRunning (obj);
            }
        } else {
            foreach (var obj in objects) {
                startRunning (obj);
            }
        }
    }
}