﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2016-06-14
// 

using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SocialLoginSettingsViewModel
    {
        public Guid SiteId { get; set; } = Guid.Empty; 
        public string SiteName { get; set; } = string.Empty;
        [StringLength(100, ErrorMessage = "Microsoft ClientId has a maximum length of 100 characters")]
        public string MicrosoftClientId { get; set; } = string.Empty;
        public string MicrosoftClientSecret { get; set; } = string.Empty;
        [StringLength(100, ErrorMessage = "Google ClientId has a maximum length of 100 characters")]
        public string GoogleClientId { get; set; } = string.Empty;
        public string GoogleClientSecret { get; set; } = string.Empty;
        [StringLength(100, ErrorMessage = "Facebook AppId has a maximum length of 100 characters")]
        public string FacebookAppId { get; set; } = string.Empty;
        public string FacebookAppSecret { get; set; } = string.Empty;
        [StringLength(100, ErrorMessage = "Twitter Consumer Key has a maximum length of 100 characters")]
        public string TwitterConsumerKey { get; set; } = string.Empty;
        public string TwitterConsumerSecret { get; set; } = string.Empty;

    }
}
