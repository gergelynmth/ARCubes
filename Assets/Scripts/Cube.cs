using System.Collections.Generic;
using UnityEngine;
public abstract class Cube : MonoBehaviour
{
    //DRAGG AND DROPPING
    [SerializeField] private bool isSelected;

    public bool Selected
    {
        get => isSelected;
        set => isSelected = value;
    }
    
    //MOVEMENT
    private float movementSpeed = 0.1f;
    public float MovementSpeed
    {
        get => movementSpeed;
    }
    
    private float radius = 0.1f;
    public float Radius
    {
        get => radius;
        set => radius = value;
    }

    //COLLISION
    private Rigidbody2D rgby;

    //Healtbar
    [SerializeField] private int maxHealt = 100;
    public int MaxHealt
    {
        get => maxHealt;
    }
    
    private int currentHealt;
    public int CurrentHealt
    {
        get => currentHealt;
        set => currentHealt = value;
    }

    [SerializeField] private HealtBar healtBar;

    [SerializeField] private int damage = 10;
    public int Damage
    {
        get => damage;
    }

    [SerializeField] private int pushBackForce = 3;
    public int PushBackForce
    {
        get => pushBackForce;
    }
    
    void Start()
    {
        CurrentHealt = MaxHealt;
        healtBar.SetMaxHealt(MaxHealt);
    }

    //MOVEMENT
    public abstract void move(List<Cube> cubes);
    public void moveCube(Vector3 p, float speed)
    {
        float step = speed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, p, step);
        fallDestroy();
    }
    
    private void fallDestroy()
    {
        if (transform.position.y < -100)
        {
            Destroy();
        }
    }

    //COLLISION
    public abstract void OnCollisionStay(Collision other);

    public void takeDamage(int damage) {
        CurrentHealt -= damage;
        healtBar.SetHealt(CurrentHealt);
    }

    public void pushBack(Collision c, Cube p)
    {
        Vector3 dir = c.contacts[0].point - transform.position;
        dir = -dir.normalized;
        GetComponent<Rigidbody>().AddForce(dir * p.PushBackForce);
    }
    
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}