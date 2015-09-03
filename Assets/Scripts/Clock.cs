using UnityEngine;
using System.Collections;


public class Clock : MonoBehaviour {

	
	public float counter;
	public float speed;

	[Space(20)]
	public float daysPerYear = 365.242199f;
	public int monthsPerYear = 12;
	public float daysPerMonth;
	public int hoursPerDay = 24;

	[Space(20)]
	//variables for clock
	public int hour;
	public int minute;
	public int day;
	public int month;
	public int year;
	float fullHour;
	int daysTotal;

	void Start(){
		daysPerMonth = daysPerYear / monthsPerYear;
	}
	
	void Update(){
		counter += speed;
		CounterToTime();	
	}
	
		
	void CounterToTime(){
		fullHour = counter % hoursPerDay;
		hour = (int)fullHour;
		//scale only the decimal part to minutes
		minute = (int)(((fullHour - (int)fullHour) / 1) * 60);
		

		daysTotal = (int)(counter / hoursPerDay);
		day = (int)(daysTotal % daysPerMonth);
		
		month = ((int)(daysTotal / daysPerMonth)) % monthsPerYear;
		year = (int)(daysTotal / daysPerYear);
	}
}