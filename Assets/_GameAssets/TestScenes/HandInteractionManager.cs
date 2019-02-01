// ==========================================================
// HandInteractionManager.cs
//
// References:
//   https://qiita.com/arcsin16/items/daa4886de736c994fa9a
// ==========================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.Unity.InputModule;

[RequireComponent(typeof(LineRenderer))]
public class HandInteractionManager : MonoBehaviour, ISourcePositionHandler, ISourceStateHandler
{
    List<GameObject> objects;
    LineRenderer lineRenderer;

    private Dictionary<uint, HandState> handStateMap;
    private class HandState
    {
        public Vector3 Position { get; set; }
    }

    private HandState GetHandState(uint sourceId)
    {
        if (handStateMap.ContainsKey(sourceId))
        {
            return handStateMap[sourceId];
        }
        else
        {
            var handState = new HandState();
            handStateMap.Add(sourceId, handState);
            return handState;
        }
    }

    private void OnEnable()
    {
        // FallbackInputHandlerを登録
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    private void OnDisable()
    {
        // 不要になったら解放。アプリ終了時に例外が出る事があるのでnullチェック
        if (InputManager.Instance)
        {
            InputManager.Instance.PopFallbackInputHandler();
        }
    }

    void Start()
    {
        objects = new List<GameObject>();
        handStateMap = new Dictionary<uint, HandState>();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
    }

    void Update()
    {
        // Debug.Log("Num sources: " + InteractionManager.numSourceStates);
        Debug.Log("HandStateMap.Count: " + handStateMap.Count);

        int count = 0;
        foreach (HandState hand in handStateMap.Values)
        {
            lineRenderer.SetPosition(count, hand.Position);
            count++;
        }
    }

    public void OnSourceDetected(SourceStateEventData eventData)
    {
        var handState = new HandState();
        handStateMap.Add(eventData.SourceId, handState);
    }

    public void OnPositionChanged(SourcePositionEventData eventData)
    {
        // PointerPositionとGripPositionがあるけど、手の位置はGripの方
        var handState = GetHandState(eventData.SourceId);
        handState.Position = eventData.GripPosition;
    }

    public void OnSourceLost(SourceStateEventData eventData)
    {
        Debug.Log("Removed");
        // 手の認識ロスト時に解放する。
        // 再度同じ手を認識しても同じIDが降られるとは限らないので、
        // 手の情報は認識時～ロスト時までのライフサイクルとしておく。
        if (handStateMap.ContainsKey(eventData.SourceId))
        {
            handStateMap.Remove(eventData.SourceId);
        }
    }
}
