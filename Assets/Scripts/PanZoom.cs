using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour {
    public static Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    private bool multiTouch = false;

    public float minX, minY, maxX, maxY;
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0)){
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            multiTouch = false;
        }

        if(Input.touchCount == 2 && (!QuestionPanel.questionPanelState && !GameManager.endPanelState)){
            multiTouch = true;
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * -0.25f);
        } else if (Input.GetMouseButton(0) && multiTouch == false && (!QuestionPanel.questionPanelState && !GameManager.endPanelState)) 
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
            Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, minX, maxX), Mathf.Clamp(Camera.main.transform.position.y, minY, maxY), -10);
        } else if (Input.GetMouseButton(0) && (QuestionPanel.questionPanelState || GameManager.endPanelState))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Mathf.Clamp(Camera.main.transform.position.y + (touchStart.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y), minY + 300f, maxY - 300f), -10);
        }

        if (!QuestionPanel.questionPanelState && !GameManager.endPanelState)
        {
            zoom(Input.GetAxis("Mouse ScrollWheel") * 500);
        }
	}

    void zoom(float increment){
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
}
