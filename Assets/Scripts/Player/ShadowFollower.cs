using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class ShadowFollower : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        Vector3 newPos = player.position;
        // Adjust the Y position or offset for shadow height if necessary
        newPos.y = 0; // Adjust to floor or shadow plane
        transform.position = newPos;

        // transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}

