using System;

namespace RefactoringExercise.Domain
{
    public abstract class BusItemBase : IBusItem
    {
        public Guid BusItemId { get; set; } = Guid.NewGuid();

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}