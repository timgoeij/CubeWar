using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour 
{
	//variabels
    public Transform gate;
	Vector3 gatepos;
	public Vector3 endpos;

	public bool stopspawning = false;

	void Start () 
	{
		//gatepos de lokale positie van de gate van een kasteel
        gatepos = gate.localPosition;
	}

	void Update () 
	{
	    //maak een ray met als waardes de postie van de spawner en de forward vector van de spawner
        Ray ray = new Ray(this.transform.position, this.transform.TransformDirection(Vector3.forward));
        //maak een raycasthit hit aan
		RaycastHit hit;

        //voer een raycst uit met als waardes de ray ray en raycasthit hit
		if(Physics.Raycast(ray, out hit))
		{
			//als de afstand kleiner is dan 14.75 dat de ray iets raakt
            if(hit.distance < 14.75f)
			{
				//geef de lokale positie van de gate een nieuwe positie met een snelheid van 10
                //keer time.deltaTime
                gate.localPosition = Vector3.Lerp(gate.localPosition,endpos, 10 * Time.deltaTime);

				//als de afstand kleiner is dan 1.5 tot de ray iets raakt
                //zet stopspawning op true
                //anders zet stopspawning op false
                if(hit.distance < 1.5)
					stopspawning = true;
				else
					stopspawning = false;
			}
			else
				gate.localPosition = Vector3.Lerp(gate.localPosition,gatepos, 10 * Time.deltaTime);
            //als de afstand groter is dan 14.75 zet de lokale positie van de gate op de oude
            //plek met een snelheid van 10 * time.deltaTime
		}

	}
}
