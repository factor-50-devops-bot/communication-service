﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationService.Core.Domains.SendGrid
{
    public class Templates
    {
        public Template[] templates { get; set; }
        public UnsubscribeGroups[] createUpdateUnsubscribeGroups { get; set; }
        public UnsubscribeGroups[] deleteUnsubscribeGroups { get; set; }

    }
}
