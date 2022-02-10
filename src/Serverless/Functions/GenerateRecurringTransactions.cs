using System;
using System.Threading;
using System.Threading.Tasks;
using Budgetly.Application.Transactions.Commands.GenerateRecurringTransactions;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Budgetly.Serverless.Functions;

public class GenerateRecurringTransactions
{
    private readonly IMediator _mediator;

    public GenerateRecurringTransactions(IMediator mediator)
    {
        _mediator = mediator;
    }

    [FunctionName("GenerateRecurringTransactions")]
    public async Task RunAsync([TimerTrigger("30 0 1 * *")] TimerInfo myTimer,
        CancellationToken cancellationToken)
    { 
        await _mediator.Send(new GenerateRecurringTransactionsCommand(), cancellationToken);
    }
}