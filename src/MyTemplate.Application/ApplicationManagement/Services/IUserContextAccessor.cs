namespace MyTemplate.Application.ApplicationManagement.Services
{
    public interface IUserContextAccessor
    {
        public int UserId { get;}
        public string EncodedUserId { get; }
        public bool IsAuthenticated { get; }
    }
}