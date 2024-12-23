using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject _player;

    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.05f;
    private GameObject _BG2;
    private GameObject _BG3;
    void Start()
    {
        _player = GameObject.Find("Player");
        _BG2 = GameObject.Find("BG2");
        _BG3 = GameObject.Find("BG3");
    }

    // Update is called once per frame
    void Update()
    {
        _BG2.transform.position = new Vector3(-transform.position.x / 16.0f, _BG2.transform.position.y, _BG2.transform.position.z);
        _BG3.transform.position = new Vector3(-transform.position.x / 10.0f, _BG3.transform.position.y, _BG3.transform.position.z);
        transform.position =Vector3.SmoothDamp(transform.position,new Vector3(_player.transform.position.x,_player.transform.position.y,0)+offset,ref velocity,smoothTime);
    }
}
