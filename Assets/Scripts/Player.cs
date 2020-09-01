using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[Header("Player")]
	[SerializeField] float moveSpeed = 10f;
	[SerializeField] float padding = 1f;
	[SerializeField] int health = 200;
	[SerializeField] GameObject deathVFX;
	[SerializeField] AudioClip deathSound;
	[SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f;
	[SerializeField] AudioClip shootSound;
	[SerializeField] [Range(0, 1)] float shootSoundVolume = 0.3f;

	[Header("Projectile")]
	[SerializeField] GameObject laserPrefab;
	[SerializeField] float projectileSpeed = 10f;
	[SerializeField] float projectileFireTime = 0.5f;

	float xMin, xMax, yMin, yMax;

	Coroutine fireCoroutine;

	void Start () {
		SetBorder();
	}
	
	void Update () {
		Move();
		Fire();
	}

	// Untuk mengatur tembakan peluru
	void Fire() {
		// Bisa menggunakan Input.GetKeyDown
		if (Input.GetButtonDown("Fire1")) { 
			// Fire1 menyesuaikan settingan Pada Input Manager. Default = Left Ctrl
			// Pada project ini diubah menjadi tombol "Space".
			fireCoroutine = StartCoroutine(FireContinously());
		}
		if (Input.GetButtonUp("Fire1")) {
			StopCoroutine(fireCoroutine);
		}
	}

	// Coroutine untuk menembakkan peluru terus menerus
	IEnumerator FireContinously() {
		while(true){
			GameObject laser = Instantiate(
					laserPrefab,
					transform.position,
					Quaternion.identity
				);

			laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
			AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
			yield return new WaitForSeconds(projectileFireTime);
		}
	}

	// Untuk mengatur gerakan Player
	void Move() {
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		float moveX = Mathf.Clamp(
			transform.position.x + horizontalInput * Time.deltaTime * moveSpeed,
			xMin,
			xMax
		);
		float moveY = Mathf.Clamp(
			transform.position.y + verticalInput * Time.deltaTime * moveSpeed,
			yMin,
			yMax
		);
		transform.position = new Vector2(moveX, moveY);
	}

	void SetBorder() {
		Camera gameCamera = Camera.main;
		xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + padding;
		xMax = gameCamera.ViewportToWorldPoint(new Vector3(1,0,0)).x - padding;
		yMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).y + padding;
		yMax = gameCamera.ViewportToWorldPoint(new Vector3(0,1,0)).y - padding;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
		ProcessHit(damageDealer);
		Destroy(other.gameObject);
	}

	private void ProcessHit(DamageDealer damageDealer)
	{
		health -= damageDealer.GetDamage();
		if (health <= 0)
		{
			gameObject.SetActive(false);
			GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
			Destroy(explosion, 1f);
			AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
			Invoke("GameOver", 2f);
		}
	}

	private void GameOver()
	{
		print("gameover");
		GameObject.Find("Level").GetComponent<Level>().LoadGameOver();
	}
}
