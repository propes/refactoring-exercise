using System;
using RefactoringExercise.Domain;

namespace RefactoringExercise.Models.PatientMessaging
{
    public abstract class BusItemBase : IBusItem
    {
        public Guid BusItemId { get; set; } = Guid.NewGuid();

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}