namespace PicPaySimplified.Application.Interfaces;

public interface IExternalAuthorizationService
{
    public Task<bool> Authorize();
}