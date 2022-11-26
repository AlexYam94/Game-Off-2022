using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    [SerializeField] GameObject _leftMissle;
    [SerializeField] GameObject _rightMissle;
    [SerializeField] GameObject[] _batteries;
    [SerializeField] GameObject _jetBackpack;
    [SerializeField] GameObject _otherArm;
    [SerializeField] GameObject _originalArm;

    public bool isLeftMissleEnabled = false;
    public bool isRightMissleEnabled = false;
    public bool isExternalBatteryEnabled = false;
    public bool isJetBackpackEnabled = false;
    public bool isDualWieldingEnabled = false;

    private ThrusterController _thrustController;
    private PlayerController _playerController;
    // Start is called before the first frame update
    void Start()
    {
        _thrustController = GetComponent<ThrusterController>();
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLeftMissleEnabled)
        {
            EnableMissle(_leftMissle);
        }
        if (isRightMissleEnabled)
        {
            EnableMissle(_rightMissle);
        }
        if (isExternalBatteryEnabled)
        {
            _batteries.ToList().ForEach(f => f.SetActive(true));
            _thrustController.maxEnergy *= 2;
            isExternalBatteryEnabled = false;
        }
        if (isJetBackpackEnabled)
        {
            _jetBackpack.SetActive(true);
            _thrustController.maxEnergy *= 1.25f;
            _playerController.thrustSpeedMutiplier *= 1.5f;
            isJetBackpackEnabled = false;
        }
        if (isDualWieldingEnabled)
        {

        }
        
    }

    private void EnableMissle(GameObject missleAttachment)
    {
        missleAttachment.SetActive(true);
    }
}
