using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField] int health = 100;
	[SerializeField] float shotCounter;
	[SerializeField] float minTimeBetweenShots = 0.2f;
	[SerializeField] float maxTimeBetweenShots = 3f;
	[SerializeField] GameObject projectile;
	[SerializeField] float projectileSpeed = 10f;
	[SerializeField] GameObject deathVFX;
	[SerializeField] AudioClip deathSound;
	[SerializeField] [Range(0,1)] float deathSoundVolume = 0.7f;
	[SerializeField] AudioClip shootSound;
	[SerializeField] [Range(0, 1)] float shootSoundVolume = 0.3f;

	void Start()
	{
		shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
	}

	void Update()
	{
		CountDownAndShoot();
	}
	
	void CountDownAndShoot()
	{
		shotCounter -= Time.deltaTime;
		if (shotCounter <= 0f)
		{
			Fire();
			shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
		}
	}

	void Fire()
	{
		GameObject laser = Instantiate(projectile, transform.position, Quaternion.identity);
		laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
		AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
	}

	private void OnTriggerEnter2D(Collider2D target) {
		DamageDealer damageDealer = target.gameObject.GetComponent<DamageDealer>();
		health -= damageDealer.GetDamage();
		Destroy(target.gameObject);

		if (health <= 0) {
			Destroy(gameObject);
			GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
			Destroy(explosion, 1f);
			AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
		}
	}
}
