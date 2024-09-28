using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchScript : MonoBehaviour
{
    [SerializeField] Transform spot;


    void Update()
    {
        Vector3 position = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        spot.position += position/10;

        spot.localPosition = new Vector3(Mathf.Clamp(spot.localPosition.x, -4, 4), 0, Mathf.Clamp(spot.localPosition.z, -3, 4.8f));
    }
}
