using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Checks to see if the plane that contains the turrets is clicked
//If so a turret will spawn
public class TurretSpawn : MonoBehaviour
{
    //The turret Transform
    public Transform turret;
    public GameObject gridObject;

    private Grid grid;

    private void Start()
    {
        grid = gridObject.GetComponent<Grid>();
    }
    void Update()
    {
        //Checks for the mouse button to be pressed
        if (Input.GetMouseButtonDown(0))
        {
            //Casts a ray from camera point to the point in which the mouse is currently pointing
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Casts the ray and get the first game object hit
            Physics.Raycast(ray, out hit);
            //Debug.Log("This hit " + hit.transform.name);

            //If the ray casted hits the spawn area then a turret will spawn in the location clicked
            //Note that I update the grid and gizmos here
            //This is because the obstacles will only change in this way
            //If the game required a moving turret I would change
            //The way I do this and instead work with the gizmos to
            //update when they collide with other objects
            //and update the grid then
            if(hit.transform.tag.CompareTo("Spawn") == 0)
            {
                Transform newTurret = Instantiate(turret, hit.point + new Vector3(0, 1, 0), Quaternion.identity);
                newTurret.gameObject.SetActive(true);
                grid.CreateGrid();
                SceneView.RepaintAll();
            } else if (hit.transform.tag.CompareTo("Turret") == 0)
            {
                //If it a turret destroy the turret
                Destroy(hit.transform.gameObject);
                grid.CreateGrid();
                SceneView.RepaintAll();
            }
        }
    }
}
