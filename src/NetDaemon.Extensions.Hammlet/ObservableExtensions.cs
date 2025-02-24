using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hammlet.Apps.SceneOnButton;
using Hammlet.Extensions.Observables;
using Hammlet.Models.Base;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Formatters;
using NetDaemon.Extensions.Observables;
using NetDaemon.HassModel.Entities;
using Reactive.Boolean;

namespace Hammlet.Extensions;


public record EntityState<TAttributes,TData>(EntityState<TAttributes> Source, TData Data) : EntityState<TAttributes>(Source)
    where TAttributes : class
{
}
public record StateChangeEvent<TData>(StateChange StateChange, TData Data);

public class ButtonTiming
{
    //public const long DefaultDoublePressMilliseconds = DefaultPressDelay-DefaultDebounceTimeMilliseconds;
    public const long DefaultPressDelay = 800;
    public const long DefaultHoldingMilliseconds = 500;
    public const long DefaultLongPressMilliseconds = 1000;
    public const long DebounceTimeMilliseconds = 50;
    private static int HoldTimeoutMilliseconds => 10000;

    public TimeSpan LongPressTime { get; set; } = TimeSpan.FromMilliseconds(DefaultLongPressMilliseconds);
    public TimeSpan PressDelay { get; set; } = TimeSpan.FromMilliseconds(DefaultPressDelay);
    public TimeSpan HoldDelay { get; set; } = TimeSpan.FromMilliseconds(DefaultHoldingMilliseconds);
    public TimeSpan HoldTickRate { get; set; } = TimeSpan.FromMilliseconds(100);
    public bool FireTickEvents { get; set; } = false;
    public TimeSpan DebounceTime { get; set; } = TimeSpan.FromMilliseconds(DebounceTimeMilliseconds);
    public TimeSpan HoldTimeout { get; set; } = TimeSpan.FromMilliseconds(HoldTimeoutMilliseconds);


    public record StateChangeEvent<TStateChange, TData>(TStateChange State, TData Data);

    public record ToggleEvent<TData>(bool State, TData Data) : StateChangeEvent<bool, TData>(State, Data);


