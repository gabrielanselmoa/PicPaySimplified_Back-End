namespace PicPaySimplified.Domain.Entities;

public class ResultModel<T>
{
    public T? Data { get; set; }
    public string Message { get; set; } = null!;
    public bool Success { get; set; }
}