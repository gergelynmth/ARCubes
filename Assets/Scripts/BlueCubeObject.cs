using System.Collections.Generic;
using UnityEngine;

public class BlueCubeObject : Cube
{
    public override void move(List<Cube> cubes)
    {
        foreach (var otherCube in cubes)
        {
            if (this != otherCube && otherCube.gameObject.CompareTag("RedTeam"))
            {
                var distance = Vector3.Distance(transform.position,otherCube.transform.position);
                if (distance < Radius)
                {
                    moveCube(otherCube.transform.position, MovementSpeed);
                }
                else
                {
                    Radius += 0.1f;
                }
            }
        }
    }

    public override void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("RedTeam"))
        {
            Cube enemy = other.gameObject.GetComponent<Cube>();
			
            takeDamage(enemy.Damage);
            pushBack(other, enemy);
        }
    }
}