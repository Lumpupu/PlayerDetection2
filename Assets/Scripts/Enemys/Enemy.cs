using System.Collections;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    private enum EnemyMode : int { view = 0, attack }

    [SerializeField] private int  HealthAmount;
    [Header("Searchable player")] 
    [SerializeField] private Player PlayerRef;
    [SerializeField] private Weapon Weapon;
    [Space(5), Header("Layers for Raycasting")]
    [SerializeField] private LayerMask TargetLayer;
    [SerializeField] private LayerMask BarrierLayer;
    [Space(5), Header("Observation area")]
    [SerializeField] private float Radius = 5F;
    [Range(0, 360), SerializeField] private float Angle = 90F;
    [Space(5), Header("Area scan period")]
    [SerializeField] private float CheckInAreaDelay = 0.2F;

    private bool _playerDetected;
    private EnemyMode _enemyMode;

    private Vector3 _position;
    private RaycastHit _hit;
    private Vector3 _positionPlayer, _directionToPlayer;
    private float _distanceToPlayer;

    private string _weaponName; // debug

    private void Start()
    {
        _weaponName = Weapon.name;
        _enemyMode = EnemyMode.view;
        StartCoroutine(SearchCoroutine());
    }

    #region [SearchCoroutine() and CheckInArea() and ShootCoroutine()]
    private IEnumerator SearchCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(CheckInAreaDelay);
            CheckInArea();

            if (_playerDetected && _enemyMode != EnemyMode.attack)
            {
                yield return new WaitForSeconds(2); // wait for identificate
                CheckInArea();
                if (_playerDetected)
                {
                    _enemyMode = EnemyMode.attack;
                    Debug.Log("Enemy: set mode to attack, START ShootCoroutine");
                    StartCoroutine(ShootCoroutine());
                }
                else Debug.Log("Enemy: lost target"); // lost target from view
            }
        }
    }

    private IEnumerator ShootCoroutine()
    {
        while (_playerDetected)
        {
            switch (Weapon.Shoot(PlayerRef.transform.position))
            {
                case FiringStatus.reload:
                    {
                        yield return new WaitForSeconds(Weapon.ReloadTime);
                        Weapon.Reload();
                        Debug.Log($"Enemy.{_weaponName}: reload");
                        break;
                    }
                case FiringStatus.jammed:
                    {
                        Debug.Log($"Enemy.{_weaponName}: jammed");
                        yield return new WaitForSeconds(2);
                        break;
                    }
                case FiringStatus.firing:
                    Debug.Log($"Enemy.{_weaponName}: firing");
                    break;

            }
            yield return new WaitForSeconds(Weapon.FireRateSeconds);
        }
        Debug.Log("Enemy: set mode to view, STOP ShootCoroutine");
        _enemyMode = EnemyMode.view;
        StopCoroutine(ShootCoroutine());
    }

    private void CheckInArea()
    {
        _position = transform.position;
        if (Physics.CheckSphere(_position, Radius, TargetLayer))
        {
            _positionPlayer = PlayerRef.transform.position;
            _directionToPlayer = (_positionPlayer - _position).normalized;
            if (Vector3.Angle(transform.forward, _directionToPlayer) < Angle / 2)
            {
                _distanceToPlayer = Vector3.Distance(_position, _positionPlayer);

                if (!Physics.Raycast(_position, _directionToPlayer, out _hit, _distanceToPlayer, BarrierLayer))
                    _playerDetected = true;
                else
                    _playerDetected = false;
            }
            else
                _playerDetected = false;
        }
        else _playerDetected = false;
    }
    #endregion

    #region[Debug gizmo]
    private void OnDrawGizmos()
    {
        _position = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_position, Radius);
        Gizmos.DrawLine(_position, _position + DirectionFromAngle(transform.eulerAngles.y, -Angle / 2) * Radius);
        Gizmos.DrawLine(_position, _position + DirectionFromAngle(transform.eulerAngles.y, Angle / 2) * Radius);
        if (_playerDetected)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_position, PlayerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    #endregion
}
