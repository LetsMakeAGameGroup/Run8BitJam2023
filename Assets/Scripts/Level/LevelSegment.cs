using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelSegmentData 
{
    public float levelSegmentXExtend;
    public float levelSegmentYExtend;
}

public class LevelSegment : MonoBehaviour
{
    LevelManager levelManager;
    public Transform levelSegmentPrefab;

    bool isProcedural;
    public Color segmentGizmosColor = new Color(1, 1, 1, 1);
    [SerializeField] LevelSegmentData levelSegmentData;

    public void InitializeSegment(LevelManager manager)
    {
        levelManager = manager;
    }

    public void Start()
    {
        SetUpLevelSegment();
    }

    public void InitializeSegment(LevelManager manager, LevelSegmentData spawnSegmentData)
    {
        levelManager = manager;
        levelSegmentData = spawnSegmentData;
    }

    void SetUpLevelSegment()
    {
        int randomPreset = Random.Range(0, ObjectPooler.Instance.objects.Count);

        GameObject preset = ObjectPooler.Instance.GetObjectFromPool(randomPreset);
        preset.transform.SetParent(transform);
        preset.transform.position = transform.position;

    }

    public void SetLevelSegmentLocation(Vector2 newPosition)
    {
        transform.position = newPosition;
    }

    public void ResetLevelSegment()
    {
        //We restart the Level Segment & we Set it up again
        //Restart Code goes here

        //We then set it up 
        SetUpLevelSegment();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = segmentGizmosColor;
        Gizmos.DrawWireCube(transform.position, new Vector3(levelSegmentData.levelSegmentXExtend, levelSegmentData.levelSegmentYExtend, 1));
    }

}
