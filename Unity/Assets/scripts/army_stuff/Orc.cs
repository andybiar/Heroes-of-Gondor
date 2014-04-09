using UnityEngine;
using System.Collections;
using BoothGame;

public class Orc : Infantry, Enemy, Lockable {
	private GameObject selectionAura;
	private float highlightStrength = .35f;
	private float separationL = 999;
	private float separationR = 999;
	private string lName, rName;

	protected int turnDegrees;
	protected bool turning;
	protected float startRotation;

	public Stats stats;

	void Awake() {
		selectionAura = transform.GetChild(0).gameObject;
	}

	public void onLock() {
		selectionAura.SetActive(true);
		//Color c = renderer.material.color;
		//float h = highlightStrength;
		//renderer.material.color = new Color(c.r + h, c.g + h, c.b + h);
	}

	// When Gandalf hit an orc with a spell, it dies
	public virtual void onFire() {
		Debug.Log("Orc shot by Gandalf");
		int i = random.Next();
		mySounds.PlayOneShot(Resources.Load<AudioClip>("Orc/falling" + (i%3)));
	}

	public void onStab() {
		Debug.Log("Orc death by stabbing");
		int i = Random.Range(0, 6);
		mySounds.PlayOneShot(Resources.Load<AudioClip>("Orc/die"+i));              
		die();
	}

	public void onArrow() {
		Debug.Log("Orc death by arrow");
		int i = Random.Range(0,6);
		mySounds.PlayOneShot(Resources.Load<AudioClip>("Orc/die"+i));
		die();
	}

	protected override void onFall() {
		animation.Play("Flail");

		int i = Random.Range(1, 3);
		mySounds.PlayOneShot(Resources.Load<AudioClip>("Orc/falling" + i));
		// TODO: play falling sound
	}

	protected override void onCrash() {
		die();
		animation.Play("FlailToDead");
		Debug.Log("Orc death by falling");
		// TODO: play crashing sound
	}

	protected override void onGuardEngage() {
	}

	protected override void onMyStab() {
		animation.CrossFade("Strike");
	}

	public void onRelease() {
		selectionAura.SetActive(false);
		//Color c = renderer.material.color;
		//float h = highlightStrength;
		//renderer.material.color = new Color(c.r - h, c.g - h, c.b - h);
	}

	public bool isItAlive() {
		return getIsAlive();
	}

	private bool isAlly(Transform t) {
		Component[] allies = t.GetComponents(typeof(Ally));
		
		if (allies.Length > 0 && ((Ally)allies[0]).getIsAlive()) return true;
		return false;
	}
		
	protected override Transform aggroCast() {
		Vector3 v = new Vector3(0,.8f,0) + transform.forward * .2f;
		Debug.DrawRay(transform.position + v, 
		              Quaternion.Euler(0, -6, 0) * (transform.forward) * aggroRange);
		Debug.DrawRay(transform.position + v,
		              Quaternion.Euler(0, 6, 0) * (transform.forward) * aggroRange);

		// Double Raycast
		RaycastHit l = new RaycastHit();
		RaycastHit r = new RaycastHit();

		bool isL = Physics.Raycast(
			transform.position + new Vector3(0,.6f,0), 
			Quaternion.Euler(0,-6,0)*transform.forward, out l, aggroRange);
		bool isR = Physics.Raycast(
			transform.position + new Vector3(0,.6f,0), 
			Quaternion.Euler(0,6,0)*transform.forward, out r, aggroRange);

		float allyDist = 999;
		Transform ally = null;

		// Figure out what to return
		if (isL) {
			if (isAlly(l.transform)) {
				ally = l.transform;
				allyDist = l.distance;
			}
			else if (!l.transform.renderer) {
				separationL = l.distance;
				lName = l.transform.name;
			}
			else {
				// you hit a trigger
			}
		}
		if (isR) {
			if (isAlly(r.transform) && r.distance < allyDist) {
				allyDist = r.distance;
				ally = r.transform;
			}
			else if (!r.transform.renderer){
				separationR = r.distance;
				rName = r.transform.name;
			}
			else {
				// you're looking at a trigger
			}
		}
		return ally;
	}

	// CALLED WHEN THE TOWER LANDS AT THE WALL
	public void setAttackRun() {
		currentStance = Stance.ATTACK;
		currentTask = Task.RUNNING;
		animation.Play("Run");
		if (!canTalk) return;

		mySounds.PlayOneShot(Resources.Load<AudioClip>("Orc/battlecry2"));
	}

	protected override void die() {
		currentTask = Task.IDLE;
		currentStance = Stance.DEAD;
		setIsAlive(false);
		stats.orcDied();
	}

	protected override void charge() {
		move();
		
		Transform t = aggroCast();
		
		if (t) {
			setTarget(t);
			currentTask = Task.ENGAGING;
		}
	}

	protected override void specialBehavior() {}

	private void move() {
		// SHIT BE'S IN THE WAY
		if (separationL < .5 && separationR < .5) {
			if (moving) {
				moving = false;
				animation.Play("RunToStand");
			}
			Debug.DrawRay(transform.position, transform.forward, Color.magenta);
			//Debug.Log("Orc obstructed by: " + lName + ", " + rName);
			return;
		}
		else {
			moving = true;
			if (turning == true) {
				transform.Rotate(new Vector3(0, turnDegrees/40.0f, 0));
				if (Mathf.Abs(transform.rotation.eulerAngles.y - (startRotation + turnDegrees)) < 5) {
					turning = false;
				}
			}
			transform.position += transform.forward * speed * Time.deltaTime;
		}
	}

	public void turn(int degrees) {
		turning = true;
		startRotation = transform.rotation.eulerAngles.y;
		turnDegrees = degrees;
	}
}