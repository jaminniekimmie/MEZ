using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection
{
    Front,
    Right,
    Back,
    Left
}

public class GameManager : MonoBehaviour {

    public FacingDirection facingDirection;
    public Transform level;
    public Transform building;
    public GameObject player;
    public GameObject invisiCube;
    public float worldUnits = 1.0f;

    private List<Transform> invisiList = new List<Transform>();
    private FacingDirection lastFacing;
    private PlayerMovement playerMove;
    private float lastDepth = 0f;
    private float degree = 0;


	// Use this for initialization
	void Start () {
        facingDirection = FacingDirection.Front;
        playerMove = player.GetComponent<PlayerMovement>();
        UpdateLevelData(true);
	}
	
	// Update is called once per frame
	void Update () {

        if (!playerMove.isJumping)
        {
            bool updateData = false;

            if (OnInvisiblePlatform())
                if (MovePlayerDepthToClosestPlatform())
                    updateData = true;

            if (MoveToClosestPlatformToCamera())
                updateData = true;

            if (updateData)
                UpdateLevelData(false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if(OnInvisiblePlatform())
                MovePlayerDepthToClosestPlatform();

            lastFacing = facingDirection;
            facingDirection = RotateDirectionRight();
            degree -= 90f;
            UpdateLevelData(false);
            playerMove.UpdateToFacingDirection(facingDirection, degree);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (OnInvisiblePlatform())
                MovePlayerDepthToClosestPlatform();

            lastFacing = facingDirection;
            facingDirection = RotateDirectionLeft();
            degree += 90f;
            UpdateLevelData(false);
            playerMove.UpdateToFacingDirection(facingDirection, degree);
        }
    }
    //Destroy current platforms and create new ones referencing player's facing direction and depth
    private void UpdateLevelData(bool forceRebuild)
    {
        //We don't need to rebuild when facing direction and depth remains the same.
        if (!forceRebuild)
            if (lastFacing == facingDirection && 
                lastDepth == GetPlayerDepth())
                return;
        //Move old invisicubes away and eliminate
        foreach (Transform tr in invisiList)
        {
            tr.position = Vector3.zero;
            Destroy(tr.gameObject);
        }

        invisiList.Clear();

        //Create new ones
        float newDepth = GetPlayerDepth();

        CreateInvisiCubesAtNewDepth(newDepth);
    }
    //Returns true if player is standing on an invisicube
    private bool OnInvisiblePlatform()
    {
        foreach(Transform item in invisiList)
        {
            if (Mathf.Abs(item.position.x - playerMove.transform.position.x) < worldUnits &&
                Mathf.Abs(item.position.z - playerMove.transform.position.z) < worldUnits)
            {
                if (playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f && 
                    playerMove.transform.position.y - item.position.y > 0)
                {
                    return true;
                }
            }
        }

        return false;
    }
    //Moves player to the closest platform with the same height when facing the camera
    private bool MoveToClosestPlatformToCamera()
    {
        bool moveCloser = false;
        
        foreach(Transform item in level)
        {
            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            {
                if (Mathf.Abs(item.position.x - playerMove.transform.position.x) < worldUnits + 0.1f)
                {
                    if (playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f &&
                        playerMove.transform.position.y - item.position.y > 0 && !playerMove.isJumping)
                    {
                        if ((facingDirection == FacingDirection.Front && item.position.z < playerMove.transform.position.z) ||
                            (facingDirection == FacingDirection.Back && item.position.z > playerMove.transform.position.z))
                            moveCloser = true;

                        if (moveCloser)
                        {
                            playerMove.transform.position = new Vector3(playerMove.transform.position.x, playerMove.transform.position.y, item.position.z);
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (Mathf.Abs (item.position.z - playerMove.transform.position.z) < worldUnits + 0.1f)
                {
                    if (playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f &&
                        playerMove.transform.position.y - item.position.y > 0 && !playerMove.isJumping)
                    {
                        if ((facingDirection == FacingDirection.Right && item.position.z < playerMove.transform.position.z) ||
                            (facingDirection == FacingDirection.Left && item.position.z > playerMove.transform.position.z))
                            moveCloser = true;
                    }
                }
            }
        }

        return true;
    }

    private bool FindTransformInvisiList(Vector3 cube)
    {
        
        return true;
    }

    private bool FindTransformLevel(Vector3 cube)
    {

        return true;
    }

    private bool FindTransformBuilding(Vector3 cube)
    {

        return true;
    }
    //move player to closest platform with the same height when player jumps onto an invisicube
    private bool MovePlayerDepthToClosestPlatform()
    {
        foreach (Transform item in level)
        {
            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            {
                if (Mathf.Abs(item.position.x - playerMove.transform.position.x) < worldUnits + 0.1f)
                {
                    if (playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f && 
                        playerMove.transform.position.y - item.position.y > 0)
                    {
                        playerMove.transform.position = new Vector3(playerMove.transform.position.x, playerMove.transform.position.y, item.position.z);
                        return true;
                    }
                }
            }
            else
            {
                if (Mathf.Abs(item.position.z - playerMove.transform.position.z) < worldUnits + 0.1f)
                {
                    if (playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f && 
                        playerMove.transform.position.y - item.position.y > 0)
                    {
                        playerMove.transform.position = new Vector3(item.position.x, playerMove.transform.position.y, playerMove.transform.position.z);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private Transform CreateInvisiCube(Vector3 position)
    {
        GameObject go = Instantiate(invisiCube) as GameObject;

        go.transform.position = position;

        return go.transform;
    }
    //creates invisible cubes for the player to move on
    private void CreateInvisiCubesAtNewDepth(float newDepth)
    {
        Vector3 tempCube = Vector3.zero;
        foreach (Transform child in level)
        {
            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            {
                tempCube = new Vector3(child.position.x, child.position.y, newDepth);
                if (!FindTransformInvisiList(tempCube) && !FindTransformLevel(tempCube) && !FindTransformBuilding(child.position))
                {
                    Transform go = CreateInvisiCube(tempCube);
                    invisiList.Add(go);
                }
            }

            else if (facingDirection == FacingDirection.Right || facingDirection == FacingDirection.Left)
            {
                tempCube = new Vector3(newDepth, child.position.y, child.position.z);
                if (!FindTransformInvisiList(tempCube) && !FindTransformLevel(tempCube) && !FindTransformBuilding(child.position))
                {
                    Transform go = CreateInvisiCube(tempCube);
                    invisiList.Add(go);
                }
            }
        }
    }

    public void ReturnToStart()
    {

    }

    private float GetPlayerDepth()
    {
        float ClosestPoint = 0f;

        if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            ClosestPoint = playerMove.transform.position.z;
        else if (facingDirection == FacingDirection.Right || facingDirection == FacingDirection.Left)
            ClosestPoint = playerMove.transform.position.x;

        return Mathf.Round(ClosestPoint);
    }

    private FacingDirection RotateDirectionRight()
    {
        int change = (int)(facingDirection);

        change++;

        if (change > 3) change = 0;

        return (FacingDirection)(change);
    }

    private FacingDirection RotateDirectionLeft()
    {
        int change = (int)(facingDirection);

        change--;

        if (change < 0) change = 3;

        return (FacingDirection)(change);
    }
}
