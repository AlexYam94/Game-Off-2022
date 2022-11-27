using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    [SerializeField] GameObject[] bodyPartsPrefabs;
    [SerializeField] float _bodyPartsYOffset = 10f;
    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0, bodyPartsPrefabs.Length);
        var pos = transform.position;
        pos.y += _bodyPartsYOffset;
        Instantiate(bodyPartsPrefabs[index], pos, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
