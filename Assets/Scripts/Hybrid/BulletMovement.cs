using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour {

	public string _owner;
	private float _speed;
	private float _damage;
	public Vector3 _direction;

	void Start () 
	{
		//sets variables
		_speed = 60.0f;

		_direction.y = 0.0f;

		_damage = 1.0f;

		_direction.Normalize ();
	}

	void Update () 
	{
		//moves object based on speed, direction and delta time
		transform.position += _direction * _speed * Time.deltaTime;
	}

	void OnTriggerEnter(Collider collisionObj)
	{
		//checks if bullet owner is player
		if (_owner == "Player") 
		{
			//checks if the collision object tag is not player object, bullet object, ammo object or turret object
			if (collisionObj.gameObject.tag != "Player" && collisionObj.gameObject.tag != "Bullet" && collisionObj.gameObject.tag != "Ammo" && collisionObj.gameObject.tag != "Turret") 
			{
				//destroys bullet object
				Destroy (gameObject);
			}

			//checks if collision object is enemy
			if (collisionObj.gameObject.tag == "Enemy") 
			{
				//damages enemy object
				collisionObj.GetComponent<EnemyController> ().DamageEnemy (_damage);
			}
		}
		//checks if bullet owner is turret
		else if (_owner == "Turret") 
		{
			//checks if the collision object tag is not player object, bullet object, ammo object or turret object
			if (collisionObj.gameObject.tag != "Player" && collisionObj.gameObject.tag != "Bullet" && collisionObj.gameObject.tag != "Ammo" && collisionObj.gameObject.tag != "Turret") 
			{
				//destroys bullet object
				Destroy (gameObject);
			}

			//checks if collision object is enemy
			if (collisionObj.gameObject.tag == "Enemy") 
			{
				//damages enemy object
				collisionObj.GetComponent<EnemyController> ().DamageEnemy (3.0f);
			}
		}
	}
}