    internal sealed class ButtonEventProducer(IObservable<bool> source, IScheduler scheduler,ButtonTiming options)
        : Producer<ToggleEvent<ButtonEvent>, ButtonEventProducer.ButtonEventSink>
    {
        protected override ButtonEventSink CreateSink(
            IObserver<ToggleEvent<ButtonEvent>> observer) => new(observer, scheduler, options);

        protected override void Run(ButtonEventSink sink) => sink.Run(source);


        public class ButtonEventSink(IObserver<ToggleEvent<ButtonEvent>> observer, IScheduler scheduler, ButtonTiming options)
            : Sink<bool, ToggleEvent<ButtonEvent>>(observer)
        {
            internal const long DefaultDoublePressMilliseconds = DefaultPressDelay - DefaultDebounceTimeMilliseconds;
            internal const long DefaultPressDelay = 250;
            internal const long DefaultHoldingMilliseconds = 2000;
            public const long DefaultDebounceTimeMilliseconds = 50;

            private readonly TimeSpan _doublePressTicks = TimeSpan.FromMilliseconds(DefaultDoublePressMilliseconds);
            private readonly TimeSpan _pressDelay = TimeSpan.FromMilliseconds(DefaultPressDelay);
            private readonly TimeSpan _holdDelay = TimeSpan.FromMilliseconds(DefaultHoldingMilliseconds);
            private readonly TimeSpan _debounceTime = TimeSpan.FromMilliseconds(DefaultDebounceTimeMilliseconds);

            private long _debounceStartTicks;
            private ButtonHoldingState? _holdingState;
            private DateTimeOffset? _lastKeyDown = DateTimeOffset.UtcNow;
            private DateTimeOffset? _buttonHeldStart = DateTimeOffset.UtcNow;
            private TimeSpan? _buttonHeldTime;
            private DateTimeOffset? _lastPress;
            private IDisposable? _holdingSchedule;
            private IDisposable? _pressDelaySchedule;
            private ButtonTiming _options;
            private bool ShouldDebounce => DateTime.UtcNow.Ticks - _debounceStartTicks < _debounceTime.Ticks;

            public override void Run(IObservable<bool> source)
            {
                base.Run(source.DistinctUntilChanged());
            }

            public override void OnNext(bool change)
            {
                try
                {
                    if (change == true)
                    {
                        HandleButtonPressed(change);
                    }
                    else
                    {
                        HandleButtonReleased(change);
                    }
                }
                catch (Exception ex)
                {
                    this.ForwardOnError(ex);
                }
            }



            /// <summary>
            /// Gets or sets a value indicating whether single press event is enabled or disabled on the button.
            /// </summary>
            public bool IsPressed { get; set; } = false;

            /// <summary>
            /// Handler for pressing the button.
            /// </summary>
            /// <param name="change"></param>
            protected void HandleButtonPressed(bool change)
            {
                if (IsPressed || ShouldDebounce)
                    return;

                IsPressed = true;
                UpdateDebounce();
                _lastKeyDown = DateTimeOffset.UtcNow;
                if (_holdingState != ButtonHoldingState.Started)
                {
                    if (_holdingState != ButtonHoldingState.Pending)
                    {
                        _holdingState = ButtonHoldingState.Pending;
                        _holdingSchedule ??= scheduler.Schedule(change, _holdDelay,
                            (_, state) =>
                            {
                                _buttonHeldStart = DateTimeOffset.UtcNow;
                                _holdingState = ButtonHoldingState.Started;
                                _holdingSchedule?.Dispose();
                                _holdingSchedule = null;
                                OnNext(change, ButtonEventType.HeldDown);
                            });
                    }
                }

                OnNext(change, ButtonEventType.Down);
            }

            public void StartHolding(bool change)
            {
                _buttonHeldStart = DateTimeOffset.UtcNow;
                _holdingState = ButtonHoldingState.Started;
                _holdingSchedule?.Dispose();
                OnNext(change, ButtonEventType.HeldDown);
                _holdingTicks?.Dispose();
                if (_options.FireTickEvents)
                {
                    _holdingTicks = scheduler.SchedulePeriodic(change, _options.HoldTickRate, s =>
                    {
                        if (_holdingState == ButtonHoldingState.Started)
                            OnNext(s, ButtonEventType.HeldDownTick);
                    });
                }

                _longPressTimer?.Dispose();
                _longPressTimer = scheduler.Schedule(change, _options.LongPressTime, (_, state) =>
                {
//                if (_holdingState == ButtonHoldingState.Started)
                    //                   OnNext(change, ButtonEventType.LongPress);
                });
            }

            private void OnNext(bool change, ButtonEventType type)
            {
                ForwardOnNext(new ToggleEvent<ButtonEvent>(change,
                    new ButtonEvent(type, _holdingState)
                    {
                        LastKeyDown = _lastKeyDown,
                        HoldingState = _holdingState,
                        ButtonHeldTime =
                            _holdingState switch
                            {
                                ButtonHoldingState.Started => _buttonHeldStart.HasValue
                                    ? DateTimeOffset.UtcNow.Subtract(_buttonHeldStart.Value)
                                    : null,
                                ButtonHoldingState.Completed => _buttonHeldTime,
                                _ => null
                            }
                    }));
            }

            /// <summary>
            /// Handler for releasing the button.
            /// </summary>
            /// <param name="change"></param>
            protected void HandleButtonReleased(bool change)
            {
                ClearHoldingTimer();

                if (!IsPressed) return;
                IsPressed = false;
                UpdateDebounce();
                _lastPress = DateTimeOffset.UtcNow;
                OnNext(change, ButtonEventType.Up);
                _pressDelaySchedule = scheduler.Schedule(change, _pressDelay,
                    (_, state) => { OnNext(change, ButtonEventType.Pressed); });
                if (_holdingState == ButtonHoldingState.Started)
                {
                    _holdingState = ButtonHoldingState.Completed;
                    _lastButtonHeldTime = _buttonHeldStart.HasValue
                        ? DateTimeOffset.UtcNow.Subtract(_buttonHeldStart.Value)
                        : null;
                    OnNext(change, ButtonEventType.HoldReleased);
                }
            }

            ButtonEventType GetNextPress(ButtonEventType pressType) => (ButtonEventType)pressType + 1;
            private ButtonEventType? _nextPressType;
            private object _lastButtonHeldTime;
            private IDisposable _howManyClicksTimer;
            private IDisposable _longPressTimer;
            private IDisposable _holdingTicks;

            private void StartPressTimer(bool change)
            {
                _nextPressType = _nextPressType == null ? ButtonEventType.Pressed : GetNextPress(_nextPressType.Value);
                if (_nextPressType <= ButtonEventType.Pressed3x)
                {
                    _howManyClicksTimer?.Dispose();
                    _howManyClicksTimer = scheduler.Schedule(_nextPressType.Value, _options.PressDelay, (type, state) =>
                    {
                        //                 OnNext(change, type);
                        _nextPressType = null;
                        _howManyClicksTimer?.Dispose();
                        _howManyClicksTimer = null;
                    });
                }
            }

            private void UpdateDebounce() => _debounceStartTicks = DateTime.UtcNow.Ticks;

            private void ClearHoldingTimer()
            {
                _holdingSchedule?.Dispose();
                _buttonHeldStart = null;
                if (_holdingState == ButtonHoldingState.Pending)
                {
                    _holdingState = ButtonHoldingState.Canceled;
                }

            }

            public override void OnCompleted()
            {
                ClearHoldingTimer();
            }

            protected override void Dispose(bool disposing)
            {
                ClearHoldingTimer();
                base.Dispose(disposing);
            }

        }
    }
}


