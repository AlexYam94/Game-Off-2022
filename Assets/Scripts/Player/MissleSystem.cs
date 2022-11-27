using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissleSystem : MonoBehaviour
{
    [SerializeField] Transform _firePosition;
    [SerializeField] HomingMissile _missile;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] Slider _reloadSlider;
    [SerializeField] float _reloadTime;
    [SerializeField] TextMeshProUGUI _ammoText;


    public float fireRate = .5f;
    public float fireRateMultiplier;
    public float damageMultiplier = 1f;
    public bool subWeaponEnabled = false;
    //private Weapon _currentSubWeapon;
    public int maxAmmo = 10;
    public float reloadTimeMultiplier = 1f;

    private int _currentAmmo;
    private UpgradeController _upgradeController;
    float _reloadTimeCounter;
    float _fireCounter = 0;
    //private MissileFireLogic _fireLogic;

    private GameObject[] _targets = new GameObject[10];
    private bool _reloading;

    // Start is called before the first frame update
    void Start()
    {
        _currentAmmo = maxAmmo;
        _upgradeController = GetComponentInParent<UpgradeController>();
    }

    // Update is called once per frame
    void Update()
    {
        //DrawBox(GetScreenCenter(), Quaternion.identity, Vector3.one, Color.red);
        if (!subWeaponEnabled) return;
        _ammoText.text = _currentAmmo + "/" + maxAmmo;
        if (Input.GetKeyDown(KeyCode.R) && _currentAmmo < maxAmmo)
        {
            TriggerReload();
        }
        _fireCounter = Mathf.Max(0, _fireCounter);
        if (_reloadTimeCounter > 0)
        {
            _reloadSlider.value = _reloadTimeCounter / _reloadTime * reloadTimeMultiplier;
            _reloadTimeCounter -= Time.deltaTime;
            return;
        }
        else if (_reloading && _reloadTimeCounter <= 0)
        {
            _reloadSlider.gameObject.SetActive(false);
            _reloading = false;
            _currentAmmo = maxAmmo;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (_currentAmmo > 0)
            {
                if (_fireCounter <= 0)
                {
                    _fireCounter = fireRate * fireRateMultiplier;
                    var m = Instantiate(_missile, _firePosition.position, Quaternion.identity);
                    m.SetRotation(transform.parent.rotation);
                    Fire(m);
                    _currentAmmo--;
                }
            }
            else
            {
                TriggerReload();
            }
        }

    }

    private void Fire(HomingMissile m)
    {
        //TODO:
        m.direction = _firePosition.transform.right;
        m.damageMultiplier = _upgradeController.damageMultiplier;
    }

    private RaycastHit2D[] FindTargets()
    {
        var hits = Physics2D.BoxCastAll(transform.position, new Vector2(10, 10), 0, transform.forward, 10, _enemyLayer);
        var l = hits.ToList();
        l.Sort((a, b) =>
        {
            return Vector2.Distance(a.transform.position, transform.position).CompareTo(Vector2.Distance(b.transform.position, transform.position));
        });
        return l.ToArray();
    }

    private Vector2 GetScreenCenter()
    {
        return new Vector2(Screen.width / 2, Screen.height / 2);
    }
    private void TriggerReload()
    {
        if (_reloading) return;
        _reloadTimeCounter = _reloadTime * reloadTimeMultiplier;
        _reloadSlider.gameObject.SetActive(true);
        _reloadSlider.value = 0;
        _reloading = true;
    }

    public void DrawBox(Vector3 pos, Quaternion rot, Vector3 scale, Color c)
    {
        // create matrix
        Matrix4x4 m = new Matrix4x4();
        m.SetTRS(pos, rot, scale);

        var point1 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0.5f));
        var point2 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, 0.5f));
        var point3 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, -0.5f));
        var point4 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, -0.5f));

        var point5 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0.5f));
        var point6 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, 0.5f));
        var point7 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, -0.5f));
        var point8 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, -0.5f));

        Debug.DrawLine(point1, point2, c);
        Debug.DrawLine(point2, point3, c);
        Debug.DrawLine(point3, point4, c);
        Debug.DrawLine(point4, point1, c);

        Debug.DrawLine(point5, point6, c);
        Debug.DrawLine(point6, point7, c);
        Debug.DrawLine(point7, point8, c);
        Debug.DrawLine(point8, point5, c);

        Debug.DrawLine(point1, point5, c);
        Debug.DrawLine(point2, point6, c);
        Debug.DrawLine(point3, point7, c);
        Debug.DrawLine(point4, point8, c);

        // optional axis display
        Debug.DrawRay(m.GetPosition(), Vector3.forward, Color.magenta);
        Debug.DrawRay(m.GetPosition(), Vector3.up, Color.yellow);
        Debug.DrawRay(m.GetPosition(), Vector3.right, Color.red);
    }
}
