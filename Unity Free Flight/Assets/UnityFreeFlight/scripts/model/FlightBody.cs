﻿using UnityEngine;
using System.Collections;



//Units unit = UnitConverter.Units.Metric;


public class FlightBody {
	
	//NOTE NOTE NOTE!!!!
	//We will always store things in METRIC
	//We convert, if we ever give things back in something else
	//NOTE NOTE NOTE!!!


//	public enum Units{ Metric, Emperial };
	private UnitConverter conv;
	public UnitConverter.Units unit {
		get {return conv.unit;}
		set {conv.unit = value;}
	}

	public enum Presets { Custom, TurkeyVulture, Albatross};
	private Presets _preset;

	//FLYING BODY SPECIFICATIONS
	private float _wingChord; //in meters
	private float _wingSpan;  //in meters
	private float _wingArea; // span * chord
	private float _aspectRatio; //span / chord
	private float _weight;	// in kilograms
	//End flying body statistics

	public FlightBody () {
		conv = new UnitConverter (UnitConverter.Units.Metric);
		Preset = Presets.TurkeyVulture;
	}


//Doesn't bloody work, because the editor likes to call the setter every bloody second.
	public Presets Preset {
		get {return _preset; }
		set {
			_preset = value;
			switch (_preset) {
				case Presets.TurkeyVulture:
					//Turkey Vulture stats. We should eventually load this data from disk.
					_wingSpan = 1.715f;
					_wingChord = .7f;
					_weight = 1.55f;
					setFromWingDimensions();
					break;
				case Presets.Albatross:
					_wingSpan = 3.5f;
					_wingChord = 0.21875f;
					_weight = 11.0f;
					setFromWingDimensions();
					// also a lift to drag (L/D) of 25
					break;

			}
		}
	}

	public float WingSpan {
		get { 
			return conv.getLength (_wingSpan); 
		}
		set {
			_wingSpan = conv.setLength (value);
		}
	}

	public float WingChord {
		get { 
			return conv.getLength (_wingChord); 
		}
		set {
			_wingChord = conv.setLength (value);
		}
	}

	public float WingArea {
		get{
			return conv.getArea (_wingArea );
		} 
		set{
			_wingArea = conv.setArea (value);
		}
		
	}
	
	public float AspectRatio {
		//Dimensionless number! Yay, no converting! (wingspan / wingchord)
		get { return _aspectRatio; } 
		set { _aspectRatio = value;}
	}
	
	public float Weight { 
		get{ 
			return conv.getWeight(_weight);
		} 
		set{ 
			_weight = conv.setWeight (value);
		}
	}

	public void setFromWingDimensions() {
		if (_wingChord > 0 && _wingSpan > 0) {
				_wingArea = _wingChord * _wingSpan;
				_aspectRatio = _wingSpan / _wingChord;
		} else {
			throw new UnityException("Wing Span and Wing Chord must be greator than zero");
		}
	}

	public void setWingDimensions() {
		if (_aspectRatio > 0 && _wingArea > 0) {
			_wingSpan = Mathf.Sqrt (_wingArea * _aspectRatio);
			_wingChord = Mathf.Sqrt (_wingArea / _aspectRatio);
		} else {
			throw new UnityException("Aspect Ratio and Wing Area must be greator than zero");
		}

	}

	public bool isValid(bool log = false) {
		if(_wingSpan * _wingChord == _wingArea && _wingSpan / _wingChord == _aspectRatio) {
			return true;
		} else {
			if (log == true) {
				Debug.LogWarning(string.Format("*FlightBody* has invalid wing dimensions. You can fix these via the Flight Body Editor in the inspector"));
			}	
			return false;
		}
	}


}