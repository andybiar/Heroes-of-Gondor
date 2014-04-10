using UnityEngine;
using System.Collections;
using BoothGame;

public class Spearman : Infantry, Ally {
	private bool braced;
	private bool turning;
	private float turnDegrees;
	private float startRotation;

	public Spearman() : base() {
	}

	protected override Transform aggroCast() {
		Debug.DrawRay(transform.position + new Vector3(0,1.7f,0), transform.forward * aggroRange);

		// If we see an enemy within our aggro range, engage it in combat!
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast (transform.position + new Vector3(0,1.7f,0), transform.forward, out hit, aggroRange)) {
			Component[] enemies = hit.transform.GetComponents(typeof(Enemy));
			
			if (enemies.Length > 0 && ((Enemy)enemies[0]).getIsAlive()) return hit.transform;
		}

		return null;
	}

	public void turn(int degrees) {
		turning = true;
		startRotation = transform.rotation.eulerAngles.y;
		turnDegrees = degrees;
	}

	protected override void charge() {
		if (turning == true) {
			transform.Rotate(new Vector3(0, turnDegrees/130.0f, 0));
			if (Mathf.Abs(transform.rotation.eulerAngles.y - (startRotation + turnDegrees)) < 5) {
				turning = false;
			}
		}
	}

	protected override void die() {
	}

	protected override void onFall() {
		int i = random.Next(1,3);
		mySounds.PlayOneShot(Resources.Load<AudioClip>("Human/falling"+i));
		rigidbody.freezeRotation = false;
	}

	protected override void onCrash() {
	}

	protected override void specialBehavior() {
	}

	protected override void onGuardEngage() {
		if (!braced) {
			animation.Play("Brace");
			braced = true;
		}
		Debug.DrawRay(transform.position, transform.up * 5, Color.yellow);
	}

	public void onMace() {
		animation.CrossFade("Flail");
		int i = random.Next(1,3);
		mySounds.PlayOneShot(Resources.Load<AudioClip>("Human/falling"+i));
		rigidbody.freezeRotation = false;
	}

	protected override void onMyStab() {
		animation.CrossFade("Strike");
		int i = random.Next();
		if (i % 3 == 0) {
			i = random.Next();
			audio.PlayOneShot(Resources.Load<AudioClip>("Human/stab"+(i%3)));
		}
	}

	public void brace() {
		animation.Play("Brace");
		braced = true;
	}

	void OnCollisionEnter(Collision c) {
	}
}
