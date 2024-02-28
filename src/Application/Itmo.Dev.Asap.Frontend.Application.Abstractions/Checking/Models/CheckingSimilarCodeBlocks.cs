namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;

public record CheckingSimilarCodeBlocks(CheckingCodeBlock[] First, CheckingCodeBlock[] Second, double SimilarityScore);