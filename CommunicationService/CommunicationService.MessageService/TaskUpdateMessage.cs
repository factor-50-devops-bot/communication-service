﻿using CommunicationService.Core.Domains;
using CommunicationService.Core.Interfaces;
using CommunicationService.Core.Interfaces.Repositories;
using CommunicationService.Core.Interfaces.Services;
using CommunicationService.MessageService.Substitution;
using HelpMyStreet.Contracts.UserService.Response;
using HelpMyStreet.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationService.MessageService
{
    public class TaskUpdateMessage : IMessage
    {
        private readonly IConnectRequestService _connectRequestService;
        private const int REQUESTOR_DUMMY_USERID = -1;

        List<SendMessageRequest> _sendMessageRequests;

        public string UnsubscriptionGroupName
        {
            get
            {
                return UnsubscribeGroupName.TaskNotification;
            }
        }

        public TaskUpdateMessage(IConnectRequestService connectRequestService)
        {
            _connectRequestService = connectRequestService;
            _sendMessageRequests = new List<SendMessageRequest>();
        }

        public async Task<EmailBuildData> PrepareTemplateData(int? recipientUserId, int? jobId, int? groupId, string templateName)
        {
            var job = _connectRequestService.GetJobDetailsAsync(jobId.Value).Result;
            var timeOfDay = DateTime.Now.ToString("t");
            timeOfDay = Regex.Replace(timeOfDay, @"\s+", "");
            var timeUpdated = $"today at {timeOfDay.ToLower()}";
            bool isFaceMask = job.SupportActivity == SupportActivities.FaceMask;
            bool isOpen = job.JobStatus == JobStatuses.Open;
            bool isDone = job.JobStatus == JobStatuses.Done;
            return new EmailBuildData()
            {
                BaseDynamicData = new TaskUpdateData
                (
                job.DateRequested.ToString("dd/MM/yyyy"),
                Mapping.ActivityMappings[job.SupportActivity],
                Mapping.StatusMappings[job.JobStatus],
                timeUpdated,
                isFaceMask,
                isDone,
                isOpen
                ),
                EmailToAddress = job.Requestor.EmailAddress,
                EmailToName = $"{job.Requestor.FirstName} {job.Requestor.LastName}",
                RecipientUserID = recipientUserId.Value,
            };
        }

        public List<SendMessageRequest> IdentifyRecipients(int? recipientUserId, int? jobId, int? groupId)
        {
            var job = _connectRequestService.GetJobDetailsAsync(jobId.Value).Result;

            _sendMessageRequests.Add(new SendMessageRequest()
            {
                TemplateName = TemplateName.TaskUpdate,
                RecipientUserID = REQUESTOR_DUMMY_USERID,
                GroupID = groupId,
                JobID = jobId
            });
            
            return _sendMessageRequests;
        }
    }
}