using System.Collections.Generic;
using System.Reflection;

namespace RefactoringExercise.Domain
{
    public class DynamicReportingEvent : BusItemBase, IEvent, IBusItem
    {
        public string Source { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public Dictionary<string, object> Payload { get; set; } = new Dictionary<string, object>();

        public Dictionary<string, object> NonPIDPayload { get; set; } = new Dictionary<string, object>();

        public DynamicReportingEvent() => this.Source = Assembly.GetCallingAssembly().GetName().Name;
    }
}
