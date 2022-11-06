using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ChooseUpgrade : MonoBehaviour
{
    [SerializeField] Upgrade[] upgradeList;
    [SerializeField] Button[] _upgradeButtons;
    [SerializeField] float _selectedUpgradeButtonDisappearAfterSecond = 1.5f;
    [SerializeField] float _chooseUpgradeTime = 10f;
    [SerializeField] Scrollbar _chooseUpgradeSlider;

    UpgradeController _playerUpgradeController;
    GameObject _selectedUpgradeButton;
    private float _upgradeTimeCounter = 0;
    private bool _isChoosingUpgrade = false;

    // Start is called before the first frame update
    void Start()
    {
        _playerUpgradeController = GameObject.FindGameObjectWithTag("Player").GetComponent<UpgradeController>();
    }

    private void Update()
    {
        if (_upgradeTimeCounter > 0)
        {
            _upgradeTimeCounter -= Time.deltaTime;
            _chooseUpgradeSlider.size = _upgradeTimeCounter/_chooseUpgradeTime;
        }
        if (_isChoosingUpgrade && _upgradeTimeCounter <= 0)
        {
            _upgradeButtons[0].onClick.Invoke();
            _chooseUpgradeSlider.gameObject.SetActive(false);
        }
    }

    public void ShowUpgrades(int numOfUpgrades)
    {
        _chooseUpgradeSlider.gameObject.SetActive(true);
           Upgrade[] nextUpgrades = new Upgrade[numOfUpgrades];
        for(int i = 0; i < numOfUpgrades; i++)
        {
            Upgrade u;
            bool duplicated;
            do
            {
                duplicated = false;
                u = upgradeList[Random.Range(0, upgradeList.Length - 1)];
                foreach(var n in nextUpgrades)
                {
                    if(n!=null && n.upgradeType == u.upgradeType)
                    {
                        duplicated = true;
                    }
                }
            } while (!_playerUpgradeController.CanUpgrade(u.upgradeType) || duplicated);

            _upgradeButtons[i].onClick.RemoveAllListeners();
            _upgradeButtons[i].gameObject.SetActive(true);
            _upgradeButtons[i].image.sprite = u.upgradeIcon;
            _upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = u.description;
            _upgradeButtons[i].onClick.AddListener(Upgrade(u.upgradeType, _upgradeButtons[i]));
            nextUpgrades[i] = u;
        }
        _upgradeTimeCounter = _chooseUpgradeTime;
        _isChoosingUpgrade = true;
    }

    public UnityAction Upgrade(UpgradeEnum type, Button button)
    {
        return () =>
        {
            _playerUpgradeController.Upgrade(type);
            DisableNotSelectedButton(button);
            _selectedUpgradeButton = button.gameObject;
            StartCoroutine("DisableSelectedButtonAfterSeconds");
            _chooseUpgradeSlider.gameObject.SetActive(false);
            _isChoosingUpgrade = false;
        };
    }

    public void DisableNotSelectedButton(Button button)
    {
        foreach(var b in _upgradeButtons)
        {
            if (!b.Equals(button))
            {
                b.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator DisableSelectedButtonAfterSeconds()
    {
        yield return new WaitForSeconds(_selectedUpgradeButtonDisappearAfterSecond);
        _selectedUpgradeButton.SetActive(false);
        _selectedUpgradeButton = null;
    }
}
