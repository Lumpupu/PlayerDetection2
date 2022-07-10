using System.Collections;
using UnityEngine;
public class MachineGunner : Enemy
{
    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator SearchCoroutine()
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
                    Debug.Log($"Enemy({_enemyName}): set mode to attack, START ShootCoroutine");
                    StartCoroutine(ShootCoroutine());
                }
                else Debug.Log($"Enemy({_enemyName}): lost target"); // lost target from view
            }
        }
    }

    protected override IEnumerator ShootCoroutine()
    {
        while (_playerDetected)
        {
            switch (Weapon.Shoot(PlayerRef.transform.position))
            {
                case FiringStatus.reload:
                    {
                        yield return new WaitForSeconds(Weapon.ReloadTime);
                        Weapon.Reload();
                        Debug.Log($"Enemy({_weaponName}): reload");
                        break;
                    }
                case FiringStatus.jammed:
                    {
                        Debug.Log($"Enemy({_weaponName}): jammed, wait 2 seconds");
                        yield return new WaitForSeconds(2);
                        break;
                    }
                case FiringStatus.firing:
                    Debug.Log($"Enemy({_weaponName}): firing");
                    break;
            }
            yield return new WaitForSeconds(Weapon.FireRateSeconds);
        }
        Debug.Log($"Enemy({_enemyName}): set mode to view, STOP ShootCoroutine");
        _enemyMode = EnemyMode.view;
        StopCoroutine(ShootCoroutine());
    }

    protected override void CheckInArea()
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
                    _playerDetected = PlayerRef.GetStatus() == PlayerStatus.life ? true : false;
                else
                    _playerDetected = false;
            }
            else
                _playerDetected = false;
        }
        else _playerDetected = false;
    }

    #region[Debug gizmos]
    private void OnDrawGizmos()
    {
        if(DebugGozmo)
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
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    #endregion

    private void Reset() // std values for machine gunner
    {
        DebugGozmo = true;
        HealthAmount = 150;
        TargetLayer = 1 << 8; // get layer from flags
        BarrierLayer = 1 << 9; // barrier
        Radius = 5F;
        Angle = 90F;
        CheckInAreaDelay = 0.2F;
    }
}
