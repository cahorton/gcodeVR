using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Vectrosity;

public class GCodeLoader : MonoBehaviour {

	List<GCode> gcodes;
	GCodeHolder foo;

	string filename = "Assets/Resources/test.gcode";
	const string GMATCH = @"([G]\d{1,3})( X[-+]?\d*[\.]?\d*)?( Y[-+]?\d*[\.]?\d*)?( Z[-+]?\d*[\.]?\d*)?( E[-+]?\d*[\.]?\d*)?( F[-+]?\d*[\.]?\d*)?";
	//const string GMATCH = @"([G]\d{1,3})( [XYZEFS][-+]?\d*[\.]?\d*)*";

	//const string GMATCH = @"(^[G]\d{1,3}) ([XYZEFS][-+]?\d*[\.]?\d*) ([XYZEFS][-+]?\d*[\.]?\d*) ([XYZEFS][-+]?\d*[\.]?\d*)*";

	// Use this for initialization
	void Start () {
	
		gcodes = new List<GCode>();
		ReadFile (filename);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ReadFile(string filename){
		int counter = 0;
		int gcounter = 0;
		string line;
		
		// Read the file and display it line by line.
		System.IO.StreamReader file = 
			new System.IO.StreamReader(filename);


		// NOTE: !!! the counter cap is temporary
		while((line = file.ReadLine()) != null && counter < 1000)
		{
			//System.Console.WriteLine (line);
			if (GCode.isGcode(line)){
				gcodes.Add (new GCode(line));
				gcounter++;		// gcode lines parsed
			}

			counter++; 		// total lines parsed

		}
		
		file.Close();
		print ("Total Gcode lines = " + gcodes.Count);
		print ("Total Lines in file = " + counter);

		//for (int i=0; i< gcodes.Count; i++) {
		//	print (gcodes [i].ToString ());
		//}
		drawLines ();

	}

	public void drawLines(){
		VectorLine myLine;
		List<Vector3> linePoints = new List<Vector3>(); // C# 

		Vector3 tmp = new Vector3(0f,0f,0f);
		Vector3 last = new Vector3(0f,0f,0f);

		myLine =  new VectorLine("MyLine", linePoints, null, 2.0f, LineType.Continuous);


		for (int i=0; i< gcodes.Count; i++) {
			GCode g = gcodes[i];
			float _pos;

			if (g.containsKey("X") && g.getf ("X", out _pos)){
				tmp.x = _pos;
			} else {
				tmp.x = last.x;
			}
			if (g.containsKey("Y") && g.getf ("Y", out _pos)){
				tmp.z = _pos;
			} else {
				tmp.z = last.z;
			}	
			if (g.containsKey("Z") && g.getf ("Z", out _pos)){
				tmp.y = _pos;
			} else {
				tmp.y = last.y;
			}

			if (g.isMove ()){
				if (g.extrudes()){
					// draw line from last to tmp
					print ("LINE "+last +" - " + tmp +"     : "+g);

				} else {
					print ("move "+last +" - " + tmp+"     : "+g);
				}
				last = tmp;

				linePoints.Add (tmp); 
			}

		}
		myLine.Draw(); 
	}

	/**
	public void ParseLine(string line){

		// find G or M codes
		//Regex regex = new Regex(@"[GM]\d{1,3}");
		Regex regex = new Regex(GMATCH);
		MatchCollection matches = regex.Matches(line);


		GCode g = new GCode ();
		int mc = 0;	

		foreach (Match match in matches){
			// matched a G code
			// get the G code type
			//g.g_value = Int32.Parse (match.Groups[1].Value.Substring(1));
			for(int i=0; i<match.Groups.Count; i++){
				print ("Group: "+i+", : "+ match.Groups[i].Value);
			}
			//print ("Count " + match.Groups.Count);
			//print ("MC: "+mc+", : "+ capture.Value);
			//}


			for (int i =2; i <= match.Groups.Count; i++){
				print (match.Groups[i].Value.ToString());
				ParseGParam(match.Groups[i].Value, g);
			}

			gcodes.Add (g);

		} 
	}
	
	public void ParseGParam(string s, GCode g){
		s.Trim ();

		//print (s.ToString());
		// get the first character
		if (s.Length > 0) {
			
			char c = s [0];
			switch (c) {
			case 'X':
				//g.x = Single.Parse (s.Substring (1));
				g.x=20f;
				break;
			case 'Y':
				g.y = Single.Parse (s.Substring (1));
				break;
			case 'Z':
				g.z = Single.Parse (s.Substring (1));
				break;
			case ' ':
				g.x = -1.0f;
				break;
			}
		}
	}
**/


}
