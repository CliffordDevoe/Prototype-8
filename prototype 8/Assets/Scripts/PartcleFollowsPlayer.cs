using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartcleFollowsPlayer : MonoBehaviour
{
    public GameObject  Player;
    private Transform  PlayerTransform;
    void Start()
    {
        PlayerTransform = Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newyPos = new Vector3(transform.position.x, transform.position.y, PlayerTransform.position.z);
        transform.position = newyPos;  
    }
}
