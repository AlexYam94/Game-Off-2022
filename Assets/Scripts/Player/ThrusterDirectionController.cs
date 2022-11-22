using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterDirectionController : MonoBehaviour
{
    [SerializeField] bool _isOppositeDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (!_isOppositeDirection)
        {
            //var rotationVec = transform.rotation.eulerAngles;
            //rotationVec.z =
        }
        else
        {

        }
        
    }
}
