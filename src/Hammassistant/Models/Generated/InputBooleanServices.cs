//------------------------------------------------------------------------------
// <auto-generated>
// Generated using NetDaemon CodeGenerator nd-codegen v1.0.0.0
//   At: 2025-02-14T20:58:04.0392845-08:00
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
public partial class InputBooleanServices
{
    private readonly IHaContext _haContext;
    public InputBooleanServices(IHaContext haContext)
    {
        _haContext = haContext;
    }

    ///<summary>Reloads helpers from the YAML-configuration.</summary>
    public void Reload(object? data = null)
    {
        _haContext.CallService("input_boolean", "reload", null, data);
    }

    ///<summary>Toggles the helper on/off.</summary>
    ///<param name="target">The target for this service call</param>
    public void Toggle(ServiceTarget target, object? data = null)
    {
        _haContext.CallService("input_boolean", "toggle", target, data);
    }

    ///<summary>Turns off the helper.</summary>
    ///<param name="target">The target for this service call</param>
    public void TurnOff(ServiceTarget target, object? data = null)
    {
        _haContext.CallService("input_boolean", "turn_off", target, data);
    }

    ///<summary>Turns on the helper.</summary>
    ///<param name="target">The target for this service call</param>
    public void TurnOn(ServiceTarget target, object? data = null)
    {
        _haContext.CallService("input_boolean", "turn_on", target, data);
    }
}