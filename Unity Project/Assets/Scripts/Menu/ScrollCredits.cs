using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCredits : MonoBehaviour
{
    Vector3 startPos;

    [SerializeField]
    float speed;

	void Start()
    {
        startPos = transform.position;
	}
	
	public void ScrollUpdate()
    {
        transform.position += new Vector3(transform.position.x, speed * Time.deltaTime, transform.position.z);
	}

    public void ResetPosition()
    {
        transform.position = startPos;
    }
}
