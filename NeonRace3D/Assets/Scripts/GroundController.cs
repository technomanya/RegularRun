using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    [SerializeField] private Material _matGround;
    [SerializeField] private Material _matGroundSelf;
    [SerializeField] private Renderer _rendererSelf;

    [SerializeField] private bool _colorChange = false;
    [SerializeField] private float playerSpeed = 10;
    void Start()
    {
        _colorChange = false;
        _rendererSelf = GetComponent<Renderer>();
        _matGroundSelf = _rendererSelf.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (_colorChange)
        {
            _rendererSelf.material.Lerp(_matGroundSelf, _matGround, Time.deltaTime*playerSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<PlayerController>())
            {
                playerSpeed = other.gameObject.GetComponent<PlayerController>().speed;
            }
            _colorChange = true;
        }
    }
}
