namespace Sep.Gateway.Services;

public record BaseResponse<T>(string Message, bool IsSuccess, T Data);