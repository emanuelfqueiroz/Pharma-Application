
namespace PharmaRep.Application.Common
{
    public record CommandResponse<T>
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

        public static CommandResponse<T> NotFound()
        {
            return CommandResponse<T>.Error("Object not found");
        }
    }
    public record EntityCreated
    {
        public int Id { get; set; }
    }
    public record DeactivatedEntity(int Id, int ByUserId)
    {
        public int Id { get; set; } = Id;
        public int ByUserId { get; set; } = ByUserId;
        public const string Operation = "Entity has been deactivated (soft delete)";
    }
    public record EntityUpdated
    {
        public int Id { get; set; }
    }
}
