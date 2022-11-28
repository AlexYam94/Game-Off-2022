using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ChooseUpgrade : MonoBehaviour
{
    [SerializeField] Upgrade[] upgradeList;
    [SerializeField] UpgradeButton[] _upgradeButtons;
    [SerializeField] float _selectedUpgradeButtonDisappearAfterSecond = 1.5f;
    [SerializeField] float _chooseUpgradeTime = 10f;
    [SerializeField] Scrollbar _chooseUpgradeSlider;
    [SerializeField] int _maxRandomUpgradeLoop = 50;

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
            _upgradeButtons[0].button?.onClick?.Invoke();
            _chooseUpgradeSlider.gameObject.SetActive(false);
        }
    }

    public void ShowUpgrades(int numOfUpgrades)
    {
        _chooseUpgradeSlider.gameObject.SetActive(true);
        int upgradeLoopCount = 0;
        ChooseNextUpgrades(numOfUpgrades, upgradeLoopCount);
        _upgradeTimeCounter = _chooseUpgradeTime;
        _isChoosingUpgrade = true;
    }

    private void ChooseNextUpgrades(int numOfUpgrades, int upgradeLoopCount)
    {
        Upgrade[] nextUpgrades = new Upgrade[numOfUpgrades];
        for (int i = 0; i < numOfUpgrades; i++)
        {
            Upgrade u;
            bool duplicated;
            do
            {
                duplicated = false;
                u = upgradeList[Random.Range(0, upgradeList.Length - 1)];
                foreach (var n in nextUpgrades)
                {
                    if (n != null && n.upgradeType == u.upgradeType)
                    {
                        duplicated = true;
                    }
                }
                if (upgradeLoopCount > _maxRandomUpgradeLoop) break;
                upgradeLoopCount++;
            } while (!_playerUpgradeController.CanUpgrade(u.upgradeType) || duplicated);

            if (upgradeLoopCount > _maxRandomUpgradeLoop)
            {
                nextUpgrades[i] = null;
                _upgradeButtons[i].button.onClick.RemoveAllListeners();
                _upgradeButtons[i].gameObject.SetActive(false);
            }
            else
            {
                _upgradeButtons[i].button.onClick.RemoveAllListeners();
                _upgradeButtons[i].gameObject.SetActive(true);
                _upgradeButtons[i].icon.sprite = u.upgradeIcon;
                _upgradeButtons[i].titleText.text = u.title;
                _upgradeButtons[i].description.text = u.description;
                _upgradeButtons[i].button.onClick.AddListener(Upgrade(u.upgradeType, _upgradeButtons[i].button));
                nextUpgrades[i] = u;
            }
        }
    }

    public UnityAction Upgrade(UpgradeEnum type, Button button)
    {
        return () =>
        {
            _playerUpgradeController.Upgrade(type);
            DisableNotSelectedButton(button);
            button.onClick.RemoveAllListeners();
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
            if (!b.button.Equals(button))
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
