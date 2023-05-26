// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mediapipe;
using Mediapipe.Unity;

namespace Augbox
{
  public class HandTrackingSolution : ImageSourceSolution<HandTrackingGraph>
  {
    [SerializeField]public GameObject Hand1;
    [SerializeField]public GameObject Hand2;

    [SerializeField] private DetectionListAnnotationController _palmDetectionsAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _handRectsFromPalmDetectionsAnnotationController;
    [SerializeField] private MultiHandLandmarkListAnnotationController _handLandmarksAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _handRectsFromLandmarksAnnotationController;

    private List<NormalizedLandmarkList> hloutput;

    public HandTrackingGraph.ModelComplexity modelComplexity
    {
      get => graphRunner.modelComplexity;
      set => graphRunner.modelComplexity = value;
    }

    public int maxNumHands
    {
      get => graphRunner.maxNumHands;
      set => graphRunner.maxNumHands = value;
    }

    public float minDetectionConfidence
    {
      get => graphRunner.minDetectionConfidence;
      set => graphRunner.minDetectionConfidence = value;
    }

    public float minTrackingConfidence
    {
      get => graphRunner.minTrackingConfidence;
      set => graphRunner.minTrackingConfidence = value;
    }

    private void Update() {
      // print(hloutput[0].Landmark);

      if(hloutput != null){
        if( hloutput[0] != null){
          float x1 = hloutput[0].Landmark[5].X;
          float y1 =  hloutput[0].Landmark[5].Y; 
          float x2 = hloutput[0].Landmark[17].X;
          float y2 =  hloutput[0].Landmark[17].Y; 
          
          Hand1.transform.localPosition = new Vector3(hloutput[0].Landmark[0].X, hloutput[0].Landmark[0].Y, hloutput[0].Landmark[0].Z-zDepth(CalculateDistance(x1, y1, x2, y2)));
          print(zDepth(CalculateDistance(x1, y1, x2, y2)));
        }
        if(hloutput.Count > 1 && hloutput[1] != null){
          float x1 = hloutput[1].Landmark[5].X;
          float y1 =  hloutput[1].Landmark[5].Y; 
          float x2 = hloutput[1].Landmark[17].X;
          float y2 =  hloutput[1].Landmark[17].Y; 
           Hand2.transform.localPosition = new Vector3(hloutput[1].Landmark[0].X, hloutput[1].Landmark[0].Y, hloutput[1].Landmark[0].Z-zDepth(CalculateDistance(x1, y1, x2, y2)));
        }
      }

    }

    static float CalculateDistance(float x1, float y1, float x2, float y2)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
        return distance;
    }
    private float zDepth(float z){
      float temp1 = 0.1f;
      float tempwidth = 3;
      float focal = (tempwidth * temp1) / 3; // manual cam calibration
      return distanceToCamera(3, focal, z);
    }
    private float distanceToCamera(float knownWidth, float focalLength, float perWidth){
      return (knownWidth * focalLength) / perWidth;
    }

    protected override void OnStartRun()
    {
      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnPalmDetectectionsOutput += OnPalmDetectionsOutput;
        graphRunner.OnHandRectsFromPalmDetectionsOutput += OnHandRectsFromPalmDetectionsOutput;
        graphRunner.OnHandLandmarksOutput += OnHandLandmarksOutput;
        // TODO: render HandWorldLandmarks annotations
        graphRunner.OnHandRectsFromLandmarksOutput += OnHandRectsFromLandmarksOutput;
        graphRunner.OnHandednessOutput += OnHandednessOutput;
      }

      var imageSource = ImageSourceProvider.ImageSource;
      SetupAnnotationController(_palmDetectionsAnnotationController, imageSource, true);
      SetupAnnotationController(_handRectsFromPalmDetectionsAnnotationController, imageSource, true);
      SetupAnnotationController(_handLandmarksAnnotationController, imageSource, true);
      SetupAnnotationController(_handRectsFromLandmarksAnnotationController, imageSource, true);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      List<Detection> palmDetections = null;
      List<NormalizedRect> handRectsFromPalmDetections = null;
      List<NormalizedLandmarkList> handLandmarks = null;
      List<LandmarkList> handWorldLandmarks = null;
      List<NormalizedRect> handRectsFromLandmarks = null;
      List<ClassificationList> handedness = null;

      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out palmDetections, out handRectsFromPalmDetections, out handLandmarks, out handWorldLandmarks, out handRectsFromLandmarks, out handedness, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out palmDetections, out handRectsFromPalmDetections, out handLandmarks, out handWorldLandmarks, out handRectsFromLandmarks, out handedness, false));
      }

      _palmDetectionsAnnotationController.DrawNow(palmDetections);
      _handRectsFromPalmDetectionsAnnotationController.DrawNow(handRectsFromPalmDetections);
      _handLandmarksAnnotationController.DrawNow(handLandmarks, handedness);

            print("Landmark print here!");
            print(handLandmarks[0]);

      // TODO: render HandWorldLandmarks annotations
      _handRectsFromLandmarksAnnotationController.DrawNow(handRectsFromLandmarks);

      // print(handLandmarks);

      
    }

    private void OnPalmDetectionsOutput(object stream, OutputEventArgs<List<Detection>> eventArgs)
    {
      
      _palmDetectionsAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnHandRectsFromPalmDetectionsOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
    {
      _handRectsFromPalmDetectionsAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnHandLandmarksOutput(object stream, OutputEventArgs<List<NormalizedLandmarkList>> eventArgs)
    {
      _handLandmarksAnnotationController.DrawLater(eventArgs.value);
      // print(eventArgs.value);
      // foreach(var x in eventArgs.value){
      //   print(x);
      // }
      if(eventArgs.value != null){
        hloutput = eventArgs.value;
      }

    }

    private void OnHandRectsFromLandmarksOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
    {
      _handRectsFromLandmarksAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnHandednessOutput(object stream, OutputEventArgs<List<ClassificationList>> eventArgs)
    {
      _handLandmarksAnnotationController.DrawLater(eventArgs.value);
    }

  }
}