public enum TriToggleState
    {
        None,
        Up,
        Down
    }

    public record ButtonEvent(
        ButtonEventType EventType,
        ButtonHoldingState? HoldingState = null,
        DateTimeOffset? HoldStart = null,
        TimeSpan? ButtonHeldTime = null,
        DateTimeOffset? LastKeyDown = null,
        ButtonEventType? PendingPressState = null)
    {
        public bool IsPressAndAHalf => PendingPressState != null && EventType == ButtonEventType.Down;
    }


public static class ObservableExtensions
{
    //public static ButtonEvent ToEvent(this ButtonEventType type, DateTimeOffset? holdStart = null) =>
    //    new(type,HoldStart:holdStart);
    //public static IObservable<ToggleEvent<ButtonEvent>> ToButtonObserver(this Entity upToggle,IScheduler scheduler) =>
    //    new ButtonEventProducer(upToggle.ToBooleanObservable(), scheduler);

    //public static IObservable<TriToggleState> ToTriToggle(this Entity upToggle, IObservable<bool> downToggle) =>
    //        upToggle.ToBooleanObservable().ToTriToggle(downToggle);


    /// <summary>
    /// Returns an observable that emits true once the entity is on for a minimum timeSpan.
    /// This method takes into account EntityState.LastChanged, meaning the returned observable can emit true even if the time did not pass in the application.
    /// Resulting observable is distinct.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="timeSpan">The minimum time the entity needs to be on before true is emitted in the resulting observable.</param>
    /// <param name="scheduler"></param>
    public static IObservable<bool> WhenTrueFor(this Entity entity, TimeSpan timeSpan,
            IScheduler scheduler)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(scheduler);

            return Observable.Defer(() =>
                Observable.Return(entity.IsOn() &&
                                  entity.EntityState != null &&
                                  entity.EntityState.LastChanged.HasValue &&
                                  entity.EntityState.LastChanged.Value.ToUniversalTime() <= DateTime.UtcNow - timeSpan)
                    .Concat(
                        entity.ToBooleanObservable().WhenTrueFor(timeSpan, scheduler).Skip(1)));
        }

        /// <summary>
        /// Returns an observable that emits true once the predicate applied on the entity state returns true for a minimum timeSpan.
        /// This method takes into account EntityState.LastChanged, meaning the returned observable can emit true even if the time did not pass in the application.
        /// Predicate will be evaluated on all state changes, including attribute changes.
        /// Resulting observable is distinct.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="timeSpan">The minimum time the entity needs to be on before true is emitted in the resulting observable.</param>
        /// <param name="predicate"></param>
        /// <param name="scheduler"></param>
        public static IObservable<bool> WhenTrueFor(this Entity entity,
            TimeSpan timeSpan, Func<EntityState, bool> predicate, IScheduler scheduler)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(scheduler);

            return Observable.Defer(() =>
                Observable.Return(entity.EntityState != null &&
                                  predicate(entity.EntityState) &&
                                  entity.EntityState.LastChanged.HasValue &&
                                  entity.EntityState.LastChanged.Value.ToUniversalTime() <= DateTime.UtcNow - timeSpan)
                    .Concat(
                        entity.ToBooleanObservable(predicate).WhenTrueFor(timeSpan, scheduler).Skip(1)));
        }

}
