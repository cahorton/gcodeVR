using UnityEngine;

public class Segment{
	
	private Vector3 start;
	private Vector3 end;
	private bool extrude;
	
	public Segment (Vector3 start, Vector3 end, bool extrude) {
		start = start;
		end = end;
		extrude = extrude;
	}
	
	public bool extrudes(){
		return extrude;
	}
}