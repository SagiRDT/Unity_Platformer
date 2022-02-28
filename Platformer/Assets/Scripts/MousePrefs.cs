/*
 *  MousePrefs
 *  Show/Hide the mouse in the cur screen
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePrefs : MonoBehaviour
{

    // config params
    [SerializeField] bool showMouse = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = showMouse;
    }

}
