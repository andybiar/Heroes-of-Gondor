#pragma strict

private var centerx : float;
private var centery : float;
private var centerz : float;

private var timeRemaining : float;
private var distancePerSecond : Vector3[];
private var animating : boolean;

public var duration : float = 1.0f;

public var center : GameObject;
public var topLeft : GameObject;
public var topRight : GameObject;
public var bottomLeft : GameObject;
public var bottomRight : GameObject;
public var points : GameObject[];
private var startPoints: Vector3[];



function Start () {
	initIndicator();
	startAnimating();
}

function Update () {
	if (animating) {
		timeRemaining -= Time.deltaTime;
		if (timeRemaining <= 0) {
			animating = false;
		}
		updateIndicator();
	}
}


function initIndicator() {
	points = [topLeft, topRight, bottomLeft, bottomRight];
	centerx = center.rigidbody.position.x;
	centery = center.rigidbody.position.y;
	centerz = center.rigidbody.position.z;
	
	timeRemaining = duration;
	animating = false;
	
	setIndicatorStartPos();
	
	distancePerSecond = new Vector3[points.length];
	
	for(var i = 0; i < points.length; i++) {
		var point = points[i];
		var v  = Vector3((centerx - point.rigidbody.position.x)/duration,
						(centery - point.rigidbody.position.y)/duration,
						(centerz - point.rigidbody.position.z)/duration);
		distancePerSecond[i] = v;
	}
}

function setIndicatorStartPos() {
	startPoints = new Vector3[points.length];
	for(var i = 0; i < points.length; i++) {
		startPoints[i] = points[i].rigidbody.position;
	}
}

function resetIndicatorPos() {
	for(var i = 0; i < points.length; i++) {
		points[i].rigidbody.position = startPoints[i];
	}
}

function startAnimating() {
	resetIndicatorPos();
	animating = true;
	timeRemaining = duration;
}

function updateIndicator() {
	for(var i = 0; i < points.length; i++) {
		var point = points[i];
		var dx : float;
		var dy : float;
		var dz : float;
		
		dx = Time.deltaTime * distancePerSecond[i].x;
		dy = Time.deltaTime * distancePerSecond[i].y;
		dz = Time.deltaTime * distancePerSecond[i].z;
		
		//Debug.Log(dx + " " + dy + " " + dz);
		
		point.rigidbody.position.x += dx;
		point.rigidbody.position.y += dy;
		point.rigidbody.position.z += dz;
	}
}