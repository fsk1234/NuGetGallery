﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Web;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace NuGetGallery
{
    public class ClientInformationTelemetryEnricher : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            var request = telemetry as RequestTelemetry;

            if (request != null)
            {
                var httpContext = GetHttpContext();
                if (httpContext != null && httpContext.Request != null)
                {
                    // ClientVersion is available for NuGet clients starting version 4.1.0-~4.5.0 
                    // Was deprecated and replaced by Protocol version
                    telemetry.Context.Properties.Add(
                        TelemetryService.ClientVersion,
                        httpContext.Request.Headers[Constants.ClientVersionHeaderName]);

                    telemetry.Context.Properties.Add(
                        TelemetryService.ProtocolVersion,
                        httpContext.Request.Headers[Constants.NuGetProtocolHeaderName]);

                    telemetry.Context.Properties.Add(TelemetryService.ClientInformation, httpContext.GetClientInformation());
                }
            }
        }

        protected virtual HttpContextBase GetHttpContext()
        {
            return new HttpContextWrapper(HttpContext.Current);
        }
    }
}