using Microsoft.Azure.WebJobs;

namespace Budgetly.Serverless.Functions;

public class ArchivePreviousMonthBudgets
{
    [FunctionName("ArchivePreviousMonthBudgets")]
    public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
    {
    }
}