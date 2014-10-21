using UnityEngine;
using UnityEditor;
using System.Threading;
using System.Collections;
 
public class Take3DScreenshot : ScriptableWizard {
    public static string fileName = "Unity 3D Screenshot _";
    public static string folder = "/Users/Casemon/Desktop/Screens/";
    public GameObject useCamera;
    public GameObject rotateAround;
    public int everyXdegrees = 30;
	public int captureDelayMs = 500;
 
    [MenuItem ("Custom/Take 3D Screenshot of Game View")]
 
    static void DoSet () {
        ScriptableWizard.DisplayWizard("Set params for 3D Screenshot", typeof(Take3DScreenshot), "Create");
    }
 
    void OnWizardUpdate () {
        helpString = "Set the parameters to create a series of pictures in a circle... \n\nThese settings will create: " + (360 / everyXdegrees) + " images";
 
    }
 
    void OnWizardCreate () {
        Thread thread = new Thread (TakeScreenshot);
        thread.Start ();
    }
 
    void TakeScreenshot () {
        for (int number = 0; number < (360 / everyXdegrees); number++) {
            Application.CaptureScreenshot(folder +fileName +number+"_"+number.ToString("d4") +".png");
            if (rotateAround)
                useCamera.transform.RotateAround(rotateAround.transform.position, Vector3.up, everyXdegrees);
            else
                useCamera.transform.Rotate(Vector3.up, everyXdegrees);
 			Debug.Log ("Saving " +folder +fileName +number.ToString("d4") +".png... DO NOT use the Unity Editor!!");
            Thread.Sleep (captureDelayMs);
        }
		Debug.Log ("All Done! We now return you to your previously scheduled Unity work...");
    }
}