using System;
using System.Threading.Tasks;

namespace RefactoringExercise.Domain
{
    public interface IBus : IDisposable
    {
        bool IsConnected { get; }

        void RegisterAsyncHandler<T>(Func<T, Task> handler) where T : class, IBusItem;

        void RegisterAsyncHandler<T>(Func<T, Task> handler, string subscriptionId) where T : class, IBusItem;

        void RegisterHandler<T>(Action<T> handler) where T : class, IBusItem;

        void RegisterHandler<T>(Action<T> handler, string subscriptionId) where T : class, IBusItem;

        void Publish<T>(T @event) where T : class, IEvent;

        void Send<T>(T command) where T : class, ICommand;
    }

    public interface IBusItem
    {
        Guid BusItemId { get; }

        DateTime Timestamp { get; }
    }

    public interface IEvent : IBusItem
    {
    }

    public interface ICommand : IBusItem
    {
    }
}
