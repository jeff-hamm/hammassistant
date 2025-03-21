//------------------------------------------------------------------------------
// <auto-generated>
// Generated using NetDaemon CodeGenerator nd-codegen v1.0.0.0
//   At: 2025-02-14T20:58:04.1607073-08:00
//
// *** Make sure the version of the codegen tool and your nugets NetDaemon.* have the same version.***
// You can use following command to keep it up to date with the latest version:
//   dotnet tool update NetDaemon.HassModel.CodeGen
//
// To update this file with latest entities run this command in your project directory:
//   dotnet tool run nd-codegen
//
// In the template projects we provided a convenience powershell script that will update
// the codegen and nugets to latest versions update_all_dependencies.ps1.
//
// For more information: https://netdaemon.xyz/docs/user/hass_model/hass_model_codegen
// For more information about NetDaemon: https://netdaemon.xyz/
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetDaemon.HassModel;
using NetDaemon.HassModel.Entities;
using NetDaemon.HassModel.Entities.Core;

namespace Hammlet.NetDaemon.Models;
public partial class SystemLogServices
{
    private readonly IHaContext _haContext;
    public SystemLogServices(IHaContext haContext)
    {
        _haContext = haContext;
    }

    ///<summary>Deletes all log entries.</summary>
    public void Clear(object? data = null)
    {
        _haContext.CallService("system_log", "clear", null, data);
    }

    ///<summary>Write log entry.</summary>
    public void Write(SystemLogWriteParameters data)
    {
        _haContext.CallService("system_log", "write", null, data);
    }

    ///<summary>Write log entry.</summary>
    ///<param name="message">Message to log. eg: Something went wrong</param>
    ///<param name="level">Log level.</param>
    ///<param name="logger">Logger name under which to log the message. Defaults to `system_log.external`. eg: mycomponent.myplatform</param>
    public void Write(string message, object? level = null, string? logger = null)
    {
        _haContext.CallService("system_log", "write", null, new SystemLogWriteParameters { Message = message, Level = level, Logger = logger });
    }
}