﻿using CommunicationService.Core.Domains;
using CommunicationService.Core.Interfaces;
using CommunicationService.Core.Interfaces.Services;
using CommunicationService.MessageService.Substitution;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunicationService.MessageService
{
    public class WelcomeMessage : IMessage
    {
        private readonly IConnectUserService _connectUserService;

        public string UnsubscriptionGroupName
        {
            get
            {
                return "Welcome";
            }
        }

        public WelcomeMessage(IConnectUserService connectUserService)
        {
            _connectUserService = connectUserService;
            
        }

        public async Task<EmailBuildData> PrepareTemplateData(int? recipientUserId, int? jobId, int? groupId)
        {
            var user = await _connectUserService.GetUserByIdAsync(recipientUserId.Value);

            if (user != null)
            {
                return new EmailBuildData()
                {
                    BaseDynamicData = new WelcomeData(user.UserPersonalDetails.FirstName, user.UserPersonalDetails.LastName),
                    EmailToAddress = user.UserPersonalDetails.EmailAddress,
                    EmailToName = user.UserPersonalDetails.DisplayName,
                    RecipientUserID = recipientUserId.Value
                };
            }
            else
            {
                throw new Exception("unable to retrive user details");
            }
        }

        public Dictionary<int,string> IdentifyRecipients(int? recipientUserId, int? jobId, int? groupId)
        {
            Dictionary<int, string> response = new Dictionary<int, string>();
            response.Add(recipientUserId.Value,"Welcome");
            return response;
        }
    }
}
