namespace RocketsApi.Tests
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PriorityAttribute : Attribute
    {
        public int Priority { get; }
        public PriorityAttribute(int priority) => Priority = priority;
    }

}