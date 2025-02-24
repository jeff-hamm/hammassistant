using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hammlet.Apps.SceneOnButton;
using Hammlet.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Formatters;
using NetDaemon.HassModel.Entities;

namespace Hammlet.Models.Base;

/// <summary>
/// The different states of a button that is being held.
/// </summary>
public enum ButtonHoldingState
{
    Pending,

    /// <summary>Button holding started.</summary>
    Started,

    /// <summary>Button holding completed.</summary>
    Completed,

    /// <summary>Button holding cancelled.</summary>
    Canceled,
}
//}

//public class ButtonHoldingEventArgs : EventArgs
//{
//    /// <summary>
//    /// Gets or sets button holding state.
//    /// </summary>
//    public ButtonHoldingState HoldingState { get; set; }
//};


//public interface IStateEvent<TStateChange,TEventData>
//    where TStateChange:StateChange
//{
//    public TStateChange StateChange { get; }
//    public TEventData EventData { get; }
//}

//public class ButtonStateEvent<TStateChange> : IStateEvent<TStateChange, ButtonEvent>
//    where TStateChange : StateChange
//{
//    public TStateChange StateChange { get; init; }
//    public ButtonEvent EventData { get; set; }
//}

//public class ButtonService<TEntity, TEntityState> : IDisposable, IObservable<ButtonStateEvent<StateChange<TEntity, TEntityState>>>
//    where TEntity : Entity
//    where TEntityState : EntityState
//{
//    private readonly IDisposable _subscription;
//    private readonly Subject<ButtonStateEvent<StateChange<TEntity, TEntityState>>> _subject = new();
//    public ButtonService(TEntity entity, IScheduler scheduler)
//    {
//        _subscription = entity
//            .ToBooleanObservable()
//            .SubscribeOn(() => { }
                
                    
//                )
//                .SubscribeOnOff(onAction: () =>
//            {

//            }, 
//            offAction: () =>
//            {

//            });
//        {
//            _subject.OnNext(new ButtonStateEvent<StateChange<TEntity, TEntityState>> { StateChange = e, 
//                EventData = new ButtonEvent(ButtonEventType.KeyDown) });
//        });
//    }

//    public IDisposable Subscribe(IObserver<ButtonStateEvent<StateChange<TEntity, TEntityState>>> observer)
//    {
//        return _subject.Subscribe(observer);
//    }
//    public void Dispose()
//    {
//        _subscription.Dispose();
//        _subject.Dispose();
//    }
//}

//public class ButtonClickSubscription<TStateEvent> : IObservable<ButtonStateEvent<TStateEvent>>
//    where TStateEvent : StateChange
//{
//        internal const long DefaultDoublePressTicks = 15000000;
//        internal const long DefaultHoldingMilliseconds = 2000;

//        private readonly long _doublePressTicks;
//        private readonly long _holdingMs;
//        private readonly TimeSpan _debounceTime;

//        private bool _disposed = false;
//        private long _debounceStartTicks;
//        private ButtonHoldingState? _holdingState;
//        private long _lastPress = DateTime.MinValue.Ticks;
//        private bool ShouldDebounce => DateTime.UtcNow.Ticks - _debounceStartTicks < _debounceTime.Ticks;

//        private Timer? _holdingTimer;


//        ///// <summary>
//        ///// Delegate for button pressed.
//        ///// </summary>
//        ///// <param name="sender">Caller object.</param>
//        ///// <param name="e">Arguments for invoked delegate.</param>
//        //public delegate void ButtonPressedDelegate(object sender, ButtonEventType e);

//        ///// <summary>
//        ///// Delegate for button holding.
//        ///// </summary>
//        ///// <param name="sender">Caller object.</param>
//        ///// <param name="e">Arguments for invoked delegate.</param>
//        //public delegate void ButtonHoldingDelegate(object sender, ButtonHoldingEventArgs e);

//        ///// <summary>
//        ///// Delegate for button up event.
//        ///// </summary>
//        //public event ButtonPressedDelegate ButtonUp;

//        ///// <summary>
//        ///// Delegate for button down event.
//        ///// </summary>
//        //public event ButtonPressedDelegate ButtonDown;

//        ///// <summary>
//        ///// Delegate for button pressed event.
//        ///// </summary>
//        //public event ButtonPressedDelegate Press;

//        ///// <summary>
//        ///// Delegate for button double pressed event.
//        ///// </summary>
//        //public event ButtonPressedDelegate DoublePress;

//        /// <summary>
//        /// Delegate for button holding event.
//        /// </summary>
//        //public event ButtonHoldingDelegate Holding;

//        //public IDisposable Subscribe(IObserver<StateChange> observer)
//        //{
        
//        //    return eventHandler.Subscribe(observer);
//        //}
//        protected abstract IObservable<TStateChange> InnerObservable { get; }
//    /// <summary>
//    /// Gets or sets a value indicating whether holding event is enabled or disabled on the button.
//    /// </summary>
//    public bool IsHoldingEnabled { get; set; } = false;

//        /// <summary>
//        /// Gets or sets a value indicating whether double press event is enabled or disabled on the button.
//        /// </summary>
//        public bool IsDoublePressEnabled { get; set; } = false;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ButtonBase" /> class.
//        /// </summary>
//        public ButtonBase(Entity entity) : this(entity,TimeSpan.FromTicks(DefaultDoublePressTicks), TimeSpan.FromMilliseconds(DefaultHoldingMilliseconds))
//        {
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ButtonBase" /> class.
//        /// </summary>
//        /// <param name="doublePress">Max ticks between button presses to count as doublepress.</param>
//        /// <param name="holding">Min ms a button is pressed to count as holding.</param>
//        /// <param name="debounceTime">The amount of time during which the transitions are ignored, or zero.</param>
//        public ButtonBase(Entity entity, TimeSpan doublePress, TimeSpan holding, TimeSpan debounceTime = default(TimeSpan))
//        {
//            if (debounceTime.TotalMilliseconds * 3 > doublePress.TotalMilliseconds)
//            {
//                throw new ArgumentException($"The parameter {nameof(doublePress)} should be at least three times {nameof(debounceTime)}");
//            }
            
//            _doublePressTicks = doublePress.Ticks;
//            _holdingMs = (long)holding.TotalMilliseconds;
//            _debounceTime = debounceTime;
//            entity.ToBooleanObservable().SubscribeOnOff(() =>
//            {
                
//            })

//        }

//}

//public class Button<TEntity,TEntityState> : ButtonBase<TEntity, TEntityState>
//    where TEntity : Entity
//    where TEntityState : EntityState
//{

////}
//public class ButtonState<TEntity, TEntityState>
//    where TEntity : Entity
//    where TEntityState : EntityState
//{
//    public StateChange<TEntity, TEntityState>? StateChange { get; set; }
//    public ButtonEventType ButtonEvent { get; set; }
//}
///// <summary>
//    /// Base implementation of Button logic.
//    /// Hardware independent. Inherit for specific hardware handling.
//    /// </summary>
//    public class ButtonBase<TEntity, TEntityState> : IDisposable, IObservable<ButtonState<TEntity,TEntityState>>
//        where TEntity : Entity
//        where TEntityState : EntityState
//    {

//    }
