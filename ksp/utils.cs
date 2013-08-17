using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP.IO;

namespace MissionControl  {


	public class MCUtils 
	{
		public string getStateLine(Vessel vessel) {
			Orbit orbit = vessel.GetOrbit ();
			//string referenceBody = FlightGlobals.Bodies.IndexOf (orbit.referenceBody).ToString ();
			string referenceBody = orbit.referenceBody.GetName ();

			List<string> buffer = new List<string>();
			buffer.Add ("V"); // # 0

			if (vessel.situation == Vessel.Situations.LANDED) {
				buffer.Add ("L");
			} 
			else if (vessel.situation == Vessel.Situations.SPLASHED) {
				buffer.Add ("S");
			} 
			else if (vessel.situation == Vessel.Situations.PRELAUNCH) {
				buffer.Add ("P");
			} 
			else if (vessel.situation == Vessel.Situations.FLYING) {
				buffer.Add ("F");
			}
			else if (vessel.situation == Vessel.Situations.ORBITING) {
				buffer.Add ("O");
			} 
			else if (vessel.situation == Vessel.Situations.DOCKED) {
				buffer.Add ("D");
			} 
			else if (vessel.situation == Vessel.Situations.SUB_ORBITAL) {
				buffer.Add ("SO");
			} 
			else if (vessel.situation == Vessel.Situations.ESCAPING) {
				buffer.Add ("E");
			} 

			else {
				Debug.Log ("Unknown vessel situation");
				return "X";
			} // # 1

			buffer.Add (vessel.id.ToString ()); // # 2
			buffer.Add (vessel.vesselName); // # 3
			buffer.Add (Planetarium.GetUniversalTime ().ToString ()); // # 4
			buffer.Add (referenceBody); // # 5

			buffer.Add (vessel.longitude.ToString ()); // # 6
			buffer.Add (vessel.latitude.ToString ()); // # 7
			// Why was this zero?
			Vector3d r = orbit.getRelativePositionAtUT (Planetarium.GetUniversalTime ());
			Vector3d v = orbit.getOrbitalVelocityAtUT (Planetarium.GetUniversalTime ());

			//Vector3d r = orbit.pos.xzy;
			//Vector3d v = orbit.vel.xzy;

			String rx = r.x.ToString (); // X in pygame?
			String ry = r.y.ToString (); // Z in pygame?
			String rz = r.z.ToString (); // Y in pygame?

			String vx = v.x.ToString ();
			String vy = v.y.ToString ();
			String vz = v.z.ToString ();

			buffer.Add (rx + ":" + ry + ":" + rz + ":" + vx + ":" + vy + ":" + vz);	// # 8

			//Debug.Log ("SPEED1: " + orbit.getOrbitalVelocityAtUT (Planetarium.GetUniversalTime ()).ToString ()); // Tis is currently used velocity
			//Debug.Log ("SPEED2: " + orbit.GetFrameVelAtUT (Planetarium.GetUniversalTime ()).ToString ());
			//Debug.Log ("SPEED3: " + orbit.GetRelativeVel ().ToString () );
			// GetRelativeVel seems to be OK to get the correct position

			//Debug.Log ("SPEED4: " + orbit.GetRotFrameVel ( ().ToString () );


			//Debug.Log ("POSIT1: " + orbit.getPositionAtUT(Planetarium.GetUniversalTime ()).ToString ());
			//Debug.Log ("POSIT2: " + orbit.getRelativePositionAtUT(Planetarium.GetUniversalTime ()).ToString ()); // Tis is currently used pos
			//Debug.Log ("POSIT3: " + orbit.getTruePositionAtUT (Planetarium.GetUniversalTime ()).ToString ());
			//Debug.Log ("POSIT4: " + orbit.pos.xzy.ToString ());

			//Debug.Log ("FRAMEROT: " + Planetarium.FrameIsRotating ().ToString ());
			//Debug.Log ("FRAMEROT: " + Planetarium.ZupRotation.ToString ());
			//Debug.Log ("FRAMEROT: " + Planetarium.Rotation.ToString ());

			//Debug.Log ("Framerot: " + Planetarium.InverseRotAngle.ToString ());



			//buffer.Add (orbit.epoch.ToString ());
			buffer.Add (orbit.semiMajorAxis.ToString () + ":" +
				orbit.eccentricity.ToString () + ":" +
				orbit.inclination.ToString () + ":" +
				orbit.LAN.ToString () + ":" +
				orbit.argumentOfPeriapsis.ToString ()); // # 9
			//buffer.Add (orbit.meanAnomalyAtEpoch.ToString ());


			buffer.AddRange(getFlightData (vessel));

	
			return string.Join ("\t",buffer.ToArray ());
			
		}

