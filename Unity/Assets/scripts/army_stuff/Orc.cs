using UnityEngine;
using System.Collections;
using BoothGame;

public class Orc : Infantry, Enemy, Lockable {
	private GameObject selectionAura;
	private float highlightStrength = .35f;
	private float separationL = 999;
	private float separationR = 999;
	private string lName, rName;

	public Stats stats;
	public AudioSource orcSounds;

	void Awake() {
		selectionAura = transform.GetChild(0).gameObject;
	}

	public void onLock() {
		selectionAura.SetActive(true);
		Color c = renderer.material.color;
		float h = highlightStrength;
		renderer.material.color = new Color(c.r + h, c.g + h, c.b + h);
	}

	// When Gandalf hit an orc with a spell, it dies
	public void onFire() {
		Debug.Log("Orc death by Gandalf");
		die();
	}

	public void onStab() {
		Debug.Log("Orc death by stabbing");
		die();
	}

	public void onArrow() {
		Debug.Log("Orc death by arrow");
		die();
	}

	protected override void onFall() {
		animation.Play("Flail");
		// TODO: play falling sound
	}

	protected override void onCrash() {
		die();
		animation.Play("FlailToDead");
		Debug.Log("Orc death by falling");
		// TODO: play crashing sound
	}

	public void onRelease() {
		selectionAura.SetActive(false);
		Color c = renderer.material.color;
		float h = highlightStrength;
		renderer.material.color = new Color(c.r - h, c.g - h, c.b - h);
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
		Vector3 v = new Vector3(0,.8f,0);
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
			else {
				separationL = l.distance;
				lName = l.transform.name;
			}
		}
		if (isR) {
			if (isAlly(r.transform) && r.distance < allyDist) {
				allyDist = r.distance;
				ally = r.transform;
			}
			else {
				separationR = r.distance;
				rName = r.transform.name;
			}
		}
		return ally;
	}

	public void setAttackRun() {
		currentStance = Stance.ATTACK;
		currentTask = Task.RUNNING;
		animation.Play("Run");
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

	private void move() {
		if (separationL < .5 && separationR < .5) {
			Debug.DrawRay(transform.position, transform.forward, Color.magenta);
			Debug.Log(lName + ", " + rName);
			return;
		}
		else {
			transform.position += transform.forward * speed * Time.deltaTime;
		}
	}
}