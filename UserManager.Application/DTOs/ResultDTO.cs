namespace UserManager.Application.DTOs
{
    public class ResultDTO
    {
        public bool IsSuccess { get; init; }
        public IEnumerable<string> ErrorMessages { get; init; }

        private ResultDTO(bool isSuccess, IEnumerable<string>? errors = null)
        {
            IsSuccess = isSuccess;
            ErrorMessages = errors ?? [];
        }

        public static ResultDTO Success() => new ResultDTO(true);
        public static ResultDTO Failure(IEnumerable<string> messages) => new ResultDTO(false, messages);
    }
}
