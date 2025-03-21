//------------------------------------------------------------------------------
// <auto-generated>
// Generated using NetDaemon CodeGenerator nd-codegen v1.0.0.0
//   At: 2025-02-14T20:58:04.1689778-08:00
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
public partial class TodoServices
{
    private readonly IHaContext _haContext;
    public TodoServices(IHaContext haContext)
    {
        _haContext = haContext;
    }

    ///<summary>Adds a new to-do list item.</summary>
    ///<param name="target">The target for this service call</param>
    public void AddItem(ServiceTarget target, TodoAddItemParameters data)
    {
        _haContext.CallService("todo", "add_item", target, data);
    }

    ///<summary>Adds a new to-do list item.</summary>
    ///<param name="item">The name that represents the to-do item. eg: Submit income tax return</param>
    ///<param name="dueDate">The date the to-do item is expected to be completed. eg: 2023-11-17</param>
    ///<param name="dueDatetime">The date and time the to-do item is expected to be completed. eg: 2023-11-17 13:30:00</param>
    ///<param name="description">A more complete description of the to-do item than provided by the item name. eg: A more complete description of the to-do item than that provided by the summary.</param>
    public void AddItem(ServiceTarget target, string item, DateOnly? dueDate = null, DateTime? dueDatetime = null, string? description = null)
    {
        _haContext.CallService("todo", "add_item", target, new TodoAddItemParameters { Item = item, DueDate = dueDate, DueDatetime = dueDatetime, Description = description });
    }

    ///<summary>Gets items on a to-do list.</summary>
    ///<param name="target">The target for this service call</param>
    public void GetItems(ServiceTarget target, TodoGetItemsParameters data)
    {
        _haContext.CallService("todo", "get_items", target, data);
    }

    ///<summary>Gets items on a to-do list.</summary>
    ///<param name="status">Only return to-do items with the specified statuses. Returns not completed actions by default. eg: needs_action</param>
    public void GetItems(ServiceTarget target, IEnumerable<object>? status = null)
    {
        _haContext.CallService("todo", "get_items", target, new TodoGetItemsParameters { Status = status });
    }

    ///<summary>Gets items on a to-do list.</summary>
    ///<param name="target">The target for this service call</param>
    public Task<JsonElement?> GetItemsAsync(ServiceTarget target, TodoGetItemsParameters data)
    {
        return _haContext.CallServiceWithResponseAsync("todo", "get_items", target, data);
    }

    ///<summary>Gets items on a to-do list.</summary>
    ///<param name="target">The target for this service call</param>
    ///<param name="status">Only return to-do items with the specified statuses. Returns not completed actions by default. eg: needs_action</param>
    public Task<JsonElement?> GetItemsAsync(ServiceTarget target, IEnumerable<object>? status = null)
    {
        return _haContext.CallServiceWithResponseAsync("todo", "get_items", target, new TodoGetItemsParameters { Status = status });
    }

    ///<summary>Removes all to-do list items that have been completed.</summary>
    ///<param name="target">The target for this service call</param>
    public void RemoveCompletedItems(ServiceTarget target, object? data = null)
    {
        _haContext.CallService("todo", "remove_completed_items", target, data);
    }

    ///<summary>Removes an existing to-do list item by its name.</summary>
    ///<param name="target">The target for this service call</param>
    public void RemoveItem(ServiceTarget target, TodoRemoveItemParameters data)
    {
        _haContext.CallService("todo", "remove_item", target, data);
    }

    ///<summary>Removes an existing to-do list item by its name.</summary>
    ///<param name="item">The name for the to-do list item.</param>
    public void RemoveItem(ServiceTarget target, string item)
    {
        _haContext.CallService("todo", "remove_item", target, new TodoRemoveItemParameters { Item = item });
    }

    ///<summary>Updates an existing to-do list item based on its name.</summary>
    ///<param name="target">The target for this service call</param>
    public void UpdateItem(ServiceTarget target, TodoUpdateItemParameters data)
    {
        _haContext.CallService("todo", "update_item", target, data);
    }

    ///<summary>Updates an existing to-do list item based on its name.</summary>
    ///<param name="item">The current name of the to-do item. eg: Submit income tax return</param>
    ///<param name="rename">The new name for the to-do item eg: Something else</param>
    ///<param name="status">A status or confirmation of the to-do item. eg: needs_action</param>
    ///<param name="dueDate">The date the to-do item is expected to be completed. eg: 2023-11-17</param>
    ///<param name="dueDatetime">The date and time the to-do item is expected to be completed. eg: 2023-11-17 13:30:00</param>
    ///<param name="description">A more complete description of the to-do item than provided by the item name. eg: A more complete description of the to-do item than that provided by the summary.</param>
    public void UpdateItem(ServiceTarget target, string item, string? rename = null, object? status = null, DateOnly? dueDate = null, DateTime? dueDatetime = null, string? description = null)
    {
        _haContext.CallService("todo", "update_item", target, new TodoUpdateItemParameters { Item = item, Rename = rename, Status = status, DueDate = dueDate, DueDatetime = dueDatetime, Description = description });
    }
}