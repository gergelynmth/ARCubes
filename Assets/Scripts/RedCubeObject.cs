using System.Collections.Generic;
using UnityEngine;

public class RedCubeObject : Cube
{
    public override void move(List<Cube> cubes)
    {
        foreach (var otherCube in cubes)
        {
            if (this != otherCube && otherCube.gameObject.CompareTag("BlueTeam"))
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
        if (other.gameObject.CompareTag("BlueTeam"))
        {
            Cube enemy = other.gameObject.GetComponent<Cube>();
			
            takeDamage(enemy.Damage);
            pushBack(other, enemy);
        }
    }
}

 