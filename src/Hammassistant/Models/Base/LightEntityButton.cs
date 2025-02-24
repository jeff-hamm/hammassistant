using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Hammlet.NetDaemon.Models;
using NetDaemon.Extensions.Observables;
using NetDaemon.HassModel.Entities;

namespace Hammlet.Models.Base;

/// <summary>
/// GPIO implementation of Button.
/// Inherits from ButtonBase.
/// </summary>
//public class SwitchButton : ButtonBase
//{
//    private bool _shouldDispose;
//    private bool _disposed = false;
//    public SwitchButton(Entity entity)
//    {
//        entity.ToBooleanObservable().SubscribeOnOff(() =>
//        {

//        })

//    }

//    private void OnStateChanged(StateChange obj)
//    {
//        if (obj.New?.IsOn() ?? obj.Entity.IsOn())
//        {
//            if (obj.Old?.IsOn() != true)
//            {
//                foreach (var change in HandleButtonPressed())
//                {

//                }
//            }
//        } else if (obj.Old?.IsOn() == true)
//        {
//            HandleButtonReleased();
//        }
//    }

//    public IDisposable Subscribe(IObserver<StateChange> observer)
//    {
//        throw new NotImplementedException();
//    }
//}