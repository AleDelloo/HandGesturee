using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Gestures;
using UnityEngine.XR.Hands.Samples.GestureSample;

public class DetectGesture : MonoBehaviour
{
    [SerializeField] private XRHandTrackingEvents handTrackingEvents;
    [SerializeField] private XRHandShape[] handShapes;
    [SerializeField] private float gestureDetectionInterval = 0.1f;
    [SerializeField] private float minimumDetectionThreshold = 0.9f;
    [SerializeField] private HandShapeCompletenessCalculator handShapeCompletenessCalculator;

    private float timeOfLastConditionCheck;

    void OnEnable() => handTrackingEvents.jointsUpdated.AddListener(OnJointsUpdated);

    void OnDisable() => handTrackingEvents.jointsUpdated.RemoveListener(OnJointsUpdated);

    void OnJointsUpdated(XRHandJointsUpdatedEventArgs eventArgs)
    {
        if (Time.time - timeOfLastConditionCheck < gestureDetectionInterval)
            return;

        foreach (var handShape in handShapes)
        {
            handShapeCompletenessCalculator.TryCalculateHandShapeCompletenessScore(eventArgs.hand,
                handShape, out float completenessScore);
            var detected =
                handTrackingEvents.handIsTracked && completenessScore >= minimumDetectionThreshold;
            if (detected)
            {
                Debug.Log($"Hand Gesture Detected: {handShape.name} | Score: {completenessScore}");
            }
        }
        timeOfLastConditionCheck = Time.time;
    }
}