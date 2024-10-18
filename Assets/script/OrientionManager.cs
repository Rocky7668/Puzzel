using UnityEngine;

public class OrientionManager : MonoBehaviour
{    
    private void OnEnable()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
    private void OnDisable()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
   
}
