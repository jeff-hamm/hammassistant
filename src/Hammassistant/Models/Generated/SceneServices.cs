//------------------------------------------------------------------------------
// <auto-generated>
// Generated using NetDaemon CodeGenerator nd-codegen v1.0.0.0
//   At: 2025-02-14T20:58:04.1326558-08:00
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
public partial class SceneServices
{
    private readonly IHaContext _haContext;
    public SceneServices(IHaContext haContext)
    {
        _haContext = haContext;
    }

    ///<summary>Activates a scene with configuration.</summary>
    public void Apply(SceneApplyParameters data)
    {
        _haContext.CallService("scene", "apply", null, data);
    }

    ///<summary>Activates a scene with configuration.</summary>
    ///<param name="entities">List of entities and their target state. eg: light.kitchen: &quot;on&quot; light.ceiling:   state: &quot;on&quot;   brightness: 80 </param>
    ///<param name="transition">Time it takes the devices to transition into the states defined in the scene.</param>
    public void Apply(object entities, double? transition = null)
    {
        _haContext.CallService("scene", "apply", null, new SceneApplyParameters { Entities = entities, Transition = transition });
    }

    ///<summary>Creates a new scene.</summary>
    public void Create(SceneCreateParameters data)
    {
        _haContext.CallService("scene", "create", null, data);
    }

    ///<summary>Creates a new scene.</summary>
    ///<param name="sceneId">The entity ID of the new scene. eg: all_lights</param>
    ///<param name="entities">List of entities and their target state. If your entities are already in the target state right now, use &apos;Entities snapshot&apos; instead. eg: light.tv_back_light: &quot;on&quot; light.ceiling:   state: &quot;on&quot;   brightness: 200 </param>
    ///<param name="snapshotEntities">List of entities to be included in the snapshot. By taking a snapshot, you record the current state of those entities. If you do not want to use the current state of all your entities for this scene, you can combine &apos;Entities snapshot&apos; with &apos;Entity states&apos;. eg: - light.ceiling - light.kitchen </param>
    public void Create(string sceneId, object? entities = null, IEnumerable<string>? snapshotEntities = null)
    {
        _haContext.CallService("scene", "create", null, new SceneCreateParameters { SceneId = sceneId, Entities = entities, SnapshotEntities = snapshotEntities });
    }

    ///<summary>Deletes a dynamically created scene.</summary>
    ///<param name="target">The target for this service call</param>
    public void Delete(ServiceTarget target, object? data = null)
    {
        _haContext.CallService("scene", "delete", target, data);
    }

    ///<summary>Reloads the scenes from the YAML-configuration.</summary>
    public void Reload(object? data = null)
    {
        _haContext.CallService("scene", "reload", null, data);
    }

    ///<summary>Activates a scene.</summary>
    ///<param name="target">The target for this service call</param>
    public void TurnOn(ServiceTarget target, SceneTurnOnParameters data)
    {
        _haContext.CallService("scene", "turn_on", target, data);
    }

    ///<summary>Activates a scene.</summary>
    ///<param name="transition">Time it takes the devices to transition into the states defined in the scene.</param>
    public void TurnOn(ServiceTarget target, double? transition = null)
    {
        _haContext.CallService("scene", "turn_on", target, new SceneTurnOnParameters { Transition = transition });
    }
}