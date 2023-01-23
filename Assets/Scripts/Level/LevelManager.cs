using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] List<LevelSegment> segments = new List<LevelSegment>();

    //We need to track the player and camera position to do the segment switching
    public Transform player;
    public Transform playerCamera;


    public int segmentSwapIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            LevelSegment segment = transform.GetChild(i).GetComponent<LevelSegment>();

            if (segment)
            {
                segment.InitializeSegment(this);
                segments.Add(segment);
            }
        }
    }

    private void Update()
    {
        if (player.position.x >= 45)
        {
            Vector2 PlayerLocalPositionInSegment = Vector2.zero;
            Vector3 CameraLocalPositionInSegment = Vector3.zero;

            if (segmentSwapIndex == 0)
            {
                //Get the player and camera position based on a segment
                PlayerLocalPositionInSegment = segments[2].transform.InverseTransformPoint(player.position);
                CameraLocalPositionInSegment = segments[2].transform.InverseTransformPoint(playerCamera.position);

                segments[2].SetLevelSegmentLocation(new Vector3(0, 0, 0));
                segments[3].SetLevelSegmentLocation(new Vector3(18, 0, 0));

                //move these up front and reset
                segments[0].SetLevelSegmentLocation(new Vector3(36, 0, 0));
                segments[0].ResetLevelSegment();
                segments[1].SetLevelSegmentLocation(new Vector3(54, 0, 0));
                segments[1].ResetLevelSegment();

                segmentSwapIndex = 1;
            }
            else if (segmentSwapIndex == 1)
            {
                //Get the player and camera position based on a segment
                PlayerLocalPositionInSegment = segments[0].transform.InverseTransformPoint(player.position);
                CameraLocalPositionInSegment = segments[0].transform.InverseTransformPoint(playerCamera.position);

                segments[0].SetLevelSegmentLocation(new Vector3(0, 0, 0));
                segments[1].SetLevelSegmentLocation(new Vector3(18, 0, 0));

                //move these up front and reset
                segments[2].SetLevelSegmentLocation(new Vector3(36, 0, 0));
                segments[2].ResetLevelSegment();
                segments[3].SetLevelSegmentLocation(new Vector3(54, 0, 0));
                segments[3].ResetLevelSegment();

                segmentSwapIndex = 0;
            }

            //Move player and camera to the calculated position
            player.position = PlayerLocalPositionInSegment;

            CameraLocalPositionInSegment.z = -10;
            playerCamera.position = CameraLocalPositionInSegment;
        }
    }

    public void ResetAllSegments()
    {
        foreach (LevelSegment levelSegment in segments)
        {
            levelSegment.ResetLevelSegment();
        }
    }
}
