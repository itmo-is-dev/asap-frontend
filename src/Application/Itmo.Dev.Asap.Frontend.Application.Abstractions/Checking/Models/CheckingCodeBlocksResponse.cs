namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;

public record CheckingCodeBlocksResponse(IEnumerable<CheckingSimilarCodeBlocks> CodeBlocks, int Count, bool HasNext);