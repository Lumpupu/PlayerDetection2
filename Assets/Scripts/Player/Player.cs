using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageble
{
    [SerializeField] private string Name;
    [SerializeField] private float TurnSpeed;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private int HealthAmount = 100;
    [SerializeField] private Material DieMaterial;

    private Renderer _renderer;
    private int _health;
    public int Health 
    { 
        get => _health;
        private set
        {
            if(value > 0) _health -= value;
            else Die();
        }
    }

    public void Detected()
    {
        Debug.Log($"Player: {Name} detected");
    }

    public void Die()
    {
        Debug.Log($"Player: {Name} die");
        _renderer.material = DieMaterial;
        //Destroy(this.gameObject);
    }

    public void GetDamage(int damage)
    {
        Health -= damage;
        Debug.Log($"Player: get {damage} damage, health = {_health}");
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.W))
            transform.position += Vector3.forward * MoveSpeed;
        
        if(Input.GetKey(KeyCode.S)) 
            transform.position += Vector3.back * MoveSpeed;

        if(Input.GetKey(KeyCode.A)) 
            transform.Rotate(Vector3.up * -TurnSpeed * Time.deltaTime);

        if(Input.GetKey(KeyCode.D)) 
            transform.Rotate(Vector3.up * TurnSpeed * Time.deltaTime);
    }

    private void Start()
    {
        _health = HealthAmount;
        _renderer = GetComponent<Renderer>();
    }

    private void Reset()
    {
        Name = "Standart";
        TurnSpeed = 2;
        MoveSpeed = 2;
        HealthAmount = 100;
    }
}