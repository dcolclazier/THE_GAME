#pragma strict
import Vectrosity;

var lineMaterial : Material;
var textureScale = 4.0;
var rounded = false;
var roundedRadius = 6.0;
private var selectionLine : VectorLine;
private var originalPos : Vector2;

function Start () {
	selectionLine = new VectorLine("Selection", new Vector2[5], lineMaterial, 4.0, LineType.Continuous, Joins.Fill);
	selectionLine.textureScale = textureScale;
}

function OnGUI () {
	GUI.Label(Rect(10, 10, 300, 25), "Click & drag to make a selection box");
}

function Update () {
	if (Input.GetMouseButtonDown(0)) {
		originalPos = Input.mousePosition;
	}
	if (Input.GetMouseButton(0)) {
		if (!rounded) {
			selectionLine.MakeRect (originalPos, Input.mousePosition);
		}
		else {
			selectionLine.MakeRoundedRect (originalPos, Input.mousePosition, roundedRadius, 5);
		}
		selectionLine.Draw();
	}
	selectionLine.textureOffset = -Time.time*2.0 % 1;
}