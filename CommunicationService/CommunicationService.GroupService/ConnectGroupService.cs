﻿using CommunicationService.Core.Interfaces.Services;
using HelpMyStreet.Contracts.GroupService.Response;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Contracts.Shared;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Utils;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationService.GroupService
{
    public class ConnectGroupService : IConnectGroupService
    {
        private readonly IHttpClientWrapper _httpClientWrapper;

        public ConnectGroupService(IHttpClientWrapper httpClientWrapper)
        {
            _httpClientWrapper = httpClientWrapper;
        }

        public async Task<GetGroupMembersResponse> GetGroupMembers(int groupID)
        {
            string path = $"/api/GetGroupMembers?groupID=" + groupID;
            string absolutePath = $"{path}";
            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, absolutePath, CancellationToken.None).ConfigureAwait(false))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var getJobsResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetGroupMembersResponse, GroupServiceErrorCode>>(jsonResponse);
                if (getJobsResponse.HasContent && getJobsResponse.IsSuccessful)
                {
                    return getJobsResponse.Content;
                }
                return null;
            }
        }

        public async Task<GetGroupResponse> GetGroupResponse(int groupId)
        {
            string path = $"/api/GetGroup?groupID=" + groupId;
            string absolutePath = $"{path}";
            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, absolutePath, CancellationToken.None).ConfigureAwait(false))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var getJobsResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetGroupResponse, GroupServiceErrorCode>>(jsonResponse);
                if (getJobsResponse.HasContent && getJobsResponse.IsSuccessful)
                {
                    return getJobsResponse.Content;
                }
                return null;
            }
        }

        public async Task<GetUserGroupsResponse> GetUserGroups(int userId)
        {
            string path = $"/api/GetUserGroups?userId={userId}";
            string absolutePath = $"{path}";

            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, absolutePath, CancellationToken.None).ConfigureAwait(false))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var getJobsResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetUserGroupsResponse, GroupServiceErrorCode>>(jsonResponse);
                if (getJobsResponse.HasContent && getJobsResponse.IsSuccessful)
                {
                    return getJobsResponse.Content;
                }
                return null;
            }
        }
    }
}