		public List<string> getFlightData (Vessel vessel)
		{
			List<string> buffer = new List<string>();



			buffer.Add (vessel.missionTime.ToString ()); // # 10
			buffer.Add (vessel.atmDensity.ToString ()); // # 11
			buffer.Add (vessel.geeForce.ToString ()); // # 12
			buffer.Add (vessel.obt_velocity.magnitude.ToString ()); // # 13
			buffer.Add (vessel.srf_velocity.magnitude.ToString ()); // # 14
			buffer.Add (vessel.verticalSpeed.ToString ());   // # 15
			buffer.Add (vessel.staticPressure.ToString ()); // # 16

			if (vessel.Parts.Count () > 0) {
				Part part = vessel.Parts [0];
				buffer.Add (part.dynamicPressureAtm.ToString ()); // # 17
				buffer.Add (part.temperature.ToString ()); // #  18
			} else {
				buffer.Add ("0");
				buffer.Add ("0");
			}

			buffer.Add (vessel.altitude.ToString ()); // # 19
			buffer.Add (vessel.heightFromSurface.ToString ()); // # 20
			buffer.Add (vessel.heightFromTerrain.ToString ()); // # 21

			//buffer.Add (vessel.acceleration.magnitude.ToString ());
			//buffer.Add (vessel.angularMomentum.magnitude.ToString ());
			//buffer.Add (vessel.angularVelocity.magnitude.ToString ());
			//buffer.Add (vessel.geeForce_immediate.ToString ());
			//buffer.Add (vessel.horizontalSrfSpeed.ToString ());
			//buffer.Add (vessel.pqsAltitude.ToString ());
			//buffer.Add (vessel.rb_velocity.magnitude.ToString ());
			//buffer.Add (vessel.specificAcceleration.ToString ());
			//buffer.Add (vessel.terrainAltitude.ToString ());

			return buffer;
		}
		public string getCelestialState(CelestialBody celestial) 
		{
			Debug.Log ("Collecting: " + celestial.GetName ());
			List<string> buffer = new List<string>();
			buffer.Add ("C");
			buffer.Add (celestial.GetName ());

			if (celestial.orbitDriver != null) {
				Orbit orbit = celestial.GetOrbit ();

				Vector3d r = orbit.getRelativePositionAtUT (0);
				Vector3d v = orbit.getOrbitalVelocityAtUT (0);

				String rx = r.x.ToString ();
				String ry = r.y.ToString ();
				String rz = r.z.ToString ();

				String vx = v.x.ToString ();
				String vy = v.y.ToString ();
				String vz = v.z.ToString ();
;
				buffer.Add (celestial.referenceBody.GetName ());
				buffer.Add (rx + ":" + ry + ":" + rz + ":" + vx + ":" + vy + ":" + vz);
			} else {
				buffer.Add ("None");
				buffer.Add ("None");
			}
	
			buffer.Add (celestial.gravParameter.ToString ());
			buffer.Add (celestial.Radius.ToString ());
			buffer.Add (celestial.sphereOfInfluence.ToString ());
			if (celestial.atmosphere == true) {
				buffer.Add (celestial.maxAtmosphereAltitude.ToString ());
			} else {
				buffer.Add ("None");
			}

			// Angular velocity data
			buffer.Add (celestial.zUpAngularVelocity.magnitude.ToString ());

			buffer.Add (celestial.initialRotation.ToString ());
			buffer.Add (celestial.rotationAngle.ToString ());
			return string.Join ("\t",buffer.ToArray ());
		}
	}
}