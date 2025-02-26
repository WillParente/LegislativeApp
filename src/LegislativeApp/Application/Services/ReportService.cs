using LegislativeApp.Application.Extensions;
using LegislativeApp.Domain.Entities;
using LegislativeApp.Domain.Enums;
using LegislativeApp.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace LegislativeApp.Application.Services;

public class ReportService : IReportService
{
    private readonly ILogger<ReportService> _logger;
    private readonly ILegislativeRepository _legislativeRepository;

    public ReportService(ILogger<ReportService> logger, ILegislativeRepository legislativeRepository)
    {
        _logger = logger;
        _legislativeRepository = legislativeRepository;
    }

    
    public async Task GenerateBillsReportAsync(string csvPath, string reportName, CancellationToken cancellationToken)
    {
        /*For every bill in the dataset, how many legislators supported the bill? 
         *How many legislators opposed the bill? 
         *Who was the primary sponsor of the bill?*/
        
        _logger.LogInformation($"Generating {reportName} report");

        Task<IEnumerable<Bill>> billsTask = _legislativeRepository.LoadBillsAsync(csvPath.ToCsvFilePath<Bill>(), cancellationToken);
        Task<IEnumerable<Vote>> votesTask = _legislativeRepository.LoadVotesAsync(csvPath.ToCsvFilePath<Vote>(), cancellationToken);
        Task<IEnumerable<VoteResult>> voteResultsTask = _legislativeRepository.LoadVoteResultsAsync(csvPath.ToCsvFilePath<VoteResult>(), cancellationToken);

        await Task.WhenAll(billsTask, votesTask, voteResultsTask);

        var bills = billsTask.Result;
        var votes = votesTask.Result;
        var voteResults = voteResultsTask.Result;

        var reportData = bills
                            .GroupJoin(votes,
                                bill => bill.Id,
                                vote => vote.BillId,
                                (bill, billVotes) => new { bill, billVotes })
                            .SelectMany(
                                x => x.billVotes.DefaultIfEmpty(),
                                (x, vote) => new { x.bill, vote })
                            .GroupJoin(voteResults,
                                bv => bv.vote?.Id,
                                voteResult => voteResult.VoteId,
                                (bv, voteResultsGroup) => new { bv.bill, voteResultsGroup })
                            .SelectMany(
                                x => x.voteResultsGroup.DefaultIfEmpty(),
                                (x, voteResult) => new { x.bill, voteResult })
                            .GroupBy(x => new { x.bill.Id, x.bill.Title, x.bill.SponsorId })
                            .Select(g => new
                            {
                                id = g.Key.Id,
                                title = g.Key.Title,
                                supporter_count = g.Count(vr => vr.voteResult != null && vr.voteResult.VoteType == VoteType.Yea),
                                opposer_count = g.Count(vr => vr.voteResult != null && vr.voteResult.VoteType == VoteType.Nay),
                                primary_sponsor = g.Key.SponsorId,
                            })
                            .OrderBy(r => r.id)
                            .ToList();
        
        var reportPath = Path.Combine(csvPath, "Reports", reportName);
        await _legislativeRepository.ExportReportAsync(reportPath, reportData, cancellationToken);
    }

    public async Task GenerateLegislatorVotesReportAsync(string csvPath, string reportName, CancellationToken cancellationToken)
    {
        /*For every legislatorin the dataset, how many bills did the legislator support (voted for the bill)?
         *How many bills did the legislator oppose?*/
        
        _logger.LogInformation($"Generating {reportName} report");

        Task<IEnumerable<Legislator>> legislatorsTask = _legislativeRepository.LoadLegislatorsAsync(csvPath.ToCsvFilePath<Legislator>(), cancellationToken);
        Task<IEnumerable<Vote>> votesTask = _legislativeRepository.LoadVotesAsync(csvPath.ToCsvFilePath<Vote>(), cancellationToken);
        Task<IEnumerable<VoteResult>> voteResultsTask = _legislativeRepository.LoadVoteResultsAsync(csvPath.ToCsvFilePath<VoteResult>(), cancellationToken);

        await Task.WhenAll(legislatorsTask, votesTask, voteResultsTask);

        var legislators = legislatorsTask.Result;
        var votes = votesTask.Result;
        var voteResults = voteResultsTask.Result;

        var reportData = legislators
                            .GroupJoin(voteResults,
                                legislator => legislator.Id,
                                voteResult => voteResult.LegislatorId,
                                (legislator, voteResultsGroup) => new { legislator, voteResultsGroup })
                            .SelectMany(
                                x => x.voteResultsGroup.DefaultIfEmpty(),
                                (x, voteResult) => new { x.legislator, voteResult })
                            .GroupJoin(votes,
                                vr => vr.voteResult?.VoteId,
                                vote => vote.Id,
                                (vr, votesGroup) => new { vr.legislator, vr.voteResult, votesGroup })
                            .SelectMany(
                                x => x.votesGroup.DefaultIfEmpty(),
                                (x, vote) => new { x.legislator, x.voteResult, vote })
                            .GroupBy(x => new { x.legislator.Id, x.legislator.Name })
                            .Select(g => new
                            {
                                id = g.Key.Id,
                                name = g.Key.Name,
                                num_supported_bills = g.Count(vr => vr.voteResult != null && vr.voteResult.VoteType == VoteType.Yea),
                                num_opposed_bills = g.Count(vr => vr.voteResult != null && vr.voteResult.VoteType == VoteType.Nay)
                            })
                            .OrderBy(r => r.id)
                            .ToList();

        var reportPath = Path.Combine(csvPath, "Reports", reportName);
        await _legislativeRepository.ExportReportAsync(reportPath, reportData, cancellationToken);
    }
}
