using System.Threading;
using System.Threading.Tasks;
using Budgetly.Application.BudgetHistory.Commands.ArchivePreviousMonthBudgets;
using MediatR;
using Microsoft.Azure.WebJobs;

namespace Budgetly.Serverless.Functions;

public class ArchivePreviousMonthBudgets
{
    private readonly IMediator _mediator;
    
    public ArchivePreviousMonthBudgets(IMediator mediator)
    {
        _mediator = mediator;
    }

    [FunctionName("ArchivePreviousMonthBudgets")]
    public async Task Run([TimerTrigger("0 0 1 * *")] TimerInfo myTimer, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ArchivePreviousMonthBudgetsCommand(), cancellationToken);
    }
}