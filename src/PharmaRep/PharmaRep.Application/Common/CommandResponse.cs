namespace PharmaRep.Application.Common
{
    public class CommandResponse<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        private CommandResponse(T? data, bool isSuccess, string? message) : this(isSuccess, message) => Data = data;
        private CommandResponse(bool isSuccess, string? message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static CommandResponse<T> Success(T data)
        {
            return new CommandResponse<T>(data, true, null);
        }
        public static CommandResponse<T> Error(string message)
        {
            return new CommandResponse<T>(false, message);
        }
    }
    public record EntityCreated
    {
        public int Id { get; set; }
    }
    public record DeactivatedEntity(int id, int byUserId)
    {
        public int Id { get; set; } = id;
        public int ByUserId { get; set; } = byUserId;
        public const string Operation = "Entity has been deactivated (soft delete)";
    }
    public record EntityUpdated
    {
        public int Id { get; set; }
    }
}
