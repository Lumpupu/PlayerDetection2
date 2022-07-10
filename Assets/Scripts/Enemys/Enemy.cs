using System.Collections;
using UnityEngine;
public abstract class Enemy : MonoBehaviour
{
    protected enum EnemyMode : int { view = 0, attack }

    [SerializeField] protected int HealthAmount;
    [Header("Searchable player")] 
    [SerializeField] protected Player PlayerRef;
    [SerializeField] protected Weapon Weapon;
    [Space(5), Header("Layers for Raycasting")]
    [SerializeField] protected LayerMask TargetLayer;
    [SerializeField] protected LayerMask BarrierLayer;
    [Space(5), Header("Observation area")]
    [SerializeField] protected float Radius;
    [Range(0, 360), SerializeField] protected float Angle;
    [Space(5), Header("Area scan period")]
    [SerializeField] protected float CheckInAreaDelay = 0.2F;
    [Space(5), Header("Show gizmos?")]
    [SerializeField] protected bool DebugGozmo;

    protected bool _playerDetected;
    protected EnemyMode _enemyMode;

    protected Vector3 _position;
    protected RaycastHit _hit;
    protected Vector3 _positionPlayer, _directionToPlayer;
    protected float _distanceToPlayer;

    protected string _weaponName; // debug
    protected string _enemyName; // debug

    protected virtual void Start()
    {
        _weaponName = Weapon.name;
        _enemyName = this.name;
        _enemyMode = EnemyMode.view;
        StartCoroutine(SearchCoroutine());
    }

    protected abstract IEnumerator SearchCoroutine();
    protected abstract IEnumerator ShootCoroutine();
    protected abstract void CheckInArea();

}
