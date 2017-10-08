/* Group Project Camera Control Codes
 * by RJ Ortega 
 * 
 * Important: Do not attach this to the Camera object, but instead to a blank object that holds the camera object as its child
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	public float m_DampTime = 0.2f;                             //approximate time for the camera to move to new location
	public float m_ScreenEdgeBufer = 4f;                        //added to sides to ensure players don't go off camera
	public float m_MinSize = 6.5f;                              //stops camera from zooming in

	private PlayerController[] m_Targets = new PlayerController[4];           //array of gaming objects that would be the targets for camera to adjust to
	private Camera m_Camera;                                    //reference to the camera attached as child
	private float m_ZoomSpeed;                                  //damps zooming, slows it down to make it less jarring
	private Vector3 m_MoveVelocity;                             //damps camera movement and panning to avoid jarring camera movements
	private Vector3 m_DesiredPosition;                          //position that camera is trying to reach

	[SerializeField]
	private GameObject m_gmObject;
	private GameManager m_gm;

	[SerializeField]
	bool debug = false;

	// Use this for initialization
	private void Awake ()
	{
		m_Camera = GetComponentInChildren<Camera>();	        //references camera and gets values

		m_gm = m_gmObject.GetComponent<GameManager>();
	}

	//this updates every runtime
	private void FixedUpdate()                                  
	{
		for (int i = 0; i < 4; i++)
		{
			if (m_gm.players[i] != null)
			{
				m_Targets[i] = m_gm.players[i].GetComponentInChildren<PlayerController>();
			}
			else
			{
				m_Targets[i] = null;
			}
		}

		Move();                                                //calls move function to move camera
		//Zoom();                                                //calls Zoom function to Zoom Camera
	}

	private void Move()
	{
		FindAveragePosition();                                //Function that tries to find all playerobjects target position

		m_DesiredPosition = FindAveragePosition(); 
		m_DesiredPosition.y = transform.position.y;


		// Smoothly move to the desired position
		transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
	}

	private float FindDistance()
	{
		float minFrustumHeight = 0;
		float minFrustumWidth = 0;

		Plane frustrumPlane = new Plane(FindAveragePosition() - m_Camera.transform.position, FindAveragePosition());

		// Calculate minimum frustum height


		// Calculate minimum frustum width



		float frustumHeight = 0;
		float frustumWidth = 0;

		//frustumWidth = frustumHeight * m_Camera.aspect;
		//frustumHeight = frustumWidth / m_Camera.aspect;


		return 0;
	}

	private void DrawPlane(Vector3 position, Vector3 normal)
	{

		Vector3 v3;

		if (normal.normalized != Vector3.forward)
			v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
		else
			v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

		var corner0 = position + v3;
		var corner2 = position - v3;
		var q = Quaternion.AngleAxis(90.0f, normal);
		v3 = q * v3;
		var corner1 = position + v3;
		var corner3 = position - v3;

		Gizmos.DrawLine(corner0, corner2);
		Gizmos.DrawLine(corner1, corner3);
		Gizmos.DrawLine(corner0, corner1);
		Gizmos.DrawLine(corner1, corner2);
		Gizmos.DrawLine(corner2, corner3);
		Gizmos.DrawLine(corner3, corner0);
		Gizmos.DrawRay(position, normal);
	}

	private Vector3 FindAveragePosition()
	{
		Vector3 averagePos = new Vector3();
		int numTargets = 0;

		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (m_Targets[i] == null || !m_Targets[i].isAlive) // If the player doesn't exist or isn't alive don't follow its position
				continue;

			averagePos += m_Targets[i].gameObject.transform.position;
			numTargets++;
		}

		if (numTargets > 0)
			averagePos /= numTargets;

		return averagePos;
	}

	private void Zoom()
	{
		float requiredSize = FindRequiredSize();
		m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
	}

	private float FindRequiredSize()
	{
		Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

		float size = 0f;

		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (m_Targets[i] == null || !m_Targets[i].isAlive) // If the player doesn't exist or isn't alive don't follow its position
				continue;

			Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].gameObject.transform.position);

			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
		}

		size += m_ScreenEdgeBufer;

		size = Mathf.Max(size, m_MinSize);

		return size;
	}

	void OnDrawGizmos()
	{
		if (Application.isPlaying && debug)
		{
			Vector3 avgPosition = FindAveragePosition();
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(avgPosition, 1);

			Gizmos.color = Color.green;
			DrawPlane(FindAveragePosition(), FindAveragePosition() - m_Camera.transform.position);

			Gizmos.color = Color.yellow;
			Matrix4x4 mat = new Matrix4x4();
			mat.SetTRS(m_Camera.transform.position, m_Camera.transform.rotation, m_Camera.transform.localScale);
			Gizmos.matrix = mat;
			Gizmos.DrawFrustum(Vector3.zero, m_Camera.fieldOfView, m_Camera.farClipPlane, m_Camera.nearClipPlane, m_Camera.aspect);
		}
	}

	public void SetStartPositionAndSize()
	{
		FindAveragePosition();

		transform.position = m_DesiredPosition;

		m_Camera.orthographicSize = FindRequiredSize();
	}

}
