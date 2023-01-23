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

    public LevelSegmentData SegmentSpawnData;
    public int segmentToSpawnCount;
    public int segmentSwitchXPosition;

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
        for (int i = 0; i < segmentToSpawnCount; i++) 
        {
            GameObject newSegmentObject = new GameObject("LevelSegment " + i);
            newSegmentObject.transform.SetParent(transform);
            newSegmentObject.transform.position = new Vector3(18 * i, 0, 0);

            LevelSegment segment = newSegmentObject.AddComponent<LevelSegment>();

            segment.InitializeSegment(this, SegmentSpawnData);
            segments.Add(segment);
        }



        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    LevelSegment segment = transform.GetChild(i).GetComponent<LevelSegment>();
        //
        //    if (segment)
        //    {
        //        segment.InitializeSegment(this);
        //        segments.Add(segment);
        //    }
        //}
    }

    private void Update()
    {
        if (player.position.x >= segmentSwitchXPosition)
        {
            Vector2 PlayerLocalPositionInSegment = segments[4].transform.InverseTransformPoint(player.position);
            Vector3 CameraLocalPositionInSegment = segments[4].transform.InverseTransformPoint(playerCamera.position);

            //Grabs last two segments and swift them to the front
            segments[4].SetLevelSegmentLocation(new Vector3(0, 0, 0));
            segments[5].SetLevelSegmentLocation(new Vector3(18, 0, 0));

            //moves the rest forward
            segments[0].SetLevelSegmentLocation(new Vector3(36, 0, 0));
            segments[0].ResetLevelSegment();
            segments[1].SetLevelSegmentLocation(new Vector3(54, 0, 0));
            segments[1].ResetLevelSegment();
            segments[2].SetLevelSegmentLocation(new Vector3(72, 0, 0));
            segments[2].ResetLevelSegment();
            segments[3].SetLevelSegmentLocation(new Vector3(90, 0, 0));
            segments[3].ResetLevelSegment();

            //Reorders the list based on their X position
            segments.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

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
