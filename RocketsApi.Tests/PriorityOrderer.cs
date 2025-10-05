using Xunit.Abstractions;
using Xunit.Sdk;

namespace RocketsApi.Tests
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            var sortedCases = testCases
                .Select(tc => new
                {
                    TestCase = tc,
                    Priority = tc.TestMethod.Method
                        .GetCustomAttributes(typeof(PriorityAttribute))
                        .FirstOrDefault()?.GetNamedArgument<int>("Priority") ?? 0
                })
                .OrderBy(tc => tc.Priority)
                .Select(tc => tc.TestCase);

            return sortedCases.ToList();
        }
    }
}