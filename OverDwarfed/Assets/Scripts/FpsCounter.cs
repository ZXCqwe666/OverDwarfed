using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    private float timer, refresh, avgFramerate;
    private Text m_Text;
    private void Start()
    {
        Application.targetFrameRate = 240; //Screen.currentResolution.refreshRate + 60; ///MB change later
        refresh = 0.05f;
        m_Text = GetComponent<Text>();
    }
    private void Update()
    {
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if (timer <= 0) avgFramerate = (int)(1f / timelapse);
        m_Text.text = avgFramerate.ToString();
    }
}