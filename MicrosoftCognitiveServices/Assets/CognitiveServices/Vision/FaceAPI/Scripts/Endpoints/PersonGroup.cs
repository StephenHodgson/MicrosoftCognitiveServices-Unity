using System;
using System.Threading.Tasks;
using Microsoft.Cognitive.DataStructures.PersonGroup;
using UnityEngine;

namespace Microsoft.Cognitive.Vision.Face
{
    public static class PersonGroup
    {
        private const string PersonGroupQuery = @"https://{0}.api.cognitive.microsoft.com/face/v1.0/persongroups/{1}";
        private const string PersonListQuery = @"https://{0}.api.cognitive.microsoft.com/face/v1.0/persongroups?start={1}&top={2}";
        private const string TrainQuery = @"https://{0}.api.cognitive.microsoft.com/face/v1.0/persongroups/{1}/train";
        private const string TrainingStatusQuery = @"https://{0}.api.cognitive.microsoft.com/face/v1.0/persongroups/{1}/training";

        /// <summary>
        /// Retrieve person group name and userData.
        /// </summary>
        /// <param name="personGroupId"></param>
        /// <returns>A successful call returns the person group's information.</returns>
        public static async Task<PersonGroupInfo?> GetGroupAsync(string personGroupId)
        {
            string query = string.Format(PersonGroupQuery, FaceApiClient.ResourceRegion.ToString().ToLower(), personGroupId);
            Rest.Response response = await Rest.GetAsync(query, FaceApiClient.ApiKeyHeader);
            return response.Successful ? JsonUtility.FromJson<PersonGroupInfo>(response.ResponseBody) : (PersonGroupInfo?)null;
        }

        /// <summary>
        /// List person groups’s personGroupId, name, and userData.
        /// </summary>
        /// <param name="start">List person groups from the least personGroupId greater than the "start". It contains no more than 64 characters. Default is empty.</param>
        /// <param name="top">The number of person groups to list, ranging in [1, 1000]. Default is 1000.</param>
        /// <returns>A successful call returns an array of person groups and their information (personGroupId, name and userData).</returns>
        public static async Task<PersonGroupInfo[]> GetGroupListAsync(string start = "", int top = 1000)
        {
            string query = string.Format(PersonListQuery, FaceApiClient.ResourceRegion, start, top);
            Rest.Response response = await Rest.GetAsync(query, FaceApiClient.ApiKeyHeader);
            return response.Successful ? JsonUtility.FromJson<PersonGroupList>(string.Format("{{\"PersonGroups\":{0}}}", response.ResponseBody)).PersonGroups : null;
        }

        /// <summary>
        /// Create a new person group with specified personGroupId, name, and user-provided userData.<para/>
        /// </summary>
        /// <param name="displayName">Person group display name. The maximum length is 128.</param>
        /// <param name="userData">User-provided data attached to the person group. The size limit is 16KB.</param>
        /// <returns>A successful call returns an empty response body.</returns>
        public static async Task CreateGroupAsync(string displayName, string userData = "")
        {
            string personGroupId = Guid.NewGuid().ToString().ToLower();

            // If we don't have a display name then set it as our group id.
            if (string.IsNullOrEmpty(displayName))
            {
                displayName = personGroupId;
            }

            // The maximum length of the displayName is 128.
            if (displayName.Length > 128)
            {
                displayName = displayName.Substring(0, 128);
                Debug.LogWarningFormat("The display name was truncated to {0}", displayName);
            }

            var query = string.Format(PersonGroupQuery, FaceApiClient.ResourceRegion.ToString().ToLower(), personGroupId);
            await Rest.PutAsync(query, JsonUtility.ToJson(new CreateGroupRequest(displayName, userData)), FaceApiClient.ApiKeyHeader);
        }

        /// <summary>
        /// Delete an existing person group with specified personGroupId. Persisted data in this person group will be deleted.
        /// </summary>
        /// <param name="personGroupId">The personGroupId of the person group to be deleted.</param>
        /// <returns>A successful call returns an empty response body.</returns>
        public static async Task DeleteGroupAsync(string personGroupId)
        {
            var query = string.Format(PersonGroupQuery, FaceApiClient.ResourceRegion.ToString().ToLower(), personGroupId);
            await Rest.DeleteAsync(query, FaceApiClient.ApiKeyHeader);
        }

        /// <summary>
        /// Submit a person group training task. Training is a crucial step that only a trained person group can be used by Face - Identify. 
        /// </summary>
        /// <param name="personGroupId">Target person group to be trained.</param>
        /// <returns>A successful call returns an empty JSON body.</returns>
        public static async Task TrainGroupAsync(string personGroupId)
        {
            var query = string.Format(TrainQuery, FaceApiClient.ResourceRegion.ToString().ToLower(), personGroupId);
            await Rest.PostAsync(query, FaceApiClient.ApiKeyHeader);
        }

        /// <summary>
        /// To check person group training status completed or still ongoing.
        /// </summary>
        /// <param name="personGroupId">personGroupId of target person group.</param>
        /// <returns>A successful call returns the person group's training status.</returns>
        public static async Task<TrainingStatus?> GetTrainingStatusAsync(string personGroupId)
        {
            var query = string.Format(TrainingStatusQuery, FaceApiClient.ResourceRegion.ToString().ToLower(), personGroupId);
            Rest.Response response = await Rest.GetAsync(query, FaceApiClient.ApiKeyHeader);
            return response.Successful ? JsonUtility.FromJson<TrainingStatus>(response.ResponseBody) : (TrainingStatus?)null;
        }
    }
}
