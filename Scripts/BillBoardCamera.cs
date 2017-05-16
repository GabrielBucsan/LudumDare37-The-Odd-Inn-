using UnityEngine;
using System.Collections;

public class BillBoardCamera : MonoBehaviour {

	//Camera variable
	private Camera m_Camera;

	//Start Method
	void Start()
	{
		m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	//Update Method
	void Update()
	{
		transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
	}
}
