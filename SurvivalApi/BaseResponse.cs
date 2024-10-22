namespace api;

//public class BaseResponse<T>
//{
//	public bool Success { get; set; }
//	public string Message { get; set; }
//	public T Body { get; set; }
//}

abstract public class BaseResponseBase
{
	public bool Success { get; set; } = true;
	public string Message { get; set; } = "";
}

public class BaseResponseEmpty: BaseResponseBase
{
	public object Body { get; set; } = new { };

	public BaseResponseEmpty(bool success, string message)
	{
		base.Success = success;
		base.Message = message;
	}
}

public class BaseResponse<T> : BaseResponseBase
{
	public T Body { get; set; }

	public BaseResponse(bool success, string message, T body)
	{
		base.Success = success;
		this.Body = body;
		base.Message = message;
	}
}