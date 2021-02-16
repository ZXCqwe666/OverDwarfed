using Unity.Mathematics;
using UnityEngine;

public class TaskPointer : MonoBehaviour
{
    public GameObject target;
    private RectTransform pointerRectTransform;
    private Camera mainCam;
    private const float borderSize = 75f;
    private int2 ScrenSize;

    private void Start()
    {
        InitializeTaskPointer();
    }
    private void FixedUpdate()
    {
        Vector3 targetPositionScreenPoint = mainCam.WorldToScreenPoint(PlayersPositions.instance.TaskDropOffPosition);
        
        bool isOffScreen = 
            targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= ScrenSize.x - borderSize || 
            targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= ScrenSize.y - borderSize;
        
        if (isOffScreen)
        {
            Vector3 cappedTagetScreenPosition = targetPositionScreenPoint;
            if (cappedTagetScreenPosition.x <= borderSize) cappedTagetScreenPosition.x = borderSize;
            if (cappedTagetScreenPosition.x >= ScrenSize.x - borderSize) cappedTagetScreenPosition.x = ScrenSize.x - borderSize;
            if (cappedTagetScreenPosition.y <= borderSize) cappedTagetScreenPosition.y = borderSize;
            if (cappedTagetScreenPosition.y >= ScrenSize.y - borderSize) cappedTagetScreenPosition.y = ScrenSize.y - borderSize;

            Vector3 camPosition = mainCam.transform.position;
            camPosition.z = 0f;
            Vector3 direction = (PlayersPositions.instance.TaskDropOffPosition - camPosition).normalized;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360;
            pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);

            pointerRectTransform.position = mainCam.ScreenToWorldPoint(cappedTagetScreenPosition);
        }
        else
            pointerRectTransform.position = Vector3.zero;
    }
    private void InitializeTaskPointer()
    {
        pointerRectTransform = transform.Find("TaskPointer").GetComponent<RectTransform>();
        mainCam = Camera.main;
        ScrenSize = new int2(Screen.width, Screen.height);
    }
}
