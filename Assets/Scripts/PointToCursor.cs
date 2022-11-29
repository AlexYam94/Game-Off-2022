using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToCursor : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;
    [SerializeField] SpriteRenderer _playerSprite;
    [SerializeField] bool _changeFacing = true;
    [SerializeField] float _maxAngle = 90f;
    [SerializeField] bool _invertArmScale = false;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= 0) return;
        ////rotation
        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = 5.23f;

        //Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        //mousePos.x = mousePos.x - objectPos.x;
        //mousePos.y = mousePos.y - objectPos.y;

        //float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        //Debug.Log(angle);
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;
        Vector2 difference = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;

        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        var scale = _playerTransform.localScale;
        if (_changeFacing)
        {
            if (rotationZ <= (_maxAngle * -1) || rotationZ > _maxAngle)
            {
                //_playerSprite.flipX = true;
                scale.x = -1;
                transform.localRotation = Quaternion.Euler(_invertArmScale? 0 : 180, 0, rotationZ);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(_invertArmScale? 180 : 0, 0, rotationZ);
                scale.x = 1;
                //_playerSprite.flipX = false;
            }
            _playerTransform.localScale = scale;
        }
        else
        {
            if(scale.x >= 1)
            {
                if (rotationZ <= (_maxAngle * -1) || rotationZ > _maxAngle)
                {
                    //_playerSprite.flipX = true;
                    scale.x = -1;
                    transform.localRotation = Quaternion.Euler(180, 0, rotationZ);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
                    scale.x = 1;
                    //_playerSprite.flipX = false;
                }
            }
            else
            {
                if (rotationZ >= -180 - (_maxAngle * -1) || rotationZ < 180 - _maxAngle)
                {
                    //_playerSprite.flipX = true;
                    scale.x = -1;
                    transform.localRotation = Quaternion.Euler(180, 0, rotationZ);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
                    scale.x = 1;
                    //_playerSprite.flipX = false;
                }

            }
        }
        transform.localScale = _invertArmScale? new Vector3(scale.x * -1, scale.y * -1, scale.z) : scale;
    }
}
