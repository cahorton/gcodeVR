using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/**
 * 
 * VERSION 0.1
 * 
    G0 : Rapid linear Move
    G1 : Linear Move 

Usage
    G0 Xnnn Ynnn Znnn Ennn Fnnn Snnn 
Parameters
    Not all parameters need to be used, but at least one has to be used 
    Xnnn The position to move to on the X axis 
    Ynnn The position to move to on the Y axis 
    Znnn The position to move to on the Z axis 
    Ennn The amount to extrude between the starting point and ending point 
    Fnnn The feedrate per minute of the move between the starting point and ending point (if supplied) 
    Snnn Flag to check if an endstop was hit (S1 to check, S0 to ignore, S2 see note, default is S0)1 

**/

public class GCode {
	
	private char prefix;
	private string code;		// note requires int, though some codes are float
	//private List<string> gparams;
	private Dictionary<string, string> gparams;

	
	private const string gmatch = @"^([GM]\d{1,3})";		// looks for G??? or M???
	private string parmmatch = @"\b([XYZEFS][+-]*\d*\.*\d*)";	
	

	public static bool isGcode(string line){
		Regex Gregex = new Regex(gmatch);
		Match match = Gregex.Match(line);
		if (match.Success) {
			return true;
		}
		return false;

	}

	public GCode(string line){
		parseString(line);
	}
	
	public void parseString(string s){
		Regex Gregex = new Regex(gmatch);
		Regex Pregex = new Regex (parmmatch);

		String tmp;

		Match match = Gregex.Match(s);
		if (match.Success)	{
			// get G or M
			prefix = match.Value[0];
			// get the number value
			code = match.Value.Substring(1);
			
			gparams = new Dictionary<string, string>();

			foreach (Match pmatch in Pregex.Matches(s))
			{
				tmp = pmatch.Value[0].ToString();
				if (! gparams.ContainsKey(tmp)) {
					gparams.Add(tmp, pmatch.Value.Substring(1));
				}
				
			}
		}
		
	}
	
	public override string ToString(){
		string s = prefix.ToString() + code.ToString();
		if (gparams.Count > 0){
			foreach( KeyValuePair<string, string> kvp in gparams )
			{
				s = s+ " "+ kvp.Key + kvp.Value;
			}
		}
		return s;
	}

	public bool getf(string p, out float f){
		// check we have that key
		if (gparams.ContainsKey (p)) {
			// we do, so get the value
			string v = gparams[p];
			
			// attempt to convert to float
			if (Single.TryParse(v, out f)) {
				return true;
			}
		}
		// either couldn't convert to float, or no such key, so return false
		f = 0;
		return false;
	}

	public bool isMove(){
		return (prefix == 'G' && (code == "0" || code == "1"));
	}

	public bool extrudes(){
		// NOTE: NEED TO CHCK IF E VAL > 0
		return (containsKey ("E"));
	}
	
	public bool containsKey(string p){
		if (gparams.ContainsKey (p)) return true;
		return false;
	}
	
}