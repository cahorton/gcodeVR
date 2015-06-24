using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GCodeHolder {

	// An GCodeHolder object is a List of Levels
	List<List<Segment>> levels;

	// A Level is a list of segments

	// A Segment is a start and end point, plus extrude bool, and is its own class

	public GCodeHolder(){
		levels = new List<List<Segment>> ();
	}

	public List<Segment> addLevel(){
		List<Segment> _level = List<Segment>();
		levels.Add (_level);
		return _level;
	}

}



