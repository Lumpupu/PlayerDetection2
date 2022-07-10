using UnityEngine;
public enum PlayerStatus { life = 0, dead }
public class Player : MonoBehaviour, IDamageble
{
    [SerializeField] private string Name;
    [SerializeField] private float TurnSpeed;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private int HealthAmount = 100;
    [SerializeField] private Material DieMaterial;

    private Renderer _renderer;
    private PlayerStatus _status;
    private int _health;

    public int Health 
    { 
        get => _health;
        private set
        {
            if(value > 0) _health = value;
            else if (_status == PlayerStatus.life) Die();
        }
    }

    public PlayerStatus GetStatus() => _status;

    public void Die()
    {
        _status = PlayerStatus.dead;
        Debug.Log($"Player({Name}): die");
        _renderer.material = DieMaterial;
        //Destroy(this.gameObject);
    }

    public void GetDamage(int damage)
    {
        Health -= damage;
        // Debug.Log($"Player({Name}): get {damage} damage, health = {Health}");
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        
        if(Input.GetKey(KeyCode.S)) 
            transform.Translate(-Vector3.forward * MoveSpeed * Time.deltaTime);

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