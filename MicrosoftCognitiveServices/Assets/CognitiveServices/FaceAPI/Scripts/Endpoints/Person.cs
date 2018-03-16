using System;
using System.Threading.Tasks;
using Microsoft.Cognitive.DataStructures.Face;
using Microsoft.Cognitive.DataStructures.Person;
using UnityEngine;

namespace Microsoft.Cognitive.Face
{
    public static class Person
    {
        private const string GetPersonQuery = @"https://{0}.api.cognitive.microsoft.com/face/v1.0/persongroups/{1}/persons{2}";
        private const string PersistedFacesQuery = @"https://{0}.api.cognitive.microsoft.com/face/v1.0/persongroups/{1}/persons/{2}/persistedFaces{3}";

        /// <summary>
        /// Retrieve a person's name and userData, and the persisted faceIds representing the registered person face image.
        /// </summary>
        /// <param name="personGroupId">Specifying the person group containing the target person.</param>
        /// <param name="personId">Specifying the target person.</param>
        /// <returns>A successful call returns the person's information.</returns>
        public static async Task<PersonInfo?> GetPersonAsync(string personGroupId, string personId)
        {
            var query = string.Format(GetPersonQuery, FaceApiClient.ResourceRegion.ToString().ToLower(), personGroupId, "/" + personId);
            Rest.Response response = await Rest.GetAsync(query, FaceApiClient.FaceApiKeyHeader);
            return response.Successful ? JsonUtility.FromJson<PersonInfo>(response.ResponseBody) : (PersonInfo?)null;
        }

        /// <summary>
        /// List all persons’ information in the specified person group, including personId, name, userData and persistedFaceIds of registered person faces.
        /// </summary>
        /// <param name="personGroupId">personGroupId of the target person group.</param>
        /// <param name="start">List persons from the least personId greater than the "start". It contains no more than 64 characters. Default is empty.</param>
        /// <param name="top">The number of persons to list, ranging in [1, 1000]. Default is 1000.</param>
        /// <returns>A successful call returns an array of person information that belong to the person group. </returns>
        public static async Task<PersonInfo[]> GetPersonListAsync(string personGroupId, string start = "", int top = 1000)
        {
            var query = string.Format(GetPersonQuery, FaceApiClient.ResourceRegion.ToString().ToLower(), personGroupId, string.Format("?start={0}&top={1}", start, top));
            Rest.Response response = await Rest.GetAsync(query, FaceApiClient.FaceApiKeyHeader);
            return response.Successful ? JsonUtility.FromJson<PersonList>(string.Format("{{\"People\":{0}}}", response.ResponseBody)).People : null;
        }

        /// <summary>
        /// Create a new person in a specified person group.
        /// </summary>
        /// <param name="personName">Display name of the target person. The maximum length is 128.</param>
        /// <param name="personGroupId">Specifying the target person group to create the person.</param>
        /// <param name="userData">Optional fields for user-provided data attached to a person. Size limit is 16KB.</param>
        /// <returns>A successful call returns a new personId created.</returns>
        public static async Task<string> CreatePersonAsync(string personName, string personGroupId, string userData = "")
        {
            if (personName.Length > 128)
            {
                personName = personName.Substring(0, 128);
            }

            var query = string.Format(GetPersonQuery, FaceApiClient.ResourceRegion.ToString().ToLower(), personGroupId, "");
            var json = JsonUtility.ToJson(new CreatePerson(personName, userData));
            Rest.Response response = await Rest.PostAsync(query, json, FaceApiClient.FaceApiKeyHeader);
            return response.Successful ? JsonUtility.FromJson<PersonId>(response.ResponseBody).personId : string.Empty;
        }

        /// <summary>
        /// Delete an existing person from a person group. All stored person data, and face images in the person entry will be deleted.
        /// </summary>
        /// <param name="personGroupId">Specifying the person group containing the person.</param>
        /// <param name="personId">The target personId to delete.</param>
        /// <returns>A successful call returns an empty response body.</returns>
        public static async Task DeletePersonAsync(string personGroupId, string personId)
        {
            var query = string.Format(GetPersonQuery, FaceApiClient.ResourceRegion.ToString().ToLower(), personGroupId, "/" + personId);
            await Rest.DeleteAsync(query, FaceApiClient.FaceApiKeyHeader);
        }

        /// <summary>
        /// Add a face image to a person into a person group for face identification or verification. To deal with the image of multiple faces, input face
        /// can be specified as an image with a targetFace rectangle. It returns a persistedFaceId representing the added face. The face image and related
        /// info will be stored on server until PersonGroup PersonFace - Delete, PersonGroup Person - Delete or PersonGroup - Delete is called.
        /// </summary>
        /// <param name="personId">Target person that the face is added to.</param>
        /// <param name="personGroupId">Specifying the person group containing the target person.</param>
        /// <param name="imageData">Face image data. Valid image size is from 1KB to 4MB. Only one face is allowed per image.</param>
        /// <param name="userData">User-specified data about the target face to add for any purpose. The maximum length is 1KB.</param>
        /// <param name="targetFace">A face rectangle to specify the target face to be added to a person, in the format of "targetFace=left,top,width,height".
        ///  E.g. "targetFace=10,10,100,100". If there is more than one face in the image, targetFace is required to specify which face to add. No targetFace
        ///  means there is only one face detected in the entire image.</param>
        /// <returns>A successful call returns the new persistedFaceId.</returns>
        public static async Task<string> CreateFaceAsync(string personId, string personGroupId, byte[] imageData, string userData = "", FaceRectangle targetFace = null)
        {
            string args = string.Format("?userData={0}&targetFace={1}", userData, targetFace);
            var query = string.Format(PersistedFacesQuery, FaceApiClient.ResourceRegion.ToString().ToLower(), personGroupId, personId, args);
            Rest.Response response = await Rest.PostAsync(query, imageData, FaceApiClient.FaceApiKeyHeader);
            return response.Successful ? JsonUtility.FromJson<PersistedFaceInfo>(response.ResponseBody).persistedFaceId : string.Empty;
        }
    }
}
