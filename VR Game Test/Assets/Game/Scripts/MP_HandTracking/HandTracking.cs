using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Unity.CoordinateSystem;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace Augbox
{
    public class HandTracking : MonoBehaviour
    {
        [SerializeField] private TextAsset _configAsset;
        [SerializeField] private RawImage _screen;
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private int _fps;
        [SerializeField] private MultiHandLandmarkListAnnotationController _HandLandmarkAnnotationController;

        private CalculatorGraph _graph;
        private ResourceManager _resourceManager;

        private WebCamTexture _webCamTexture;
        private Texture2D _inputTexture;
        private Color32[] _inputPixelData;
        private Texture2D _outputTexture;
        private Color32[] _outputPixelData;

        public RotationAngle rotation { get; private set; } = 0;
        private IEnumerator Start()
        {
            if (WebCamTexture.devices.Length == 0)
            {
                throw new System.Exception("Web Camera devices are not found");
            }
            var webCamDevice = WebCamTexture.devices[0];
            _webCamTexture = new WebCamTexture(webCamDevice.name, _width, _height, _fps);
            _webCamTexture.Play();

            yield return new WaitUntil(() => _webCamTexture.width > 16);

            _screen.rectTransform.sizeDelta = new Vector2(_width, _height);

            _inputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
            _inputPixelData = new Color32[_width * _height];
            _outputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
            _outputPixelData = new Color32[_width * _height];

            _screen.texture = _outputTexture;

            _resourceManager = new LocalResourceManager();
            yield return _resourceManager.PrepareAssetAsync("hand_landmark_lite.bytes");
            yield return _resourceManager.PrepareAssetAsync("hand_landmark_full.bytes");
            yield return _resourceManager.PrepareAssetAsync("palm_detection_lite.bytes");
            yield return _resourceManager.PrepareAssetAsync("palm_detection_full.bytes");

            var stopwatch = new Stopwatch();

            // _graph = new CalculatorGraph(_configAsset.text);

            var config = CalculatorGraphConfig.Parser.ParseFromTextFormat(_configAsset.text);
            using (var validatedGraphConfig = new ValidatedGraphConfig())
            {
                validatedGraphConfig.Initialize(config).AssertOk();
                _graph = new CalculatorGraph(validatedGraphConfig.Config());
            }

            // var outputVideoStream = new OutputStream<ImageFramePacket, ImageFrame>(_graph, "transformed_input_video");
            var handLandMarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(_graph, "hand_landmarks");

            // outputVideoStream.StartPolling().AssertOk();
            handLandMarksStream.StartPolling().AssertOk();
            // SetImageTransformationOptions();
            
            _graph.StartRun(BuildSidePacket()).AssertOk();
            stopwatch.Start();

            var screenRect = _screen.GetComponent<RectTransform>().rect;

            while (true)
            {
                _inputTexture.SetPixels32(_webCamTexture.GetPixels32(_inputPixelData));
                var imageFrame = new ImageFrame(ImageFormat.Types.Format.Srgba, _width, _height, _width * 4, _inputTexture.GetRawTextureData<byte>());
                var currentTimestamp = stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000);
                _graph.AddPacketToInputStream("input_video", new ImageFramePacket(imageFrame, new Timestamp(currentTimestamp))).AssertOk();

                yield return new WaitForEndOfFrame();

                // if (outputVideoStream.TryGetNext(out var outputVideo))
                // {
                //     if (outputVideo.TryReadPixelData(_outputPixelData))
                //     {
                //         _outputTexture.SetPixels32(_outputPixelData);
                //         _outputTexture.Apply();
                //     }
                // }

                if (handLandMarksStream.TryGetNext(out var handLandmark))
                {
                    if (handLandmark != null && handLandmark.Count > 0)
                    {
                        foreach (var landmarks in handLandmark)
                        {
                            // _HandLandmarkAnnotationController.isMirrored = true;

                            print(landmarks);
                            // top of the head
                            // var topOfHead = landmarks.Landmark[10];
                            // Debug.Log($"Unity Local Coordinates: {screenRect.GetPoint(topOfHead)}, Image Coordinates: {topOfHead}");
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (_webCamTexture != null)
            {
                _webCamTexture.Stop();
            }

            if (_graph != null)
            {
                try
                {
                    _graph.CloseInputStream("input_video").AssertOk();
                    _graph.WaitUntilDone().AssertOk();
                }
                finally
                {

                    _graph.Dispose();
                }
            }
        }
        private SidePacket BuildSidePacket()
        {
            int maxNumHands = 2;
            var sidePacket = new SidePacket();

            SetImageTransformationOptions(sidePacket,  true);
            sidePacket.Emplace("model_complexity", new IntPacket((int)0));
            sidePacket.Emplace("num_hands", new IntPacket(maxNumHands));

            return sidePacket;
        }

        protected void SetImageTransformationOptions(SidePacket sidePacket, bool expectedToBeMirrored = false)
        {
            // NOTE: The origin is left-bottom corner in Unity, and right-top corner in MediaPipe.
            var inputRotation = rotation;
            var isInverted = ImageCoordinate.IsInverted(rotation);
            var shouldBeMirrored = true;
            var inputHorizontallyFlipped = isInverted ^ shouldBeMirrored;
            var inputVerticallyFlipped = !isInverted;

            if ((inputHorizontallyFlipped && inputVerticallyFlipped) || rotation == RotationAngle.Rotation180)
            {
                inputRotation = inputRotation.Add(RotationAngle.Rotation180);
                inputHorizontallyFlipped = !inputHorizontallyFlipped;
                inputVerticallyFlipped = !inputVerticallyFlipped;
            }

            // Logger.LogDebug($"input_rotation = {inputRotation}, input_horizontally_flipped = {inputHorizontallyFlipped}, input_vertically_flipped = {inputVerticallyFlipped}");

            sidePacket.Emplace("input_rotation", new IntPacket((int)inputRotation));
            sidePacket.Emplace("input_horizontally_flipped", new BoolPacket(inputHorizontallyFlipped));
            sidePacket.Emplace("input_vertically_flipped", new BoolPacket(inputVerticallyFlipped));
        }
    }
}