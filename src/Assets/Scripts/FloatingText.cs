using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    public bool active;
    public GameObject go; // this GameObject
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;
    
    public void Show()
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active); // activate Game Object
    }

    public void Hide()
    {
        active = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText() 
    {
        if (!active)
            return;

        if(Time.time - lastShown > duration) // if duration has lasted
            Hide();

        go.transform.position += motion * Time.deltaTime; // constantly move this object each frame by the motion
    }
}
