using UnityEngine;

public class TimeLogic : MonoBehaviour
{

    public static TimeLogic _Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (_Instance == null || _Instance == this)
        {
            _Instance = this;
            return;
        }
        Destroy(this);
    }

  
 
}
