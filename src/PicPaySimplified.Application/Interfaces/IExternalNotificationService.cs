namespace PicPaySimplified.Application.Interfaces;

public interface IExternalNotificationService
{
    public Task<bool> Notify();
}