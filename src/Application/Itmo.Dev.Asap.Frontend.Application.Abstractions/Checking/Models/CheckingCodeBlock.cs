namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;

public record struct CheckingCodeBlock(string FilePath, int LineFrom, int LineTo, string Content);