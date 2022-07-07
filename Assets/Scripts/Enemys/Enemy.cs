using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageble
{
    enum Mode : int { view = 0, attack }
    /*
    Умер - попал/не попал в триггер игрока
    возможно Движение
    Стрельба
    + Угол обзора
    +/- Какое препятствие

    когда игрок стреляет - enemy начинает стрелять в ответ
    */
    [SerializeField] private int  HealthAmount;
    [Header("Searchable player")]
    [SerializeField] private Player PlayerRef;
    [Space(5), Header("Layers for Raycasting")]
    [SerializeField] private LayerMask TargetLayer;
    [SerializeField] private LayerMask BarrierLayer;
    [Space(5), Header("Observation area")]
    [SerializeField] private float Radius = 5F;
    [Range(0, 360), SerializeField] private float Angle = 90F;
    [Space(5), Header("Area scan period")]
    [SerializeField] private float Delay = 0.2F;

    private bool _playerDetected;

    private Vector3 _position;
    private RaycastHit _hit;
    private Vector3 _positionPlayer, _directionToPlayer;
    private float _distanceToPlayer;

    public void GetDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        StartCoroutine(SearchCoroutine());
    }
    #region [SearchCoroutine and raycasting]
    private IEnumerator SearchCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Delay);
            CheckInArea();
            if (_playerDetected)
            {
                yield return new WaitForSeconds(2);
                CheckInArea();
                if (_playerDetected) PlayerRef.Detected();
                else Debug.Log("Maybe rats..."); // lost target from viwe
            }

        }
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
}
