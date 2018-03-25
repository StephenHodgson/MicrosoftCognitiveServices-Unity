using System;
using System.Threading.Tasks;
using Microsoft.Cognitive.DataStructures.Face;
using UnityEngine;

namespace Microsoft.Cognitive.Vision.Face
{
    public static class Face
    {
        private const string DetectQuery = @"https://{0}.api.cognitive.microsoft.com/face/v1.0/detect";
        private const string IdentityQuery = @"https://{0}.api.cognitive.microsoft.com/face/v1.0/identify";

        [Flags]
        public enum FaceAttributes
        {
            None = 0,
            Age = 1 << 0,
            Gender = 1 << 1,
            HeadPose = 1 << 2,
            Smile = 1 << 3,
            FacialHair = 1 << 4,
            Glasses = 1 << 5,
            Emotion = 1 << 6,
            Hair = 1 << 7,
            Makeup = 1 << 8,
            Occlusion = 1 << 9,
            Accessories = 1 << 10,
            Blur = 1 << 11,
            Exposure = 1 << 12,
            Noise = 1 << 13
        }

        /// <summary>
        /// Detect human faces in an image, return face rectangles, and optionally with faceIds, landmarks, and attributes.<para/>
        ///
        /// Optional parameters including faceId, landmarks, and attributes.  Attributes include age, gender, headPose, smile, facialHair,
        ///  glasses, emotion, hair, makeup, occlusion, accessories, blur, exposure and noise.<para/>
        /// </summary>
        /// <param name="imageUrl">The url of the image to use to detect the face.</param>
        /// <param name="returnFaceId">Return faceIds of the detected faces or not. The default value is true.</param>
        /// <param name="returnFaceLandmarks">Return face landmarks of the detected faces or not. The default value is false.</param>
        /// <param name="returnFaceAttributes">Analyze and return the one or more specified face attributes.
        ///  Face attribute analysis has additional computational and time cost.</param>
        /// <returns>A successful call returns an array of face entries ranked by face rectangle size in descending order.
        ///  An empty response indicates no faces detected.</returns>
        public static async Task<FaceInfo[]> DetectAsync
            (string imageUrl, bool returnFaceId = true, bool returnFaceLandmarks = false, FaceAttributes returnFaceAttributes = FaceAttributes.None)
        {
            var query = string.Format("{0}{1}{2}",
                string.Format(DetectQuery, FaceApiClient.ResourceRegion.ToString().ToLower()),
                string.Format("?returnFaceId={0}&returnFaceLandmarks={1}", returnFaceId, returnFaceLandmarks),
                ParseFaceAttributes(returnFaceAttributes));
            Rest.Response response = await Rest.PostAsync(query, "{\"url\":\"" + imageUrl + "\"}", FaceApiClient.ApiKeyHeader);
            if (!response.Successful) { throw new InvalidOperationException(response.ResponseBody); }
            return JsonUtility.FromJson<FaceList>(response.ResponseBody).Faces;
        }

        /// <summary>
        /// Detect human faces in an image, return face rectangles, and optionally with faceIds, landmarks, and attributes.<para/>
        ///
        /// Optional parameters including faceId, landmarks, and attributes.  Attributes include age, gender, headPose, smile, facialHair,
        ///  glasses, emotion, hair, makeup, occlusion, accessories, blur, exposure and noise.<para/>
        /// </summary>
        /// <param name="imageData">Ray byte data of the image to use to detect the face.</param>
        /// <param name="returnFaceId">Return faceIds of the detected faces or not. The default value is true.</param>
        /// <param name="returnFaceLandmarks">Return face landmarks of the detected faces or not. The default value is false.</param>
        /// <param name="returnFaceAttributes">Analyze and return the one or more specified face attributes.
        ///  Face attribute analysis has additional computational and time cost.</param>
        /// <returns>A successful call returns an array of face entries ranked by face rectangle size in descending order.
        ///  An empty response indicates no faces detected.</returns>
        public static async Task<FaceInfo[]> DetectAsync
            (byte[] imageData, bool returnFaceId = true, bool returnFaceLandmarks = false, FaceAttributes returnFaceAttributes = FaceAttributes.None)
        {
            var query = string.Format("{0}{1}{2}",
                string.Format(DetectQuery, FaceApiClient.ResourceRegion.ToString().ToLower()),
                string.Format("?returnFaceId={0}&returnFaceLandmarks={1}", returnFaceId, returnFaceLandmarks),
                ParseFaceAttributes(returnFaceAttributes));
            Rest.Response response = await Rest.PostAsync(query, imageData, FaceApiClient.ApiKeyHeader);
            if (!response.Successful) { throw new InvalidOperationException(response.ResponseBody); }
            return JsonUtility.FromJson<FaceList>(string.Format("{{\"Faces\":{0}}}", response.ResponseBody)).Faces;
        }

        private static string ParseFaceAttributes(FaceAttributes returnFaceAttributes)
        {
            int count = 0;
            string attributes = string.Empty;
            foreach (FaceAttributes attributeValue in Enum.GetValues(typeof(FaceAttributes)))
            {
                if ((returnFaceAttributes & attributeValue) == attributeValue && attributeValue != FaceAttributes.None)
                {
                    attributes = string.Format("{0}{1}{2}",
                                               attributes,
                                               count == 0 ? string.Empty : ",",
                                               attributeValue.ToStringCamelCase());
                    count++;
                }
            }

            if (count > 0 && !string.IsNullOrEmpty(attributes))
            {
                return string.Format("&returnFaceAttributes={0}", attributes);
            }

            return string.Empty;
        }

        /// <summary>
        /// 1-to-many identification to find the closest matches of the specific query person face from a person group or large person group.
        /// </summary>
        /// <param name="personGroupId">personGroupId of the target person group, created by PersonGroup - Create.
        ///  Parameter personGroupId and largePersonGroupId should not be provided at the same time.</param>
        /// <param name="faceIds">Array of query faces faceIds, created by the Face - Detect. Each of the faces are identified independently.
        ///  The valid number of faceIds is between [1, 10].</param>
        /// <param name="maxNumOfCandidatesReturned">The range of maxNumOfCandidatesReturned is between 1 and 100 (default is 10).</param>
        /// <param name="confidenceThreshold">Customized identification confidence threshold, in the range of [0, 1]. Advanced user can tweak
        ///  this value to override default internal threshold for better precision on their scenario data. Note there is no guarantee of this
        ///  threshold value working on other data and after algorithm updates.</param>
        /// <returns>1-to-many identification to find the closest matches of the specific query person face from a person group.</returns>
        public static async Task<IdentifyResult[]> IdentifyAsync
            (string[] faceIds, string personGroupId, int maxNumOfCandidatesReturned = 10, float confidenceThreshold = 0f)
        {
            var query = string.Format(IdentityQuery, FaceApiClient.ResourceRegion.ToString().ToLower());
            var json = JsonUtility.ToJson(new IdentifyRequest(faceIds, personGroupId, maxNumOfCandidatesReturned, confidenceThreshold));
            Rest.Response response = await Rest.PostAsync(query, json, FaceApiClient.ApiKeyHeader);
            if (!response.Successful) { throw new InvalidOperationException(response.ResponseBody); }
            return JsonUtility.FromJson<IdentifyResultList>(string.Format("{{\"Results\":{0}}}", response.ResponseBody)).Results;
        }
    }
}
